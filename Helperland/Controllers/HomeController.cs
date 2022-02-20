using Helperland.Models;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Helperland.Services;
using Helperland.Repository;


namespace Helperland.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contact_Us _contactus;
        private readonly IUserRepository _userRepository;

        public HomeController(ILogger<HomeController> logger, Contact_Us _contactus,IUserRepository userRepository)
        {
            _logger = logger;
            this. _contactus = _contactus;
            this._userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BookService()
        {
            return View();
        }
        [HttpPost]
        public JsonResult CheckPostalCode(string postalcode)
        {
            List<User> users = _userRepository.GetUserByPostalCode(postalcode);
            bool isServiceProviderAvailable=false;

            if (users.Any())
            {
                isServiceProviderAvailable=true;
            }
            return Json(isServiceProviderAvailable);
        }

        [HttpPost]
        public JsonResult AddCustomerAddress([FromBody] UserAddressViewModel userAddressViewModel)
        {
            string user = HttpContext.Session.GetString("User");
            SessionUser sessionUser = new SessionUser();
            if (user != null)
            {
                sessionUser = JsonConvert.DeserializeObject<SessionUser>(user);
            }

            UserAddress userAddress = new UserAddress
            {
                AddressLine1 = userAddressViewModel.StreetName,
                AddressLine2 = userAddressViewModel.HouseNumber,
                PostalCode = userAddressViewModel.PostalCode,
                City = userAddressViewModel.City,
                Mobile = userAddressViewModel.PhoneNumber,
                UserId = Convert.ToInt32(sessionUser.UserID)
            };
            userAddress = _userAddressRepository.AddUserAddress(userAddress);
            return Json(userAddress);
        }

        public IActionResult Price()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contactus(ContactUsViewModel contact)
        {
            _contactus.Add(contact);
            
            return RedirectToAction("Contact");
        }

        public IActionResult BecomeAHelper()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
