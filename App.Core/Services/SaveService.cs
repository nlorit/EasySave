using App.Core.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Resources;

namespace App.Core.Services
{
    public class SaveService
    {
        /// <summary>
        /// Method to execute the copy service
        /// </summary>
        /// <param name="saveModel"></param>
        /// <param name="listSavesModel"></param>
        /// <param name="listStateManager"></param>
        public static void ExecuteCopy(SaveModel saveModel)

        {   //Method to execute the copy service

            CopyService copyService = new();
            //Execute the copy service
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel);


        }

        public ObservableCollection<SaveModel> LoadSave()
        {
            ObservableCollection<SaveModel> ListSaveModel = new ObservableCollection<SaveModel>();

            if (File.Exists("saves.json"))
            {
                string json = File.ReadAllText("saves.json");
                ListSaveModel = JsonConvert.DeserializeObject<ObservableCollection<SaveModel>>(json)!;
            }
            else
            {
                // Create the file if it doesn't exist
                File.WriteAllText("saves.json", "[]");
            }

            return ListSaveModel;
        }

        /// <summary>
        /// Method to show the information of the save
        /// </summary>
        /// <param name="saveModel"></param>
        public static void ShowInfo(SaveModel saveModel)
        {
            //Method to show the information of the save
            Console.WriteLine("");
            Console.WriteLine("+-------------------------------------------------+");

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Name"));
            
            Console.ResetColor();
            Console.WriteLine(saveModel.SaveName);

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("In"));
            Console.ResetColor();
            Console.WriteLine(saveModel.InPath);

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Out"));
            Console.ResetColor();
            Console.WriteLine(saveModel.OutPath);
            
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Type : ");
            Console.ResetColor();

            //Check the type of the save (Complete or Sequentiel)
            if(saveModel.Type == "Complete")
            {
                Console.WriteLine(DisplayService.GetResource("TypeAnswer1"));
            }
            else
            {
                Console.WriteLine(DisplayService.GetResource("TypeAnswer2"));
            }

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Date : ");
            Console.ResetColor();
            Console.WriteLine(saveModel.Date);
  
            Console.WriteLine("+-------------------------------------------------+");
        }

        public void Get()
        {
           
        }
    }
}
