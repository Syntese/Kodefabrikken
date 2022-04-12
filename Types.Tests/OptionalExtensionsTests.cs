using System;

using FluentAssertions;

using Kodefabrikken.Types;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kodefabrikken.Types.Tests
{
    [TestClass]
    public class OptionalExtensionsTests
    {
        [TestMethod]
        public void Optional_with_value_type_transforms_to_nullable()
        {
            var value = 3;
            var SUT = new Optional<int>(value);

            var result = SUT.ToNullable();

            result.HasValue.Should().BeTrue();
            result.Value.Should().Be(value);
        }

        [TestMethod]
        public void Optional_with_empty_value_type_transforms_to_nullable()
        {
            var SUT = Optional<int>.Empty;

            var result = SUT.ToNullable();

            result.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Nullable_value_type_transforms_to_Optional()
        {
            var value = 3;
            Nullable<int> v = value;

            var result = v.ToOptional();

            result.HasValue.Should().BeTrue();
            int resultValue = -1;
            result.IfValue(p => resultValue = p);
            resultValue.Should().Be(value);
        }

        [TestMethod]
        public void Empty_nullable_value_type_transforms_to_Optional()
        {
            Nullable<int> v = null;

            var result = v.ToOptional();

            result.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Value_type_transforms_to_Optional()
        {
            var value = 3;

            var result = value.ToOptional();

            result.HasValue.Should().BeTrue();
            var resultValue = -1;
            result.IfValue(p => resultValue = p);
            resultValue.Should().Be(value);
        }

        [TestMethod]
        public void Reference_type_transforms_to_Optional()
        {
            OptionalExtensionsTests value = new OptionalExtensionsTests();

            var result = value.ToOptional();

            result.HasValue.Should().BeTrue();
            OptionalExtensionsTests resultValue = null;
            result.IfValue(p => resultValue = p);
            resultValue.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Empty_reference_type_transforms_to_Optional()
        {
            OptionalExtensionsTests value = null;

            var result = value.ToOptional();

            result.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Optional_with_reference_type_transforms_to_object()
        {
            var value = new OptionalExtensionsTests();
            var SUT = new Optional<OptionalExtensionsTests>(value);

            var result = SUT.ToObject();

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Optional_with_emtpy_reference_type_transforms_to_object()
        {
            var SUT = Optional<OptionalExtensionsTests>.Empty;

            var result = SUT.ToObject();

            result.Should().BeNull();
        }
    }
}
