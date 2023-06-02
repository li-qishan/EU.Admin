using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject;
using EU.Model.Loan;
using EU.Web.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.Loan
{
    public class BannerController : EU.Web.Controllers.BaseController<SmModule>
    {
        public BannerController(DataContext _context, IBaseCRUDVM<SmModule> BaseCrud) : base(_context, BaseCrud)
        {

        }

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
    }

    public class ContractDetail
    {
        public decimal ContractAmount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? CompleteDate { get; set; }
    }
}
