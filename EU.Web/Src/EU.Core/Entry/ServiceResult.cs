using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU.Core.Entry
{
    /// <summary>
    /// 服务层响应实体
    /// </summary>
    public class ServiceResult
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 数量
        /// </summary>

        public long Count { get; set; }

        /// <summary>
        /// 响应码
        /// </summary>
        public ServiceResultCode Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public bool Success => Code == ServiceResultCode.Succeed;

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ServiceResult OprateSuccess(string message = "操作成功", ServiceResultCode code = ServiceResultCode.Succeed)
        {
            return new ServiceResult
            {
                Message = message,
                Code = code,
                Data = null,
                Count = 0
            };
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ServiceResult OprateFailed(string message = "操作失败", ServiceResultCode code = ServiceResultCode.Failed)
        {
            return new ServiceResult
            {
                Message = message,
                Code = code,
                Data = null,
                Count = 0
            };
        }
    }

    /// <summary>
    /// 服务层响应实体(泛型)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// 返回结果
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// 数量
        /// </summary>

        public long Count { get; set; }

        /// <summary>
        /// 响应码
        /// </summary>
        public ServiceResultCode Code { get; set; }

        /// <summary>
        /// 响应信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public bool Success => Code == ServiceResultCode.Succeed;

        /// <summary>
        /// 当前页
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 一页多少数据
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ServiceResult<T> OprateSuccess(T data = default, string message = "操作成功", ServiceResultCode code = ServiceResultCode.Succeed)
        {
            return new ServiceResult<T>
            {
                Message = message,
                Code = code,
                Data = data,
                Count = data == null ? 0 : 1
            };
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count"></param>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ServiceResult<T> OprateSuccess(T data, long count, string message = "操作成功", ServiceResultCode code = ServiceResultCode.Succeed)
        {
            return new ServiceResult<T>
            {
                Message = message,
                Code = code,
                Data = data,
                Count = count
            };
        }

        public static ServiceResult<T> OprateSuccess(T data, long count, int current, int pageSize, string message = "操作成功", ServiceResultCode code = ServiceResultCode.Succeed)
        {
            return new ServiceResult<T>
            {
                Message = message,
                Code = code,
                Data = data,
                Count = count,
                Current = current,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// 操作失败
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ServiceResult<T> OprateFailed(string message = "操作失败", ServiceResultCode code = ServiceResultCode.Failed)
        {
            return new ServiceResult<T>
            {
                Message = message,
                Code = code,
                Data = default,
                Count = 0
            };
        }

        public static implicit operator ServiceResult(ServiceResult<T> @this)
        {
            return new ServiceResult
            {
                Message = @this.Message,
                Code = @this.Code,
                Data = @this.Data,
                Count = @this.Count
            };
        }
    }
}
