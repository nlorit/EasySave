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
        public readonly SaveService saveService = new();
        private readonly HashSet<SaveModel> runningSaves = new(); // HashSet to store the IDs of running saves
        public int percentage { get; set; } = 100;
        public bool IsLoadCorrectly { get; set; }



        public MainViewModel()
        {
            (saves,IsLoadCorrectly) = saveService.LoadSave();
        }

        public void AddSave(SaveModel save)
        {
            saveService.CreateSave(save);
        }

        public void DeleteSave(SaveModel saveModel)
        {
            saveService.DeleteSave(saveModel);
            saves.Clear();
            foreach (var item in saveService.listThreads)
            {
                saves.Add(item.savemodel);
            }
        }

        public void PlaySave(SaveModel saveModel)
        {
            saveService.ResumeSave(saveModel);
        }

        public void PauseSave(SaveModel saveModel)
        {
            saveService.PauseSave(saveModel);
        }

        public void StopSave(SaveModel saveModel)
        {
            saveService.StopSave(saveModel);
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

        public ObservableCollection<SaveModel> LoadSave()
        {
            (ObservableCollection<SaveModel> saves,IsLoadCorrectly) = saveService.LoadSave();
            return saves ;
        }
    }
}
