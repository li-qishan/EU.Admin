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
    public class ProjectImplementController : BaseController<ProjectImplement>
    {
        public ProjectImplementController(DataContext _context, BaseCRUDVM<ProjectImplement> BaseCrud) : base(_context, BaseCrud)
        {
        }
    }
}
