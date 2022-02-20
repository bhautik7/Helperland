using Microsoft.AspNetCore.Mvc;

namespace Helperland.Controllers
{
    public class CustomerController : Controller
    {
      
        public IActionResult ServiceHistory()
        {
            return View();
        }
        
    }
}
