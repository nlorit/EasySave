using App.Core.Models;
using System.Diagnostics;
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
        private bool isEncrypted =true;
        private long totalFile = 0;
        private long totalSize = 0;
        private float progress =0;

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
            totalFile = 0;
            totalSize = 0;
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

        private void EncryptFile(string sourcePath)
        {
            string additionalArgument = "abcabcab";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            string FileName = @"Library\Cryptosoft.exe"; // Assuming Cryptosoft.exe is located in the Library directory relative to the current working directory
            startInfo.Arguments = $"{FileName} {sourcePath} {additionalArgument} .*";

            Process.Start(startInfo);
        }


        private void ProcessCopy(SaveModel saveModel)
        {
            if (isPaused || isStopped)
                return;

            foreach (var item in Directory.GetFiles(this.CopyModel.SourcePath, "*", SearchOption.AllDirectories))
            {
                totalFile += 1;
                totalSize += new FileInfo(item).Length;
            }

            foreach (StateManagerModel stateModel in stateManagerService.listStateModel!)
            {
                if (stateModel.SaveName == saveModel.SaveName)
                {

                    stateModel.TotalFilesToCopy += totalFile;
                    stateModel.NbFilesLeftToDo = totalFile;
                    stateModel.TotalFilesSize = totalSize;
                    stateModel.Progression = 100 - ((stateModel.NbFilesLeftToDo / totalFile) * 100);
                    progress = stateModel.Progression;
                    stateModel.State = "ACTIVE";
                    stateManagerService.UpdateStateFile();

                    if (File.Exists(this.CopyModel.SourcePath))
                    {
                        // File copying operation

                        stateModel.SourceFilePath = this.CopyModel.SourcePath;
                        stateModel.TargetFilePath = this.CopyModel.TargetPath;
                        stateManagerService.UpdateStateFile();
                            


                        File.Copy(this.CopyModel.SourcePath, this.CopyModel.TargetPath, true);
                        stateModel.Progression = 100 - ((stateModel.NbFilesLeftToDo / totalFile) * 100);
                        progress = stateModel.Progression;
                        if (isEncrypted) EncryptFile(this.CopyModel.TargetPath);
                        stateModel.NbFilesLeftToDo = stateModel.NbFilesLeftToDo > 0 ? stateModel.NbFilesLeftToDo - 1 : 0;

                        stateManagerService.UpdateStateFile();


                        loggerService!.loggerModel!.Name = saveModel.SaveName;
                        loggerService.loggerModel.FileSource = this.CopyModel.SourcePath;
                        loggerService.loggerModel.FileTarget = this.CopyModel.TargetPath;
                        loggerService.loggerModel.FileSize = new FileInfo(this.CopyModel.SourcePath).Length.ToString();
                        loggerService.AddEntryLog();

                    }
                    else if (Directory.Exists(this.CopyModel.SourcePath))
                    {
                        // Directory copying operation
                        CopyDirectory(this.CopyModel.SourcePath, this.CopyModel.TargetPath, stateModel);

                    }
                    else
                    {
                        // Log the error and handle it gracefully
                        loggerService!.loggerModel!.Name = saveModel.SaveName;
                        loggerService.AddEntryLog();
                    }

                    stateModel.State = "END";
                    stateManagerService.UpdateStateFile();
                }
            }
            



            
        }


        private void CopyDirectory(string sourceDirPath, string targetDirPath, StateManagerModel stateModel)
        {


            // Vérifie si le répertoire source existe
            if (!Directory.Exists(sourceDirPath))
            {
                Console.WriteLine("Le répertoire source n'existe pas.");
                return;
            }

            // Crée le répertoire cible s'il n'existe pas encore
            if (!Directory.Exists(targetDirPath))
            {
                Directory.CreateDirectory(targetDirPath);
            }

            // Obtient la liste des fichiers et des sous-répertoires dans le répertoire source
            string[] files = Directory.GetFiles(sourceDirPath);
            string[] subdirectories = Directory.GetDirectories(sourceDirPath);

            
            // Copie les fichiers
            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                string targetFilePath = Path.Combine(targetDirPath, fileName);
                stateModel.SourceFilePath = filePath;
                stateModel.TargetFilePath = targetFilePath;

                stateManagerService.UpdateStateFile();

                DateTime start = DateTime.Now;
                File.Copy(filePath, targetFilePath, true); // Le paramètre true permet d'écraser le fichier s'il existe déjà dans le répertoire cible
                if (isEncrypted) EncryptFile(this.CopyModel.TargetPath);
                stateModel.NbFilesLeftToDo = stateModel.NbFilesLeftToDo > 0 ? stateModel.NbFilesLeftToDo - 1 : 0;
                start = DateTime.Now;

                if (isEncrypted) EncryptFile(targetFilePath);
                loggerService!.loggerModel!.FileEncryptionTime = (DateTime.Now - start).ToString();

                stateManagerService.UpdateStateFile();
                loggerService!.loggerModel!.Name = stateModel.SaveName;
                loggerService!.loggerModel!.FileSource = filePath;
                loggerService!.loggerModel.FileTarget = targetFilePath;
                loggerService!.loggerModel.FileSize = new FileInfo(filePath).Length.ToString();
                loggerService!.loggerModel.FileTransferTime = (DateTime.Now - start).ToString();
                loggerService.AddEntryLog();
            }

            // Copie les sous-répertoires récursivement
            foreach (string subdirectory in subdirectories)
            {
                string subdirectoryName = Path.GetFileName(subdirectory);
                string targetSubdirectoryPath = Path.Combine(targetDirPath, subdirectoryName);
                CopyDirectory(subdirectory, targetSubdirectoryPath, stateModel); // Appel récursif pour copier les sous-répertoires
            }
        }
    }
}
