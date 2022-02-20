using Helperland.Enums;
using Helperland.Models;
using Helperland.Services;
using Helperland.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
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
        private readonly EmailService sendEmail;
        public readonly IDataProtector protector;
        public readonly string Key = "bhautik@2001@sorathiya";

        public AccountController(HelperlandContext helperlandContext, IDataProtectionProvider protector, EmailService sendEmail)
        {
            this._helperlandContext = helperlandContext;
            this.sendEmail=sendEmail;
            this.protector = protector.CreateProtector(Key);
        }
        public IActionResult UserRegistration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(CombinedViewModel model)
        {
            //based on type of  user remainining
            if (ModelState.IsValid)
            {
                // note : real time we save password with encryption into the database
                // so to check that viewModel.Password also need to encrypt with same algorithm 
                // and then that encrypted password value need compare with database password value
                User _user = _helperlandContext.User.Where(_ => _.Email.ToLower() == model.LoginModel.Email.ToLower() && _.Password == model.LoginModel.Password).FirstOrDefault();

                if(_user != null && _user.IsApproved == true)
                {
                    _helperlandContext.SaveChanges();
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,_user.FirstName),
                        new Claim(ClaimTypes.Email,_user.Email)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties() { IsPersistent = model.LoginModel.IsPersistant };
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    
                    if (_user.UserTypeId == (int)UserTypeEnum.Admin)
                    {
                        return RedirectToAction("UserManagement", "Admin");
                    }
                    else if (_user.UserTypeId == (int)UserTypeEnum.Customer)
                    {
                        
                        return RedirectToAction("ServiceHistory", "Customer");
                    }
                    else if (_user.UserTypeId == (int)UserTypeEnum.ServiceProvider)
                    {
                        return RedirectToAction("UpcomingService", "ServiceProvider");
                    }
                     
                    return RedirectToAction("Index", "Home");
                }
                else if (_user != null && _user.IsApproved == false)
                {
                    TempData["msg"] = "<script>alert('You have not yet approved by Admin')</script>";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //ModelState.AddModelError("InvalidCredentials", "Either username or password is not correct");
                    TempData["msg"] = "<script>alert('InvalidCredentials, Either username or password is not correct')</script>";
                    return RedirectToAction("Index", "home");
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


        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            try
            {
                var decryptId = protector.Unprotect(token);
                DateTime expiryDate = DateTime.Parse(decryptId.Split("%")[2]).AddHours(1);
                DateTime current = DateTime.UtcNow;
                int isvalid = current.CompareTo(expiryDate);

                if (isvalid > 0)
                {
                    throw new Exception();
                }
                return View("~/Views/Account/ResetPassword.cshtml");

            }
            catch
            {
                return BadRequest(error: "Invalid Link");
            }
        }

        [HttpPost]
        public IActionResult ResetPassword(UserRegistrationViewModel model, string token)
        {
            string decryptId = protector.Unprotect(token);
            if (decryptId != null)
            {
                int userId = Convert.ToInt32(decryptId.Split("%")[1]);
                var user = _helperlandContext.User.Where(e => e.UserId == userId).FirstOrDefault();
                //user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
                user.Password = model.Password;
                _helperlandContext.User.Attach(user);
                _helperlandContext.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            return BadRequest(error: "Invalid Link");
        }

        [HttpPost]
        public IActionResult ResetLink(CombinedViewModel model)
        {
            var user = _helperlandContext.User.Where(x => x.Email.Equals(model.LoginModel.Email)).FirstOrDefault();

            if (user != null)
            {

                string Tokenstr = model.LoginModel.Email + "%" + user.UserId + "%" + DateTime.UtcNow;
                string Token = protector.Protect(Tokenstr);

                string ResetURL = Url.Action("ResetPassword", "Account", new { token = Token }, Request.Scheme);
                var email = new ResetPasswordViewModel()
                {
                    To = model.LoginModel.Email,
                    Subject = "Reset password of your account in helperland",
                    IsHTML = true,
                    Body = $"To reset your password of Helperland.<p><a href='{ResetURL}'>Click Here</a></p>",
                };
                bool result = EmailService.SendMail(email);
                if (result)
                {
                    return RedirectToAction("index", "Home");
                }
                else
                {
                    return BadRequest(error: "Internal Server Error");
                }
            }
            return BadRequest(error: "Email is not registered");
        }
        
    }
    }