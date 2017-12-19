using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fabricam.Shared;
using Microsoft.AspNetCore.Http;

namespace Fabricam.Web.Core.Services
{
    public interface ICurrentUserService
    {
        int? GetCurrentUserId(HttpContext HttpContext);
    }

    public class CurrentUserService : ICurrentUserService {

        public int? GetCurrentUserId(HttpContext HttpContext)
        {
            if (HttpContext == null) {
                throw new ArgumentNullException(nameof(HttpContext));
            }
            return HttpContext.User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value.ToIntOrNull();
        }

    }
}
