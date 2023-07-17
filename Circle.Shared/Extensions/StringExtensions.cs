using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Circle.Shared.Extensions
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
        public static string IncrementalRandomNumber(this string value)
        {
            return DateTime.Now.ToString("yyMMddHHmmssff");
        }

        public static string ReceiptNo(this string value)
        {
            return DateTime.Now.ToString("yyMMddHHmmssff");
        }

        public static (string FirstName, string LastName) FullNameSplitter(this string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return (null, null);

            var names = fullName.Split(' ');
            string firstName = names[0];
            string lastName = names.Length > 1 ? names[1] : null;
            return (firstName, lastName);
        }

        public static bool IsValidPhoneNumber(this string value)
        {
            var regex = new Regex(@"^(0|234)\d{6,14}$");
            return regex.IsMatch(value);
        }

        public static bool IsValidJson(this string strInput)
        {
            strInput = strInput.Trim();
            if (strInput.StartsWith("{") && strInput.EndsWith("}") || //For object
                strInput.StartsWith("[") && strInput.EndsWith("]")) //For array
            {
                try
                {
                    var obj = JsonSerializer.Serialize(strInput);
                    return true;
                }

                catch (Exception) //some other exception
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static string MaskDigit(this string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return "**************";

            var firstDigits = cardNumber.Substring(0, 3);
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

            var requiredMask = new string('*', cardNumber.Length - firstDigits.Length - lastDigits.Length);

            var maskedString = string.Concat(firstDigits, requiredMask, lastDigits);
            var maskedCardNumberWithSpaces = Regex.Replace(maskedString, ".{4}", "$0 ");

            return maskedString;
        }
        public static string MaskInput(this string input, int startIndex = 3, string mask = "******")
        {
            //if (string.IsNullOrEmpty(input))
            //    return string.Empty;

            //string result = input;
            //int starLengh = mask.Length;


            //if (input.Length >= startIndex)
            //{
            //    result = input.Insert(startIndex, mask);
            //    if (result.Length >= (startIndex + starLengh * 2))
            //        result = result.Remove((startIndex + starLengh), starLengh);
            //    else
            //        result = result.Remove((startIndex + starLengh), result.Length - (startIndex + starLengh));

            //}

            //return result;
            var str = input;
            if (str.Length > 4)
            {

                str = string.Concat(
                      "".PadLeft(12, '*'),
                      str.Substring(str.Length - 2)
                  );

            }

            return str;
        }

        public static bool IsImageExtension(this string value)
        {
            string AllImages = "Image file|" + "*.png; *.jpg; *.jpeg; *.jfif; *.bmp;*.tif; *.tiff; *.gif";
            return AllImages.Contains(value);
        }

        public static bool IsValidDateTime(this string value, string format = null)
        {
            DateTimeFormatInfo dtfi = DateTimeFormatInfo.InvariantInfo;

            if (string.IsNullOrWhiteSpace(format))
                return DateTime.TryParseExact(value, format, dtfi, DateTimeStyles.RoundtripKind, out _);
            else
                return DateTime.TryParseExact(value, format, dtfi, DateTimeStyles.None, out _);
        }

        public static bool IsValidEmail(this string value)
        {
            var regex = new Regex(@"(\w+\.)*\w+@(\w+\.)+[A-Za-z]+");
            return regex.IsMatch(value);
        }

        public static string RemovePreFix(this string str, params string[] preFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (preFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var preFix in preFixes)
            {
                if (str.StartsWith(preFix))
                {
                    return str.Right(str.Length - preFix.Length);
                }
            }

            return str;
        }

        public static string RemovePostFix(this string str, params string[] postFixes)
        {
            if (str == null)
            {
                return null;
            }

            if (str == string.Empty)
            {
                return string.Empty;
            }

            if (postFixes.IsNullOrEmpty())
            {
                return str;
            }

            foreach (var postFix in postFixes)
            {
                if (str.EndsWith(postFix))
                {
                    return str.Left(str.Length - postFix.Length);
                }
            }

            return str;
        }

        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(0, len);
        }

        public static string Right(this string str, int len)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            if (str.Length < len)
            {
                throw new ArgumentException("len argument can not be bigger than given string's length!");
            }

            return str.Substring(str.Length - len, len);
        }
    }
}
