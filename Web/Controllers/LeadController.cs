using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fabricam.Web.Controllers
{
    [Authorize]
    public class LeadController : Controller
    {

        public IActionResult Index() {
            return View(); // !!!!!
        }

    }
}
