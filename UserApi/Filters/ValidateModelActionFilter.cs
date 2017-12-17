using System.Linq;
using Fabricam.UserApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fabricam.UserApi.Filters
{
    // TODO: consider adding as global action filter
    // Source: https://www.jerriepelser.com/blog/validation-response-aspnet-core-webapi/

    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                ValidationResponse response = new ValidationResponse
                {
                    Message = "Validation Failed",
                    Errors = (
                        from key in context.ModelState.Keys
                        from e in context.ModelState[key].Errors
                        select new ValidationError {
                            Field = key,
                            Message = e.ErrorMessage
                        }
                    ).ToList()
                };
                context.Result = new ValidationFailedResult(response);
            }
        }
    }

    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ValidationResponse Response)
            : base(Response)
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
