using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

namespace DataBase
{
    public class DataAccess
    {
        private DataBaseType m_CurrentDataBaseType = DataBaseType.Oracle;
        private string m_strConn = string.Empty;
        private ConnectName m_ConnectName = DataAccess.ConnectName.MyData;
        /// <summary>
        /// 数据访问类
        /// </summary>
        /// <param name="pConnectName">..</param>
        public DataAccess(ConnectName pConnectName)
        {
            m_ConnectName = pConnectName;
        }
        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DataBaseType
        {
            /// <summary>
            /// 当前数据库类型为Oracle
            /// </summary>
            Oracle,
            /// <summary>
            /// 当前数据库类型为SqlServer
            /// </summary>
            SqlServer,
            /// <summary>
            /// 当前数据库类型为Mysql
            /// </summary>
            Mysql
        }
        /// <summary>
        /// 数据库连接实例名称
        /// </summary>
        public enum ConnectName
        {
            /// <summary>
            /// 用于访问MyData数据库
            /// </summary>
            MyData
        }
        /// <summary>
        /// 获取当前数据库连接字符串
        /// </summary>
        /// <returns></returns>
        private string GetDBConnection()
        {
            DBFactory pDBFactory = new DBFactory(m_ConnectName);
            m_strConn = pDBFactory.GetConnectInfo();
            m_CurrentDataBaseType = pDBFactory.CurrentDataBaseType;
            return m_strConn;
        }
        /// <summary>
        /// 返回第一行第一列
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public string ExecuteScalar(string strSql)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    OracleCommand cmd = new OracleCommand(strSql);
                    cmd.CommandTimeout = 180;
                    using (OracleConnection conn = new OracleConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        cmd.Connection = conn;
                        string val = string.Empty;
                        object obj = cmd.ExecuteScalar();
                        if (obj != null)
                        {
                            val = obj.ToString();
                        }
                        conn.Close();
                        return val;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    SqlCommand cmd = new SqlCommand(strSql);
                    cmd.CommandTimeout = 180;
                    using (SqlConnection conn = new SqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        cmd.Connection = conn;
                        string val = string.Empty;
                        object obj = cmd.ExecuteScalar();
                        if (obj != null)
                        {
                            val = obj.ToString();
                        }
                        conn.Close();
                        return val;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    MySqlCommand mcmd = new MySqlCommand(strSql);
                    mcmd.CommandTimeout = 180;
                    using (MySqlConnection conn = new MySqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        mcmd.Connection = conn;
                        string val = string.Empty;
                        object obj = mcmd.ExecuteScalar();
                        if (obj != null)
                        {
                            val = obj.ToString();
                        }
                        conn.Close();
                        return val;
                    }
                }
                return string.Empty;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 执行增删改查
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                int iResult = -1;
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection conn = new OracleConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        OracleCommand cmd = new OracleCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        iResult = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        iResult = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    using (MySqlConnection conn = new MySqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        MySqlCommand cmd = new MySqlCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        iResult = cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                }
                return iResult > 0;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 同一事物中执行多条sql语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns>返回受影响的行数</returns>
        public int ExecuteNonQuery(string[] cmdText)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection myConnection = new OracleConnection(m_strConn))
                    {
                        int val = 0;
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        OracleCommand myCommand = myConnection.CreateCommand();
                        myCommand.CommandTimeout = 180;
                        OracleTransaction myTrans;
                        myTrans = ((OracleConnection)myConnection).BeginTransaction();
                        myCommand.Connection = (OracleConnection)myConnection;
                        myCommand.Transaction = myTrans;
                        try
                        {
                            foreach (string strSql in cmdText)
                            {
                                if (string.IsNullOrEmpty(strSql))
                                {
                                    continue;
                                }
                                myCommand.CommandText = strSql;
                                int i = myCommand.ExecuteNonQuery();
                                if (i > 0)
                                {
                                    val = val + i;
                                }
                            }
                            myTrans.Commit();
                        }
                        catch (Exception err)
                        {
                            myTrans.Rollback();
                            myConnection.Close();
                            throw err;
                        }
                        myConnection.Close();
                        return val;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    using (SqlConnection myConnection = new SqlConnection(m_strConn))
                    {
                        int val = 0;
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        SqlCommand myCommand = myConnection.CreateCommand();
                        myCommand.CommandTimeout = 180;
                        SqlTransaction myTrans;
                        myTrans = ((SqlConnection)myConnection).BeginTransaction();
                        myCommand.Connection = (SqlConnection)myConnection;
                        myCommand.Transaction = myTrans;
                        try
                        {
                            foreach (string strSql in cmdText)
                            {
                                if (string.IsNullOrEmpty(strSql))
                                {
                                    continue;
                                }
                                myCommand.CommandText = strSql;
                                int i = myCommand.ExecuteNonQuery();
                                if (i > 0)
                                {
                                    val = val + i;
                                }
                            }
                            myTrans.Commit();
                        }
                        catch (Exception err)
                        {
                            myTrans.Rollback();
                            myConnection.Close();
                            throw err;
                        }
                        myConnection.Close();
                        return val;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    using (MySqlConnection myConnection = new MySqlConnection(m_strConn))
                    {
                        int val = 0;
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        MySqlCommand myCommand = myConnection.CreateCommand();
                        myCommand.CommandTimeout = 180;
                        MySqlTransaction myTrans;
                        myTrans = ((MySqlConnection)myConnection).BeginTransaction();
                        myCommand.Connection = (MySqlConnection)myConnection;
                        myCommand.Transaction = myTrans;
                        try
                        {
                            foreach (string strSql in cmdText)
                            {
                                if (string.IsNullOrEmpty(strSql))
                                {
                                    continue;
                                }
                                myCommand.CommandText = strSql;
                                int i = myCommand.ExecuteNonQuery();
                                if (i > 0)
                                {
                                    val = val + i;
                                }
                            }
                            myTrans.Commit();
                        }
                        catch (Exception err)
                        {
                            myTrans.Rollback();
                            myConnection.Close();
                            throw err;
                        }
                        myConnection.Close();
                        return val;
                    }
                }
                return -1;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 执行查询--带参数查询
        /// </summary>
        /// <param name="strSql">带有参数的sql语句</param>
        /// <param name="paramList">key为上strSql语句中的参数名称，value为该参数值</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, System.Collections.Hashtable paramList)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                int iResult = -1;
                if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (paramList != null && paramList.Keys.Count > 0)
                        {
                            foreach (string pn in paramList.Keys)
                            {
                                cmd.Parameters.Add(new SqlParameter(pn, paramList[pn]));
                            }
                        }
                        iResult = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection conn = new OracleConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        OracleCommand cmd = new OracleCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (paramList != null && paramList.Keys.Count > 0)
                        {
                            foreach (string pn in paramList.Keys)
                            {
                                cmd.Parameters.Add(new OracleParameter(pn, paramList[pn]));
                            }
                        }
                        iResult = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    using (MySqlConnection conn = new MySqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        MySqlCommand cmd = new MySqlCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (paramList != null && paramList.Keys.Count > 0)
                        {
                            foreach (string pn in paramList.Keys)
                            {
                                cmd.Parameters.Add(new MySqlParameter(pn, paramList[pn]));
                            }
                        }
                        iResult = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                return iResult > 0;

            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 带参数查询语句，返回数据集合
        /// </summary>
        /// <param name="strSql"></param>
        /// <param name="paramList"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string strSql, System.Collections.Hashtable paramList)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                DataSet ds = new DataSet();
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection conn = new OracleConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        OracleCommand cmd = new OracleCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (paramList != null && paramList.Keys.Count > 0)
                        {
                            foreach (string pn in paramList.Keys)
                            {
                                cmd.Parameters.Add(new OracleParameter(pn, paramList[pn]));
                            }
                        }
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    using (SqlConnection conn = new SqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        SqlCommand cmd = new SqlCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (paramList != null && paramList.Keys.Count > 0)
                        {
                            foreach (string pn in paramList.Keys)
                            {
                                cmd.Parameters.Add(new SqlParameter(pn, paramList[pn]));
                            }
                        }
                        System.Data.SqlClient.SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    using (MySqlConnection conn = new MySqlConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        MySqlCommand cmd = new MySqlCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (paramList != null && paramList.Keys.Count > 0)
                        {
                            foreach (string pn in paramList.Keys)
                            {
                                cmd.Parameters.Add(new MySqlParameter(pn, paramList[pn]));
                            }
                        }
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(ds);
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                return ds;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 查询语句，返回数据集合
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string cmdText)
        {
            string[] strs = new string[1];
            strs[0] = cmdText;
            return GetDataSet(strs);
        }
        /// <summary>
        /// 查询多条sql语句
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string[] cmdText)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection myConnection = new OracleConnection(m_strConn))
                    {
                        DataSet ds = new DataSet();
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        foreach (string strSql in cmdText)
                        {
                            if (string.IsNullOrEmpty(strSql))
                            {
                                continue;
                            }
                            DataTable dt = new DataTable();
                            OracleDataAdapter sda = new OracleDataAdapter(strSql, myConnection);
                            sda.Fill(dt);
                            ds.Tables.Add(dt);
                        }
                        myConnection.Close();
                        return ds;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    using (SqlConnection myConnection = new SqlConnection(m_strConn))
                    {
                        DataSet ds = new DataSet();
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        foreach (string strSql in cmdText)
                        {
                            if (string.IsNullOrEmpty(strSql))
                            {
                                continue;
                            }
                            DataTable dt = new DataTable();
                            SqlDataAdapter sda = new SqlDataAdapter(strSql, myConnection);
                            sda.SelectCommand.CommandTimeout = 180;
                            sda.Fill(dt);
                            ds.Tables.Add(dt);
                        }
                        myConnection.Close();
                        return ds;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    using (MySqlConnection myConnection = new MySqlConnection(m_strConn))
                    {
                        DataSet ds = new DataSet();
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        foreach (string strSql in cmdText)
                        {
                            if (string.IsNullOrEmpty(strSql))
                            {
                                continue;
                            }
                            DataTable dt = new DataTable();
                            MySqlDataAdapter sda = new MySqlDataAdapter(strSql, myConnection);
                            sda.Fill(dt);
                            ds.Tables.Add(dt);
                        }
                        myConnection.Close();
                        return ds;
                    }
                }
                return null;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 执行程储过程
        /// </summary>
        /// <param name="strProcedureName">程储过程的名称</param>
        /// <param name="InputParms">输入参数</param>
        /// <returns></returns>
        public DataSet ExecuteProcedure(string strProcedureName, params string[] InputParms)
        {
            try
            {
                if (string.IsNullOrEmpty(strProcedureName) || InputParms == null)
                {
                    return null;
                }
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection myConnection = new OracleConnection(m_strConn))
                    {
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        OracleCommand cmd = new OracleCommand(strProcedureName, myConnection);
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        OracleCommandBuilder.DeriveParameters(cmd);
                        for (int i = 0; i < InputParms.Length; i++)
                        {
                            cmd.Parameters[i + 1].Value = InputParms[i];
                        }
                        DataSet ds = new DataSet();
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(ds);
                        myConnection.Close();
                        return ds;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.SqlServer)
                {
                    using (SqlConnection myConnection = new SqlConnection(m_strConn))
                    {
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        SqlCommand cmd = new SqlCommand(strProcedureName, myConnection);
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlCommandBuilder.DeriveParameters(cmd);
                        for (int i = 0; i < InputParms.Length; i++)
                        {
                            cmd.Parameters[i + 1].Value = InputParms[i];
                        }
                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        myConnection.Close();
                        return ds;
                    }
                }
                else if (m_CurrentDataBaseType == DataBaseType.Mysql)
                {
                    using (MySqlConnection myConnection = new MySqlConnection(m_strConn))
                    {
                        if (myConnection.State != System.Data.ConnectionState.Open)
                        {
                            myConnection.Open();
                        }
                        MySqlCommand cmd = new MySqlCommand(strProcedureName, myConnection);
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        MySqlCommandBuilder.DeriveParameters(cmd);
                        for (int i = 0; i < InputParms.Length; i++)
                        {
                            cmd.Parameters[i + 1].Value = InputParms[i];
                        }
                        DataSet ds = new DataSet();
                        MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                        da.Fill(ds);
                        myConnection.Close();
                        return ds;
                    }
                }
                return null;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        /// <summary>
        /// 执行查询--带参数查询
        /// </summary>
        /// <param name="strSql">带有参数的sql语句</param>
        /// <param name="paramList">key为上strSql语句中的参数名称，value为该参数值</param>
        /// <returns></returns>
        public bool ExecuteNonQuery(string strSql, params OracleParameter[] oracleParas)
        {
            try
            {
                if (string.IsNullOrEmpty(m_strConn))
                {
                    GetDBConnection();
                }
                int iResult = -1;
                if (m_CurrentDataBaseType == DataBaseType.Oracle)
                {
                    using (OracleConnection conn = new OracleConnection(m_strConn))
                    {
                        if (conn.State != System.Data.ConnectionState.Open)
                        {
                            conn.Open();
                        }
                        OracleCommand cmd = new OracleCommand(strSql, conn);
                        cmd.CommandTimeout = 180;
                        if (oracleParas.Length > 0)
                        {
                            cmd.Parameters.AddRange(oracleParas);
                        }
                        iResult = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        conn.Close();
                    }
                }
                Console.WriteLine("执行完成");
                return iResult > 0;

            }
            catch (Exception err)
            {
                throw err;
            }
        }
    }
}
