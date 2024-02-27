using App.Core.Models;
using App.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Animation;

namespace WpfApp.ViewModels
{
    public class MainViewModel
    {
        public readonly ObservableCollection<SaveModel> saves = new();
        private readonly SaveService saveService = new();
        private readonly HashSet<SaveModel> runningSaves = new(); // HashSet to store the IDs of running saves



        public MainViewModel()
        {
            saves = saveService.LoadSave();
        }

        public void AddSave(SaveModel save)
        {
            saveService.CreateSave(save);
        }



        public void DeleteSave(SaveModel saveModel)
        {
            saveService.DeleteSave(saveModel);
        }

        public void PlaySave()
        {
            saveService.ResumeSave();
        }

        public void PauseSave()
        {
            saveService.PauseSave();
        }

        public void StopSave()
        {
            saveService.StopSave();
        }

        public bool IsSaveRunning(SaveModel saveModel)
        {
            lock (runningSaves)
            {
                return runningSaves.Contains(saveModel); // Check if the saveModel instance is in the runningSaves collection
            }
        }

        public void LaunchSave(SaveModel saveModel)
        {
            // Vérifiez si la sauvegarde est en cours d'exécution
            saveService.LaunchSave(saveModel);
            
        }



    }
}
