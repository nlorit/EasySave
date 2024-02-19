using App.Core.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace App.Core.Services
{
    public class StateManagerService
    {

        public ObservableCollection<StateManagerModel>? listStateModel;
        private readonly string stateFilePath = "state.json";


        //Options for the JsonSerializer
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };


        public StateManagerService()
        {   
            //Create the state file if it does not exist
            if (!File.Exists(stateFilePath))
            {
                CreateStateFile();
            }
            //UpdateStateFile();

        }

        public void OpenStateFile()
        {
            // Open the state file in notepad
            System.Diagnostics.Process.Start("notepad.exe", stateFilePath);
        }

        public void UpdateStateFile()
        {
            // Clear the state file
            ClearStateFile();
            int i = 0;
            foreach (StateManagerModel stateModel in listStateModel!)
            {
                using (StreamWriter stateWriter = File.AppendText(stateFilePath))
                {
                    stateWriter.WriteLineAsync(JsonSerializer.Serialize(stateModel, options) + ",");
                }
                //Increment the index to get the next save name
                i += 1;
            }
        }

        public void CreateStateFile() 
        {
            // Create the state file
            File.WriteAllText(stateFilePath, "[]");
        }

        public void ClearStateFile()
        {
            // Clear the state file
            File.WriteAllText(stateFilePath, "");
        }


    }
}
