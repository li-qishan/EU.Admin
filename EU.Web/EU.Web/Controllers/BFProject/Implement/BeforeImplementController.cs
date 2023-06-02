using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EU.DataAccess;
using EU.Domain;
using EU.Domain.Repositories;
using EU.Model.BFProject;
using Microsoft.AspNetCore.Mvc;

namespace EU.Web.Controllers.BFProject.Implement
{
    public class BeforeImplementController : BaseController<BeforeImplement>
    {
        public BeforeImplementController(DataContext _context, BaseCRUDVM<BeforeImplement> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
