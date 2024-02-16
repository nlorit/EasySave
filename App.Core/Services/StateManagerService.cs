using App.Core.Models;
using System.Text.Json;

namespace App.Core.Services
{
    public class StateManagerService
    {
        private readonly string stateFilePath = "state.json";
        //Options for the JsonSerializer
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Method to write the state to the state file
        /// </summary>
        /// <param name="listStateModel"></param>
        /// <param name="saveModel"></param>
        /// <param name="listSavesModel"></param>
        public void UpdateState(List<StateManagerModel> listStateModel, SaveModel saveModel)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNull(listStateModel);
            ArgumentNullException.ThrowIfNull(saveModel);

            //Clean the file
            FileStream fileStream = File.Open(stateFilePath, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Dispose();
            
           
            //Write the new state
            try
            {
                int i = 0;
                foreach (StateManagerModel stateModel in listStateModel)
                {
                    //Set the state model properties
                    //stateModel.SaveName = listSavesModel[i].SaveName;
                    //Serialize the state model to JSON
                    string stateEntry = JsonSerializer.Serialize(stateModel, options)+",";
                    using (StreamWriter stateWriter = File.AppendText(stateFilePath))
                    {
                        stateWriter.WriteLineAsync(stateEntry);
                    }

                    //Increment the index to get the next save name
                    i += 1;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
        /// <summary>
        /// Method to open the state file
        /// </summary>
        public void OpenStateFile()
        {
            // Open the state file in notepad
            System.Diagnostics.Process.Start("notepad.exe", stateFilePath);
        }
    }
}
