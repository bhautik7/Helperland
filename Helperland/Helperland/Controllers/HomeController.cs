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
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Helperland.Enums;

namespace Helperland.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Contact_Us _contactus;
        private readonly IUserRepository _userRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IServiceRequestAddressRepository _serviceRequestAddressRepository;
        private readonly IServiceRequestExtraRepository _serviceRequestExtraRepository;

        public HomeController(ILogger<HomeController> logger, Contact_Us _contactus,IUserRepository userRepository,
                                                            IUserAddressRepository userAddressRepository,IServiceRequestRepository serviceRequestRepository,
                                                            IServiceRequestAddressRepository serviceRequestAddressRepository,IServiceRequestExtraRepository serviceRequestExtraRepository)
        {
            _logger = logger;
            this. _contactus = _contactus;
            this._userRepository = userRepository;
            this._userAddressRepository=userAddressRepository;
            this._serviceRequestRepository = serviceRequestRepository;
            this._serviceRequestAddressRepository = serviceRequestAddressRepository;
            this._serviceRequestExtraRepository = serviceRequestExtraRepository;
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
           string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
           

            //State state = _stateRepository.GetStateByCityName(userAddressViewModel.City.ToString().Trim());

            UserAddress userAddress = new UserAddress
            {
                AddressLine1 = userAddressViewModel.StreetName.ToString().Trim(),
                AddressLine2 = userAddressViewModel.HouseNumber.ToString().Trim(),
                PostalCode = userAddressViewModel.PostalCode,
                City = userAddressViewModel.City.ToString().Trim(),
                Mobile = userAddressViewModel.PhoneNumber,
                UserId = Convert.ToInt32(userID)      
            };
            userAddress = _userAddressRepository.AddUserAddress(userAddress);
            return Json(userAddress);
        }

        public IActionResult GetCustomerAddressList(int userId)
        {
            List<UserAddress> userAddresseList = _userAddressRepository.GetUserAddress(userId);
            return View("BookServiceCustomerAddressList", userAddresseList);
        }
        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _userAddressRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }
        [HttpPost]
        public JsonResult BookCustomerServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = new ServiceRequest
            {
                UserId = model.UserId,
                ServiceId = 0,
                ServiceStartDate = Convert.ToDateTime(model.ServiceStartDate.ToString().Trim() + " " + model.ServiceStartTime.ToString().Trim()),
                ZipCode = model.PostalCode.ToString().Trim(),
                ServiceHourlyRate = model.ServiceHourlyRate,
                ServiceHours = model.ServiceHours,
                ExtraHours = model.ExtraHours,
                SubTotal = Convert.ToDecimal(model.SubTotal),
                Discount = 0,
                TotalCost = Convert.ToDecimal(model.TotalCost),
                Comments = model.Comments.ToString().Trim(),
                PaymentDue = false,
                HasPets = model.HasPets,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                Distance = 0,
                RecordVersion = Guid.NewGuid()
            };

            _serviceRequestRepository.Add(serviceRequest);

            model.ServiceRequestId = serviceRequest.ServiceRequestId;

            UserAddress userAddress = _userAddressRepository.SelectByPK(Convert.ToInt32(model.UserAddressId));

            ServiceRequestAddress serviceRequestAddress = new ServiceRequestAddress
            {
                ServiceRequestId = serviceRequest.ServiceRequestId,
                AddressLine1 = userAddress.AddressLine1,
                AddressLine2 = userAddress.AddressLine2,
                City = userAddress.City,
                State = userAddress.State,
                PostalCode = userAddress.PostalCode,
                Mobile = userAddress.Mobile,
                Email = userAddress.Email
            };

            _serviceRequestAddressRepository.Add(serviceRequestAddress);

            ServiceRequestExtra serviceRequestExtra = new ServiceRequestExtra
            {
                ServiceRequestId = serviceRequest.ServiceRequestId
            };

            foreach (string extraService in model.ExtraServicesName)
            {
                serviceRequestExtra.ServiceRequestExtraId = 0;
                serviceRequestExtra.ServiceExtraId = Convert.ToInt32((ExtraServiceEnum)System.Enum.Parse(typeof(ExtraServiceEnum), extraService));
                _serviceRequestExtraRepository.Add(serviceRequestExtra);
            }

            return Json(model);
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
