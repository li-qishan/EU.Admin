﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using EU.DataAccess;
using EU.Model.Base;
using EU.Model.System;
using EU.Model.System.WorkFlow;
using EU.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ExpressionType = EU.Model.System.ExpressionType;
using EU.Domain;

namespace EU.Web.Controllers
{
    /// <summary>
    /// 基础路由类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Policy = "Permission")]
    //[AuthorizeJwt]
    public class BaseController<T> : ControllerBase where T : class
    {
        public readonly DataContext _context;
        public readonly IBaseCRUDVM<T> _BaseCrud;

        public BaseController(DataContext context, IBaseCRUDVM<T> BaseCrud)
        {
            _context = context;
            _BaseCrud = BaseCrud;
        }

        [NonAction]
        public void DoAddPrepare(T Entity)
        {
            //自动设定添加日期和添加人
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                BasePoco<Guid> ent = Entity as BasePoco<Guid>;
                if (string.IsNullOrEmpty(ent.CreatedBy.ToString()))
                {
                    ent.CreatedBy = !string.IsNullOrEmpty(User.Identity.Name) ? new Guid(User.Identity.Name) : Guid.Empty;
                }
                if (ent.CreatedTime == null)
                {
                    ent.CreatedTime = DateTime.Now;
                }
                ent.IsActive = true;
            }
        }

        [NonAction]
        public void DoEditPrepare(T Entity)
        {
            //自动设定修改日期和添加人
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                BasePoco<Guid> ent = Entity as BasePoco<Guid>;
                if (string.IsNullOrEmpty(ent.UpdateBy.ToString()))
                {
                    ent.UpdateBy = new Guid(User.Identity.Name);
                }
                if (ent.UpdateBy == null)
                {
                    ent.UpdateTime = DateTime.Now;
                }
            }
        }

        [HttpGet]
        public virtual IActionResult GetPageList(string paramData, string sorter = "{}", string filter = "{}", string parentColumn = null, string parentId = null)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;
            int current = 1;
            int pageSize = 20;
            int total = 0;

            string defaultSorter = "TaxisNo";
            string sortType = string.Empty;

            try
            {
                IQueryable<T> query = null;
                query = _context.Set<T>();


                var searchParam = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(paramData);
                var lamadaExtention = new LamadaExtention<T>();

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

                    if (item.Key == "_timestamp")
                    {
                        continue;
                    }
                    string type = typeof(T).GetProperties().Where(x => x.Name.ToLower() == item.Key.ToLower())
                                      .FirstOrDefault()
                                      ?.PropertyType.GetGenericArguments().FirstOrDefault()
                                      ?.Name ?? typeof(T).GetProperties().Where(x => x.Name.ToLower() == item.Key.ToLower())
                                      .FirstOrDefault()
                                      ?.PropertyType.Name;
                    if (item.Value.GetType().Name == "JArray")
                    {
                        var jArray = JArray.Parse(item.Value.ToString());
                        if (type == "DateTime" && jArray.Count == 2)
                        {
                            lamadaExtention.GetExpression(item.Key, DateTime.Parse(jArray.First.ToString()), ExpressionType.GreaterThanOrEqual);
                            lamadaExtention.GetExpression(item.Key, DateTime.Parse(jArray.Last.ToString()), ExpressionType.LessThanOrEqual);
                        }
                    }
                    else
                    {
                        lamadaExtention.GetExpression(item.Key, item.Value.ToString().Trim(), GetExpressionType(type));
                    }
                }
                lamadaExtention.GetExpression("IsDeleted", "false", ExpressionType.Equal);
                if (!string.IsNullOrEmpty(parentId) && !string.IsNullOrEmpty(parentColumn))
                    lamadaExtention.GetExpression(parentColumn, Guid.Parse(parentId), ExpressionType.Equal);

                if (lamadaExtention.GetLambda() != null)
                    query = query.Where(lamadaExtention.GetLambda());

                var sorterParam = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(sorter);
                if (sorterParam.Count > 0)
                {
                    foreach (var item in sorterParam)
                    {
                        string type = typeof(T).GetProperties().Where(x => x.Name.ToLower() == item.Key.ToLower())
                                          .FirstOrDefault()
                                          ?.PropertyType.GetGenericArguments().FirstOrDefault()
                            ?.Name ?? typeof(T).GetProperties().Where(x => x.Name.ToLower() == item.Key.ToLower())
                                           .FirstOrDefault()
                                           ?.PropertyType.Name;

                        #region 排序
                        if (type == "Int32")
                        {
                            if (item.Value == "ascend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderBy(LamadaExtention<T>.SortLambda<T, int>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query.OrderBy(LamadaExtention<T>.SortLambda<T, int>(defaultSorter, item.Key));
                                }
                            }
                            else if (item.Value == "descend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, int>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, int>(defaultSorter, item.Key));
                                }
                            }
                        }
                        else if (type == "Decimal")
                        {
                            if (item.Value == "ascend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderBy(LamadaExtention<T>.SortLambda<T, decimal>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query
                                        .OrderBy(LamadaExtention<T>.SortLambda<T, decimal>(defaultSorter, item.Key));
                                }
                            }
                            else if (item.Value == "descend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, decimal>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, decimal>(defaultSorter, item.Key));
                                }
                            }
                        }
                        else if (type == "DateTime")
                        {
                            if (item.Value == "ascend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderBy(LamadaExtention<T>.SortLambda<T, DateTime>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query
                                        .OrderBy(LamadaExtention<T>.SortLambda<T, DateTime>(defaultSorter, item.Key));
                                }
                            }
                            else if (item.Value == "descend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, DateTime>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, DateTime>(defaultSorter, item.Key));
                                }
                            }
                        }
                        else
                        {
                            if (item.Value == "ascend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderBy(LamadaExtention<T>.SortLambda<T, string>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query
                                        .OrderBy(LamadaExtention<T>.SortLambda<T, string>(defaultSorter, item.Key));
                                }
                            }
                            else if (item.Value == "descend")
                            {
                                if (lamadaExtention.GetLambda() != null)
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, string>(defaultSorter, item.Key));
                                }
                                else
                                {
                                    query = query.OrderByDescending(LamadaExtention<T>.SortLambda<T, string>(defaultSorter, item.Key));
                                }
                            }
                        }
                        #endregion
                    }
                }

                obj.data = query.Skip((current - 1) * pageSize).Take(pageSize).ToList();


                total = query.Count();

                status = "ok";
                message = "查询成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.current = current;
            obj.pageSize = pageSize;
            obj.total = total;
            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [HttpGet]
        public virtual IActionResult GetById(Guid Id)
        {
            try
            {
                return Ok(_BaseCrud.GetById(Id));
            }
            catch (Exception E)
            {
                throw E;
            }
        }

        [HttpPost]
        public virtual IActionResult Add(T Model)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                DoAddPrepare(Model);
                _BaseCrud.DoAdd(Model);

                obj.Id = Model.GetType().GetProperties().Where(x => x.Name.ToLower() == "id").FirstOrDefault()
                    ?.GetValue(Model).ToString();

                status = "ok";
                message = "添加成功！";
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
        public virtual IActionResult Update(dynamic modelModify)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Update<T>(modelModify);
                _context.SaveChanges();
                //DoEditPrepare(Model);
                //_BaseCrud.DoUpdate(Model);

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

        [HttpGet]
        public virtual IActionResult Delete(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                _BaseCrud.DoDelete(Id);

                status = "ok";
                message = "删除成功！";
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
        public virtual IActionResult BatchDelete(List<T> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                for (int i = 0; i < entryList.Count; i++)
                {
                    var navigations = _context.Entry(entryList[i]).Navigations.ToList();
                    for (int n = 0; n < navigations.Count; n++)
                    {
                        navigations[n].CurrentValue = null;
                    }

                    PersistPoco ent = entryList[i] as PersistPoco;
                    ent.IsDeleted = true;

                    _context.Update(entryList[i]);
                }
                _context.SaveChanges();

                status = "ok";
                message = "批量删除成功！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        #region 提交
        [HttpPost]
        public virtual IActionResult SubmitAudit(Guid moduleId, List<T> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //获取模块信息
                var moduleInfo = _context.SmModules.Where(x => x.IsDeleted == false && x.ID == moduleId)
                    .SingleOrDefault();

                for (int i = 0; i < entryList.Count; i++)
                {
                    BasePoco<Guid> ent = entryList[i] as BasePoco<Guid>;
                    if (ent.AuditStatus != "Add")
                    {
                        throw new Exception("包含非新增数据，请重新选择！");
                    }


                    //流程id
                    Guid FlowId = _context.SmProjectFlow
                        .Where(x => x.IsDeleted == false && x.IsActive == true && x.SmModuleId == moduleId)
                        .SingleOrDefault().ID;

                    //该流程所有节点
                    var nodes = _context.SmNodes.Where(x => x.IsDeleted == false && x.SmProjectFlowId == FlowId).OrderBy(x => x.index)
                        .ToList();

                    //该流程所有的线
                    var edges = _context.SmEdges.Where(x => x.IsDeleted == false && x.SmProjectFlowId == FlowId)
                        .OrderBy(x => x.index).ToList();

                    //当前节点信息
                    SmNode currentNodeInfo = nodes.Where(x => x.IsDeleted == false && x.label == "Start").SingleOrDefault();

                    #region 保存节点信息
                    //ent.CurrentNode = currentNodeInfo.nodeid;
                    ent.AuditStatus = "Auditing";

                    var navigations = _context.Entry(entryList[i]).Navigations.ToList();
                    for (int n = 0; n < navigations.Count; n++)
                    {
                        navigations[n].CurrentValue = null;
                    }

                    _context.Update(entryList[i]);
                    #endregion

                    //获取下一个节点
                    string nextNodeid = string.Empty;
                    //source为当前节点的线
                    List<SmEdge> nextEdges = edges.Where(x => x.source == currentNodeInfo.nodeid).ToList();
                    if (nextEdges.Count == 1)
                    {
                        nextNodeid = nextEdges[0].target;
                    }
                    else if (nextEdges.Count > 1)
                    {
                        for (int n = 0; n < nextEdges.Count; n++)
                        {
                            var conditionField = nextEdges[n].ConditionField;
                            var condition = nextEdges[n].Condition;
                            var conditionValue = nextEdges[n].ConditionValue;

                            IQueryable<T> query = null;
                            var lamadaExtention = new LamadaExtention<T>();

                            lamadaExtention.GetExpression("ID", ent.ID, ExpressionType.Equal);
                            lamadaExtention.GetExpression(conditionField, conditionValue, GetExpressionType(condition));
                            var count = query.Where(lamadaExtention.GetLambda()).Count();
                            if (count > 0)
                            {
                                nextNodeid = nextEdges[n].target;
                            }
                        }
                    }

                    //获取下一节点的用户
                    string roles = nodes.Where(x => x.nodeid == nextNodeid).SingleOrDefault().role;
                    var users = _context.SmUserRole.Where(x => x.IsDeleted == false && roles.Contains(x.SmRoleId.ToString())).Select(y => y.SmUserId).Distinct()
                        .ToList();

                    //获取数据创建人姓名
                    var userName = _context.SmUsers.Where(x => x.ID == ent.CreatedBy).SingleOrDefault().UserName;

                    #region 待办事项
                    for (int n = 0; n < users.Count; n++)
                    {

                        SmSchedule smSchedule = new SmSchedule();
                        smSchedule.UserId = users[n];
                        smSchedule.Title = moduleInfo.ModuleName;
                        smSchedule.Status = "Auditing";
                        smSchedule.ModuleId = moduleId;
                        smSchedule.MasterId = ent.ID;
                        smSchedule.Content = "由【" + userName + "】于【" + DateTime.Now + "】提交，请您尽快处理！";
                        smSchedule.UpdateBy = new Guid(User.Identity.Name);
                        smSchedule.UpdateTime = DateTime.Now;
                        _context.Add(smSchedule);
                    }
                    #endregion
                }
                _context.SaveChanges();

                status = "ok";
                message = "已提交！";
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

        #region 审批

        [HttpPost]
        public virtual IActionResult Audit(Guid moduleId, List<T> entryList)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                //获取模块信息
                var moduleInfo = _context.SmModules.Where(x => x.IsDeleted == false && x.ID == moduleId)
                    .SingleOrDefault();

                for (int i = 0; i < entryList.Count; i++)
                {
                    BasePoco<Guid> ent = entryList[i] as BasePoco<Guid>;
                    if (ent.AuditStatus != "Auditing")
                    {
                        throw new Exception("包含非“审批中”的数据，请重新选择！");
                    }

                    var currentNode = ent.CurrentNode;

                    //流程id
                    Guid FlowId = _context.SmProjectFlow
                        .Where(x => x.IsDeleted == false && x.IsActive == true && x.SmModuleId == moduleId)
                        .SingleOrDefault().ID;

                    //该流程所有节点
                    var nodes = _context.SmNodes.Where(x => x.IsDeleted == false && x.SmProjectFlowId == FlowId).OrderBy(x => x.index)
                        .ToList();

                    //该流程所有的线
                    var edges = _context.SmEdges.Where(x => x.IsDeleted == false && x.SmProjectFlowId == FlowId)
                        .OrderBy(x => x.index).ToList();

                    //当前节点信息
                    SmNode currentNodeInfo = nodes.Where(x => x.IsDeleted == false && x.nodeid == currentNode).SingleOrDefault();

                    #region 审批
                    //获取审批的节点
                    string nextNodeid = string.Empty;
                    //source为当前节点的线
                    List<SmEdge> nextEdges = edges.Where(x => x.source == currentNodeInfo.nodeid).ToList();
                    if (nextEdges.Count == 1)
                    {
                        nextNodeid = nextEdges[0].target;
                    }
                    else if (nextEdges.Count > 1)
                    {
                        for (int n = 0; n < nextEdges.Count; n++)
                        {
                            var conditionField = nextEdges[n].ConditionField;
                            var condition = nextEdges[n].Condition;
                            var conditionValue = nextEdges[n].ConditionValue;

                            IQueryable<T> query = null;
                            query = _context.Set<T>();

                            var lamadaExtention = new LamadaExtention<T>();

                            lamadaExtention.GetExpression("ID", ent.ID, ExpressionType.Equal);
                            lamadaExtention.GetExpression(conditionField, conditionValue, GetExpressionType(condition));
                            var count = query.Where(lamadaExtention.GetLambda()).Count();
                            if (count > 0)
                            {
                                nextNodeid = nextEdges[n].target;
                            }
                        }
                    }


                    #endregion

                    #region 向下一环节添加待办事项
                    //获取审批后下一节点
                    string nextnextNodeid = string.Empty;
                    //source为当前节点的线
                    List<SmEdge> nextnextEdges = edges.Where(x => x.source == nextNodeid).ToList();
                    if (nextnextEdges.Count == 1)
                    {
                        nextnextNodeid = nextnextEdges[0].target;
                    }
                    else if (nextnextEdges.Count > 1)
                    {
                        for (int n = 0; n < nextnextEdges.Count; n++)
                        {
                            var conditionField = nextnextEdges[n].ConditionField;
                            var condition = nextnextEdges[n].Condition;
                            var conditionValue = nextnextEdges[n].ConditionValue;

                            IQueryable<T> query = null;
                            query = _context.Set<T>();

                            var lamadaExtention = new LamadaExtention<T>();

                            lamadaExtention.GetExpression("ID", ent.ID, ExpressionType.Equal);
                            lamadaExtention.GetExpression(conditionField, conditionValue, GetExpressionType(condition));
                            var count = query.Where(lamadaExtention.GetLambda()).Count();
                            if (count > 0)
                            {
                                nextnextNodeid = nextnextEdges[n].target;
                            }
                        }
                    }

                    ent.CurrentNode = nextNodeid;
                    if (!string.IsNullOrEmpty(nextnextNodeid))
                    {
                        //审批后下一个节点信息
                        SmNode nextnextNodeInfo = nodes.Where(x => x.IsDeleted == false && x.nodeid == nextnextNodeid).SingleOrDefault();
                        if (nextnextNodeInfo.label == "End")
                        {
                            ent.CurrentNode = "End";
                            ent.AuditStatus = "ComplateAudit";
                        }
                    }

                    var navigations = _context.Entry(entryList[i]).Navigations.ToList();
                    for (int n = 0; n < navigations.Count; n++)
                    {
                        navigations[n].CurrentValue = null;
                    }
                    _context.Update(entryList[i]);

                    string roles = nodes.Where(x => x.nodeid == nextnextNodeid).SingleOrDefault().role;
                    var users = _context.SmUserRole
                        .Where(x => x.IsDeleted == false && roles.Contains(x.SmRoleId.ToString()))
                        .Select(y => y.SmUserId).Distinct()
                        .ToList();

                    //获取数据上次修改人姓名
                    var userName = _context.SmUsers.Where(x => x.ID == ent.UpdateBy).SingleOrDefault().UserName;

                    for (int n = 0; n < users.Count; n++)
                    {
                        SmSchedule smSchedule = new SmSchedule();
                        smSchedule.UserId = users[n];
                        smSchedule.Title = moduleInfo.ModuleName;
                        smSchedule.Status = "Auditing";
                        smSchedule.Path = moduleInfo.RoutePath + "/FormPage";
                        smSchedule.ModuleId = moduleId;
                        smSchedule.MasterId = ent.ID;
                        smSchedule.Content = "【" + userName + "】于【" + DateTime.Now + "】审核转入，请您尽快处理！"; ;
                        smSchedule.UpdateBy = new Guid(User.Identity.Name);
                        smSchedule.UpdateTime = DateTime.Now;
                        _context.Add(smSchedule);
                    }
                    #endregion

                }

                _context.SaveChanges();

                status = "ok";
                message = "审批完成！";
            }
            catch (Exception E)
            {
                message = E.Message;
            }

            obj.status = status;
            obj.message = message;
            return Ok(obj);
        }

        [NonAction]
        public ExpressionType GetExpressionType(string conditionValue)
        {
            switch (conditionValue)
            {
                case "String":
                    {
                        return ExpressionType.Contains;
                    }
                case "Contains":
                    {
                        return ExpressionType.Contains;
                    }
                case "Equal":
                    {
                        return ExpressionType.Equal;
                    }
                case "GreaterThan":
                    {
                        return ExpressionType.GreaterThan;
                    }
                case "GreaterThanOrEqual":
                    {
                        return ExpressionType.GreaterThanOrEqual;
                    }
                case "LessThan":
                    {
                        return ExpressionType.LessThan;
                    }
                case "LessThanOrEqual":
                    {
                        return ExpressionType.LessThanOrEqual;
                    }
                default:
                    {
                        return ExpressionType.Equal;
                    }
            }
        }
        #endregion

        #region 动态更新表字段
        /// <summary>
        /// 更新指定实体
        /// </summary>
        /// <typeparam name="T">数据表实体Model模型</typeparam>
        /// <param name="modelNew">动态Json数据</param>
        protected virtual void Update<T>(dynamic modelNew)
        {
            //序列化动态Json为字符串
            string json = modelNew.ToString();

            //反序列化为数据表中的实体对象
            T model = JsonConvert.DeserializeObject<T>(json);

            //自动设定修改日期和添加人
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                BasePoco<Guid> ent = model as BasePoco<Guid>;
                if (string.IsNullOrEmpty(ent.UpdateBy.ToString()))
                {
                    ent.UpdateBy = new Guid(User.Identity.Name);
                }
                if (ent.UpdateTime == null)
                {
                    ent.UpdateTime = DateTime.Now;
                }
            }

            //把状态全部变为不可更改
            _context.Entry(model).State = EntityState.Unchanged;

            //反序列化为动态对象中的属性
            var jsonModel = JsonConvert.DeserializeObject<dynamic>(json);

            //定义一个List来添加属性
            List<string> listName = new List<string>();

            //动态添加要修改的字段
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                //如果EF表中有实体对象，则排除，否则更新会报错,保留枚举
                if ((info.PropertyType.IsClass && info.PropertyType == typeof(String)) || info.PropertyType.IsClass == false)
                {
                    //解决大小写问题
                    foreach (var property in jsonModel)
                    {
                        if (info.Name.ToLower().Trim() == property.Name.ToLower().Trim())
                        {
                            listName.Add(info.Name);
                        }
                    }
                }
            }
            //寻找主键
            PropertyInfo pkProp = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();

            //遍历修改,并排除主键
            foreach (string Name in listName)
            {
                if (Name.ToLower() != pkProp.Name.ToLower())
                {
                    _context.Entry(model).Property(Name).IsModified = true;
                }
            }

            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                _context.Entry(model).Property("UpdateBy").IsModified = true;
                _context.Entry(model).Property("UpdateTime").IsModified = true;
            }

            if (typeof(T).GetTypeInfo().Name == "SmUser")
            {
                SmUser ent = model as SmUser;
                if (!string.IsNullOrEmpty(ent.PassWord))
                {
                    ent.PassWord = Utility.GetMD5String(ent.PassWord);
                    _context.Entry(model).Property("PassWord").IsModified = true;
                }
            }

            //return db.SaveChanges();
        }

        /// <summary>
        /// 更新指定实体,不更新指定字段
        /// </summary>
        /// <typeparam name="T">数据表实体Model模型</typeparam>
        /// <param name="modelNew">动态Json数据</param>
        /// <param name="fieldProNames">不更新的字段列表数组</param>
        protected virtual void Update<T>(dynamic modelNew, string fieldProNames)
        {
            //序列化动态Json为字符串
            string json = modelNew.ToString();

            //反序列化为数据表中的实体对象
            T model = JsonConvert.DeserializeObject<T>(json);

            //自动设定修改日期和添加人
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                BasePoco<Guid> ent = model as BasePoco<Guid>;
                if (string.IsNullOrEmpty(ent.UpdateBy.ToString()))
                {
                    ent.UpdateBy = new Guid(User.Identity.Name);
                }
                if (ent.UpdateTime == null)
                {
                    ent.UpdateTime = DateTime.Now;
                }
            }

            //把状态全部变为不可更改
            _context.Entry(model).State = EntityState.Unchanged;

            //反序列化为动态对象中的属性
            var jsonModel = JsonConvert.DeserializeObject<dynamic>(json);

            //定义一个List来添加属性
            List<string> listName = new List<string>();

            //动态添加要修改的字段
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                //如果EF表中有实体对象，则排除，否则更新会报错,保留枚举
                if ((info.PropertyType.IsClass && info.PropertyType == typeof(String)) || info.PropertyType.IsClass == false)
                {
                    //解决大小写问题
                    foreach (var property in jsonModel)
                    {
                        if (info.Name.ToLower().Trim() == property.Name.ToLower().Trim())
                        {
                            listName.Add(info.Name);
                        }
                    }
                }
            }//寻找主键
            PropertyInfo pkProp = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();

            //遍历修改,并排除主键
            foreach (string Name in listName)
            {
                if (Name.ToLower() != pkProp.Name.ToLower() && !fieldProNames.Split(",").Select(n => n.ToLower()).Contains(Name.ToLower()))
                {
                    _context.Entry(model).Property(Name).IsModified = true;
                }
            }
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                _context.Entry(model).Property("UpdateBy").IsModified = true;
                _context.Entry(model).Property("UpdateTime").IsModified = true;
            }

            if (typeof(T).GetTypeInfo().Name == "SmUser")
            {
                SmUser ent = model as SmUser;
                if (!string.IsNullOrEmpty(ent.PassWord))
                {
                    ent.PassWord = Utility.GetMD5String(ent.PassWord);
                    _context.Entry(model).Property("PassWord").IsModified = true;
                }
            }
            //return db.SaveChanges();
        }

        /// <summary>
        /// 更新指定实体,不更新指定字段,如果每个表中有相同不更新的字段，可以这样写
        /// </summary>
        /// <typeparam name="T">数据表实体Model模型</typeparam>
        /// <param name="modelNew">动态Json数据</param>
        protected virtual void UpdateSpecify<T>(dynamic modelNew)
        {
            //序列化动态Json为字符串
            string json = modelNew.ToString();

            //反序列化为数据表中的实体对象
            T model = JsonConvert.DeserializeObject<T>(json);

            //自动设定修改日期和添加人
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                BasePoco<Guid> ent = model as BasePoco<Guid>;
                if (string.IsNullOrEmpty(ent.UpdateBy.ToString()))
                {
                    ent.UpdateBy = new Guid(User.Identity.Name);
                }
                if (ent.UpdateTime == null)
                {
                    ent.UpdateTime = DateTime.Now;
                }
            }

            //把状态全部变为不可更改
            _context.Entry(model).State = EntityState.Unchanged;

            //反序列化为动态对象中的属性
            var jsonModel = JsonConvert.DeserializeObject<dynamic>(json);

            //定义一个List来添加属性
            List<string> listName = new List<string>();

            //定义不需要更新的字段
            string fieldProNames = "CreatedBy,CreatedTime,ImportDataId";

            //动态添加要修改的字段
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                //如果EF表中有实体对象，则排除，否则更新会报错,保留枚举
                if ((info.PropertyType.IsClass && info.PropertyType == typeof(String)) || info.PropertyType.IsClass == false)
                {
                    //解决大小写问题
                    foreach (var property in jsonModel && !fieldProNames.Split(",").Select(n => n.ToLower()).Contains(info.Name.ToLower()))
                    {
                        if (info.Name.ToLower().Trim() == property.Name.ToLower().Trim())
                        {
                            listName.Add(info.Name);
                        }
                    }
                }
            }//寻找主键
            PropertyInfo pkProp = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();

            //遍历修改,并排除主键
            foreach (string Name in listName)
            {
                if (Name.ToLower() != pkProp.Name.ToLower())
                {
                    _context.Entry(model).Property(Name).IsModified = true;
                }
            }
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                _context.Entry(model).Property("UpdateBy").IsModified = true;
                _context.Entry(model).Property("UpdateTime").IsModified = true;
            }

            if (typeof(T).GetTypeInfo().Name == "SmUser")
            {
                SmUser ent = model as SmUser;
                if (!string.IsNullOrEmpty(ent.PassWord))
                {
                    ent.PassWord = Utility.GetMD5String(ent.PassWord);
                    _context.Entry(model).Property("PassWord").IsModified = true;
                }
            }
            //return db.SaveChanges();
        }

        /// <summary>
        /// 更新指定实体,不更新指定字段,如果每个表中有相同不更新的字段，可以这样写,扩展方法
        /// </summary>
        /// <typeparam name="T">数据表实体Model模型</typeparam>
        /// <param name="modelNew">动态Json数据</param>
        /// <param name="fieldProNames">不更新的字段列表数组</param>
        protected virtual void UpdateSpecify<T>(dynamic modelNew, string fieldProNames)
        {
            //序列化动态Json为字符串
            string json = modelNew.ToString();

            //反序列化为数据表中的实体对象
            T model = JsonConvert.DeserializeObject<T>(json);

            //自动设定修改日期和添加人
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                BasePoco<Guid> ent = model as BasePoco<Guid>;
                if (string.IsNullOrEmpty(ent.UpdateBy.ToString()))
                {
                    ent.UpdateBy = new Guid(User.Identity.Name);
                }
                if (ent.UpdateTime == null)
                {
                    ent.UpdateTime = DateTime.Now;
                }
            }

            //把状态全部变为不可更改
            _context.Entry(model).State = EntityState.Unchanged;

            //反序列化为动态对象中的属性
            var jsonModel = JsonConvert.DeserializeObject<dynamic>(json);

            //定义一个List来添加属性
            List<string> listName = new List<string>();

            //定义不需要更新的字段
            string fieldProNameses = "CreatedBy,CreatedTime,ImportDataId," + fieldProNames;

            //动态添加要修改的字段
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                //如果EF表中有实体对象，则排除，否则更新会报错,保留枚举
                if ((info.PropertyType.IsClass && info.PropertyType == typeof(String)) || info.PropertyType.IsClass == false)
                {
                    //解决大小写问题
                    foreach (var property in jsonModel && !fieldProNameses.Split(",").Select(n => n.ToLower()).Contains(info.Name.ToLower()))
                    {
                        if (info.Name.ToLower().Trim() == property.Name.ToLower().Trim())
                        {
                            listName.Add(info.Name);
                        }
                    }
                }
            }

            //寻找主键
            PropertyInfo pkProp = typeof(T).GetProperties().Where(p => p.GetCustomAttributes(typeof(KeyAttribute), false).Length > 0).FirstOrDefault();

            //遍历修改,并排除主键
            foreach (string Name in listName)
            {
                if (Name.ToLower() != pkProp.Name.ToLower())
                {
                    _context.Entry(model).Property(Name).IsModified = true;
                }
            }
            if (typeof(T).GetTypeInfo().IsSubclassOf(typeof(BasePoco<Guid>)))
            {
                _context.Entry(model).Property("UpdateBy").IsModified = true;
                _context.Entry(model).Property("UpdateTime").IsModified = true;
            }

            if (typeof(T).GetTypeInfo().Name == "SmUser")
            {
                SmUser ent = model as SmUser;
                if (!string.IsNullOrEmpty(ent.PassWord))
                {
                    ent.PassWord = Utility.GetMD5String(ent.PassWord);
                    _context.Entry(model).Property("PassWord").IsModified = true;
                }
            }
            //return db.SaveChanges();
        }
        #endregion
    }
}
