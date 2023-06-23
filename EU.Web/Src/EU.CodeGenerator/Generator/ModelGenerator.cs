using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EU.CodeGenerator;
using EU.Core.Utilities;
using MySqlX.XDevAPI.Relational;

namespace JianLian.HDIS.CodeGenerator
{
    /// <summary>
    /// ModelGenerator
    /// </summary>
    public class ModelGenerator
    {
        private static readonly Regex Pattern = new Regex(
            "`DIC.([\\w\\W]*?)`");

        /// <summary>
        /// 生成Model，每次都是删除重建
        /// </summary>
        /// <param name="tables"></param>
        public static void Generator(string tableName)
        {
            string _tmpl = File.ReadAllText("..\\..\\..\\Tmpl\\ModelTmpl.txt");
            string path = $"{Utilities.BasePath}Src\\EU.Model\\{Utilities.FileName}\\";
            _tmpl = _tmpl.Replace("@TableNameBase", tableName);
            _tmpl = _tmpl.Replace("@CurrentTime", DateTime.Now.ToString());
            _tmpl = _tmpl.Replace("@CurrentTimeYear", DateTime.Now.Year.ToString());

            string modelName = tableName;
            string TableCnName = string.Empty;

            StringBuilder build = new StringBuilder();
            string sql = @"SELECT A.name AS table_name,
                                       B.name AS column_name,
                                       D.data_type,
                                       C.value AS column_description,
                                       D.NUMERIC_PRECISION, D.NUMERIC_SCALE
                                FROM sys.tables A
                                     INNER JOIN sys.columns B ON B.object_id = A.object_id
                                     LEFT JOIN sys.extended_properties C
                                        ON C.major_id = B.object_id AND C.minor_id = B.column_id
                                     LEFT JOIN information_schema.columns D
                                        ON D.column_name = B.name AND D.TABLE_NAME = '{0}'
                                WHERE A.name = '{0}'";
            sql = string.Format(sql, tableName);
            DataTable dtColumn = DBHelper.Instance.GetDataTable(sql);

            #region 获取表中文名
            sql = @"SELECT f.value TableName
                            FROM sysobjects d
                                 LEFT JOIN sys.extended_properties f
                                    ON d.id = f.major_id AND f.minor_id = 0
                            WHERE d.name = '{0}'";
            sql = string.Format(sql, tableName);
            TableCnName = Convert.ToString(DBHelper.Instance.ExecuteScalar(sql));
            _tmpl = _tmpl.Replace("@TableName", tableName);
            #endregion

            #region 属性
            //build.Append("<br/>");

            #region 处理表字段
            string columnCode = string.Empty;
            string dataType = string.Empty;
            string column_description = string.Empty;
            string NUMERIC_PRECISION = string.Empty;
            string NUMERIC_SCALE = string.Empty;

            string[] a = {
                        "ID", "CreatedBy", "CreatedTime", "UpdateBy", "UpdateTime", "ImportDataId", "ModificationNum",
                    "Tag", "GroupId", "CompanyId", "CurrentNode", "Remark","IsActive","AuditStatus","IsDeleted"

            };

            for (int i = 0; i < dtColumn.Rows.Count; i++)
            {
                columnCode = dtColumn.Rows[i]["column_name"].ToString();
                dataType = dtColumn.Rows[i]["data_type"].ToString();
                column_description = dtColumn.Rows[i]["column_description"].ToString();
                NUMERIC_PRECISION = dtColumn.Rows[i]["NUMERIC_PRECISION"].ToString();
                NUMERIC_SCALE = dtColumn.Rows[i]["NUMERIC_SCALE"].ToString();

                if (a.Contains(columnCode))
                    continue;
                if (i != 0)
                {
                    build.AppendLine();
                    build.AppendLine();
                }

                build.AppendLine("        /// <summary>");
                build.AppendLine($"        /// {column_description}");
                build.AppendLine("        /// </summary>");
                if (dataType == "decimal")
                    build.Append($"        [Display(Name = \"" + columnCode + "\"), Description(\"" + column_description + "\"), Column(TypeName = \"decimal(" + NUMERIC_PRECISION + "," + NUMERIC_SCALE + ")\")]");
                else build.Append("        [Display(Name = \"" + columnCode + "\"), Description(\"" + column_description + "\")]");
                build.AppendLine();

                switch (dataType)
                {
                    #region 字符串
                    case "varchar":
                        {
                            build.Append("        public string " + columnCode + " { get; set; }");
                            break;
                        }
                    case "char":
                        {
                            build.Append("        public string " + columnCode + " { get; set; }");
                            break;
                        }
                    case "text":
                        {
                            build.Append("        public string " + columnCode + " { get; set; }");
                            break;
                        }
                    #endregion

                    #region 日期
                    case "datetime":
                        {
                            build.Append("        public DateTime? " + columnCode + " { get; set; }");
                            break;
                        }
                    case "date":
                        {
                            build.Append("        public DateTime? " + columnCode + " { get; set; }");
                            break;
                        }
                    #endregion

                    #region 数字
                    case "decimal":
                        {

                            build.Append("        public decimal? " + columnCode + " { get; set; }");
                        }
                        break;
                    case "int":
                        {

                            build.Append("        public int? " + columnCode + " { get; set; }");

                            break;
                        }
                    case "uniqueidentifier":
                        {

                            build.Append("        public Guid? " + columnCode + " { get; set; }");

                            break;
                        }
                    case "bit":
                        {

                            build.Append("        public bool? " + columnCode + " { get; set; }");

                            break;
                        }
                        #endregion
                }
            }
            #endregion
            #endregion

            //生成文件
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            _tmpl = _tmpl.Replace("@Colunms", build.ToString());
            Utilities.CreateFile($"{path}\\{tableName}.cs", _tmpl, true);

            Console.WriteLine("ModelGenerator Completed!");
        }

    }
}
