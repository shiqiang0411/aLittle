using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DataBaseConfig
{
    /// <summary>
    /// 以文件XML形式读写配置的类
    /// </summary>
    public class XMLFileConfig
    {
        private readonly string m_ConfigFile;
        private readonly string m_ConfigKeyField;
        /// <summary>
        /// 默认构造函数,该配置文件默认为c:\DSFConfig\DSFConfig.cfg
        /// </summary>
        public XMLFileConfig()
        {
            m_ConfigFile = @"c:\DSFConfig\DSFConfig.cfg";
            m_ConfigKeyField = "MainNodeName";
        }
        /// <summary>
        /// 重载构造函数,提供一个设置配置文件的接口
        /// </summary>
        /// <param name="ConfigFile">该配置文件的全路径和文件名</param>
        public XMLFileConfig(string ConfigFile)
        {
            m_ConfigKeyField = "MainNodeName";
            m_ConfigFile = ConfigFile;
        }
        /// <summary>
        /// 设置一个节点下的字段值
        /// </summary>
        /// <param name="VarName">该节点的名称</param>
        /// <param name="VarField">节点下的字段名</param>
        /// <param name="VarValue">该字段对应的值</param>
        /// <returns>返回设置值是否成功</returns>
        public bool SetValue(string VarName, string VarField, string VarValue)
        {//当该文件m_ConfigFile正在被使用的话怎么办????
            //			System.Threading.Monitor.Enter(this);
            string PathName = Path.GetDirectoryName(m_ConfigFile);
            if (!System.IO.Directory.Exists(PathName))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(PathName);
                }
                catch (Exception)
                {
                    //					System.Threading.Monitor.Exit(this);
                    return false;
                }
            }
            string FileName = Path.GetFileNameWithoutExtension(m_ConfigFile);
            DataSet CfgData = new DataSet(FileName);
            if (File.Exists(m_ConfigFile))
            {
                try
                {//如果文件存在，读取文件
                    CfgData.ReadXml(m_ConfigFile);
                }
                catch (Exception)
                {//当前xml格式的文件被破坏了
                    File.Delete(m_ConfigFile);
                }
                if (CfgData.Tables.Count <= 0)
                {
                    //如果其中不存在表，创建
                    DataTable JDConfig = new DataTable(FileName);
                    DataColumn colyjdconfigname = new DataColumn(m_ConfigKeyField, System.Type.GetType("System.String"));
                    JDConfig.Columns.Add(colyjdconfigname);
                    CfgData.Tables.Add(JDConfig);
                }

                if (CfgData.Tables[0].Columns.IndexOf(VarField) < 0)//列不存在
                {
                    //如果列不存在,创建列
                    DataColumn colField = new DataColumn(VarField, System.Type.GetType("System.String"));
                    CfgData.Tables[0].Columns.Add(colField);
                }
                DataRow[] rows = CfgData.Tables[0].Select(m_ConfigKeyField + "=" + "'" + VarName + "'");
                DataRow row = null;
                if (rows.Length <= 0)
                {
                    //如果主关键字不存在，创建
                    row = CfgData.Tables[0].NewRow();
                    row[m_ConfigKeyField] = VarName;
                    row[VarField] = VarValue;
                    CfgData.Tables[0].Rows.Add(row);
                }
                else
                {
                    row = rows[0];
                    row[VarField] = VarValue;
                }
                CfgData.WriteXml(m_ConfigFile, XmlWriteMode.WriteSchema);
            }
            else
            {
                DataTable JDConfig = new DataTable(FileName);
                DataColumn colyjdconfigname = new DataColumn(m_ConfigKeyField, System.Type.GetType("System.String"));
                JDConfig.Columns.Add(colyjdconfigname);
                DataColumn colField = new DataColumn(VarField, System.Type.GetType("System.String"));
                JDConfig.Columns.Add(colField);
                DataRow row = JDConfig.NewRow();
                row[m_ConfigKeyField] = VarName;
                row[VarField] = VarValue;
                JDConfig.Rows.Add(row);
                CfgData.Tables.Add(JDConfig);
                CfgData.WriteXml(m_ConfigFile, XmlWriteMode.WriteSchema);
            }
            //			System.Threading.Monitor.Exit(this);
            return true;
        }
        /// <summary>
        /// 取回一个节点下的字段值
        /// </summary>
        /// <param name="VarName">该节点的名称</param>
        /// <param name="VarField">该节点下的字段名</param>
        /// <returns>返回该字段对应的值</returns>
        public string GetValue(string VarName, string VarField)
        {
            try
            {
                if (!File.Exists(m_ConfigFile))
                {
                    return string.Empty;
                }
                DataSet CfgData = new DataSet();
                //如果文件存在，读取文件
                CfgData.ReadXml(m_ConfigFile);
                if (CfgData.Tables.Count <= 0)
                {
                    return null;
                }
                if (CfgData.Tables[0].Columns.IndexOf(VarField) < 0)//列不存在
                {
                    //如果列不存在,返回null
                    return null;
                }
                DataRow[] rows = CfgData.Tables[0].Select(m_ConfigKeyField + "=" + "'" + VarName + "'");
                string strReturn = null;
                if (rows.Length <= 0)
                {
                    return string.Empty;
                }
                else
                {
                    strReturn = rows[0][VarField].ToString();
                }
                return strReturn;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 删除一节点下的字段值
        /// </summary>
        /// <param name="VarName">节点名称</param>
        /// <param name="VarField">该节点下的字段名</param>
        public void DeleteValue(string VarName, string VarField)
        {
            if (!File.Exists(m_ConfigFile))
            {
                return;
            }
            DataSet CfgData = new DataSet();
            //如果文件存在，读取文件
            CfgData.ReadXml(m_ConfigFile);
            if (CfgData.Tables.Count <= 0)
            {
                return;
            }
            if (CfgData.Tables[0].Columns.IndexOf(VarField) < 0)//列不存在
            {
                //如果列不存在,返回null
                return;
            }
            DataRow[] rows = CfgData.Tables[0].Select(m_ConfigKeyField + "=" + "'" + VarName + "'");
            if (rows.Length <= 0)
            {
                return;
            }
            else
            {
                rows[0][VarField] = string.Empty;
            }
            int ValidValueCount = 0;
            foreach (object obj in rows[0].ItemArray)
            {
                if (obj.ToString().Length > 0)
                {
                    ValidValueCount++;
                }
            }
            if (ValidValueCount <= 1)
            {
                rows[0].Delete();
            }
            CfgData.AcceptChanges();
            CfgData.WriteXml(m_ConfigFile, XmlWriteMode.WriteSchema);
        }
        /// <summary>
        /// 删除一节点下的所有字段值
        /// </summary>
        /// <param name="VarName">节点名称</param>
        public void DeleteVar(string VarName)
        {
            if (!File.Exists(m_ConfigFile))
            {
                return;
            }
            DataSet CfgData = new DataSet();
            //如果文件存在，读取文件
            CfgData.ReadXml(m_ConfigFile);
            if (CfgData.Tables.Count <= 0)
            {
                return;
            }
            DataRow[] rows = CfgData.Tables[0].Select(m_ConfigKeyField + "=" + "'" + VarName + "'");
            if (rows.Length <= 0)
            {
                return;
            }
            else
            {
                foreach (DataRow row in rows)
                {
                    row.Delete();
                }
            }
            CfgData.AcceptChanges();
            CfgData.WriteXml(m_ConfigFile, XmlWriteMode.WriteSchema);
        }
        /// <summary>
        /// 取的该配置文件下的所有数据
        /// </summary>
        /// <returns>返回以Table形式的数据</returns>
        public DataTable GetVarList()
        {
            try
            {
                if (!File.Exists(m_ConfigFile))
                {
                    return null;
                }
                DataSet CfgData = new DataSet();
                //如果文件存在，读取文件
                CfgData.ReadXml(m_ConfigFile);
                if (CfgData.Tables.Count <= 0)
                {
                    return null;
                }
                return CfgData.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据筛选条件取值
        /// </summary>
        /// <param name="Limit">要筛选的语句,类似SQL条件语句</param>
        /// <returns>返回过滤后的数据</returns>
        public DataTable GetVarList(string Limit)
        {
            try
            {
                if (!File.Exists(m_ConfigFile))
                {
                    return null;
                }
                DataSet CfgData = new DataSet();
                //如果文件存在，读取文件
                CfgData.ReadXml(m_ConfigFile);
                if (CfgData.Tables.Count <= 0)
                {
                    return null;
                }
                DataRow[] rows = CfgData.Tables[0].Select("not(" + Limit + ")");
                foreach (DataRow row in rows)
                {
                    row.Delete();
                }
                CfgData.Tables[0].AcceptChanges();
                if (CfgData.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                return CfgData.Tables[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
