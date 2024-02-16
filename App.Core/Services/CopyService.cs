using App.Core.Models;
using System.Resources;

namespace App.Core.Services
{
    public class CopyService
    {
        private readonly LoggerService loggerService = new();
        private readonly StateManagerService stateManagerService = new();
   

        /// <summary>
        /// Method to execute the copy service
        /// </summary>
        /// <param name="copyModel"></param>
        /// <param name="saveModel"></param>
        /// <param name="listSavesModel"></param>
        /// <param name="listStateManager"></param>
        public void RunCopy(CopyModel copyModel, SaveModel saveModel, List<SaveModel> listSavesModel, List<StateManagerModel> listStateManager)
        {
            string? Output;
            Output = DisplayService.GetResource("IsRunning");
            Console.WriteLine(saveModel.SaveName + Output);

            // Check if the source path exists
            if (File.Exists(copyModel.SourcePath))
            {
                // File copying operation
                File.Copy(copyModel.SourcePath, copyModel.TargetPath, true);
                Output = DisplayService.GetResource("FileCopied");
                Console.WriteLine(Output);

            }
            // Check if the source path is a directory
            else if (Directory.Exists(copyModel.SourcePath))
            {
                // Directory copying operation
                CopyDirectory(copyModel.SourcePath, copyModel.TargetPath, saveModel, listSavesModel, listStateManager);
                Output = DisplayService.GetResource("DirectoryCopied");
                Console.WriteLine(Output);

            }
            // If the source path does not exist
            else
            {
                Output = DisplayService.GetResource("SourceError");
                Console.WriteLine(Output);
            }
            
           

        }

        /// <summary>
        /// Method to copy a directory
        /// </summary>
        /// <param name="sourceDirPath"></param>
        /// <param name="targetDirPath"></param>
        /// <param name="saveModel"></param>
        /// <param name="listSavesModel"></param>
        /// <param name="listStateManager"></param>
        private void CopyDirectory(string sourceDirPath, string targetDirPath, SaveModel saveModel, List<SaveModel> listSavesModel, List<StateManagerModel> listStateManager)
        {
            // Check for nulls
            ArgumentNullException.ThrowIfNull(sourceDirPath);
            ArgumentNullException.ThrowIfNull(targetDirPath);
            ArgumentNullException.ThrowIfNull(saveModel);
            ArgumentNullException.ThrowIfNull(listSavesModel);
            ArgumentNullException.ThrowIfNull(listStateManager);

            // Get the total number of files and the total size of the files
            int TotalFilesCount = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length;
            long TotalFilesSize = Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Sum(f => new FileInfo(f).Length);

            // Update the state manager
            saveModel.StateManager.SaveName = saveModel.SaveName;
            saveModel.StateManager.TotalFilesSize = TotalFilesSize;
            saveModel.StateManager.TotalFilesToCopy = TotalFilesCount;
            saveModel.StateManager.NbFilesLeftToDo = TotalFilesCount;
            saveModel.StateManager.State = "ACTIVE";
            int Percentage = 0;

            //Calculate the progression
            saveModel.StateManager.Progression = (Percentage / Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length);
            try
            {
                stateManagerService.UpdateState(listStateManager, saveModel, listSavesModel);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
             

            // Copy files recursively
            foreach (string filePath in Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories))
            {
                Percentage += 1;
                // Get the file name
                string FileName = Path.GetFileName(filePath);
                // Get the destination file path
                string DestinationFilePath = Path.Combine(targetDirPath, FileName);
                // Create the directory if it does not exist
                System.IO.Directory.CreateDirectory(targetDirPath);

                // Create a log model
                LoggerModel logModel = new()
                {
                    Name = saveModel.SaveName,
                    FileSource = filePath,
                    FileTarget = DestinationFilePath,
                    FileSize = new FileInfo(filePath).Length.ToString()
                };
                // File copying operation
                File.Copy(filePath, DestinationFilePath, true);

                // Write the log
                logModel.FileTransferTime = (DateTime.Now - logModel.Time).ToString();
                loggerService.WriteLog(logModel, saveModel);
                saveModel.StateManager.NbFilesLeftToDo = saveModel.StateManager.NbFilesLeftToDo - 1;
                saveModel.StateManager.Progression = ((Percentage * 100) / Directory.GetFiles(sourceDirPath, "*", SearchOption.AllDirectories).Length);
                saveModel.StateManager.TargetFilePath = DestinationFilePath;
                saveModel.StateManager.SourceFilePath = filePath;

                //Delay for avoid the stateManagerService to update the state too fast

                try
                {
                    stateManagerService.UpdateState(listStateManager, saveModel, listSavesModel);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error: {e.Message}");
                }



            }
            // Update the state manager
            saveModel.StateManager.State = "END";
            stateManagerService.UpdateState(listStateManager, saveModel, listSavesModel);


        }
    }
}

       

