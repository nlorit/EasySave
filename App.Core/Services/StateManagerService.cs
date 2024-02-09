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

        public void UpdateState(List<StateManagerModel> listStateModel, SaveModel saveModel, List<SaveModel> listSavesModel)
        {
            //Check for nulls
            ArgumentNullException.ThrowIfNull(listStateModel);
            ArgumentNullException.ThrowIfNull(saveModel);
            ArgumentNullException.ThrowIfNull(listSavesModel);

            //Clean the file
            FileStream fileStream = File.Open(stateFilePath, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();
           
            //Write the new state
            try
            {
                int i = 0;
                foreach (StateManagerModel stateModel in listStateModel)
                {
                    //Set the state model properties
                    stateModel.SaveName = listSavesModel[i].SaveName;
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
    }
}
