using System;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kodefabrikken.Types.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void Type_is_correct_for_value_type()
        {
            var SUT = Option<int>.OptionType;

            SUT.Should().Be(typeof(int));
        }

        [TestMethod]
        public void Type_is_correct_for_reference_type()
        {
            var SUT = Option<OptionTests>.OptionType;

            SUT.Should().Be(typeof(OptionTests));
        }

        [TestMethod]
        public void Empty_for_Nullable_value_type_throws_exception()
        {
            Action act = () => _ = Option<Nullable<int>>.Empty;

            act.Should().Throw<InvalidOperationException>();

            // repeat test with nullable context
#nullable enable
            act = () => _ = Option<int?>.Empty;
#nullable disable

            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Empty_for_Option_type_throws_exception()
        {
            Action act = () => _ = Option<Option<int>>.Empty;

            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Empty_for_Value_type_has_no_value()
        {
            var SUT = Option<int>.Empty;

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_for_Nullable_reference_type_has_no_value()
        {
            // compiler transforms to wrapped type
#nullable enable
            var SUT = Option<OptionTests?>.Empty;
#nullable disable

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_is_created_for_value_type()
        {
            var SUT = Option<int>.Create();

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Empty_is_created_for_reference_type()
        {
            var SUT = Option<OptionTests>.Create();

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Option_is_created_for_value_type()
        {
            int i = 3;

            var SUT = Option<int>.Create(i);

            SUT.Should().NotBeNull();
            int v = 0;
            SUT.IfValue(p => v = p);
            v.Should().Be(i);
        }

        [TestMethod]
        public void Option_is_created_for_reference_type()
        {
            OptionTests x = new OptionTests();

            var SUT = Option<OptionTests>.Create(x);

            SUT.Should().NotBeNull();
            OptionTests v = null;
            SUT.IfValue(p => v = p);
            v.Should().BeSameAs(x);
        }

        [TestMethod]
        public void Option_is_created_for_null_reference_type()
        {
            OptionTests x = null;

            var SUT = Option<OptionTests>.Create(x);

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Value_type_is_cast_to_Option()
        {
            int i = 3;
            Option<int> SUT = i;

            SUT.Should().NotBeNull();
            int v = 0;
            SUT.IfValue(p => v = p);
            v.Should().Be(i);
        }

        [TestMethod]
        public void Reference_type_is_cast_to_Option()
        {
            OptionTests o = new OptionTests();
            Option<OptionTests> SUT = o;

            SUT.Should().NotBeNull();
            OptionTests v = null;
            SUT.IfValue(p => v = p);
            v.Should().BeSameAs(o);
        }

        [TestMethod]
        public void Null_reference_type_is_cast_to_Option()
        {
            OptionTests o = null;
            Option<OptionTests> SUT = o;

            SUT.Should().NotBeNull();
            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Value_type_has_correct_state()
        {
            var value = 3;
            var SUT = new Option<int>(value);

            SUT.HasValue.Should().BeTrue();
        }

        [TestMethod]
        public void Reference_type_has_correct_state()
        {
            var value = new OptionTests();
            var SUT = new Option<OptionTests>(value);

            SUT.HasValue.Should().BeTrue();
        }

        [TestMethod]
        public void Default_constructor_with_reference_type_has_no_value()
        {
            var SUT = new Option<OptionTests>();

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Default_constructor_with_value_type_has_no_value()
        {
            var SUT = new Option<int>();

            SUT.HasValue.Should().BeFalse();
        }

        [TestMethod]
        public void Construction_with_initialized_nullable_value_type_throws_exception()
        {
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Option<Nullable<int>>(3);
#pragma warning restore CA1806 // Do not ignore method results

            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Construction_with_empty_nullable_value_type_throws_exception()
        {
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Option<Nullable<int>>(null);
#pragma warning restore CA1806 // Do not ignore method results

            act.Should().Throw<ArgumentException>();
        }

        [TestMethod]
        public void Construction_with_initialized_nullable_reference_type_allowed()
        {
#nullable enable
            // compiler transforms to wrapped type
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Option<OptionTests?>(new OptionTests());
#pragma warning restore CA1806 // Do not ignore method results
#nullable disable

            act.Should().NotThrow();
        }

        [TestMethod]
        public void Construction_with_empty_nullable_reference_throws_exception()
        {
#nullable enable
            // compiler transforms to wrapped type
#pragma warning disable CA1806 // Do not ignore method results
            Action act = () => new Option<OptionTests?>(null);
#pragma warning restore CA1806 // Do not ignore method results
#nullable disable

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Registered_value_action_called_with_correct_value()
        {
            var value = 3;
            var SUT = new Option<int>(value);
            int paramValue = -1;

            SUT.IfValue(p => paramValue = p);

            paramValue.Should().Be(value);
        }

        [TestMethod]
        public void Registered_value_action_not_called_when_empty()
        {
            var SUT = Option<int>.Empty;
            bool isCalled = false;

            SUT.IfValue(_ => isCalled = true);

            isCalled.Should().BeFalse();
        }

        [TestMethod]
        public void Registered_value_action_called_when_value_and_context_used()
        {
            var SUT = new Option<int>(3);
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
            var SUT = Option<int>.Empty;
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
            var SUT = Option<int>.Empty;

            var result = SUT.Coalesce(value);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Value_type_with_value_is_not_coalsced()
        {
            int value = 3;
            var SUT = new Option<int>(value);

            var result = SUT.Coalesce(-1);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Value_type_is_coalesced_with_functor()
        {
            int value = 3;
            var SUT = Option<int>.Empty;

            var result = SUT.Coalesce(() => value);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Value_type_with_value_is_not_coalsced_with_functor()
        {
            int value = 3;
            var SUT = new Option<int>(value);

            var result = SUT.Coalesce(() => -1);

            result.Should().Be(value);
        }

        [TestMethod]
        public void Reference_type_is_coalesced()
        {
            OptionTests value = new OptionTests();
            var SUT = Option<OptionTests>.Empty;

            var result = SUT.Coalesce(value);

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Reference_type_with_value_is_not_coalesced()
        {
            OptionTests value = new OptionTests();
            var SUT = new Option<OptionTests>(value);

            var result = SUT.Coalesce(new OptionTests());

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Reference_type_is_coalesced_with_functor()
        {
            OptionTests value = new OptionTests();
            var SUT = Option<OptionTests>.Empty;

            var result = SUT.Coalesce(() => value);

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Reference_type_with_value_is_not_coalesced_with_functor()
        {
            OptionTests value = new OptionTests();
            var SUT = new Option<OptionTests>(value);

            var result = SUT.Coalesce(() => new OptionTests());

            result.Should().BeSameAs(value);
        }

        [TestMethod]
        public void Empty_reference_type_in_coalesce_throws()
        {
            OptionTests value = null;
            var SUT = Option<OptionTests>.Empty;

            Action act = () => SUT.Coalesce(value);

            act.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Coalesce_functor_returning_empty_reference_throws()
        {
            var SUT = Option<OptionTests>.Empty;

            Action act = () => SUT.Coalesce(() => null);

            act.Should().Throw<InvalidOperationException>();
        }

        [TestMethod]
        public void Option_are_equal_to_self()
        {
            var SUT = new Option<OptionTests>();

            // TODO : Are we boxing, should we implement Equals<T>?
            SUT.Equals(SUT).Should().BeTrue();
        }

        [TestMethod]
        public void Empty_option_are_equal_to_null()
        {
            var SUT = new Option<OptionTests>();

            SUT.Equals(null).Should().BeTrue();
        }

        [TestMethod]
        public void Different_value_options_are_not_equal()
        {
            var SUT1 = new Option<int>(1);
            var SUT2 = new Option<int>(2);

            SUT1.Equals(SUT2).Should().BeFalse();
        }

        [TestMethod]
        public void Option_should_be_equal_to_same_value()
        {
            var value = 3;
            var SUT = new Option<int>(value);

            SUT.Equals(value).Should().BeTrue();
        }

        [TestMethod]
        public void Options_of_different_type_should_not_be_equal()
        {
            var SUT1 = new Option<int>(7);
            var SUT2 = new Option<long>(7);

            SUT1.Equals(SUT2).Should().BeFalse();
        }

        [TestMethod]
        public void Option_is_equal_to_self()
        {
            var SUT = new Option<int>(1);

            var result = SUT == SUT;

            result.Should().BeTrue();
        }

        [TestMethod]
        public void Option_with_different_values_is_different()
        {
            var SUT1 = new Option<int>(1);
            var SUT2 = new Option<int>(2);

            var result = SUT1 != SUT2;

            result.Should().BeTrue();
        }
    }
}
