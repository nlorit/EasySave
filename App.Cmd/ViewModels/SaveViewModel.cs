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
        public List<SaveModel> ListSaveModel { get; set; } = [];
        public List<StateManagerModel> StateManagerList { get; set; } = [];

        public SaveViewModel()
        {
            stringService = new();
            saveService = new();
            openerService = new();
            stateManagerService = new();

        }

        public void TestSaves() //Jeux de test
        {
            Model = new SaveModel
            {
                InPath = "C:/Users/Julien/Documents/Autres",
                OutPath = "C:/Users/Julien/Documents/Autres2",
                Type = false,
                SaveName = "Save1",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);


            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/safran2",
                OutPath = "C:/Users/Nathan/Desktop/safran3",
                Type = false,
                SaveName = "Save2",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);

            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/safran2",
                OutPath = "C:/Users/Nathan/Desktop/safran3",
                Type = false,
                SaveName = "Save3",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };


            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);

            Model = new SaveModel
            {
                InPath = "C:/Users/Utilisateur/Documents/Projet/IN",
                OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT",
                Type = false,
                SaveName = "Save4",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);
        }

        public bool CreateSave(ResourceManager resources)
        {
            //Method to create a save
            string? Output;
            if (ListSaveModel.Count < 5)
            {
                //Create a new save
                Model = new();
                Console.WriteLine("");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Output = resources.GetString("Number1");
                Console.Write(Output);
                Console.ResetColor();
                Output = resources.GetString("SourceDirectory");
                Console.WriteLine(Output);
                Console.WriteLine("+---------------------------------------------+");
                Console.WriteLine("");
                //Get the source directory
                Model.InPath = Console.ReadLine()!;


                Console.WriteLine("");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Output = resources.GetString("Number2");
                Console.Write(Output);
                Console.ResetColor();
                Output = resources.GetString("TargetFile");
                Console.WriteLine(Output);
                Console.WriteLine("+---------------------------------------------+");
                Console.WriteLine("");
                //Get the target directory
                Model.OutPath = Console.ReadLine()!;

                Console.WriteLine("");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Output = resources.GetString("Number3");
                Console.Write(Output);
                Console.ResetColor();
                Output = resources.GetString("SaveType");
                Console.WriteLine(Output);
                Console.WriteLine("+---------------------------------------------+");
                Console.WriteLine("");
                Output = resources.GetString("SaveTypeAnswer1");
                Console.WriteLine(Output);
                Output = resources.GetString("SaveTypeAnswer2");
                Console.WriteLine(Output);
                Console.WriteLine("");

                //Get the type of the save
                int UserEntry = int.Parse(Console.ReadLine()!);
                switch (UserEntry)
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
                Output = resources.GetString("Number4");
                Console.Write(Output);
                Console.ResetColor();
                Output = resources.GetString("SaveName");
                Console.WriteLine(Output);
                Console.WriteLine("+---------------------------------------------+");
                Console.WriteLine("");
                //Get the name of the save
                Model.SaveName = Console.ReadLine()!;

                Console.WriteLine("");
                Console.WriteLine("+---------------------------------------------+");
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Output = resources.GetString("Number5");
                Console.Write(Output);
                Console.ResetColor();
                Output = resources.GetString("SaveDate");
                Console.WriteLine(Output);
                Console.WriteLine("|---------------------------------------------|");
                Console.Write("| ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Format:");
                Console.ResetColor();
                Console.WriteLine("  MM/dd/yyyy HH:mm                   |");
                Console.WriteLine("+---------------------------------------------+");
                //Get the date of the save
                Model.Date = DateTime.Parse(Console.ReadLine()!);

                //Check if the save is valid
                if (string.IsNullOrEmpty(Model.InPath) ||
                                   string.IsNullOrEmpty(Model.OutPath) ||
                                                  string.IsNullOrEmpty(Model.SaveName))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Output = resources.GetString("ErrorEmptySave");
                    Console.WriteLine(Output);
                    Console.ResetColor();
                    System.Threading.Thread.Sleep(2000);
                    return true;
                }

                //Create the state of the save
                StateManagerList.Add(Model.StateManager);
                //Add the save to the list
                ListSaveModel.Add(Model);

            }
            else
            {
                //If the list of saves is full
                Console.ForegroundColor = ConsoleColor.Red;
                Output = resources.GetString("MaxSaveError");
                Console.WriteLine(Output);
                Console.ResetColor();
                System.Threading.Thread.Sleep(2000);

            }
            //Return to the main menu
            return true;
        }


        public void RunSave(ResourceManager resources)
        {
            string? Output;
            Console.WriteLine("");
            Console.WriteLine("+----------------------------------------------------------+");
            Output = resources.GetString("Run");
            Console.WriteLine(Output);
            Console.WriteLine("|----------------------------------------------------------|");
            Console.WriteLine("|                                                          |");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Output = resources.GetString("List");
            Console.Write(Output);
            Console.ResetColor();
            Output = resources.GetString("ListAnswer1");
            Console.WriteLine(Output);
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Output = resources.GetString("Element");
            Console.Write(Output);
            Console.ResetColor();
            Output = resources.GetString("ListAnswer2");
            Console.WriteLine(Output);
            Console.WriteLine("|                                                          |");
            Console.WriteLine("+----------------------------------------------------------+");


            string? UserEntry = Console.ReadLine();
            try
            {
                bool isCommaSeparatedOrHyphen = StringService.IsCommaSeparatedOrHyphen(UserEntry!);
                switch (isCommaSeparatedOrHyphen)
                {
                    case true:
                        ProcessCommaSeparatedInput(UserEntry!, resources);
                        break;

                    case false:
                        ProcessHyphenSeparatedInput(UserEntry!, resources);
                        break;
                }

            }
            catch (System.IndexOutOfRangeException)
            {
                SaveService.ExecuteCopy(ListSaveModel[int.Parse(UserEntry!) - 1], ListSaveModel, StateManagerList, resources);
            }

        }

        private void ProcessCommaSeparatedInput(string input, ResourceManager resources)
        {
            //Method to process the input if it is comma separated
            string[] CommaSeparatedParts = input.Split(',');
            int Start = int.Parse(CommaSeparatedParts[0]);
            int End = int.Parse(CommaSeparatedParts[1]);

            //Execute the copy service to First and Last save
            SaveService.ExecuteCopy(ListSaveModel[Start - 1], ListSaveModel, StateManagerList, resources);
            SaveService.ExecuteCopy(ListSaveModel[End - 1], ListSaveModel, StateManagerList, resources);
        }

        private void ProcessHyphenSeparatedInput(string Input, ResourceManager Resources)
        {
            //Method to process the input if it is hyphen separated
            string[] HyphenSeparatedParts = Input.Split('-');
            //Execute the copy service to the range of saves
            for (int i = int.Parse(HyphenSeparatedParts[0]) - 1; i <= int.Parse(HyphenSeparatedParts[1]) - 1; i++)
            {
                SaveService.ExecuteCopy(ListSaveModel[i], ListSaveModel, StateManagerList, Resources);
            }
        }

        public void ShowLogs()
        {
            //Method to show the logs
            openerService.OpenLogFile();

        }

        public void ShowStateFile()
        {
            //Method to show the state file
            openerService.OpenStateFile();
        }

        public void ShowSavesSchedule(ResourceManager Resources)
        {
            string? Output;
            //Method to show the saves schedule
            foreach (SaveModel item in ListSaveModel)
            {
                SaveService.ShowInfo(item, Resources);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("");
            Output = Resources.GetString("EnterExit");
            Console.WriteLine(Output);
            Console.ResetColor();
        }


    }
}
