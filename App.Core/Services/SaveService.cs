using App.Core.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Resources;

namespace App.Core.Services
{
    public class SaveService
    {

        public required ObservableCollection<StateManagerModel> ListStateManager { get; set; }
        public required ObservableCollection<SaveModel> ListSaveModel { get; set; }


        public SaveService() 
        { 

            LoadSave();
        }

        public static void ExecuteSave(SaveModel saveModel)

        {   //Method to execute the copy service
            CopyService copyService = new CopyService();
            //Execute the copy service
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel);


        }
            
        /// <summary>
        ///  Load Save from saves.json
        /// </summary>
        public void LoadSave()
        {

            if (File.Exists("saves.json"))
            {
                ListSaveModel = JsonConvert.DeserializeObject<ObservableCollection<SaveModel>>(File.ReadAllText("saves.json"))!;
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
                ListStateManager.Add(new StateManagerModel { SaveName = saveName });
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
