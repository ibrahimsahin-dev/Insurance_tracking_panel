using InsuranceTrancking.Models;
using System.Linq;
using System.Web.Mvc;

namespace InsuranceTrancking.Controllers
{
    public class LoginController : Controller
    {
        private Model1 db = new Model1();

        public ActionResult Loginpage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Loginpage(string Email, string Password)
        {
            var admin = db.adminPanels.SingleOrDefault(u => u.mail == Email && u.adminpassword == Password);

            if (admin == null)
            {
                TempData["AlertMessage"] = "Invalid email or password.";
                return View();
            }
            else
            {
                Session["User"] = admin.adminID; // Store user ID in session
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Clear all session data
            return RedirectToAction("Loginpage", "Login");
        }
    }
}
