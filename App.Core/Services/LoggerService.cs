﻿using System.Text.Json;
using System.Xml.Serialization;
using App.Core.Models;

namespace App.Core.Services
{
    public class LoggerService
    {
        public LoggerModel? loggerModel;
        private readonly string jsonlogFilePath = "logs.json";
        private readonly string xmlLogFilePath = "logs.xml";
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        public LoggerService()
        {
            loggerModel = new();
        }


        public void WriteLog()
        {
            // Check for nulls
            //TODO : Gerer les exeptions

            try
            { 
                // Serialize the log model to JSON

                string logEntry = JsonSerializer.Serialize(loggerModel, options) + ",";
                string jsonlogEntry = JsonSerializer.Serialize(loggerModel, options) + ",";
                // Append the log entry to the log file or create if not exist
                using StreamWriter LogWriter = File.AppendText(jsonlogFilePath);
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
            /// <summary>
            /// Method to open the log file
            /// </summary>
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
            File.WriteAllText(jsonlogFilePath, "");
        }

        public void AddEntryLog()
        {
            using StreamWriter logWriter = File.AppendText(jsonlogFilePath);
            logWriter.WriteLineAsync(JsonSerializer.Serialize(logWriter, options) + ",");
        }
    }


}
