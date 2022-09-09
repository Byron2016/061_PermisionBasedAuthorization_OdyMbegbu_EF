using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Core.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        //tiene multiples roles.
        public ICollection<RoleModel> Roles { get; set; } = new List<RoleModel>();
    }
}
