﻿// Copyright 2018 Google LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     https://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Cloud.ClientTesting;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Xunit;

namespace Google.Cloud.BigQuery.V2.Tests
{
    public class BigQueryNumericTest
    {
        private const string MaxText = "99999999999999999999999999999.999999999";
        private const string MinText = "-99999999999999999999999999999.999999999";
        private const string EpsilonText = "0.000000001";
        private const string NegativeEpsilonText = "-0.000000001";

        [Theory, CombinatorialData]
        public void ConversionFromDecimal(
            [CombinatorialValues(
            "1",
            "10",
            "1.123456789",
            "79228162514264337593543950335", // decimal.MaxValue
            "1234567890123456789012345678"
            )]
            string text, bool negate)
        {
            if (negate)
            {
                text = "-" + text;
            }
            decimal parsed = decimal.Parse(text, CultureInfo.InvariantCulture);
            BigQueryNumeric numeric = (BigQueryNumeric)parsed;
            Assert.Equal(text, numeric.ToString());
        }

        [Theory]
        [InlineData("1.123456789123", "1.123456789")]
        [InlineData("-1.123456789123", "-1.123456789")]
        [InlineData("0.000000000000001", "0")]
        [InlineData("-0.000000000000001", "0")]
        public void ConversionFromDecimal_Lossy(string decimalText, string expectedText)
        {
            decimal input = decimal.Parse(decimalText, CultureInfo.InvariantCulture);
            BigQueryNumeric expected = BigQueryNumeric.Parse(expectedText);

            BigQueryNumeric conversionOutput = (BigQueryNumeric)input;
            BigQueryNumeric fromDecimalOutput = BigQueryNumeric.FromDecimal(input, LossOfPrecisionHandling.Truncate);

            Assert.Equal(expected, conversionOutput);
            Assert.Equal(expected, fromDecimalOutput);
            Assert.Throws<ArgumentException>(() => BigQueryNumeric.FromDecimal(input, LossOfPrecisionHandling.Throw));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        public void ConversionFromInt32(int input)
        {
            BigQueryNumeric value = input;
            Assert.Equal(input.ToString(CultureInfo.InvariantCulture), value.ToString());
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(-1L)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        public void ConversionFromInt64(long input)
        {
            BigQueryNumeric value = input;
            Assert.Equal(input.ToString(CultureInfo.InvariantCulture), value.ToString());
        }

        [Theory]
        [InlineData(0UL)]
        [InlineData(1UL)]
        [InlineData(ulong.MaxValue)]
        public void ConversionFromUInt64(ulong input)
        {
            BigQueryNumeric value = input;
            Assert.Equal(input.ToString(CultureInfo.InvariantCulture), value.ToString());
        }

        [Theory, CombinatorialData]
        public void ToDecimal_Precise(
            [CombinatorialValues(
                "79228162514264337593543950335", // decimal.MaxValue
                "0.000000001", // Epsilon for numeric
                "12345678901234567890.123456789" // Maximum significant digits with 9dps
            )]
            string text, LossOfPrecisionHandling handling, bool negate)
        {
            if (negate)
            {
                text = "-" + text;
            }
            BigQueryNumeric numeric = BigQueryNumeric.Parse(text);
            decimal expected = decimal.Parse(text, CultureInfo.InvariantCulture);
            decimal actual = numeric.ToDecimal(handling);
            Assert.Equal(expected, actual);
        }

        [Theory, CombinatorialData]
        public void ToDecimal_Overflow(LossOfPrecisionHandling handling, bool negate)
        {
            string text = "79228162514264337593543950336"; // decimal.MaxValue + 1
            BigQueryNumeric numeric = BigQueryNumeric.Parse(text);
            if (negate)
            {
                numeric = -numeric;
            }
            Assert.Throws<OverflowException>(() => numeric.ToDecimal(handling));
        }

        [Theory, CombinatorialData]
        public void ToDecimal_LossOfPrecision_Truncate(
            [CombinatorialValues(
                "79228162514264337593543950335.0000000001", // decimal.MaxValue + epsilon; doesn't count as overflow
                "123456789012345678900.123456789" // Simpler example of more significant digits than decimal can handle
            )]
            string text, bool negate)
        {
            if (negate)
            {
                text = "-" + text;
            }
            BigQueryNumeric numeric = BigQueryNumeric.Parse(text);
            // Decimal.Parse will silently lose precision
            decimal expected = decimal.Parse(text, CultureInfo.InvariantCulture);
            decimal actual = numeric.ToDecimal(LossOfPrecisionHandling.Truncate);
            Assert.Equal(expected, actual);
            // Conversion via the explicit conversion should do the same thing
            decimal actual2 = (decimal)numeric;
            Assert.Equal(expected, actual2);
        }

        [Theory, CombinatorialData]
        public void ToDecimal_LossOfPrecision_Throw(
            [CombinatorialValues(
                "79228162514264337593543950335.000000001", // decimal.MaxValue + epsilon; doesn't count as overflow
                "123456789012345678900.123456789" // Simpler example of more significant digits than decimal can handle
            )]
            string text, bool negate)
        {
            if (negate)
            {
                text = "-" + text;
            }
            BigQueryNumeric numeric = BigQueryNumeric.Parse(text);
            Assert.Throws<InvalidOperationException>(() => numeric.ToDecimal(LossOfPrecisionHandling.Throw));
        }

        [Theory, CombinatorialData]
        public void ParseToStringRoundTrip(
            [CombinatorialValues(
                "123456789012345678901234567.890123456",
                "79228162514264337593543950335.000000001",
                MaxText,
                EpsilonText,
                "5",
                "50",
                "50.01",
                "0.1",
                "0.123",
                "0.00123"
            )]
            string text, bool negate)
        {
            if (negate)
            {
                text = "-" + text;
            }
            BigQueryNumeric numeric = BigQueryNumeric.Parse(text);
            Assert.Equal(text, numeric.ToString());

            // Check that TryParse works too
            Assert.True(BigQueryNumeric.TryParse(text, out var numeric2));
            Assert.Equal(numeric, numeric2);
        }

        [Theory]
        [InlineData("")]
        [InlineData("-")]
        [InlineData("+")]
        [InlineData("non-digits")]
        [InlineData("   ")]
        [InlineData(" 0")]
        [InlineData("0 ")]
        [InlineData(" 0 ")]
        // Overflow
        [InlineData("100000000000000000000000000000")]
        [InlineData("-100000000000000000000000000000")]
        // Different parsing path but leading to overflow
        [InlineData("100000000000000000000000000000.0")]
        [InlineData("-100000000000000000000000000000.0")]
        // Things we may wish to make valid later
        [InlineData("+5")]
        [InlineData(".5")]
        [InlineData("-.5")]
        [InlineData("+.5")]
        [InlineData("1e6")]
        [InlineData("1e-6")]
        public void TryParse_Invalid(string text)
        {
            Assert.False(BigQueryNumeric.TryParse(text, out var value));
            Assert.Equal(BigQueryNumeric.Zero, value);
        }

        [Theory]
        [InlineData("1234,567", "es-ES")]
        [InlineData("1.234,567", "es-ES")]
        [InlineData("1234,567", "fr-FR")]
        public void TryParse_UnsupportedCultures(string text, string culture)
        {
            CultureInfo oldCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture(culture);

                // Just to demonstrate that these numbers are properly parsed with the given culture.
                // We don't parse them because we don't support the culture.
                Assert.True(Decimal.TryParse(text, out decimal decimalValue));

                Assert.False(BigQueryNumeric.TryParse(text, out var value));
                Assert.Equal(BigQueryNumeric.Zero, value);
            }
            finally
            {
                CultureInfo.CurrentCulture = oldCulture;
            }
        }

        [Fact]
        public void ZeroIsDefaultValue()
        {
            Assert.Equal("0", default(BigQueryNumeric).ToString());
        }

        [Fact]
        public void Constants()
        {
            Assert.Equal(MaxText, BigQueryNumeric.MaxValue.ToString());
            Assert.Equal(MinText, BigQueryNumeric.MinValue.ToString());
            Assert.Equal(EpsilonText, BigQueryNumeric.Epsilon.ToString());
            Assert.Equal("0", BigQueryNumeric.Zero.ToString());
        }

        [Theory]
        [InlineData(MaxText, new[] { MaxText }, new[] { "99999999999999999999999999999.999999998", "99999999999999999999999999999.999999998" })]
        [InlineData("1", new[] { "1.0", "1.00", "01" }, new[] { "-1", "0.999999999", "1.000000001" })]
        [InlineData("0", new[] { "0.0", "-0", "-0.0" }, new[] { "0.000000001", "-0.000000001" })]
        public void Equality(string controlText, string[] equalText, string[] unequalText)
        {
            BigQueryNumeric control = BigQueryNumeric.Parse(controlText);
            BigQueryNumeric[] equal = equalText.Select(BigQueryNumeric.Parse).ToArray();
            BigQueryNumeric[] unequal = unequalText.Select(BigQueryNumeric.Parse).ToArray();
            EqualityTester.AssertEqual(control, equal, unequal);
            EqualityTester.AssertEqualityOperators(control, equal, unequal);
        }

        [Theory]
        [InlineData("0", EpsilonText)]
        [InlineData(NegativeEpsilonText, "0")]
        [InlineData("-5", "2")]
        [InlineData("1.0", "1.1")]
        [InlineData("99999999999999999999999999999.99999998", MaxText)]
        [InlineData(MinText, "-99999999999999999999999999999.99999998")]
        public void CompareTo_Unequal(string smallerText, string biggerText)
        {
            var smaller = BigQueryNumeric.Parse(smallerText);
            var bigger = BigQueryNumeric.Parse(biggerText);
            Assert.InRange(smaller.CompareTo(bigger), int.MinValue, -1);
            Assert.InRange(bigger.CompareTo(smaller), 1, int.MaxValue);
            Assert.InRange(((IComparable)smaller).CompareTo(bigger), int.MinValue, -1);
            Assert.InRange(((IComparable)bigger).CompareTo(smaller), 1, int.MaxValue);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("-1")]
        [InlineData(MaxText)]
        [InlineData(MinText)]
        [InlineData(EpsilonText)]
        public void CompareTo_Equal(string text)
        {
            var value = BigQueryNumeric.Parse(text);
            // To compare with a different instance.
            var value1 = BigQueryNumeric.Parse(text);
            Assert.Equal(0, value.CompareTo(value));
            Assert.Equal(0, (IComparable)value.CompareTo(value));
            Assert.Equal(0, value1.CompareTo(value));
            Assert.Equal(0, (IComparable)value1.CompareTo(value));
        }

        [Fact]
        public void CompareTo_NegativeZero()
        {
            // These really are the same value... we don't differentiate.
            var regularZero = BigQueryNumeric.Zero;
            var negativeZero = BigQueryNumeric.Parse("-0");
            Assert.Equal(0, regularZero.CompareTo(negativeZero));
            Assert.Equal(0, (IComparable)regularZero.CompareTo(negativeZero));
            Assert.Equal(0, negativeZero.CompareTo(regularZero));
            Assert.Equal(0, (IComparable)negativeZero.CompareTo(regularZero));
        }

        [Fact]
        public void CompareTo_Null()
        {
            IComparable minValue = BigQueryNumeric.MinValue;
            Assert.InRange(minValue.CompareTo(null), 1, int.MaxValue);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("1.1")]
        [InlineData(MaxText)]
        public void UnaryNegation(string text)
        {
            var positive = BigQueryNumeric.Parse(text);
            var negative = BigQueryNumeric.Parse("-" + text);
            Assert.Equal(negative, -positive);
            Assert.Equal(positive, -negative);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1")]
        [InlineData("1.1")]
        [InlineData("-1.1")]
        [InlineData(MaxText)]
        public void UnaryPlus(string text)
        {
            var value = BigQueryNumeric.Parse(text);
            Assert.Equal(value, +value);
        }

        [Theory]
        [InlineData("2", "3", "5")]
        [InlineData("1", "0.1", "1.1")]
        [InlineData("1", "-0.1", "0.9")]
        [InlineData("-1", "0.1", "-0.9")]
        public void Addition_Valid(string leftText, string rightText, string expectedText)
        {
            var left = BigQueryNumeric.Parse(leftText);
            var right = BigQueryNumeric.Parse(rightText);
            var expected = BigQueryNumeric.Parse(expectedText);
            Assert.Equal(expected, left + right);
        }

        [Theory]
        [InlineData("2", "3", "-1")]
        [InlineData("1", "0.1", "0.9")]
        [InlineData("1", "-0.1", "1.1")]
        [InlineData("-1", "0.1", "-1.1")]
        public void Subtraction_Valid(string leftText, string rightText, string expectedText)
        {
            var left = BigQueryNumeric.Parse(leftText);
            var right = BigQueryNumeric.Parse(rightText);
            var expected = BigQueryNumeric.Parse(expectedText);
            Assert.Equal(expected, left - right);
        }

        [Theory]
        [InlineData("50000000000000000000000000000", "50000000000000000000000000000")]
        [InlineData("-50000000000000000000000000000", "-50000000000000000000000000000")]
        [InlineData(MaxText, EpsilonText)]
        [InlineData(MinText, NegativeEpsilonText)]
        public void Addition_Overflow(string leftText, string rightText)
        {
            var left = BigQueryNumeric.Parse(leftText);
            var right = BigQueryNumeric.Parse(rightText);
            Assert.Throws<OverflowException>(() => left + right);
        }

        [Theory]
        [InlineData("50000000000000000000000000000", "-50000000000000000000000000000")]
        [InlineData("-50000000000000000000000000000", "50000000000000000000000000000")]
        [InlineData(MaxText, NegativeEpsilonText)]
        [InlineData(MinText, EpsilonText)]
        public void Subtraction_Overflow(string leftText, string rightText)
        {
            var left = BigQueryNumeric.Parse(leftText);
            var right = BigQueryNumeric.Parse(rightText);
            Assert.Throws<OverflowException>(() => left - right);
        }
    }
}
