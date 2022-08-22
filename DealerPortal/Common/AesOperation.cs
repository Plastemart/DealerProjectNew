using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DealerPortal.Common
{
    public class AesOperation
    {
        public  string Encrypt(string data, string key)
        {
            string strEncodedData = "";
            if (data != null && data.Trim().Length > 0)
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC; //remember this parameter
                rijndaelCipher.Padding = PaddingMode.PKCS7; //remember this parameter

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;

                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }

                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);

                strEncodedData = Convert.ToBase64String
                (transform.TransformFinalBlock(plainText, 0, plainText.Length));
            }
            return strEncodedData;
        }

        public  string Decrypt(string data, string key)
        {
            string strEncodedData = "";
            if (data != null && data.Trim().Length > 0)
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                rijndaelCipher.Mode = CipherMode.CBC;
                rijndaelCipher.Padding = PaddingMode.PKCS7;

                rijndaelCipher.KeySize = 0x80;
                rijndaelCipher.BlockSize = 0x80;
                byte[] encryptedData = Convert.FromBase64String(data);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[0x10];
                int len = pwdBytes.Length;

                if (len > keyBytes.Length)
                {
                    len = keyBytes.Length;
                }

                Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                rijndaelCipher.IV = keyBytes;
                byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock
                            (encryptedData, 0, encryptedData.Length);

                strEncodedData = Encoding.UTF8.GetString(plainText);
            }
            return strEncodedData;
        }



    }
}