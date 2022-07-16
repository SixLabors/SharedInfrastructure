// Copyright (c) Six Labors.
// Licensed under the Six Labors Split License.

using System;
using SixLabors;
using Xunit;

namespace SharedInfrastructure.Tests
{
    public class GuardTests
    {
        private class Foo
        {
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
            var foo = new Foo();
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

        public static readonly TheoryData<int, int, bool> SizeCheckData = new TheoryData<int, int, bool>
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
            => Guard.MustBeLessThan(0, 1, "myParamName");

        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        public void MustBeLessThan_IsGreaterOrEqual_ThrowsNoException(int value, int max)
        {
            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeLessThan(value, max, "myParamName"));

            Assert.Equal("myParamName", exception.ParamName);
            Assert.Contains($"Parameter \"myParamName\" ({typeof(int)}) must be less than {max}, was {value}", exception.Message);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        public void MustBeLessThanOrEqualTo_IsLessOrEqual_ThrowsNoException(int value, int max)
            => Guard.MustBeLessThanOrEqualTo(value, max, "myParamName");

        [Fact]
        public void MustBeLessThanOrEqualTo_IsGreater_ThrowsNoException()
        {
            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeLessThanOrEqualTo(2, 1, "myParamName"));

            Assert.Equal("myParamName", exception.ParamName);
            Assert.Contains($"Parameter \"myParamName\" ({typeof(int)}) must be less than or equal to {1}, was {2}", exception.Message);
        }

        [Fact]
        public void MustBeGreaterThan_IsGreater_ThrowsNoException() => Guard.MustBeGreaterThan(2, 1, "myParamName");

        [Theory]
        [InlineData(1, 2)]
        [InlineData(1, 1)]
        public void MustBeGreaterThan_IsLessOrEqual_ThrowsNoException(int value, int min)
        {
            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeGreaterThan(value, min, "myParamName"));

            Assert.Equal("myParamName", exception.ParamName);
            Assert.Contains($"Parameter \"myParamName\" ({typeof(int)}) must be greater than {min}, was {value}", exception.Message);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(1, 1)]
        public void MustBeGreaterThanOrEqualTo_IsGreaterOrEqual_ThrowsNoException(int value, int min)
            => Guard.MustBeGreaterThanOrEqualTo(value, min, "myParamName");

        [Fact]
        public void MustBeGreaterThanOrEqualTo_IsLess_ThrowsNoException()
        {
            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeGreaterThanOrEqualTo(1, 2, "myParamName"));

            Assert.Equal("myParamName", exception.ParamName);
            Assert.Contains($"Parameter \"myParamName\" ({typeof(int)}) must be greater than or equal to {2}, was {1}", exception.Message);
        }

        [Theory]
        [InlineData(1, 1, 3)]
        [InlineData(2, 1, 3)]
        [InlineData(3, 1, 3)]
        public void MustBeBetweenOrEqualTo_IsBetweenOrEqual_ThrowsNoException(int value, int min, int max)
            => Guard.MustBeBetweenOrEqualTo(value, min, max, "myParamName");

        [Theory]
        [InlineData(0, 1, 3)]
        [InlineData(4, 1, 3)]
        public void MustBeBetweenOrEqualTo_IsLessOrGreater_ThrowsNoException(int value, int min, int max)
        {
            ArgumentOutOfRangeException exception =
                Assert.Throws<ArgumentOutOfRangeException>(() => Guard.MustBeBetweenOrEqualTo(value, min, max, "myParamName"));

            Assert.Equal("myParamName", exception.ParamName);
            Assert.Contains($"Parameter \"myParamName\" ({typeof(int)}) must be between or equal to {min} and {max}, was {value}", exception.Message);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        public void MustBeSizedAtLeast_Array_LengthIsGreaterOrEqual_ThrowsNoException(int valueLength, int minLength)
            => Guard.MustBeSizedAtLeast<int>(new int[valueLength], minLength, "myParamName");

        [Fact]
        public void MustBeSizedAtLeast_Array_LengthIsLess_ThrowsException()
        {
            ArgumentException exception =
                Assert.Throws<ArgumentException>(() => Guard.MustBeSizedAtLeast<int>(new int[] { 1, 2 }, 3, "myParamName"));

            Assert.Equal("myParamName", exception.ParamName);
            Assert.Contains("Spans must be at least of length 3", exception.Message);
        }
    }
}
