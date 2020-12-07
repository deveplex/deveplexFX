using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deveplex.Security.Cryptography
{
    public sealed class HMACMD5
    {
        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encrypt">需要加密的字符串</param>
        /// <param name="key">加密的密钥</param>
        /// <returns></returns>
        public static string Encrypt(string encrypt, string key)
        {
            return Encrypt(encrypt, key, Encoding.UTF8);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encrypt">需要加密的字符串</param>
        /// <param name="key">加密的密钥</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Encrypt(string encrypt, string key, Encoding encode)
        {
            byte[] hashBytes = MD5Encrypt(encrypt, key, encode);
            StringBuilder sb = new StringBuilder(32);
            foreach (var hash in hashBytes)
                sb.Append(hash.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// MD5对文件流加密
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key">加密的密钥</param>
        /// <returns></returns>
        public static string Encrypt(Stream stream, string key)
        {
            return Encrypt(stream, key, Encoding.UTF8);
        }
        /// <summary>
        /// MD5对文件流加密
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key">加密的密钥</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Encrypt(Stream stream, string key, Encoding encode)
        {
            byte[] hashBytes = MD5Encrypt(stream, key, encode);
            StringBuilder sb = new StringBuilder();
            foreach (byte hash in hashBytes)
                sb.Append(hash.ToString("X2"));
            return sb.ToString();
        }
        #endregion

        private static byte[] MD5Encrypt(string encrypt, string key, Encoding encode)
        {
            System.Security.Cryptography.HMACMD5 hmac = new System.Security.Cryptography.HMACMD5(encode.GetBytes(key));
            return hmac.ComputeHash(encode.GetBytes(encrypt.ToString()));
        }
        private static byte[] MD5Encrypt(Stream stream, string key, Encoding encode)
        {
            System.Security.Cryptography.HMACMD5 hmac = new System.Security.Cryptography.HMACMD5(encode.GetBytes(key));
            return hmac.ComputeHash(stream);
        }
    }
}
