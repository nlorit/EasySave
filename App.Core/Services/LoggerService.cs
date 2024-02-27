using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Xml.Serialization;
using App.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.Core.Services
{
    public class LoggerService
    {
        private readonly string jsonlogFilePath = "logs.json";
        private readonly string xmlLogFilePath = "logs.xml";
        private readonly ConfigService configService = new ConfigService();

        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        private StreamWriter? streamWriter;

        public LoggerService()
        {
        }

        public void OpenLogFile()
        {
            // Open the log file in notepad
            System.Diagnostics.Process.Start("notepad.exe", jsonlogFilePath);
        }
            
        public void ClearLogFile()
        {
            // Clear the log file
            File.WriteAllText(jsonlogFilePath, "[]");
        }

        public void CreateLogFile()
        {
            // Create the log file
            File.WriteAllText(jsonlogFilePath, "[]");
        }

        public string GetLogFormat()
        {
            if (configService.LogsFormat == "JSON") return jsonlogFilePath;
            else return xmlLogFilePath;
        }

        public void AddEntryLog(LoggerModel loggerModel)
        {
            if (configService.LogsFormat == "JSON")
            {
                try
                {
                    using (streamWriter = File.AppendText(jsonlogFilePath)) ;
                    { 
                        streamWriter.WriteLineAsync(JsonSerializer.Serialize(loggerModel, options)); 
                    }
                }
                catch (InvalidOperationException)
                {
                    CreateLogFile();
                }
            }
            else
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(LoggerModel));

                    using (streamWriter = File.AppendText(xmlLogFilePath))
                    {
                        xmlSerializer.Serialize(streamWriter, loggerModel );
                    }
                }
                catch (InvalidOperationException)
                {
                    CreateLogFile();
                }
            }
        }
    }


}
