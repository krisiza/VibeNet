using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VibeNet.Core.Interfaces;

namespace VibeNet.Attributes
{
    public class NotExistingUserAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var vibenetService = context.HttpContext.RequestServices.GetService<IVibeNetService>();

            if (vibenetService == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            if (context.ActionArguments.ContainsKey("userId"))
            {
                var userId = context.ActionArguments["userId"] as string;

                if (string.IsNullOrEmpty(userId) || await vibenetService.GetByIdentityIdAsync(userId) == null)
                {
                    context.Result = new StatusCodeResult(StatusCodes.Status400BadRequest);
                    return;
                }
            }

            await next();
        }
    }
}

