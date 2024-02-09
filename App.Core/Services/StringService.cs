using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
