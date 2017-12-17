using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fabricam.Web.Core.Models
{
    public class AuthenticateResult
    {
        public List<ValidationError> Errors { get; set; } = new List<ValidationError>();
        public bool Success => this.Errors == null || this.Errors.Count < 1;
        public ClaimsIdentity Identity { get; set; }
    }
}
