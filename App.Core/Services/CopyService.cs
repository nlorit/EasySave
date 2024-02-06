using App.Core.Models;
using System;
using System.IO;

namespace App.Core.Services
{
    public class CopyService
    {
        public void RunCopy(CopyModel model)
        {
            try
            {
                FileService fileService = new FileService();

                fileService.GetFileInfo(new FileModel { FilePath = model.SourcePath });

                if (File.Exists(model.SourcePath))
                {
                    
                    
                    Console.WriteLine("File copied successfully.");
                    
                }
                else if (Directory.Exists(model.SourcePath))
                {
                    // Source is a directory
                    CopyDirectory(model.SourcePath, model.TargetPath);
                    Console.WriteLine("Directory copied successfully.");
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
