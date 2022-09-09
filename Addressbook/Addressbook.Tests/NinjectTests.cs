using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Addressbook.Tests
{
    [TestClass]
    public class NinjectTests
    {
        [TestMethod]
        public void TestBindings()
        {
            //Create Kernel and Load Assembly Application.Web
            var kernel = new StandardKernel();
            kernel.Load(new Assembly[] { Assembly.Load("Addressbook.Web") });

            var query = from types in Assembly.Load("AddressBook.Core").GetExportedTypes()
                        where types.IsInterface
                        where types.Namespace.StartsWith("AddressBook.Core.Interface")
                        select types;
            foreach (var i in query.ToList())
            {
                kernel.Get(i);
            }
        }
    }
}
