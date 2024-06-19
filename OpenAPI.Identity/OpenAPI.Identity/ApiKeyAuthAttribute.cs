using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using OpenAPI.Identity.Data;

namespace OpenAPI.Identity
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    public class ApiKeyAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("apiKey", out var apiKey) ||
                !context.HttpContext.Request.Headers.TryGetValue("apiSecret", out var apiSecret))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Get the database context service
            var dbContext = context.HttpContext.RequestServices.GetService<ApplicationDbContext>();
            if (dbContext == null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            // Ensure a database query is thread-safe
            var company = dbContext.Companies.FirstOrDefault(c => c.APIKey == apiKey.ToString() && c.APISecret == apiSecret.ToString());

            if (company == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            base.OnActionExecuting(context);
        }
    }

}
