using Helperland.Models;
using System.Collections.Generic;

namespace Helperland.Repository
{
    public interface IUserAddressRepository
    {
        public List<UserAddress> GetUserAddress(int userId);

        public UserAddress AddUserAddress(UserAddress userAddress);
        public List<City> GetCitiesByPostalCode(string postalCode);
        public UserAddress SelectByPK(int addressId);
    }
}
