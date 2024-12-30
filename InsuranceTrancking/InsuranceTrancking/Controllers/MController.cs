using System.Web.Mvc;

namespace InsuranceTrancking.Controllers
{
    public class MController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["User"] == null)
            {
                filterContext.Result = RedirectToAction("Loginpage", "Login");
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
