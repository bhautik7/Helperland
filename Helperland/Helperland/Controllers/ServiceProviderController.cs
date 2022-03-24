using Helperland.Enums;
using Helperland.Models;
using Helperland.Repository;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Helperland.Controllers
{
    public class ServiceProviderController : Controller
    {
        private readonly IServiceProviderManagementRepository _serviceProviderManagementRepository;
        private readonly IConfiguration _configuration;
        private readonly HelperlandContext _helperlandContext;

        public ServiceProviderController(IServiceProviderManagementRepository serviceProviderControllerRepository, IConfiguration configuration, HelperlandContext helperlandContext)
        {
            this._serviceProviderManagementRepository = serviceProviderControllerRepository;
            this._configuration = configuration;
            this._helperlandContext = helperlandContext;
        }
        public IActionResult NewServiceRequests()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetNewServiceRequestsList()
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

                var includePetatHome = Request.Form["includePetatHome"].FirstOrDefault();

                User serviceProvider = _serviceProviderManagementRepository.GetUserByPK(Convert.ToInt32(userId));

                IEnumerable<ServiceRequest> serviceRequest;

                if (includePetatHome.ToString().Trim() == "true")
                {
                    serviceRequest = _serviceProviderManagementRepository.GetNewServiceRequestsListByPostalCode(serviceProvider.ZipCode);
                }
                else
                {
                    serviceRequest = _serviceProviderManagementRepository.GetNewServiceRequestsListByPostalCodeIncludePetAtHome(serviceProvider.ZipCode);
                }

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
                    case "CustomerName_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User.FirstName).ThenBy(s => s.User.LastName);  //check once for sorting
                        break;
                    case "CustomerName_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.User.FirstName).ThenBy(s => s.User.LastName);
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

                foreach (ServiceRequest tmp in data)
                {
                    tmp.User = _serviceProviderManagementRepository.GetUserByPK(Convert.ToInt32(tmp.UserId));
                    tmp.ServiceRequestAddress = _serviceProviderManagementRepository.ServiceRequestAddressByServiceRequestId(tmp.ServiceRequestId);
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
        public JsonResult GetServiceRequestWithCustomerDetails(string serviceRequestId)
        {
            ServiceRequest serviceRequest = _serviceProviderManagementRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId));

            serviceRequest.User = _serviceProviderManagementRepository.GetUserByPK(serviceRequest.UserId);

            String serviceRequestExtraName = "";

            foreach (ServiceRequestExtra serviceRequestExtra in serviceRequest.ServiceRequestExtra)
            {
                serviceRequestExtraName = serviceRequestExtraName + ", " + Enum.GetName(typeof(ExtraServiceEnum), serviceRequestExtra.ServiceExtraId);
            }

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
        public JsonResult AcceptServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _serviceProviderManagementRepository.GetServiceRequestByPK(Convert.ToInt32(model.ServiceRequestId));

            if (serviceRequest.RecordVersion.ToString() != model.RecordVersion)
            {
                return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "Error", ErrorMessage = "This service request is no more available. It has been assigned to another provider" });
            }

            DateTime newServiceRequestStartDateTime = serviceRequest.ServiceStartDate.AddMinutes(-60);
            DateTime newServiceRequestEndDateTime = serviceRequest.ServiceStartDate.AddMinutes((serviceRequest.ServiceHours * 60) + 60);

            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<ServiceRequest> serviceRequestList = _serviceProviderManagementRepository.GetServiceRequestListByServiceProviderId(Convert.ToInt32(userId));
            Boolean serviceRequestConflict = false;
            string errorMessage = "";

            foreach (ServiceRequest temp in serviceRequestList)
            {
                DateTime serviceRequestStartDateTime = temp.ServiceStartDate;
                DateTime serviceRequestEndDateTime = serviceRequestStartDateTime.AddMinutes(temp.ServiceHours * 60);

                if (serviceRequestStartDateTime <= newServiceRequestEndDateTime && newServiceRequestStartDateTime <= serviceRequestEndDateTime)
                {
                    serviceRequestConflict = true;
                    errorMessage = "Another service request " + temp.ServiceRequestId + " has already been assigned which has time overlap with this service request. You can’t pick this one!";
                    break;
                }
            }

            if (serviceRequestConflict == true)
            {
                return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "Error", ErrorMessage = errorMessage });
            }

            serviceRequest.ServiceProviderId = Convert.ToInt32(userId);
            serviceRequest.SpacceptedDate = DateTime.Now;
            serviceRequest.Status = (int)ServiceRequestStatusEnum.Accepted;

            serviceRequest.ModifiedBy = Convert.ToInt32(userId);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest.RecordVersion = Guid.NewGuid();

            _serviceProviderManagementRepository.UpdateServiceRequest(serviceRequest);

            List<User> userList = _serviceProviderManagementRepository.GetUserByPostalCode(serviceRequest.ZipCode);

            

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        [HttpPost]
        public JsonResult CompeleteServiceRequest(string serviceRequestId)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ServiceRequest serviceRequest = _serviceProviderManagementRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId.ToString().Trim()));

            serviceRequest.Status = (int)ServiceRequestStatusEnum.Completed;

            serviceRequest.ModifiedBy = Convert.ToInt32(userId);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _serviceProviderManagementRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        [HttpPost]
        public JsonResult CancelServiceRequest(string serviceRequestId)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ServiceRequest serviceRequest = _serviceProviderManagementRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId.ToString().Trim()));

            serviceRequest.Status = (int)ServiceRequestStatusEnum.Cancelled;

            serviceRequest.ModifiedBy = Convert.ToInt32(userId);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _serviceProviderManagementRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        public IActionResult UpcomingService()
        {
            return View();
        }

        [HttpPost]
       
        public IActionResult GetUpcomingServiceRequestsList()
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

                IEnumerable<ServiceRequest> serviceRequest = _serviceProviderManagementRepository.GetUpcomingServiceRequestsListByServiceProviderId(Convert.ToInt32(userId));

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
                    case "CustomerName_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);  //check once for sorting
                        break;
                    case "CustomerName_desc":
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
                    temp.ServiceRequestAddress = _serviceProviderManagementRepository.ServiceRequestAddressByServiceRequestId(temp.ServiceRequestId);
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult ServiceHistory()
        {
            return View();
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

                IEnumerable<ServiceRequest> serviceRequest = _serviceProviderManagementRepository.GetServiceRequestsHistoryListByServiceProviderId(Convert.ToInt32(userId));

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
                    case "CustomerName_asc":
                        serviceRequest = serviceRequest.OrderBy(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);  //check once for sorting
                        break;
                    case "CustomerName_desc":
                        serviceRequest = serviceRequest.OrderByDescending(s => s.User == null ? string.Empty : s.User.FirstName).ThenBy(s => s.User == null ? string.Empty : s.User.LastName);
                        break;
                    default:
                        serviceRequest = serviceRequest.OrderBy(s => s.ServiceRequestId);
                        break;
                }

                recordsTotal = serviceRequest.Count();
                var data = serviceRequest.Skip(skip).Take(pageSize).ToList();

                foreach (ServiceRequest temp in data)
                {
                    //temp.User = _serviceProviderControllerRepository.GetUserByPK(Convert.ToInt32(temp.UserId));
                    temp.ServiceRequestAddress = _serviceProviderManagementRepository.ServiceRequestAddressByServiceRequestId(temp.ServiceRequestId);
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult MyRatings()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetCustomerRating()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                decimal ddlRating = Convert.ToDecimal(Request.Form["ratings"].FirstOrDefault());

                IEnumerable<Rating> ratings = _serviceProviderManagementRepository.GetServiceProviderRatingByServiceProviderId(Convert.ToInt32(userId), ddlRating);

                recordsTotal = ratings.Count();
                var data = ratings.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IActionResult BlockCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetBlockCustomerList()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                var includePetatHome = Request.Form["includePetatHome"].FirstOrDefault();


                var customerList = _helperlandContext.User.Join(_helperlandContext.ServiceRequest.Where(x => x.ServiceProviderId == Convert.ToInt32(userId)),
                                                                         u => u.UserId,
                                                                         s => s.UserId,
                                                                         (user, serviceRequest) => user).Distinct();

               
                recordsTotal = customerList.Count();
                var data = customerList.Skip(skip).Take(pageSize).ToList();

                foreach (User temp in data)
                {
                    temp.FavoriteAndBlockedUser = _helperlandContext.FavoriteAndBlocked.Where(x => x.UserId == Convert.ToInt32(userId) && x.TargetUserId == temp.UserId).ToList();
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
        public JsonResult UpdateCustomerBlockStatus([FromBody] FavoriteAndBlockedViewModel model)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            FavoriteAndBlocked favoriteAndBlocked = _serviceProviderManagementRepository.GetFavoriteAndBlockedByUserIdAndTargetUserId(Convert.ToInt32(userId), model.TargetUserId);

            if (favoriteAndBlocked == null)
            {
                favoriteAndBlocked = new FavoriteAndBlocked
                {
                    UserId = Convert.ToInt32(userId),
                    TargetUserId = model.TargetUserId,
                    IsFavorite = false,
                    IsBlocked = model.IsBlocked
                };

                _serviceProviderManagementRepository.AddFavoriteAndBlocked(favoriteAndBlocked);
            }
            else
            {
                favoriteAndBlocked.IsBlocked = model.IsBlocked;

                _serviceProviderManagementRepository.UpdateFavoriteAndBlocked(favoriteAndBlocked);
            }

            return Json(new SingleEntity<FavoriteAndBlockedViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }
        public IActionResult MyAccount()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetServiceProviderDetail()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User customer = _serviceProviderManagementRepository.GetUserByPK(Convert.ToInt32(userId));

            customer.Password = null;

            return Json(new SingleEntity<User> { Result = customer, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult GetServiceProviderAddress()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserAddress userAddress = _serviceProviderManagementRepository.GetUserAddressByUserId(Convert.ToInt32(userId));

            return Json(new SingleEntity<UserAddress> { Result = userAddress, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _serviceProviderManagementRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }

        [HttpPost]
        public JsonResult UpdateServiceProviderProfileDetails([FromBody] UserViewModel model)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User serviceProvider = _serviceProviderManagementRepository.GetUserByPK(Convert.ToInt32(userId));

            serviceProvider.FirstName = model.FirstName;
            serviceProvider.LastName = model.LastName;
            serviceProvider.Mobile = model.Mobile;
            serviceProvider.NationalityId = model.NationalityId;
            serviceProvider.Gender = model.Gender;

            if (model.DateOfBirth != null)
            {
                serviceProvider.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
            }

            serviceProvider.ZipCode = model.ZipCode;
            serviceProvider.WorksWithPets = model.WorksWithPets;
            serviceProvider.UserProfilePicture = model.UserProfilePicture;

            serviceProvider.ModifiedBy = Convert.ToInt32(userId);
            serviceProvider.ModifiedDate = DateTime.Now;

            _serviceProviderManagementRepository.UpdateUser(serviceProvider);

            UserAddress userAddress = _serviceProviderManagementRepository.GetUserAddressByUserId(Convert.ToInt32(userId));

            if (userAddress == null)
            {
                userAddress = new UserAddress();
            }

            userAddress.UserId = Convert.ToInt32(userId);
            userAddress.AddressLine1 = model.UserAddress.StreetName;
            userAddress.AddressLine2 = model.UserAddress.HouseNumber;
            userAddress.City = model.UserAddress.City;
            userAddress.PostalCode = model.UserAddress.PostalCode;

            State state = _serviceProviderManagementRepository.GetStateByCityName(userAddress.City);

            userAddress.State = state.StateName;

            if (userAddress.AddressId == 0)
            {
                _serviceProviderManagementRepository.AddUserAddress(userAddress);
            }
            else
            {
                _serviceProviderManagementRepository.UpdateUserAddress(userAddress);
            }

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult UpdateServiceProviderPassword([FromBody] UserViewModel model)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            User customer = _serviceProviderManagementRepository.GetUserByPK(Convert.ToInt32(userId));

            if (model.Password != customer.Password)
            {
                return Json(new SingleEntity<UserViewModel> { Result = model, Status = "Error", ErrorMessage = "Your current password is wrong!" });
            }

            customer.Password = model.NewPassword.ToString().Trim();

            _serviceProviderManagementRepository.UpdateUser(customer);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }
    }
}
