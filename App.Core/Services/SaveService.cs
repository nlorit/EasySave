using App.Core.Models;
using System.Resources;


namespace App.Core.Services
{
    public class SaveService
    {

        public static void Run(SaveModel saveModel, List<SaveModel> saves, List<StateManagerModel> list, ResourceManager Resources)
    
        {
            CopyService copyService = new();
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel, saves, list, Resources);

        }

        public static void ShowInfo(SaveModel model, ResourceManager Resources)
        {
            string? print;
            Console.WriteLine("");
            Console.WriteLine("+-------------------------------------------------+");

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            print = Resources.GetString("Name");
            Console.Write(print);
            

            Console.ResetColor();
            Console.WriteLine(model.SaveName);

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            print = Resources.GetString("In");
            Console.Write(print);
            Console.ResetColor();
            Console.WriteLine(model.InPath);


            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            print = Resources.GetString("Out");
            Console.Write(print);
            Console.ResetColor();
            Console.WriteLine(model.OutPath);
            
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Type : ");
            Console.ResetColor();
            if(model.Type == false)
            {
                print = Resources.GetString("TypeAnswer1");
                Console.WriteLine(print);
            }
            else
            {
                print = Resources.GetString("TypeAnswer2");
                Console.WriteLine(print);
            }


            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Date : ");
            Console.ResetColor();
            Console.WriteLine(model.Date);
  
            Console.WriteLine("+-------------------------------------------------+");
            

        }
    }
}
