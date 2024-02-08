using App.Core.Models;
using App.Core.Services;
using System;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace App.Cmd.ViewModels
{
    public class SaveViewModel
    {
        private readonly SaveService saveService;
        private readonly StringService stringService;
        private readonly OpenerService openerService;
        private readonly StateManagerService stateManagerService;
        public SaveModel model { get; set; }
        public List<SaveModel> ListSave { get; set; } = [];
        public List<StateManagerModel> StateManagerList { get; set; } = [];
    
        public SaveViewModel()
        {
            
            stringService = new StringService();
            saveService = new SaveService();
            openerService = new OpenerService();
            stateManagerService = new StateManagerService();            
        }

        public void test() //Jeux de test
        {

            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/AnnivNathan";
            model.OutPath = "C:/Users/Nathan/Desktop/AnnivNathan2";
            model.Type = false;
            model.SaveName = "Save1";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            StateManagerList.Add(model.StateManager);
            ListSave.Add(model);
            

            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/safran2";
            model.OutPath = "C:/Users/Nathan/Desktop/safran3";
            model.Type = false;
            model.SaveName = "Save2";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            StateManagerList.Add(model.StateManager);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/safran3/1. Lettre d'engagement.docx";
            model.OutPath = "C:/Users/Nathan/Desktop/safran3/1. Lettre d'engagement 2.docx";
            model.Type = false;
            model.SaveName = "Save3";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");


            StateManagerList.Add(model.StateManager);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Utilisateur/Documents/Projet/IN";
            model.OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT";
            model.Type = false;
            model.SaveName = "Save4";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            StateManagerList.Add(model.StateManager);
            ListSave.Add(model);
        }

        public bool Save()
        {

            if (ListSave.Count < 5)
             {
              model = new SaveModel();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("1- ");
              Console.ResetColor();
              Console.WriteLine("Répertoire d'entrée / Source Directory ? |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.InPath = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("   2- ");
              Console.ResetColor();
              Console.WriteLine("Fichier de sortie / Target file ?     |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.OutPath = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("    3- ");
              Console.ResetColor();
              Console.WriteLine("Type de Sauvegarde / Save type ?     |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              Console.WriteLine("1 - Complète / Complet");
              Console.WriteLine("2 - Séquentielle / sequential");
              Console.WriteLine("");
              int choice = int.Parse(Console.ReadLine());
              switch (choice)
              {
                  case 1:
                      model.Type = false;
                      break;
                  case 2:
                      model.Type = true;
                      break;
                  default:
                      break;
              }

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("   4- ");
              Console.ResetColor();
              Console.WriteLine("Nom de la sauvegarde / Save name ?    |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.SaveName = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("  5- ");
              Console.ResetColor();
              Console.WriteLine("Date de la sauvegarde / Date name ?    |");
              Console.WriteLine("|---------------------------------------------|");
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Format:");
                Console.ResetColor();
                Console.WriteLine("  MM / dd / yyyy HH: mm              |");
                Console.WriteLine("+---------------------------------------------+");
                model.Date = DateTime.Parse(Console.ReadLine());
              //TODO Gérer l'exception du service
              if (string.IsNullOrEmpty(model.InPath) ||
                                 string.IsNullOrEmpty(model.OutPath) ||
                                                string.IsNullOrEmpty(model.SaveName))
                  throw new System.InvalidOperationException();

                StateManagerList.Add(model.StateManager);
                ListSave.Add(model);
                
           }
           else
           {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erreur : Vous avez atteint le nombre maximum de sauvegardes / Error : You have reached the maximum number of saves");
                Console.ResetColor();
           }
            return true;
        }
        

        public void Run()
        {
            Console.WriteLine("");
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("| Que voulez-vous exécuter / What do you want to execute ? |");
            Console.WriteLine("|----------------------------------------------------------|");
            Console.WriteLine("|                                                          |");
            Console.Write("| ");
            Console.ForegroundColor= ConsoleColor.Cyan;
            Console.Write("Une liste / A list:");
            Console.ResetColor();
            Console.WriteLine(" 1-3 => 1 à/to 3                      |");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Certains éléments / Some element:");
            Console.ResetColor();
            Console.WriteLine("  1,3 => 1 et/and 3     |");
            Console.WriteLine("|                                                          |");
            Console.WriteLine("+----------------------------------------------------------+");


            string input = Console.ReadLine();

            try
            {
                bool isCommaSeparatedOrHyphen = stringService.IsCommaSeparatedOrHyphen(input);
                switch (isCommaSeparatedOrHyphen)
                {
                    case true:
                        ProcessCommaSeparatedInput(input);
                        break;

                    case false:
                        ProcessHyphenSeparatedInput(input);
                        break;
                }

            }
            catch (System.IndexOutOfRangeException e)
            {
                saveService.Run(ListSave[int.Parse(input) - 1], ListSave, StateManagerList);
            }
            
        }

        private void ProcessCommaSeparatedInput(string input)
        {
            string[] commaSeparatedParts = input.Split(',');
            int start = int.Parse(commaSeparatedParts[0]);
            int end = int.Parse(commaSeparatedParts[1]);

            saveService.Run(ListSave[start - 1], ListSave, StateManagerList);
            saveService.Run(ListSave[end - 1], ListSave, StateManagerList);
        }

        private void ProcessHyphenSeparatedInput(string input)
        {
            string[] hyphenSeparatedParts = input.Split('-');
            for (int i = int.Parse(hyphenSeparatedParts[0]) - 1; i <= int.Parse(hyphenSeparatedParts[1]) - 1; i++)
            {
                saveService.Run(ListSave[i], ListSave, StateManagerList);
            }
        }

        public void ShowLogs()
        {
            openerService.OpenLogFile();

        }

        public void ShowStateFile()
        {
            openerService.OpenStateFile();
        }

        public void ShowSchedule()
        {
            foreach (var item in ListSave)
            {
                saveService.ShowInfo(item);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n Press ENTER to exit");
            Console.ResetColor();
        }

       
    }
}
