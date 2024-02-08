using App.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class CopyService
    {

        private readonly LoggerService loggerService = new LoggerService();
        private readonly StateManagerService stateManagerService = new StateManagerService();
        private StateManagerModel stateModel = new StateManagerModel();

        public void RunCopy(CopyModel model, SaveModel saveModel, List<SaveModel> saves)
        {
            Console.WriteLine(saveModel.SaveName + " is running...");

            try
            {

                if (File.Exists(model.SourcePath))
                {
                    // File copying operation
                    File.Copy(model.SourcePath, model.TargetPath, true);
                    Console.WriteLine("File copied successfully.");

                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Directory copying operation
                    CopyDirectory(model.SourcePath, model.TargetPath, saveModel, saves);
                    Console.WriteLine("Directory copied successfully.");
                    
                }
                else
                {
                    Console.WriteLine("Source does not exist or is neither a file nor a directory.");
                    // Log an entry for non-existing source
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

        }
        private void CopyDirectory(string sourceDirPath, string targetDirPath, SaveModel saveModel, List<SaveModel> saves)
        {
            int totalFiles = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length;
            long totalSize = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);

            // Copy files recursively
            foreach (string filePath in Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(targetDirPath, fileName);

                LoggerModel logModel = new LoggerModel
                {
                    Name = saveModel.SaveName,
                    FileSource = filePath,
                    FileTarget = destFilePath,
                    FileSize = new FileInfo(filePath).Length.ToString()
                };

                File.Copy(filePath, destFilePath, true);

                logModel.FileTransferTime = (DateTime.Now - logModel.Time).ToString();
                loggerService.WriteLog(logModel, saveModel);
            }
        }
    }
}

       

