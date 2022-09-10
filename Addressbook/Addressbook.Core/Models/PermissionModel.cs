using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Core.Models
{
    public class PermissionModel : Model
    {
        public int PermissionID { get; set; }
        public string Name { get; set; }
    }
}
