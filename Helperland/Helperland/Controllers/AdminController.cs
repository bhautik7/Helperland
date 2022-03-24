using Helperland.Enums;
using Helperland.Models;
using Helperland.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using Helperland.ViewModels;
using System.Security.Claims;

namespace Helperland.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminControllerRepository _adminControllerRepository;
        private readonly IConfiguration _configuration;

        public AdminController(IAdminControllerRepository adminControllerRepository, IConfiguration configuration)
        {
            this._adminControllerRepository = adminControllerRepository;
            this._configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UserManagement()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetUserList()
        {
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

                var searchUserName = Request.Form["searchUserName"].FirstOrDefault();
                var searchUserType = Request.Form["searchUserType"].FirstOrDefault();
                var searchPhoneNumber = Request.Form["searchPhoneNumber"].FirstOrDefault();
                var searchZipcode = Request.Form["searchZipcode"].FirstOrDefault();
                var searchRegistrationStartDate = Request.Form["searchRegistrationStartDate"].FirstOrDefault();
                var searchRegistrationEndDate = Request.Form["searchRegistrationEndDate"].FirstOrDefault();

                IEnumerable<User> userList = _adminControllerRepository.GetUserList();

                if (!string.IsNullOrEmpty(searchUserName))
                {
                    userList = userList.Where(x => (x.FirstName.ToString().ToLower() + " " + x.LastName.ToString().ToLower()).Contains(searchUserName.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchUserType))
                {
                    userList = userList.Where(x => x.UserTypeId.ToString().Contains(searchUserType));
                }

                if (!string.IsNullOrEmpty(searchPhoneNumber))
                {
                    userList = userList.Where(x => x.Mobile.ToString().Contains(searchPhoneNumber));
                }

                if (!string.IsNullOrEmpty(searchZipcode))
                {
                    userList = userList.Where(x => !string.IsNullOrEmpty(x.ZipCode) && x.ZipCode.Contains(searchZipcode));
                }

                if (!string.IsNullOrEmpty(searchRegistrationStartDate))
                {
                    DateTime startDate = Convert.ToDateTime(searchRegistrationStartDate);
                    userList = userList.Where(x => x.CreatedDate > startDate);
                }

                if (!string.IsNullOrEmpty(searchRegistrationEndDate))
                {
                    DateTime endDate = Convert.ToDateTime(searchRegistrationEndDate);
                    userList = userList.Where(x => x.CreatedDate < endDate);
                }

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "UserId_asc":
                        userList = userList.OrderBy(s => s.UserId);
                        break;
                    case "UserId_desc":
                        userList = userList.OrderByDescending(s => s.UserId);
                        break;
                    case "UserName_asc":
                        userList = userList.OrderBy(s => s.FirstName).ThenBy(s => s.LastName);
                        break;
                    case "UserName_desc":
                        userList = userList.OrderByDescending(s => s.FirstName).ThenByDescending(s => s.LastName);
                        break;
                    case "CreatedDate_asc":
                        userList = userList.OrderBy(s => s.CreatedDate);
                        break;
                    case "CreatedDate_desc":
                        userList = userList.OrderByDescending(s => s.CreatedDate);
                        break;
                    case "UserType_asc":
                        userList = userList.OrderBy(s => (int)s.UserTypeId == 1 ? "Admin" : (int)s.UserTypeId == 2 ? "Service Provider" : "Customer");
                        break;
                    case "UserType_desc":
                        userList = userList.OrderByDescending(s => (int)s.UserTypeId == 1 ? "Admin" : (int)s.UserTypeId == 2 ? "Service Provider" : "Customer");
                        break;
                    case "Mobile_asc":
                        userList = userList.OrderBy(s => s.Mobile);
                        break;
                    case "Mobile_desc":
                        userList = userList.OrderByDescending(s => s.Mobile);
                        break;
                    case "Zipcode_asc":
                        userList = userList.OrderBy(s => s.ZipCode);
                        break;
                    case "Zipcode_desc":
                        userList = userList.OrderByDescending(s => s.ZipCode);
                        break;
                    case "Status_asc":
                        userList = userList.OrderBy(s => (bool)s.IsActive == true ? "Active" : "Inactive");
                        break;
                    case "Status_desc":
                        userList = userList.OrderByDescending(s => (bool)s.IsActive == true ? "Active" : "Inactive");
                        break;
                    default:
                        userList = userList.OrderBy(s => s.UserId);
                        break;
                }

                recordsTotal = userList.Count();
                var data = userList.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public JsonResult SearchUser(string userType, string term)
        {
            IEnumerable<User> users;

            if (!string.IsNullOrEmpty(term))
            {
                if (userType == "customer")
                {
                    users = _adminControllerRepository.GetUserListByUserTypeId((int)UserTypeEnum.Customer);
                }
                else if (userType == "serviceProvider")
                {
                    users = _adminControllerRepository.GetUserListByUserTypeId((int)UserTypeEnum.ServiceProvider);
                }
                else
                {
                    users = _adminControllerRepository.GetUserList();
                }

                var data = users.Where(a => a.FirstName.ToString().ToLower().Contains(term.ToLower())
                || a.LastName.ToString().ToLower().Contains(term.ToLower())).Select(x => new { id = x.FirstName + " " + x.LastName, text = x.FirstName + " " + x.LastName }).ToList().Distinct();
                return Json(data);
            }
            else
            {
                return Json("");
            }
        }
        [HttpPost]
        public JsonResult UpdateUserActiveStatus([FromBody] UserViewModel model)
        {
            User serviceProvider = _adminControllerRepository.GetUserByPK(Convert.ToInt32(model.UserId));

            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            serviceProvider.IsActive = model.IsActive;
            serviceProvider.ModifiedBy = Convert.ToInt32(userId);
            serviceProvider.ModifiedDate = DateTime.Now;

            _adminControllerRepository.UpdateUser(serviceProvider);

            return Json(new SingleEntity<UserViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult ApproveServiceProvider(string userId)
        {
            User serviceProvider = _adminControllerRepository.GetUserByPK(Convert.ToInt32(userId));

            String userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            serviceProvider.IsApproved = true;
            serviceProvider.ModifiedBy = Convert.ToInt32(userID);
            serviceProvider.ModifiedDate = DateTime.Now;

            _adminControllerRepository.UpdateUser(serviceProvider);

            return Json(new SingleEntity<User> { Result = serviceProvider, Status = "ok", ErrorMessage = null });
        }

        public IActionResult ServiceRequests()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetServiceRequestList()
        {
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

                var searchServiceRequestId = Request.Form["searchServiceRequestId"].FirstOrDefault();
                var searchCustomerName = Request.Form["searchCustomerName"].FirstOrDefault();
                var searchServiceProviderName = Request.Form["searchServiceProviderName"].FirstOrDefault();
                var searchStatus = Request.Form["searchStatus"].FirstOrDefault();
                var searchServiceStartDate = Request.Form["searchServiceStartDate"].FirstOrDefault();
                var searchServiceEndDate = Request.Form["searchServiceEndDate"].FirstOrDefault();

                IEnumerable<ServiceRequest> serviceRequestList = _adminControllerRepository.GetServiceRequestList();

                if (!string.IsNullOrEmpty(searchServiceRequestId))
                {
                    serviceRequestList = serviceRequestList.Where(x => x.ServiceRequestId == Convert.ToInt32(searchServiceRequestId));
                }

                if (!string.IsNullOrEmpty(searchCustomerName))
                {
                    serviceRequestList = serviceRequestList.Where(x => (x.User.FirstName.ToString().ToLower() + " " + x.User.LastName.ToString().ToLower()).Contains(searchCustomerName.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchServiceProviderName))
                {
                    serviceRequestList = serviceRequestList.Where(x => x.ServiceProvider != null && (x.ServiceProvider.FirstName.ToString().ToLower() + " " + x.ServiceProvider.LastName.ToString().ToLower()).Contains(searchServiceProviderName.ToLower()));
                }

                if (!string.IsNullOrEmpty(searchStatus))
                {
                    serviceRequestList = serviceRequestList.Where(x => x.Status.ToString().Contains(searchStatus));
                }

                if (!string.IsNullOrEmpty(searchServiceStartDate))
                {
                    DateTime startDate = Convert.ToDateTime(searchServiceStartDate);
                    serviceRequestList = serviceRequestList.Where(x => x.CreatedDate > startDate);
                }

                if (!string.IsNullOrEmpty(searchServiceEndDate))
                {
                    DateTime endDate = Convert.ToDateTime(searchServiceEndDate);
                    serviceRequestList = serviceRequestList.Where(x => x.CreatedDate < endDate);
                }

                var sortOrder = sortColumn + "_" + sortColumnDirection;

                switch (sortOrder)
                {
                    case "ServiceRequestId_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceRequestId);
                        break;
                    case "ServiceRequestId_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceRequestId);
                        break;
                    case "ServiceStartDate_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceStartDate);
                        break;
                    case "ServiceStartDate_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceStartDate);
                        break;
                    case "Customer_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.User.FirstName).ThenBy(s => s.User.LastName);
                        break;
                    case "Customer_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.User.FirstName).ThenBy(s => s.User.LastName);
                        break;
                    case "ServiceProvider_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.FirstName).ThenBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.LastName);
                        break;
                    case "ServiceProvider_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.FirstName).ThenBy(s => s.ServiceProvider == null ? string.Empty : s.ServiceProvider.LastName);
                        break;
                    case "Status_asc":
                        serviceRequestList = serviceRequestList.OrderBy(s => (s.Status == (int)ServiceRequestStatusEnum.New ? "New" : (s.Status == (int)ServiceRequestStatusEnum.Cancelled ? "Cancelled" : (s.Status == (int)ServiceRequestStatusEnum.Completed ? "Completed" : "Pending"))));
                        break;
                    case "Status_desc":
                        serviceRequestList = serviceRequestList.OrderByDescending(s => (s.Status == (int)ServiceRequestStatusEnum.New ? "New" : (s.Status == (int)ServiceRequestStatusEnum.Cancelled ? "Cancelled" : (s.Status == (int)ServiceRequestStatusEnum.Completed ? "Completed" : "Pending"))));
                        break;
                    default:
                        serviceRequestList = serviceRequestList.OrderByDescending(s => s.ServiceProviderId);
                        break;
                }

                recordsTotal = serviceRequestList.Count();
                var data = serviceRequestList.Skip(skip).Take(pageSize).ToList();

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetServiceRequest(string serviceRequestId)
        {
            ServiceRequest serviceRequest = _adminControllerRepository.GetServiceRequestByPK(Convert.ToInt32(serviceRequestId.ToString().Trim()));

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok" });
        }

        [HttpPost]
        public JsonResult GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = _adminControllerRepository.GetCitiesByPostalCode(postalCode);
            return Json(cities);
        }
        [HttpPost]
        public JsonResult CancelServiceRequest(int serviceRequestId)
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ServiceRequest serviceRequest = _adminControllerRepository.GetServiceRequestByPK(serviceRequestId);

            serviceRequest.Status = (int)ServiceRequestStatusEnum.Cancelled;
            serviceRequest.ModifiedBy = Convert.ToInt32(userId);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _adminControllerRepository.UpdateServiceRequest(serviceRequest);

            return Json(new SingleEntity<ServiceRequest> { Result = serviceRequest, Status = "ok", ErrorMessage = null });
        }
        [HttpPost]
        public JsonResult UpdateServiceRequest([FromBody] EditServiceRequestAdminViewModel model)
        {
            ServiceRequest serviceRequest = _adminControllerRepository.GetServiceRequestByPK(model.ServiceRequestId);

            DateTime newServiceRequestStartDateTime = Convert.ToDateTime(model.ServiceStartDate + " " + model.ServiceStartTime);
            DateTime newServiceRequestEndDateTime = newServiceRequestStartDateTime.AddMinutes(serviceRequest.ServiceHours * 60);

            DateTime dateLimit = Convert.ToDateTime(model.ServiceStartDate).AddHours(21);

            if (newServiceRequestEndDateTime > dateLimit)
            {
                return Json(new SingleEntity<EditServiceRequestAdminViewModel> { Result = model, Status = "Error", ErrorMessage = "Could not completed the service request, because service booking request is must be completed within 21:00 time" });
            }

            newServiceRequestStartDateTime = newServiceRequestStartDateTime.AddMinutes(-60);
            newServiceRequestEndDateTime = newServiceRequestEndDateTime.AddMinutes(+60);

            if (serviceRequest.ServiceProviderId != null)
            {
                List<ServiceRequest> serviceRequestList = _adminControllerRepository.GetFutureServiceRequestByServiceProviderId(Convert.ToInt32(serviceRequest.ServiceProviderId));

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
                    return Json(new SingleEntity<EditServiceRequestAdminViewModel> { Result = model, Status = "Error", ErrorMessage = errorMessage });
                }
            }

            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            serviceRequest.ServiceStartDate = Convert.ToDateTime(model.ServiceStartDate + " " + model.ServiceStartTime);
            serviceRequest.ModifiedBy = Convert.ToInt32(userId);
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _adminControllerRepository.UpdateServiceRequest(serviceRequest);

            ServiceRequestAddress serviceRequestAddress = _adminControllerRepository.GetServiceRequestAddressByServiceRequestId(serviceRequest.ServiceRequestId);

            serviceRequestAddress.AddressLine1 = model.StreetName;
            serviceRequestAddress.AddressLine2 = model.HouseNumber;
            serviceRequestAddress.PostalCode = model.PostalCode;
            serviceRequestAddress.City = model.City;

            State state = _adminControllerRepository.GetStateByCityName(model.City.ToString().Trim());

            serviceRequestAddress.State = state.StateName;

            serviceRequestAddress = _adminControllerRepository.UpdateServiceRequestAddress(serviceRequestAddress);

            User customer = _adminControllerRepository.GetUserByPK(Convert.ToInt32(serviceRequest.UserId));

            return Json(new SingleEntity<EditServiceRequestAdminViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        [HttpPost]
        public JsonResult RefundAmountServiceRequest([FromBody] ServiceRequestViewModel model)
        {
            ServiceRequest serviceRequest = _adminControllerRepository.GetServiceRequestByPK(Convert.ToInt32(model.ServiceRequestId));

            serviceRequest.RefundedAmount = Convert.ToDecimal(model.RefundedAmount);
            serviceRequest.ModifiedBy = GetLogInUserId();
            serviceRequest.ModifiedDate = DateTime.Now;

            serviceRequest = _adminControllerRepository.UpdateServiceRequest(serviceRequest);

            User customer = _adminControllerRepository.GetUserByPK(serviceRequest.UserId);

            return Json(new SingleEntity<ServiceRequestViewModel> { Result = model, Status = "ok", ErrorMessage = null });
        }

        private int GetLogInUserId()
        {
            String userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Convert.ToInt32(userId);
        }
    }
}
