using App.Core.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Security.Policy;

namespace App.Core.Services
{
    public class SaveService : IDisposable
    {

        public ObservableCollection<StateManagerModel> ListStateManager { get; set; } = [];
        public ObservableCollection<SaveModel> ListSaveModel { get; set; } = [];

        private readonly StreamWriter? logWriter ;
        private readonly StreamWriter? stateWriter;

        private readonly ConfigService configService = new();
        private readonly StateManagerService stateManagerService = new();
        private readonly LoggerService loggerService = new();
        public CopyService copyService = new();
        private ManualResetEvent resetEvent = new ManualResetEvent(false);
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

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
            Process[] processes = Process.GetProcessesByName(configService.Software);
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

        public void PauseSave()
        {
            copyService.PauseCopy();
        }

        public void StopSave()
        {
            copyService.StopCopy();
        }

        public async Task<bool> LaunchSave(SaveModel saveModel)
        {
            //TODO : Detection logicel Métier

            if (saveModel.Type == "Complete")
            {
                // Run CompleteSave asynchronously
                await Task.Run(() =>
                {
                    while (CompleteSave(saveModel.InPath, saveModel.OutPath, true, saveModel))
                    {
                        // Optionally, you can introduce a small delay to avoid tight loops
                        Thread.Sleep(100); // Adjust as needed
                    }
                });

                return false;
            }

            if (saveModel.Type == "Incremental")
            {
                IncrementalSave(saveModel.InPath, saveModel.OutPath, true);
                return false;
            }

            return true;
        }



        private bool CompleteSave(string InPath, string OutPath, bool verif, SaveModel saveModel)
        {
            //DataState = new DataState(NameStateFile);
            //this.DataState.SaveState = true;
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
            //System which allows the values ​​to be reset to 0 at the end of the backup
            //DataState.TotalSize = TotalSize;
            //DataState.SourceFile = null;
            //DataState.TargetFile = null;
            //DataState.TotalFile = 0;
            //DataState.TotalSize = 0;
            //DataState.TotalSizeRest = 0;
            //DataState.FileRest = 0;
            //DataState.Progress = 0;
            //DataState.SaveState = false;

            //UpdateStatefile(); //Call of the function to start the state file system

            stopwatch.Stop(); //Stop the stopwatch
            return false;
            //this.TimeTransfert = stopwatch.Elapsed; // Transfer of the chrono time to the variable
        }

        //public void DifferentialSave(string pathA, string pathB, string pathC) // Function that allows you to make a differential backup
        //{
        //    DataState = new DataState(NameStateFile); //Instattation of the method
        //    Stopwatch stopwatch = new Stopwatch(); // Instattation of the method
        //    stopwatch.Start(); //Starting the stopwatch

        //    DataState.SaveState = true;
        //    TotalSize = 0;
        //    nbfilesmax = 0;

        //    System.IO.DirectoryInfo dir1 = new System.IO.DirectoryInfo(pathA);
        //    System.IO.DirectoryInfo dir2 = new System.IO.DirectoryInfo(pathB);

        //    // Take a snapshot of the file system.  
        //    IEnumerable<System.IO.FileInfo> list1 = dir1.GetFiles("*.*", System.IO.SearchOption.AllDirectories);
        //    IEnumerable<System.IO.FileInfo> list2 = dir2.GetFiles("*.*", System.IO.SearchOption.AllDirectories);

        //    //A custom file comparer defined below  
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

        //    //Loop that allows the backup of different files
        //    foreach (var v in queryList1Only)
        //    {
        //        string tempPath = Path.Combine(pathC, v.Name);
        //        //Systems which allows to insert the values ​​of each file in the report file.
        //        DataState.SourceFile = Path.Combine(pathA, v.Name);
        //        DataState.TargetFile = tempPath;
        //        DataState.TotalSize = nbfilesmax;
        //        DataState.TotalFile = TotalSize;
        //        DataState.TotalSizeRest = TotalSize - size;
        //        DataState.FileRest = nbfilesmax - nbfiles;
        //        DataState.Progress = progs;

        //        UpdateStatefile();//Call of the function to start the state file system
        //        v.CopyTo(tempPath, true); //Function that allows you to copy the file to its new folder.
        //        size += v.Length;
        //        nbfiles++;
        //    }

        //    //System which allows the values ​​to be reset to 0 at the end of the backup
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
        //}

        private void IncrementalSave(string InPath, string OutPath, bool verif)
        {

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
       

        //public bool CreateSave(string inPath, string outPath, string type, string saveName)
        //{
        //    try
        //    {
        //        ListSaveModel.Add(new SaveModel { InPath = inPath, OutPath = outPath, Type = type, SaveName = saveName});
        //        ListStateManager.Add(new StateManagerModel { SaveName = saveName, SourceFilePath = inPath, TargetFilePath = outPath });
        //        File.WriteAllText("saves.json", JsonSerializer.Serialize(ListSaveModel, options));
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}



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
