using Helperland.Enums;
using Helperland.Models;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //based on type of  user remainining
            if (ModelState.IsValid)
            {
                // note : real time we save password with encryption into the database
                // so to check that viewModel.Password also need to encrypt with same algorithm 
                // and then that encrypted password value need compare with database password value
                User user = _helperlandContext.User.Where(_ => _.Email.ToLower() == model.Email.ToLower() && _.Password == model.Password).FirstOrDefault();

                if(user != null)
                {
                    _helperlandContext.SaveChanges();

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,user.Email),
                        new Claim("FirstName",user.FirstName)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties() { IsPersistent = model.IsPersistant };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //ModelState.AddModelError("InvalidCredentials", "Either username or password is not correct");
                    TempData["msg"] = "<script>alert('InvalidCredentials, Either username or password is not correct')</script>";
                    return RedirectToAction("index", "home");
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (login != null)
            {
                TempData["msg"] = "<script>alert('successfully logout')</script>";
            }
            return RedirectToAction("index","home");
        }

        public bool isEmailExit(String email)
        {
            var IsCheck = _helperlandContext.User.Where(_ => _.Email == email).FirstOrDefault();
            return IsCheck != null;
  
        }
        
        [HttpPost]
        public IActionResult UserRegistration(UserRegistrationViewModel model)
        {

            if (ModelState.IsValid)
            {
                //User userEmail=_helperlandContext.User.Where(u => u.Email == model.Email).FirstOrDefault();
                if (isEmailExit(model.Email))
                {
                    TempData["ErrorMessage"] = "Email already exists,please choose another email!!";
                }
                else
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
            }

            return View(model);
        }
      
        public IActionResult ServiceProviderRegistration(UserRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                //User userEmail = _helperlandContext.User.Where(u => u.Email == model.Email).FirstOrDefault();
                if (isEmailExit(model.Email))
                {
                    TempData["ErrorMessage"] = "Email already exists,please choose another email!!";
                }
                else
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
            }
              

            return View("BecomeAPro", model);
        }
        public IActionResult BecomeAPro()
        {
            return View();
        }

    }
    }