using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonTools
{
    /// <summary>
    /// 字符串帮助类
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// 获得一个百分比字符串
        /// </summary>
        /// <param name="d">十进制数</param>
        /// <returns>百分比字符串</returns>
        public static string GetPercent(Decimal d)
        {
            return string.Format("{0:P}", d);
        }
    }
}
