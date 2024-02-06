﻿using App.Core.Models;
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
                    string destFilePath = Path.Combine(model.TargetPath, Path.GetFileName(model.SourcePath));
                    File.Copy(model.SourcePath, destFilePath, true);
                    Console.WriteLine("File copied successfully.");
                    loggerService.WriteLog(new LoggerModel { FileSource = model.SourcePath, FileTarget = destFilePath }, saveModel);
                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Source is a directory
                    string[] files = Directory.GetFiles(model.SourcePath);

                    if (files.Length > 0)
                    {
                        CopyDirectory(model.SourcePath,
                                      model.TargetPath);
                        Console.WriteLine("Directory copied successfully.");

                        // Log an entry for each file in the directory
                        foreach (string filePath in files)
                        {
                            string destFilePath = Path.Combine(model.TargetPath, Path.GetFileName(filePath));
                            loggerService.WriteLog(new LoggerModel
                            {
                                FileSource = filePath,
                                FileTarget = destFilePath,
                                FileSize = new FileInfo(filePath).Length/1024.0 + " kb"
                            }, saveModel);
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
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }



        private void CopyDirectory(string sourceDir, string targetDir)
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
                File.Copy(filePath, destFilePath, true);
            }

            // Recursively copy subdirectories
            foreach (string subDirPath in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDirPath);
                string destSubDirPath = Path.Combine(targetDir, subDirName);
                CopyDirectory(subDirPath, destSubDirPath);
            }
        }
    }
}
