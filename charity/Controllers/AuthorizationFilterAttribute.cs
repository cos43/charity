using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace charity.Controllers;

public class AuthorizationFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // get current user
        var user = filterContext.HttpContext.Session.GetString("UserEmail");

        // If the user is not logged in, redirect to the login page
        if (user == null)
        {
            var redirectTarget = new RouteValueDictionary
            {
                { "action", "Login" },
                { "controller", "User" }
            };
            filterContext.Result = new RedirectToRouteResult(redirectTarget);
        }

        // check permission
        base.OnActionExecuting(filterContext);
    }
}