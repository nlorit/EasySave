using App.Core.Models;
using App.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;

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
        }

        public void PauseSave()
        {
        }

        public void StopSave()
        {
        }

        public bool IsSaveRunning(SaveModel saveModel)
        {
            lock (runningSaves)
            {
                return runningSaves.Contains(saveModel); // Check if the saveModel instance is in the runningSaves collection
            }
        }

        public void StartSave(SaveModel saveModel)
        {
            lock (runningSaves)
            {
                runningSaves.Add(saveModel); // Add the saveModel instance to the runningSaves collection
            }

            // Start the save operation
            saveService.LaunchSave(saveModel);

            // Remove the saveModel instance from the runningSaves collection when the operation is completed
            lock (runningSaves)
            {
                runningSaves.Remove(saveModel);
            }
        }

    }
}
