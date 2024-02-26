using App.Core.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Security.Policy;
using System.Threading;

namespace App.Core.Services
{
    public class SaveService : IDisposable
    {

        public ObservableCollection<StateManagerModel> ListStateManager { get; set; } = [];
        public ObservableCollection<SaveModel> ListSaveModel { get; set; } = [];
        public AutoResetEvent pauseEvent { get; private set; } = new AutoResetEvent(false);

        private readonly StreamWriter? logWriter ;
        private readonly StreamWriter? stateWriter;

        private readonly ConfigService configService = new();
        private readonly StateManagerService stateManagerService = new();
        private readonly LoggerService loggerService = new();
        public CopyService copyService = new();
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        private Thread saveThread;
        private bool isPaused;
        private bool shouldAbort;

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
            Process[] processes = Process.GetProcessesByName("Paint.exe");
            if (processes.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

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
           
                saveThread = new Thread(() =>
                
                   CompleteSave(saveModel.InPath, saveModel.OutPath, true, saveModel)
                );

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
            Thread pauseThread = new Thread(() =>
            {
                isPaused = true;
                this.pauseEvent.WaitOne(); // Met en pause le thread jusqu'à ce que le signal soit reçu
            });

            pauseThread.Start(); // Start the thread; // Met en pause le thread jusqu'à ce que le signal soit reçu
        }

        public void StopSave()
        {

        }

        // Ajoutez une méthode pour reprendre la sauvegarde
        public void ResumeSave()
        {
            isPaused = false;
            pauseEvent.Set(); // Signale au thread de reprendre
        }



        private bool CompleteSave(string InPath, string OutPath, bool verif, SaveModel saveModel)
        {
            //DataState = new DataState(NameStateFile);
            //this.DataState.SaveState = true;

            while (IsSoftwareRunning())
            {
                //wait for the software to close
                Thread.Sleep(1000);
                //display message box to inform the user that the software is still running

            }

            if (isPaused)
            {
                // Attendre l'événement de reprise avant de continuer
                pauseEvent.WaitOne(); // Attend que l'événement soit déclenché
            }



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
            float progs = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start(); //Starting the timed for the log file

            DirectoryInfo dir = new DirectoryInfo(InPath);  // Get the subdirectories for the specified directory.

            if (!dir.Exists) //Check if the file is present
            {
                throw new DirectoryNotFoundException("ERROR 404 : Directory Not Found ! " + InPath);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            Directory.CreateDirectory(OutPath); // If the destination directory doesn't exist, create it.  

            FileInfo[] files = dir.GetFiles(); // Get the files in the directory and copy them to the new location.

            if (!verif) //  Check for the status file if it needs to reset the variables
            {
                TotalSize = 0;
                nbfilesmax = 0;
                size = 0;
                nbfiles = 0;
                progs = 0;

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

            //Loop that allows to copy the files to make the backup
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(OutPath, file.Name);

                if (size > 0)
                {
                    progs = ((float)size / TotalSize) * 100;
                }

                stateModel.SourceFilePath = Path.Combine(OutPath, file.Name);
                stateModel.TargetFilePath = tempPath;
                stateModel.TotalFilesToCopy = nbfilesmax;
                stateModel.TotalFilesSize = TotalSize;
                stateModel.NbFilesLeftToDo = nbfilesmax - nbfiles;
                stateModel.Progression = progs;
                stateModel.State = "IN PROGRESS";
         
                stateManagerService.UpdateStateFile(ListStateManager);

                //Systems which allows to insert the values ​​of each file in the report file.

                file.CopyTo(tempPath, true); //Function that allows you to copy the file to its new folder.
                nbfiles++;
                size += file.Length;

            }

            // If copying subdirectories, copy them and their contents to new location.

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(OutPath, subdir.Name);
                CompleteSave(subdir.FullName, tempPath, true, saveModel);
            }


            stopwatch.Stop(); //Stop the stopwatch
            return false;
            //this.TimeTransfert = stopwatch.Elapsed; // Transfer of the chrono time to the variable
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
    }
}
