using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class SaveService
    {

        //public void Create(SaveModel model) 
        //{ 
        //    Console.WriteLine("Save created");
        //}


        //TODO A supprimer ici
        public ResourceManager Resources;
        public CultureInfo cultureInfo = CultureInfo.CurrentCulture;

        public SaveService()
        {
            //TODO A supprimer ici
            string nomFichierRessources = cultureInfo.Name == "fr-FR" ? "ResourcesFR-FR" : "ResourcesEN-UK";
            Resources = new ResourceManager("Resources." + nomFichierRessources, typeof(SaveService).Assembly);
        }
        public void Run(SaveModel saveModel, List<SaveModel> saves, List<StateManagerModel> list)
    
        {
            CopyService copyService = new CopyService();
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel, saves, list);

        }

        public void ShowInfo(SaveModel model)
        {
            string print;
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
