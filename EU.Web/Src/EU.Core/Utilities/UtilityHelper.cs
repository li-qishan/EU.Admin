using EU.Core.CacheManager;
using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using EU.Core.Extensions;
using System.Collections.Generic;
using System.Data;
using EU.Core.Module;
using EU.Domain.System;
using System.Linq;
using EU.Model.System;
using Newtonsoft.Json;
using EU.Core.UserManager;
using System.Threading.Tasks;

namespace EU.Core.Utilities
{
    /// <summary>
    /// 方法类
    /// </summary>
    public partial class Utility
    {
        #region  格式化object类型为字符串类型
        /// <summary>
        /// 格式化object类型为字符串类型，精确到天，如：2008/01/01
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToDayString(object dateTime)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dateTime)))
            {
                return "";
            }
            return ConvertToDayString(Convert.ToDateTime(dateTime));
        }
        #endregion

        #region 格式化DateTime类型为字符串类型
        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到天，如：2008/01/01
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToDayString(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dateTime)))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd");
        }
        #endregion

        /// <summary>
		/// 格式化DateTime类型为字符串类型，精确到分钟，如：2008/01/01 18:09
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ConvertToMiniuteString(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dateTime)))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd HH:mm");
        }

        /// <summary>
		/// 格式化object类型为字符串类型，精确到分钟，如：2008/01/01 18:09
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		public static string ConvertToMiniuteString(object dateTime)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dateTime)))
            {
                return "";
            }
            return ConvertToMiniuteString(Convert.ToDateTime(dateTime));
        }

        /// <summary>
        /// 格式化DateTime类型为字符串类型，精确到秒，如：2008/01/01 18:09:20
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToSecondString(DateTime dateTime)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dateTime)))
            {
                return "";
            }
            return dateTime.ToString(@"yyyy\/MM\/dd HH:mm:ss");
        }
        /// <summary>
        /// 格式化object类型为字符串类型，精确到秒，如：2008/01/01 18:09:20
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ConvertToSecondString(object dateTime)
        {
            if (string.IsNullOrEmpty(Convert.ToString(dateTime)))
            {
                return "";
            }
            return ConvertToSecondString(Convert.ToDateTime(dateTime));
        }

        #region 求系统唯一字符串
        /// <summary>
        /// 求系统唯一字符串，常用于ROW_ID值。
        /// </summary>
        /// <returns>字符串</returns>
        public static string GetSysID()
        {
            string sid = string.Empty;

            byte[] buffer = Guid.NewGuid().ToByteArray();
            sid = DateTime.Now.ToString("yyMMddHHmmss") + BitConverter.ToInt64(buffer, 0).ToString();
            return sid;
        }
        #endregion


        #region 时间戳Timestamp转换成日期 
        /// <summary>  
        /// 时间戳Timestamp转换成日期  
        /// </summary>  
        /// <param name="timeStamp"></param>  
        /// <returns></returns>  
        [Obsolete]
        public static DateTime ConvertTimeStampToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime targetDt = dtStart.Add(toNow);
            return dtStart.Add(toNow);
        }
        #endregion


        /// <summary>
        /// 求系统当前日期（数据库所在服务器的日期）。
        /// </summary>
        /// <returns></returns>
        public static DateTime GetSysDate()
        {
            DateTime time = DateTime.Now;
            return time;
        }
        public static DateTime GetSysDateTime()
        {
            //string empty = string.Empty;
            //empty = ((!IsOracle()) ? "SELECT GETDATE()" : "SELECT TO_DATE(TO_CHAR(SYSDATE,'YYYY/MM/DD HH24:MI:SS'),'YYYY/MM/DD HH24:MI:SS') FROM DUAL");
            //return (DateTime)DbAccess.ExecuteScalar(empty);
            return GetSysDate();
        }
        public static string GetSysDateTimeString()
        {
            return ConvertToSecondString(GetSysDateTime());
        }

        #region 清空Redis缓存
        /// <summary>
        /// 清空Redis缓存
        /// </summary>
        /// <param name="moduleCode"></param>
        public static void ClearCache()
        {
            try
            {
                RedisCacheService di = new RedisCacheService();
                di.Clear();
            }
            catch (Exception) { throw; }
        }
        #endregion

        #region 加解密
        /// <summary>
        /// 通过密钥将内容加密
        /// </summary>
        /// <param name="stringToEncrypt">要加密的字符串</param>
        /// <param name="encryptKey">加密密钥</param>
        /// <returns></returns>
        public static string EncryptString(string stringToEncrypt, string encryptKey)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
            {
                return "";
            }

            string stringEncrypted = string.Empty;
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(stringToEncrypt);
            MemoryStream encryptStream = new System.IO.MemoryStream();
            CryptoStream encStream = new CryptoStream(encryptStream, GenerateDESCryptoServiceProvider(encryptKey).CreateEncryptor(), CryptoStreamMode.Write);

            try
            {
                encStream.Write(bytIn, 0, bytIn.Length);
                encStream.FlushFinalBlock();
                stringEncrypted = Convert.ToBase64String(encryptStream.ToArray(), 0, (int)encryptStream.Length);
            }
            catch
            {
                return "";
            }
            finally
            {
                encryptStream.Close();
                encStream.Close();
            }

            return stringEncrypted;
        }

        /// <summary>
        /// 通过密钥讲内容解密
        /// </summary>
        /// <param name="stringToDecrypt">要解密的字符串</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns></returns>
        public static string DecryptString(string stringToDecrypt, string encryptKey)
        {
            if (String.IsNullOrEmpty(stringToDecrypt))
            {
                return "";
            }

            string stringDecrypted = string.Empty;
            byte[] bytIn = Convert.FromBase64String(stringToDecrypt.Replace(" ", "+"));
            MemoryStream decryptStream = new MemoryStream();
            CryptoStream encStream = new CryptoStream(decryptStream, GenerateDESCryptoServiceProvider(encryptKey).CreateDecryptor(), CryptoStreamMode.Write);

            try
            {
                encStream.Write(bytIn, 0, bytIn.Length);
                encStream.FlushFinalBlock();
                stringDecrypted = Encoding.Default.GetString(decryptStream.ToArray());
            }
            catch
            {
                return "";
            }
            finally
            {
                decryptStream.Close();
                encStream.Close();
            }

            return stringDecrypted;
        }

        private static DESCryptoServiceProvider GenerateDESCryptoServiceProvider(string key)
        {
            DESCryptoServiceProvider dCrypter = new DESCryptoServiceProvider();

            string sTemp;
            if (dCrypter.LegalKeySizes.Length > 0)
            {
                int moreSize = dCrypter.LegalKeySizes[0].MinSize;
                while (key.Length > 8)
                {
                    key = key.Substring(0, 8);
                }
                sTemp = key.PadRight(moreSize / 8, ' ');
            }
            else
            {
                sTemp = key;
            }
            byte[] bytKey = UTF8Encoding.UTF8.GetBytes(sTemp);

            dCrypter.Key = bytKey;
            dCrypter.IV = bytKey;

            return dCrypter;
        }

        #endregion

        #region MD5加密
        /// <summary>
        /// 字符串MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回大写32位MD5值</returns>
        public static string GetMD5String(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);

            return MD5String(buffer);
        }

        /// <summary>
        /// 流MD5加密
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetMD5Stream(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return MD5String(buffer);
        }

        /// <summary>
        /// 文件MD5加密
        /// </summary>
        /// <param name="path"></param>
        /// <returns>返回大写32位MD5值</returns>
        public static string GetMD5File(string path)
        {
            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return GetMD5Stream(fs);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        private static string MD5String(byte[] buffer)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] cryptBuffer = md5.ComputeHash(buffer);
            StringBuilder sb = new StringBuilder();
            foreach (byte item in cryptBuffer)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
        }
        #endregion

        #region 获得当前公司ID
        /// <summary>
        /// 获得当前公司ID
        /// </summary>
        /// <returns></returns>
        public static string GetCompanyId()
        {
            try
            {
                return GetCompanyGuidId().ToString();
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }
        /// <summary>
        /// 获得当前公司ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetCompanyGuidId()
        {
            try
            {
                return Guid.Parse("e26f359a-4983-42d8-8769-19ddec5b7d23");
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }
        #endregion

        #region 获得当前用户ID
        /// <summary>
        /// 获得当前用户ID
        /// </summary>
        /// <returns></returns>
        public static string GetUserIdString()
        {
            try
            {
                var userId = GetUserId();
                if (userId is null)
                    return null;
                return userId.ToString();
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }
        /// <summary>
        /// 获得当前用户ID
        /// </summary>
        /// <returns></returns>
        public static Guid? GetUserId()
        {
            try
            {
                return UserContext.Current.User_Id;
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }
        #endregion

        #region 获得当前集团ID
        /// <summary>
        /// 获得当前集团ID
        /// </summary>
        /// <returns></returns>
        public static string GetGroupId()
        {
            try
            {
                return GetGroupGuidId().ToString();
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }

        /// <summary>
        /// 获得当前集团ID
        /// </summary>
        /// <returns></returns>
        public static Guid GetGroupGuidId()
        {
            try
            {
                return Guid.Parse("e26f359a-4983-42d8-8769-19ddec5b7d23");
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
        }
        #endregion

        #region 记录登录日志
        /// <summary>
        /// 记录登录日志
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="loginClass">登录类型</param>
        /// <param name="remark">备注</param>
        /// <param name="companyId">公司id</param>
        public static void RecordEntryLog(Guid userId, string loginClass, string remark = null, string companyId = null)
        {
            try
            {

                Task.Factory.StartNew(() =>
                {

                    string ipAddress = string.Empty;
                    string countryName = string.Empty;
                    string cityName = string.Empty;
                    string clientType = string.Empty;
                    string os = string.Empty;
                    if (string.IsNullOrEmpty(companyId))
                        companyId = GetCompanyId();

                    ipAddress = HttpContextExtension.GetUserIp(HttpContext.Current);

                    #region 求IP地址归属地
                    //var userAgent = HttpContext.Current.Request.Headers["User-Agent"];
                    //string uaString = Convert.ToString(userAgent[0]);
                    //var uaParser = Parser.GetDefault();
                    //ClientInfo c = uaParser.Parse(uaString);
                    //os = c.OS.Family + c.OS.Major;
                    //clientType = c.UA.Family + c.UA.Major;
                    //if (clientType == "Web")
                    //{

                    //}
                    #endregion

                    DbInsert di = new DbInsert("SmEntryLog");
                    di.Values("LoginUserId", userId.ToString());
                    di.Values("IpAddress", ipAddress);
                    di.Values("IpAddressName1", countryName);
                    di.Values("IpAddressName2", cityName);
                    di.Values("LoginDate", Utility.GetSysDate());
                    di.Values("LoginClass", loginClass);
                    di.Values("OSName", os);
                    di.Values("ClientType", clientType);
                    di.Values("Remark", remark);
                    DBHelper.Instance.ExcuteNonQuery(di.GetSql());

                });

                //DbUpdate du = new DbUpdate("SM_USER", "ROW_ID", userId);
                //du.IsInitDefaultValue = false;
                //du.SetCompute("LOGIN_TIMES_STAT", "LOGIN_TIMES_STAT+1");
                //DBHelper.Instance.ExcuteNonQuery(du.GetSql());
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region 字典转String
        /// <summary>
        /// 字典转String
        /// </summary>
        /// <param name="dic">字典</param>
        /// <returns></returns>
        public static string ConvertDictionaryToString(IDictionary<string, object> dic)
        {
            if (dic == null || dic.Keys.Count <= 0)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (KeyValuePair<string, object> item in dic)
                {
                    if (item.Value == null)
                        continue;
                    sb.Append($"{item.Key}:{(item.Value.ToString().StartsWith("{") ? item.Value.ToString() : Newtonsoft.Json.JsonConvert.SerializeObject(item.Value))};");
                }
            }
            catch { }
            return sb.ToString();
        }
        #endregion

        #region DataTable转Tree
        /// <summary>
        /// DataTable转Tree
        /// </summary>
        /// <param name="moduleCode">模块代码</param>
        /// <param name="userId">用户ID</param>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        public static DataTable FormatDataTableForTree(string moduleCode, string userId, DataTable dt)
        {
            ModuleSqlColumn moduleColumnInfo = new ModuleSqlColumn(moduleCode);
            List<SmModuleColumn> dvModuleColumns = moduleColumnInfo.GetModuleSqlColumn();

            List<SmModuleColumn> List = dvModuleColumns.Where(x => x.dataIndex == "ID").ToList();

            if (List.Count == 0)
            {
                SmModuleColumn SmModuleColumn = new SmModuleColumn();
                SmModuleColumn.dataIndex = "ID";
                dvModuleColumns.Add(SmModuleColumn);
            }

            string columnName = string.Empty;
            string valueType = string.Empty;
            string dateFormat = string.Empty;
            string value = string.Empty;
            bool IsBool = false;

            DataTable dtTree = new DataTable();
            DataRow drTree = null;

            if (dvModuleColumns != null && dvModuleColumns.Count > 0)
                for (int i = 0; i < dvModuleColumns.Count; i++)
                {
                    columnName = dvModuleColumns[i].dataIndex;
                    dtTree.Columns.Add(columnName, typeof(string));
                }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                drTree = dtTree.NewRow();
                if (dvModuleColumns != null && dvModuleColumns.Count > 0)
                {
                    for (int j = 0; j < dvModuleColumns.Count; j++)
                    {
                        columnName = dvModuleColumns[j].dataIndex;
                        valueType = dvModuleColumns[j].valueType;
                        dateFormat = dvModuleColumns[j].DataFormate;
                        value = dt.Rows[i][columnName].ToString();
                        IsBool = dvModuleColumns[j].IsBool;
                        if ((valueType == "date" || valueType == "dateTime" || valueType == "time") && !string.IsNullOrEmpty(dateFormat))
                        {
                            switch (dateFormat)
                            {
                                case "Y/m":
                                    value = DateTimeHelper.ConvertToYearMonthString(value);
                                    break;
                                case "Y-m":
                                    value = DateTimeHelper.ConvertToYearMonthString1(value);
                                    break;
                                case "Y/m/d":
                                    value = DateTimeHelper.ConvertToDayString(value);
                                    break;
                                case "Y/m/d H":
                                    value = DateTimeHelper.ConvertToHourString(value);
                                    break;
                                case "Y/m/d H:i":
                                    value = DateTimeHelper.ConvertToMiniuteString(value);
                                    break;
                                case "Y/m/d H:i:s":
                                    value = DateTimeHelper.ConvertToSecondString(value);
                                    break;
                                case "H:i":
                                    value = DateTimeHelper.ConvertToOnlyHourMinuteString(value);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else if (valueType == "digit" && !string.IsNullOrEmpty(dateFormat))
                        {
                            if (string.IsNullOrEmpty(dateFormat) || dateFormat == "-1")
                            {
                                value = StringHelper.TrimDecimalString(value, -1);
                            }
                            else
                            {
                                value = StringHelper.TrimDecimalString(value, Convert.ToInt32(dateFormat));
                            }
                        }
                        if (IsBool)
                        {
                            if (value == "True")
                                drTree[columnName] = "true";
                            else
                                drTree[columnName] = "false";
                        }
                        else
                            drTree[columnName] = value;
                    }
                }
                dtTree.Rows.Add(drTree);
            }

            #region 处理合计
            SmModule module = ModuleInfo.GetModuleInfo(moduleCode);
            if (module.IsSum)
            {
                List<SmModuleColumn> sumColumns = dvModuleColumns.Where(o => o.IsSum == true && (o.valueType == "digit" || o.valueType == "money")).ToList();
                if (!sumColumns.IsNullOrEmpty() && sumColumns.Any())
                {
                    drTree = dtTree.NewRow();
                    drTree["ID"] = "2108131534125450674040828132228";

                    for (int j = 0; j < sumColumns.Count; j++)
                    {
                        decimal sum = 0;
                        columnName = sumColumns[j].dataIndex;
                        valueType = sumColumns[j].valueType;
                        dateFormat = sumColumns[j].DataFormate;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            value = dt.Rows[i][columnName].ToString();
                            if (!string.IsNullOrEmpty(value))
                            {
                                if (string.IsNullOrEmpty(dateFormat) || dateFormat == "-1")
                                    value = StringHelper.TrimDecimalString(value, -1);
                                else
                                    value = StringHelper.TrimDecimalString(value, Convert.ToInt32(dateFormat));
                                sum += Convert.ToDecimal(value);
                            }
                        }
                        drTree[columnName] = sum;
                    }
                    dtTree.Rows.Add(drTree);
                }
            }
            #endregion

            return dtTree;
        }
        #endregion

        #region 最小时间
        public static string MinDateString
        {
            get
            {
                return "1900/01/01";
            }
        }
        /// <summary>
        /// 最小时间（0001/01/01）。
        /// </summary>
        public static DateTime MinDate
        {
            get
            {
                return Convert.ToDateTime(MinDateString);
            }
        }
        #endregion

        #region 自动产生序列号(不一定是连续的，但永远不会重复)
        /// <summary>
        /// 自动产生序列号(不一定是连续的，但永远不会重复)
        /// </summary>
        /// <param name="sequenceCode">规则代码</param>
        /// <param name="trans"></param>
        /// <returns></returns>
        //public static string GenerateSequence(string sequenceCode, bool trans = false)
        //{
        //    try
        //    {
        //        #region 变量定义
        //        string result = string.Empty;
        //        string prefix = string.Empty;
        //        int length = 0;
        //        int numberLength = 0;
        //        int prefixLength = 0;
        //        #endregion

        //        DbSelect dsSequenceSetup = new DbSelect("SmAutoCode A", "A");
        //        dsSequenceSetup.IsInitDefaultValue = false;
        //        dsSequenceSetup.Select("A.Prefix,A.NumberLength,A.TableName,A.ColumnName");
        //        dsSequenceSetup.Where("A.NumberCode", "=", sequenceCode);
        //        DataTable dtSequenceSetup = DBHelper.Instance.GetDataTable(dsSequenceSetup.GetSql());
        //        if (dtSequenceSetup.Rows.Count > 0)
        //        {
        //            prefix = dtSequenceSetup.Rows[0]["PREFIX"].ToString();
        //            if (!string.IsNullOrEmpty(prefix))
        //            {
        //                prefixLength = prefix.Length;
        //            }
        //            length = Convert.ToInt32(dtSequenceSetup.Rows[0]["NumberLength"]);
        //            string tableCode = dtSequenceSetup.Rows[0]["TableName"].ToString();
        //            string columnCode = dtSequenceSetup.Rows[0]["ColumnName"].ToString();
        //            numberLength = length - prefix.Length;

        //            var param = new DynamicParameters();
        //            param.Add("@tableCode", tableCode);
        //            param.Add("@columnCode", columnCode);
        //            param.Add("@value", 32);
        //            //var param = new
        //            //{
        //            //    tableCode = tableCode,
        //            //    columnCode = columnCode
        //            //};
        //            string maxSequence1 = (string)DBHelper.Instance.ExecuteScalar("p_get_seq", param, CommandType.StoredProcedure, trans);
        //            string maxSequence = param.Get<string>("@value");
        //            //StoredProcedure store = DbAccess.GetStoredProcedure("p_get_seq");
        //            //store.AddInParameter("tableCode", DbType.String, tableCode);
        //            //store.AddInParameter("columnCode", DbType.String, columnCode);
        //            //store.AddOutParameter("value", DbType.String, 32);
        //            //DbAccess.ExecuteStoredProcedure(store, trans);
        //            //string maxSequence = (string)store.GetParameterValue("@value");

        //            //if (string.IsNullOrEmpty(maxSequence))
        //            //{
        //            //    result = prefix + Convert.ToString(1).PadLeft(numberLength, '0');
        //            //}
        //            //else
        //            //{
        //            //    result = prefix + maxSequence.PadLeft(numberLength, '0');
        //            //}
        //        }
        //        return result;
        //    }
        //    catch (Exception E)
        //    {
        //        throw E;
        //    }
        //}

        /// <summary>
        /// 自动产生连续的序列号（使用此函数时，一定要把存放此Sequence的列设为Unique）
        /// </summary>
        /// <param name="sequenceCode">规则代码</param>
        /// <returns>新的序列号</returns>
        public static string GenerateContinuousSequence(string sequenceCode, bool trans = false)
        {
            try
            {
                string result = GenerateContinuousSequence(sequenceCode, "", trans);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 自动产生连续的序列号（使用此函数时，一定要把存放此Sequence的列设为Unique）
        /// </summary>
        /// <param name="sequenceCode">规则代码</param>
        /// <param name="prefix">代码前缀</param>
        /// <returns>新的序列号</returns>
        public static string GenerateContinuousSequence(string sequenceCode, string prefix, bool trans = false)
        {
            try
            {
                #region 变量定义
                string result = string.Empty;
                string prefixTemp = string.Empty;
                int length = 0;     //设定字符串长度
                int tempLength = 0; //设定字符串临时长度
                //int numberLength = 0;
                //int prefixLength = 0;
                string tableCode = string.Empty;
                string columnCode = string.Empty;
                string dataFormatType = string.Empty;
                string dateString = string.Empty;
                int sequence;
                #endregion

                DbSelect dsSequenceSetup = new DbSelect("SmAutoCode A", "A");
                dsSequenceSetup.IsInitDefaultValue = false;
                dsSequenceSetup.Select("A.Prefix,A.NumberLength,A.TableName,A.ColumnName,A.DateFormatType");
                dsSequenceSetup.Where("A.NumberCode", "=", sequenceCode);
                DataTable dtSequenceSetup = DBHelper.Instance.GetDataTable(dsSequenceSetup.GetSql());
                if (dtSequenceSetup.Rows.Count > 0)
                {
                    //设定字符串长度
                    length = Convert.ToInt32(dtSequenceSetup.Rows[0]["NumberLength"]);

                    #region 字符前添加固定字符
                    prefixTemp = dtSequenceSetup.Rows[0]["Prefix"].ToString();
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        prefixTemp = prefix + prefixTemp;
                        length = length + prefix.Length;
                    }
                    tempLength = length;
                    if (!string.IsNullOrEmpty(prefixTemp))
                    {
                        tempLength = tempLength - prefixTemp.Length;
                        result = prefixTemp;
                    }
                    #endregion

                    #region 增长日期格式
                    dataFormatType = dtSequenceSetup.Rows[0]["DateFormatType"].ToString();
                    if (!string.IsNullOrEmpty(dataFormatType))
                    {
                        if (dataFormatType == "YYYYMMDDHHMM")
                            dateString = DateTime.Now.ToString("yyyyMMddhhmm");
                        else if (dataFormatType == "YYYYMMDDHH")
                            dateString = DateTime.Now.ToString("yyyyMMddhh");
                        else if (dataFormatType == "YYYYMMDD")
                            dateString = DateTime.Now.ToString("yyyyMMdd");
                        else if (dataFormatType == "YYYYMM")
                            dateString = DateTime.Now.ToString("yyyyMM");
                        else if (dataFormatType == "YYYY")
                            dateString = DateTime.Now.ToString("yyyy");
                    }
                    result += dateString;
                    tempLength = tempLength - dateString.Length;
                    #endregion

                    tableCode = dtSequenceSetup.Rows[0]["TableName"].ToString();
                    columnCode = dtSequenceSetup.Rows[0]["ColumnName"].ToString();
                    #region 查询
                    DbSelect dbSelect = new DbSelect(tableCode + " A", "A", null);
                    dbSelect.IsInitDefaultValue = false;
                    //if (string.IsNullOrEmpty(dateString))
                    //{
                    //    dbSelect.Select("MAX(A." + columnCode + ")");
                    //    //dbSelect.Select("MAX(CONVERT(DECIMAL,SUBSTRING(A.ISSUE_NO," + (prefix.Length + dateString.Length + 1).ToString() + "," + tempLength.ToString() + ")))");
                    //}
                    //else
                    //{
                    if (!string.IsNullOrEmpty(prefixTemp) || !string.IsNullOrEmpty(dateString))
                        dbSelect.Select("MAX(SUBSTRING(A." + columnCode + "," + (prefixTemp.Length + dateString.Length + 1).ToString() + "," + tempLength.ToString() + "))");
                    else
                        dbSelect.Select("MAX(A." + columnCode + ")");
                    //}
                    //dbSelect.Select("MAX(CONVERT(DECIMAL,SUBSTRING(A.ISSUE_NO," + (prefix.Length + dateString.Length + 1).ToString() + "," + tempLength.ToString() + ")))");
                    if (!string.IsNullOrEmpty(prefixTemp) || !string.IsNullOrEmpty(dateString))
                        dbSelect.Where("SUBSTRING(A." + columnCode + ",1," + (prefixTemp.Length + dateString.Length).ToString() + ")", " = ", prefixTemp + dateString);
                    dbSelect.Where("LEN(A." + columnCode + ")", "=", length);
                    string maxSequence = Convert.ToString(DBHelper.Instance.ExecuteScalar(dbSelect.GetSql(), null, null, trans));
                    #endregion
                    //tempLength = tempLength - dateString.Length;
                    if (string.IsNullOrEmpty(maxSequence))
                        result = prefixTemp + dateString + Convert.ToString(1).PadLeft(tempLength, '0');
                    else
                    {
                        if (!string.IsNullOrEmpty(prefixTemp) || !string.IsNullOrEmpty(dateString))
                        {
                            if (int.TryParse(maxSequence, out sequence))
                            {
                                sequence += 1;
                                if (sequence.ToString().Length > tempLength)
                                    throw new Exception("自动生成字串长度已经超过设定长度!");
                            }
                            else
                                throw new Exception("表中的数据无法进行自动编号,请联系软件开发商!");
                            result = prefixTemp + dateString + sequence.ToString().PadLeft(tempLength, '0');
                        }
                        else
                        {
                            if (int.TryParse(maxSequence, out sequence))
                            {
                                sequence += 1;
                                if (sequence.ToString().Length > length)
                                    throw new Exception("自动生成字串长度已经超过设定长度!");
                            }
                            else
                                throw new Exception("表中的数据无法进行自动编号,请联系软件开发商!");
                            result = sequence.ToString().PadLeft(length, '0');
                        }
                    }
                }
                else
                    throw new Exception("自动编号代码：" + sequenceCode + "没有设置！");
                return result;
            }
            catch (Exception) { throw; }
        }

        public static string GenerateContinuousSequence(string tableCode, string columnCode, string prefix, int length, bool trans = false)
        {
            try
            {
                #region 变量定义
                string result = string.Empty;
                int tempLength = 0; //设定字符串临时长度
                int sequence;
                #endregion
                tempLength = length - prefix.Length;
                DbSelect dbSelect = new DbSelect(tableCode + " A", "A", null);
                dbSelect.IsInitDefaultValue = false;
                if (!string.IsNullOrEmpty(prefix))
                    dbSelect.Select("MAX(SUBSTRING(A." + columnCode + "," + (prefix.Length + 1).ToString() + "," + tempLength.ToString() + "))");
                else
                    dbSelect.Select("MAX(A." + columnCode + ")");
                if (!string.IsNullOrEmpty(prefix))
                    dbSelect.Where("SUBSTRING(A." + columnCode + ",1," + (prefix.Length).ToString() + ")", " = ", prefix);
                dbSelect.Where("LEN(A." + columnCode + ")", "=", length);
                string maxSequence = Convert.ToString(DBHelper.Instance.ExecuteScalar(dbSelect.GetSql(), trans));
                if (string.IsNullOrEmpty(maxSequence))
                    result = prefix + Convert.ToString(1).PadLeft(tempLength, '0');
                else
                {
                    if (!string.IsNullOrEmpty(prefix))
                    {
                        if (int.TryParse(maxSequence, out sequence))
                        {
                            sequence += 1;
                            if (sequence.ToString().Length > tempLength)
                                throw new Exception("自动生成字串长度已经超过设定长度!");
                        }
                        else
                            throw new Exception("表中的数据无法进行自动编号,请联系软件开发商!");
                        result = prefix + sequence.ToString().PadLeft(tempLength, '0');
                    }
                    else
                    {
                        if (int.TryParse(maxSequence, out sequence))
                        {
                            sequence += 1;
                            if (sequence.ToString().Length > length)
                                throw new Exception("自动生成字串长度已经超过设定长度!");
                        }
                        else
                            throw new Exception("表中的数据无法进行自动编号,请联系软件开发商!");
                        result = sequence.ToString().PadLeft(length, '0');
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }

        public static int GenerateContinuousSequence(string tableCode, string columnCode, string fieldName = null, string fieldValue = null, bool trans = false)
        {
            try
            {
                #region 变量定义
                int sequence = 0;
                #endregion
                DbSelect dbSelect = new DbSelect(tableCode + " A", "A", null);
                dbSelect.IsInitDefaultValue = false;
                dbSelect.Select("MAX(A." + columnCode + ")");
                if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(fieldValue))
                    dbSelect.Where(fieldName, " = ", fieldValue);
                dbSelect.Where("IsDeleted", " = ", false);

                string maxSequence = Convert.ToString(DBHelper.Instance.ExecuteScalar(dbSelect.GetSql(), trans));
                if (string.IsNullOrEmpty(maxSequence))
                    maxSequence = "0";
                if (int.TryParse(maxSequence, out sequence))
                    sequence += 1;
                else
                    throw new Exception("表中的数据无法进行自动编号,请联系软件开发商!");
                return sequence;
            }
            catch (Exception) { throw; }
        }

        #endregion

        #region 根据身份证号获取年龄，性别
        /// <summary>
        /// 通过身份证号获取出生日期
        /// </summary>
        /// <param name="idNumber"></param>
        public static DateTime? GetBirthdateByIdNumber(string idNumber)
        {
            string date = "";
            if (!string.IsNullOrEmpty(idNumber))
            {
                //必须是15位或18位的身份证号码
                if (idNumber.Length != 15 && idNumber.Length != 18)
                {
                    if (idNumber.Length == 18)
                    {
                        date = idNumber.Substring(6, 4) + "-" + idNumber.Substring(10, 2) + "-" + idNumber.Substring(12, 2);
                    }
                    if (idNumber.Length == 15)
                    {
                        date = "19" + idNumber.Substring(6, 2) + "-" + idNumber.Substring(8, 2) + "-" + idNumber.Substring(10, 2);
                    }
                }
            }
            if (DateTime.TryParse(date, out DateTime tDateTime))
            {
                return tDateTime;
            }
            return default;

        }
        /// <summary>
        /// 根据身份证号获取性别
        /// </summary>
        /// <param name="idNumber">身份证号</param>
        /// <returns></returns>
        public static string GetGenderByIdNumber(string idNumber)
        {
            string gender = string.Empty;
            if (idNumber.Length == 15)
            {
                gender = idNumber.Substring(12, 3);
            }
            else if (idNumber.Length == 18)
            {
                gender = idNumber.Substring(14, 3);
            }
            int.TryParse(gender, out int temp);
            if (temp % 2 == 0)
                return "W";

            return "M";
        }
        #endregion

        #region 检查端口开放情况
        /// <summary>
        /// 检查端口是否打开
        /// </summary>
        /// <param name="host">地址</param>
        /// <param name="port">端口</param>
        /// <param name="timeout">超时</param>
        /// <returns></returns>
        public static bool IsPortOpen(string host, int port, TimeSpan? timeout = null)
        {
            try
            {
                if (timeout == null)
                    timeout = TimeSpan.FromSeconds(3.0);
                using var client = new System.Net.Sockets.TcpClient();
                var result = client.BeginConnect(host, port, null, null);
                var success = result.AsyncWaitHandle.WaitOne(timeout.Value);
                client.EndConnect(result);
                return success;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取分页数据起始
        /// <summary>
        /// 获取分页数据起始
        /// </summary>
        /// <param name="paramData"></param>
        /// <param name="current">当前页码</param>
        /// <param name="pageSize">一个多个数据</param>
        public static void GetPageData(string paramData, out int current, out int pageSize)
        {
            var searchParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
            current = 1;
            pageSize = 10000;
            foreach (var item in searchParam)
            {
                if (item.Key == "current")
                {
                    current = int.Parse(item.Value.ToString());
                    continue;
                }

                if (item.Key == "pageSize")
                {
                    pageSize = int.Parse(item.Value.ToString());
                    continue;
                }
            }
        }
        #endregion

        #region 计算分页起始索引
        /// <summary>
        /// 计算分页起始索引
        /// </summary>
        /// <param name="paramData"></param>
        /// <param name="current">计算分页起始索引</param>
        /// <param name="pageSize">计算分页结束索引</param>
        public static void GetPageIndex(string paramData, out int startIndex, out int endIndex)
        {
            var searchParam = JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
            int current = 1;
            int pageSize = 10000;
            foreach (var item in searchParam)
            {
                if (item.Key == "current")
                {
                    current = int.Parse(item.Value.ToString());
                    continue;
                }

                if (item.Key == "pageSize")
                {
                    pageSize = int.Parse(item.Value.ToString());
                    continue;
                }
            }

            int _pageSize = pageSize;
            //计算分页起始索引
            startIndex = current > 1 ? (current - 1) * _pageSize : 0;

            //计算分页结束索引
            endIndex = current * _pageSize;
        }
        #endregion

        #region 字符串获取类型
        /// <summary>
        /// 根据 <paramref name="name"/> 获取 <see cref="Type"/> 类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type StringConvertToType(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return default;
            }

            switch (name.Trim().ToUpper())
            {
                case "INT":
                case "INT32":
                    return typeof(int);
                case "INT?":
                case "INT32?":
                    return typeof(int?);
                case "INT[]":
                case "INT32[]":
                    return typeof(int[]);
                case "INT?[]":
                case "INT32?[]":
                    return typeof(int?[]);
                case "LIST<INT>":
                case "LIST<INT32>":
                    return typeof(List<int>);
                case "LIST<INT?>":
                case "LIST<INT32?>":
                    return typeof(List<int?>);

                case "LONG":
                case "INT64":
                    return typeof(long);
                case "LONG?":
                case "INT64?":
                    return typeof(long?);
                case "LONG[]":
                case "INT64[]":
                    return typeof(long[]);
                case "LONG?[]":
                case "INT64?[]":
                    return typeof(long?[]);
                case "LIST<LONG>":
                case "LIST<INT64>":
                    return typeof(List<long>);
                case "LIST<LONG?>":
                case "LIST<INT64?>":
                    return typeof(List<long?>);

                case "FLOAT":
                case "SINGLE":
                    return typeof(float);
                case "FLOAT?":
                case "SINGLE?":
                    return typeof(float?);
                case "FLOAT[]":
                case "SINGLE[]":
                    return typeof(float[]);
                case "FLOAT?[]":
                case "SINGLE?[]":
                    return typeof(float?[]);
                case "LIST<FLOAT>":
                case "LIST<SINGLE>":
                    return typeof(List<float>);
                case "LIST<FLOAT?>":
                case "LIST<SINGLE?>":
                    return typeof(List<float?>);

                case "DOUBLE":
                    return typeof(double);
                case "DOUBLE?":
                    return typeof(double?);
                case "DOUBLE[]":
                    return typeof(double[]);
                case "DOUBLE?[]":
                    return typeof(double?[]);
                case "LIST<DOUBLE>":
                    return typeof(List<double>);
                case "LIST<DOUBLE?>":
                    return typeof(List<double?>);

                case "DECIMAL":
                    return typeof(decimal);
                case "DECIMAL?":
                    return typeof(decimal?);
                case "DECIMAL[]":
                    return typeof(decimal[]);
                case "DECIMAL?[]":
                    return typeof(decimal?[]);
                case "LIST<DECIMAL>":
                    return typeof(List<decimal>);
                case "LIST<DECIMAL?>":
                    return typeof(List<decimal?>);

                case "DATETIME":
                    return typeof(DateTime);
                case "DATETIME?":
                    return typeof(DateTime?);
                case "DATETIME[]":
                    return typeof(DateTime[]);
                case "DATETIME?[]":
                    return typeof(DateTime?[]);
                case "LIST<DATETIME>":
                    return typeof(List<DateTime>);
                case "LIST<DATETIME?>":
                    return typeof(List<DateTime?>);

                case "GUID":
                    return typeof(Guid);
                case "GUID?":
                    return typeof(Guid?);
                case "GUID[]":
                    return typeof(Guid[]);
                case "GUID?[]":
                    return typeof(Guid?[]);
                case "LIST<GUID>":
                    return typeof(List<Guid>);
                case "LIST<GUID?>":
                    return typeof(List<Guid?>);

                case "BOOL":
                    return typeof(bool);
                case "BOOL?":
                    return typeof(bool?);
                case "BOOL[]":
                    return typeof(bool[]);
                case "BOOL?[]":
                    return typeof(bool?[]);
                case "LIST<BOOL>":
                    return typeof(List<bool>);
                case "LIST<BOOL?>":
                    return typeof(List<bool?>);

                case "STRING":
                    return typeof(string);
                case "STRING[]":
                    return typeof(string[]);
                case "LIST<STRING>":
                    return typeof(List<string>);

                default:
                    break;
            }

            return Type.GetType(name);
        }
        #endregion
    }
}