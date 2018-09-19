using System;

using System.Text;
using System.Security.Cryptography;
using System.Globalization;

namespace DeviceCiperWrapper.kavo.com
{

    public class DeviceCiperWarpper
    {
        //[DllImport("DeviceCiperSDK.dll", EntryPoint = "?Encrypt@Rc4Ciper@@QEAAPEADPEADH@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        ////[return: MarshalAs(UnmanagedType.LPArray)]
        ////[MarshalAs(UnmanagedType.LPArray)] byte[]
        //public static extern IntPtr Encrpt( string pPlainText,int txtLen);

        //[DllImport("DeviceCiperSDK.dll", EntryPoint = "?Decrypt@Rc4Ciper@@QEAAPEADPEADH@Z", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        ////[return: MarshalAs(UnmanagedType.LPArray)]
        //public static extern IntPtr Decrypt(string pPlainText, int txtLen);
        public static string Encrypt(string toEncrypt, bool useHashing=false)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            // Get the key from config file
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            string key= new CiperKey().getCiperKey();
            //System.Windows.Forms.MessageBox.Show(key);
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        

        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing=false)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey", typeof(String));
            string key = new CiperKey().getCiperKey();
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string BuildKavoDeviceId(string deviceInfo)
        {

            string kavoDevice = "KavoIotDevice";
            DateTime currentTime = DateTime.Now;
            DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
            dtfi.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss,fff";
            string strTimeStamp = Convert.ToString(currentTime, dtfi);
            return kavoDevice + "|" + deviceInfo + "|" + strTimeStamp;
        }

        public static string EncryptionDeviceId(string deviceId) {
            string strDeviceId = Encrypt(deviceId);
            return strDeviceId;

        }

        public static bool DecryptionDeviceId(string ciperDeviceId, ref string deciperDeviceId, ref string kavoEquipmentFlag, ref string deviceId, ref DateTime dt) {
            if (!ciperDeviceId.Equals("")){
                deciperDeviceId = Decrypt(ciperDeviceId);
                string[] arrDeviceInfo = deciperDeviceId.Split(new char[] { '|'});
                if (arrDeviceInfo.Length < 3)
                {
                    return false;
                }
                else {
                    kavoEquipmentFlag = arrDeviceInfo[0];
                    int i = 1,len=arrDeviceInfo.Length;
                    for (; i < arrDeviceInfo.Length - 2; i++) {
                        deviceId += arrDeviceInfo[i];
                    }

                    try{

                        dt=Convert.ToDateTime(arrDeviceInfo[len - 1]);

                    }catch (Exception e) {
                        throw new Exception("can't convert to data time format");
                        
                    }

                }

            }
            return true;
        }



    }

   


}

