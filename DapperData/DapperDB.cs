using Dapper;
using DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperData
{
    public class DapperDB
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static readonly string connectionString = new DBFactory(DataAccess.ConnectName.MyData).GetConnectInfo();

        public static List<dt_users> dt_Users()
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.Query<dt_users>(@"SELECT * FROM [DTcmsdb5].[dbo].[dt_users]").ToList();
            }
        }
    }
}
