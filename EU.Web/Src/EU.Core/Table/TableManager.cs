using EU.Core.Const;
using EU.Core.DBManager;
using EU.Core.Enums;
using EU.Core.LogHelper;
using EU.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace EU.Core
{
    public class TableManager
    {
        public static void InitTableAndField(string tableCode)
        {
            InitTableAndField(tableCode, "U");
            InitTableAndField(tableCode, "V");
        }
        #region 初始化公共方法
        private static void InitTableAndField(string tableCode, string typeCode)
        {
            string sql1 = string.Empty;
            try
            {


                #region 变量定义
                int count = 0;
                string sql = string.Empty;
                if (typeCode == "U")
                {
                    typeCode = "TABLE";
                }
                else
                {
                    typeCode = "VIEW";
                }
                #endregion
                #region 处理表
                sql = "SELECT COUNT(0) FROM SmTableCatalog A WHERE A.TableCode='{0}' AND A.IsDeleted='false' AND A.IsActive='true'";
                sql = string.Format(sql, tableCode);
                count = int.Parse(Convert.ToString(DBHelper.Instance.ExecuteScalar(sql)));
                if (count == 0)
                {
                    DbInsert di = new DbInsert("SmTableCatalog", "InitTableAndField");
                    di.Values("TableCode", tableCode);
                    di.Values("TableName", tableCode);
                    di.Values("TypeCode", typeCode);
                    DBHelper.Instance.ExcuteNonQuery(di.GetSql());
                }
                #endregion
                #region 处理新增列
                DataTable dtFieldCatalog = new DataTable();
                DataTable dtUserTabColumns = null;
                bool isMySql = DBType.Name == DbCurrentType.MySql.ToString();
                if (isMySql)
                {
                    sql = @"SELECT A.COLUMN_NAME,
                               CASE
                                  WHEN A.DATA_TYPE IN ('varchar', 'char', 'text')
                                  THEN
                                     A.character_maximum_length
                                  WHEN A.DATA_TYPE IN ('timestamp')
                                  THEN
                                     A.DATETIME_PRECISION
                                  WHEN A.DATA_TYPE IN ('int', 'decimal')
                                  THEN
                                     A.NUMERIC_PRECISION
                               END
                                  AS DATA_LENGTH,
                               CASE
                                  WHEN data_type IN ('BIT','BOOL','bit','bool')
                                  THEN
                                     'BOOL'
                                  WHEN data_type IN ('tinyint', 'TINYINT')
                                  THEN
                                     'SBYTE'
                                  WHEN data_type IN ('MEDIUMINT','mediumint','int','INT','year','Year')
                                  THEN
                                     'INT'
                                  WHEN data_type IN ('BIGINT', 'bigint')
                                  THEN
                                     'BIGINT'
                                  WHEN data_type IN ('FLOAT','DOUBLE','DECIMAL', 'float','double', 'decimal')
                                  THEN
                                     'DECIMAL'
                                  WHEN data_type IN ('CHAR',
                                                     'VARCHAR',
                                                     'TINY TEXT',
                                                     'TEXT',
                                                     'MEDIUMTEXT',
                                                     'LONGTEXT',
                                                     'TINYBLOB',
                                                     'BLOB',
                                                     'MEDIUMBLOB',
                                                     'LONGBLOB',
                                                     'Time',
                                                     'char',
                                                     'varchar',
                                                     'tiny text',
                                                     'text',
                                                     'mediumtext',
                                                     'longtext',
                                                     'tinyblob',
                                                     'blob',
                                                     'mediumblob',
                                                     'longblob',
                                                     'time')
                                  THEN
                                     'STRING'
                                  WHEN data_type IN ('Date','DateTime','TimeStamp','date','datetime','timestamp')
                                  THEN
                                     'DATETIME'
                                  ELSE
                                     'STRING'
                               END
                                  AS DATA_TYPE
                        FROM information_schema.COLUMNS A
                        WHERE table_name = '{0}' and table_schema = '{1}'";
                    sql = string.Format(sql, tableCode, GetMysqlTableSchema());
                }
                else
                {
                    sql = "SELECT B.NAME COLUMN_NAME,B.TYPE DATA_TYPE,B.LENGTH DATA_LENGTH FROM SYSOBJECTS A,SYSCOLUMNS B WHERE A.ID=B.ID AND A.NAME='{0}'";
                    sql = string.Format(sql, tableCode);
                }
                dtUserTabColumns = DBHelper.Instance.GetDataTable(sql);
                if (dtUserTabColumns != null && dtUserTabColumns.Rows.Count > 0)
                {
                    sql = "DELETE A FROM SmFieldCatalog A WHERE A.TableCode='{0}' AND (A.DataType='NUMBER' OR A.DataType='DATE')";
                    sql = string.Format(sql, tableCode);
                    DBHelper.Instance.ExecuteScalar(sql);
                    for (int i = 0; i < dtUserTabColumns.Rows.Count; i++)
                    {
                        sql = "SELECT COUNT(0) FROM SmFieldCatalog A WHERE A.TableCode='{0}' AND A.ColumnCode='{1}'";
                        sql = string.Format(sql, tableCode, Convert.ToString(dtUserTabColumns.Rows[i]["COLUMN_NAME"]).ToUpper());
                        count = int.Parse(Convert.ToString(DBHelper.Instance.ExecuteScalar(sql)));
                        if (count == 0)
                        {

                            //DataRow row = dtFieldCatalog.NewRow();
                            //row["ID"] = StringHelper.Id;
                            //row["TABLE_CODE"] = tableCode;
                            //row["COLUMN_CODE"] = Convert.ToString(dtUserTabColumns.Rows[i]["COLUMN_NAME"]).ToUpper();
                            //row["COLUMN_NAME"] = Convert.ToString(dtUserTabColumns.Rows[i]["COLUMN_NAME"]).ToUpper();

                            DbInsert di = new DbInsert("SmFieldCatalog", "InitTableAndField");
                            di.Values("TableCode", tableCode);
                            di.Values("ColumnCode", Convert.ToString(dtUserTabColumns.Rows[i]["COLUMN_NAME"]).ToUpper());
                            di.Values("ColumnName", Convert.ToString(dtUserTabColumns.Rows[i]["COLUMN_NAME"]).ToUpper());
                            string dataType = Convert.ToString(dtUserTabColumns.Rows[i]["DATA_TYPE"]);
                            if (!isMySql)
                            {
                                if (dataType == "39")
                                {
                                    dataType = "STRING";
                                }
                                else if (dataType == "37")
                                {
                                    dataType = "GUID";
                                }
                                //else if (dataType == "35")
                                //{
                                //    dataType = "TEXT";
                                //}
                                else if (dataType == "111")
                                {
                                    dataType = "DATETIME";
                                }
                                else if (dataType == "0")
                                {
                                    dataType = "DATE";
                                }
                                else if (dataType == "106")
                                {
                                    dataType = "DECIMAL";
                                }
                                else if (dataType == "38")
                                {
                                    dataType = "INT";
                                }
                                else
                                {
                                    dataType = "STRING";
                                }
                            }
                            di.Values("DataType", dataType);
                            string len = Convert.ToString(dtUserTabColumns.Rows[i]["DATA_LENGTH"]);
                            int length = 0;
                            if (!string.IsNullOrEmpty(len))
                            {
                                length = int.Parse(len);
                            }
                            di.Values("DataLength", length);
                            sql1 = di.GetSql();
                            DBHelper.Instance.ExcuteNonQuery(sql1);
                            //row["DATA_TYPE"] = dataType;
                            //row["DATA_LENGTH"] = int.Parse(Convert.ToString(dtUserTabColumns.Rows[i]["DATA_LENGTH"]));
                            //dtFieldCatalog.Rows.Add(row);
                            //DBHelper.Instance.BulkInsert(dtFieldCatalog, "SmFieldCatalog");
                        }
                    }
                }
                #endregion
                #region 删除列中不存在的数据
                sql = "SELECT A.* FROM SmFieldCatalog A WHERE A.TableCode='{0}'";
                sql = string.Format(sql, tableCode);
                dtFieldCatalog = DBHelper.Instance.GetDataTable(sql);
                for (int i = 0; i < dtFieldCatalog.Rows.Count; i++)
                {
                    if (isMySql)
                    {
                        sql = @"SELECT COUNT(0) FROM information_schema.COLUMNS WHERE table_name = '{0}'  AND table_schema = '{1}' AND COLUMN_NAME = '{2}'";
                        sql = string.Format(sql, tableCode, GetMysqlTableSchema(), dtFieldCatalog.Rows[i]["COLUMN_CODE"].ToString());
                    }
                    else
                    {
                        sql = "SELECT COUNT(0) FROM SYSOBJECTS A,SYSCOLUMNS B WHERE A.ID=B.ID AND A.NAME='{0}' AND B.NAME='{1}'";
                        sql = string.Format(sql, tableCode, dtFieldCatalog.Rows[i]["ColumnCode"].ToString());
                    }
                    count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(sql));
                    if (count == 0)
                    {
                        sql = "DELETE FROM SmFieldCatalog WHERE ID='{0}'";
                        sql = string.Format(sql, dtFieldCatalog.Rows[i]["ID"].ToString());
                        DBHelper.Instance.ExecuteScalar(sql);
                    }
                }
                #endregion
            }
            catch (Exception E)
            {
                Logger.WriteLog("GridList", sql1);
                Logger.WriteLog("GridList", E.StackTrace);
                throw;
            }
        }
        #endregion

        /// <summary>
        /// 2020.05.17增加mysql获取表结构时区分当前所在数据库
        /// </summary>
        /// <returns></returns>
        private static string GetMysqlTableSchema()
        {
            try
            {
                string dbName = DBServerProvider.GetConnectionString().Split("Database=")[1].Split(";")[0]?.Trim();
                if (string.IsNullOrEmpty(dbName))
                {
                    Console.WriteLine($"获取mysql数据库名失败:值为空!");
                }
                return dbName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取mysql数据库名异常:{ex.Message}");
                return "";
            }
        }
    }
}
