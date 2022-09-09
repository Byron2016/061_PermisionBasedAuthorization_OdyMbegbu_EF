using Addressbook.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Addressbook.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            int userID = User.Identity.GetUserId<int>(); //v5 8.47
            string email = User.Identity.GetUserName();


            var user = new UserModel
            {
                UserID = userID,
                Email = email
            }; //v6 1.58

            return View(user);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View();
        }
    }
}