using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EU.Core.CacheManager;
using EU.Core.Const;
using EU.Core.Entry;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.System;
using EU.Model;
using EU.Model.JWT;
using EU.Model.System;
using EU.Model.System.CompanyStructure;
using EU.Model.System.Privilege;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Senparc.NeuChar.App.AppStore;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Auth)]
    public class AccountController : BaseController<SmUser>
    {
        /// <summary>
        /// Jwt 服务
        /// </summary>
        private readonly IJwtAppService _jwtApp;
        private RedisCacheService Redis = new RedisCacheService(4);

        /// <summary>
        /// 系统用户
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="jwtApp"></param>
        /// <param name="BaseCrud"></param>
        public AccountController(DataContext _context, IJwtAppService jwtApp, IBaseCRUDVM<SmUser> BaseCrud) : base(_context, BaseCrud)
        {
            _jwtApp = jwtApp;
        }

        /// <summary>
        ///用户管理 -- 新增数据
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public override IActionResult Add(SmUser Model)
        {
            Model.IsActive = true;
            var count = _context.SmUsers.Where(x => x.IsDeleted == false && x.UserAccount == Model.UserAccount).Count();
            if (count > 0)
            {
                throw new Exception("账号已存在！");
            }
            Model.PassWord = Utility.GetMD5String(Model.PassWord);
            Model.IsActive = Model.IsActive ?? true;
            return base.Add(Model);
        }

        #region 用户认证-认证授权

        /// <summary>
        /// 用户认证-认证授权
        /// </summary>
        /// <param name="smUser"></param>
        /// <returns></returns>
        [AllowAnonymous, HttpPost]
        public async Task<ServiceResult<dynamic>> Login(SmUser smUser)
        {
            dynamic obj = new ExpandoObject();

            var user = await _BaseCrud.GetAsync(x => x.IsDeleted == false && x.UserAccount == smUser.UserAccount && x.PassWord == Utility.GetMD5String(smUser.PassWord));
            if (user != null)
            {
                SmUser User = user;
                Redis.Remove(User.ID.ToString());
                Redis.AddObject(User.ID.ToString(), User, new TimeSpan(0, 1, 0, 0, 0));

                var result = _jwtApp.Create(User);
                obj.Token = result.Token;
                obj.UserId = user.ID;

                //string sql = @"SELECT DISTINCT C.*
                //                    FROM SmRoleModule A
                //                         JOIN SmUserRole_V B
                //                            ON     A.SmRoleId = B.SmRoleId
                //                               AND B.SmUserId = '{0}'
                //                         JOIN SmModules C ON A.SmModuleId = C.ID AND C.IsDeleted = 'false'
                //                    WHERE A.IsDeleted = 'false'";
                //sql = string.Format(sql, user.ID);
                //var roleModule = DBHelper.Instance.QueryList<SmModule>(sql);
                obj.Modules = new List<SmModule>();

                #region 记录用户登录日志
                Utility.RecordEntryLog(User.ID, "user");
                #endregion
            }
            else
            {
                throw new Exception("用户名或密码错误！");
            }
            return ServiceResult<dynamic>.OprateSuccess(obj, ResponseText.QUERY_SUCCESS);

        }

        /// <summary>
        /// 用户认证-认证授权
        /// </summary>
        /// <returns></returns>
        [HttpGet, AllowAnonymous, HttpPost]
        public IActionResult CheckLogin()
        {
            dynamic obj = new ExpandoObject();
            string status = "N";
            string message = "登陆成功！";
            string userId = string.Empty;
            try
            {
                string openid = HttpContext.Request.Form["openid"].ToString();
                string UserAccount = HttpContext.Request.Form["UserAccount"].ToString();
                string PassWord = HttpContext.Request.Form["PassWord"].ToString();

                var user = _BaseCrud.Get().Where(x => x.IsDeleted == false &&
                                                     x.UserAccount == UserAccount &&
                                                     x.PassWord == Utility.GetMD5String(PassWord)).ToList();
                if (user.Count == 1)
                {
                    var result = _jwtApp.Create(user.FirstOrDefault());
                    obj.token = result.Token;
                }
                else
                {
                    throw new Exception("用户名或密码错误！");
                }

                SmUser smUser = user.FirstOrDefault();
                userId = Convert.ToString(smUser.ID);
                obj.UserName = smUser.UserName;
                string sql = "UPDATE WX_USER SET [USER_ID]='{1}' WHERE OPEN_ID='{0}'";
                sql = string.Format(sql, openid, userId);
                DataTable dt = DBHelper.Instance.GetDataTable(sql, null);

                status = "Y";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.flag = status;
            obj.userId = userId;
            obj.message = message;
            return Ok(obj);
        }
        #endregion

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ServiceResult> ResetPassword(SmUser user)
        {
            try
            {
                var pssWord = user.PassWord;

                user = await _context.SmUsers.FindAsync(user.ID);
                user.PassWord = Utility.GetMD5String(pssWord);
                await _context.SaveChangesAsync();
                return ServiceResult.OprateSuccess("重置成功！");
            }
            catch (Exception E)
            {
                return ServiceResult.OprateFailed(E.Message);
            }
        }


        /// <summary>
        /// 当前用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult CurrentUser()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid UserId = Guid.Parse(User.Identity.Name);
                if (UserId != Guid.Empty)
                {
                    //obj.data = _context.Set<SmUser>().Where(x => x.ID == UserId).Select(x => new CurrentUser
                    //{
                    //    name = x.UserName,
                    //    userid = x.ID.ToString(),
                    //}).FirstOrDefault();
                    SmUser user = Redis.Get<SmUser>(UserId.ToString());
                    if (user == null)
                    {
                        user = _context.Set<SmUser>().Where(x => x.ID == UserId).FirstOrDefault();
                        Redis.AddObject(UserId.ToString(), User, new TimeSpan(0, 1, 0, 0, 0));
                    }
                    obj.data = new CurrentUser
                    {
                        name = user.UserName,
                        userid = user.ID.ToString(),
                    };

                    var roleList = _context.Set<SmUserRole>().Where(x => x.IsDeleted == false & x.SmUserId == UserId)
                        .Join(_context.Set<SmRole>(), x => x.SmRoleId, y => y.ID, (x, y) => new { x, y })
                        .Where(y => y.y.IsDeleted == false && y.y.IsActive == true)
                        .Select(x => x.y.RoleCode).ToList();
                    string role = string.Join(",", roleList);
                    obj.role = role;
                }

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult LogOut()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                _jwtApp.DeactivateCurrentAsync();
                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        /// <summary>
        /// 获取token
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAccessToken()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string token = this.HttpContext.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer"))
                {
                    DateTime ExpirationTime = DateTime.Parse(User.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Expiration).Value);
                    DateTime NowTime = DateTime.UtcNow;

                    TimeSpan timeSpan = ExpirationTime.Subtract(NowTime);
                    if (timeSpan.Minutes < 10)//过期时间最后的十分钟刷新token
                    {
                        var dto = _context.Set<SmUser>().Where(x => x.ID == Guid.Parse(User.Identity.Name)).FirstOrDefault();
                        var result = _jwtApp.RefreshAsync(token, dto).Result;
                        if (result.Success != false)
                        {
                            _jwtApp.DeactivateAsync(token);//停用刷新前的token
                            obj.token = result.Token;
                        }
                    }
                }
                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #region 获取用户部门信息
        /// <summary>
        /// 获取用户部门信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserDepartment()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                string UserId = User.Identity.Name;
                if (!string.IsNullOrEmpty(UserId))
                {
                    //SmUser SmUser = _context.Set<SmUser>().Where(x => x.ID == UserId).FirstOrDefault();
                    //obj.data = _context.SmEmployee.Where(x => x.ID == SmUser.EmployeeId).FirstOrDefault(); ;
                    string sql = @"SELECT A.ID, A.DepartmentName
                                FROM SmDepartment A
                                     JOIN SmEmployee B ON A.ID = B.DepartmentId
                                     JOIN SmUsers C ON B.ID = C.EmployeeId
                                WHERE C.ID = '{0}'";
                    sql = string.Format(sql, UserId);
                    obj.data = DBHelper.Instance.QueryFirst<SmDepartment>(sql, null);

                }

                status = "ok";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class CurrentUser
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string userid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }
    }
}
