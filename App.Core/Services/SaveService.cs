using App.Core.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Resources;

namespace App.Core.Services
{
    public class SaveService
    {

        public required ObservableCollection<StateManagerModel> ListStateManager { get; set; } = [];
        public required ObservableCollection<SaveModel> ListSaveModel { get; set; } = [];
        private readonly CopyService copyService = new();


        public SaveService() 
        { 
            LoadSave();
            copyService.stateManagerService.listStateModel = ListStateManager;
            copyService.stateManagerService.UpdateStateFile();
        }

        public void ExecuteSave(SaveModel saveModel)

        {   //Method to execute the copy service
            //Execute the copy service
            copyService.CopyModel.SourcePath = saveModel.InPath;
            copyService.CopyModel.TargetPath = saveModel.OutPath;
            copyService.stateManagerService.listStateModel = ListStateManager;
            copyService.ExecuteCopy(saveModel);


        }
            
        /// <summary>
        ///  Load Save from saves.json
        /// </summary>
        public void LoadSave()
        {

            if (File.Exists("saves.json"))
            {
                ListSaveModel = JsonConvert.DeserializeObject<ObservableCollection<SaveModel>>(File.ReadAllText("saves.json"))!;

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

        /// <summary>
        /// Create a save
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="outPath"></param>
        /// <param name="type"></param>
        /// <param name="saveName"></param>
        /// <returns></returns>
        public bool CreateSave(string inPath, string outPath, string type, string saveName )
        {
            try
            {
                ListSaveModel.Add(new SaveModel { InPath = inPath, OutPath = outPath, Type = type, SaveName = saveName });
                ListStateManager.Add(new StateManagerModel { SaveName = saveName, SourceFilePath = inPath, TargetFilePath = outPath });
                File.WriteAllText("saves.json", JsonConvert.SerializeObject(ListSaveModel));
                return true;
            }
            catch 
            {
                return false;
            }

        }

        /// <summary>
        /// Delete a save
        /// </summary>
        /// <param name="saveModel"></param>
        public void DeleteSave(SaveModel saveModel)
        {
            ListSaveModel.Remove(saveModel);
        }
        



    }
}
