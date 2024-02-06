using App.Cmd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace App.Cmd.Views
{
    public class SaveView
    {
        public SaveViewModel ViewModel;

        public SaveView()
        {
            ViewModel = new SaveViewModel();
            ViewModel.add();
        }

        public bool Initialize()
        {
            Console.WriteLine("");
            Console.WriteLine("+-----------------------------------------------------------+");
            Console.WriteLine("|                                                           |");
            Console.Write("|                          ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("EasySave");
            Console.ResetColor();
            Console.WriteLine("                         |");
            Console.ResetColor(); 
            Console.WriteLine("|                                                           |");
            Console.WriteLine("| Menu :                                                    |");
            Console.WriteLine("|                                                           |");
            Console.ResetColor();
            Console.WriteLine("| 1 - Planifier une savegarde / Plan a save.......          |");
            Console.WriteLine("| 2 - Exécuter une sauvegarde / Run a save........          |");
            Console.WriteLine("| 3 - Afficher Logs / Show Logs...................          |");
            Console.WriteLine("| 4 - Afficher le fichier d'état / Show state file          |");
            Console.WriteLine("| 5 - Afficher la plannification / Show schedule..          |");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Red;   
            Console.Write("6 - Quitter / Quit..............................");
            Console.ResetColor();
            Console.WriteLine("          | ");
            Console.WriteLine("|                                                           |");
            Console.WriteLine("+-----------------------------------------------------------+");
            Console.WriteLine("");


            try {
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Planifier une sauvegarde / Plan a save");
                        ViewModel.Save();
                        return true;
                    case 2:
                        Console.WriteLine("Executer une sauvegarde / Run a save");
                        ViewModel.Run();
                        return true;
                    case 3:
                        Console.WriteLine("Afficher Logs / Show Logs");
                        ViewModel.ShowLogs();
                        return true;
                    case 4:
                        Console.WriteLine("Afficher le fichier d'état / Show state file");
                        ViewModel.ShowStateFile();
                        return true;
                    case 5:
                        Console.WriteLine("Afficher la plannification / Show schedule");
                        ViewModel.ShowSchedule();
                        return true;
                    case 6:
                        Console.WriteLine("Quitter / Quit");
                        Console.WriteLine("Exiting EasySave ...");
                        return false;
                    default:
                        Console.WriteLine("Choix invalide / Invalid choice");
                        return true;
                }
            }    
            catch (System.FormatException)
            {
                Console.WriteLine("Choix invalide / Invalid choice");
                return true;
            }

            


            

        }
    }
}


