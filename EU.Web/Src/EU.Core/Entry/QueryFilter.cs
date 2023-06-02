using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EU.Core.Entry
{
    /// <summary>
    /// 动态查询条件
    /// </summary>
    public class QueryFilter
    {
        private int _pageIndex;
        /// <summary>
        /// 起始位置(e.g. 0)
        /// </summary>
        [Required]
        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                //前端默认从分页显示默认1开始，所以后端需要-1
                if (value >= 1)
                    value -= 1;
                _pageIndex = value;
            }
        }
        /// <summary>
        /// 每页数量(e.g. 10)
        /// </summary>
        [Required]
        public int PageSize { get; set; }
        private string _predicate;
        /// <summary>
        /// 查询条件表达式(e.g. LoginName.Contains(@0))
        /// </summary>
        public string Predicate
        {
            get { return _predicate; }
            set
            {
                //前端默认从分页显示默认1开始，所以后端需要-1
                if (value == "1=1")
                    value = null;
                _predicate = value;
            }
        }
        /// <summary>
        /// 查询条件表达式参数(e.g. LoginName)
        /// </summary>
        public object[] PredicateValues { get; set; }
        /// <summary>
        /// 排序条件表达式(e.g. LoginName ASC,Name DESC)
        /// </summary>
        public string Sorting { get; set; }

        /// <summary>
        /// 分组条件(e.g. [10,20,30])
        /// </summary>
        public List<decimal> GroupValues { get; set; }
        /// <summary>
        /// 导出字段，按照顺序填写(e.g. [{ Key: 'PatientNameFull', Name: '患者姓名' }, { Key: 'DeviceCode', Name: '设备编号' }])
        /// </summary>
        public List<ExportField> ExportFields { get; set; }
        /// <summary>
        /// 导出字段的宽度（默认20），按照顺序与字段一一对应填写(e.g. [20,50,20])
        /// </summary>
        public List<int> ExportFieldsWidth { get; set; }
        /// <summary>
        /// 缺省值
        /// </summary>
        public static QueryFilter Default => new QueryFilter
        {
            PageIndex = 1,
            PageSize = 100000,
            Sorting = string.Empty,
            Predicate = string.Empty,
            PredicateValues = Array.Empty<object>(),
            GroupValues = new List<decimal>(),
            ExportFields = new List<ExportField>(),
            ExportFieldsWidth = new List<int>()
        };
    }

    public class ExportField
    {
        public string Key { get; set; }
        public string Name { get; set; }
    }
}
