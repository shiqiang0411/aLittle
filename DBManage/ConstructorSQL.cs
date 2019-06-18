using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace DBManage
{
    /// <summary>
    /// SQL语句构造类---支持构造Select,Update,Delete,Insert,Count语句
    /// </summary>
    public class ConstructorSQL
    {
        /// <summary>
        /// 表名
        /// </summary>
        private string m_strTableName = string.Empty;
        /// <summary>
        /// 字段列表---字段名对应字段值
        /// </summary>
        private Hashtable m_FieldList = null;
        /// <summary>
        /// 执行Select语句时需要返回的字段
        /// </summary>
        private string[] m_SelectReturnField = null;

        /// <summary>
        /// 构造SQL语句时Where后的条件字段列表[字段名，字段值]----为空的话就默认之前的字段列表m_FieldList
        /// </summary>
        private Hashtable m_WhereCondition = null;

        /// <summary>
        ///下面是各种附加部分，包括
        /// 1.查询前缀，如distinct
        /// 2.查询后缀，如括号
        /// 3.条件后缀
        /// 4.子句后缀
        /// 表（记录）查询前缀，例如distinct等
        /// </summary>
        private string m_strSelectPrefix = string.Empty;

        /// <summary>
        /// 表查询后缀，例如括号
        /// </summary>
        private string m_strSelectSuffix = string.Empty;

        /// <summary>
        /// 表（记录）条件后缀
        /// </summary>
        private string m_strConditionSuffix = string.Empty;

        /// <summary>
        /// 子句后缀，如order by, group by等，在它前面不加and
        /// </summary>
        private string m_strSubClauseSuffix = string.Empty;


        /// <summary>
        /// SQL语句构造类
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="ParamHash">字段列表[字段名，字段值]</param>
        public ConstructorSQL(string strTableName, Hashtable ParamHash)
        {
            m_strTableName = strTableName;
            //m_WhereCondition默认等于ParamHash
            m_WhereCondition = m_FieldList = ParamHash;//同时为条件字段列表
        }

        /// <summary>
        /// SQL语句构造类
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="ParamHash">字段列表[字段名，字段值]</param>
        /// <param name="WhereCondition">构造SQL语句时Where后的条件字段列表[字段名，字段值]----如果为空的话就是空Where语句，执行Update或者Delete语句会出错</param>
        public ConstructorSQL(string strTableName, Hashtable ParamHash, Hashtable WhereCondition)
        {
            m_strTableName = strTableName;
            m_FieldList = ParamHash;
            m_WhereCondition = WhereCondition;
        }
        /// <summary>
        /// 语句构造类
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="ParamHash">字段列表[字段名，字段值]</param>
        /// <param name="SelectReturnField">执行Select语句时需要返回的字段</param>
        public ConstructorSQL(string strTableName, Hashtable ParamHash, params string[] SelectReturnField)
        {
            m_strTableName = strTableName;
            m_WhereCondition = m_FieldList = ParamHash;
            m_SelectReturnField = SelectReturnField;
        }
        /// <summary>
        /// 语句构造类
        /// </summary>
        /// <param name="strTableName">表名</param>
        /// <param name="ParamHash">字段列表[字段名，字段值]</param>
        /// <param name="WhereCondition">构造SQL语句时Where后的条件字段列表[字段名，字段值]----如果为空的话就是空Where语句，执行Update或者Delete语句会出错</param>
        /// <param name="SelectReturnField">执行Select语句时需要返回的字段</param>
        public ConstructorSQL(string strTableName, Hashtable ParamHash, Hashtable WhereCondition, params string[] SelectReturnField)
        {
            m_strTableName = strTableName;
            m_FieldList = ParamHash;
            if (WhereCondition != null)
            {
                m_WhereCondition = WhereCondition;
            }
            else
            {
                m_WhereCondition = m_FieldList;
            }
            m_SelectReturnField = SelectReturnField;
        }



        /// <summary>
        /// 表（记录）查询前缀，例如distinct等
        /// </summary>
        public string SelectPrefix
        {
            get
            {
                return m_strSelectPrefix;
            }
            set
            {
                m_strSelectPrefix = value;
            }
        }
        /// <summary>
        /// 表查询后缀，例如括号
        /// </summary>
        public string SelectSuffix
        {
            get
            {
                return m_strSelectSuffix;
            }
            set
            {
                m_strSelectSuffix = value;
            }
        }
        /// <summary>
        ///  表（记录）条件后缀
        /// </summary>
        public string ConditionSuffix
        {
            get
            {
                return m_strConditionSuffix;
            }
            set
            {
                m_strConditionSuffix = value;
            }
        }
        /// <summary>
        /// 子句后缀，如order by, group by等，在它前面不加and
        /// </summary>
        public string SubClauseSuffix
        {
            get
            {
                return m_strSubClauseSuffix;
            }
            set
            {
                m_strSubClauseSuffix = value;
            }
        }
        public String getSelectSQL()
        {
            return getSelectSQL(true);
        }

        /// <summary>
        /// 获得查询语句
        /// </summary>
        /// <returns></returns>
        public String getSelectSQL(bool blLike)
        {
            StringBuilder sbSQL = new StringBuilder(256);

            sbSQL.Append("SELECT ");
            sbSQL.Append(SelectPrefix + " ");
            if (m_SelectReturnField == null)
            {
                sbSQL.Append(" * ");
            }
            else
            {//返回需要设置的字段
                for (int i = 0, j = 0; i < m_SelectReturnField.Length; i++)
                {
                    if (j != 0)
                    {
                        sbSQL.Append(",");
                    }
                    sbSQL.Append(m_SelectReturnField[i]);
                    j++;
                }
            }
            sbSQL.Append(" " + SelectSuffix);
            sbSQL.Append(" FROM ");
            sbSQL.Append(m_strTableName);
            sbSQL.Append(getWhereClause(blLike));

            if (m_strSubClauseSuffix != null)
                sbSQL.Append(m_strSubClauseSuffix);


            return sbSQL.ToString();
        }


        /// <summary>
        /// 获得更新语句
        /// </summary>
        /// <returns></returns>
        public String getUpdateSQL()
        {
            CleanFieldList();

            StringBuilder sbSQL = new StringBuilder(256);

            sbSQL.Append("UPDATE ");
            sbSQL.Append(m_strTableName);
            sbSQL.Append(" SET ");

            // 查询前缀是指查询语句的前面的修饰前缀，如distinct等

            if (!string.IsNullOrEmpty(SelectPrefix))
            {
                sbSQL.Append(SelectPrefix + ",");
            }

            if (m_FieldList != null && m_FieldList.Keys.Count > 0)
            {
                int j = 0;
                foreach (string pn in m_FieldList.Keys)
                {
                    if (j != 0)
                    {
                        sbSQL.Append(",");
                    }
                    sbSQL.Append(pn);
                    sbSQL.Append("=");
                    sbSQL.Append(convertField(m_FieldList[pn]));
                    j++;
                }
            }
            sbSQL.Append(getWhereClause());
            sbSQL.Append(";");
            return sbSQL.ToString();
        }

        /// <summary>
        /// 清理字段列表m_FieldList中的条件列表
        /// </summary>
        private void CleanFieldList()
        {
            if (m_WhereCondition == null || m_WhereCondition.Keys.Count < 1) return;
            foreach (string pn in m_WhereCondition.Keys)
            {
                if (m_FieldList.Contains(pn))
                {
                    m_FieldList.Remove(pn);
                }
            }
        }

        /// <summary>
        /// 获得本记录对应的插入语句
        /// </summary>
        /// <returns></returns>
        public String getInsertSQL()
        {
            StringBuilder sbFieldNameList = new StringBuilder(256);
            StringBuilder sbFieldValueList = new StringBuilder(256);

            if (m_FieldList != null && m_FieldList.Keys.Count > 0)
            {
                int j = 0;
                foreach (string pn in m_FieldList.Keys)
                {
                    if (j != 0)
                    {
                        sbFieldNameList.Append(",");
                        sbFieldValueList.Append(",");
                    }
                    sbFieldNameList.Append(pn);
                    sbFieldValueList.Append(convertField(m_FieldList[pn]));
                    j++;
                }
            }

            StringBuilder sbSQL = new StringBuilder(256);

            sbSQL.Append("INSERT INTO ");
            sbSQL.Append(m_strTableName); // insert statement head
            sbSQL.Append(" (");
            sbSQL.Append(sbFieldNameList);
            sbSQL.Append(")");
            sbSQL.Append(" VALUES (");
            sbSQL.Append(sbFieldValueList);
            sbSQL.Append(");");

            return sbSQL.ToString();
        }
        /// <summary>
        /// 获取插入语句(存在就不插入-->Mysql)
        /// </summary>
        /// <param name="repetition">是否去重复</param>
        /// <returns></returns>
        public String getInsertSQL(bool isRepetition)
        {
            StringBuilder sbFieldNameList = new StringBuilder(256);
            StringBuilder sbFieldValueList = new StringBuilder(256);

            if (m_FieldList != null && m_FieldList.Keys.Count > 0)
            {
                int j = 0;
                foreach (string pn in m_FieldList.Keys)
                {
                    if (j != 0)
                    {
                        sbFieldNameList.Append(",");
                        sbFieldValueList.Append(",");
                    }
                    sbFieldNameList.Append(pn);
                    sbFieldValueList.Append(convertField(m_FieldList[pn]));
                    j++;
                }
            }

            StringBuilder sbSQL = new StringBuilder(256);
            if (isRepetition)
            {
                sbSQL.Append("INSERT IGNORE INTO ");
            }
            else
            {
                sbSQL.Append("INSERT INTO ");
            }
            sbSQL.Append(m_strTableName); // insert statement head
            sbSQL.Append(" (");
            sbSQL.Append(sbFieldNameList);
            sbSQL.Append(")");
            sbSQL.Append(" VALUES (");
            sbSQL.Append(sbFieldValueList);
            sbSQL.Append(");");

            return sbSQL.ToString();
        }

        /// <summary>
        /// 获得删除语句
        /// </summary>
        /// <returns></returns>
        public String getDeleteSQL()
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("DELETE FROM ");
            sbSQL.Append(m_strTableName);
            sbSQL.Append(getWhereClause());
            return sbSQL.ToString();
        }


        /// <summary>
        /// 获得查询数量语句
        /// </summary>
        /// <returns></returns>
        public String getCountSQL()
        {
            StringBuilder sbSQL = new StringBuilder();

            sbSQL.Append("SELECT COUNT(*) FROM ");
            sbSQL.Append(m_strTableName);
            sbSQL.Append(getWhereClause());

            if (m_strSubClauseSuffix != null)
                sbSQL.Append(m_strSubClauseSuffix);

            return sbSQL.ToString();
        }

        //public String getMaxSQL()
        //{
        //    StringBuilder sbSQL = new StringBuilder(256);

        //    sbSQL.Append("SELECT ");
        //    sbSQL.Append("MAX(");
        //    sbSQL.Append(SelectPrefix);
        //    sbSQL.Append(") FROM ");
        //    sbSQL.Append(m_strTableName);
        //    sbSQL.Append(getWhereClause());

        //    if (m_strSubClauseSuffix != null)
        //        sbSQL.Append(m_strSubClauseSuffix);

        //    return sbSQL.ToString();
        //}
        private String getWhereClause()
        {
            return getWhereClause(false);
        }

        /// <summary>
        /// 获得条件语句
        /// </summary>
        /// <param name="blLike">是否模糊</param>
        /// <returns></returns>
        private String getWhereClause(bool blLike)
        {
            StringBuilder sbWhereClause = new StringBuilder(1024);

            // j记录有几个条件字段，i记录当前字段
            if (m_WhereCondition != null && m_WhereCondition.Keys.Count > 0)
            {
                int j = 0;
                foreach (string pn in m_WhereCondition.Keys)
                {
                    if (m_WhereCondition[pn] != null && m_WhereCondition[pn].ToString().Length > 0)
                    {// 如果条件值非空
                        if (j != 0)
                        {
                            sbWhereClause.Append(" AND ");
                        }
                        string strConditionValue = m_WhereCondition[pn].ToString();
                        if (strConditionValue.ToLower() == "is null")
                        { // 如果条件值是null
                            sbWhereClause.Append(pn);
                            sbWhereClause.Append(" IS NULL");
                        }
                        else
                        { // 如果条件值非空
                            sbWhereClause.Append(pn);
                            if (ExistsOperationalRelations(pn))
                            {//存在关系运算符号才采用默认的等号运算
                                sbWhereClause.Append(convertField(strConditionValue));
                            }
                            else
                            {
                                if (!blLike)
                                {
                                    sbWhereClause.Append("=");
                                    sbWhereClause.Append(convertField(strConditionValue));
                                }
                                else
                                {
                                    sbWhereClause.Append(" like '%");
                                    sbWhereClause.Append(FilterStringForDB(strConditionValue));
                                    sbWhereClause.Append("%'");
                                }
                            }


                            j++;
                        }
                    }
                }
            }
            if (sbWhereClause.Length != 0 && ConditionSuffix.Length != 0)
                sbWhereClause.Append(" AND ");

            sbWhereClause.Append(ConditionSuffix);

            if (sbWhereClause.Length != 0)
            {
                sbWhereClause.Insert(0, " WHERE ");
            }
            return sbWhereClause.ToString();
        }

        private string[] strWheres = new string[] { "<", ">", "<=", ">=", "=" };

        /// <summary>
        /// 判断字段中是否添加了关系运算符号---如时间和数字类型需要手动添加运算符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private bool ExistsOperationalRelations(string strField)
        {
            foreach (string str in strWheres)
            {
                if (strField.EndsWith(str))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 验证是否是数字
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        internal bool IsMath(string Value)
        {
            return Regex.IsMatch(Value, @"^[0-9]*$");
        }

        private String convertField(object strFieldValue)
        {
            if (strFieldValue != null && !string.IsNullOrEmpty(strFieldValue.ToString()))
            {
                return convertField(strFieldValue.ToString());
            }
            if (strFieldValue == null || string.IsNullOrEmpty(strFieldValue.ToString()))
            {
                return "NULL";
            }
            else
            {
                return "''";
            }
        }

        private String convertField(string strFieldValue)
        {
            String strFieldValueResult = "";
            char Temp = '\'';
            if (!string.IsNullOrEmpty(strFieldValue))
            {

                strFieldValueResult = "'"
                        + FilterStringForDB(strFieldValue) + "'";
                if (IsMath(strFieldValue) && strFieldValue.Length<3)
                    strFieldValueResult = strFieldValueResult.Trim(Temp);
            }
            else
            {
                strFieldValueResult = "''";
            }
            return strFieldValueResult;
        }

        private string FilterStringForDB(string strSource)
        {
            /* 原来曾经自动去掉空格，但是发现不可，因为有些字段如文章前面

            必然空两个格，去掉是不对的

            // 首先去掉两边的空格

            // strSource = strSource.trim(); */

            if (!string.IsNullOrEmpty(strSource))
            {
                // 将双字节括号变为单字节括号，这样防止查询不到指定内容
                strSource = strSource.Replace('（', '(');
                strSource = strSource.Replace('）', ')');

                // 将一个单引号变为双单引号，这样数据库才认为是字符串内部的引号
                strSource = strSource.Replace("'", "''");

                return strSource;
            }
            else
                return "";
        }

    }
}
