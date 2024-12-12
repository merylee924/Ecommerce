using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Tp_Panier.Filtre
{
    public class IsAdmin :ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("Role") == null && context.HttpContext.Session.GetString("Role") != "Admin")
            {
                context.Result = new RedirectResult("/User/Login");
            }


        }
    }
}
