using Microsoft.AspNetCore.Mvc;

namespace TenexCars.Controllers.Account_Controller
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateNewPassword()
        {
            return View();
        }

    /*    [HttpPost]
        public ActionResult CreateNewPassword(string Password, string ConfirmPassword)
        {
            if (Password == ConfirmPassword && IsValidPassword(Password))
            {
                // Handle successful password creation
                ViewBag.Message = "Password creation successful, please login with your email and new password";
                return View();
            }
            else
            {
                // Handle validation errors
                ViewBag.Message = "Password creation failed. Please check the requirements and try again.";
                return View();
            }
        }

        private bool IsValidPassword(string password)
        {
            // Implement your password validation logic here
            return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsSymbol);
        }
*/
    }
}
