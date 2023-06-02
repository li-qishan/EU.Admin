using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System
{
    /// <summary>
    /// 文件服务
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.System)]
    public class FileController : BaseController<FileAttachment>
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FileController(DataContext _context, IBaseCRUDVM<FileAttachment> BaseCrud, IConfiguration configuration, IWebHostEnvironment hostingEnvironment) : base(_context, BaseCrud)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult GetFileList(Guid masterId, string imageType = null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                var src = _context.Set<FileAttachment>().Where(x => x.IsDeleted == false);
                if (!string.IsNullOrEmpty(imageType))
                    src = src.Where(o => o.ImageType == imageType);

                obj.data = src.Where(x => x.MasterId == masterId)
                    .OrderByDescending(x => x.CreatedTime).ToList();
                obj.prefixUrl = $"{Request.Scheme}://{Request.Host}/";

                status = "ok";
                message = "查询成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFileCollection fileList, Guid? masterId, bool isUnique = false, string filePath = null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string pathHeader = string.Empty;
            string url = string.Empty;

            try
            {
                filePath = !string.IsNullOrEmpty(filePath) ? filePath : _configuration["FileUploadOptions:UploadDir"];
                string ImageType = filePath;
                if (fileList.Count > 0)
                {
                    foreach (var file in fileList)
                    {

                        var ext = string.Empty;
                        if (string.IsNullOrEmpty(file.FileName) == false)
                        {
                            var dotPos = file.FileName.LastIndexOf('.');
                            ext = file.FileName.Substring(dotPos + 1);
                        }
                        filePath += "/" + Utility.GetSysID() + "/";

                        pathHeader = "wwwroot/" + filePath;
                        if (!Directory.Exists(pathHeader))
                        {
                            Directory.CreateDirectory(pathHeader);
                        }

                        string fileName = file.FileName;
                        var filepath = Path.Combine(pathHeader, fileName);
                        //var filepath = Path.Combine(pathHeader, file.FileName);
                        using (var stream = global::System.IO.File.Create(filepath))
                        {
                            await file.CopyToAsync(stream);
                        }
                        FileAttachment fileAttachment = new FileAttachment();
                        fileAttachment.OriginalFileName = fileName;
                        fileAttachment.CreatedBy = !string.IsNullOrEmpty(User.Identity.Name) ? new Guid(User.Identity.Name) : Guid.Empty;
                        fileAttachment.CreatedTime = Utility.GetSysDate();
                        fileAttachment.FileName = fileName;
                        fileAttachment.FileExt = ext;
                        fileAttachment.MasterId = masterId;
                        fileAttachment.Length = file.Length;
                        fileAttachment.Path = filePath;
                        fileAttachment.ImageType = ImageType;
                        url = fileName;
                        _context.Add(fileAttachment);
                    }

                    _context.SaveChanges();

                    if (isUnique)
                    {
                        string sql = @"UPDATE FileAttachment
                                SET IsDeleted = 'true'
                                WHERE MasterId = '57ec49b5-1a49-4538-9278-4fa7ad86cbc5'
                                      AND ID NOT IN(SELECT TOP (1) ID
                                                    FROM FileAttachment
                                                    WHERE MasterId = '57ec49b5-1a49-4538-9278-4fa7ad86cbc5'
                                                     ORDER BY CreatedTime DESC)
                                      AND IsDeleted = 'false'";
                        sql = string.Format(sql, masterId);
                        DBHelper.Instance.ExcuteNonQuery(sql);
                    }
                }

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.url = url;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImageAsync(IFormFileCollection fileList, Guid? masterId, bool isUnique = false, string filePath = null, string masterTable = null, string masterColumn = null, string imageType = null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string pathHeader = string.Empty;
            string url = string.Empty;

            try
            {
                filePath = !string.IsNullOrEmpty(filePath) ? filePath : _configuration["FileUploadOptions:UploadDir"];
                if (fileList.Count > 0)
                {
                    foreach (var file in fileList)
                    {

                        var ext = string.Empty;
                        if (string.IsNullOrEmpty(file.FileName) == false)
                        {
                            var dotPos = file.FileName.LastIndexOf('.');
                            ext = file.FileName.Substring(dotPos + 1);
                        }

                        pathHeader = "wwwroot/" + filePath;
                        if (!Directory.Exists(pathHeader))
                        {
                            Directory.CreateDirectory(pathHeader);
                        }

                        string fileName = Utility.GetSysID();
                        var filepath = Path.Combine(pathHeader, $"{fileName}.{ext}");
                        //var filepath = Path.Combine(pathHeader, file.FileName);
                        using (var stream = global::System.IO.File.Create(filepath))
                        {
                            await file.CopyToAsync(stream);
                        }
                        FileAttachment fileAttachment = new FileAttachment();
                        fileAttachment.OriginalFileName = file.FileName;
                        fileAttachment.CreatedBy = !string.IsNullOrEmpty(User.Identity.Name) ? new Guid(User.Identity.Name) : Guid.Empty;
                        fileAttachment.CreatedTime = Utility.GetSysDate();
                        fileAttachment.FileName = fileName;
                        fileAttachment.FileExt = ext;
                        fileAttachment.MasterId = masterId;
                        fileAttachment.Length = file.Length;
                        fileAttachment.Path = filePath;
                        fileAttachment.ImageType = imageType ?? filePath;
                        url = fileName + "." + ext;
                        _context.Add(fileAttachment);
                        _context.SaveChanges();

                        if (!string.IsNullOrEmpty(masterTable) && !string.IsNullOrEmpty(masterColumn))
                        {
                            string sql = "UPDATE {2} SET {3}='{1}' WHERE ID='{0}'";
                            sql = string.Format(sql, masterId, fileName + "." + ext, masterTable, "ImageUrl");
                            DBHelper.Instance.ExcuteNonQuery(sql);
                        }
                    }
                }

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.url = url;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }


        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet, AllowAnonymous]
        public IActionResult Download(string id)
        {
            string sql = "SELECT * FROM FileAttachment where ID='{0}' and IsDeleted='false'";
            sql = string.Format(sql, id);
            FileAttachment attachment = DBHelper.Instance.QueryFirst<FileAttachment>(sql);

            if (attachment != null)
            {
                var fileName = attachment.FileName;
                string path = "wwwroot/" + attachment.Path + fileName;
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                fs.Close();
                return File(new FileStream(path, FileMode.Open), "application/octet-stream", fileName);
            }
            else
            {
                return Ok("无效ID");
            }

        }
        #endregion

        #region 上传Excel导入模板
        [HttpPost]
        public async Task<IActionResult> UploadImportAsync(IFormFileCollection fileList, Guid? masterId, bool isUnique = false, string filePath = null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            string pathHeader = string.Empty;
            string url = string.Empty;

            try
            {
                filePath = !string.IsNullOrEmpty(filePath) ? filePath : _configuration["FileUploadOptions:UploadDir"];
                if (fileList.Count > 0)
                {
                    foreach (var file in fileList)
                    {

                        var ext = string.Empty;
                        if (string.IsNullOrEmpty(file.FileName) == false)
                        {
                            var dotPos = file.FileName.LastIndexOf('.');
                            ext = file.FileName.Substring(dotPos + 1);
                        }

                        pathHeader = "wwwroot/" + filePath;
                        if (!Directory.Exists(pathHeader))
                        {
                            Directory.CreateDirectory(pathHeader);
                        }

                        string fileName = Guid.NewGuid().ToString();
                        var filepath = Path.Combine(pathHeader, $"{fileName}.{ext}");
                        //var filepath = Path.Combine(pathHeader, file.FileName);
                        using (var stream = global::System.IO.File.Create(filepath))
                        {
                            await file.CopyToAsync(stream);
                        }
                        FileAttachment fileAttachment = new FileAttachment();
                        fileAttachment.OriginalFileName = file.FileName;
                        fileAttachment.CreatedBy = !string.IsNullOrEmpty(User.Identity.Name) ? new Guid(User.Identity.Name) : Guid.Empty;
                        fileAttachment.CreatedTime = Utility.GetSysDate();
                        fileAttachment.FileName = fileName;
                        fileAttachment.FileExt = ext;
                        fileAttachment.MasterId = masterId;
                        fileAttachment.Length = file.Length;
                        fileAttachment.Path = filePath;
                        url = fileName + "." + ext;
                        _context.Add(fileAttachment);
                    }

                    _context.SaveChanges();

                    if (isUnique)
                    {
                        string sql = @"UPDATE FileAttachment
                                SET IsDeleted = 'true'
                                WHERE MasterId = '57ec49b5-1a49-4538-9278-4fa7ad86cbc5'
                                      AND ID NOT IN(SELECT TOP (1) ID
                                                    FROM FileAttachment
                                                    WHERE MasterId = '57ec49b5-1a49-4538-9278-4fa7ad86cbc5'
                                                     ORDER BY CreatedTime DESC)
                                      AND IsDeleted = 'false'";
                        sql = string.Format(sql, masterId);
                        DBHelper.Instance.ExcuteNonQuery(sql);
                    }
                }

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.url = url;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion


        #region 获取图片
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetByUrl(string url)
        {
            try
            {
                var webRootPath = _hostingEnvironment.WebRootPath;

                var dotPos = url.LastIndexOf('.');
                var ext = url.Substring(dotPos + 1);
                var file = await _context.FileAttachment.Where(o => o.FileName == url.Replace("." + ext, null)).FirstOrDefaultAsync();

                if (file == null)
                    return Ok("找不到文件");

                var filePath = $"{webRootPath}{"\\" + file.Path + "\\" + url}";
                var fileName = !string.IsNullOrEmpty(file.FileName) ? file.FileName : Path.GetFileName(filePath);

                var contentTypDict = new Dictionary<string, string> {
                    {"jpg","image/jpeg"},
                    {"jpeg","image/jpeg"},
                    {"jpe","image/jpeg"},
                    {"png","image/png"},
                    {"gif","image/gif"},
                    {"ico","image/x-ico"},
                    {"tif","image/tiff"},
                    {"tiff","image/tiff"},
                    {"fax","image/fax"},
                    {"wbmp","image//vnd.wap.wbmp"},
                    {"rp","image/vnd.rn-realpix"}
                };
                var contentTypeStr = "image/jpeg";
                //未知的图片类型
                contentTypeStr = contentTypDict[ext];

                using (var sw = new FileStream(filePath, global::System.IO.FileMode.Open))
                {
                    var bytes = new byte[sw.Length];
                    sw.Read(bytes, 0, bytes.Length);
                    sw.Close();
                    return new FileContentResult(bytes, contentTypeStr);
                }

            }
            catch (Exception ex)
            {
                return Ok($"下载异常：{ex.Message}");
            }
        }
        #endregion

    }
}
