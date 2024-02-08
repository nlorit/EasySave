using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using App.Core.Models;

namespace App.Core.Services
{
    public class LoggerService
    {
        private readonly string logFilePath = "logs.json";
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void WriteLog(LoggerModel model, SaveModel saveModel)
        {
            try
            {
                model.Name = saveModel.SaveName;
                // Serialize the log model to JSON
                string logEntry = JsonSerializer.Serialize(model, options);
                // Append the log entry to the log file or create if not exist
                using (StreamWriter LogWriter = File.AppendText(logFilePath))
                {
                    LogWriter.WriteLineAsync(logEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}