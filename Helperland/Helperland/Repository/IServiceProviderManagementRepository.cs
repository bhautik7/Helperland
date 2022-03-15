using Helperland.Models;
using System.Collections.Generic;

namespace Helperland.Repository
{
    public interface IServiceProviderManagementRepository
    {
        //User Table
        User GetUserByPK(int userId);
        List<User> GetUserByPostalCode(string postalCode);
        User UpdateUser(User user);
        

        //ServiceRequest Table
        IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCode(string postalCode);
        IEnumerable<ServiceRequest> GetNewServiceRequestsListByPostalCodeIncludePetAtHome(string postalCode);
        ServiceRequest GetServiceRequestByPK(int serviceRequestId);
        List<ServiceRequest> GetServiceRequestListByServiceProviderId(int serviceProviderId);
        
        ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest);
        IEnumerable<ServiceRequest> GetUpcomingServiceRequestsListByServiceProviderId(int serviceProviderId);
        IEnumerable<ServiceRequest> GetServiceRequestsHistoryListByServiceProviderId(int serviceProviderId);


        //Rating table
        IEnumerable<Rating> GetServiceProviderRatingByServiceProviderId(int serviceProviderId, decimal ratings);

        //ServiceRequestAddress Table
        List<ServiceRequestAddress> ServiceRequestAddressByServiceRequestId(int ServiceRequestId);
        
        //UserAddress Table
        UserAddress GetUserAddressByUserId(int userId);
        UserAddress AddUserAddress(UserAddress userAddress);
        UserAddress UpdateUserAddress(UserAddress userAddress);

        //City Table
        List<City> GetCitiesByPostalCode(string postalCode);

        //State Table
        State GetStateByCityName(string cityName);
    }
}
