using System.Text.RegularExpressions;

namespace App.Core.Services
{
    public class StringService
    {
        //Method to check if the input is a comma separated or hyphen separated
        public static bool IsCommaSeparatedOrHyphen(string input)
        {
            string Pattern = @"^\d+,\d+$";
            return Regex.IsMatch(input, Pattern); //True = Comma separated / False = Hyphen separated
        }

    }
}
