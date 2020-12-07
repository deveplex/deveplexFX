using System;
using System.Text;
using System.IO;

namespace Deveplex.Security.Cryptography
{
    public sealed class MD5
    {
        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encrypt">需要加密的字符串</param>
        /// <returns></returns>
        public static string Encrypt(string encrypt)
        {
            return Encrypt(encrypt, Encoding.UTF8);
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encrypt">需要加密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Encrypt(string encrypt, Encoding encode)
        {
            byte[] hashBytes = MD5Encrypt(encrypt, encode);
            StringBuilder sb = new StringBuilder(32);
            foreach (var hash in hashBytes)
                sb.Append(hash.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// MD5对文件流加密
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string Encrypt(Stream stream)
        {
            byte[] hashBytes = MD5Encrypt(stream);
            StringBuilder sb = new StringBuilder();
            foreach (byte hash in hashBytes)
                sb.Append(hash.ToString("X2"));
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密(返回16位加密串)
        /// </summary>
        /// <param name="encrypt">需要加密的字符串</param>
        /// <param name="encode">字符的编码</param>
        /// <returns></returns>
        public static string Encrypt16(string encrypt, Encoding encode)
        {
            byte[] hashBytes = MD5Encrypt(encrypt, encode);
            string result = BitConverter.ToString(hashBytes, 4, 8);
            result = result.Replace("-", "");
            return result;
        }
        #endregion

        private static byte[] MD5Encrypt(string encrypt, Encoding encode)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            return md5.ComputeHash(encode.GetBytes(encrypt)); ;

        }
        private static byte[] MD5Encrypt(Stream stream)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            return md5.ComputeHash(stream);
        }
    }
}
