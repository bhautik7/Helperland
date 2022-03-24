using Helperland.Models;
using System.Collections.Generic;

namespace Helperland.Repository
{
    public interface IAdminControllerRepository
    {
        //City Table
        List<City> GetCitiesByPostalCode(string postalCode);

        //User Table
        User GetUserByPK(int userId);
        IEnumerable<User> GetUserList();
        IEnumerable<User> GetUserListByUserTypeId(int userTypeId);
        User UpdateUser(User user);

        //ServiceRequest Table
        IEnumerable<ServiceRequest> GetServiceRequestList();
        ServiceRequest GetServiceRequestByPK(int serviceProviderId);
        List<ServiceRequest> GetFutureServiceRequestByServiceProviderId(int serviceProviderId);
        ServiceRequest UpdateServiceRequest(ServiceRequest serviceRequest);

        //ServiceRequestAddress Table
        ServiceRequestAddress GetServiceRequestAddressByServiceRequestId(int serviceRequestId);
        ServiceRequestAddress UpdateServiceRequestAddress(ServiceRequestAddress serviceRequestAddress);

        //State Table
        State GetStateByCityName(string cityName);
    }
}
