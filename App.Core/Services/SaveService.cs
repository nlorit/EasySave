using App.Core.Models;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App.Core.Services
{
    public class SaveService
    {

        public required ObservableCollection<StateManagerModel> ListStateManager { get; set; } = [];
        public required ObservableCollection<SaveModel> ListSaveModel { get; set; } = [];

        private readonly StreamWriter? logWriter ;
        private readonly StreamWriter? stateWriter;

        


        private readonly ConfigService configService = new();
        private readonly LoggerService loggerService = new();
        public CopyService copyService = new();
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };


        public SaveService() 
        { 
            logWriter = new StreamWriter(loggerService.GetLogFormat());
            stateWriter = new StreamWriter(StateManagerService.stateFilePath);
            LoadSave();
            copyService.stateManagerService.listStateModel = ListStateManager;
            copyService.stateManagerService.UpdateStateFile(stateWriter);
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

        public void ExecuteSave(List<SaveModel> saveModel)
        {
            foreach (SaveModel save in saveModel)
            {
                copyService = new();
                copyService.CopyModel.SourcePath = save.InPath;
                copyService.CopyModel.TargetPath = save.OutPath;
                copyService.stateManagerService.listStateModel = ListStateManager;

                // Start copying process in a separate thread


                copyService.BackupStarted += (sender, args) =>
                {
                    Console.WriteLine($"Backup started from {args.SourcePath} to {args.TargetPath} at {args.StartTime}");
                };

                copyService.BackupCompleted += (sender, args) =>
                {
                    Console.WriteLine($"Backup completed from {args.SourcePath} to {args.TargetPath} at {args.EndTime}");
                };
                Thread thread = new Thread(() =>
                {
                    copyService.ExecuteCopy(save, logWriter!, stateWriter!);
                });
                thread.Start();

                copyService.BackupStarted -= (sender, args) =>
                {
                    Console.WriteLine($"Backup started from {args.SourcePath} to {args.TargetPath} at {args.StartTime}");
                };

                copyService.BackupCompleted -= (sender, args) =>
                {
                    Console.WriteLine($"Backup completed from {args.SourcePath} to {args.TargetPath} at {args.EndTime}");
                };


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



        public void LoadSave()
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

        }

        public Tuple<string, string, string, string, DateTime> ShowInfo(SaveModel saveModel)
        {
            return Tuple.Create(saveModel.SaveName, saveModel.InPath, saveModel.OutPath, saveModel.Type, saveModel.Date);
        }


       

        public bool CreateSave(string inPath, string outPath, string type, string saveName)
        {
            try
            {
                ListSaveModel.Add(new SaveModel { InPath = inPath, OutPath = outPath, Type = type, SaveName = saveName});
                ListStateManager.Add(new StateManagerModel { SaveName = saveName, SourceFilePath = inPath, TargetFilePath = outPath });
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
    }
}
