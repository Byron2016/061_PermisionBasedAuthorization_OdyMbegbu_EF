using Addressbook.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Core.Interface.Managers
{
    public interface IAccountManager
    {
        Operation<UserModel> CreateUser(UserModel user);
        Operation DeleteUser(UserModel user); //
        Operation<UserModel> UpdateUser(UserModel user);
        Operation<string> SetPasswordHash(UserModel user, string passwordHash);
        Operation RemoveFromRole(UserModel user, string roleName);
        Operation<bool> IsUserInRole(UserModel user, string roleName);
        Operation<IList<string>> GetRoles(UserModel user);
        Operation<string> GetPasswordHash(UserModel user);
        Operation<UserModel> FindByEmail(string userId);
        Operation<UserModel> FindById(int userId);
        //Operation<PermissionModel[]> GetPermissions(int userID); //
    }
}
