using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Core.Services
{
    public class LoggerService
    {
        public void WriteLog(String message)
        {
            Console.WriteLine(message);
        }

        public void WriteError(String message)
        {
            Console.WriteLine("Error: "+ message);

        }
        public void WriteInfo(String message)
        {
            Console.WriteLine("Info: "+ message);

        }
        public void WriteWarning(String message)
        {
               Console.WriteLine("Warning: "+ message);
        }

        public void ClearLog()
        {
            Console.Clear();
        }
    }
}
