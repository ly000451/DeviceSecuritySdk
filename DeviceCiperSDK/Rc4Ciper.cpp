#include "stdafx.h"
#include "Rc4Ciper.h"

Rc4Ciper::Rc4Ciper()
{
	memset(sbox, 0, 256);
	memset(key, 0, 256);
}
Rc4Ciper::~Rc4Ciper()
{
	memset(sbox, 0, 256);  /* remove Key traces in memory  */
	memset(key, 0, 256);
}

char*  Rc4Ciper::Encrypt(char *pszText, int itxtLength) {
	return Encrypt(pszText, itxtLength,pszKey);
}

char* Rc4Ciper::Decrypt(char *pszText, int itxtLength) {
	return Decrypt(pszText, itxtLength,pszKey);

}