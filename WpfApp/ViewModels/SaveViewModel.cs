using App.Core.Models;
using App.Core.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Cmd.ViewModels
{

    public partial class SaveViewModel
    {
        private readonly SaveService saveService;
        private readonly StateManagerService stateManagerService;
        private readonly LoggerService loggerService;
        public SaveModel? Model { get; set; }
        public ObservableCollection<SaveModel> ListSaveModel { get; set; } = [];
        public List<StateManagerModel> StateManagerList { get; set; } = [];


        public ObservableCollection<SaveModel> ChargerSauvegardes()
        {

                if (File.Exists("saves.json"))
                {
                    string json = File.ReadAllText("saves.json");
                    ListSaveModel = JsonConvert.DeserializeObject<ObservableCollection<SaveModel>>(json)!;
                    Console.WriteLine(ListSaveModel);
                    return ListSaveModel;  // Ajoutez cette ligne pour retourner la liste après la désérialisation.
                }
                else
                {
                    // Créer le fichier s'il n'existe pas
                    File.WriteAllText("saves.json", "[]");
                    return [];
                } 
        }

        public enum TypeOfSave
        {
            Sequential,
            Complete
        }


        public SaveViewModel()
        {
            saveService = new();
            stateManagerService = new();
            loggerService = new();
            ChargerSauvegardes();

        }

        public ObservableCollection<SaveModel> GetSave()
        {
            return ListSaveModel;
        }


        public void CreateSave()
        {

        }

        /// <summary>
        /// Method to show the logs
        /// </summary>
        public void ShowLogs()
        {
            //Method to show the logs

            loggerService.OpenLogFile();
        }

        /// <summary>
        /// Method to show the state file
        /// </summary>
        public void ShowStateFile()
        {
            //Method to show the state file
            stateManagerService.OpenStateFile();
        }

        /// <summary>
        /// Method to show the saves schedule
        /// </summary>
        public void ShowSavesSchedule()
        {

        }
    }
}
