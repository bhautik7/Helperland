using Microsoft.AspNetCore.Mvc;

namespace Helperland.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserManagement()
        {
            return View();
        }
    }
}
