using Helperland.Models;
using System.Collections.Generic;
using System.Linq;

namespace Helperland.Repository
{
    public class UserAddressRepository:IUserAddressRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public UserAddressRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }
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
        public List<UserAddress> GetUserAddress(int userId)
        {
            List<UserAddress> userAddressList = _helperlandContext.UserAddress.Where(x => x.UserId == userId).ToList();
            return userAddressList;
        }

        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddress.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress SelectByPK(int addressId)
        {
            UserAddress userAddress = _helperlandContext.UserAddress.Where(x => x.AddressId == addressId).FirstOrDefault();
            _helperlandContext.SaveChanges();
            return userAddress;
        }

    }
}
