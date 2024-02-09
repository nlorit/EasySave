using App.Core.Models;
using App.Core.Services;
using System.Resources;


namespace App.Cmd.ViewModels
{
    public class SaveViewModel
    {
        private readonly SaveService saveService;
        private readonly StringService stringService;
        private readonly OpenerService openerService;
        private readonly StateManagerService stateManagerService;
        public SaveModel? Model { get; set; }
        public List<SaveModel> ListSave { get; set; } = [];
        public List<StateManagerModel> StateManagerList { get; set; } = [];

        public SaveViewModel()
        {

            stringService = new();
            saveService = new();
            openerService = new();
            stateManagerService = new();

        }

        public void Test() //Jeux de test
        {

            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/AnnivNathan",
                OutPath = "C:/Users/Nathan/Desktop/AnnivNathan2",
                Type = false,
                SaveName = "Save1",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSave.Add(Model);


            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/safran2",
                OutPath = "C:/Users/Nathan/Desktop/safran3",
                Type = false,
                SaveName = "Save2",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSave.Add(Model);

            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/safran2",
                OutPath = "C:/Users/Nathan/Desktop/safran3",
                Type = false,
                SaveName = "Save3",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };


            StateManagerList.Add(Model.StateManager);
            ListSave.Add(Model);

            Model = new SaveModel
            {
                InPath = "C:/Users/Utilisateur/Documents/Projet/IN",
                OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT",
                Type = false,
                SaveName = "Save4",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSave.Add(Model);
        }

        public bool Save(ResourceManager Resources)
        {
            string? print;
            if (ListSave.Count < 5)
            {
                Model = new SaveModel();

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
                Model.InPath = Console.ReadLine()!;


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
                Model.OutPath = Console.ReadLine()!;

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
                //TODO : A changer le bool 
                int choice = int.Parse(Console.ReadLine()!);
                switch (choice)
                {
                    case 1:
                        Model.Type = false;
                        break;
                    case 2:
                        Model.Type = true;
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
                Model.SaveName = Console.ReadLine()!;

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
                    
                Model.Date = DateTime.Parse(Console.ReadLine()!);

                if (string.IsNullOrEmpty(Model.InPath) ||
                                   string.IsNullOrEmpty(Model.OutPath) ||
                                                  string.IsNullOrEmpty(Model.SaveName))
                    throw new System.InvalidOperationException();

                StateManagerList.Add(Model.StateManager);
                ListSave.Add(Model);

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


        public void Run(ResourceManager Resources)
        {
            string? print;
            Console.WriteLine("");
            Console.WriteLine("+----------------------------------------------------------+");
            print = Resources.GetString("Run");
            Console.WriteLine(print);
            Console.WriteLine("|----------------------------------------------------------|");
            Console.WriteLine("|                                                          |");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
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


            string? input = Console.ReadLine();

            try
            {
                bool isCommaSeparatedOrHyphen = StringService.IsCommaSeparatedOrHyphen(input!);
                switch (isCommaSeparatedOrHyphen)
                {
                    case true:
                        ProcessCommaSeparatedInput(input!, Resources);
                        break;

                    case false:
                        ProcessHyphenSeparatedInput(input!, Resources);
                        break;
                }

            }
            catch (System.IndexOutOfRangeException)
            {
                SaveService.Run(ListSave[int.Parse(input!) - 1], ListSave, StateManagerList, Resources);
            }

        }

        private void ProcessCommaSeparatedInput(string input, ResourceManager Resources)
        {
            string[] commaSeparatedParts = input.Split(',');
            int start = int.Parse(commaSeparatedParts[0]);
            int end = int.Parse(commaSeparatedParts[1]);

            SaveService.Run(ListSave[start - 1], ListSave, StateManagerList, Resources);
            SaveService.Run(ListSave[end - 1], ListSave, StateManagerList, Resources);
        }

        private void ProcessHyphenSeparatedInput(string input, ResourceManager Resources)
        {
            string[] hyphenSeparatedParts = input.Split('-');
            for (int i = int.Parse(hyphenSeparatedParts[0]) - 1; i <= int.Parse(hyphenSeparatedParts[1]) - 1; i++)
            {
                SaveService.Run(ListSave[i], ListSave, StateManagerList, Resources);
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

        public void ShowSchedule(ResourceManager Resources)
        {
            string? print;
            foreach (var item in ListSave)
            {
                SaveService.ShowInfo(item, Resources);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            print = Resources.GetString("EnterExit");
            Console.WriteLine(print);
            Console.ResetColor();
        }


    }
}
