using Addressbook.Web.Models;
using Addressbook.Core.Interface.Managers;
//using Addressbook.Infrastructure.Entities;
using Addressbook.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Addressbook.Web.Utils
{
    public class UserStore : IUserStore<User, int>, IUserPasswordStore<User, int>, IUserRoleStore<User, int>
    {
        private IAccountManager _account;

        public UserStore(IAccountManager account)
        {
            _account = account;
        }

        public Task AddToRoleAsync(User user, string roleName)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(User user)
        {
            return _account.CreateUser(user).AsTask();
        }

        public Task DeleteAsync(User user)
        {
            return _account.DeleteUser(user).AsTask();
        }

        public void Dispose()
        {
            
        }

        public Task<User> FindByIdAsync(int userId)
        {
            return _account.FindById(userId).Select(u => new User(u)).AsTask();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _account.FindByEmail(userName).Select(u => new User(u)).AsTask();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return _account.GetPasswordHash(user).AsTask();
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return _account.GetRoles(user).AsTask();
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return Task.FromResult(true);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return _account.IsUserInRole(user, roleName).AsTask();
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            return _account.RemoveFromRole(user, roleName).AsTask();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return _account.SetPasswordHash(user, passwordHash).AsTask();
        }

        public Task UpdateAsync(User user)
        {
            return _account.UpdateUser(user).AsTask();
        }
    }
}