using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NumberToWords.Core
{
    public static class MoneyToWordsConverter
    {
        private static readonly Regex ValidInput = new Regex(
            @"^-?\d+(\.\d+)?$",
            RegexOptions.Compiled
        );

              public static string Convert(string input)
        {
            if (input is null) throw new ArgumentNullException(nameof(input));
              if (input.Length == 0) throw new ArgumentException("Input cannot be empty.", nameof(input));
                          if (!ValidInput.IsMatch(input))
                throw new ArgumentException("Invalid format. Use digits, optional '.', no spaces, optional leading '-'.", nameof(input));

            if (!decimal.TryParse(input, NumberStyles.Number, CultureInfo.InvariantCulture, out var value))
                throw new ArgumentException("Invalid number.", nameof(input));

            bool isNegative = value < 0;
            value = Math.Abs(value);

          
            value = Math.Round(value, 2, MidpointRounding.AwayFromZero);

          
                long dollars = (long)decimal.Truncate(value);
                int cents = (int)decimal.Truncate((value - dollars) * 100m);

         
                    if (cents == 100)
                    {
                        dollars += 1;
                        cents = 0;
                    }

            string result = FormatMoneyWords(dollars, cents);

            if (isNegative)
                result = "Negative " + result;

            return result;  }

        private static string FormatMoneyWords(long dollars, int cents)
        {
            string dollarsWords = NumberToWords(dollars);
            string dollarsUnit = dollars == 1 ? "Dollar" : "Dollars";

            if (cents == 0)
                return $"{dollarsWords} {dollarsUnit}";

            string centsWords = NumberToWords(cents);
            string centsUnit = cents == 1 ? "Cent" : "Cents";

            return $"{dollarsWords} {dollarsUnit} And {centsWords} {centsUnit}";
        }

        
        private static string NumberToWords(long n)
        {
            if (n == 0) return "Zero";
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "NumberToWords expects a non-negative number.");

            string[] ones =
            {
                "Zero","One","Two","Three","Four","Five","Six","Seven","Eight","Nine",
                "Ten","Eleven","Twelve","Thirteen","Fourteen","Fifteen","Sixteen","Seventeen","Eighteen","Nineteen"
            };

            string[] tens =
            {
                "Zero","Ten","Twenty","Thirty","Forty","Fifty","Sixty","Seventy","Eighty","Ninety"
            };

            (long value, string name)[] scales =
            {
                (1_000_000_000_000_000_000, "Quintillion"),
                (1_000_000_000_000_000, "Quadrillion"),
                (1_000_000_000_000, "Trillion"),
                (1_000_000_000, "Billion"),
                (1_000_000, "Million"),
                (1_000, "Thousand")
            };

            string Below100(long x)
            {
                if (x < 20) return ones[x];

                long t = x / 10;
                long r = x % 10;

                if (r == 0) return tens[t];
                return $"{tens[t]}-{ones[r]}";
            }

            string Below1000(long x)
            {
                if (x < 100) return Below100(x);

                long h = x / 100;
                long r = x % 100;

                if (r == 0) return $"{ones[h]} Hundred";
                return $"{ones[h]} Hundred And {Below100(r)}";
            }

            var sb = new StringBuilder();
            long remaining = n;

            foreach (var (value, name) in scales)
            {
                if (remaining >= value)
                {
                    long chunk = remaining / value;
                    remaining %= value;

                    if (sb.Length > 0) sb.Append(' ');
                    sb.Append(Below1000(chunk));
                    sb.Append(' ');
                    sb.Append(name);
                }
            }

            if (remaining > 0)
            {
               
                if (sb.Length > 0 && remaining < 100)
                    sb.Append(" And ");
                else if (sb.Length > 0)
                    sb.Append(' ');

                sb.Append(Below1000(remaining));
            }

            return sb.ToString();
        }
    }
}
