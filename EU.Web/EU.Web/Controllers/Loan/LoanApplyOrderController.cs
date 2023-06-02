using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EU.Core.Services;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject;
using EU.Model.Loan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.Loan
{
    public class LoanApplyOrderController : BaseController<LoanApplyOrder>
    {
        public LoanApplyOrderController(DataContext _context, IBaseCRUDVM<LoanApplyOrder> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 获取订单历史
        /// <summary>
        /// 获取订单历史
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public IActionResult GetOrderList()
        {
            #region 变量定义
            dynamic obj = new ExpandoObject();
            string flag = "N";
            string message = "";
            string currentPage1 = HttpContext.Request.Form["currentPage"].ToString();
            string pageSize1 = HttpContext.Request.Form["pageSize"].ToString();
            int currentPage = Convert.ToInt32(currentPage1);
            int pageSize = Convert.ToInt32(pageSize1);
            string userId = HttpContext.Request.Form["userId"].ToString();
            string sql = string.Empty;
            #endregion

            try
            {
                string whereSql = " AND A.ASSIGN_USER_ID = '" + userId + "'";
                sql = $@"SELECT A.ID, A.APPLY_NO,
                               A.APPLY_MAN,
                               A.APPLY_PHONE,
                               A.APPLY_STATUS,
                               B.TypeName,
                               C.TEXT APPLY_STATUS_TEXT,
                               A.CreatedTime
                        FROM LOAN_APPLY_ORDER A
                             LEFT JOIN LOAN_TYPE B ON A.APPLY_TYPE_ID = B.ID
                             LEFT JOIN SM_LOV_V C
                                ON A.APPLY_STATUS = C.VALUE AND C.Lov_Code = 'APPLY_STATUS'
                        WHERE A.IsDeleted = 'false' {whereSql}";
                sql = "SELECT * FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY CreatedTime DESC) NUM FROM (SELECT * FROM (" + sql + " ";
                sql += ") A ) B ) C";
                sql += " WHERE NUM <= " + currentPage * pageSize + " AND NUM >" + (currentPage - 1) * pageSize;
                DataTable dt = DBHelper.Instance.GetDataTable(sql, null);

                string CountSql = $@"SELECT COUNT (1) COUNT
                                      FROM LOAN_APPLY_ORDER A
                                 WHERE A.IsDeleted = 'false' {whereSql}";
                var Count = DBHelper.Instance.ExecuteScalar(CountSql, null);
                int totalItemCount = Count == null ? 0 : Convert.ToInt32(Count);

                List<LoanApplyOrderExtend> orderList = new List<LoanApplyOrderExtend>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LoanApplyOrderExtend order = new LoanApplyOrderExtend();
                    order.ID = (Guid)dt.Rows[i]["ID"];
                    order.APPLY_NO = dt.Rows[i]["APPLY_NO"].ToString();
                    order.APPLY_MAN = dt.Rows[i]["APPLY_MAN"].ToString();
                    order.APPLY_PHONE = dt.Rows[i]["APPLY_PHONE"].ToString();
                    order.APPLY_STATUS = dt.Rows[i]["APPLY_STATUS_TEXT"].ToString();
                    order.CreatedTimeStr = dt.Rows[i]["CreatedTime"].ToString();
                    order.APPLY_STATUS_TEXT = dt.Rows[i]["APPLY_STATUS_TEXT"].ToString();
                    orderList.Add(order);
                }

                //JArray ja = JArray.Parse(JsonConvert.SerializeObject(orderList));
                obj.result = orderList;
                obj.currentPage = currentPage;
                obj.totalCounts = totalItemCount;

                flag = "Y";
            }
            catch (Exception E)
            {
                Logger.WriteLog("StackTrace：" + E.StackTrace);

                message = E.Message;
            }
            obj.flag = flag;
            obj.message = message;
            return Ok(obj);
        }
        #endregion


        #region 生成二维码
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public IActionResult GenerateUserQrCode()
        {
            #region 变量定义
            dynamic obj = new ExpandoObject();
            string flag = "N";
            string message = "";
            string sql = string.Empty;
            string userId = HttpContext.Request.Form["userId"].ToString();
            string content = HttpContext.Request.Form["content"].ToString();

            #endregion

            try
            {
                if (string.IsNullOrEmpty(userId))
                    userId = "123456";
                if (string.IsNullOrEmpty(content))
                    content = "123456qqqq";
                string photosPhysicalPath = "wwwroot/";
                string thumbnailPath = photosPhysicalPath + @"barcode/" + userId + ".png";
                #region 判断文件是否存在
                if (!FileHelper.FileExists(thumbnailPath))
                {
                    string waterMarkPicPath = photosPhysicalPath + @"barcode/" + EU.Core.Utilities.Utility.GetSysID() + ".png";

                    StringHelper.CreateQrCode(content, waterMarkPicPath);

                    string originalImagePath = photosPhysicalPath + @"barcode\invite-template.png";

                    //添加二维码
                    MakeImageWaterThumbnail(originalImagePath, thumbnailPath, 444, 714, MakeThumbnailMode.None, ImageFormat.Png, waterMarkPicPath);
                }
                #endregion

                obj.url = @"barcode/" + userId + ".png";
                flag = "Y";
            }
            catch (Exception E)
            {
                message = E.Message;
            }
            obj.flag = flag;
            obj.message = message;
            return Ok(obj);
        }

        public static Bitmap GetThumbnail(Image originalImage, int width, int height,
           MakeThumbnailMode mode, out Graphics graphics,
           //DONE: 由外部设置缩略图的清晰度, 拟减少文件大小 BEN ADD 20130123 22:41
           //TODO: 参数太多, 用对象封装, 拟减少调用的痛苦程度 BEN ADD 20130124 16:40
           InterpolationMode interpolationMode = InterpolationMode.High,
           SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;
            int ow = originalImage.Width;
            int oh = originalImage.Height;

            //TODO: Auto目前按照容器宽度进行等比例判断, 这是错误的, 应该根据原图的宽高比进行判断
            if (mode == MakeThumbnailMode.Auto)
            {
                if (towidth > toheight)
                {
                    mode = MakeThumbnailMode.W;
                }
                else
                {
                    mode = MakeThumbnailMode.H;
                }
            }
            //当原图尺寸小于指定缩略尺寸时, 不进行缩略, 直接输出原图, 并且不更改尺寸.
            if (ow < towidth && oh < toheight)
            {
                towidth = ow;
                toheight = oh;
            }
            else
            {
                switch (mode)
                {
                    case MakeThumbnailMode.HW: //指定高宽缩放（可能变形）
                        break;
                    case MakeThumbnailMode.W: //指定宽，高按比例
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case MakeThumbnailMode.H: //指定高，宽按比例
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case MakeThumbnailMode.Cut: //指定高宽裁减（不变形）
                        if ((double)originalImage.Width / (double)originalImage.Height >
                        (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }
            }
            //新建一个bmp图片
            Bitmap bitmap = new Bitmap(towidth, toheight);
            bitmap.MakeTransparent(Color.Transparent);
            //新建一个画板
            graphics = Graphics.FromImage(bitmap);
            //设置高质量插值法
            graphics.InterpolationMode = interpolationMode;
            //设置高质量,低速度呈现平滑程度
            graphics.SmoothingMode = smoothingMode;
            //清空画布并以透明背景色填充
            graphics.Clear(Color.Transparent);
            //在指定位置并且按指定大小绘制原图片的指定部分
            graphics.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
            new Rectangle(x, y, ow, oh),
            GraphicsUnit.Pixel);
            return bitmap;
        }

        public enum MakeThumbnailMode
        {
            None,
            /// <summary>
            /// 自动缩放
            /// </summary>
            Auto,
            /// <summary>
            /// 指定宽，高按比例
            /// </summary>
            W,
            /// <summary>
            /// 指定高，宽按比例
            /// </summary>
            H,
            /// <summary>
            /// 指定高宽缩放（可能变形）
            /// </summary>
            HW,
            /// <summary>
            /// 指定高宽裁减（不变形）
            /// </summary>
            Cut
        }

        public static void MakeImageWaterThumbnail(string originalImagePath, string thumbnailPath, int width, int height,
                                            MakeThumbnailMode mode, ImageFormat imageFormat, string waterMarkPicPath,
                                            int transparentValue = 100,
                                            InterpolationMode interpolationMode = InterpolationMode.High,
                                            SmoothingMode smoothingMode = SmoothingMode.HighQuality)
        {
            using (Image originalImage = Image.FromFile(originalImagePath))
            {
                Graphics picture;
                Bitmap bitmap = GetThumbnail(originalImage, width, height, mode, out picture, interpolationMode, smoothingMode);

                Image watermark = new Bitmap(waterMarkPicPath);
                ImageAttributes imageAttributes = new ImageAttributes();
                ColorMap colorMap = new ColorMap();
                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                ColorMap[] remapTable = { colorMap };
                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
                float tranValue = (float)transparentValue / 100;
                float[][] colorMatrixElements = {
                                                 new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                              new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                                 new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                               new float[] {0.0f,  0.0f,  0.0f,  tranValue, 0.0f},//透明度默认值为0.3
                                               new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                            };

                ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                int xpos = 0;
                int ypos = 0;
                int watermarkWidth = 0;
                int watermarkHeight = 0;
                double bl = 1d;
                //计算水印图片的比率
                //取背景的1/4宽度来比较
                if ((bitmap.Width > watermark.Width * 4) && (bitmap.Height > watermark.Height * 4))
                {
                    bl = 1;
                }
                else if ((bitmap.Width > watermark.Width * 4) && (bitmap.Height < watermark.Height * 4))
                {
                    bl = Convert.ToDouble(bitmap.Height / 4) / Convert.ToDouble(watermark.Height);
                }
                else

                    if ((bitmap.Width < watermark.Width * 4) && (bitmap.Height > watermark.Height * 4))
                {
                    bl = Convert.ToDouble(bitmap.Width / 4) / Convert.ToDouble(watermark.Width);
                }
                else
                {
                    if ((bitmap.Width * watermark.Height) > (bitmap.Height * watermark.Width))
                    {
                        bl = Convert.ToDouble(bitmap.Height / 4) / Convert.ToDouble(watermark.Height);
                    }
                    else
                    {
                        bl = Convert.ToDouble(bitmap.Width / 4) / Convert.ToDouble(watermark.Width);
                    }
                }

                watermarkWidth = Convert.ToInt32(watermark.Width * bl);
                watermarkHeight = Convert.ToInt32(watermark.Height * bl);
                xpos = bitmap.Width - watermarkWidth - 30;
                ypos = bitmap.Height - watermarkHeight - 30;
                //ypos += 200;
                picture.DrawImage(watermark, new Rectangle(xpos, ypos, watermarkWidth, watermarkHeight), 0, 0, watermark.Width, watermark.Height, GraphicsUnit.Pixel, imageAttributes);

                try
                {
                    //以xxx格式保存缩略+水印图
                    bitmap.Save(thumbnailPath, imageFormat);
                }
                finally
                {
                    watermark.Dispose();
                    imageAttributes.Dispose();
                    bitmap.Dispose();
                    picture.Dispose();
                }
            }
        }
        #endregion

        #region 提交申请工单
        /// <summary>
        /// 提交申请工单
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public IActionResult SubmitApplayOrder(LoanApplyOrder Model)
        {
            #region 变量定义
            dynamic obj = new ExpandoObject();
            string flag = "Y";
            string message = "提交成功！";
            string sql = string.Empty;
            //string userId = HttpContext.Request.Form["userId"].ToString();
            //string type = HttpContext.Request.Form["type"].ToString();
            //string phone = HttpContext.Request.Form["phone"].ToString();
            //string name = HttpContext.Request.Form["name"].ToString();

            #endregion

            try
            {
                DoAddPrepare(Model);
                _BaseCrud.DoAdd(Model);

                obj.Id = Model.GetType().GetProperties().Where(x => x.Name.ToLower() == "id").FirstOrDefault()
                    ?.GetValue(Model).ToString();

                flag = "Y";
            }
            catch (Exception E)
            {
                flag = "N";
                message = E.Message;
            }
            obj.flag = flag;
            obj.message = message;
            return Ok(obj);
        }
        #endregion
    }
}
