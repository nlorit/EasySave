using App.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Services
{
    public class SaveService
    {
        public void Create(SaveModel model) 
        { 
            Console.WriteLine("Save created");
        }

        public void Run(SaveModel saveModel) 

        {
            CopyService copyService = new CopyService();
            copyService.RunCopy(new CopyModel { SourcePath = saveModel.InPath, TargetPath = saveModel.OutPath }, saveModel);

            //Console.WriteLine("Save "+ model.SaveName +" running ");
        }

        public void ShowInfo(SaveModel model)
        {
            Console.WriteLine("");
            Console.WriteLine("+-------------------------------------------------+");

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Name : ");
            Console.ResetColor();
            Console.WriteLine(model.SaveName);

            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("IN : ");
            Console.ResetColor();
            Console.WriteLine(model.InPath);


            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("OUT : ");
            Console.ResetColor();
            Console.WriteLine(model.OutPath);
            
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Type : ");
            Console.ResetColor();
            if(model.Type == false)
            {
                Console.WriteLine("Complete");
            }
            else
            {
                Console.WriteLine("Differential");
            }


            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Date : ");
            Console.ResetColor();
            Console.WriteLine(model.Date);
  
            Console.WriteLine("+-------------------------------------------------+");
            
        }

        /*public void TableScreen()
        {
            Console.WriteLine("+----------------------+----------------------+------------+-------------------+-------------------------+");
            Console.WriteLine("|         IN           |         OUT          |    Type    |        Name       |          DateTime       |");
            Console.WriteLine("+----------------------+----------------------+------------+-------------------+-------------------------+");
        }*/
    }
}
