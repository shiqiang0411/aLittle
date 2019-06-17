using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommonTools
{
    /// <summary>
    /// 加密帮助类
    /// </summary>
    public static class EncryptHelper
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="Value">要加密的字符串</param>
        /// <returns></returns>
        public static string MD5Encrypt(this string Value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bt = System.Text.Encoding.Unicode.GetBytes(Value);
            byte[] md5bt = md5.ComputeHash(bt);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5bt.Length; i++)
            {
                sb.Append(md5bt[i].ToString("x"));
            }
            return sb.ToString();
        }
    }
}
