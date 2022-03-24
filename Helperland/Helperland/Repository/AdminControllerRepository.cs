using Helperland.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Helperland.Repository
{
    public class AdminControllerRepository:IAdminControllerRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public AdminControllerRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

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

        #region User Table

        public User GetUserByPK(int userId)
        {
            return _helperlandContext.User.Where(x => x.UserId == userId).FirstOrDefault();
        }

        public IEnumerable<User> GetUserList()
        {
            var users = _helperlandContext.User;
            return users;
        }

        public IEnumerable<User> GetUserListByUserTypeId(int userTypeId)
        {
            var users = _helperlandContext.User.Where(x => x.UserTypeId == userTypeId);
            return users;
        }
        public User UpdateUser(User user)
        {
            _helperlandContext.User.Update(user);
            _helperlandContext.SaveChanges();
            return user;
        }

        #endregion User Table

        #region ServiceRequest Table

        public IEnumerable<ServiceRequest> GetServiceRequestList()
        {
            var serviceRequests = _helperlandContext.ServiceRequest.Include(x => x.User).Include(x => x.ServiceProvider).ThenInclude(x => x.RatingRatingToNavigation).Include(x => x.ServiceRequestAddress).AsNoTracking();
            return serviceRequests;
        }

        public ServiceRequest GetServiceRequestByPK(int serviceRequestId)
        {
            ServiceRequest serviceRequest = _helperlandContext.ServiceRequest.Where(x => x.ServiceRequestId == serviceRequestId).Include(x => x.ServiceRequestAddress).FirstOrDefault();
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

        #region ServiceRequestAddress Table

        public ServiceRequestAddress GetServiceRequestAddressByServiceRequestId(int serviceRequestId)
        {
            ServiceRequestAddress serviceRequestAddress = _helperlandContext.ServiceRequestAddress.Where(x => x.ServiceRequestId == serviceRequestId).FirstOrDefault();
            return serviceRequestAddress;
        }

        public ServiceRequestAddress UpdateServiceRequestAddress(ServiceRequestAddress serviceRequestAddress)
        {
            _helperlandContext.ServiceRequestAddress.Update(serviceRequestAddress);
            _helperlandContext.SaveChanges();
            return serviceRequestAddress;
        }

        #endregion ServiceRequestAddress Table

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

    }
}
