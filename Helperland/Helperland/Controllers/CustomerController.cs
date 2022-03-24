using Microsoft.AspNetCore.Mvc;
using Helperland.Repository;
using Microsoft.Extensions.Configuration;
using Helperland.Models;
using System;
using Helperland.Enums;
using Helperland.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;

namespace Helperland.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ICustomerManagementRepository _customerManagementRepository;
        private readonly IConfiguration _configuration;
        private readonly HelperlandContext _helperlandContext;

        public CustomerController(ICustomerManagementRepository customerManagementRepository, IConfiguration configuration,
            HelperlandContext helperlandContext)
        {
            this._customerManagementRepository = customerManagementRepository;
            this._configuration = configuration;
            this._helperlandContext = helperlandContext;
        }
        public IActionResult ServiceHistory()
        {
            return View();
        }
        public IActionResult GetServiceHistoryList()
        {
            return View();
        }
        public IActionResult MyAccount()
        {
            return View();
        }
        public IActionResult DashBoard()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _customerManagementRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }

        [HttpPost]
        public IActionResult GetServiceRequestHistoryList()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var serviceRequestList = _customerManagementRepository.GetServiceRequestHistoryListByCustomerId(Convert.ToInt32(userId));

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceDateTime_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceDateTime_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "ServiceProvider_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);  //check once for sorting
                        break;
                    case "ServiceProvider_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);
                        break;
                    case "TotalCost_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.TotalCost);
                        break;
                    case "TotalCost_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.TotalCost);
                        break;
                    default:
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequestList.Count();
                var data = serviceRequestList.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    if (temp.ServiceProviderId != null)
                    {
                        temp.User = _customerManagementRepository.GetUserByPK(Convert.ToInt32(temp.ServiceProviderId));
                        temp.Rating= _customerManagementRepository.GetRatingsByServiceProviderId(temp.ServiceProviderId);
                    }
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetServiceProviderRatingFromServiceRequest(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _customerManagementRepository.GetServiceRequest(serviceRequestId);

            Rating rating = _customerManagementRepository.GetRatingsByServiceRequestId(serviceRequestId);

            RatingViewModel ratingViewModel = new RatingViewModel();

            if (rating != null)
            {
                ratingViewModel.RatingId = rating.RatingId;
                ratingViewModel.RatingFrom = rating.RatingFrom;
                ratingViewModel.RatingTo = rating.RatingTo;
                ratingViewModel.RatingDate = rating.RatingDate;
                ratingViewModel.Ratings = rating.Ratings;
                ratingViewModel.Comments = rating.Comments;
                ratingViewModel.RatingDate = rating.RatingDate;
                ratingViewModel.OnTimeArrival = rating.OnTimeArrival;
                ratingViewModel.Friendly = rating.Friendly;
                ratingViewModel.QualityOfService = rating.QualityOfService;
            }

            List<Rating> serviceProviderRating = _customerManagementRepository.GetRatingsByServiceProviderId(serviceRequest.ServiceProviderId);

            decimal totalRating = 0;
            ratingViewModel.ServiceProviderRating = 0;
            ratingViewModel.ServiceRequestId = serviceRequestId;

            foreach (Rating temp in serviceProviderRating)
            {
                totalRating = totalRating + temp.Ratings;
            }

            if (serviceProviderRating.Any())
            {
                ratingViewModel.ServiceProviderRating = totalRating / serviceProviderRating.Count;
                ratingViewModel.ServiceProviderRating = Math.Round(ratingViewModel.ServiceProviderRating * 10) / 10;
            }

            ratingViewModel.ServiceProvider = _customerManagementRepository.GetUserByPK(Convert.ToInt32(serviceRequest.ServiceProviderId));

            return Json(new SingleEntity<RatingViewModel> { Result = ratingViewModel, Status = "ok", ErrorMessage = null });
        }

        public JsonResult SubmitRatingFromCustomer([FromBody] RatingViewModel model)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Rating rating = new Rating()
            { 
                ServiceRequestId = model.ServiceRequestId,
                RatingFrom = Convert.ToInt32(userId),
                RatingTo = model.RatingTo,
                Ratings = model.Ratings,
                Comments = model.Comments,
                RatingDate = DateTime.Now,
                OnTimeArrival = model.OnTimeArrival,
                Friendly = model.Friendly,
                QualityOfService = model.QualityOfService
            };

            Console.WriteLine("after rating object");
            _customerManagementRepository.AddRating(rating);

            return Json(new SingleEntity<RatingViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }


        [HttpPost]
        public IActionResult GetCurrentServiceRequestList()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var serviceRequest = _customerManagementRepository.GetCurrentServiceRequestByCustomerId(Convert.ToInt32(userId));

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceDateTime_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceDateTime_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "ServiceProvider_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);  //check once for sorting
                        break;
                    case "ServiceProvider_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);
                        break;
                    case "TotalCost_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.TotalCost);
                        break;
                    case "TotalCost_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.TotalCost);
                        break;
                    default:
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequest.Count();
                var data = serviceRequest.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    if (temp.ServiceProviderId != null)
                    {
                        temp.Rating = _customerManagementRepository.GetRatingsByServiceProviderId(temp.ServiceProviderId);
                    }
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [HttpPost]
        public JsonResult GetServiceRequest(int serviceRequestId)
        {

            ServiceRequest serviceRequest = _customerManagementRepository.GetServiceRequest(serviceRequestId);
            String serviceRequestExtraName = "";

            foreach (ServiceRequestExtra serviceRequestExtra in serviceRequest.ServiceRequestExtra)
            {
                serviceRequestExtraName = serviceRequestExtraName + ", " + Enum.GetName(typeof(ExtraServiceEnum), serviceRequestExtra.ServiceExtraId);
            }
            Console.WriteLine(serviceRequestExtraName);

            if (serviceRequestExtraName.Length > 2)
            {
                serviceRequestExtraName = serviceRequestExtraName.Remove(0, 2);
            }
            else
            {
                serviceRequestExtraName = "-";
            }

            var jsonData = new { data = serviceRequest, extraServiceRequest = serviceRequestExtraName };
            return Json(jsonData);
        }

        [HttpPost]
        public JsonResult UpdateRescheduleServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _customerManagementRepository.GetServiceRequest(model.ServiceRequestId);

            Console.WriteLine(serviceRequest);

            DateTime newServiceRequestStartDateTime = Convert.ToDateTime(model.ServiceStartDate + " " + model.ServiceStartTime);
            DateTime newServiceRequestEndDateTime = newServiceRequestStartDateTime.AddMinutes(serviceRequest.ServiceHours * 60);

            if (serviceRequest.ServiceProviderId != null)
            {
                List<ServiceRequest> serviceRequestList = _customerManagementRepository.GetFutureServiceRequestByServiceProviderId(Convert.ToInt32(serviceRequest.ServiceProviderId));

                Boolean serviceRequestConflict = false;

                string errorMessage = "";

                foreach (ServiceRequest temp in serviceRequestList)
                {
                    if (serviceRequest.ServiceRequestId != temp.ServiceRequestId)
                    {
                        DateTime serviceRequestStartDateTime = temp.ServiceStartDate;
                        DateTime serviceRequestEndDateTime = serviceRequestStartDateTime.AddMinutes(temp.ServiceHours * 60);

                        if (serviceRequestStartDateTime <= newServiceRequestEndDateTime && newServiceRequestStartDateTime <= serviceRequestEndDateTime)
                        {
                            serviceRequestConflict = true;
                            errorMessage = "Another service request has been assigned to the service provider on " + serviceRequestStartDateTime.ToShortDateString()
                                + " from " + serviceRequestStartDateTime.ToShortTimeString() + " to " + serviceRequestEndDateTime.ToShortTimeString() + ". Either choose another date or pick up a different time slot";
                            break;
                        }
                    }
                }
                if (serviceRequestConflict == true)
                {
                    return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "Error", ErrorMessage = errorMessage });
                }
            }

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            serviceRequest.ServiceStartDate = newServiceRequestStartDateTime;
            serviceRequest.ModifiedBy = Convert.ToInt32(userID);
            serviceRequest.ModifiedDate = DateTime.Now;

            _customerManagementRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "ok", ErrorMessage = null });

        }

        [HttpPost]
        public JsonResult CancelServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _customerManagementRepository.GetServiceRequest(model.ServiceRequestId);

            string userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            serviceRequest.Comments = model.Comments;
            serviceRequest.Status = (int)ServiceRequestStatusEnum.Cancelled;
            serviceRequest.ModifiedBy = Convert.ToInt32(userID);
            serviceRequest.ModifiedDate = DateTime.Now;

            _customerManagementRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "ok", ErrorMessage = null });

        }

        [HttpPost]
        public JsonResult GetCustomerDetail()
        {
            string userId=User.FindFirstValue(ClaimTypes.NameIdentifier);

            User customer = _customerManagementRepository.GetUserByPK(Convert.ToInt32(userId));

            return Json(new SingleEntity<User> { Result = customer, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult UpdateCustomerProfileDetail([FromBody] UserViewModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User customer = _customerManagementRepository.GetUserByPK(Convert.ToInt32(userId));

            customer.FirstName = model.FirstName.ToString().Trim();
            customer.LastName = model.LastName.ToString().Trim();
            customer.Mobile = model.Mobile.ToString().Trim();
            customer.LanguageId = model.LanguageId;


            if (model.DateOfBirth != null)
            {
                customer.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
            }

            customer.ModifiedBy = Convert.ToInt32(userId);
            customer.ModifiedDate=DateTime.Now;

            _customerManagementRepository.UpdateUser(customer);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });

        }

        [HttpPost]
        public JsonResult GetCustomerAddress(string addressId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserAddress customerAddress = _customerManagementRepository.GetUserAddressByPK(Convert.ToInt32(addressId), Convert.ToInt32(userId));

            return Json(new SingleEntity<UserAddress> { Result = customerAddress, Status = "ok", ErrorMessage = null });
        }
        [HttpPost]
        public JsonResult GetCustomerAddresses()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<UserAddress> customerAddresses = _customerManagementRepository.GetUserAddressByUserId(Convert.ToInt32(userId));

            return Json(new SingleEntity<List<UserAddress>> { Result = customerAddresses, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult SaveCustomerAddress([FromBody] UserAddressViewModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            State state = _customerManagementRepository.GetStateByCityName(model.City.ToString().Trim());

            UserAddress userAddress = new UserAddress
            {
                AddressLine1 = model.StreetName.ToString().Trim(),
                AddressLine2 = model.HouseNumber.ToString().Trim(),
                City = model.City.ToString().Trim(),
                State = state.StateName,
                PostalCode = model.PostalCode.ToString().Trim(),
                Mobile = model.PhoneNumber.ToString().Trim(),
                UserId = Convert.ToInt32(userId)
            };

            if (string.IsNullOrEmpty(model.AddressId))
            {
                userAddress = _customerManagementRepository.AddUserAddress(userAddress);
                model.AddressId = userAddress.AddressId.ToString();
            }
            else
            {
                userAddress.AddressId = Convert.ToInt32(model.AddressId);
                userAddress = _customerManagementRepository.UpdateUserAddress(userAddress);
            }

            return Json(new SingleEntity<UserAddressViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        public JsonResult DeleteCustomerAddress(string addressId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            UserAddress customerAddress = _customerManagementRepository.GetUserAddressByPK(Convert.ToInt32(addressId), Convert.ToInt32(userId));

            _customerManagementRepository.DeleteUserAddress(customerAddress);

            return Json(new SingleEntity<UserAddress> { Result = customerAddress, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult UpdateCustomerPassword([FromBody] UserViewModel model)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User customer = _customerManagementRepository.GetUserByPK(Convert.ToInt32(userId));

            if (model.Password != customer.Password)
            {
                return Json(new SingleEntity<UserViewModel> { Result = model, Status = "Error", ErrorMessage = "Your current password is wrong!" });
            }

            customer.Password = model.NewPassword.ToString().Trim();

            _customerManagementRepository.UpdateUser(customer);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

    }
}
