using App.Cmd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Cmd.Views
{
    public class SaveView
    {
        public SaveViewModel ViewModel;

        public SaveView()
        {
            ViewModel = new SaveViewModel();
        }

        public void Initialize()
        {
            
            Console.WriteLine("Veuillez faire un choix / Please me a choise :\n");
            Console.WriteLine("1 - Planifier une savegarde / Plan a save");
            Console.WriteLine("2 - Exécuter une sauvegarde / Run a save");
            Console.WriteLine("3 - Afficher Logs / Show Logs");
            Console.WriteLine("4 - Afficher le fichier d'état / Show state file");
            Console.WriteLine("5 - Afficher la plannification / Show schedule");
            Console.WriteLine("6 - Quitter / Quit");

            try {
                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Planigier une savegarde / Plan a save");
                        ViewModel.Save();
                        break;
                    case 2:
                        Console.WriteLine("Executer une sauvegarde / Run a save");
                        ViewModel.Run();
                        break;
                    case 3:
                        Console.WriteLine("Afficher Logs / Show Logs");
                        ViewModel.ShowLogs();
                        break;
                    case 4:
                        Console.WriteLine("Afficher le fichier d'état / Show state file");
                        ViewModel.ShowStateFile();
                        break;
                    case 5:
                        Console.WriteLine("Afficher la plannification / Show schedule");
                        ViewModel.ShowSchedule();
                        break;
                    case 6:
                        Console.WriteLine("Quitter / Quit");
                        break;
                    default:
                        Console.WriteLine("Choix invalide / Invalid choice");

                        break;
                }
            }
                
            catch (System.FormatException)
            {
                Console.WriteLine("Choix invalide / Invalid choice");
                return;
            }


            

        }
    }
}
