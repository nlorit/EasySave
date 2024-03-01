using System.Configuration;

namespace App.Core.Services
{
    public class ConfigService
    {

        public string ActiveLanguage { get; set; } 
        public string Software { get; set; }
        public string LogsFormat { get; set; }
        public string Priority { get; set; }
       
        public ConfigService() 
        {
            ActiveLanguage = ConfigurationManager.AppSettings["Language"]!;
            Software = ConfigurationManager.AppSettings["LogicielMetier"]!;
            LogsFormat =  ConfigurationManager.AppSettings["LogsExtension"]!;
            Priority = ConfigurationManager.AppSettings["priority"]!;

        }
    }   
}
