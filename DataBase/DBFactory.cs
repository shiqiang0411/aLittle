using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBase
{
    public class DBFactory
    {
        private DataAccess.DataBaseType m_CurrentDataBaseType = DataAccess.DataBaseType.Oracle;
        private DataBaseConfig.XMLFileConfig m_WMSConfigFile = null;
        private DataAccess.ConnectName m_ConnectName = DataAccess.ConnectName.MyData;
        /// <summary>
        /// 数据库配置文件
        /// </summary>
        private string m_ConfigFile = @"C:\DBConfig.ini";
        public DBFactory(DataAccess.ConnectName pConnectName)
        {
            m_ConfigFile = System.AppDomain.CurrentDomain.BaseDirectory + "DBConfig.ini";
            m_WMSConfigFile = new DataBaseConfig.XMLFileConfig(m_ConfigFile);
            m_ConnectName = pConnectName;
        }
        public DataAccess.DataBaseType CurrentDataBaseType
        {
            get
            {
                return m_CurrentDataBaseType;
            }
        }
        public string GetConnectInfo()
        {
            if (!System.IO.File.Exists(m_ConfigFile))
            {
                throw new Exception("系统没有找到访问数据库的配置文件");
            }
            string strConn = string.Empty;
            string strDataBase = m_WMSConfigFile.GetValue(m_ConnectName.ToString(), "DataBase").ToString();
            string strIPAddress = string.Empty;
            try
            {
                strIPAddress = m_WMSConfigFile.GetValue(m_ConnectName.ToString(), "IPAddress").ToString();
            }
            catch { }
            string strUserId = m_WMSConfigFile.GetValue(m_ConnectName.ToString(), "UserId").ToString();
            string strPassWord = m_WMSConfigFile.GetValue(m_ConnectName.ToString(), "Password").ToString();
            string strDataBaseType = m_WMSConfigFile.GetValue(m_ConnectName.ToString(), "DataBaseType").ToString().Trim();
            switch (strDataBaseType.ToLower())
            {
                case "oracle":
                    m_CurrentDataBaseType = DataAccess.DataBaseType.Oracle;
                    strConn = String.Format("Data Source={0}/{1};User ID={2};PassWord={3}", strIPAddress, strDataBase, strUserId, strPassWord);
                    break;
                case "sqlserver":
                    m_CurrentDataBaseType = DataAccess.DataBaseType.SqlServer;
                    strConn = String.Format("server={0};uid={1};pwd={2};database={3}", strIPAddress, strUserId, strPassWord, strDataBase);
                    break;
                case "mysql":
                    m_CurrentDataBaseType = DataAccess.DataBaseType.Mysql;
                    strConn = string.Format("Database={0};Data Source={1};User Id={2};Password={3};Charset=utf8", strDataBase, strIPAddress, strUserId, strPassWord);
                    break;
                default:
                    throw new Exception("未知的数据库类型,请检查配置文件是否正确配置");

            }
            return strConn;
        }
    }
}
