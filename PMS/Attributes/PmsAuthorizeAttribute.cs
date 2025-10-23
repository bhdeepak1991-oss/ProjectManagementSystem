using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PMS.Helpers;

namespace PMS.Attributes
{
    public class PmsAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;

            var userId = httpContext.GetUserId();

            var routeData = context.ActionDescriptor.RouteValues;

            var controller = routeData["controller"];

            var action = routeData["action"];

            if (string.Equals(controller, "Dashboard", StringComparison.OrdinalIgnoreCase) &&
                            string.Equals(action, "ProjectSelection", StringComparison.OrdinalIgnoreCase))
            {
                if (userId is null)
                {
                    context.Result = new RedirectToActionResult("Authenticate", "UserManagement", null);

                    return;
                }
            }

            var projectId = httpContext.GetProjectId();

            if (userId is null && projectId is null)
            {
                context.Result = new RedirectToActionResult("Authenticate", "UserManagement", null);
                return;

            }
            else if (userId is null && projectId is not null)
            {
                context.Result = new RedirectToActionResult("Authenticate", "UserManagement", null);
                return;
            }
            else if (userId is not null && projectId is  null)
            {
                context.Result = new RedirectToActionResult("ProjectSelection", "Dashboard", null);
                return;
            }
        }
    }
}
