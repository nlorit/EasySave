using App.Cmd.Views;
using App.Core.Models;
using App.Core.Services;
using System;
using System.Globalization;
using System.Resources;
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

        //TODO A supprimer ici
        public ResourceManager Resources;
        public CultureInfo cultureInfo = CultureInfo.CurrentCulture;

        public SaveViewModel()
        {
            
            stringService = new StringService();
            saveService = new SaveService();
            openerService = new OpenerService();
            stateManagerService = new StateManagerService();


            //TODO A supprimer ici
            string nomFichierRessources = cultureInfo.Name == "fr-FR" ? "ResourcesFR-FR" : "ResourcesEN-UK";
            Resources = new ResourceManager("App.Cmd." + nomFichierRessources, typeof(SaveViewModel).Assembly);


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

            model.InPath = "C:/Users/Nathan/Desktop/safran2";
            model.OutPath = "C:/Users/Nathan/Desktop/safran3";
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
            string print;
            if (ListSave.Count < 5)
             {
              model = new SaveModel();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
                print = Resources.GetString("Number1");
                Console.Write(print);
                Console.ResetColor();
                print = Resources.GetString("SourceDirectory");
                Console.WriteLine(print);
                Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.InPath = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
                print = Resources.GetString("Number2");
                Console.Write(print);
                Console.ResetColor();
                print = Resources.GetString("TargetFile");
                Console.WriteLine(print);
                Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.OutPath = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
                print = Resources.GetString("Number3");
                Console.Write(print);
                Console.ResetColor();
                print = Resources.GetString("SaveType");
                Console.WriteLine(print);
                Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
                print = Resources.GetString("SaveTypeAnswer1");
                Console.WriteLine(print);
                print = Resources.GetString("SaveTypeAnswer2");
                Console.WriteLine(print);
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
                print = Resources.GetString("Number4");
                Console.Write(print);
                Console.ResetColor();
                print = Resources.GetString("SaveName");
                Console.WriteLine(print);
                Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.SaveName = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
                print = Resources.GetString("Number5");
                Console.Write(print);
                Console.ResetColor();
                print = Resources.GetString("SaveDate");
                Console.WriteLine(print);
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
                print = Resources.GetString("MaxSaveError");
                Console.WriteLine(print);
                Console.ResetColor();
           }
            return true;
        }
        

        public void Run()
        {
            string print;
            Console.WriteLine("");
            Console.WriteLine("+----------------------------------------------------------+");
            print = Resources.GetString("Run");
            Console.WriteLine(print);
            Console.WriteLine("|----------------------------------------------------------|");
            Console.WriteLine("|                                                          |");
            Console.Write("| ");
            Console.ForegroundColor= ConsoleColor.Cyan;
            print = Resources.GetString("List");
            Console.Write(print);
            Console.ResetColor();
            print = Resources.GetString("ListAnswer1");
            Console.WriteLine(print);
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            print = Resources.GetString("Element");
            Console.Write(print);
            Console.ResetColor();
            print = Resources.GetString("ListAnswer2");
            Console.WriteLine(print);
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
            string print;
            foreach (var item in ListSave)
            {
                saveService.ShowInfo(item);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            print = Resources.GetString("EnterExit");
            Console.WriteLine(print);
            Console.ResetColor();
        }

       
    }
}
