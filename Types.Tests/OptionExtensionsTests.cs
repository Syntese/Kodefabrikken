using System;

using FluentAssertions;

using Kodefabrikken.Types;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Types.Tests
{
    [TestClass]
    public class OptionExtensionsTests
    {
        [TestMethod]
        public void Value_type_transforms_to_nullable()
        {
            var value = 3;
            var SUT = new Option<int>(value);

            var result = SUT.ToNullable();

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void Empty_value_type_transforms_to_nullable()
        {
            var SUT = Option<int>.Empty;

            var result = SUT.ToNullable();

            result.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Nullable_value_type_transforms_to_option()
        {
            var value = 3;
            Nullable<int> v = value;

            var result = v.ToOption();

            result.HasValue.Should().BeTrue();
            int resultValue = -1;
            result.IfValue(p => resultValue = p);
            resultValue.Should().Be(value);
        }

        [TestMethod]
        public void Empty_nullable_value_type_transforms_to_option()
        {
            Nullable<int> v = null;

            var result = v.ToOption();

            result.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Value_type_transforms_to_option()
        {
            var value = 3;

            var result = value.ToOption();

            result.HasValue.Should().BeTrue();
            var resultValue = -1;
            result.IfValue(p => resultValue = p);
            resultValue.Should().Be(value);
        }

        [TestMethod]
        public void Reference_type_transforms_to_option()
        {
            OptionExtensionsTests value = new OptionExtensionsTests();

            var result = value.ToOption();

            result.HasValue.Should().BeTrue();
            OptionExtensionsTests resultValue = null;
            result.IfValue(p => resultValue = p);
            resultValue.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Empty_reference_type_transforms_to_option()
        {
            OptionExtensionsTests value = null;

            var result = value.ToOption();

            result.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Reference_type_transforms_to_object()
        {
            var value = new OptionExtensionsTests();
            var SUT = new Option<OptionExtensionsTests>(value);

            var result = SUT.ToObject();

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Emtpy_reference_type_transforms_to_object()
        {
            var SUT = Option<OptionExtensionsTests>.Empty;

            var result = SUT.ToObject();

            result.Should().BeNull();
        }
    }
}
