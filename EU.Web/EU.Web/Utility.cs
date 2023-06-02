using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EU.Web
{
    public class Utility
    {
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

        #region Web Api返回结果
        /// <summary>把字符串格式化为Web Api返回的结果</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static HttpResponseMessage FormatApiString(string value)
        {
            try
            {
                return new HttpResponseMessage()
                {
                    Content = (HttpContent)new StringContent(value, Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>把Json对象格式化为Web Api返回的结果</summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static HttpResponseMessage FormatApiJson(object json)
        {
            try
            {
                string content = JsonConvert.SerializeObject(json);
                return new HttpResponseMessage()
                {
                    Content = (HttpContent)new StringContent(content, Encoding.UTF8, "application/json")
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
