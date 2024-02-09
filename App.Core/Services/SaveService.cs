using App.Core.Models;
using System.Resources;


namespace App.Core.Services
{
    public class SaveService
    {
        
        public static void ExecuteCopy(SaveModel saveModel, List<SaveModel> listSavesModel, List<StateManagerModel> listStateManager, ResourceManager resources)
        {   //Method to execute the copy service

            CopyService copyService = new();
            //Execute the copy service
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel, listSavesModel, listStateManager, resources);

        }

        public static void ShowInfo(SaveModel saveModel, ResourceManager resources)
        {
            //Method to show the information of the save
            string? Output;
            Console.WriteLine("");
            Console.WriteLine("+-------------------------------------------------+");

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Output = resources.GetString("Name");
            Console.Write(Output);
            

            Console.ResetColor();
            Console.WriteLine(saveModel.SaveName);

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Output = resources.GetString("In");
            Console.Write(Output);
            Console.ResetColor();
            Console.WriteLine(saveModel.InPath);


            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Output = resources.GetString("Out");
            Console.Write(Output);
            Console.ResetColor();
            Console.WriteLine(saveModel.OutPath);
            
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Type : ");
            Console.ResetColor();

            //Check the type of the save (Complete or Sequentiel)
            if(saveModel.Type == false)
            {
                Output = resources.GetString("TypeAnswer1");
                Console.WriteLine(Output);
            }
            else
            {
                Output = resources.GetString("TypeAnswer2");
                Console.WriteLine(Output);
            }


            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Date : ");
            Console.ResetColor();
            Console.WriteLine(saveModel.Date);
  
            Console.WriteLine("+-------------------------------------------------+");
            

        }
    }
}
