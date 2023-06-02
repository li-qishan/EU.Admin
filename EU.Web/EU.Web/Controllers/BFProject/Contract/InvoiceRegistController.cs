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
    public class InvoiceRegistController : BaseController<InvoiceRegist>
    {
        public InvoiceRegistController(DataContext _context, IBaseCRUDVM<InvoiceRegist> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
