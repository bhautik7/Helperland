using Helperland.Data;
using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public UserAddressRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        public List<UserAddress> GetUserAddress(int userId)
        {
            List<UserAddress> userAddressList = _helperlandContext.UserAddresses.Where(x => x.UserId == userId).ToList();
            return userAddressList;
        }

        public UserAddress AddUserAddress(UserAddress userAddress)
        {
            _helperlandContext.UserAddresses.Add(userAddress);
            _helperlandContext.SaveChanges();
            return userAddress;
        }

        public UserAddress SelectByPK(int addressId)
        {
            UserAddress userAddress = _helperlandContext.UserAddresses.Where(x => x.AddressId == addressId).FirstOrDefault();
            _helperlandContext.SaveChanges();
            return userAddress;
        }
    }
}