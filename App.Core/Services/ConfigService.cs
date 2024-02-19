using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class ConfigService
    {
        public string ActiveLanguage { get; set; }

        public string LogsFormat { get; set; }
       
        public ConfigService() 
        {
            ActiveLanguage =  ConfigurationManager.AppSettings["Language"];
            LogsFormat =  ConfigurationManager.AppSettings["LogsExtension"];
        }
    }   
}
