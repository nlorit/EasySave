using App.Core.Models;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class StateManagerService
    {
        private readonly string stateFilePath = "state.json";

        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void UpdateState( List<StateManagerModel> states, SaveModel saveModel, List<SaveModel> saves)
        {
            FileStream fileStream = File.Open(stateFilePath, FileMode.Open);
            fileStream.SetLength(0);
            fileStream.Close();
           
            try
            {
                int i = 0;
                foreach (StateManagerModel model in states)
                {
                    model.SaveName = saves[i].SaveName;
                    string stateEntry = JsonSerializer.Serialize(model, options)+",";
                    using (StreamWriter stateWriter = File.AppendText(stateFilePath))
                    {
                        stateWriter.WriteLineAsync(stateEntry);
                    }
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
