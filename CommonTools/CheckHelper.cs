using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonTools
{
    /// <summary>
    /// string检验帮助扩展类
    /// </summary>
    public static class CheckHelper
    {
        /// <summary>
        /// 验证是否是邮箱格式
        /// </summary>
        /// <param name="s">邮箱字符串</param>
        /// <returns></returns>
        public static bool IsEmail(this string s)
        {
            return Regex.IsMatch(s, @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$");
        }
        /// <summary>
        /// 密码复杂度验证[字母+数字结合]
        /// </summary>
        /// <param name="s">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsComplex(this string s)
        {
            return Regex.IsMatch(s, @"^(?![0-9]+$)(?![a-zA-Z]+$)[0-9A-Za-z]{6,10}$");
        }
        /// <summary>
        /// 验证数字
        /// </summary>
        /// <param name="s">要验证的字符串</param>
        /// <returns></returns>
        public static bool IsMath(this string s)
        {
            return Regex.IsMatch(s, @"^[0-9]*$");
        }
    }
}
