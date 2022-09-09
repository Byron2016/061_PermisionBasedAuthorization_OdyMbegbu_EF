using Addressbook.Core.Interface.Managers;
using Addressbook.Core.Models;
//using Addressbook.Infrastructure.Entities;
using Addressbook.Web.Models;
using Addressbook.Web.Utils;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Addressbook.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User, int> _user;

        //Give me the owin authentication context
        public IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication; //new CSharp sintax.

        public AccountController(IAccountManager account)
        {
            _user = new UserManager<User, int>(new UserStore(account)); //v15 5.17
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var validateAndSigIn = from user in ValidateUser(model)
                                   from signIn in SignIn(user, model.RememberMe)
                                   select user;
            
            //TODO: Perform validation
            var isValid = validateAndSigIn.Succeeded;

            //Sign User In
            if (isValid)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    //return Redirect(returnUrl);
                    return RedirectToAction("Index", "Home"); //v7. 1.51
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View(model);
            } 
        }

        private Operation<User> ValidateUser(LoginModel model) //v16 0.16
        {
            return Operation.Create(() =>
            {
                if (ModelState.IsValid)
                {
                    var user = _user.Find(model.Email, model.Password);
                    if (user == null)
                        throw new Exception("Invalid Username");
                    return user;
                }
                else
                {
                    var error = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .Aggregate((ag, e) => ag + ", " + e);

                    throw new Exception(error);
                }
            });
        }

        private Operation<ClaimsIdentity> SignIn(User model, bool rememberMe) //v6 7.29 //remember me v15 8.23
        {
            return Operation.Create(() =>
            {
                var identity = _user.CreateIdentity(model, DefaultAuthenticationTypes.ApplicationCookie); //v5 3.44 - v8 1.42
            
                //optionally add additional claims
                Authentication.SignIn(new AuthenticationProperties { IsPersistent = rememberMe }, identity); //v5 3.18 //V15 9.04

                return identity;
            });
        }

        public ActionResult LogOut()
        {
            Authentication.SignOut();
            return Redirect("login");
        }
    }
}