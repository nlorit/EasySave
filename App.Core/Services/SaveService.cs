using App.Core.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App.Core.Services
{
    public class SaveService
    {

        public ObservableCollection<(Thread thread, ManualResetEvent pauseEvent, ManualResetEvent resumeEvent, ManualResetEvent stopEvent, SaveModel savemodel)> listThreads { get; private set; } = new ObservableCollection<(Thread, ManualResetEvent, ManualResetEvent, ManualResetEvent, SaveModel)>();
        public (Thread thread, ManualResetEvent pauseEvent, ManualResetEvent resumeEvent, ManualResetEvent stopEvent, SaveModel savemodel) currentThread { get; private set; }

        private readonly ConfigService? configService = new();

        public string? priority { get; set; }


        JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private readonly LoggerService? loggerService = new();
        private readonly StateManagerService? stateManagerService = new();

        private LoggerModel loggerModel = new();

        public SaveService()
        {
            ObservableCollection<SaveModel> ListSaveModel = new();
            (ListSaveModel, _) = LoadSave();

            priority = configService!.Priority;
    }

        public bool IsProcessRunning()
        {
            return Process.GetProcesses().Any(process => process.ProcessName.Equals(configService!.Software, StringComparison.OrdinalIgnoreCase));
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
                        saveModel.fileDo = 0;
                        saveModel.fileTotal = CountFiles(saveModel.InPath);
                        listThreads[i].stopEvent.Reset();
                        listThreads[i].resumeEvent.Set(); // Signal the resume event to allow the save operation to proceed
                        Thread thread = new Thread(() =>
                        {
                            CompleteSave(saveModel.InPath, saveModel.OutPath, saveModel, i);
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
                        saveModel.fileDo = 0;
                        saveModel.fileTotal = CountFiles(saveModel.InPath);
                        listThreads[i].stopEvent.Reset();
                        listThreads[i].resumeEvent.Set(); // Signal the resume event to allow the save operation to proceed
                        Thread thread = new Thread(() =>
                        {
                            IncrementalSave(saveModel.InPath, saveModel.OutPath, saveModel, i);
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

        public void IncrementalSave(string InPath, string OutPath, SaveModel saveModel, int index)
        {
            DirectoryInfo DirIn = new DirectoryInfo(InPath);
            DirectoryInfo DirOut = new DirectoryInfo(OutPath);


            //give me the loggermodel variables
            loggerModel.Name = saveModel.SaveName;

            stateManagerService!.listStateModel![index].TotalFilesSize = new DirectoryInfo(InPath).GetFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length); ;


            CopyDifferential(DirIn, DirOut, saveModel, index);

            if (saveModel.EncryptChoice == "True")
            {
                EncryptFile(OutPath);
            }


            stateManagerService!.listStateModel![index].State = "END";
            stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);
            saveModel.percentage = 100;
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

        private void CompleteSave(string InPath, string OutPath, SaveModel saveModel, int index)
        {

            DirectoryInfo DirIn = new DirectoryInfo(InPath);
            DirectoryInfo DirOut = new DirectoryInfo(OutPath);

            loggerModel.Name = saveModel.SaveName;

            stateManagerService!.listStateModel![index].TotalFilesSize = new DirectoryInfo(InPath).GetFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);

            CopyAll(DirIn, DirOut, saveModel, index);

            if(saveModel.EncryptChoice == "True")
            {
                EncryptFile(OutPath);
            }

            stateManagerService!.listStateModel![index].State = "END";
            stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);
            saveModel.percentage = 100;


        }


        private bool CopyAll(DirectoryInfo source, DirectoryInfo target, SaveModel saveModel, int index)
        {

            listThreads[index].Item2.WaitOne(0);

            while (IsProcessRunning() && !listThreads[index].Item3.WaitOne(0))
            {
                Thread.Sleep(1000);
            }

            if (listThreads[index].Item4.WaitOne(0))
            {
                return false;
            }

            listThreads[index].Item3.WaitOne();

            stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);

            int i = 0;
            foreach (var item in stateManagerService!.listStateModel!)
            {
                if (item.SaveName == saveModel.SaveName)
                {
                    break;
                }
                else
                {
                    i++;
                }
            }

            stateManagerService!.listStateModel![i].State = "ACTIVE";
            stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);

            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {

                saveModel.percentage = (long)(((float)saveModel.fileDo++ / (float)saveModel.fileTotal) * 100);

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                stopwatch.Stop();

                loggerModel.FileSource = fi.FullName;
                loggerModel.FileTarget = Path.Combine(target.FullName, fi.Name);
                loggerModel.FileTransferTime = stopwatch.ElapsedMilliseconds.ToString();
                loggerModel.FileSize = fi.Length.ToString();

                stateManagerService!.listStateModel![i].SourceFilePath = fi.FullName;
                stateManagerService!.listStateModel![i].TargetFilePath = Path.Combine(target.FullName, fi.Name);
                stateManagerService!.listStateModel![i].Progression = saveModel.percentage;
                stateManagerService!.listStateModel![i].TotalFilesToCopy = saveModel.fileTotal;
                stateManagerService!.listStateModel![i].NbFilesLeftToDo = saveModel.fileTotal - saveModel.fileDo;
                stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);

                loggerService!.AddEntryLog(loggerModel);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir,saveModel, index);
            }
            return true;
        }

        private bool CopyDifferential(DirectoryInfo source, DirectoryInfo target, SaveModel saveModel, int index)
        {
            listThreads[index].Item2.WaitOne(0);

            while (IsProcessRunning() && !listThreads[index].Item3.WaitOne(0))
            {
                Thread.Sleep(1000);
            }

            if (listThreads[index].Item4.WaitOne(0))
            {
                return false;
            }

            listThreads[index].Item3.WaitOne();

            stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);

            int i = 0;
            foreach (var item in stateManagerService!.listStateModel!)
            {
                if (item.SaveName == saveModel.SaveName)
                {
                    break;
                }
                else
                {
                    i ++;
                }
            }

            Directory.CreateDirectory(target.FullName);

            stateManagerService!.listStateModel![i].State = "ACTIVE";
            stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);

            // Copy each file into the new directory if it's modified or doesn't exist in target
            foreach (FileInfo fi in source.GetFiles())
            {
                FileInfo targetFile = new FileInfo(Path.Combine(target.FullName, fi.Name));

                if (!targetFile.Exists || fi.LastWriteTimeUtc > targetFile.LastWriteTimeUtc)
                {
                    saveModel.percentage = (long)(((float)saveModel.fileDo++ / (float)saveModel.fileTotal) * 100);
                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                    stopwatch.Stop();

                    loggerModel.FileSource = fi.FullName;
                    loggerModel.FileTarget = Path.Combine(target.FullName, fi.Name);
                    loggerModel.FileTransferTime = stopwatch.ElapsedMilliseconds.ToString();
                    loggerModel.FileSize = fi.Length.ToString();

                    stateManagerService!.listStateModel![i].SourceFilePath = fi.FullName;
                    stateManagerService!.listStateModel![i].TargetFilePath = Path.Combine(target.FullName, fi.Name);
                    stateManagerService!.listStateModel![i].Progression = saveModel.percentage;
                    stateManagerService!.listStateModel![i].TotalFilesToCopy = saveModel.fileTotal;
                    stateManagerService!.listStateModel![i].NbFilesLeftToDo = saveModel.fileTotal - saveModel.fileDo;
                    stateManagerService!.UpdateStateFile(stateManagerService.listStateModel!);

                    loggerService!.AddEntryLog(loggerModel);
                }
            }

            // Recursively copy subdirectories differentially
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyDifferential(diSourceSubDir, nextTargetSubDir, saveModel, index);
            }

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
                    stateManagerService!.listStateModel!.Clear();
                    foreach (SaveModel saveModel in listSaveModel)
                    {
                        
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
