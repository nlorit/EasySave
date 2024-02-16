using App.Core.Models;
using System.Text.Json;

namespace App.Core.Services
{
    public class StateManagerService
    {

        private readonly List<StateManagerModel> listStateModel;
        private readonly string jsonStateFilePath = "state.json";


        //Options for the JsonSerializer
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };


        public StateManagerService(List<StateManagerModel> listStateModel)
        {
            //Create the state file if it does not exist
            if (!File.Exists(jsonStateFilePath))
            {
                CreateStateFile();
            }

            this.listStateModel = listStateModel;
            UpdateStateFile();

        }

        public void OpenStateFile()
        {
            // Open the state file in notepad
            System.Diagnostics.Process.Start("notepad.exe", jsonStateFilePath);
        }

        public void UpdateStateFile()
        {
            // Clear the state file
            ClearStateFile();
            int i = 0;
            foreach (StateManagerModel stateModel in listStateModel)
            {
                using (StreamWriter stateWriter = File.AppendText(jsonStateFilePath))
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
            File.WriteAllText(jsonStateFilePath, "[]");
        }

        public void ClearStateFile()
        {
            // Clear the state file
            File.WriteAllText(jsonStateFilePath, "");
        }
    }
}
