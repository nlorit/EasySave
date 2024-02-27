using App.Core.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App.Core.Services
{
    public class SaveService : IDisposable
    {

        public ObservableCollection<StateManagerModel> ListStateManager { get; set; } = [];
        public ObservableCollection<SaveModel> ListSaveModel { get; set; } = [];
        public ManualResetEvent pauseEvent { get; private set; } = new ManualResetEvent(false);
        public ManualResetEvent resumeEvent { get; private set; } = new ManualResetEvent(true);
        public ManualResetEvent stopEvent { get; private set; } = new ManualResetEvent(false);

        private int progress = 0;
        //TODO : Mettre manualResetsEvent et autre event resume 

        private readonly StreamWriter? logWriter ;
        private readonly StreamWriter? stateWriter;

        private readonly StateManagerService stateManagerService = new();
        public CopyService copyService = new();
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        private Thread saveThread;
        private long fileDo =0;
        private long fileTotal = 0;


        public SaveService() 
        { 
            //logWriter = new StreamWriter(loggerService.GetLogFormat());
            //stateWriter = new StreamWriter(StateManagerService.stateFilePath);
            LoadSave();
            copyService.stateManagerService.listStateModel = ListStateManager;
            //copyService.stateManagerService.UpdateStateFile(stateWriter);
        }

        public bool IsSoftwareRunning()
        {
            // Check if the software is running (e.g., "notepad.exe")

            Process[] processes = Process.GetProcessesByName("mspaint");
            return processes.Length > 0;
            

        }

        public void ExecuteSave(List<SaveModel> saveModel, ThreadManagerService threadManagerService)
        {
            foreach (SaveModel save in saveModel)
            {
                copyService = new();
                copyService.CopyModel.SourcePath = save.InPath;
                copyService.CopyModel.TargetPath = save.OutPath;
                copyService.stateManagerService.listStateModel = ListStateManager;

                // Start copying process in a separate thread


                copyService.BackupStarted += (sender, args) =>Console.WriteLine($"Backup started from {args.SourcePath} to {args.TargetPath} at {args.StartTime}");

                copyService.BackupCompleted += (sender, args) => Console.WriteLine($"Backup completed from {args.SourcePath} to {args.TargetPath} at {args.EndTime}");


                Thread thread = new Thread(() => copyService.ExecuteCopy(save, logWriter!, stateWriter!));
                threadManagerService.AddThread(thread);
                thread.Start();

                copyService.BackupStarted -= (sender, args) => Console.WriteLine($"Backup started from {args.SourcePath} to {args.TargetPath} at {args.StartTime}");

                copyService.BackupCompleted -= (sender, args) =>Console.WriteLine($"Backup completed from {args.SourcePath} to {args.TargetPath} at {args.EndTime}");


            }
        }

        public void LaunchSave(SaveModel saveModel)
        {
            //TODO : Detection logicel Métier

            if (saveModel.Type == "Complete")
            {
                 // Set the resume event to allow it to be resumed later
                saveThread = new Thread(() =>
                {
                    //resumeEvent.Set();
                    //pauseEvent.Set();

                    stopEvent.Reset();
                    fileDo = 0;
                    CompleteSave(saveModel.InPath, saveModel.OutPath, true, saveModel);
                    fileDo = fileTotal;
                    saveModel.percentage = (fileDo * 100 / fileTotal);
                });

                // Start the thread
                saveThread.Start();

            }

            if (saveModel.Type == "Incremental")
            {
                //IncrementalSave(saveModel.InPath, saveModel.OutPath, true);
            }
        }

        public void PauseSave()
        {
            pauseEvent.Set(); // Reset the pause event to block threads
            resumeEvent.Reset(); // Set the resume event to allow it to be resumed later
        }

        public void StopSave()
        {
            fileDo = 0;
            stopEvent.Set(); // Signal the stop event to stop the save operation
        }

        public void ResumeSave()
        {
            pauseEvent.Reset(); // Signal the pause event to resume the save operation
            resumeEvent.Set(); // Reset the resume event to block it until it's signaled again
        }



        private bool CompleteSave(string InPath, string OutPath, bool verif, SaveModel saveModel)
        {
            // DataState = new DataState(NameStateFile);
            // this.DataState.SaveState = true;

            // Wait for pauseEvent to be signaled before proceeding
            pauseEvent.WaitOne(0);

            while (IsSoftwareRunning() && !stopEvent.WaitOne(0))
            {
                // Software is running, wait for it to close
                Thread.Sleep(1000);
                // Display message box to inform the user that the software is still running
            }

            if (stopEvent.WaitOne(0))
            {
                // Stop event has been signaled, return false to indicate incomplete save
                return false;
            }

            // Implement the logic with the event resume, pause, and stop

            // Wait for resumeEvent to be signaled before continuing
            resumeEvent.WaitOne();

            fileTotal += Directory.GetFiles(InPath).Length;

            // Recursively count files in subdirectories

            fileTotal += CountFiles(InPath);

            StateManagerModel stateModel = new()
            {
                SaveName = saveModel.SaveName,
                State = "END"
            };

            foreach (StateManagerModel item in ListStateManager)
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
            if (verif) // Check for the status file if it needs to reset the variables
            {
                TotalSize = 0;
                nbfilesmax = 0;
                size = 0;
                nbfiles = 0;

                foreach (FileInfo file in files) // Loop to allow calculation of files and folder size
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
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(OutPath, file.Name);

                stateModel.SourceFilePath = Path.Combine(OutPath, file.Name);
                stateModel.TargetFilePath = tempPath;
                stateModel.TotalFilesToCopy = nbfilesmax;
                stateModel.TotalFilesSize = TotalSize;
                stateModel.NbFilesLeftToDo = nbfilesmax - nbfiles;
                stateModel.State = "IN PROGRESS";

                stateManagerService.UpdateStateFile(ListStateManager);

                // Systems which allows to insert the values of each file in the report file.

                file.CopyTo(tempPath, true); // Function that allows you to copy the file to its new folder.
                nbfiles++;
                fileDo+= nbfiles;
                size += file.Length;
                saveModel.percentage = (int)((fileDo * 100) / fileTotal);

                // Calculate the global percentage of execution and store it in saveModel.percentage

            }

            // If copying subdirectories, copy them and their contents to new location.

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(OutPath, subdir.Name);
                CompleteSave(subdir.FullName, tempPath, true, saveModel);
            }

            stopwatch.Stop(); // Stop the stopwatch
            return false;
        }


        public ObservableCollection<SaveModel> LoadSave()
        {

            if (File.Exists("saves.json"))
            {
                ListSaveModel = JsonSerializer.Deserialize<ObservableCollection<SaveModel>>(File.ReadAllText("saves.json"))!;

                foreach (SaveModel saveModel in ListSaveModel)
                {
                    StateManagerModel stateModel = new()
                    {
                        SaveName = saveModel.SaveName,
                        SourceFilePath = saveModel.InPath,
                        TargetFilePath = saveModel.OutPath,
                        State = "END"
                    };
                    ListStateManager.Add(stateModel);
                }
                
            }
            else
            {
                File.WriteAllText("saves.json", "[]");
            }

            return ListSaveModel;

        }

        public Tuple<string, string, string, string, DateTime> ShowInfo(SaveModel saveModel)
        {
            return Tuple.Create(saveModel.SaveName, saveModel.InPath, saveModel.OutPath, saveModel.Type, saveModel.Date);
        }

        public bool CreateSave(SaveModel saveModel)
        {
            try
            {
                ListSaveModel.Add(saveModel);
                ListStateManager.Add(new StateManagerModel { SaveName = saveModel.SaveName, SourceFilePath = saveModel.InPath, TargetFilePath = saveModel.OutPath });
                File.WriteAllText("saves.json", JsonSerializer.Serialize(ListSaveModel, options));
                return true;
            }
            catch
            {
                return false;
            }
        }
       

        public void DeleteSave(SaveModel saveModel)
        {
            ListSaveModel.Remove(saveModel);
        }

        public void Dispose()
        {
            logWriter?.Dispose();
            stateWriter?.Dispose();
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
