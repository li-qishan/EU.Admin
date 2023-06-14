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
using EU.Core.LogHelper;
using System.Globalization;
using EU.Core.Randoms;
using Util.Tools.QrCode.QrCoder;
using QRCoder;
using System.Drawing;

namespace EU.Core.Utilities
{
    public class StringHelper
    {
        //public static ICacheService GetInstance()
        //{
        //    //测试Util
        //    //string test=GuidRandomGenerator.Instance.Generate();
        //    //Export e = new Export();
        //    //e.Test_1();
        //    //e.Test_2();
        //    //Logger.WriteLog("test");
        //    ICacheService cacheService = AutofacContainerModule.GetService<ICacheService>();
        //    return cacheService;
        //}
        public static string Id
        {
            get
            {
                Guid id = Guid.NewGuid();
                return id.ToString();
                //return GuidRandomGenerator.Instance.Generate();
            }
        }

        public static string ToTitleCase(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = title.ToLower();
                TextInfo ti = new CultureInfo("en-US", false).TextInfo;
                title = ti.ToTitleCase(title);
            }
            return title;
        }

        /// <summary>
        /// 初始化ModuleSql.Oracle.xml中SQL的变量
        /// </summary>
        /// <param name="sqlString"></param>
        /// <returns></returns>
        //public static string FormatSqlVariable(string sqlString)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(sqlString))
        //        {
        //            if (sqlString.IndexOf("[CompanyId]") > -1)
        //            {
        //                sqlString = sqlString.Replace("[CompanyId]", UserContext.Current.CompanyId);
        //            }
        //            if (sqlString.IndexOf("[QueryGroupId]") > -1)
        //            {
        //                sqlString = sqlString.Replace("[QueryGroupId]", "jt20080101");
        //            }
        //            if (sqlString.IndexOf("[UserId]") > -1)
        //            {
        //                sqlString = sqlString.Replace("[UserId]", UserContext.Current.User_Id);
        //            }
        //            if (sqlString.IndexOf("[UserCode]") > -1)
        //            {
        //                sqlString = sqlString.Replace("[UserCode]", UserContext.Current.UserName);
        //            }
        //            if (sqlString.IndexOf("[Language]") > -1)
        //            {
        //                sqlString = sqlString.Replace("[Language]", UserContext.Current.Language);
        //            }
        //            if (sqlString.IndexOf("[EmployeeId]") > -1)
        //            {
        //                sqlString = sqlString.Replace("[EmployeeId]", UserContext.Current.EmployeeId);
        //            }
        //        }
        //        return sqlString;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception(e.Message);
        //    }
        //}

        /// <summary>
        /// 格式化数字字符，如传入1.24500，返回1.245
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimDecimalString(string value)
        {
            try
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    Decimal tmp = Decimal.Parse(value);
                    result = string.Format("{0:#0.##########}", tmp);
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 格式化数字字符，并保留指定的小数位
        /// </summary>
        /// <param name="value">需要处理的值</param>
        /// <param name="reservedDigit">保留小数点后位数</param>
        /// <returns></returns>
        public static string TrimDecimalString(string value, int reservedDigit)
        {
            try
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    Decimal tmp = Decimal.Parse(value);
                    if (reservedDigit == -1)
                    {
                        result = string.Format("{0:#0.##########}", tmp);
                    }
                    else
                    {
                        result = String.Format("{0:N" + reservedDigit.ToString() + "}", tmp);
                        result = result.Replace(",", "");
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 格式化数字字符，并保留指定的小数位
        /// </summary>
        /// <param name="value">需要处理的值</param>
        /// <param name="reservedDigit">保留小数点后位数，-1时只会去除小数点后最后几位的0</param>
        /// <returns></returns>
        public static string TrimDecimalString(object value, int reservedDigit)
        {
            try
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    Decimal tmp = Decimal.Parse(Convert.ToString(value));
                    if (reservedDigit == -1)
                    {
                        result = string.Format("{0:#0.##########}", tmp);
                    }
                    else
                    {
                        result = String.Format("{0:N" + reservedDigit.ToString() + "}", tmp);
                        result = result.Replace(",", "");
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 格式化数字字符，并保留指定的小数位
        /// </summary>
        /// <param name="value">需要处理的值</param>
        /// <param name="reservedDigit">保留小数点后位数，-1时只会去除小数点后最后几位的0</param>
        /// <returns></returns>
        public static decimal TrimDecimal(object value, int reservedDigit)
        {
            try
            {
                string result = string.Empty;
                if (!string.IsNullOrEmpty(Convert.ToString(value)))
                {
                    Decimal tmp = Decimal.Parse(Convert.ToString(value));
                    if (reservedDigit == -1)
                    {
                        result = string.Format("{0:#0.##########}", tmp);
                    }
                    else
                    {
                        result = String.Format("{0:N" + reservedDigit.ToString() + "}", tmp);
                        result = result.Replace(",", "");
                    }
                }
                return Convert.ToDecimal(result);
            }
            catch (Exception) { throw; }
        }

        public static byte[] CreateQrCode(string content)
        {
            QrCoderService service = new QrCoderService();
            byte[] qrCode = service.CreateQrCode(content);
            return qrCode;
        }

        public static void CreateQrCode(string content, string fileName, int size = 0)
        {
            QrCoderService service = new QrCoderService();
            if (size != 0)
                service.Size(size);
            byte[] qrCode = service.CreateQrCode(content);

            //QRCodeGenerator qrGenerator = new QRCodeGenerator();
            //QRCodeData qrCodeData = qrGenerator.CreateQrCode("The text which should be encoded.", QRCodeGenerator.ECCLevel.Q);
            //QRCode qrCode = new QRCode(qrCodeData);
            //Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //ByteArrayOutputStream baos = new ByteArrayOutputStream();
            //mRBmp.compress(Bitmap.CompressFormat.JPEG, 100, baos);
            //byte[] data = baos.toByteArray();

            FileHelper.WriteByteToFile(qrCode, fileName);
        }

    }
}
