using App.Core.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace App.Core.Services
{
    public class StateManagerService
    {

        public ObservableCollection<StateManagerModel>? listStateModel;
        public static readonly string stateFilePath = "state.json";


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

        public void UpdateStateFile(StreamWriter streamWriter)
        {
            try
            {
                // Open the file for writing (append mode)
                using (streamWriter = File.AppendText(stateFilePath))
                {
                    foreach (StateManagerModel stateModel in listStateModel!)
                    {
                        // Write the serialized stateModel to the file
                        streamWriter.WriteLineAsync(JsonSerializer.Serialize(stateModel, options) + ",");
                    }
                }
            }
            catch (IOException ex)
            {
                // Handle the exception (e.g., log or retry)
                Console.WriteLine($"Error updating state file: {ex.Message}");
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
            File.WriteAllText(stateFilePath, "") ;
        }


    }
}
