using System;

namespace NSTOX.BODataProcessor.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveTrailComma(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return text.Trim(",".ToCharArray()).Trim();
        }

        public static string RemoveTrailCommaAndSanitize(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            return text.Replace("'", "`").TrimEnd(",".ToCharArray()).Trim();
        }

        public static long ToLong(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            return Convert.ToInt64(text.RemoveTrailComma());
        }

        public static int ToInt(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            return Convert.ToInt32(text.RemoveTrailComma());
        }

        public static int? ToNullableInt(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            text = text.RemoveTrailComma();

            int result = 0;

            if (Int32.TryParse(text, out result))
            {
                return result;
            }
            return null;
        }

        public static double ToDouble(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            text = text.RemoveTrailComma();

            double result = 0;
            double.TryParse(text, out result);

            return result;
        }

        public static bool ToBool(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            text = text.RemoveTrailComma();

            return text == "1" ? true : false;
        }
    }
}
