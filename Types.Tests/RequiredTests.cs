using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Kodefabrikken.Types.Tests
{
    [TestClass]
    public class RequiredTests
    {
        [TestMethod]
        public void Object_initialized_with_is_wrapped()
        {
            RequiredTests o = new RequiredTests();
            
            var SUT = new Required<RequiredTests>(o);

            SUT.Value.Should().BeSameAs(o);
        }

        [TestMethod]
        public void Initialization_with_null_throws_exception()
        {
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Required<RequiredTests>(null);
#pragma warning restore CA1806 // Do not ignore method results

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Default_initialized_throws_exception_on_dereference()
        {
            var SUT = new Required<RequiredTests>();

            Action act = () => { var x = SUT.Value; };

            act.Should().Throw<InvalidOperationException>();
        }
    }
}
