using App.Core.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace App.Core.Services
{
    public class SaveService
    {

        public ObservableCollection<(Thread thread, ManualResetEvent pauseEvent, ManualResetEvent resumeEvent, ManualResetEvent stopEvent, SaveModel savemodel)> listThreads { get; private set; } = new ObservableCollection<(Thread, ManualResetEvent, ManualResetEvent, ManualResetEvent, SaveModel)>();
        public (Thread thread, ManualResetEvent pauseEvent, ManualResetEvent resumeEvent, ManualResetEvent stopEvent, SaveModel savemodel) currentThread { get; private set; }

        public string? priority = "txt,docx,xlsx,pptx,doc,pdf,zip,rar,7z,mp3,mp4,avi,flv,wmv,mpg,mov,exe,msi,apk,iso,img";

        private long fileDo = 0;
        private long fileTotal = 0;

        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly StateManagerService stateManagerService;


        public SaveService()
        {
            stateManagerService = new StateManagerService();
            ObservableCollection<SaveModel> ListSaveModel = new();
            (ListSaveModel, _) = LoadSave();
        }

        public bool IsProcessRunning(string processName)
        {
            return Process.GetProcesses().Any(process => process.ProcessName.Equals("mspaint.exe", StringComparison.OrdinalIgnoreCase));
        }

        public void LaunchSave(SaveModel saveModel)
        {
            //TODO : Detection logicel Métier

            foreach (var item in listThreads)
            {
                if (item.savemodel.SaveName == saveModel.SaveName)
                {
                    currentThread = item;
                    break;
                }
            }

            if (saveModel.Type == "Complete")
            {
                for (int i = 0; i < listThreads.Count; i++)
                {
                    var element = listThreads[i];

                    if (element.savemodel.SaveName == saveModel.SaveName)
                    {
                        saveModel.percentage = 0;
                        listThreads[i].stopEvent.Reset();
                        listThreads[i].resumeEvent.Set(); // Signal the resume event to allow the save operation to proceed
                        Thread thread = new Thread(() =>
                        {
                            CompleteSave(saveModel.InPath, saveModel.OutPath, true, saveModel, i);
                        });
                        element.thread = thread; // Assign the newly created thread

                        // Replace the element in listThreads with the updated tuple
                        listThreads[i] = (element.thread, listThreads[i].pauseEvent, listThreads[i].resumeEvent, listThreads[i].stopEvent, listThreads[i].savemodel);

                        thread.Start();
                        listThreads[i].stopEvent.Reset();
                        listThreads[i].resumeEvent.Set();
                        saveModel.percentage = 100;
                        break; // Exit the loop since we found the matching element
                    }
                }
            }

            if (saveModel.Type == "Incremental")
            {
                for (int i = 0; i < listThreads.Count; i++)
                {
                    var element = listThreads[i];

                    if (element.savemodel.SaveName == saveModel.SaveName)
                    {
                        saveModel.percentage = 0;
                        listThreads[i].stopEvent.Reset();
                        listThreads[i].resumeEvent.Set(); // Signal the resume event to allow the save operation to proceed
                        Thread thread = new Thread(() =>
                        {
                            IncrementalSave(saveModel.InPath, saveModel.OutPath, true, saveModel, i);
                        });
                        element.thread = thread; // Assign the newly created thread

                        // Replace the element in listThreads with the updated tuple
                        listThreads[i] = (element.thread, listThreads[i].pauseEvent, listThreads[i].resumeEvent, listThreads[i].stopEvent, listThreads[i].savemodel);

                        thread.Start();
                        listThreads[i].stopEvent.Reset();
                        listThreads[i].resumeEvent.Set();
                        saveModel.percentage = 100;
                        break; // Exit the loop since we found the matching element
                    }
                }
            }
        }

        public void PauseSave(SaveModel saveModel)
        {
            foreach (var item in listThreads)
            {
                if (item.savemodel.SaveName == saveModel.SaveName)
                {
                    currentThread.pauseEvent.Set(); // Reset the pause event to block threads
                    currentThread.resumeEvent.Reset();
                    break;
                }
            }
        }
        public void StopSave(SaveModel saveModel)
        {
            foreach (var item in listThreads)
            {
                if (item.savemodel.SaveName == saveModel.SaveName)
                {
                    currentThread.stopEvent.Set(); // Signal the stop event to stop the save operation
                    break;
                }
            }

        }
        public void ResumeSave(SaveModel saveModel)
        {
            foreach (var item in listThreads)
            {
                if (item.savemodel.SaveName == saveModel.SaveName)
                {
                    currentThread.pauseEvent.Reset(); // Reset the pause event to block threads
                    currentThread.resumeEvent.Set();
                    break;
                }
            } // Reset the resume event to block it until it's signaled again
        }

        private bool CompleteSave(string InPath, string OutPath, bool verif, SaveModel saveModel, int index)
        {

            // Wait for pauseEvent to be signaled before proceeding
            listThreads[index].Item2.WaitOne(0);

            while (IsProcessRunning("mspaint.exe") && !listThreads[index].Item3.WaitOne(0))
            {
                // Software is running, wait for it to close
                Thread.Sleep(1000);
                // Display message box to inform the user that the software is still running
            }

            if (listThreads[index].Item4.WaitOne(0))
            {
                return false;
            }


            // Implement the logic with the event resume, pause, and stop

            // Wait for resumeEvent to be signaled before continuing
            listThreads[index].Item3.WaitOne();

            fileTotal += Directory.GetFiles(InPath).Length;

            // Recursively count files in subdirectories

            fileTotal += CountFiles(InPath);

            StateManagerModel stateModel = new()
            {
                SaveName = saveModel.SaveName,
                State = "END"
            };

            foreach (StateManagerModel item in stateManagerService.listStateModel!)
            {
                if (item.SaveName == stateModel.SaveName)
                {
                    stateModel = item;
                }
            }

            long TotalSize = 0;
            long nbfilesmax = 0;
            long size = 0;
            long nbfiles = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); // Starting the timed for the log file

            DirectoryInfo dir = new DirectoryInfo(InPath);  // Get the subdirectories for the specified directory.

            if (!dir.Exists) // Check if the file is present
            {
                throw new DirectoryNotFoundException("ERROR 404 : Directory Not Found ! " + InPath);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(OutPath); // If the destination directory doesn't exist, create it.  

            FileInfo[] files = dir.GetFiles(); // Get the files in the directory and copy them to the new location.

            List<string> priorities = priority?.Split(',')?.ToList() ?? new List<string>();

            // Prioritize files based on the extension's presence in the priorities list
            var sourcefiles = files.OrderBy(x => priorities.Contains(Path.GetExtension(x.FullName).ToLower())).Reverse().ToList();

            if (verif) // Check for the status file if it needs to reset the variables
            {
                TotalSize = 0;
                nbfilesmax = 0;
                size = 0;
                nbfiles = 0;

                foreach (FileInfo file in sourcefiles) // Loop to allow calculation of files and folder size
                {
                    TotalSize += file.Length;
                    nbfilesmax++;
                }
                foreach (DirectoryInfo subdir in dirs) // Loop to allow calculation of subfiles and subfolder size
                {
                    FileInfo[] Maxfiles = subdir.GetFiles();
                    foreach (FileInfo file in Maxfiles)
                    {
                        TotalSize += file.Length;
                        nbfilesmax++;
                    }
                }

            }

            // Loop that allows to copy the files to make the backup
            foreach (FileInfo file in sourcefiles)
            {
                string tempPath = Path.Combine(OutPath, file.Name);

                stateModel.SourceFilePath = Path.Combine(OutPath, file.Name);
                stateModel.TargetFilePath = tempPath;
                stateModel.TotalFilesToCopy = nbfilesmax;
                stateModel.TotalFilesSize = TotalSize;
                stateModel.NbFilesLeftToDo = nbfilesmax - nbfiles;
                stateModel.State = "IN PROGRESS";

                stateManagerService.UpdateStateFile(stateManagerService.listStateModel!);

                // Systems which allows to insert the values of each file in the report file.

                file.CopyTo(tempPath, true); // Function that allows you to copy the file to its new folder.
                nbfiles++;
                fileDo += nbfiles;
                size += file.Length;
                saveModel.percentage = (int)((fileDo * 100) / fileTotal);

                // Calculate the global percentage of execution and store it in saveModel.percentage

            }

            // If copying subdirectories, copy them and their contents to new location.

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(OutPath, subdir.Name);
                CompleteSave(subdir.FullName, tempPath, true, saveModel, index);
            }

            stopwatch.Stop(); // Stop the stopwatch
            if (Convert.ToBoolean(saveModel.EncryptChoice)) EncryptFile(OutPath); // Encrypt the file
            return false;
        }
        private bool IncrementalSave(string InPath, string OutPath, bool verif, SaveModel saveModel, int index)
        {
            // Wait for pauseEvent to be signaled before proceeding
            listThreads[index].Item2.WaitOne(0);

            while (IsProcessRunning("mspaint.exe") && !listThreads[index].Item3.WaitOne(0))
            {
                // Software is running, wait for it to close
                Thread.Sleep(1000);
                // Display message box to inform the user that the software is still running
            }

            if (listThreads[index].Item4.WaitOne(0))
            {
                return false;
            }

            // Implement the logic with the event resume, pause, and stop

            // Wait for resumeEvent to be signaled before continuing
            listThreads[index].Item3.WaitOne();

            DirectoryInfo sourceDir = new DirectoryInfo(InPath);
            DirectoryInfo destinationDir = new DirectoryInfo(OutPath);

            // Create the destination directory if it doesn't exist
            if (!destinationDir.Exists)
            {
                destinationDir.Create();
            }

            long totalSize = 0;
            long filesCopied = 0;


            FileInfo[] sourceFiles = sourceDir.GetFiles("*.*", SearchOption.AllDirectories);

            List<string> priorities = priority?.Split(',')?.ToList() ?? new List<string>();

            // Prioritize files based on the extension's presence in the priorities list
            var fileList = sourceFiles.OrderBy(x => priorities.Contains(Path.GetExtension(x.FullName).ToLower())).Reverse().ToList();



            // Get the list of files in the destination directory
            FileInfo[] destinationFiles = destinationDir.GetFiles("*.*", SearchOption.AllDirectories);

            // Dictionary to store the last modified time of files in the destination directory
            Dictionary<string, DateTime> destinationFileLastModified = new Dictionary<string, DateTime>();
            foreach (FileInfo file in destinationFiles)
            {
                destinationFileLastModified[file.FullName] = file.LastWriteTimeUtc;
            }


            foreach (FileInfo sourceFile in sourceFiles)
            {
                string relativePath = sourceFile.FullName.Substring(sourceDir.FullName.Length + 1);
                string destinationFilePath = Path.Combine(OutPath, relativePath);
                DateTime lastModifiedTime;

                saveModel.percentage = (int)((++filesCopied * 100) / sourceFiles.Length);
                stateManagerService.UpdateStateFile(stateManagerService.listStateModel!);

                if (destinationFileLastModified.TryGetValue(destinationFilePath, out lastModifiedTime))
                {
                    // Compare last modified time of source and destination files
                    if (sourceFile.LastWriteTimeUtc > lastModifiedTime)
                    {
                        // File in source directory is newer, copy it to destination
                        string destinationDirectory = Path.GetDirectoryName(destinationFilePath)!;
                        if (!Directory.Exists(destinationDirectory))
                        {
                            Directory.CreateDirectory(destinationDirectory);
                        }

                        sourceFile.CopyTo(destinationFilePath, true);
                        totalSize += sourceFile.Length;
                        filesCopied++;
                    }
                }
                else
                {
                    // File does not exist in the destination directory, copy it
                    string destinationDirectory = Path.GetDirectoryName(destinationFilePath)!;
                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    sourceFile.CopyTo(destinationFilePath, false);
                    totalSize += sourceFile.Length;
                    filesCopied++;
                }
            }

            // Update saveModel or any other necessary progress tracking here
            if (Convert.ToBoolean(saveModel.EncryptChoice)) EncryptFile(OutPath);
            return true;
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

        public (ObservableCollection<SaveModel>, bool) LoadSave()
        {
            listThreads.Clear();
            try
            {
                if (File.Exists("saves.json"))
                {
                    ObservableCollection<SaveModel> listSaveModel = JsonSerializer.Deserialize<ObservableCollection<SaveModel>>(File.ReadAllText("saves.json"))!;
                    foreach (SaveModel saveModel in listSaveModel)
                    {
                        stateManagerService.listStateModel!.Clear();
                        stateManagerService.listStateModel!.Add(new StateManagerModel { SaveName = saveModel.SaveName, State = "END" });
                    }

                    foreach (SaveModel saveModel in listSaveModel)
                    {
                        listThreads.Add((new Thread(() => { }), new ManualResetEvent(false), new ManualResetEvent(true), new ManualResetEvent(false), saveModel));
                    }

                    string jsonString = JsonSerializer.Serialize(listSaveModel, options);
                    File.WriteAllText("saves.json", jsonString);
                }
                else
                {
                    //TODO : Gerer le else
                }

                ObservableCollection<SaveModel> ListSaveModel = new ObservableCollection<SaveModel>();
                foreach (var item in listThreads)
                {
                    ListSaveModel.Add(item.savemodel);
                }

                return (ListSaveModel, true);
            }
            catch
            {
                return (new ObservableCollection<SaveModel>(), false);
            }
        }
        public Tuple<string, string, string, string, DateTime> ShowInfo(SaveModel saveModel)
        {
            return Tuple.Create(saveModel.SaveName, saveModel.InPath, saveModel.OutPath, saveModel.Type, saveModel.Date);
        }

        public bool CreateSave(SaveModel saveModel)
        {
            try
            {

                listThreads.Add((new Thread(() => { }), new ManualResetEvent(false), new ManualResetEvent(true), new ManualResetEvent(false), saveModel));
                stateManagerService.listStateModel!.Add(new StateManagerModel { SaveName = saveModel.SaveName, State = "END" });

                ObservableCollection<SaveModel> ListSaveModel = new();

                foreach (var item in listThreads)
                {
                    ListSaveModel.Add(item.savemodel);
                }

                File.WriteAllText("saves.json", JsonSerializer.Serialize(ListSaveModel, options));
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteSave(SaveModel saveModel)
        {
            try
            {
                // Supprimer l'élément de listThreads
                int index = 0;
                foreach (var item in listThreads.ToList()) // ToList() pour travailler sur une copie
                {
                    if (item.Item5.SaveName == saveModel.SaveName)
                    {
                        listThreads.RemoveAt(index);
                        break; // Sortir de la boucle après avoir supprimé le premier élément correspondant
                    }
                    else
                    {
                        index++;
                    }
                }

                // Supprimer l'élément de stateManagerService.listStateModel
                var stateModelToRemove = stateManagerService.listStateModel!.FirstOrDefault(x => x.SaveName == saveModel.SaveName);
                if (stateModelToRemove != null)
                {
                    stateManagerService.listStateModel!.Remove(stateModelToRemove);
                }

                // Mettre à jour le fichier "saves.json"
                ObservableCollection<SaveModel> ListSaveModel = new ObservableCollection<SaveModel>();
                foreach (var item in listThreads)
                {
                    ListSaveModel.Add(item.Item5);
                }
                File.WriteAllText("saves.json", JsonSerializer.Serialize(ListSaveModel, options));

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int CountFiles(string directoryPath)
        {
            int fileCount = 0;

            // Count files in the current directory
            fileCount += Directory.GetFiles(directoryPath).Length;

            // Recursively count files in subdirectories
            foreach (string subDirectory in Directory.GetDirectories(directoryPath))
            {
                fileCount += CountFiles(subDirectory);
            }

            return fileCount;
        }

    }
}
