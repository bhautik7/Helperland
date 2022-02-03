using Helperland.Enums;
using Helperland.Models;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Helperland.Controllers
{

    public class AccountController : Controller
    {
        private readonly HelperlandContext _helperlandContext;
        public AccountController(HelperlandContext helperlandContext)
        {
            this._helperlandContext = helperlandContext;
        }
        public IActionResult UserRegistration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UserRegistration(UserRegistrationViewModel model)
        {

            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.MobileNumber,
                    UserTypeId = (int)UserTypeEnum.Customer,
                    CreatedDate = DateTime.Now,
                    IsApproved = true,
                    ModifiedDate = DateTime.Now
                };

                _helperlandContext.User.Add(user);
                _helperlandContext.SaveChanges();

                TempData["SuccessMessage"] = "Register Successfully.";

                return RedirectToAction();
            }

            return View(model);
        }
      
        public IActionResult ServiceProviderRegistration(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Mobile = model.MobileNumber,
                    UserTypeId = (int)UserTypeEnum.ServiceProvider,
                    CreatedDate = DateTime.Now,
                    IsApproved = false,
                    ModifiedDate = DateTime.Now
                };

                _helperlandContext.User.Add(user);
                _helperlandContext.SaveChanges();

                TempData["SuccessMessage"] = "Register Successfully. You can login after admin can approved your request.";

                return RedirectToAction("BecomeAPro");
            }

            return View("BecomeAPro", model);
        }
        public IActionResult BecomeAPro()
        {
            return View();
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            User user = await _helperlandContext.User.FirstOrDefaultAsync(l => l.Email == model.Email && l.Password == model.Password);
            //var user = await (from tempUser in _helperlandContext.Users.where tempUser.Email == model.Email select tempUser).FirstOrDefault();
            //return RedirectToAction(;
            return RedirectToAction("Index", "Home");
        }
    }
}
