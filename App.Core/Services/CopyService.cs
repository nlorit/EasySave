using App.Core.Models;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace App.Core.Services
{
    public class CopyService 
    {
        public LoggerService loggerService = new();
        public StateManagerService stateManagerService = new();
        public bool isEncrypted;

        private readonly object locker = new();
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
        }

        public void ResumeCopy()
        {

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
            while (true)
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



        //public void CompleteSave(string inputpathsave, string inputDestToSave, bool copyDir, bool verif) //Function for full folder backup
        //{
        //    StateManagerModel stateModel = new StateManagerModel();
        //    stateModel.State = "ACTIVE";
        //    Stopwatch stopwatch = new Stopwatch();
        //    Stopwatch cryptwatch = new Stopwatch();
        //    stopwatch.Start(); //Starting the timed for the log file

        //    DirectoryInfo dir = new DirectoryInfo(inputpathsave);  // Get the subdirectories for the specified directory.

        //    if (!dir.Exists) //Check if the file is present
        //    {
        //        throw new DirectoryNotFoundException("ERROR 404 : Directory Not Found ! " + inputpathsave);
        //    }

        //    DirectoryInfo[] dirs = dir.GetDirectories();
        //    Directory.CreateDirectory(inputDestToSave); // If the destination directory doesn't exist, create it.  

        //    FileInfo[] files = dir.GetFiles(); // Get the files in the directory and copy them to the new location.

        //    if (!verif) //  Check for the status file if it needs to reset the variables
        //    {
        //        TotalSize = 0;
        //        nbfilesmax = 0;
        //        size = 0;
        //        nbfiles = 0;
        //        progs = 0;

        //        foreach (FileInfo file in files) // Loop to allow calculation of files and folder size
        //        {
        //            TotalSize += file.Length;
        //            nbfilesmax++;
        //        }
        //        foreach (DirectoryInfo subdir in dirs) // Loop to allow calculation of subfiles and subfolder size
        //        {
        //            FileInfo[] Maxfiles = subdir.GetFiles();
        //            foreach (FileInfo file in Maxfiles)
        //            {
        //                TotalSize += file.Length;
        //                nbfilesmax++;
        //            }
        //        }

        //    }

        //    Loop that allows to copy the files to make the backup
        //    foreach (FileInfo file in files)
        //    {
        //        if (this.button_pause == true)
        //        {
        //            MessageBox.Show("test");
        //        }
        //        if (this.button_stop == true)
        //        {
        //            Thread.ResetAbort();
        //        }

        //        string tempPath = Path.Combine(inputDestToSave, file.Name);

        //        if (size > 0)
        //        {
        //            progs = ((float)size / TotalSize) * 100;
        //        }

        //        Systems which allows to insert the values ​​of each file in the report file.
        //        DataState.SourceFile = Path.Combine(inputpathsave, file.Name);
        //        DataState.TargetFile = tempPath;
        //        DataState.TotalSize = nbfilesmax;
        //        DataState.TotalFile = TotalSize;
        //        DataState.TotalSizeRest = TotalSize - size;
        //        DataState.FileRest = nbfilesmax - nbfiles;
        //        DataState.Progress = progs;
        //        UpdateStatefile(); //Call of the function to start the state file system

        //        if (PriorityExt(Path.GetExtension(file.Name)))
        //        {
        //            if (CryptExt(Path.GetExtension(file.Name)))
        //            {
        //                cryptwatch.Start();
        //                Encrypt(DataState.SourceFile, tempPath);
        //                cryptwatch.Stop();
        //            }
        //            else
        //            {
        //                file.CopyTo(tempPath, true); //Function that allows you to copy the file to its new folder.
        //            }

        //        }
        //        else
        //        {
        //            if (CryptExt(Path.GetExtension(file.Name)))
        //            {
        //                cryptwatch.Start();
        //                Encrypt(DataState.SourceFile, tempPath);
        //                cryptwatch.Stop();
        //            }
        //            else
        //            {
        //                file.CopyTo(tempPath, true); //Function that allows you to copy the file to its new folder.
        //            }
        //        }

        //        nbfiles++;
        //        size += file.Length;

        //    }

        //    If copying subdirectories, copy them and their contents to new location.
        //    if (copyDir)
        //    {
        //        foreach (DirectoryInfo subdir in dirs)
        //        {
        //            string tempPath = Path.Combine(inputDestToSave, subdir.Name);
        //            CompleteSave(subdir.FullName, tempPath, copyDir, true);
        //        }
        //    }
        //    System which allows the values ​​to be reset to 0 at the end of the backup
        //    DataState.TotalSize = TotalSize;
        //    DataState.SourceFile = null;
        //    DataState.TargetFile = null;
        //    DataState.TotalFile = 0;
        //    DataState.TotalSize = 0;
        //    DataState.TotalSizeRest = 0;
        //    DataState.FileRest = 0;
        //    DataState.Progress = 0;
        //    DataState.SaveState = false;

        //    UpdateStatefile(); //Call of the function to start the state file system

        //    stopwatch.Stop(); //Stop the stopwatch
        //    cryptwatch.Stop();
        //    this.TimeTransfert = stopwatch.Elapsed; // Transfer of the chrono time to the variable
        //    this.CryptTransfert = cryptwatch.Elapsed;
        //}

        //public void DifferentialSave(string pathA, string pathB, string pathC) // Function that allows you to make a differential backup
        //{
        //    DataState = new DataState(NameStateFile); //Instattation of the method
        //    Stopwatch stopwatch = new Stopwatch(); // Instattation of the method
        //    Stopwatch cryptwatch = new Stopwatch();
        //    stopwatch.Start(); //Starting the stopwatch

        //    DataState.SaveState = true;
        //    TotalSize = 0;
        //    nbfilesmax = 0;

        //    System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(pathA);
        //    System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(pathB);

        //    Take a snapshot of the file system.
        //   IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        //    IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

        //    A custom file comparer defined below
        //    FileCompare myFileCompare = new FileCompare();

        //    var queryList1Only = (from file in list1 select file).Except(list2, myFileCompare);
        //    size = 0;
        //    nbfiles = 0;
        //    progs = 0;

        //    foreach (var v in queryList1Only)
        //    {
        //        TotalSize += v.Length;
        //        nbfilesmax++;

        //    }

        //    Loop that allows the backup of different files
        //    foreach (var v in queryList1Only)
        //    {
        //        string tempPath = Path.Combine(pathC, v.Name);
        //        Systems which allows to insert the values ​​of each file in the report file.
        //        DataState.SourceFile = Path.Combine(pathA, v.Name);
        //        DataState.TargetFile = tempPath;
        //        DataState.TotalSize = nbfilesmax;
        //        DataState.TotalFile = TotalSize;
        //        DataState.TotalSizeRest = TotalSize - size;
        //        DataState.FileRest = nbfilesmax - nbfiles;
        //        DataState.Progress = progs;

        //        UpdateStatefile();//Call of the function to start the state file system

        //        if (PriorityExt(Path.GetExtension(v.Name)))
        //        {
        //            if (CryptExt(Path.GetExtension(v.Name)))
        //            {
        //                cryptwatch.Start();
        //                Encrypt(DataState.SourceFile, tempPath);
        //                cryptwatch.Stop();
        //            }
        //            else
        //            {
        //                v.CopyTo(tempPath, true); //Function that allows you to copy the file to its new folder.
        //            }
        //        }
        //        else
        //        {
        //            if (CryptExt(Path.GetExtension(v.Name)))
        //            {
        //                cryptwatch.Start();
        //                Encrypt(DataState.SourceFile, tempPath);
        //                cryptwatch.Stop();
        //            }
        //            else
        //            {
        //                v.CopyTo(tempPath, true); //Function that allows you to copy the file to its new folder.
        //            }
        //        }

        //        size += v.Length;
        //        nbfiles++;
        //    }

        //    System which allows the values ​​to be reset to 0 at the end of the backup
        //    DataState.SourceFile = null;
        //    DataState.TargetFile = null;
        //    DataState.TotalFile = 0;
        //    DataState.TotalSize = 0;
        //    DataState.TotalSizeRest = 0;
        //    DataState.FileRest = 0;
        //    DataState.Progress = 0;
        //    DataState.SaveState = false;
        //    UpdateStatefile();//Call of the function to start the state file system

        //    stopwatch.Stop(); //Stop the stopwatch
        //    this.TimeTransfert = stopwatch.Elapsed; // Transfer of the chrono time to the variable
        //    this.CryptTransfert = cryptwatch.Elapsed;
        //}






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


