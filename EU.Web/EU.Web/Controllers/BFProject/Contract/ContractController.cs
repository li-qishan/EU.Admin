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
    public class ContractController : BaseController<BFContract>
    {
        public ContractController(DataContext _context, IBaseCRUDVM<BFContract> BaseCrud) : base(_context, BaseCrud)
        {

        }
    }
}
