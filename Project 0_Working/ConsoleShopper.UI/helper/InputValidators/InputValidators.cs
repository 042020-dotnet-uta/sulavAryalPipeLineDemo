using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ConsoleShopper.UI
{
    /// <summary>
    /// Extention methods around string to give it the power of validation. 
    /// </summary>
    public static class InputValidators
    {
        
        /// <summary>
        /// Checks if string is null or empty 
        /// or has anyother kind of whitespace not visible in the screen
        /// and returns a messsage if it is. 
        /// If the string is passes this test, it returns trimed and lowercased output. 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>string</returns>
        public static string Sanitize(this string str)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                return str.Trim().ToLower();
            }
            return "empty";
        }

        /// <summary>
        /// regex url: https://stackoverflow.com/questions/16699007/regular-expression-to-match-standard-10-digit-phone-number
        /// author: https://stackoverflow.com/users/1237040/ravi-thapliyal
        /// pattern match : 123-456-7890, (123) 456-7890, 123 456 7890, 123.456.7890, +91 (123) 456-7890
        /// </summary>
        /// <param name="str"></param>
        /// <returns>bool</returns>
        public static bool IsValidPhoneNumber(this string str)
        {

            Regex regex = new Regex(@"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$");
            Match match = regex.Match(str);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// regex url: https://stackoverflow.com/questions/2577236/regex-for-zip-code
        /// author: https://stackoverflow.com/users/224671/kennytm
        /// pattern match : 12345, 12345-6789, 12345 1234
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidZip(this string str)
        {

            Regex regex = new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
            Match match = regex.Match(str);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// String extention method to check password validation
        /// check results true if
        /// 1. the input received is between 8 and 16 characters
        /// 2. the input received as one or more small letters
        /// 3. the input received as one or more capital letters
        /// 4. the input received contains no spaces. 
        /// </summary>
        /// <param name="str"></param>
        /// <returns>bool</returns>
        public static bool IsValidPassword(this string str)
        {

            Regex regex = new Regex(@"(?=^.{8,15}$)(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?!.*\s).*$");
            Match match = regex.Match(str);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// String extention method to check valid Email.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidEmail(this string str)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(str);
            if (match.Success)
            {
                return true;
            }
            return false;
        }
    }
}
