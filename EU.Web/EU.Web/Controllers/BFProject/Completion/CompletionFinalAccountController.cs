using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject;
using EU.Model.System;
using EU.Model.System.Privilege;
using EU.Model.System.WorkFlow;
using Microsoft.AspNetCore.Mvc;

namespace EU.Web.Controllers.BFProject.Completion
{
    public class CompletionFinalAccountController : BaseController<CompletionFinalAccount>
    {
        public CompletionFinalAccountController(DataContext _context, IBaseCRUDVM<CompletionFinalAccount> BaseCrud) : base(_context, BaseCrud)
        {

        }

        #region 获取表单显示权限

        [HttpGet]
        public IActionResult GetComFinalAccountPriv(Guid moduleId, Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            bool IsShowAudit = false;
            bool IsAudit = false;

            try
            {
                //数据信息
                var dataInfo = _context.CompletionFinalAccount.Where(x => x.ID == Id).SingleOrDefault();
                //当前节点
                string currentNode = dataInfo.CurrentNode;

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

                        IQueryable<Approval> query = null;
                        query = _context.Set<Approval>();

                        var lamadaExtention = new LamadaExtention<Approval>();

                        lamadaExtention.GetExpression("ID", Id, ExpressionType.Equal);
                        lamadaExtention.GetExpression(conditionField, conditionValue, GetExpressionType(condition));

                        var count = query.Where(lamadaExtention.GetLambda()).Count();
                        if (count > 0)
                        {
                            nextNodeid = nextEdges[n].target;
                        }
                    }
                }
                //下一节点的数据
                var nextNodeInfo = nodes.Where(x => x.nodeid == nextNodeid).SingleOrDefault();

                //当前登录用户
                string userId = User.Identity.Name;

                //当前用户的所有角色
                var roleList = _context.Set<SmUserRole>()
                    .Where(x => x.IsDeleted == false & x.SmUserId == Guid.Parse(userId))
                    .Join(_context.Set<SmRole>(), x => x.SmRoleId, y => y.ID, (x, y) => new { x, y })
                    .Where(z => z.y.IsDeleted == false && z.y.IsActive == true)
                    .Select(x => x.y.ID).ToList();

                #region 判断是否可以审核

                for (int i = 0; i < roleList.Count; i++)
                {
                    if (nextNodeInfo.role.Contains(roleList[i].ToString()))
                    {
                        IsAudit = true;
                        break;
                    }
                }
                #endregion

                //表单显示
                if (IsAudit)
                {
                    if (nextNodeInfo.label == "审计登记")
                    {
                        IsShowAudit = true;
                    }
                }

                obj.IsAudit = IsAudit;
                obj.IsShowAudit = IsShowAudit;

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

        #region 表单选择项目带出信息
        [HttpGet]
        public IActionResult GetContractDetailById(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                ContractDetail contractDetail = new ContractDetail();
                contractDetail.ContractAmount = _context.Contract.Where(x => x.ApprovalId == Id && x.IsDeleted == false)
                    .Sum(x => x.ContractAmount);
                contractDetail.StartDate = _context.Contract.Where(x => x.ApprovalId == Id && x.IsDeleted == false)
                    .Min(x => x.StartDate);
                contractDetail.CompleteDate = _context.Contract.Where(x => x.ApprovalId == Id && x.IsDeleted == false)
                    .Max(x => x.CompleteDate);

                obj.data = contractDetail;

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
        #endregion
    }

    public class ContractDetail
    {
        public decimal ContractAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompleteDate { get; set; }
    }
}
