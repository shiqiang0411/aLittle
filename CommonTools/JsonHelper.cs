using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace CommonTools
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 对象转化为Json字符串
        /// </summary>
        /// <param name="obj">对象参数</param>
        /// <returns></returns>
        public static string ObjToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// Json字符串转为对象
        /// </summary>
        /// <param name="Json">Json字符串</param>
        /// <returns></returns>
        public static T JsonToObj<T>(string Json)
        {
            T t = JsonConvert.DeserializeObject<T>(Json);
            return t;
        }
    }
}
