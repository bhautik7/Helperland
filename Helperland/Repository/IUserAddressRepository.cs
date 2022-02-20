using Helperland.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Repository
{
    public interface IUserAddressRepository
    {
        public List<UserAddress> GetUserAddress(int userId);

        public UserAddress AddUserAddress(UserAddress userAddress);

        public UserAddress SelectByPK(int addressId);
    }
}