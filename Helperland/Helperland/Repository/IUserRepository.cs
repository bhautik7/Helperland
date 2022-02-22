using Helperland.Models;
using System.Collections.Generic;

namespace Helperland.Repository
{
    public interface IUserRepository
    {
        List<User> GetUserByPostalCode(string postalcode);
    }
}
