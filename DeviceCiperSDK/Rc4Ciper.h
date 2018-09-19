#pragma once
#include <stdio.h>
#include <string.h>

extern "C" {

#ifdef DEVICE_CIPER_SHARED_LIBRARY
#define Device_Ciper_Export __declspec(dllexport)
#else //!defined(DEVICE_CIPER_SHARED_LIBRARY)
#define Device_Ciper_Export __declspec(dllimport)
#endif

#define SWAP(a, b) ((a) ^= (b), (b) ^= (a), (a) ^= (b))
	class Rc4Ciper
	{

	private:
		unsigned char sbox[256];      /* Encryption array             */
		unsigned char key[256], k;     /* Numeric key values           */
		int  m, n, i, j, ilen;        /* Ambiguously named counters   */
		const char* pszKey = "FC6BB4032580DF75";
	public:
		Rc4Ciper();
		~Rc4Ciper();
		Device_Ciper_Export char*  Encrypt(char *pszText, int itxtLength);

		Device_Ciper_Export char* Decrypt(char *pszText, int itxtLength);

	private:

		char *Encrypt(char *pszText, int iTextLength, const char *pszKey)
		{
			i = 0, j = 0, n = 0;
			ilen = (int)strlen(pszKey);

			for (m = 0; m < 256; m++)  /* Initialize the key sequence */
			{
				*(key + m) = *(pszKey + (m % ilen));
				*(sbox + m) = m;
			}

			for (m = 0; m < 256; m++)
			{
				n = (n + *(sbox + m) + *(key + m)) & 0xff;
				SWAP(*(sbox + m), *(sbox + n));
			}

			ilen = iTextLength;
			for (m = 0; m < ilen; m++)
			{
				i = (i + 1) & 0xff;
				j = (j + *(sbox + i)) & 0xff;
				SWAP(*(sbox + i), *(sbox + j));  /* randomly Initialize the key sequence */
				k = *(sbox + ((*(sbox + i) + *(sbox + j)) & 0xff));
				if (k == *(pszText + m))       /* avoid '\0' beween the decoded text; */
					k = 0;
				*(pszText + m) ^= k;
			}
			return pszText;

		}

		char *Decrypt(char *pszText, int iTextLength, const char *pszKey)
		{
			return Encrypt(pszText, iTextLength, pszKey);  /* using the same function as encoding */
		}

	};

}