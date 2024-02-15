using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using App.Core.Models;

namespace App.Core.Services
{
    public class LoggerService
    {
        private readonly string logFilePath = "logs.json";
        private readonly string xmlLogFilePath = "logs.xml";
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        public void WriteLog(LoggerModel loggerModel, SaveModel saveModel)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNull(loggerModel);
            ArgumentNullException.ThrowIfNull(saveModel);

            try
            {
                loggerModel.Name = saveModel.SaveName;
                // Serialize the log model to JSON
                string jsonlogEntry = JsonSerializer.Serialize(loggerModel, options) + ",";
                // Append the log entry to the log file or create if not exist
                using StreamWriter LogWriter = File.AppendText(logFilePath);
                LogWriter.WriteLineAsync(jsonlogEntry);

                // Serialize the log model to XML
                using StringWriter xmlStringWriter = new();
                XmlSerializer xmlSerializer = new(typeof(LoggerModel));
                xmlSerializer.Serialize(xmlStringWriter, loggerModel);
                string xmlLogEntry = xmlStringWriter.ToString();
                // Append the XML log entry to the XML log file or create if not exist
                using StreamWriter xmlLogWriter = File.AppendText(xmlLogFilePath);
                xmlLogWriter.WriteLineAsync(xmlLogEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
