using Addressbook.Core.Interface.Managers;
using Addressbook.Core.Interface.Queries;
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
    }
}
