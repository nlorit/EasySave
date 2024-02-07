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

        
        public async Task CreateStateFileAsync(List<SaveModel> saves)
        {
                        
            try
            {
                File.Delete(stateFilePath);
                foreach (SaveModel save in saves)
                {
                    StateManagerModel model = new StateManagerModel
                    {
                        SaveName = save.SaveName,
                        State = "END",
                    };
                    string stateEntry = JsonSerializer.Serialize(model) + ",";
                    using (StreamWriter stateWriter = File.AppendText(stateFilePath))
                    {
                        await stateWriter.WriteLineAsync(stateEntry);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        
        public async Task UpdateStateAsync(StateManagerModel stateModel, SaveModel model, List<SaveModel> saves)

        {
            String stateEntry = "";
            foreach (SaveModel save in saves)
            {
                
                if (save == model)
                {
                    try
                    {
                        stateModel.SaveName = model.SaveName;
                        stateEntry = JsonSerializer.Serialize(stateModel) + ",";
                        
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    StateManagerModel Statemodel = new StateManagerModel
                    {
                        SaveName = save.SaveName,
                        State = "END",
                    };
                    
                    stateEntry = JsonSerializer.Serialize(stateModel) + ",";
                }
                
            }
            File.Delete(stateFilePath);
            using (StreamWriter stateWriter = File.AppendText(stateFilePath))
            {
                await stateWriter.WriteLineAsync(stateEntry);
            }

        }
    }
}
