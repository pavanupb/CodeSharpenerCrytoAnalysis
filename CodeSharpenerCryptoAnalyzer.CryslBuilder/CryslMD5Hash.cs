using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CodeSharpenerCryptoAnalyzer.CryslBuilder
{
    public class CryslMD5Hash : ICryslMD5Hash
    {
        public string GetHashCode(string cryslContents)
        {
            string cryslContentHashCode = string.Empty;
            using(MD5 md5Hash = MD5.Create())
            {
                byte[] hashedCryslData = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(cryslContents));
                cryslContentHashCode = Convert.ToBase64String(hashedCryslData);
            }
            return cryslContentHashCode;
        }
    }
}
