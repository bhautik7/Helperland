
using Helperland.Enums;
using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public class CustomerManagementRepository : ICustomerManagementRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public CustomerManagementRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        #region ServiceRequest Table
        public IEnumerable<ServiceRequest> GetCurrentServiceRequestByCustomerId(int customerId)
        {
            IEnumerable<ServiceRequest> serviceRequests = from serviceRequest in _helperlandContext.ServiceRequest
                                                                                         join user in _helperlandContext.User
                                                                                         on serviceRequest.ServiceProviderId equals user.UserId into serviceProvider
                                                                                         from sp in serviceProvider.DefaultIfEmpty()
                                                                                         where serviceRequest.ServiceStartDate > DateTime.Now && serviceRequest.UserId == customerId &&
                                                                                            serviceRequest.Status != (int)ServiceRequestStatusEnum.Cancelled && serviceRequest.Status != (int)ServiceRequestStatusEnum.Completed
                                                                                         select new ServiceRequest
                                                                                         {
                                                                                             ServiceRequestId = serviceRequest.ServiceRequestId,
                                                                                             UserId = serviceRequest.UserId,
                                                                                             ServiceStartDate = serviceRequest.ServiceStartDate,
                                                                                             ServiceHours = serviceRequest.ServiceHours,
                                                                                             ZipCode = serviceRequest.ZipCode,
                                                                                             ServiceHourlyRate = serviceRequest.ServiceHourlyRate,
                                                                                             ExtraHours = serviceRequest.ExtraHours,
                                                                                             SubTotal = serviceRequest.SubTotal,
                                                                                             Discount = serviceRequest.Discount,
                                                                                             Comments = serviceRequest.Comments,
                                                                                             SpacceptedDate = serviceRequest.SpacceptedDate,
                                                                                             HasPets = serviceRequest.HasPets,
                                                                                             Status = serviceRequest.Status,
                                                                                             CreatedDate = serviceRequest.CreatedDate,
                                                                                             ModifiedBy = serviceRequest.ModifiedBy,
                                                                                             ModifiedDate = serviceRequest.ModifiedDate,
                                                                                             RefundedAmount = serviceRequest.RefundedAmount,
                                                                                             TotalCost = serviceRequest.TotalCost,
                                                                                             HasIssue = serviceRequest.HasIssue,
                                                                                             PaymentDone = serviceRequest.PaymentDone,
                                                                                             PaymentDue = serviceRequest.PaymentDue,
                                                                                             ServiceProviderId = serviceRequest.ServiceProviderId,
                                                                                             RecordVersion = serviceRequest.RecordVersion,
                                                                                             User = sp,
                                                                                         };
            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetServiceRequestHistoryListByCustomerId(int customerId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequest.Where(x => x.UserId == customerId &&
                (x.Status == (int)ServiceRequestStatusEnum.Cancelled || x.Status == (int)ServiceRequestStatusEnum.Completed)).ToList();
            return serviceRequests;
        }


        public ServiceRequest GetServiceRequest(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _helperlandContext.ServiceRequest.Where(x => x.ServiceRequestId == serviceRequestId).FirstOrDefault();
            serviceRequest.ServiceRequestExtra = _helperlandContext.ServiceRequestExtra.Where(x => x.ServiceRequestId == serviceRequestId).ToList();
            serviceRequest.ServiceRequestAddress = _helperlandContext.ServiceRequestAddress.Where(x => x.ServiceRequestId == serviceRequestId).ToList();
            serviceRequest.User = _helperlandContext.User.Where(x => x.UserId == serviceRequest.UserId).FirstOrDefault();
            return serviceRequest;
        }

        public List<ServiceRequest> GetFutureServiceRequestByServiceProviderId(int serviceProviderId)
        {
            List<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequest.Where(x => x.ServiceProviderId == serviceProviderId && x.ServiceStartDate > DateTime.Now).ToList();
            return serviceRequests;
        }

        public ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            _helperlandContext.ServiceRequest.Update(serviceRequest);
            _helperlandContext.SaveChanges();
            return serviceRequest;
        }

        #endregion ServiceRequest Table

        #region Rating Table

        public List<Rating> GetRatingsByServiceProviderId(int? serviceProviderId)
        {
            return _helperlandContext.Rating.Where(x => x.RatingTo == serviceProviderId).ToList<Rating>();
        }

        public Rating GetRatingsByServiceRequestId(int? serviceRequestId)
        {
            return _helperlandContext.Rating.Where(x => x.ServiceRequestId == serviceRequestId).FirstOrDefault();
        }

        public Rating AddRating(Rating rating)
        {
            _helperlandContext.Rating.Add(rating);
            _helperlandContext.SaveChanges();
            return rating;
        }

        #endregion Rating Table

        #region User Table
        public User GetUserByPK(int userId)
        {
            return _helperlandContext.User.Where(x => x.UserId == userId).FirstOrDefault();
        }
        public User UpdateUser(User user)
        {
            _helperlandContext.User.Update(user);
            _helperlandContext.SaveChanges();
            return user;
        }
        #endregion User Table

        #region State Table

        public State GetStateByCityName(string cityName)
        {
            State objState = (from state in _helperlandContext.State
                              join city in _helperlandContext.City on state.Id equals city.StateId
                              where city.CityName == cityName
                              select new State
                              {
                                  Id = state.Id,
                                  StateName = state.StateName
                              }).FirstOrDefault();
            return objState;
        }

        #endregion State Table
        #region User Address Table
        public List<City> GetCitiesByPostalCode(string postalCode)
        {
            List<City> cities = (from city in _helperlandContext.City
                                 join zipcode in _helperlandContext.Zipcode on city.Id equals zipcode.CityId
                                 where zipcode.ZipcodeValue == postalCode
                                 select new City
                                 {
                                     Id = city.Id,
                                     CityName = city.CityName
                                 }).ToList();
            return cities;
        }
        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddress.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress UpdateUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddress.Update(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress DeleteUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddress.Remove(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }
        public List<UserAddress> GetUserAddressByUserId(int userId)
        {
            return _helperlandContext.UserAddress.Where(x => x.UserId == userId).ToList();
        }
        public UserAddress GetUserAddressByPK(int AddressId, int userId)
        {
            return _helperlandContext.UserAddress.Where(x => x.AddressId == AddressId && x.UserId == userId).FirstOrDefault();
        }

        #endregion User Address Table
    }


}
