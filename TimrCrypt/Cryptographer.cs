using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TimrCrypt
{
    class Cryptographer
    {
        public static string Encrypt(string plainText)
        {
            string _passPhrase = ConfigurationManager.AppSettings["passPhrase"].ToString();
            string _saltValue = ConfigurationManager.AppSettings["saltValue"].ToString();
            string _initVector = ConfigurationManager.AppSettings["initVector"].ToString();
            return Crypt.Encrypt(plainText, _passPhrase, _saltValue, "MD5", 1, _initVector, 128);
        }

        public static string Decrypt(string cipherText)
        {
            string _passPhrase = ConfigurationManager.AppSettings["passPhrase"].ToString();
            string _saltValue = ConfigurationManager.AppSettings["saltValue"].ToString();
            string _initVector = ConfigurationManager.AppSettings["initVector"].ToString();
            return Crypt.Decrypt(cipherText, _passPhrase, _saltValue, "MD5", 1, _initVector, 128);
        }
    }
}
