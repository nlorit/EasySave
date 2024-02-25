using App.Core.Models;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace App.Core.Services
{
    public class CopyService : IDisposable
    {
        public LoggerService loggerService = new();
        public StateManagerService stateManagerService = new();
        public bool isEncrypted;

        private readonly object locker = new();
        private ManualResetEvent? manualReset = new(false);
        public CopyModel CopyModel { get; set; }

        public event EventHandler<BackupEventArgs>? BackupStarted;
        public event EventHandler<BackupEventArgs>? BackupCompleted;



        public CopyService()
        {
            this.CopyModel = new();
        }


        public void ExecuteCopy(SaveModel saveModel, StreamWriter logWriter, StreamWriter stateWriter)
        {
            
            Thread thread = new Thread(() =>
            {
                OnBackupStarted(saveModel);
                BackupDirectory(saveModel.InPath, saveModel.OutPath, logWriter, stateWriter);
                OnBackupCompleted(saveModel);
            });
            thread.Start();
            
        }

        public void PauseCopy()
        {
            manualReset?.Reset();
        }

        public void ResumeCopy()
        {
            manualReset?.Set();
        }

        public void StopCopy()
        {
        }


        private void EncryptFile(string sourcePath)
        {
            string additionalArgument = "abcabcab";

            ProcessStartInfo startInfo = new ProcessStartInfo();
            string FileName = @"Library\Cryptosoft.exe"; // Assuming Cryptosoft.exe is located in the Library directory relative to the current working directory
            startInfo.FileName = FileName;
            startInfo.Arguments = $" {sourcePath} {additionalArgument} .png";

            Process.Start(startInfo);
        }

        void BackupDirectory(string sourceDirPath, string targetDirPath, StreamWriter logWriter, StreamWriter stateWriter)
        {
            while (!manualReset!.WaitOne(0))
            { // Wait until manualReset is set

                int TotalFailed = 0;
                // Stage 1: Create the destination folder
                if (!Directory.Exists(targetDirPath))
                {
                    try
                    {
                        Directory.CreateDirectory(targetDirPath);
                        logWriter.WriteLine($"Create folder: {targetDirPath}");
                    }
                    catch (Exception ex)
                    {
                        // Cannot create folder
                        stateWriter.WriteLine($"Failed to create folder: {targetDirPath}\r\nAccess Denied\r\n");
                        return;
                    }
                }

                // Stage 2: Get all files from the source folder
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(sourceDirPath);
                }
                catch (UnauthorizedAccessException)
                {
                    // Access denied, cannot read folder or files
                    stateWriter.WriteLine($"{sourceDirPath}\r\nAccess Denied\r\n");
                }
                catch (Exception e)
                {
                    // Other unknown read errors
                    stateWriter.WriteLine($"{sourceDirPath}\r\n{e.Message}\r\n");
                }

                // Stage 3: Copy all files from source to destination folder
                if (files != null && files.Length > 0)
                {
                    foreach (string file in files)
                    {

                        try
                        {
                            string name = Path.GetFileName(file);
                            string dest = Path.Combine(targetDirPath, name);

                            File.Copy(file, dest, true);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // Access denied, cannot write file
                            stateWriter.WriteLine($"{file}\r\nAccess Denied\r\n");
                            TotalFailed++;
                        }
                        catch (Exception e)
                        {
                            // Other unknown error
                            TotalFailed++;
                            stateWriter.WriteLine($"{file}\r\n{e.Message}\r\n");
                        }
                    }
                }

                // Stage 4: Get all sub-folders
                string[] folders = null;
                try
                {
                    folders = Directory.GetDirectories(sourceDirPath);
                }
                catch (UnauthorizedAccessException)
                {
                    // Access denied, cannot read folders
                    stateWriter.WriteLine($"{sourceDirPath}\r\nAccess denied\r\n");
                }
                catch (Exception e)
                {
                    // Other unknown read errors
                    stateWriter.WriteLine($"{sourceDirPath}\r\nAccess {e.Message}\r\n");
                }

                // Stage 5: Backup files and "sub-sub-folders" in the sub-folder
                if (folders != null && folders.Length > 0)
                {
                    foreach (string folder in folders)
                    {

                        try
                        {
                            string name = Path.GetFileName(folder);
                            string dest = Path.Combine(targetDirPath, name);

                            // recursive call with updated source and target paths
                            BackupDirectory(folder, dest, logWriter, stateWriter);
                        }
                        catch (UnauthorizedAccessException)
                        {
                            // Access denied, cannot read folders
                            stateWriter.WriteLine($"{folder}\r\nAccess denied\r\n");
                        }
                        catch (Exception e)
                        {
                            // Other unknown read errors
                            stateWriter.WriteLine($"{targetDirPath}\r\nAccess {e.Message}\r\n");
                        }
                    }
                }

                // Perform encryption if required
                if (this.isEncrypted)
                {
                    EncryptFile(this.CopyModel.TargetPath);
                }
                
                 
            }
        }

        public void Dispose()
        {
            manualReset!.Dispose();
        }

        protected void OnBackupStarted(SaveModel save)
        {
            BackupStarted?.Invoke(this, new BackupEventArgs
            {
                SourcePath = save.InPath,
                TargetPath = save.OutPath,
                StartTime = DateTime.Now
            });
        }

        protected void OnBackupCompleted(SaveModel save)
        {
            BackupCompleted?.Invoke(this, new BackupEventArgs
            {
                SourcePath = save.InPath,
                TargetPath = save.OutPath,
                EndTime = DateTime.Now
            });
        }
    }


         
}

public class BackupEventArgs : EventArgs
{
    public string SourcePath { get; set; }
    public string TargetPath { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}


