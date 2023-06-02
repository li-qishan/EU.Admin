using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject.Payment;
using EU.Web.Controllers.BFProject.Completion;
using Microsoft.AspNetCore.Mvc;

namespace EU.Web.Controllers.BFProject.Payment
{
    public class ApplyPayController : BaseController<ApplyPay>
    {
        public ApplyPayController(DataContext _context, IBaseCRUDVM<ApplyPay> BaseCrud) : base(_context, BaseCrud)
        {

        }

        [HttpGet]
        public IActionResult CalculationProjectAmount(Guid Id)
        {
            dynamic obj = new ExpandoObject();
            string status = "error";
            string message = string.Empty;

            try
            {
                ApplyPay applyPay = new ApplyPay();
                decimal TotalContractAmount = _context.Contract.Where(x => x.IsDeleted == false && x.ID == Id).Select(x => x.ContractAmount).SingleOrDefault();
                applyPay.TotalContractAmount = TotalContractAmount;

                decimal HasPayAmount = _context.ApplyPay.Where(x => x.IsDeleted == false && x.ContractId == Id)
                    .Select(x => x.ApplyPayAmount).Sum();
                applyPay.HasPayAmount = HasPayAmount;

                decimal NoPayAmount = TotalContractAmount - HasPayAmount;
                applyPay.NoPayAmount = NoPayAmount;

                applyPay.ProjectProgress = _context.ProjectImplement.Where(x => x.IsDeleted == false && x.ID == Id)
                    .Select(x => x.ImplementProgress).SingleOrDefault();

                applyPay.PayProgress = HasPayAmount / TotalContractAmount;

                obj.data = applyPay;

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
}
