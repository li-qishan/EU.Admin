using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Model.BFProject;
using Microsoft.AspNetCore.Mvc;

namespace EU.Web.Controllers.BFProject.Implement
{
    public class ImplementProcessController : BaseController<ImplementProcess>
    {
        public ImplementProcessController(DataContext _context, IBaseCRUDVM<ImplementProcess> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
