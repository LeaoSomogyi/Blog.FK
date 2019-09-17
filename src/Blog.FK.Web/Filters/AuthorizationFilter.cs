using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Blog.FK.Web.Filters
{
    /// <summary>
    /// Authorization Filter to validate token on request
    /// </summary>
    public class AuthorizationFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Called asynchronously before the action, check if Token is still valid
        /// </summary>
        /// <param name="context">The Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext.</param>
        /// <param name="next">The Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate. 
        /// Invoked to execute the next action filter or the action itself.</param>
        /// <returns>A System.Threading.Tasks.Task that on completion indicates the filter has executed</returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.HttpContext.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                executedContext.HttpContext.Response.Redirect("Admin/Login");
            }
        }
    }
}
