using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using EU.Model.System.Setup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.Setup
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class SmAutoCodeController : BaseController1<SmAutoCode>
    {
        private readonly IConfiguration Configuration;
        public SmAutoCodeController(IConfiguration configuration, DataContext _context, IBaseCRUDVM<SmAutoCode> BaseCrud) : base(_context, BaseCrud)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult GenerateCode(string code)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string Number = string.Empty;

            try
            {
                var setData = _context.Set<SmAutoCode>().Where(x => x.NumberCode == code).ToList();
                if (setData.Count > 0)
                {
                    int numberLength = setData[0].NumberLength;
                    string prefix = setData[0].Prefix;
                    string dateFormatType = setData[0].DateFormatType;
                    string tableName = setData[0].TableName;
                    string columnName = setData[0].ColumnName;
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        Number += prefix;
                    }
                    if (!string.IsNullOrEmpty(dateFormatType.ToString()))
                    {
                        if (dateFormatType == DateTypeEnum.YYYYMMDDHHMM.ToString())
                            Number += DateTime.Now.ToString("yyyyMMddhhmm");
                        else if (dateFormatType == DateTypeEnum.YYYYMMDDHH.ToString())
                            Number += DateTime.Now.ToString("yyyyMMddhh");
                        else if (dateFormatType == DateTypeEnum.YYYYMMDD.ToString())
                            Number += DateTime.Now.ToString("yyyyMMdd");
                        else if (dateFormatType == DateTypeEnum.YYYYMM.ToString())
                            Number += DateTime.Now.ToString("yyyyMM");
                        else if (dateFormatType == DateTypeEnum.YYYY.ToString())
                            Number += DateTime.Now.ToString("yyyy");
                    }
                    string sql = "SELECT MAX(SUBSTRING({1}, " + (Number.Length + 1) + "," + numberLength + ")) MaxCode FROM {0}";
                    sql = string.Format(sql, tableName, columnName);
                    DataTable dt = GetDataTable(sql);
                    int newCode;
                    if (dt.Rows.Count > 0 && !string.IsNullOrWhiteSpace(dt.Rows[0]["MaxCode"].ToString()))
                    {
                        try
                        {
                            newCode = int.Parse(dt.Rows[0]["MaxCode"].ToString()) + 1;
                        }
                        catch (Exception)
                        {
                            throw new Exception("表中的数据无法进行自动编号,请联系软件开发商！");
                        }
                    }
                    else
                    {
                        newCode = 1;
                    }

                    Number += newCode.ToString().PadLeft(numberLength - Number.Length, '0');

                    if (Number.Length > numberLength)
                    {
                        throw new Exception("自动生成字串长度已经超过设定长度！");
                    }
                }
                else
                {
                    throw new Exception("自动编号代码：" + code + "没有设置！");
                }
                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.number = Number;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #region sql查询直接返回DataTable
        [NonAction]
        public DataTable GetDataTable(string sql)
        {
            string conStr = Configuration.GetConnectionString("default");
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = conStr;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);
            conn.Close();//连接需要关闭
            conn.Dispose();
            return table;
        }
        #endregion
    }
}
