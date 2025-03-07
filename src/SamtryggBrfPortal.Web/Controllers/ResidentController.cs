using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SamtryggBrfPortal.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace SamtryggBrfPortal.Web.Controllers
{
    [AllowAnonymous]
    public class ResidentController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResidentController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Applications()
        {
            return View();
        }

        public IActionResult Documents()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}
