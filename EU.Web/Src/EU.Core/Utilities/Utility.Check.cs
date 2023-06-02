using EU.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace EU.Core.Utilities
{
    public partial class Utility
    {
        /// <summary>
        /// 检查字符串是否为日期格式
        /// </summary>
        /// <param name="date"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsDateTime(string date, out DateTime result)
        {
            bool flag = true;
            if (!string.IsNullOrEmpty(date))
            {
                flag = DateTime.TryParse(date, out result);
            }
            else
            {
                result = Utility.MinDate;
            }
            return flag;
        }

        /// <summary>
        /// 检查字符串是否为日期格式
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDateTime(string date)
        {
            DateTime result;
            bool flag = true;
            if (!string.IsNullOrEmpty(date))
            {
                flag = DateTime.TryParse(date, out result);
            }
            return flag;
        }
        /// <summary>
        /// 检查字符串中的日期是否符合设定的格式
        /// </summary>
        /// <param name="date">date数据</param>
        /// <param name="dateFormat">YYYY/MM/DD或YYYY/MM/DD HH24:MI</param>
        /// <returns></returns>
        public static bool IsDateTimeFormat(string date, string dateFormat)
        {
            try
            {
                if (!string.IsNullOrEmpty(date))
                {
                    #region 变量定义
                    int position = -1;
                    int outInt = 0;
                    string content = string.Empty;
                    int year = -1;
                    int month = -1;
                    int day = -1;
                    int hour = -1;
                    dateFormat = dateFormat.ToUpper();
                    bool result = false;
                    #endregion
                    #region 判断长度
                    if (dateFormat == "YYYY/MM/DD HH24:MI")
                    {
                        if (date.Length != dateFormat.Length - 2)
                        {
                            return result;
                        }
                    }
                    else
                    {
                        if (date.Length != dateFormat.Length)
                        {
                            return result;
                        }
                    }
                    if (date.IndexOf("-") > -1)
                    {
                        return result;
                    }
                    #endregion
                    #region 判断年
                    position = dateFormat.IndexOf("YYYY");
                    content = date.Substring(position, 4);
                    if (!string.IsNullOrEmpty(content))
                    {
                        if (int.TryParse(content, out outInt) == false)
                        {
                            return result;
                        }
                        else
                        {
                            year = Convert.ToInt32(content);
                        }
                    }
                    #endregion
                    #region 判断月
                    position = dateFormat.IndexOf("MM");
                    content = date.Substring(position, 2);
                    if (!string.IsNullOrEmpty(content))
                    {
                        if (int.TryParse(content, out outInt))
                        {
                            if (Convert.ToInt32(content) >= 1 && Convert.ToInt32(content) <= 12)
                            {
                                month = Convert.ToInt32(content);
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    #endregion 
                    #region 判断日
                    position = dateFormat.IndexOf("DD");
                    content = date.Substring(position, 2);
                    if (!string.IsNullOrEmpty(content))
                    {
                        if (int.TryParse(content, out outInt))
                        {
                            if (Convert.ToInt32(content) >= 1 && Convert.ToInt32(content) <= DateTime.DaysInMonth(year, month))
                            {
                                day = Convert.ToInt32(content);
                            }
                            else
                            {
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    #endregion
                    #region 判断时分
                    if (dateFormat == "YYYY/MM/DD HH24:MI")
                    {
                        position = dateFormat.IndexOf("HH24");
                        content = date.Substring(position, 2);
                        if (!string.IsNullOrEmpty(content))
                        {
                            if (int.TryParse(content, out outInt))
                            {
                                if (Convert.ToInt32(content) >= 1 && Convert.ToInt32(content) <= 24)
                                {
                                    hour = Convert.ToInt32(content);
                                }
                                else
                                {
                                    return result;
                                }
                            }
                            else
                            {
                                return result;
                            }
                        }

                        position = dateFormat.IndexOf("MI") - 2;
                        content = date.Substring(position, 2);
                        if (!string.IsNullOrEmpty(content))
                        {
                            if (int.TryParse(content, out outInt))
                            {
                                if (Convert.ToInt32(content) >= 0 && Convert.ToInt32(content) <= 60)
                                {
                                    hour = Convert.ToInt32(content);
                                }
                                else
                                {
                                    return result;
                                }
                            }
                            else
                            {
                                return result;
                            }
                        }
                    }
                    #endregion
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 检查字符串是否为整型
        /// </summary>
        /// <param name="stringInt"></param>
        /// <returns></returns>
        public static bool IsInt(string stringInt)
        {
            int result;
            bool flag = true;
            if (!string.IsNullOrEmpty(stringInt))
            {
                flag = int.TryParse(stringInt, out result);
            }
            return flag;
        }

        /// <summary>
        /// 检查字符串是否为数字类型
        /// </summary>
        /// <param name="stringNumber"></param>
        /// <returns></returns>
        public static bool IsNumber(string stringNumber)
        {
            decimal result;
            bool flag = true;
            if (!string.IsNullOrEmpty(stringNumber))
            {
                flag = decimal.TryParse(stringNumber, out result);
            }
            return flag;
        }

        /// <summary>
        /// 判断输入日期是否小于系统日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsLessSysDate(string date)
        {
            try
            {
                bool result = false;
                if (IsDateTime(date))
                {
                    if (Convert.ToDateTime(date) < DateTime.Now)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }
        /// <summary>
        /// 判断输入日期是否小于等于系统日期
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsLessEqualSysDate(string date)
        {
            try
            {
                bool result = false;
                if (IsDateTime(date))
                {
                    if (Convert.ToDateTime(date) <= DateTime.Now)
                    {
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// 判断输入字符是否为正确的邮件地址
        /// </summary>
        /// <param name="emailAddress">邮件地址</param>
        /// <returns></returns>
        public static bool IsEmailAddress(string emailAddress)
        {
            try
            {
                string emailPattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                bool match = Regex.IsMatch(emailAddress, emailPattern);

                return match;
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
        /// <param name="modifyType">ModifyType.Add,ModifyType.Edit</param>
        /// <param name="rowid">ModifyType.Edit时修改记录的ROW_ID值</param>
        /// <param name="promptName">判断栏位的提示名称</param>
        public static void CheckCodeExist(string companyId, string tableName, string fieldName, string fieldValue, ModifyType modifyType, string rowid, string promptName)
        {
            try
            {
                CheckCodeExist(companyId, tableName, fieldName, fieldValue, modifyType, rowid, promptName, null);
            }
            catch (Exception)
            {
                throw;
            }
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
        public static bool CheckCodeExist(string companyId, string tableName, string fieldName, string fieldValue, ModifyType modifyType, string rowid, string promptName, string whereCondition)
        {
            try
            {
                bool result = false;
                if (modifyType == ModifyType.Add)
                {
                    string sql = string.Empty;
                    if (string.IsNullOrEmpty(companyId))
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND IsDeleted='false' ";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND CompanyId='" + companyId + "' AND IsDeleted='false' ";
                    }
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        sql += " AND " + whereCondition;
                    }
                    int count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(sql));
                    if (count > 0)
                    {
                        result = true;
                        throw new Exception(string.Format("{0}【{1}】已经存在！", promptName, fieldValue));
                    }
                    else
                    {
                        result = false;
                    }
                }
                else if (modifyType == ModifyType.Edit)
                {
                    string sql = string.Empty;
                    if (string.IsNullOrEmpty(companyId))
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND IsDeleted='false' AND ID!='" + rowid + "'";
                    }
                    else
                    {
                        sql = "SELECT COUNT(*) FROM " + tableName + " WHERE " + fieldName + "='" + fieldValue + "' AND CompanyId='" + companyId + "' AND IsDeleted='false' AND ID!='" + rowid + "'";
                    }
                    if (!string.IsNullOrEmpty(whereCondition))
                    {
                        sql += " AND " + whereCondition;
                    }
                    int count = Convert.ToInt32(DBHelper.Instance.ExecuteScalar(sql));
                    if (count > 0)
                    {
                        result = true;
                        throw new Exception(string.Format("{0}【{1}】已经存在！", promptName, fieldValue));
                    }
                    else
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 是否为正确的E-mail格式
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            String EmailPattern = @"^[a-z]([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$";
            Regex regex = new Regex(EmailPattern, RegexOptions.IgnoreCase);
            if (regex.Match(email).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static void IsNullOrEmpty(string key, string message)
        {
            if (string.IsNullOrEmpty(key))
                throw new Exception(message);
        }
        public static void IsNullOrEmpty(Guid key, string message)
        {
            if (string.IsNullOrEmpty(key.ToString()))
                throw new Exception(message);
        }
    }
}
