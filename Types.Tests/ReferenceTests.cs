using Kodefabrikken.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FluentAssertions;

namespace Types.Tests
{
    [TestClass]
    public class ReferenceTests
    {
        [TestMethod]
        public void Object_initialized_with_is_wrapped()
        {
            ReferenceTests o = new ReferenceTests();
            
            var SUT = new Reference<ReferenceTests>(o);

            SUT.Value.Should().BeSameAs(o);
        }

        [TestMethod]
        public void Initialization_with_null_throws_exception()
        {
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Reference<ReferenceTests>(null);
#pragma warning restore CA1806 // Do not ignore method results

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Default_initialized_throws_exception_on_dereference()
        {
            var SUT = new Reference<ReferenceTests>();

            Action act = () => { var x = SUT.Value; };

            act.Should().Throw<InvalidOperationException>();
        }
    }
}
