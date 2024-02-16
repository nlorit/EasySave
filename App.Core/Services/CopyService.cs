using App.Core.Models;
using System.Resources;

namespace App.Core.Services
{
    public class CopyService
    {
        private readonly LoggerService loggerService = new();
        private readonly StateManagerService stateManagerService;
        private bool isStopped;
        private bool isPaused;

        public CopyModel CopyModel { get; set; }



        public CopyService(StateManagerService stateManagerService)
        {
            this.CopyModel = new();
            this.stateManagerService = stateManagerService;
        }

        public void ExecuteCopy()
        {
            isPaused = false;
            isStopped = false;
            ProcessCopy();
        }

        public void PauseCopy()
        {
            isPaused = true;
            // Implement any necessary logic for pausing the copying process
        }

        public void StopCopy()
        {
            isStopped = true;
            // Implement any necessary logic for stopping the copying process
        }

        public void ResumeCopy()
        {
            isPaused = false;
            // Implement any necessary logic for resuming the copying process
            ProcessCopy();
        }




        private void ProcessCopy()
        {
            if (isPaused || isStopped)
                return;

            // Check if the source path exists
            if (File.Exists(this.CopyModel.SourcePath))
            {
                // File copying operation
                File.Copy(this.CopyModel.SourcePath, this.CopyModel.TargetPath, true);
            }
            // Check if the source path is a directory
            else if (Directory.Exists(this.CopyModel.SourcePath))
            {
                // Directory copying operation
                CopyDirectory(this.CopyModel.SourcePath, this.CopyModel.TargetPath);
            }
            // If the source path does not exist
            else
            {
                //TODO : throw an exception that the file doesnt Recup l'exception
                // If the source path does not exist
                throw new FileNotFoundException("Source file or directory does not exist.", this.CopyModel.SourcePath);

            }
        }

        private void CopyDirectory(string sourceDirPath, string targetDirPath)
        {
            if (isPaused || isStopped)
                return;

            // Crée le répertoire cible s'il n'existe pas encore
            if (Directory.Exists(targetDirPath))
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
                File.Copy(filePath, targetFilePath, true); // Le paramètre true permet d'écraser le fichier s'il existe déjà dans le répertoire cible
            }

            // Copie les sous-répertoires récursivement
            foreach (string subdirectory in subdirectories)
            {
                string subdirectoryName = Path.GetFileName(subdirectory);
                string targetSubdirectoryPath = Path.Combine(targetDirPath, subdirectoryName);
                CopyDirectory(subdirectory, targetSubdirectoryPath); // Appel récursif pour copier les sous-répertoires
            }
        }

    }
}
