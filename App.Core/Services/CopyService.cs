using App.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class CopyService
    {
        private readonly LoggerService loggerService = new LoggerService();
        private readonly StateManagerService stateManagerService = new StateManagerService();
        private StateManagerModel stateModel = new StateManagerModel();

        public async Task RunCopyAsync(CopyModel model, SaveModel saveModel, List<SaveModel> saves)
        {
            Console.WriteLine(saveModel.SaveName + " is running...");

            try
            {
                await UpdateStateFile(this.stateModel, saveModel, saves); // Update state before operation

                if (File.Exists(model.SourcePath))
                {
                    // File copying operation
                    await CopyFileAsync(model.SourcePath, model.TargetPath, saveModel, saves);
                    Console.WriteLine("File copied successfully.");
                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Directory copying operation
                    await CopyDirectoryAsync(model.SourcePath, model.TargetPath, saveModel, saves);
                    Console.WriteLine("Directory copied successfully.");
                }
                else
                {
                    Console.WriteLine("Source does not exist or is neither a file nor a directory.");
                    // Log an entry for non-existing source
                    await LogErrorAsync(model.SourcePath, model.TargetPath, saveModel);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            stateModel.State = "END";
            await UpdateStateFile(stateModel, saveModel, saves); // Update state after operation
        }

        private async Task CopyFileAsync(string sourceFilePath, string targetFilePath, SaveModel saveModel, List<SaveModel> saves)
        {
            await Task.Run(async () =>
            {
                // Perform file copy operation
                this.stateModel.State = "START";
                UpdateStateFile(this.stateModel, saveModel, saves);

                // Simulated file copy process
                // Replace this with your actual file copy code
                for (int i = 0; i < 100; i++)
                {
                    // Update progression
                    this.stateModel.Progression = i / 100.0f;
                    UpdateStateFile(this.stateModel, saveModel, saves);

                    // Simulated delay for copying
                    await Task.Delay(200);
                }

                this.stateModel.State = "END";
                UpdateStateFile(this.stateModel, saveModel, saves);

                // Log file copy operation
                LogFileTransfer(sourceFilePath, targetFilePath, saveModel);
            });
        }

        private async Task CopyDirectoryAsync(string sourceDirPath, string targetDirPath, SaveModel saveModel, List<SaveModel> saves)
        {
            await Task.Run(async () =>
            {
                // Simulated directory copy process
                // Replace this with your actual directory copy code

                // Get total files and size to copy
                int totalFiles = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length;
                long totalSize = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);

                // Set total files and size to state model
                this.stateModel.TotalFilesToCopy = totalFiles;
                this.stateModel.TotalFilesSize = (int)(totalSize / 1024); // Convert to KB

                // Initialize progression
                this.stateModel.Progression = 0;
                UpdateStateFile(this.stateModel, saveModel, saves);

                // Copy files recursively
                foreach (string filePath in Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories))
                {
                    string fileName = Path.GetFileName(filePath);
                    string destFilePath = Path.Combine(targetDirPath, fileName);

                    // Simulated delay for copying each file
                    await Task.Delay(100);

                    // Update progression
                    this.stateModel.Progression += 1.0f / totalFiles;
                    this.stateModel.TargetFilePath = destFilePath;
                    this.stateModel.SourceFilePath = Path.Combine(sourceDirPath, fileName);
                    this.stateModel.State = "ACTIVE";
                    UpdateStateFile(this.stateModel, saveModel, saves);

                    // Simulated file copy
                    File.Copy(filePath, destFilePath, true);
                }

                // Set progression to 100% after copying all files
                this.stateModel.Progression = 1.0f;
                UpdateStateFile(this.stateModel, saveModel, saves);
            });
        }

        private async Task LogErrorAsync(string sourcePath, string targetPath, SaveModel saveModel)
        {
            await Task.Run(() =>
            {
                LoggerModel loggerModel = new LoggerModel();
                loggerModel.FileSource = sourcePath;
                loggerModel.FileTarget = targetPath;
                loggerModel.FileSize = "";
                loggerModel.FileTransferTime = "-1";
                loggerService.WriteLog(loggerModel, saveModel);
            });
        }

        private void LogFileTransfer(string sourcePath, string targetPath, SaveModel saveModel)
        {
            LoggerModel loggerModel = new LoggerModel();
            loggerModel.FileSource = sourcePath;
            loggerModel.FileTarget = targetPath;
            loggerModel.FileSize = new FileInfo(sourcePath).Length / 1024.0 + " kb";
            loggerModel.FileTransferTime = (DateTime.Now - loggerModel.Time).ToString();
            loggerService.WriteLog(loggerModel, saveModel);
        }

        private async Task UpdateStateFile(StateManagerModel stateModel, SaveModel saveModel, List<SaveModel> saves)
        {
            try
            {
                await stateManagerService.UpdateStateAsync(stateModel, saveModel, saves);
            }
            catch (Exception ex)
            {
            }
        }
    }
}
