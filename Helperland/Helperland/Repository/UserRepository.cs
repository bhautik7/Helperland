using Helperland.Models;
using System.Collections.Generic;
using System.Linq;

namespace Helperland.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly HelperlandContext _helperlandContext;

        public UserRepository(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }

        public List<User> GetUserByPostalCode(string postalcode)
        {
            return _helperlandContext.User.Where(_=>_.ZipCode==postalcode && _.IsApproved==true).ToList();
        }
        public User GetUserByEmailAndPassword(string email, string password)
        {
            return _helperlandContext.User.Where(l => l.Email == email && l.Password == password).FirstOrDefault();
        }

        public User GetUserByEmail(string email)
        {
            return _helperlandContext.User.Where(l => l.Email == email).FirstOrDefault();
        }

        public User GetUserByPK(int id)
        {
            return _helperlandContext.User.Where(_ => _.UserId == id).FirstOrDefault();
        }

        public User Update(User user)
        {
            _helperlandContext.User.Update(user);
            _helperlandContext.SaveChanges();
            return user;
        }

        public User Add(User user)
        {
            _helperlandContext.User.Add(user);
            _helperlandContext.SaveChanges();
            return user;
        }
    }
}
