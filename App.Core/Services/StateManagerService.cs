using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class StateManagerService
    {
        private readonly String stateFilePath = "state.json";

        public async void CreateStateFile(List<SaveModel> saves)
        {
            StateManagerModel model = new StateManagerModel();
            
            File.Delete(stateFilePath);
            foreach (SaveModel save in saves)
            {
                model.SaveName = save.SaveName;
                model.State = "END";
                string stateEntry = JsonSerializer.Serialize(model) + ",";
                using (StreamWriter stateWriter = File.AppendText(stateFilePath))
                {
                    await stateWriter.WriteLineAsync(stateEntry);
                }
            }

        }

        public async void UpdateState(StateManagerModel stateModel,  SaveModel model)
        {
            try
            {
                stateModel.SaveName = model.SaveName;
                string stateEntry = JsonSerializer.Serialize(stateModel) + ",";
                File.Delete(stateFilePath);
                using (StreamWriter stateWriter = File.AppendText(stateFilePath))
                {
                    await stateWriter.WriteLineAsync(stateEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to state file: {ex.Message}");
            }
        }
    }
}
