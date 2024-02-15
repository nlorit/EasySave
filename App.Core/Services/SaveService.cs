using App.Core.Models;
using System.Resources;


namespace App.Core.Services
{
    public class SaveService
    {

        public static void ExecuteCopy(SaveModel saveModel, List<SaveModel> listSavesModel, List<StateManagerModel> listStateManager)
        {   //Method to execute the copy service

            CopyService copyService = new();
            //Execute the copy service
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel, listSavesModel, listStateManager);

        }

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
    }
}
