using App.Core.Models;
using System;
using System.IO;

namespace App.Core.Services
{
    public class CopyService
    {
        private readonly LoggerService loggerService = new LoggerService();

        public void RunCopy(CopyModel model, SaveModel saveModel)
        {
            try
            {
                if (File.Exists(model.SourcePath))
                {
                    // Copy file
                    File.Copy(model.SourcePath, model.TargetPath, true);
                    Console.WriteLine("File copied successfully.");
                }

                else if (Directory.Exists(model.SourcePath))
                {
                    // Source is a directory
                    string[] files = Directory.GetFiles(model.SourcePath);

                    if (files.Length > 0)
                    {
                        CopyDirectory(model.SourcePath,
                                      model.TargetPath, saveModel);
                        Console.WriteLine("Directory copied successfully.");

                        // Log an entry for each file in the directory
                        foreach (string filePath in files)
                        {
                            string destFilePath = Path.Combine(model.TargetPath, Path.GetFileName(filePath));
                            
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Source directory is empty.");
                    }
                }
                else
                {
                    Console.WriteLine("Source does not exist or is neither a file nor a directory.");
                    LoggerModel loggerModel = new LoggerModel();

                    loggerModel.FileSource = model.SourcePath;
                    loggerModel.FileTarget = model.TargetPath;
                    loggerModel.FileSize = "";

                    loggerModel.FileTransferTime = "-1";
                    loggerService.WriteLog(loggerModel, saveModel); ;
                    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }



        private void CopyDirectory(string sourceDir, string targetDir, SaveModel saveModel)
        {
            // Create the target directory if it doesn't exist
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            // Copy files
            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(targetDir, fileName);
                LoggerModel loggerModel = new LoggerModel();

                loggerModel.FileSource = filePath;
                loggerModel.FileTarget = destFilePath;
                loggerModel.FileSize = new FileInfo(filePath).Length / 1024.0 + " kb";

                try 
                {
                    File.Copy(filePath, destFilePath, true);
                    loggerModel.FileTransferTime = (DateTime.Now - loggerModel.Time).ToString();
                    loggerService.WriteLog(loggerModel, saveModel); ;

                }
                catch (Exception)
                {
                    loggerModel.FileTransferTime = "-1";
                    loggerService.WriteLog(loggerModel, saveModel); ;
                }
                

            }

            // Recursively copy subdirectories
            foreach (string subDirPath in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDirPath);
                string destSubDirPath = Path.Combine(targetDir, subDirName);
                CopyDirectory(subDirPath, destSubDirPath, saveModel);
            }
        }
    }
}
