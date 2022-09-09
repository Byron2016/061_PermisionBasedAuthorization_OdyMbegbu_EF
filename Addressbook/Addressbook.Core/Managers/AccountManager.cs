using Addressbook.Core.Interface.Managers;
using Addressbook.Core.Interface.Queries;
using Addressbook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Core.Managers
{
    public class AccountManager : IAccountManager
    {
        private IAccountQueries _queries;

        public AccountManager(IAccountQueries queries)
        {
            _queries = queries;
        }

        public Operation<UserModel> CreateUser(UserModel user)
        {
            return Operation.Create(() =>
            {
                return user;
            });
        }

        public Operation DeleteUser(UserModel user)
        {
            return Operation.Create(() =>
            {
                //throw new NotImplementedException();
            });
        }

        public Operation<UserModel> FindByEmail(string userId)
        {
            return Operation.Create(() =>
            {
                return new UserModel();
            });
        }

        public Operation<UserModel> FindById(int userId)
        {
            return Operation.Create(() =>
            {
                return new UserModel();
            });
        }

        public Operation<string> GetPasswordHash(UserModel user)
        {
            return Operation.Create(() =>
            {
                return user.Password;
            });
        }

        public Operation<IList<string>> GetRoles(UserModel user)
        {
            return Operation.Create(() =>
            {
                IList<string> roles = new[] { "Admin" };
                return roles;
            });
        }

        public Operation<bool> IsUserInRole(UserModel user, string roleName)
        {
            return Operation.Create(() =>
            {
                return false;
            });
        }

        public Operation RemoveFromRole(UserModel user, string roleName)
        {
            return Operation.Create(() =>
            {

            });
        }

        public Operation<string> SetPasswordHash(UserModel user, string passwordHash)
        {
            return Operation.Create(() =>
            {
                return passwordHash;
            });
        }

        public Operation<UserModel> UpdateUser(UserModel user)
        {
            return Operation.Create(() => user);
        }

        //public Operation<PermissionModel[]> GetPermissions(int userID)
        //{
        //    //TODO: Debe ir??
        //    throw new NotImplementedException();
        //}
    }
}
