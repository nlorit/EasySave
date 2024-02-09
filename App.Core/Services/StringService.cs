using System.Text.RegularExpressions;

namespace App.Core.Services
{
    public class StringService
    {

        public static bool IsCommaSeparatedOrHyphen(String input)
        {
            //True = Comma separated / False = Hyphen separated
            string pattern = @"^\d+,\d+$";
            return Regex.IsMatch(input, pattern);
        }

    }
}
