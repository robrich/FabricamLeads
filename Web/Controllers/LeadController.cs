using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricam.Web.Core.DataAccess;
using Fabricam.Web.Core.Repositories;
using Fabricam.Web.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fabricam.Web.Controllers
{
    [Authorize]
    public class LeadController : Controller
    {
        private readonly ICurrentUserService currentUserService;
        private readonly ILeadRepository leadRepository;
        private readonly ILoadLeadsService loadLeadsService;

        public LeadController(ILeadRepository LeadRepository, ILoadLeadsService LoadLeadsService, ICurrentUserService CurrentUserService) {
            this.leadRepository = LeadRepository ?? throw new ArgumentNullException(nameof(LeadRepository));
            this.loadLeadsService = LoadLeadsService ?? throw new ArgumentNullException(nameof(LoadLeadsService));
            this.currentUserService = CurrentUserService ?? throw new ArgumentNullException(nameof(CurrentUserService));
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JavaScriptTests()
        {
            return View();
        }

        public async Task<IActionResult> LoadLeads()
        {
            const int count = 10; // TODO: do users need to pass this as parameter?
            await this.loadLeadsService.GetMoreLeads(count);
            return Json(new {Success = true});
        }

        public async Task<IActionResult> GetNextLead()
        {
            int currentUserId = this.currentUserService.GetCurrentUserId(HttpContext) ?? 0;
            Lead nextLead = await this.leadRepository.GetNextLeadAsync(currentUserId);
            return Json(new {Success = true, Lead = nextLead});
        }

    }
}
