using EU.Core.Entry;
using EU.Core.Utilities;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.IdentityModel.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EU.Core.Attributes
{
    public class FilterHeaderBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var name = bindingContext.FieldName;

            var headers = bindingContext.HttpContext.Request.Headers;

            QueryFilter queryFilter;

            if (!headers.ContainsKey(name))
            {
                queryFilter = QueryFilter.Default;
                bindingContext.Result = ModelBindingResult.Success(queryFilter);
                return Task.CompletedTask;
            }

            string filter = headers[name];

            if (bindingContext.ModelType == typeof(string))
            {
                bindingContext.Result = ModelBindingResult.Success(filter);
                return Task.CompletedTask;
            }

            try
            {
                if (string.IsNullOrEmpty(filter) || filter == "%22%22" || filter.ToLower() == "%7b%7d")
                {
                    queryFilter = QueryFilter.Default;
                }
                else if (filter.Trim() == "undefined" || filter.Trim() == "null")
                {
                    queryFilter = QueryFilter.Default;
                    //LoggerHelper.SendLogError($"QueryFilter 反序列化异常: {filter}\r\n请求地址: {bindingContext.HttpContext.Request.GetEncodedUrl()}");
                }
                else
                {
                    queryFilter = JsonConvert.DeserializeObject<QueryFilter>(System.Web.HttpUtility.UrlDecode(filter));
                    SetPredicateValues(queryFilter, bindingContext);
                }

                bindingContext.Result = ModelBindingResult.Success(queryFilter ?? QueryFilter.Default);
            }
            catch
            {
                //LoggerHelper.SendLogError($"QueryFilter 反序列化失败: {filter}\r\n请求地址: {bindingContext.HttpContext.Request.GetEncodedUrl()}");
                bindingContext.Result = ModelBindingResult.Success(QueryFilter.Default);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 设置 PredicateValues 的值类型
        /// </summary>
        /// <param name="queryFilter"></param>
        /// <param name="bindingContext"></param>
        private static void SetPredicateValues(QueryFilter queryFilter, ModelBindingContext bindingContext)
        {
            if (queryFilter?.PredicateValues == null || queryFilter.PredicateValues.Length == 0)
            {
                return;
            }

            for (int i = 0; i < queryFilter.PredicateValues.Length; i++)
            {
                if (queryFilter.PredicateValues[i] is JObject jObj)
                {
                    var prop = jObj.Properties()?.FirstOrDefault();
                    if (prop == null)
                        continue;
                    var type = Utility.StringConvertToType(prop.Name);
                    if (type == null)
                        continue;
                    try
                    {
                        var v = JsonConvert.DeserializeObject(prop.Value?.ToString(), type);
                        if (v != null)
                        {
                            queryFilter.PredicateValues[i] = v;
                        }
                    }
                    catch (Exception)
                    {
                        //LoggerHelper.SendLogError($"QueryFilter.PredicateValues[{i}] [{queryFilter.PredicateValues[i]}] 反序列化失败\r\n" +
                        //    $"请求地址: {bindingContext.HttpContext.Request.GetEncodedUrl()}\r\n" +
                        //    $"错误信息: {ex}");
                    }
                }
            }
        }
    }

    /// <summary>
    /// 从 Header 自动反序列化 QueryFilter 并绑定
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class FromFilterAttribute : Attribute, IBindingSourceMetadata, IModelNameProvider, IBinderTypeProviderMetadata
    {
        public FromFilterAttribute()
        {
        }

        public BindingSource BindingSource => BindingSource.Header;

        public string Name { get; set; }

        public Type BinderType => typeof(FilterHeaderBinder);
    }
}
