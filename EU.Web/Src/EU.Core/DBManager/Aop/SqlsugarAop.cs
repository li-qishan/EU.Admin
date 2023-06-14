using EU.Core;
using EU.Entity.SystemModels;
using Senparc.Weixin.MP.AdvancedAPIs.MerChant;
using SqlSugar;
using System;

namespace Blog.Core.Common.DB;

public static class SqlSugarAop
{
    public static void DataExecuting(object oldValue, DataFilterModel entityInfo)
    {
        //if (entityInfo.EntityValue is PersistPoco root)
        //{
        //    if (root.Id == 0)
        //    {
        //        root.Id = SnowFlakeSingle.Instance.NextId();
        //    }
        //}

        if (entityInfo.EntityValue is EU.Model.Base.PersistPoco baseEntity)
        {
            // 新增操作
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                if (baseEntity.CreatedTime == DateTime.MinValue)
                {
                    baseEntity.CreatedTime = DateTime.Now;
                }
            }

            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                baseEntity.UpdateTime = DateTime.Now;
            }


            //if (App.User?.ID > 0)
            //{
            //    if (baseEntity is ITenantEntity tenant && App.User.TenantId > 0)
            //    {
            //        if (tenant.TenantId == 0)
            //        {
            //            tenant.TenantId = App.User.TenantId;
            //        }
            //    }

            //    switch (entityInfo.OperationType)
            //    {
            //        case DataFilterType.UpdateByObject:
            //            baseEntity.ModifyId = App.User.ID;
            //            baseEntity.ModifyBy = App.User.Name;
            //            break;
            //        case DataFilterType.InsertByObject:
            //            if (baseEntity.CreateBy.IsNullOrEmpty() || baseEntity.CreateId is null or <= 0)
            //            {
            //                baseEntity.CreateId = App.User.ID;
            //                baseEntity.CreateBy = App.User.Name;
            //            }

            //            break;
            //    }
            //}
        }
    }

    private static string GetWholeSql(SugarParameter[] paramArr, string sql)
    {
        foreach (var param in paramArr)
        {
            sql = sql.Replace(param.ParameterName, $@"'{param.Value.ObjToString()}'");
        }

        return sql;
    }

    private static string GetParas(SugarParameter[] pars)
    {
        string key = "【SQL参数】：";
        foreach (var param in pars)
        {
            key += $"{param.ParameterName}:{param.Value}\n";
        }

        return key;
    }
}