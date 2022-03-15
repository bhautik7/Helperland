using Helperland.Enums;
using Helperland.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Helperland.Repository
{
    public class ServiceProviderManagementRepository:IServiceProviderManagementRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public ServiceProviderManagementRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        #region Rating Table

        public IEnumerable<Rating> GetServiceProviderRatingByServiceProviderId(int serviceProviderId, decimal ratings)
        {
            return _helperlandContext.Rating.Where(x => x.RatingTo == serviceProviderId && x.Ratings > (ratings == 5 ? 0 : ratings) && x.Ratings <= (ratings == 5 ? 5 : (ratings + 1))).Include(x => x.RatingFromNavigation).Include(x => x.ServiceRequest);
        }

        #endregion Rating Table
       
        #region User Table

        public User GetUserByPK(int userId)
        {
            return _helperlandContext.User.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public List<User> GetUserByPostalCode(string postalCode)
        {
            return _helperlandContext.User.Where(x => x.ZipCode == postalCode).ToList();
        }

        public User UpdateUser(User user)
        {
            _helperlandContext.User.Update(user);
            _helperlandContext.SaveChanges();
            return user;
        }

        #endregion User Table

        public List<ServiceRequestAddress> ServiceRequestAddressByServiceRequestId(int ServiceRequestId)
        {
            return _helperlandContext.ServiceRequestAddress.Where(x => x.ServiceRequestId == ServiceRequestId).ToList();
        }

        #region ServiceRequest Table

        public IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCode(string postalCode)
        {
            
            IEnumerable<ServiceRequest> serviceRequests = from serviceRequest in _helperlandContext.ServiceRequest
                                                          join favoriteAndBlocked in _helperlandContext.FavoriteAndBlocked
                                                          on serviceRequest.UserId equals favoriteAndBlocked.TargetUserId into blockedCustomer
                                                          from blocked in blockedCustomer.DefaultIfEmpty()
                                                          where serviceRequest.ZipCode == postalCode
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Accepted
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Cancelled
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Completed
                                                            && blocked.IsBlocked != true
                                                          select serviceRequest;
            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCodeIncludePetAtHome(string postalCode)
        {
            
            IEnumerable<ServiceRequest> serviceRequests = from serviceRequest in _helperlandContext.ServiceRequest
                                                          join favoriteAndBlocked in _helperlandContext.FavoriteAndBlocked
                                                          on serviceRequest.UserId equals favoriteAndBlocked.TargetUserId into blockedCustomer
                                                          from blocked in blockedCustomer.DefaultIfEmpty()
                                                          where serviceRequest.ZipCode == postalCode
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Accepted
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Cancelled
                                                            && serviceRequest.Status != (int)ServiceRequestStatusEnum.Completed
                                                            && blocked.IsBlocked != true && serviceRequest.HasPets == false
                                                          select serviceRequest;
            return serviceRequests;
        }

        public ServiceRequest GetServiceRequestByPK(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _helperlandContext.ServiceRequest.Where(x => x.ServiceRequestId == serviceRequestId).Include(c => c.User).Include(c => c.ServiceRequestAddress).Include(c => c.ServiceRequestExtra).FirstOrDefault();
            
            return serviceRequest;
        }

        public List<ServiceRequest> GetServiceRequestListByServiceProviderId(int serviceProviderId)
        {
            List<ServiceRequest> serviceRequestList = _helperlandContext.ServiceRequest.Where(x => x.ServiceProviderId == serviceProviderId
            && x.Status == (int)ServiceRequestStatusEnum.Accepted
            && x.Status != (int)ServiceRequestStatusEnum.Cancelled
            && x.Status != (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequestList;
        }

        public ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            _helperlandContext.ServiceRequest.Update(serviceRequest);
            _helperlandContext.SaveChanges();
            return serviceRequest;
        }

        public IEnumerable<ServiceRequest> GetUpcomingServiceRequestsListByServiceProviderId(int serviceProviderId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequest.Include(x => x.User).Where(x => x.ServiceProviderId == serviceProviderId
            && x.Status == (int)ServiceRequestStatusEnum.Accepted && x.Status != (int)ServiceRequestStatusEnum.Cancelled && x.Status != (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequests;
        }

        public IEnumerable<ServiceRequest> GetServiceRequestsHistoryListByServiceProviderId(int serviceProviderId)
        {
            IEnumerable<ServiceRequest> serviceRequests = _helperlandContext.ServiceRequest.Include(x => x.User).Where(x => x.ServiceProviderId == serviceProviderId
            && x.Status == (int)ServiceRequestStatusEnum.Completed).ToList();
            return serviceRequests;
        }

        #endregion ServiceRequest Table

        #region UserAddress

        public UserAddress GetUserAddressByUserId(int userId)
        {
            return _helperlandContext.UserAddress.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress UpdateUserAddress(UserAddress userAddress)
        {
            _helperlandContext.Update(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        #endregion UserAddress

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

        #region City Table

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

        #endregion City Table
    }
}
