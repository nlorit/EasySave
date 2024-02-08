using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class CopyService
    {

        private readonly LoggerService loggerService = new LoggerService();
        private readonly StateManagerService stateManagerService = new StateManagerService();

        public void RunCopy(CopyModel model, SaveModel saveModel, List<SaveModel> saves, List<StateManagerModel> list)
        {
            Console.WriteLine(saveModel.SaveName + " is running...");

           

                if (File.Exists(model.SourcePath))
                {
                    // File copying operation
                    File.Copy(model.SourcePath, model.TargetPath, true);
                    Console.WriteLine("File copied successfully.");

                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Directory copying operation
                    CopyDirectory(model.SourcePath, model.TargetPath, saveModel, saves, list);
                    Console.WriteLine("Directory copied successfully.");

                }
                else
                {
                    Console.WriteLine("Source does not exist or is neither a file nor a directory.");
                    // Log an entry for non-existing source
                }
            
           

        }
        private void CopyDirectory(string sourceDirPath, string targetDirPath, SaveModel saveModel, List<SaveModel> saves, List<StateManagerModel> list)
        {
            int totalFiles = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length;
            long totalSize = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);

            saveModel.StateManager.SaveName = saveModel.SaveName;
            saveModel.StateManager.TotalFilesSize = totalSize;
            saveModel.StateManager.TotalFilesToCopy = totalFiles;
            saveModel.StateManager.NbFilesLeftToDo = totalFiles;
            saveModel.StateManager.State = "ACTIVE";
            int percent = 0;
            saveModel.StateManager.Progression = (percent / Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length);
           
            try
            {
                stateManagerService.UpdateState(list, saveModel, saves);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
             
            


            // Copy files recursively
            foreach (string filePath in Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories))
            {
                percent += 1;
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
                saveModel.StateManager.NbFilesLeftToDo = saveModel.StateManager.NbFilesLeftToDo - 1;
                saveModel.StateManager.Progression = ((percent * 100) / Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length);
                saveModel.StateManager.TargetFilePath = destFilePath;
                saveModel.StateManager.SourceFilePath = filePath;
                Task.Delay(1000).Wait();
                try
                {
                    stateManagerService.UpdateState(list, saveModel, saves);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }


            }
            saveModel.StateManager.State = "END";
            stateManagerService.UpdateState(list, saveModel, saves);


        }
    }
}

       

