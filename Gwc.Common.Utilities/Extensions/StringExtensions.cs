using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace Gwc.Common.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string Add(this string s, string value)
        {
            return string.Concat(s, value);
        }

        public static string AddCurrencyFormat(this decimal value)
        {
            return value.ToString("c");
        }

        public static string AddCurrencyFormat(this double value)
        {
            return value.ToString("c");
        }

        public static string AddCurrencyFormat(this decimal? value)
        {
            return value == null ? null : ((decimal)value).ToString("c");
        }

        public static IEnumerable<int> GetUnicodeCodePoints(this string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                int unicodeCodePoint = char.ConvertToUtf32(s, i);
                if (unicodeCodePoint > 0xffff)
                {
                    i++;
                }
                yield return unicodeCodePoint;
            }
        }

        private static Dictionary<int,int> UnicodetoAsciiMapping()
        {
            Dictionary<int, int> mapping = new Dictionary<int, int>();
            mapping.Add(8364, 128);
            mapping.Add(8218,130);
            mapping.Add(402,131);
            mapping.Add(8222,132);
            mapping.Add(8230,133);
            mapping.Add(8224,134);
            mapping.Add(8225,135);
            mapping.Add(710,136);
            mapping.Add(8240,137);
            mapping.Add(352,138);
            mapping.Add(8249,139);
            mapping.Add(338,140);
            mapping.Add(381,142);
            mapping.Add(8216,145);
            mapping.Add(8217,146);
            mapping.Add(8220,147);
            mapping.Add(8221,148);
            mapping.Add(8226,149);
            mapping.Add(8211,150);
            mapping.Add(8212,151);
            mapping.Add(732,152);
            mapping.Add(8482,153);
            mapping.Add(353,154);
            mapping.Add(8250,155);
            mapping.Add(339,156);
            mapping.Add(382,158);
            mapping.Add(376,159);
            return mapping;
        }


        public static string Decrypt(this string strValue)
        {
            var mapping = UnicodetoAsciiMapping();

            short intRandom = 0;
            short x = 0;
            short intASC = 0;
            string strChar = null;
            string strDChar = null;
            string strReturn = "";
            short intRandom2 = 0;
            //strValue = strValue.Trim();
            //IntRandom2 is the ASCII value of the first character in the encrypted string
            intRandom2 = Encoding.Default.GetBytes(strValue[0].ToString())[0];
            //IntRandom is the ASCII value of the last character in the encrypted string
            intRandom = Encoding.Default.GetBytes(strValue.Substring(strValue.Length - 1)).First();
            //Remove the intrandom key characters
            strValue = strValue.Substring(1, strValue.Length - 2);
            //loop through each character in the string
            for (x = 1; x <= strValue.Length; x++)
            {
                //Get the next character
                strChar = strValue[x-1].ToString();
                //convert the character to its ASCII Value
                intASC = (short)strChar.ToCharArray()[0];

                if (intASC > 255)
                {
                    if (mapping.ContainsKey(intASC))
                    {
                        var map = mapping.TryGetValue(intASC, out int outVal);
                        intASC = (short)outVal;
                    }
                    else
                    {
                        throw new Exception($"No mapping found for Unicode value {intASC}");
                    }
                    
                }

                if (x % 2 == 0)
                {
                    //if x is even
                    if (intRandom % 2 == 0)
                    {
                        //If intRandom Is even Then
                        strDChar = Convert.ToChar(Convert.ToInt16(intASC - (intRandom / 2))).ToString();
                    }
                    else if (intRandom % 3 == 0)
                    {
                        //if intrandom is divisible by 3
                        strDChar = Convert.ToChar(Convert.ToInt16(intASC - (intRandom / 3))).ToString();
                    }
                    else
                    {
                        strDChar = Convert.ToChar(Convert.ToInt16(intASC - intRandom)).ToString();
                    }
                }
                else if (x % 3 == 0)
                {
                    //if x is divisible by 3
                    if (intRandom % 2 == 0)
                    {
                        //If intRandom Is even Then
                        strDChar = Convert.ToChar(Convert.ToInt16(intASC - (intRandom / 2))).ToString();
                    }
                    else if (intRandom % 3 == 0)
                    {
                        //if intrandom is divisible by 3
                        strDChar = Convert.ToChar(Convert.ToInt16(intASC - (intRandom / 3))).ToString();
                    }
                    else
                    {
                        strDChar = Convert.ToChar(Convert.ToInt16(intASC - intRandom2)).ToString();
                    }
                }
                else
                {
                    strDChar = Convert.ToChar(Convert.ToInt16(intASC - intRandom)).ToString();
                }
                strReturn = strReturn + strDChar;
            }
            //
            return strReturn;
        }


        public static string Encrypt(this string strValue)
        {
            var mapping = UnicodetoAsciiMapping();
            short x = 0;
            short intASC = 0;
            string strChar = null;
            string strEChar = null;
            string strReturn = "";
            short intRandom = 0;
            short intRandom2 = 0;

            Random rnd = new Random();


            intRandom = Convert.ToInt16(rnd.Next(1, 63));
            // Generate random value between 1 and 63.
            intRandom2 = Convert.ToInt16(rnd.Next(1, 63)); 
            // Generate random value between 1 and 63.
            //strReturn = Chr(intRandom)
            //Loop through each character in string
            for (x = 1; x <= strValue.Length; x++)
            {
                //Read each character
                strChar = strValue[x - 1].ToString();
                //Retrieve ASCII value for character
                intASC = (short)strChar.ToCharArray()[0];

                //use unicode mapping for special characters
                if (intASC > 255)
                {
                    if (mapping.ContainsKey(intASC))
                    {
                        var map = mapping.TryGetValue(intASC, out int outVal);
                        intASC = (short)outVal;
                    }
                    else
                    {
                        throw new Exception($"No mapping found for Unicode value {intASC}");
                    }
                }

                if (x % 2 == 0)
                {
                    //if x is an even number
                    if (intRandom % 2 == 0)
                    {
                        //if intRandom is an even number
                        // Encrypted character is the ASCII Equivalent of 
                        //   1/2 of intRandom + the ASCII Value of the original Character
                        strEChar = Convert.ToChar(Convert.ToInt16(intASC + (intRandom / 2))).ToString();
                    }
                    else if (intRandom % 3 == 0)
                    {
                        //if intrandom is divisible by 3
                        // Encrypted character is the ASCII Equivalent of 
                        //   1/3 of intRandom + the ASCII Value of the original Character
                        strEChar = Convert.ToChar(Convert.ToInt16(intASC + (intRandom / 3))).ToString();
                    }
                    else
                    {
                        // Encrypted character is the ASCII Equivalent of 
                        //   intRandom + the ASCII Value of the original Character
                        strEChar = Convert.ToChar(Convert.ToInt16(intASC + intRandom)).ToString();
                    }

                }
                else if (x % 3 == 0)
                {
                    //if x is divisible by 3
                    if (intRandom % 2 == 0)
                    {
                        //if intRandom is an even number
                        // Encrypted character is the ASCII Equivalent of 
                        //   1/2 of intRandom + the ASCII Value of the original Character
                        strEChar = Convert.ToChar(Convert.ToInt16(intASC + (intRandom / 2))).ToString();
                    }
                    else if (intRandom % 3 == 0)
                    {
                        //if intrandom is divisible by 3
                        // Encrypted character is the ASCII Equivalent of 
                        //   1/3 of intRandom + the ASCII Value of the original Character
                        strEChar = Convert.ToChar(Convert.ToInt16(intASC + (intRandom / 3))).ToString();
                    }
                    else
                    {
                        // Encrypted character is the ASCII Equivalent of 
                        //   intRandom2 + the ASCII Value of the original Character
                        strEChar = Convert.ToChar(Convert.ToInt16(intASC + intRandom2)).ToString();
                    }
                }
                else
                {
                    // Encrypted character is the ASCII Equivalent of 
                    //   intRandom + the ASCII Value of the original Character
                    strEChar = Convert.ToChar(Convert.ToInt16(intASC + intRandom)).ToString();
                }
                strReturn = strReturn + strEChar;
            }
            //place then ASCII equivalent of intrandom2 as the first character in the encrypted string
            //place then ASCII equivalent of intrandom as the last character in the encrypted string
            return Convert.ToChar(intRandom2) + strReturn + Convert.ToChar(intRandom);
        }


        public static string EmptyIfNull(this string input)
        {
            var retval = input;

            if (string.IsNullOrEmpty(retval))
            {
                return string.Empty;
            }

            return retval;
        }

        public static string Endpoint(this string s, string endpoint)
        {
            return string.Concat(s, endpoint);
        }

        public static string Extract(this string s, params object[] args)
        {
            var firstPosition = s.IndexOf(args[0].ToString(), StringComparison.Ordinal);
            var lastPostion = s.IndexOf(args[1].ToString(), StringComparison.Ordinal);

            if (lastPostion < firstPosition) return string.Empty;

            return s.Substring(firstPosition, lastPostion - firstPosition + 1);
        }

        public static string FirstName(this string s)
        {
            var spacePosition = s.Trim().IndexOf(" ", StringComparison.Ordinal);
            return spacePosition == -1 ? s : s.Trim().Substring(0, spacePosition);
        }

        public static string FormatString(this string s, params object[] args)
        {
            return string.Format(s, args);
        }

        public static string FormatTextToHtml(this string s)
        {
            return  "<pre>" + s + "</pre>";
        }

        public static bool IsNotNullOrEmpty(this string input)
        {
            return !string.IsNullOrEmpty(input);
        }

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNumeric(this string theValue)
        {
            long retNum;
            return long.TryParse(theValue, NumberStyles.Integer, NumberFormatInfo.InvariantInfo, out retNum);
        }

        public static string LastName(this string s)
        {
            var spacePosition = s.Trim().IndexOf(" ", StringComparison.Ordinal);
            return spacePosition == -1 ? s : s.Trim().Substring(spacePosition + 1);
        }

        public static string Left(this string s, int length)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            length = Math.Max(length, 0);
            if (length == 0) return string.Empty;
            return s.Length > length ? s.Substring(0, length) : s;
        }

        public static string Mask(this string s, string mask, int lengthToShow)
        {
            if (s.Length < lengthToShow) return s;
            return string.Join("", Enumerable.Repeat(mask, s.Length - lengthToShow)) + s.Right(lengthToShow);
        }

        public static string Right(this string s, int length)
        {
            length = Math.Max(length, 0);
            return s.Length > length ? s.Substring(s.Length - length, length) : s;
        }

        public static DateTime? ToDateTime(this string s, bool removeTimeZone = false)
        {
            DateTime dtr;
            var dateTime = removeTimeZone ? s.Left(20) : s;
            var tryDtr = DateTime.TryParse(dateTime, out dtr);
            return (tryDtr) ? dtr : new DateTime?();
        }

        public static DateTime? ToDateTimeWithFormat(this string s, string format)
        {
            DateTime dtr;
            var tryDtr = DateTime.TryParseExact(s,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                 out dtr);
            return (tryDtr) ? dtr : new DateTime?();
        }

        public static int ToInt(this string value)
        {
            int number;
            int.TryParse(value, out number);
            return number;
        }

        public static long ToLong(this string value)
        {
            long number;
            long.TryParse(value, out number);
            return number;
        }

        public static string ToUpperCheckForNull(this string input)
        {
            var retval = input;

            if (!string.IsNullOrEmpty(retval))
            {
                retval = retval.ToUpper().Trim();
            }

            return retval;
        }

        public static string UrlEncode(this string value)
        {
            return WebUtility.UrlEncode(value);
        }

        #region Phone extensions

        public static string AddPhoneFormat(this string phone)
        {
            if (phone == null) return null;
            return Regex.Replace(phone, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3");
        }
        public static string Ext(this string s)
        {
            return s.Right(4);
        }
        public static string Npa(this string s)
        {
            return s.Left(3);
        }

        public static string Nxx(this string s)
        {
            return s.Substring(3, 3);
        }
        public static string RemovePhoneFormat(this string phone)
        {
            if (phone.IsNullOrEmpty()) return string.Empty;
            var retPhone = Regex.Replace(phone, "[^.0-9]", "");
            retPhone = retPhone.Replace(" ", "").Trim();
            return retPhone;
        }

        #endregion Phone extensions
    }
}