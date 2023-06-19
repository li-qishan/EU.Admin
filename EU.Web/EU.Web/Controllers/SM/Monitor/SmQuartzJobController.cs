using System;
using System.Dynamic;
using EU.Core.Entry;
using System.Threading.Tasks;
using EU.Core.Enums;
using EU.Core.UserManager;
using EU.Core.Utilities;
using EU.DataAccess;
using EU.Domain;
using EU.Model;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;
using EU.TaskHelper;
using EU.Core.Const;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.BD
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Base)]
    public class SmQuartzJobController : BaseController1<SmQuartzJob>
    {

        public SmQuartzJobController(DataContext _context, IBaseCRUDVM<SmQuartzJob> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 新增重写
        [HttpPost]
        public override IActionResult Add(SmQuartzJob Model)
        {


            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdSmQuartzJob", "SmQuartzJobNo", Model.SmQuartzJobNo, ModifyType.Add, null, "材质编号");
                //#endregion

                return base.Add(Model);
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

        #region 更新重写
        [HttpPost]
        public override IActionResult Update(dynamic modelModify)
        {

            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {

                //Guid? userId = CompanyId;

                //#region 检查是否存在相同的编码
                //Utility.CheckCodeExist("", "BdSmQuartzJob", "SmQuartzJobNo", modelModify.SmQuartzJobNo.Value, ModifyType.Edit, modelModify.ID.Value, "材质编号");
                //#endregion

                Update<SmQuartzJob>(modelModify);
                _context.SaveChanges();

                status = "ok";
                message = "修改成功！";
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

        #region 远程操作
        /// <summary>
        /// 远程操作
        /// </summary>
        /// <param name="id">任务清单标识</param>
        /// <param name="operate">操作值 字典Code为`DIC.TASK.OPERATE`</param>
        /// <param name="args">操作参数，当操作为修改参数是必填</param>
        /// <returns></returns>
        [HttpGet("{id}/{operate}")]
        public async Task<ServiceResult> Operate(Guid id, string operate, string args)
        {
            //消息内容
            var task = await _BaseCrud.GetByIdAsync(id);
            TaskMsg taskMsg = new TaskMsg
            {
                MsgId = Guid.NewGuid(),
                TaskType = JobConsts.TASK_TYPE_JOB,
                TaskId = id,
                TaskCode = task.JobCode,
                Oprate = operate,
                Args = args
            };
            //发送消息至对应的任务
            ServiceResult result = ServiceResult.OprateSuccess();
            switch (operate)
            {
                //获取配置文件
                case "CONF":
                //当前日志
                case "LOG.CURRENT":
                //历史日志
                case "LOG.HISTORY":
                    {
                        //发送消息并等待接收返回值
                        (bool suc, object o) = TaskHelper.TaskHelper.SendMsg(taskMsg);
                        if (suc)
                            result.Data = o;
                        else
                            result = ServiceResult.OprateFailed(o.ToString());
                        break;
                    }
                default:
                    {
                        if (operate == "ARGS")
                        {
                            try
                            {
                                Quartz.CronExpression expression = new Quartz.CronExpression(taskMsg.Args);
                                if (expression == null)
                                    return ServiceResult.OprateFailed($"表达式格式不正确!");

                            }
                            catch (Exception ex)
                            {
                                return ServiceResult.OprateFailed($"表达式格式不正确:{ex.Message}");
                            }

                        }
                        //发送消息并直接返回操作成功
                         TaskHelper.TaskHelper.PostMsg(taskMsg);
                        result.Message = $"操作成功";
                        break;
                    }
            }
            return result;
        }
        #endregion
    }
}
