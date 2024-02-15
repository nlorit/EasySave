using System.IO;
using System.Text.Json;
using App.Core.Models;

namespace App.Core.Services
{
    public class LoggerService
    {
        private readonly string logFilePath = "logs.json";
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Method to write a log entry
        /// </summary>
        /// <param name="loggerModel"></param>
        /// <param name="saveModel"></param>
        public void WriteLog(LoggerModel loggerModel, SaveModel saveModel)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNull(loggerModel);
            ArgumentNullException.ThrowIfNull(saveModel);

            try
            {
                loggerModel.Name = saveModel.SaveName;
                // Serialize the log model to JSON
                string logEntry = JsonSerializer.Serialize(loggerModel, options) + ",";
                // Append the log entry to the log file or create if not exist
                using StreamWriter LogWriter = File.AppendText(logFilePath);
                LogWriter.WriteLineAsync(logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
        /// <summary>
        /// Method to open the log file
        /// </summary>
        public void OpenLogFile()
        {
            // Open the log file in notepad
            System.Diagnostics.Process.Start("notepad.exe", logFilePath);
        }
    }
}