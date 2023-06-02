using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject;
using EU.Model.Loan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EU.Web.Controllers.Loan
{
     public class LoanTypeController : BaseController<LoanType>
    {
        public LoanTypeController(DataContext _context, IBaseCRUDVM<LoanType> BaseCrud) : base(_context, BaseCrud)
        {

        }
    }
}
