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

namespace EU.Web.Controllers.BFProject.Project
{
    public class ApprovalController : BaseController<Approval>
    {
        public ApprovalController(DataContext _context, IBaseCRUDVM<Approval> BaseCrud) : base(_context, BaseCrud)
        {

        }

        [HttpGet]
        public IActionResult GetApprovalPrivilege(Guid moduleId, Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            bool IsShowReason = false;
            bool IsShowDeclareLeader = false;
            bool IsShowEngineeringDepOne = false;
            bool IsShowEngineeringDepTwo = false;
            bool IsShowEngineeringDepartmentLeader = false;
            bool IsShowMainLeader = false;
            //bool IsUpdate = false;
            bool IsAudit = false;

            bool IsViewBasic = true;
            bool IsViewReason = true;
            bool IsViewDeclareLeader = true;
            bool IsViewEngineeringDepOne = true;
            bool IsViewEngineeringDepTwo = true;
            bool IsViewEngineeringDepartmentLeader = true;
            bool IsViewMainLeader = true;

            try
            {
                if (Id != Guid.Empty)
                {
                    //数据信息
                    var approvalData = _context.Approval.Where(x => x.ID == Id).SingleOrDefault();
                    //当前节点
                    string currentNode = approvalData.CurrentNode;

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
                        if (nextNodeInfo.label == "申报(部门/社区)负责人")
                        {
                            IsShowReason = true;
                            IsViewReason = false;
                        }
                        if (nextNodeInfo.label == "申报(部门/社区)分管领导")
                        {
                            IsShowReason = true;
                            IsShowDeclareLeader = true;
                            IsViewDeclareLeader = false;
                        }
                        if (nextNodeInfo.label == "工程部门1")
                        {
                            IsShowReason = true;
                            IsShowDeclareLeader = true;
                            IsShowEngineeringDepOne = true;
                            IsViewEngineeringDepOne = false;

                        }
                        if (nextNodeInfo.label == "工程部门2")
                        {
                            IsShowReason = true;
                            IsShowDeclareLeader = true;
                            IsShowEngineeringDepOne = true;
                            IsShowEngineeringDepTwo = true;
                            IsViewEngineeringDepTwo = false;
                        }
                        if (nextNodeInfo.label == "工程部门分管领导")
                        {
                            IsShowReason = true;
                            IsShowDeclareLeader = true;
                            IsShowEngineeringDepOne = true;
                            IsShowEngineeringDepTwo = true;
                            IsShowEngineeringDepartmentLeader = true;
                            IsViewEngineeringDepartmentLeader = false;
                        }
                        if (nextNodeInfo.label == "主要领导")
                        {
                            IsShowReason = true;
                            IsShowDeclareLeader = true;
                            IsShowEngineeringDepOne = true;
                            IsShowEngineeringDepTwo = true;
                            IsShowEngineeringDepartmentLeader = true;
                            IsShowMainLeader = true;
                            IsViewMainLeader = false;
                        }
                        if (nextNodeInfo.label == "End")
                        {
                            IsShowReason = true;
                            IsShowDeclareLeader = true;
                            IsShowEngineeringDepOne = true;
                            IsShowEngineeringDepTwo = true;
                            IsShowEngineeringDepartmentLeader = true;
                            IsShowMainLeader = true;
                        }
                    }
                }

                obj.IsShowReason = IsShowReason;
                obj.IsShowDeclareLeader = IsShowDeclareLeader;
                obj.IsShowEngineeringDepOne = IsShowEngineeringDepOne;
                obj.IsShowEngineeringDepTwo = IsShowEngineeringDepTwo;
                obj.IsShowEngineeringDepartmentLeader = IsShowEngineeringDepartmentLeader;
                obj.IsShowMainLeader = IsShowMainLeader;
                obj.IsAudit = IsAudit;

                obj.IsViewBasic = IsViewBasic;
                obj.IsViewReason = IsViewReason;
                obj.IsViewDeclareLeader = IsViewDeclareLeader;
                obj.IsViewEngineeringDepOne = IsViewEngineeringDepOne;
                obj.IsViewEngineeringDepTwo = IsViewEngineeringDepTwo;
                obj.IsViewEngineeringDepartmentLeader = IsViewEngineeringDepartmentLeader;
                obj.IsViewMainLeader = IsViewMainLeader;

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
    }
}
