using Addressbook.Core.Interface.Managers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Addressbook.Core.Models;

namespace Addressbook.Web.Utils
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        private string[] _permissions;
        private IAccountManager _account;
        public AuthorizeUserAttribute(params string[] permissions)
        {
            _permissions = permissions;
            _account = NinjectContainer.Resolve<IAccountManager>();
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //First Make Sure that the User is Authenticated
            if (httpContext.User.Identity.IsAuthenticated)
            {
                //Get Permissions List in Session
                var permissions = httpContext.Session["Permissions"] as string[];
                if (permissions == null)
                {
                    //Fetch Permissions
                    //var getPermissions = _account.GetPermissions(httpContext.User.Identity.GetUserId<int>());
                    IList<PermissionModel> getPermissions = new List<PermissionModel> //Hago esto x q la expresión anterior en este momento me está dando errores en el Any.
                    {
                        new PermissionModel
                        {
                            PermissionID = 1,
                            Name = "Home-Page"
                        },
                        new PermissionModel
                        {
                            PermissionID = 1,
                            Name = "Account-Page"
                        }
                    };
                    if (getPermissions.Any())
                    {
                        //Cache Permissions
                        httpContext.Session["Permissions"] = getPermissions.Select(p => p.Name).ToArray();

                        //Check to See if User Has all the Required Permissions
                        var query = from permission in _permissions
                                    join userpermission in getPermissions
                                    on permission.ToLower() equals userpermission.Name.ToLower()
                                    select permission;
                        return query.Any();
                    }
                }
                else
                {
                    var query = from permission in _permissions
                                join userpermission in permissions
                                on permission.ToLower() equals userpermission.ToLower()
                                select permission;
                    return query.Any();
                }
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("/account/notauthorized");
        }
    }
}