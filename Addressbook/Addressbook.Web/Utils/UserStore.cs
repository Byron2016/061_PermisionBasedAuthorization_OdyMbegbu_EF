using Addressbook.Core.Interface.Managers;
using Addressbook.Web.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Addressbook.Web.Utils
{
    public class UserStore : IUserStore<User, int>, IUserPasswordStore<User, int>
    {
        private IAccountManager _account;

        public UserStore(IAccountManager account)
        {
            _account = account;
        }

        public Task CreateAsync(User user)
        {
            return _account.CreateUser(user).AsTask();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}