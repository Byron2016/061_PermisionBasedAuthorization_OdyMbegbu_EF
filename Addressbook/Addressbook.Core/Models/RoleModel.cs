using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Core.Models
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        //roll tiene múltiples permisos.
        public ICollection<PermissionModel> Permissions { get; set; } = new List<PermissionModel>();
    }
}
