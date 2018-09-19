using DeviceCiperWrapper.kavo.com;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DeviceCiperSdkDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string deviceId = DeviceCiperWarpper.BuildKavoDeviceId("CBCT|pilot|12345");
            textBox1.Text = deviceId;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                string ciperText = DeviceCiperWarpper.Encrypt(textBox1.Text);
                textBox2.Text = ciperText;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null) {

                string ciperTxt = textBox2.Text;
                string deciperTxt="", kavoDeviceFlag="",deviceId="";
                DateTime dt=new DateTime();
                try
                {
                    bool result = DeviceCiperWarpper.DecryptionDeviceId(ciperTxt, ref deciperTxt,ref kavoDeviceFlag, ref deviceId, ref dt);
                    textBox3.Text = deciperTxt;

                }catch(Exception ev)
                {
                    label3.Text = ev.Message;
                }



            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != null)
            {

                string ciperTxt = textBox2.Text;
                string deciperTxt = "", kavoDeviceFlag = "", deviceId = "";
                DateTime dt = new DateTime();
                try
                {
                    bool result = DeviceCiperWarpper.DecryptionDeviceId(ciperTxt, ref deciperTxt, ref kavoDeviceFlag, ref deviceId, ref dt);
                    //textBox3.Text = deciperTxt;
                    DateTime currentTime = DateTime.Now;
                    TimeSpan interval = currentTime - dt;
                    if (interval.TotalMinutes < 0.5)
                    {
                        label3.Text = "验证结果：token有效";

                    }
                    else {
                        label3.Text = "验证结果：token无效 超过5分钟";

                    }



                }
                catch (Exception ev)
                {
                    label3.Text = ev.Message;
                }



            }

        }
    }
}
