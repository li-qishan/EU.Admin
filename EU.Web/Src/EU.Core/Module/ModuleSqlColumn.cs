using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data;
using StackExchange.Redis;
using EU.Domain.System;
using EU.Core.Enums;
using System.Linq;
using EU.Model.System;
using EU.Core.CacheManager;
using EU.Core.Utilities;

namespace EU.Core.Module
{
    public class ModuleSqlColumn
    {
        private string moduleCode;
        public ModuleSqlColumn(string moduleCode)
        {
            this.moduleCode = moduleCode;
        }
        public List<SmModuleColumn> GetModuleSqlColumn()
        {
            List<SmModule> moduleList = ModuleInfo.GetModuleList();
            List<SmModuleColumn> cache = new RedisCacheService(2).Get<List<SmModuleColumn>>(CacheKeys.SmModuleColumn.ToString(), moduleCode);
            if (cache == null)
            {
                string sql = @"SELECT A.*, B.ModuleCode
                                FROM SmModuleColumn A
                                     JOIN SmModules B ON A.SmModuleId = B.ID AND A.IsDeleted = B.IsDeleted
                                WHERE A.IsDeleted = 'false'
                                ORDER BY A.TAXISNO ASC";
                cache = DBHelper.Instance.QueryList<SmModuleColumn>(sql);
                new RedisCacheService(2).Remove(CacheKeys.SmModuleColumn.ToString());
                foreach (SmModule item in moduleList)
                {
                    List<SmModuleColumn> ColumnList = cache.Where(x => x.ModuleCode == item.ModuleCode).ToList();
                    if (ColumnList.Count > 0)
                        new RedisCacheService(2).AddObject(CacheKeys.SmModuleColumn.ToString(), item.ModuleCode, ColumnList);
                }
                cache = cache.Where(x => x.ModuleCode == moduleCode).ToList();
            }
            return cache;
        }

        public string GetExportExcelColumns()
        {
            string columns = string.Empty;
            string name = string.Empty;
            string id = string.Empty;
            string sql = @"SELECT A.*, B.ModuleCode
                                FROM SmModuleColumn A
                                     JOIN SmModules B
                                        ON     A.SmModuleId = B.ID
                                           AND A.IsDeleted = B.IsDeleted
                                           AND B.ModuleCode = '{0}'
                                WHERE A.IsExport = 'true' AND A.IsDeleted = 'false'
                                ORDER BY A.TAXISNO ASC";
            sql = string.Format(sql, moduleCode);
            List<SmModuleColumn> moduleSqlColumn = DBHelper.Instance.QueryList<SmModuleColumn>(sql);
            if (moduleSqlColumn != null)
            {
                for (int i = 0; i < moduleSqlColumn.Count; i++)
                {
                    name = Convert.ToString(moduleSqlColumn[i].title);
                    id = Convert.ToString(moduleSqlColumn[i].dataIndex);
                    if (i < moduleSqlColumn.Count - 1)
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            columns += id + ",";
                        }
                        else
                        {
                            columns += id + " '" + name + "',";
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(name))
                        {
                            columns += id;
                        }
                        else
                        {
                            columns += id + " '" + name + "'";
                        }
                    }
                }
            }
            return columns;
        }

        public string GetExportExcelColumnRenderer(string columnName)
        {
            string renderer = string.Empty;
            //List<Sm_Module_Sql_Column> moduleSqlColumnList = GetModuleSqlAllColumn();
            //Sm_Module_Sql_Column moduleSqlColumn = moduleSqlColumnList.Where(x => x.Description?.ToLower() == columnName?.ToLower()).FirstOrDefault();
            //if (moduleSqlColumn != null)
            //{
            //    renderer = moduleSqlColumn.Renderer;
            //}
            return renderer;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            ModuleSqlColumn moduleColumnInfo = new ModuleSqlColumn("");
            moduleColumnInfo.GetModuleSqlColumn();
        }


    }
}
