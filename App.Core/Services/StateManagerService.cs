using App.Core.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace App.Core.Services
{
    public class StateManagerService
    {
        
        public ObservableCollection<StateManagerModel>? listStateModel;
        public static readonly string stateFilePath = "state.json";

        private static Mutex mut = new();


        public StateManagerService()
        {   
            //Create the state file if it does not exist
            if (!File.Exists(stateFilePath))
            {
                //CreateStateFile();
            }
            //UpdateStateFile();

        }

        public void OpenStateFile()
        {
            // Open the state file in notepad
            System.Diagnostics.Process.Start("notepad.exe", stateFilePath);
        }


        public void UpdateStateFile(ObservableCollection<StateManagerModel> stateManagerModels)
        {
            //TODO : Voir les states files
            mut.WaitOne();
            try
            {
                ClearStateFile();
                // Open the file for writing (append mode)
                StreamWriter? streamWriter = null;
                using (streamWriter = File.AppendText(stateFilePath))
                {
                    foreach (StateManagerModel stateModel in stateManagerModels!)
                    {
                        // Write the serialized stateModel to the file
                        streamWriter.WriteLineAsync(JsonConvert.SerializeObject(stateModel, Formatting.Indented) + Environment.NewLine);
                    }
                }
                streamWriter?.Dispose();
            }
            catch (IOException ex)
            {
                // Handle the exception (e.g., log or retry)
                Console.WriteLine($"Error updating state file: {ex.Message}");
            }
            mut.ReleaseMutex();
        }


        public void ClearStateFile()
        {
            // Clear the state file
            File.WriteAllText(stateFilePath, "") ;
        }



    }
}
