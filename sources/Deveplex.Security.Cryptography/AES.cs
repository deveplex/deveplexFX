using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AppApi.External
{
    public class AES
    {
        #region
        /// <summary>
        /// 256位AES加密
        /// </summary>
        /// <param name="encrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string encrypt, string key)
        {
            // 256-AES key    
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key.PadRight(32, '0'));
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.KeySize = 256;
            rDel.BlockSize = 128;
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 256位AES解密
        /// </summary>
        /// <param name="decrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string decrypt, string key)
        {
            // 256-AES key    
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key.PadRight(32, '0'));
            byte[] toEncryptArray = Convert.FromBase64String(decrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.KeySize = 256;
            rDel.BlockSize = 128;
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        /// <summary>
        /// 256位AES加密
        /// </summary>
        public static string Encrypt(string encrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key.PadRight(32, '0'));
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(encrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.KeySize = 256;
            rDel.BlockSize = 128;
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// 256位AES解密
        /// </summary>
        public static string Decrypt(string decrypt, string key, string iv)
        {
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key.PadRight(32, '0'));
            byte[] ivArray = UTF8Encoding.UTF8.GetBytes(iv);
            byte[] toEncryptArray = Convert.FromBase64String(decrypt);

            RijndaelManaged rDel = new RijndaelManaged();
            rDel.KeySize = 256;
            rDel.BlockSize = 128;
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}