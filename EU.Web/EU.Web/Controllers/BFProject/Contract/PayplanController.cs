using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject;
using Microsoft.AspNetCore.Mvc;

namespace EU.Web.Controllers.BFProject.Contract
{
    public class PayplanController : BaseController<Payplan>
    {
        public PayplanController(DataContext _context, IBaseCRUDVM<Payplan> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
