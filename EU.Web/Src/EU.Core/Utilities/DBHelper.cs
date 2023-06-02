using EU.Core.Enums;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using EU.Core.Dapper;
using EU.Core.Extensions.AutofacManager;
using EU.Core.DBManager;
using EU.Core.Const;
using System.Runtime.CompilerServices;

namespace EU.Core.Utilities
{
    public class DBHelper
    {
        //public static DataTable GetDataTable(ISqlDapper context,string sql,object param)
        //{
        //    DataTable table = new DataTable("MyTable");
        //    var reader = context.ExecuteReader(sql, param);
        //    table.Load(reader);
        //    return table;
        //}
        public static ISqlDapper Instance
        {
            get
            {
                return DBServerProvider.SqlDapper;
            }
        }


        public static bool MySql
        {
            get
            {
                bool result = false;
                if (DBType.Name == DbCurrentType.MySql.ToString())
                {
                    result = true;
                }
                return result;
            }
        }
        #region 检查表中是否已经存在相同代码的数据

        /// <summary>
        /// 检查表中是否已经存在相同代码的数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="modifyType">ModifyType.Add,ModifyType.Edit</param>
        /// <param name="rowid">ModifyType.Edit时修改记录的ROW_ID值</param>
        /// <param name="promptName">判断栏位的提示名称</param>
        public static void CheckCodeExist(string companyId, string tableName, string fieldName, string fieldValue, ModifyType modifyType, string rowid, string promptName)
        {
            try
            {
                CheckCodeExist(companyId, tableName, fieldName, fieldValue, modifyType, rowid, promptName, null);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 检查表中是否已经存在相同代码的数据
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="whereCondition">条件</param>
        /// <param name="modifyType">ModifyType.Add,ModifyType.Edit</param>
        /// <param name="rowid">ModifyType.Edit时修改记录的ROW_ID值</param>
        /// <param name="promptName">判断栏位的提示名称</param>
        /// <param name="whereCondition">Where后的条件，如：IS_ALCON='Y'</param>
        public static void CheckCodeExist(string companyId, string tableName, string fieldName, string fieldValue, ModifyType modifyType, string rowid, string promptName, string whereCondition)
        {
            try
            {
                //bool result = false;
                if (modifyType == ModifyType.Add)
                {
                    string sql = string.Empty;
                    if (string.IsNullOrEmpty(companyId))
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND DELETE_FLAG='N' ";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND COMPANY_ID='" + companyId + "' AND DELETE_FLAG='N' ";
                    }
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        sql += " AND " + whereCondition;
                    }
                    int count = Convert.ToInt32(Instance.ExecuteScalar(sql, null));
                    if (count > 0)
                    {
                        //result = true;
                        throw new Exception(promptName + "【" + fieldValue + "】已经存在！");
                    }
                    else
                    {
                        //result = false;
                    }
                }
                else if (modifyType == ModifyType.Edit)
                {
                    string sql = string.Empty;
                    if (string.IsNullOrEmpty(companyId))
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND DELETE_FLAG='N' AND ROW_ID!='" + rowid + "'";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND COMPANY_ID='" + companyId + "' AND DELETE_FLAG='N' AND ROW_ID!='" + rowid + "'";
                    }
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        sql += " AND " + whereCondition;
                    }
                    int count = Convert.ToInt32(Instance.ExecuteScalar(sql, null));
                    if (count > 0)
                    {
                        //result = true;
                        throw new Exception(promptName + "【" + fieldValue + "】已经存在！");
                    }
                    else
                    {
                        //result = false;
                    }
                }
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region 获取SQL插入语句
        /// <summary>
        /// 获取SQL插入语句
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="columnName">列名</param>
        /// <param name="columnValue">列值</param>
        /// <returns>SQL插入语句</returns>
        public StringBuilder GetInsertSql(string tableName, string columnName, string columnValue)
        {
            try
            {
                DbInsert di = null;
                string sql = null;
                StringBuilder sqls = new StringBuilder();
                DbSelect ds = new DbSelect(tableName + " A", "A");
                ds.IsInitDefaultValue = false;
                ds.Where("A." + columnName, "=", columnValue);
                DataTable dt = Instance.GetDataTable(ds.GetSql(), null);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    di = new DbInsert(tableName, "GetInsertSql");
                    di.IsInitDefaultValue = false;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        di.Values(dt.Columns[j].ColumnName, dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                    }
                    sql = di.GetSql();
                    sqls.Append(sql + ";\n");
                }
                return sqls;
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region 记录模块操作日志
        /// <summary>
        /// 记录模块操作日志
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleCode">模块代码</param>
        /// <param name="operateType">操作类型</param>
        public static void RecordOperateLog(string userId, string moduleCode, string tableCode, string tableRowId, OperateType operateType, string programName = null, string remark = null)
        {
            try
            {
                DbInsert di = new DbInsert("SmOperateLog", "RecordOperateLog");
                di.Values("UserId", userId);
                //di.Values("OperateUser", UserContext.Current.UserName);
                di.Values("OperateProgram", programName);
                di.Values("ModuleCode", moduleCode);
                di.Values("TableCode", tableCode);
                di.Values("TableRowId", tableRowId);
                di.Values("OperateDate", DateTime.Now);
                di.Values("Action", operateType.ToString());
                di.Values("Remark", remark);
                DBHelper.Instance.ExcuteNonQuery(di.GetSql());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region 数据库名称
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string DatabaseName
        {
            get
            {
                return Instance.Connection.Database;
            }
        }

        #endregion
    }

    /// <summary>
    /// 数据操作类型
    /// </summary>
    public enum OperateType
    {
        Query,
        Add,
        Update,
        Delete,
        Excel,
        Favorite,
        Execute,
        Import
    }
}
