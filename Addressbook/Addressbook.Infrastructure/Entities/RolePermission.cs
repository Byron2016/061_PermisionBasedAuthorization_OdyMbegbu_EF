using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Infrastructure.Entities
{
    public class RolePermission
    {
        public int RolePermissionID { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }

        public ICollection<Role> Roles { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
