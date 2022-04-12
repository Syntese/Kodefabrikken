using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kodefabrikken.Types.Tests
{
    [TestClass]
    public class OptionalTests
    {
        [TestMethod]
        public void Type_is_correct_for_value_type()
        {
            var SUT = Optional<int>.OptionType;

            SUT.Should().Be(typeof(int));
        }

        [TestMethod]
        public void Type_is_correct_for_reference_type()
        {
            var SUT = Optional<OptionalTests>.OptionType;

            SUT.Should().Be(typeof(OptionalTests));
        }

        [TestMethod]
        public void Empty_for_Nullable_value_type_throws_exception()
        {
            Action act = () => _ = Optional<Nullable<int>>.Empty;

            act.Should().Throw<InvalidOperationException>();

            // repeat test with nullable context
#nullable enable
            act = () => _ = Optional<int?>.Empty;
#nullable disable

            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Empty_for_Optional_type_throws_exception()
        {
            Action act = () => _ = Optional<Optional<int>>.Empty;

            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Empty_for_Value_type_has_no_value()
        {
            var SUT = Optional<int>.Empty;

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_for_Nullable_reference_type_has_no_value()
        {
            // compiler transforms to wrapped type
#nullable enable
            var SUT = Optional<OptionalTests?>.Empty;
#nullable disable

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_is_created_for_value_type()
        {
            var SUT = Optional<int>.Create();

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_is_created_for_reference_type()
        {
            var SUT = Optional<OptionalTests>.Create();

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Optional_is_created_for_value_type()
        {
            int i = 3;

            var SUT = Optional<int>.Create(i);

            SUT.Should().NotBeNull();
            int v = 0;
            SUT.IfValue(p => v = p);
            v.Should().Be(i);
        }

        [TestMethod]
        public void Optional_is_created_for_reference_type()
        {
            OptionalTests x = new OptionalTests();

            var SUT = Optional<OptionalTests>.Create(x);

            SUT.Should().NotBeNull();
            OptionalTests v = null;
            SUT.IfValue(p => v = p);
            v.Should().BeSameAs(x);
        }

        [TestMethod]
        public void Optional_is_created_for_null_reference_type()
        {
            OptionalTests x = null;

            var SUT = Optional<OptionalTests>.Create(x);

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Value_type_is_cast_to_Optional()
        {
            int i = 3;
            Optional<int> SUT = i;

            SUT.Should().NotBeNull();
            int v = 0;
            SUT.IfValue(p => v = p);
            v.Should().Be(i);
        }

        [TestMethod]
        public void Reference_type_is_cast_to_Optional()
        {
            OptionalTests o = new OptionalTests();
            Optional<OptionalTests> SUT = o;

            SUT.Should().NotBeNull();
            OptionalTests v = null;
            SUT.IfValue(p => v = p);
            v.Should().BeSameAs(o);
        }

        [TestMethod]
        public void Null_reference_type_is_cast_to_Optional()
        {
            OptionalTests o = null;
            Optional<OptionalTests> SUT = o;

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Value_type_has_correct_state()
        {
            var value = 3;
            var SUT = new Optional<int>(value);

            SUT.HasValue.Should().BeTrue();
        }

        [TestMethod]
        public void Reference_type_has_correct_state()
        {
            var value = new OptionalTests();
            var SUT = new Optional<OptionalTests>(value);

            SUT.HasValue.Should().BeTrue();
        }

        [TestMethod]
        public void Default_constructor_with_reference_type_has_no_value()
        {
            var SUT = new Optional<OptionalTests>();

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Default_constructor_with_value_type_has_no_value()
        {
            var SUT = new Optional<int>();

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Construction_with_initialized_nullable_value_type_throws_exception()
        {
            Action act = () => _ = new Optional<Nullable<int>>(3);

            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Construction_with_empty_nullable_value_type_throws_exception()
        {
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Optional<Nullable<int>>(null);
#pragma warning restore CA1806 // Do not ignore method results

            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Construction_with_initialized_nullable_reference_type_allowed()
        {
#nullable enable
            // compiler transforms to wrapped type
            Action act = () => _ = new Optional<OptionalTests?>(new OptionalTests());
#nullable disable

            act.Should().NotThrow();
        }

        [TestMethod]
        public void Construction_with_empty_nullable_reference_throws_exception()
        {
#nullable enable
            // compiler transforms to wrapped type
            Action act = () => _ = new Optional<OptionalTests?>(null);
#nullable disable

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Registered_value_action_called_with_correct_value()
        {
            var value = 3;
            var SUT = new Optional<int>(value);
            int paramValue = -1;

            SUT.IfValue(p => paramValue = p);

            paramValue.Should().Be(value);
        }

        [TestMethod]
        public void Registered_value_action_not_called_when_empty()
        {
            var SUT = Optional<int>.Empty;
            bool isCalled = false;

            SUT.IfValue(_ => isCalled = true);

            isCalled.Should().BeFalse();
        }

        [TestMethod]
        public void Registered_value_action_called_when_value_and_context_used()
        {
            var SUT = new Optional<int>(3);
            bool ifValueCalled = false;
            bool ifEmptyCalled = false;

            SUT.IfValue(_ => ifValueCalled = true)
                .Else(() => ifEmptyCalled = true);

            ifValueCalled.Should().BeTrue();
            ifEmptyCalled.Should().BeFalse();
        }

        [TestMethod]
        public void Registered_empty_action_called_when_empty_and_context_used()
        {
            var SUT = Optional<int>.Empty;
            bool ifValueCalled = false;
            bool ifEmptyCalled = false;

            SUT.IfValue(_ => ifValueCalled = true)
                .Else(() => ifEmptyCalled = true);

            ifValueCalled.Should().BeFalse();
            ifEmptyCalled.Should().BeTrue();
        }

        [TestMethod]
        public void Value_type_is_coalesced()
        {
            int value = 3;
            var SUT = Optional<int>.Empty;

            var result = SUT.Coalesce(value);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Value_type_with_value_is_not_coalsced()
        {
            int value = 3;
            var SUT = new Optional<int>(value);

            var result = SUT.Coalesce(-1);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Value_type_is_coalesced_with_functor()
        {
            int value = 3;
            var SUT = Optional<int>.Empty;

            var result = SUT.Coalesce(() => value);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Value_type_with_value_is_not_coalsced_with_functor()
        {
            int value = 3;
            var SUT = new Optional<int>(value);

            var result = SUT.Coalesce(() => -1);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Reference_type_is_coalesced()
        {
            OptionalTests value = new OptionalTests();
            var SUT = Optional<OptionalTests>.Empty;

            var result = SUT.Coalesce(value);

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Reference_type_with_value_is_not_coalesced()
        {
            OptionalTests value = new OptionalTests();
            var SUT = new Optional<OptionalTests>(value);

            var result = SUT.Coalesce(new OptionalTests());

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Reference_type_is_coalesced_with_functor()
        {
            OptionalTests value = new OptionalTests();
            var SUT = Optional<OptionalTests>.Empty;

            var result = SUT.Coalesce(() => value);

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Reference_type_with_value_is_not_coalesced_with_functor()
        {
            OptionalTests value = new OptionalTests();
            var SUT = new Optional<OptionalTests>(value);

            var result = SUT.Coalesce(() => new OptionalTests());

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Empty_reference_type_in_coalesce_throws()
        {
            OptionalTests value = null;
            var SUT = Optional<OptionalTests>.Empty;

            Action act = () => SUT.Coalesce(value);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Coalesce_with_null_functor_throws()
        {
            var SUT = Optional<OptionalTests>.Empty;

            Func<OptionalTests> func = null;

            Action act = () => SUT.Coalesce(func);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Coalesce_functor_returning_empty_reference_throws()
        {
            var SUT = Optional<OptionalTests>.Empty;

            Action act = () => SUT.Coalesce(() => null);

            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Optional_cast_to_expected_value()
        {
            var SUT = new Optional<int>(3);

            var result = SUT.Cast(Convert.ToDouble, () => throw new InvalidCastException());

            result.Should().Be(3.0);
        }

        [TestMethod]
        public void Empty_optional_cast_to_excpected_value()
        {
            var SUT = Optional<int>.Empty;

            var result = SUT.Cast(p => throw new InvalidCastException(), () => 0.0);

            result.Should().Be(0.0);
        }

        [TestMethod]
        public void Optional_are_equal_to_self()
        {
            var SUT = new Optional<OptionalTests>();

            // TODO : Are we boxing, should we implement Equals<T>?
            SUT.Equals(SUT).Should().BeTrue();
        }

        [TestMethod]
        public void Empty_optional_are_equal_to_null()
        {
            var SUT = new Optional<OptionalTests>();

            SUT.Equals(null).Should().BeTrue();
        }

        [TestMethod]
        public void Different_value_optionals_are_not_equal()
        {
            var SUT1 = new Optional<int>(1);
            var SUT2 = new Optional<int>(2);

            SUT1.Equals(SUT2).Should().BeFalse();
        }

        [TestMethod]
        public void Optional_should_be_equal_to_same_value()
        {
            var value = 3;
            var SUT = new Optional<int>(value);

            SUT.Equals(value).Should().BeTrue();
        }

        [TestMethod]
        public void Optionals_of_different_type_should_not_be_equal()
        {
            var SUT1 = new Optional<int>(7);
            var SUT2 = new Optional<long>(7);

            SUT1.Equals(SUT2).Should().BeFalse();
        }

        [TestMethod]
        public void Optional_is_equal_to_self()
        {
            var SUT = new Optional<int>(1);

#pragma warning disable CS1718 // Comparison made to same variable
            var result = SUT == SUT;
#pragma warning restore CS1718 // Comparison made to same variable

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Optionals_with_different_values_are_different()
        {
            var SUT1 = new Optional<int>(1);
            var SUT2 = new Optional<int>(2);

            var result = SUT1 != SUT2;

            result.Should().BeTrue();
        }
    }
}
