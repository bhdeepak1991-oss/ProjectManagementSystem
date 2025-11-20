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
            var routeData = context.ActionDescriptor.RouteValues;

            var controller = routeData["controller"]?.ToString();
            var action = routeData["action"]?.ToString();

            var userId = httpContext.GetUserId();
            var projectId = httpContext.GetProjectId();

            // -----------------------------
            // 1. Allow Login / Public Pages
            // -----------------------------
            if (IsPage(controller, action, "UserManagement", "Authenticate"))
            {
                return; // DO NOT BLOCK login page
            }

            if (IsPage(controller, action, "UserManagement", "Logout"))
            {
                return;
            }

            // -----------------------------
            // 2. Allow Project Selection 
            // -----------------------------
            if (IsPage(controller, action, "Dashboard", "ProjectSelection"))
            {
                if (userId is null)
                {
                    RedirectToLogin(context);
                    return;
                }
                return;
            }

            // --------------------------------------------
            // 3. User logged in BUT project not selected
            // --------------------------------------------
            if (userId is not null && projectId is null)
            {
                // Allow project list page to load
                if (IsPage(controller, action, "Project", "Index"))
                {
                    return;
                }

                // Otherwise redirect user to Project/Index
                context.Result = new RedirectToActionResult("Index", "Project", null);
                return;
            }

            // -----------------------------
            // 4. Global login check
            // -----------------------------
            if (userId is null)
            {
                RedirectToLogin(context);
                return;
            }

            // -----------------------------
            // 5. Global project check
            // -----------------------------
            if (projectId is null)
            {
                context.Result = new RedirectToActionResult("Index", "Project", null);
                return;
            }
        }

        private bool IsPage(string? controller, string? action,
                            string expectedController, string expectedAction)
        {
            return string.Equals(controller, expectedController, StringComparison.OrdinalIgnoreCase)
                && string.Equals(action, expectedAction, StringComparison.OrdinalIgnoreCase);
        }

        private void RedirectToLogin(AuthorizationFilterContext context)
        {
            context.Result = new RedirectToActionResult("Authenticate", "UserManagement", null);
        }
    }
}
