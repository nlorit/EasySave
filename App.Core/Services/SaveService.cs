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
        private readonly ConfigService configService = new();
        public CopyService copyService = new();
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };


        public SaveService() 
        { 
            LoadSave();
            copyService.stateManagerService.listStateModel = ListStateManager;
            copyService.stateManagerService.UpdateStateFile();
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

        public void ExecuteSave(SaveModel saveModel)
        {
            // Method to execute the copy service
            // Execute the copy service
            copyService.CopyModel.SourcePath = saveModel.InPath;
            copyService.CopyModel.TargetPath = saveModel.OutPath;
            copyService.stateManagerService.listStateModel = ListStateManager;
            copyService.ExecuteCopy(saveModel);

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

        /// <summary>
        /// Show info of a save
        /// </summary>
        /// <param name="saveModel"></param>
        /// <returns></returns>
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
