using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CommonTools
{
    /// <summary>
    /// DataTable帮助类
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// DataTable排序
        /// </summary>
        /// <param name="dt">需要排序的表</param>
        /// <param name="sort">排序字符串</param>
        public static void SortTable(this DataTable dt, string sort)
        {
            DataView dv = dt.DefaultView;
            dv.Sort = sort;
            dt = dv.ToTable();
        }
        /// <summary>
        /// datatable转实体list集合
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static IList<T> ToList<T>(this DataTable dt)
        {
            IList<T> list = new List<T>();
            T t = default(T);
            System.Reflection.PropertyInfo[] Property = null;
            string tempName = string.Empty;
            foreach (System.Data.DataRow item in dt.Rows)
            {
                t = Activator.CreateInstance<T>();
                Property = t.GetType().GetProperties();
                foreach (System.Reflection.PropertyInfo pro in Property)
                {
                    tempName = pro.Name;
                    if (dt.Columns.Contains(tempName))
                    {
                        object obj = null;
                        switch (pro.GetGetMethod().ReturnParameter.ToString().Trim())
                        {
                            case "System.Object":
                                obj = item[tempName] == DBNull.Value ? null : item[tempName].ToString();
                                break;
                            case "Int32":
                                obj = item[tempName] == DBNull.Value ? null : item[tempName].ToString();
                                if (obj.ToString() != "")
                                    obj = int.Parse(obj.ToString());
                                break;
                            default:
                                obj = item[tempName] == DBNull.Value ? null : item[tempName];
                                break;
                        }
                        pro.SetValue(t, obj, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }
    }
}
