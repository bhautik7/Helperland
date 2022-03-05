using Helperland.Models;
using System.Collections.Generic;

namespace Helperland.Repository
{
    public interface IUserRepository
    {
        List<User> GetUserByPostalCode(string postalcode);

        User GetUserByPK(int id);

        User GetUserByEmailAndPassword(string email, string password);

        User GetUserByEmail(string email);

        User Update(User user);

        User Add(User user);
    }
}
