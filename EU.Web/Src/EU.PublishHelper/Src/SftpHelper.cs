using FluentFTP;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JianLian.HDIS.PublishHelper
{
    /// <summary>
    /// SFTP操作类
    /// </summary>
    public class SftpHelper
    {
        #region 下载文件
        /// <summary>
        /// 下载单个文件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="remotePath"></param>
        /// <param name="localFile"></param>
        /// <param name="b_log"></param>
        /// <returns></returns>
        public static bool DownloadFile(Server server, string remoteFile, string localFile, bool b_log = false, Action<long, ulong> progress = null, Action completedAction = null)
        {
            bool b_suc = false;
            try
            {
                if (!Utility.Ping(server.Ip, b_log))
                {
                    return b_suc;
                }
                using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    sftp.Connect();
                    if (sftp.Exists(remoteFile))
                    {
                        if (b_log)
                            Utility.SendLog("下载", $"下载文件 {remoteFile}");
                        if (File.Exists(localFile))
                        {
                            File.Delete(localFile);
                            System.Threading.Thread.Sleep(50);
                        }
                        var sftFile = sftp.ListDirectory(remoteFile.Substring(0, remoteFile.LastIndexOf('/'))).Where(o => o.FullName == remoteFile).FirstOrDefault();
                        using (var file = File.OpenWrite(localFile))
                        {
                            sftp.DownloadFile(remoteFile, file, pro =>
                            {
                                progress?.Invoke(sftFile.Length, pro);
                            });
                            if (b_log)
                                Utility.SendLog("下载", $"成功 {remoteFile}  =>> {localFile} len {file.Length} byte");
                            b_suc = true;
                        }
                    }
                    else
                    {
                        if (b_log)
                            Utility.SendLog("下载", $"文件不存在 {remoteFile}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (b_log)
                    Utility.SendLog("下载", $"下载文件失败：{remoteFile} {ex.Message}");
            }
            finally
            {
                completedAction?.Invoke();
            }
            return b_suc;
        }
        #endregion

        #region 上传文件
        /// <summary>
        /// 上传单个文件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="localFile"></param>
        /// <param name="remoteFile"></param>
        /// <param name="progress"></param>
        public static void UploadFile(Server server, string localFile, string remoteFile, Action<long, ulong> progress = null, bool create = false)
        {
            if (!Utility.Ping(server.Ip))
                return;

            try
            {
                FtpClient client = new FtpClient(server.Ip, server.Port, server.UserName, server.SuPassword);
                client.AutoConnect();

                using (var file = File.OpenRead(localFile))
                {
                    try
                    {
                        var len = file.Length;
                        client.UploadFile(localFile, remoteFile);
                        Utility.SendLog("上传", $"成功 {localFile}  =>> {remoteFile} len {len} byte");
                    }
                    catch (Exception e)
                    {
                        Utility.SendLog("上传", $"失败 {remoteFile} 原因：{e}");
                    }
                }
                client.Disconnect();
            }
            catch (Exception E)
            {
                Utility.SendLog("上传", $"失败 {remoteFile} 原因：{E}");
            }

            #region 作废代码
            //using (SftpClient sftp = new SftpClient(server.Ip, server.Port, server.UserName, server.SuPassword))
            //{
            //    sftp.Connect();
            //    using (var file = File.OpenRead(localFile))
            //    {
            //        try
            //        {
            //            if (!sftp.Exists(remoteFile) && create)
            //            {
            //                sftp.Create(remoteFile);
            //            }
            //            var len = file.Length;
            //            sftp.UploadFile(file, remoteFile, pro =>
            //            {
            //                progress?.Invoke(len, pro);
            //            });
            //            Utility.SendLog("上传", $"成功 {localFile}  =>> {remoteFile} len {len} byte");
            //        }
            //        catch (Exception e)
            //        {
            //            Utility.SendLog("上传", $"失败 {remoteFile} 原因：{e}");
            //        }
            //    }
            //}
            #endregion
        }
        /// <summary>
        /// 批量上传文件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="localPath"></param>
        /// <param name="remotePath"></param>
        /// <param name="files"></param>
        /// <param name="progress"></param>
        public static void UploadFile(Server server, string localPath, string remotePath, List<string> files, Action<int, int> progress)
        {
            if (!Utility.Ping(server.Ip))
                return;

            try
            {
                progress(files.Count, 0);

                FtpClient sftp = new FtpClient(server.Ip, server.Port, server.UserName, server.SuPassword);
                sftp.AutoConnect();

                var count = files.Count;
                var index = 0;
                files?.ForEach(fname =>
                {
                    if (Utility.m_StopPublish)
                        return;
                    var localFileName = $"{localPath}\\{fname}";
                    var remoteFileName = $"{remotePath}/{fname.Replace("\\", "/")}";
                    using (var file = File.OpenRead(localFileName))
                    {
                        var len = file.Length;
                        try
                        {
                            index++;
                            if (fname.Contains("\\"))
                            {
                                StringBuilder sb = new StringBuilder();
                                var ff = fname.Substring(0, fname.LastIndexOf("\\")).Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                                ff.ToList().ForEach(f =>
                                {
                                    sb.Append($"/{f}");
                                    var fpath = $"{remotePath}{sb}";
                                    if (!sftp.DirectoryExists(fpath))
                                    {
                                        sftp.CreateDirectory(fpath);
                                        Utility.SendLog("上传", $"创建目录成功 {fpath}");
                                    }
                                });
                            }
                            //sftp.UploadFile(file, remoteFileName);
                            sftp.UploadFile(localFileName, remoteFileName);
                            progress(count, index);
                            Utility.SendLog("上传", $"[{index}/{count}]成功 {localPath}\\{fname}  =>> {remoteFileName} len {len} byte");
                        }
                        catch (Exception e)
                        {
                            Utility.SendLog("上传", $"[{index}/{count}] 失败 原因：{fname} {e}");
                            try
                            {
                                Utility.SendLog("重新上传", $"[{index}/{count}] {fname} ");
                                sftp.UploadFile(localFileName, remoteFileName);
                                progress(count, index);
                                Utility.SendLog("上传", $"[{index}/{count}]成功 {localPath}\\{fname}  =>> {remoteFileName} len {len} byte");
                            }
                            catch (Exception E)
                            {
                                Utility.SendLog("上传", $"[{index}/{count}] 失败 原因：{fname} {E}");

                            }
                        }
                    }

                });
                sftp.Disconnect();

                #region 作废代码
                //using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                //{
                //    sftp.Connect();
                //    var count = files.Count;
                //    var index = 0;
                //    files?.ForEach(fname =>
                //    {
                //        if (Utility.m_StopPublish)
                //            return;
                //        var localFileName = $"{localPath}\\{fname}";
                //        var remoteFileName = $"{remotePath}/{fname.Replace("\\", "/")}";
                //        using (var file = File.OpenRead(localFileName))
                //        {
                //            try
                //            {
                //                index++;
                //                var len = file.Length;
                //                if (fname.Contains("\\"))
                //                {
                //                    StringBuilder sb = new StringBuilder();
                //                    var ff = fname.Substring(0, fname.LastIndexOf("\\")).Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
                //                    ff.ToList().ForEach(f =>
                //                    {
                //                        sb.Append($"/{f}");
                //                        var fpath = $"{remotePath}{sb}";
                //                        if (!sftp.Exists(fpath))
                //                        {
                //                            sftp.CreateDirectory(fpath);
                //                            Utility.SendLog("上传", $"创建目录成功 {fpath}");
                //                        }
                //                    });
                //                }
                //                sftp.UploadFile(file, remoteFileName);
                //                progress(count, index);
                //                Utility.SendLog("上传", $"[{index}/{count}]成功 {localPath}\\{fname}  =>> {remoteFileName} len {len} byte");
                //            }
                //            catch (Exception e)
                //            {
                //                Utility.SendLog("上传", $"[{index}/{count}] 失败 原因：{fname} {e}");
                //            }
                //        }

                //    });
                //}
                #endregion
            }
            catch (Exception ex)
            {
                Utility.SendLog("", ex.ToString());
            }
        }
        #endregion

        #region 获取文件
        /// <summary>
        /// 获取当前目录的文件列表
        /// </summary>
        /// <param name="server"></param>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public static List<SftpFile> ListFiles(Server server, string remotePath)
        {
            List<SftpFile> list = new List<SftpFile>();
            //try
            //{
            //    using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
            //    {
            //        sftp.Connect();
            //        list = sftp.ListDirectory(remotePath).Where(f => f.IsRegularFile).ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Utility.SendLog("", $"获取文件夹失败：{remotePath} {ex.Message}");
            //}
            return list;
        }

        public static List<FtpListItem> GetListing(Server server, string remotePath)
        {
            FtpClient sftp = new FtpClient(server.Ip, server.Port, server.UserName, server.SuPassword);
            sftp.AutoConnect();

            List<FtpListItem> list = new List<FtpListItem>();
            try
            {
                foreach (FtpListItem item in sftp.GetListing(remotePath))
                {
                    list.Add(item);
                    // if this is a file
                    //if (item.Type == FtpFileSystemObjectType.File)
                    //{

                    //    // get the file size
                    //    long size = sftp.GetFileSize(item.FullName);

                    //    // calculate a hash for the file on the server side (default algorithm)
                    //    FtpHash hash = sftp.GetChecksum(item.FullName);
                    //}

                    //// get modified date/time of the file or folder
                    //DateTime time = sftp.GetModifiedTime(item.FullName);
                }
                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                Utility.SendLog("", $"获取文件夹失败：{remotePath} {ex.Message}");
            }
            return list;
        }
        #endregion

        #region 获取文件夹
        /// <summary>
        /// 获取当前目录的文件夹列表
        /// </summary>
        /// <param name="server"></param>
        /// <param name="remotePath"></param>
        /// <returns></returns>
        public static List<SftpFile> ListDirectory(Server server, string remotePath)
        {
            List<SftpFile> list = new List<SftpFile>();
            try
            {
                using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    sftp.Connect();
                    list = sftp.ListDirectory(remotePath).Where(f => f.IsDirectory && f.Name != "." && f.Name != "..").ToList();
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog("", $"获取文件夹失败：{remotePath} {ex.Message}");
            }
            return list;
        }
        #endregion

        #region 文件/文件夹是否存在
        /// <summary>
        /// 文件/文件夹是否存在
        /// </summary>
        /// <param name="server"></param>
        /// <param name="path"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public static bool Exists(Server server, string path)
        {
            bool b_suc = false;
            if (!Utility.Ping(server.Ip))
            {
                return b_suc;
            }
            try
            {
                using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    sftp.Connect();
                    b_suc = sftp.Exists(path);
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog("", ex.ToString());
            }
            return b_suc;
        }

        public static void CreateDirectory(Server server, string path)
        {
            if (!Utility.Ping(server.Ip))
            {
                return;
            }
            try
            {
                using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                {
                    sftp.Connect();
                    if (!sftp.Exists(path))
                    {
                        sftp.CreateDirectory(path);
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.SendLog("", ex.ToString());
            }
        }
        #endregion

        #region  删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="server"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Delete(Server server, string path)
        {
            bool b_suc = false;
            if (!Utility.Ping(server.Ip))
                return b_suc;

            try
            {
                FtpClient sftp = new FtpClient(server.Ip, server.Port, server.UserName, server.SuPassword);
                sftp.AutoConnect();
                if (sftp.FileExists(path))
                    sftp.DeleteFile(path);
                sftp.Disconnect();

                //using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                //{
                //    sftp.Connect();
                //    if (sftp.FileExists(path))
                //        sftp.Delete(path);
                //    b_suc = true;
                //}
            }
            catch (Exception ex)
            {
                Utility.SendLog("", ex.ToString());
            }
            return b_suc;
        }
        #endregion

        #region  删除文件夹
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="server"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool DeleteDirectory(Server server, string path)
        {
            bool b_suc = false;
            if (!Utility.Ping(server.Ip))
                return b_suc;

            try
            {
                FtpClient sftp = new FtpClient(server.Ip, server.Port, server.UserName, server.SuPassword);
                sftp.AutoConnect();
                if (sftp.DirectoryExists(path))
                    sftp.DeleteDirectory(path);
                sftp.Disconnect();

                //using (SftpClient sftp = new SftpClient(server.Ip, server.Port, "root", server.SuPassword))
                //{
                //    sftp.Connect();
                //    if (sftp.FileExists(path))
                //        sftp.Delete(path);
                //    b_suc = true;
                //}
            }
            catch (Exception ex)
            {
                Utility.SendLog("", ex.ToString());
            }
            return b_suc;
        }
        #endregion
    }
}
