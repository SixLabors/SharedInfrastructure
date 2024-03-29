// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

// Tell this file to enable debug conditional method calls, i.e. all the debug guard calls
#define DEBUG

using System.Diagnostics;
using System.Reflection;

namespace SharedInfrastructure.Tests;

public class DebugGuardTests
{
    private class Foo
    {
    }

    [Fact]
    public void AllStaticMethodsOnOnDebugGuardHaveDEBUGConditional()
    {
        IEnumerable<MethodInfo> methods = typeof(DebugGuard).GetTypeInfo().GetMethods()
            .Where(x => x.IsStatic);

        foreach (MethodInfo m in methods)
        {
            IEnumerable<ConditionalAttribute> attribs = m.GetCustomAttributes<ConditionalAttribute>();
            Assert.True(attribs.Select(x => x.ConditionString).Contains("DEBUG"), $"Method '{m.Name}' does not have [Conditional(\"DEBUG\")] set.");
        }
    }

    [Fact]
    public void NotNull_WhenNull_Throws()
    {
        Foo foo = null;
        Assert.Throws<ArgumentNullException>(() => Guard.NotNull(foo, nameof(foo)));
    }

    [Fact]
    public void NotNull_WhenNotNull()
    {
        Foo foo = new();
        Guard.NotNull(foo, nameof(foo));
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("  ", true)]
    [InlineData("$", false)]
    [InlineData("lol", false)]
    public void NotNullOrWhiteSpace(string str, bool shouldThrow)
    {
        if (shouldThrow)
        {
            Assert.ThrowsAny<ArgumentException>(() => Guard.NotNullOrWhiteSpace(str, nameof(str)));
        }
        else
        {
            Guard.NotNullOrWhiteSpace(str, nameof(str));
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsTrue(bool value)
    {
        if (!value)
        {
            Assert.Throws<ArgumentException>(() => Guard.IsTrue(value, nameof(value), "Boo!"));
        }
        else
        {
            Guard.IsTrue(value, nameof(value), "Boo.");
        }
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsFalse(bool value)
    {
        if (value)
        {
            Assert.Throws<ArgumentException>(() => Guard.IsFalse(value, nameof(value), "Boo!"));
        }
        else
        {
            Guard.IsFalse(value, nameof(value), "Boo.");
        }
    }

    public static readonly TheoryData<int, int, bool> SizeCheckData = new()
    {
        { 0, 0, false },
        { 1, 1, false },
        { 1, 0, false },
        { 13, 13, false },
        { 20, 13, false },
        { 12, 13, true },
        { 0, 1, true },
    };

    [Theory]
    [MemberData(nameof(SizeCheckData))]
    public void MustBeSizedAtLeast(int length, int minLength, bool shouldThrow)
    {
        int[] data = new int[length];

        if (shouldThrow)
        {
            Assert.Throws<ArgumentException>(() => Guard.MustBeSizedAtLeast((Span<int>)data, minLength, nameof(data)));
            Assert.Throws<ArgumentException>(() => Guard.MustBeSizedAtLeast((ReadOnlySpan<int>)data, minLength, nameof(data)));
        }
        else
        {
            Guard.MustBeSizedAtLeast((Span<int>)data, minLength, nameof(data));
            Guard.MustBeSizedAtLeast((ReadOnlySpan<int>)data, minLength, nameof(data));
        }
    }

    [Theory]
    [MemberData(nameof(SizeCheckData))]
    public void DestinationShouldNotBeTooShort(int destLength, int sourceLength, bool shouldThrow)
    {
        int[] dest = new int[destLength];
        int[] source = new int[sourceLength];

        if (shouldThrow)
        {
            Assert.Throws<ArgumentException>(() => Guard.DestinationShouldNotBeTooShort((Span<int>)source, (Span<int>)dest, nameof(dest)));
            Assert.Throws<ArgumentException>(() => Guard.DestinationShouldNotBeTooShort((ReadOnlySpan<int>)source, (Span<int>)dest, nameof(dest)));
        }
        else
        {
            Guard.DestinationShouldNotBeTooShort((Span<int>)source, (Span<int>)dest, nameof(dest));
            Guard.DestinationShouldNotBeTooShort((ReadOnlySpan<int>)source, (Span<int>)dest, nameof(dest));
        }
    }

    [Fact]
    public void MustBeLessThan_IsLess_ThrowsNoException()
        => DebugGuard.MustBeLessThan(0, 1, "myParamName");

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 1)]
    public void MustBeLessThan_IsGreaterOrEqual_ThrowsNoException(int value, int max)
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => DebugGuard.MustBeLessThan(value, max, "myParamName"));

        Assert.Equal("myParamName", exception.ParamName);
        Assert.Contains($"Value {value} must be less than {max}.", exception.Message);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 1)]
    public void MustBeLessThanOrEqualTo_IsLessOrEqual_ThrowsNoException(int value, int max)
        => DebugGuard.MustBeLessThanOrEqualTo(value, max, "myParamName");

    [Fact]
    public void MustBeLessThanOrEqualTo_IsGreater_ThrowsNoException()
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => DebugGuard.MustBeLessThanOrEqualTo(2, 1, "myParamName"));

        Assert.Equal("myParamName", exception.ParamName);
        Assert.Contains("Value 2 must be less than or equal to 1.", exception.Message);
    }

    [Fact]
    public void MustBeGreaterThan_IsGreater_ThrowsNoException()
        => DebugGuard.MustBeGreaterThan(2, 1, "myParamName");

    [Theory]
    [InlineData(1, 2)]
    [InlineData(1, 1)]
    public void MustBeGreaterThan_IsLessOrEqual_ThrowsNoException(int value, int min)
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => DebugGuard.MustBeGreaterThan(value, min, "myParamName"));

        Assert.Equal("myParamName", exception.ParamName);
        Assert.Contains($"Value {value} must be greater than {min}.", exception.Message);
    }

    [Theory]
    [InlineData(2, 1)]
    [InlineData(1, 1)]
    public void MustBeGreaterThanOrEqualTo_IsGreaterOrEqual_ThrowsNoException(int value, int min)
        => DebugGuard.MustBeGreaterThanOrEqualTo(value, min, "myParamName");

    [Fact]
    public void MustBeGreaterThanOrEqualTo_IsLess_ThrowsNoException()
    {
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(
            () => DebugGuard.MustBeGreaterThanOrEqualTo(1, 2, "myParamName"));

        Assert.Equal("myParamName", exception.ParamName);
        Assert.Contains("Value 1 must be greater than or equal to 2.", exception.Message);
    }

    [Theory]
    [InlineData(new int[] { 1, 2 }, 1)]
    [InlineData(new int[] { 1, 2 }, 2)]
    public void MustBeSizedAtLeast_Array_LengthIsGreaterOrEqual_ThrowsNoException(int[] value, int minLength)
        => DebugGuard.MustBeSizedAtLeast<int>(value, minLength, "myParamName");

    [Fact]
    public void MustBeSizedAtLeast_Array_LengthIsLess_ThrowsException()
    {
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => DebugGuard.MustBeSizedAtLeast<int>(new int[] { 1, 2 }, 3, "myParamName"));

        Assert.Equal("myParamName", exception.ParamName);
        Assert.Contains("The size must be at least 3.", exception.Message);
    }
}
