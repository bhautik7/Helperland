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
    }
}
