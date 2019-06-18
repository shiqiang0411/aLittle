using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBase;

namespace DBManage
{
    /// <summary>
    /// 数据库访问类
    /// </summary>
    public class DB
    {
        /// <summary>
        /// 配置数据库
        /// </summary>
        public static readonly DataAccess MyDataAccess = new DataAccess(DataAccess.ConnectName.MyData);
    }
}
