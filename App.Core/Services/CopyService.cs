using App.Core.Models;
using System.IO;
using System.Threading;

namespace App.Core.Services
{
    public class CopyService
    {
        public LoggerService loggerService = new();
        public StateManagerService stateManagerService = new();
        private bool isStopped;
        private bool isPaused;

        public CopyModel CopyModel { get; set; }

        public CopyService()
        {
            this.CopyModel = new();
        }

        public CopyService(StateManagerService stateManagerService)
        {
            this.CopyModel = new();
            this.stateManagerService = stateManagerService;
        }

        public void ExecuteCopy(SaveModel saveModel)
        {
            isPaused = false;
            isStopped = false;
            ProcessCopy(saveModel);
        }

        public void PauseCopy(SaveModel saveModel)
        {
            isPaused = true;
            // Implement any necessary logic for pausing the copying process
        }

        public void StopCopy(SaveModel saveModel)
        {
            isStopped = true;
            // Implement any necessary logic for stopping the copying process
        }

        public void ResumeCopy(SaveModel saveModel)
        {
            isPaused = false;
            // Implement any necessary logic for resuming the copying process
            ProcessCopy(saveModel);
        }

        private void ProcessCopy(SaveModel saveModel)
        {
            if (isPaused || isStopped)
                return;

            foreach (StateManagerModel stateModel in stateManagerService.listStateModel!)
            {
                if (stateModel.SaveName == saveModel.SaveName)
                {
                    stateModel.State = "ACTIVE";
                    stateManagerService.UpdateStateFile();

                    if (System.IO.File.Exists(this.CopyModel.SourcePath))
                    {
                        // File copying operation
                        System.IO.File.Copy(this.CopyModel.SourcePath, this.CopyModel.TargetPath, true);
                        stateModel.SourceFilePath = this.CopyModel.SourcePath;
                        stateModel.TargetFilePath = this.CopyModel.TargetPath;
                        stateManagerService.UpdateStateFile();
                    }
                    else if (Directory.Exists(this.CopyModel.SourcePath))
                    {
                        // Directory copying operation
                        CopyDirectory(this.CopyModel.SourcePath, this.CopyModel.TargetPath, stateModel);
                    }
                    else
                    {
                        throw new FileNotFoundException("Source file or directory does not exist.", this.CopyModel.SourcePath);
                    }

                    stateModel.State = "END";
                    stateManagerService.UpdateStateFile();
                }
            }
        }

        private void CopyDirectory(string sourceDirPath, string targetDirPath, StateManagerModel stateModel)
        {
            if (isPaused || isStopped)
                return;

            // Create the target directory if it does not exist yet
            if (!Directory.Exists(targetDirPath))
            {
                Directory.CreateDirectory(targetDirPath);
            }

            // Get the list of files and subdirectories in the source directory
            string[] files = Directory.GetFiles(sourceDirPath);
            string[] subdirectories = Directory.GetDirectories(sourceDirPath);

            // Update total files to copy
            stateModel.TotalFilesToCopy = Directory.GetFiles(sourceDirPath, "*.*", SearchOption.AllDirectories).Length;
            stateManagerService.UpdateStateFile();

            // Copy files
            foreach (string filePath in files)
            {
                if (isPaused || isStopped)
                    return;

                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(targetDirPath, fileName);
                System.IO.File.Copy(filePath, targetFilePath, true); // The 'true' parameter allows overwriting the file if it already exists in the target directory

                // Update state model
                stateModel.NbFilesLeftToDo--;
                stateModel.Progression = (int)(((double)(stateModel.TotalFilesToCopy - stateModel.NbFilesLeftToDo) / stateModel.TotalFilesToCopy) * 100);
                stateModel.SourceFilePath = filePath;
                stateModel.TargetFilePath = targetFilePath;
                stateManagerService.UpdateStateFile();
            }

            // Recursively copy subdirectories
            foreach (string subdirectory in subdirectories)
            {
                string subdirectoryName = Path.GetFileName(subdirectory);
                string targetSubdirectoryPath = Path.Combine(targetDirPath, subdirectoryName);
                CopyDirectory(subdirectory, targetSubdirectoryPath, stateModel); // Recursive call to copy subdirectories
            }
        }
    }
}
