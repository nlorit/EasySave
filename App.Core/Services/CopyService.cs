﻿using App.Core.Models;
using System;
using System.IO;

namespace App.Core.Services
{
    public class CopyService
    {
        private readonly LoggerService loggerService = new LoggerService();
        private readonly StateManagerService stateManagerService = new StateManagerService();
        private StateManagerModel stateModel = new StateManagerModel();

        public async Task UpdateStateFile(StateManagerModel stateModel, SaveModel saveModel)
        {
            await Task.Run(() => stateManagerService.UpdateState(stateModel, saveModel));
        }

        public void RunCopy(CopyModel model, SaveModel saveModel)
        {            
            Console.WriteLine(saveModel.SaveName + " is running...");
            stateModel.SaveName = saveModel.SaveName;
            stateModel.State = "ACTIVE";
            try
            {
                if (File.Exists(model.SourcePath))
                {
                    // Check if it's a complete or differential save
                    if (saveModel.Type == false || !File.Exists(saveModel.InPath))
                    {
                        // Complete save or source file not found for differential save
                        File.Copy(model.SourcePath, model.TargetPath, true);
                        Console.WriteLine("File copied successfully.");
                    }
                    else
                    {
                        // Differential save
                        FileInfo sourceFileInfo = new FileInfo(model.SourcePath);
                        FileInfo inFileInfo = new FileInfo(saveModel.InPath);

                        // Check if source file has been modified since the last save
                        if (sourceFileInfo.LastWriteTime > inFileInfo.LastWriteTime)
                        {
                            File.Copy(model.SourcePath, model.TargetPath, true);
                            Console.WriteLine("File copied successfully.");
                        }
                        else
                        {
                            LoggerModel loggerModel = new LoggerModel();

                            loggerModel.FileSource = model.SourcePath;
                            loggerModel.FileTarget = model.TargetPath;
                            loggerModel.FileSize = "";

                            loggerModel.FileTransferTime = "00:00:00.0000000";
                            loggerService.WriteLog(loggerModel, saveModel);
                            Console.WriteLine("Source file has not been modified since the last save.");
                        }
                    }
                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Source is a directory
                    string[] files = Directory.GetFiles(model.SourcePath);

                    if (files.Length > 0)
                    {
                        CopyDirectory(model.SourcePath, model.TargetPath, saveModel);
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
                    loggerService.WriteLog(loggerModel, saveModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            stateModel.State = "END";
            UpdateStateFile(stateModel, saveModel).Wait(); // Wait for the async operation to complete

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
