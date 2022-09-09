using Addressbook.Web.Models;
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
        //Give me the owin authentication context
        public IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication; //new CSharp sintax.

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            //TODO: Perform validation

            var isValid = true;

            //Sign User In
            if (isValid)
            {
                SignIn(model);

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

        private void SignIn(LoginModel model) //v6 7.29
        {
            var claims = new Claim[]{
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.Role, "admin")
            }; //v5 6.35

            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie); //v5 3.44 - v8 1.42
            Authentication.SignIn(identity); //v5 3.18 
        }
    }
}