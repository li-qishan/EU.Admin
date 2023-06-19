using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.System;
using EU.Model.System.CompanyStructure;
using EU.Model.System.WorkFlow;
using Microsoft.AspNetCore.Mvc;
using static EU.Core.Const.Consts;

namespace EU.Web.Controllers.System.WorkFlow
{
    [GlobalActionFilter, ApiExplorerSettings(GroupName = Grouping.Procedure)]
    public class SmProjectFlowController : BaseController1<SmProjectFlow>
    {
        public SmProjectFlowController(DataContext _context, IBaseCRUDVM<SmProjectFlow> BaseCrud) : base(_context, BaseCrud)
        {
        }

        #region 组织部门树
        [NonAction]
        public void LoopToAppendChildren(List<SmCompany> smCompanies, List<SmDepartment> smDepartments, DataTree dataTree)
        {
            List<DataTree> subItems = new List<DataTree>();
            if (dataTree.key == "All")
            {
                subItems = smCompanies.Select(y => new DataTree
                {
                    title = y.CompanyName,
                    key = y.ID.ToString().ToLower(),
                    isLeaf = smDepartments.Where(x => x.CompanyId == y.ID).Count() <= 0
                }).ToList();
            }
            else if (dataTree.isDepartment)
            {
                subItems = smDepartments.Where(x => x.DepartmentId == Guid.Parse(dataTree.key)).Select(y => new DataTree
                {
                    title = y.DepartmentName,
                    isDepartment = true,
                    key = y.ID.ToString().ToLower(),
                    isLeaf = smDepartments.Where(x => x.DepartmentId == y.ID).Count() <= 0
                }).ToList();
            }
            else if (!dataTree.isDepartment)
            {
                subItems = smDepartments.Where(x => x.CompanyId == Guid.Parse(dataTree.key) && x.DepartmentId == null).Select(y => new DataTree
                {
                    title = y.DepartmentName,
                    isDepartment = true,
                    key = y.ID.ToString().ToLower(),
                    isLeaf = smDepartments.Where(x => x.DepartmentId == y.ID).Count() <= 0
                }).ToList();
            }
            dataTree.children = new List<DataTree>();
            dataTree.children.AddRange(subItems);
            foreach (var subItem in subItems)
            {
                LoopToAppendChildren(null, smDepartments, subItem);
            }
        }

        [HttpGet]
        public IActionResult GetStructureTree()
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                DataTree dataTree = new DataTree();
                dataTree.key = "All";
                dataTree.title = "请选择";

                List<SmCompany> smCompanies = _context.SmCompany.Where(x => x.IsDeleted == false).ToList();
                List<SmDepartment> smDepartments = _context.SmDepartment.Where(x => x.IsDeleted == false).ToList();

                LoopToAppendChildren(smCompanies, smDepartments, dataTree);
                obj.data = dataTree;

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

        #region 保存流程图
        [HttpPost]
        public IActionResult SaveFlowData(SmFlowVM smFlowVm)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                Guid SmProjectFlowId = (Guid)(smFlowVm.edges.Count > 0 ? smFlowVm.edges[0].SmProjectFlowId :
                    smFlowVm.nodes.Count > 0 ? smFlowVm.nodes[0].SmProjectFlowId : null);

                //删除旧的流程
                var edgedata = _context.SmEdges.Where(x => x.SmProjectFlowId == SmProjectFlowId).ToList();
                for (int i = 0; i < edgedata.Count; i++)
                {
                    edgedata[i].IsDeleted = true;
                    _context.Update(edgedata[i]);
                }

                var nodedata = _context.SmNodes.Where(x => x.SmProjectFlowId == SmProjectFlowId).ToList();
                for (int i = 0; i < nodedata.Count; i++)
                {
                    nodedata[i].IsDeleted = true;
                    _context.Update(nodedata[i]);
                }

                //添加
                for (int i = 0; i < smFlowVm.edges.Count; i++)
                {
                    _context.Add(smFlowVm.edges[i]);
                }
                for (int i = 0; i < smFlowVm.nodes.Count; i++)
                {
                    if (smFlowVm.nodes[i].Roles != null)
                    {
                        string roles = string.Join(",", smFlowVm.nodes[i].Roles);
                        smFlowVm.nodes[i].role = roles;
                    }
                    _context.Add(smFlowVm.nodes[i]);
                }

                _context.SaveChanges();

                status = "ok";
                message = "流程图保存成功！";
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

        #region 获取流程图

        [HttpGet]
        public IActionResult GetFlowData(Guid StructureId)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                FlowDataVM flowDataVm = new FlowDataVM();
                flowDataVm.nodes = _context.SmNodes.Where(x => x.IsDeleted == false && x.SmProjectFlowId == StructureId).AsEnumerable().Select(y => new node
                {
                    color = y.color,
                    id = y.nodeid,
                    index = y.index,
                    label = y.label,
                    shape = y.shape,
                    size = y.size,
                    type = y.type,
                    x = Convert.ToDecimal(y.x),
                    y = Convert.ToDecimal(y.y),
                    RejectionType = y.RejectionType,
                    Roles = !string.IsNullOrWhiteSpace(y.role) ? y.role.Split(',').ToList() : new List<string>()
                }).ToList();
                flowDataVm.edges = _context.SmEdges.Where(x => x.IsDeleted == false && x.SmProjectFlowId == StructureId).Select(y => new edge
                {
                    id = y.edgeid,
                    index = y.index,
                    label = y.label,
                    shape = y.shape,
                    source = y.source,
                    sourceAnchor = y.sourceAnchor,
                    target = y.target,
                    targetAnchor = y.targetAnchor,
                    ConditionField = y.ConditionField,
                    Condition = y.Condition,
                    ConditionValue = y.ConditionValue
                }).ToList();
                obj.data = flowDataVm;

                status = "ok";
                message = "流程图查询成功！";
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
    public class DataTree
    {
        public string title { get; set; }

        public bool isDepartment { get; set; }

        public string key { get; set; }

        public bool isLeaf { get; set; }

        public List<DataTree> children { get; set; }
    }

    public class SmFlowVM
    {
        public List<SmNode> nodes { get; set; }
        public List<SmEdge> edges { get; set; }
    }

    public class FlowDataVM
    {
        public List<node> nodes { get; set; }
        public List<edge> edges { get; set; }
    }

    public class node
    {
        public string color { get; set; }

        public string id { get; set; }

        public string index { get; set; }

        public string label { get; set; }

        public string shape { get; set; }

        public string size { get; set; }

        public string type { get; set; }

        public decimal x { get; set; }

        public decimal y { get; set; }

        public string RejectionType { get; set; }

        public List<string> Roles { get; set; }
    }

    public class edge
    {
        public string id { get; set; }

        public string index { get; set; }

        public string label { get; set; }

        public string shape { get; set; }

        public string source { get; set; }

        public string sourceAnchor { get; set; }

        public string target { get; set; }

        public string targetAnchor { get; set; }

        public string ConditionField { get; set; }
        public string Condition { get; set; }
        public string ConditionValue { get; set; }
    }
}
