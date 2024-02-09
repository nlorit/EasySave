using App.Core.Models;
using System.Resources;

namespace App.Core.Services
{
    public class CopyService
    {


        private readonly LoggerService loggerService = new();
        private readonly StateManagerService stateManagerService = new();
   

        public void RunCopy(CopyModel model, SaveModel saveModel, List<SaveModel> saves, List<StateManagerModel> list, ResourceManager Resources)
        {
            string? print;
            print = Resources.GetString("IsRunning");
            Console.WriteLine(saveModel.SaveName + print);


            if (File.Exists(model.SourcePath))
                {
                    // File copying operation
                    File.Copy(model.SourcePath, model.TargetPath, true);
                    print = Resources.GetString("FileCopied");
                    Console.WriteLine(print);

                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Directory copying operation
                    CopyDirectory(model.SourcePath, model.TargetPath, saveModel, saves, list, Resources);
                    print = Resources.GetString("DirectoryCopied");
                    Console.WriteLine(print);

                }
                else
                {
                print = Resources.GetString("SourceError");
                Console.WriteLine(print);
                    // Log an entry for non-existing source
                }
            
           

        }
        private void CopyDirectory(string sourceDirPath, string targetDirPath, SaveModel saveModel, List<SaveModel> saves, List<StateManagerModel> list, ResourceManager Resources)
        {
            if (string.IsNullOrEmpty(sourceDirPath))
            {
                throw new ArgumentException($"'{nameof(sourceDirPath)}' cannot be null or empty.", nameof(sourceDirPath));
            }

            if (string.IsNullOrEmpty(targetDirPath))
            {
                throw new ArgumentException($"'{nameof(targetDirPath)}' cannot be null or empty.", nameof(targetDirPath));
            }

            ArgumentNullException.ThrowIfNull(saveModel);

            ArgumentNullException.ThrowIfNull(saves);

            ArgumentNullException.ThrowIfNull(list);

            ArgumentNullException.ThrowIfNull(Resources);

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
                System.IO.Directory.CreateDirectory(targetDirPath);


                LoggerModel logModel = new()
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
                Task.Delay(50).Wait();
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

       

