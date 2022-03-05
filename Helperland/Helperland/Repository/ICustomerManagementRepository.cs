using Helperland.Models;
using System.Collections.Generic;

namespace Helperland.Repository
{
    public interface ICustomerManagementRepository
    {
        //ServiceRequest Table
        public IEnumerable<ServiceRequest> GetCurrentServiceRequestByCustomerId(int customerId);
        
        public IEnumerable<ServiceRequest> GetServiceRequestHistoryListByCustomerId(int customerId);

   
        ServiceRequest GetServiceRequest(int serviceRequestId);

        public User GetUserByPK(int userId);
        public User UpdateUser(User user);
        List<ServiceRequest> GetFutureServiceRequestByServiceProviderId(int serviceProviderId);

        //Rating Table
        List<Rating> GetRatingsByServiceProviderId(int? serviceProviderId);
        public Rating GetRatingsByServiceRequestId(int? serviceRequestId);

        public Rating AddRating(Rating rating);

        ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest);

        public List<UserAddress> GetUserAddressByUserId(int userId);
        public UserAddress GetUserAddressByPK(int AddressId, int userId);

        public State GetStateByCityName(string cityName);
        public List<City> GetCitiesByPostalCode(string postalCode);

        public UserAddress AddUserAddress(UserAddress userAddress);
        public UserAddress UpdateUserAddress(UserAddress userAddress);
        public UserAddress DeleteUserAddress(UserAddress userAddress);


    }
}
