using App.Core.Models;
using App.Core.Services;
using System.Resources;
using System.Text.RegularExpressions;


namespace App.Cmd.ViewModels
{

    public partial class SaveViewModel
    {
        private readonly SaveService saveService;
        private readonly StateManagerService stateManagerService;
        private readonly LoggerService loggerService;
        public SaveModel? Model { get; set; }
        public List<SaveModel> ListSaveModel { get; set; } = [];
        public List<StateManagerModel> StateManagerList { get; set; } = [];
        [GeneratedRegex(@"^\d+,\d+$")]
        private static partial Regex MyRegex();

        public enum TypeOfSave
        {
            Sequential,
            Complete
        }

        /// <summary>
        /// Constructor of the SaveViewModel
        /// </summary>
        public SaveViewModel()
        {
            saveService = new();
            stateManagerService = new();
            loggerService = new();

        }
        /// <summary>
        /// Method to test the saves
        /// </summary>
        public void TestSaves() //Jeux de test
        {
            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/AnnivNathan",
                OutPath = "C:/Users/Nathan/Desktop/AnnivNathan2",
                Type = "Complete",
                SaveName = "Save1",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);


            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/safran2",
                OutPath = "C:/Users/Nathan/Desktop/safran3",
                Type = "Complete",
                SaveName = "Save2",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);

            Model = new SaveModel
            {
                InPath = "C:/Users/Nathan/Desktop/safran2",
                OutPath = "C:/Users/Nathan/Desktop/safran3",
                Type = "Complete",
                SaveName = "Save3",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };


            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);

            Model = new SaveModel
            {
                InPath = "C:/Users/Utilisateur/Documents/Projet/IN",
                OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT",
                Type = "Complete",
                SaveName = "Save4",
                Date = DateTime.Parse("02/05/2024 10:00:00")
            };

            StateManagerList.Add(Model.StateManager);
            ListSaveModel.Add(Model);
        }


        /// <summary>
        /// Method to get the user choice
        /// </summary>
        /// <param name="UserEntry"></param>
        /// <returns></returns>
        public bool UserChoice(int UserEntry)
        {
            switch (UserEntry)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("PlanSave"));
                    CreateSave();
                    while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                    return true;
                case 2:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("RunSave"));
                    RunSave();
                    while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                    return true;
                case 3:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("ShowLogs"));
                    ShowLogs();
                    return true;
                case 4:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("ShowStateFile"));
                    ShowStateFile();
                    return true;
                case 5:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("ShowSchedule"));
                    ShowSavesSchedule();
                    while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                    return true;
                case 6:
                    Console.Clear();
                    DisplayService.SetForegroundColor("DarkRed", DisplayService.GetResource("Exit")!);
                    System.Threading.Thread.Sleep(1000);
                    System.Environment.Exit(0);
                    return false;
                default:
                    DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                    System.Threading.Thread.Sleep(1500);
                    //Return to the main menu
                    return false;
            }
        }
        /// <summary>
        ///  Method to create a save
        /// </summary>
        /// <returns></returns>
        public void CreateSave()
        {
            //Method to create a save


            //Create a new save
            Model = new();
            Console.WriteLine("\n+---------------------------------------------+");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Number1"));
            Console.ResetColor();
            Console.WriteLine(DisplayService.GetResource("SourceDirectory"));
            Console.WriteLine("+---------------------------------------------+\n");
            //Get the source directory
                
            do
            {
                try
                {
                    Model.InPath = Console.ReadLine()!;
                }
                catch (ArgumentNullException)
                {
                    DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                    System.Threading.Thread.Sleep(1500);
                    continue; // Go back to the start of the loop to prompt for input again
                }
            } while (Model.InPath == null || Model.InPath.Trim() == "");




            Console.WriteLine("\n+---------------------------------------------+");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Number2"));
            Console.ResetColor();
            Console.WriteLine(DisplayService.GetResource("TargetFile"));
            Console.WriteLine("+---------------------------------------------+\n");

            //Get the target directory
            //TODO : Ajouter une exception dans le cas ou l'utilisateur ne rentre pas de valeur

            do
            {
                try
                {
                    Model.OutPath = Console.ReadLine()!;
                }
                catch (ArgumentNullException)
                {
                    DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                    System.Threading.Thread.Sleep(1500);
                    continue; // Go back to the start of the loop to prompt for input again
                }
            } while (Model.OutPath == null || Model.OutPath.Trim() == "");

            Console.WriteLine("\n+---------------------------------------------+");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Number3"));
            Console.ResetColor();


            Console.WriteLine(DisplayService.GetResource("SaveType"));
            Console.WriteLine("+---------------------------------------------+\n");
            Console.WriteLine(DisplayService.GetResource("SaveTypeAnswer1"));
            Console.WriteLine(DisplayService.GetResource("SaveTypeAnswer2"));
            Console.WriteLine("\n");

            //Get the type of the save
            int UserEntry = int.Parse(Console.ReadLine()!);
                
            switch (UserEntry)
            {
                case 1:
                    Model.Type = TypeOfSave.Complete.ToString();
                    break;
                case 2:
                    Model.Type = TypeOfSave.Sequential.ToString();
                    break;
                default:
                    break;
            }
                    
            Console.WriteLine("");
            Console.WriteLine("+---------------------------------------------+");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Number4"));
            Console.ResetColor();
            Console.WriteLine(DisplayService.GetResource("SaveName"));
            Console.WriteLine("+---------------------------------------------+");
            Console.WriteLine("");

            //Get the name of the save
            //TODO : AJouter exception dans le cas ou l'utilisateur ne rentre pas de nom
            do
            {
                try
                {
                    Model.SaveName = Console.ReadLine()!;
                }
                catch (ArgumentNullException)
                {
                    DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                    System.Threading.Thread.Sleep(1500);
                    continue; // Go back to the start of the loop to prompt for input again
                }
            } while (Model.SaveName == null || Model.SaveName.Trim() == "");


            Console.WriteLine("\n+---------------------------------------------+");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Number5"));
            Console.ResetColor();
            Console.WriteLine(DisplayService.GetResource("SaveDate"));
            Console.WriteLine("|---------------------------------------------|");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Format:");
            Console.ResetColor();
            Console.WriteLine("  MM/dd/yyyy HH:mm                   |");
            Console.WriteLine("+---------------------------------------------+");
            //Get the date of the save
            //TODO : Ajouter une exception dans le cas ou l'utilisateur ne rentre pas de date
            do
            {
                try
                {
                        
                    Model.Date = DateTime.Parse(Console.ReadLine()!);
                    break; // Break out of the loop if parsing is successful
                }
                catch (FormatException)
                {
                    // Handle invalid date format
                    DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("\n+---------------------------------------------+");
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(DisplayService.GetResource("Number5"));
                    Console.ResetColor();
                    Console.WriteLine(DisplayService.GetResource("SaveDate"));
                    Console.WriteLine("|---------------------------------------------|");
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Format:");
                    Console.ResetColor();
                    Console.WriteLine("  MM/dd/yyyy HH:mm                   |");
                    Console.WriteLine("+---------------------------------------------+");

                }
                catch (ArgumentNullException)
                {
                    // Handle null input
                    DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                    Thread.Sleep(1000);
                    Console.Clear();
                    Console.WriteLine("\n+---------------------------------------------+");
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(DisplayService.GetResource("Number5"));
                    Console.ResetColor();
                    Console.WriteLine(DisplayService.GetResource("SaveDate"));
                    Console.WriteLine("|---------------------------------------------|");
                    Console.Write("| ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Format:");
                    Console.ResetColor();
                    Console.WriteLine("  MM/dd/yyyy HH:mm                   |");
                    Console.WriteLine("+---------------------------------------------+");
                }
            } while (true); // Loop indefinitely until a valid date is provided

            //Create the state of the save
            StateManagerList.Add(Model.StateManager);
            //Add the save to the list
            ListSaveModel.Add(Model);


            DisplayService.SetForegroundColor("Gray", "\n" + DisplayService.GetResource("EnterExit")!);
            //Return to the main menu
        }

        /// <summary>
        /// Method to run a save
        /// </summary>
        public void RunSave()
        {
            Console.WriteLine("\n+----------------------------------------------------------+");
            Console.WriteLine(DisplayService.GetResource("Run"));
            Console.WriteLine("|----------------------------------------------------------|");
            Console.WriteLine("|                                                          |");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("List"));
            Console.ResetColor();
            Console.WriteLine(DisplayService.GetResource("ListAnswer1"));
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DisplayService.GetResource("Element"));
            Console.ResetColor();
            Console.WriteLine(DisplayService.GetResource("ListAnswer2"));
            Console.WriteLine("|                                                          |");
            Console.WriteLine("+----------------------------------------------------------+\n");

            //TODO : Ajouter une exception dans le cas ou l'utilisateur ne rentre pas de valeur
            string? UserEntry = Console.ReadLine();
            try
            {
                //TODO : Revoir ça 
                bool isCommaSeparatedOrHyphen = MyRegex().IsMatch(UserEntry!);
                switch (isCommaSeparatedOrHyphen)
                {
                    case true:
                        ProcessCommaSeparatedInput(UserEntry!);
                        break;

                    case false:
                        ProcessHyphenSeparatedInput(UserEntry!);
                        break;
                }

            }
            catch (System.IndexOutOfRangeException)
            {
                SaveService.ExecuteCopy(ListSaveModel[int.Parse(UserEntry!) - 1], ListSaveModel, StateManagerList);
            }
            DisplayService.SetForegroundColor("Gray", "\n" + DisplayService.GetResource("EnterExit")!);

        }
        /// <summary>
        /// Method to process the input if it is comma separated
        /// </summary>
        /// <param name="input"></param>
        private void ProcessCommaSeparatedInput(string input)
        {
            //Method to process the input if it is comma separated
            string[] CommaSeparatedParts = input.Split(',');
            int Start = int.Parse(CommaSeparatedParts[0]);
            int End = int.Parse(CommaSeparatedParts[1]);

            //Execute the copy service to First and Last save
            SaveService.ExecuteCopy(ListSaveModel[Start - 1], ListSaveModel, StateManagerList);
            SaveService.ExecuteCopy(ListSaveModel[End - 1], ListSaveModel, StateManagerList);
        }

        /// <summary>
        /// Method to process the input if it is hyphen separated
        /// </summary>
        /// <param name="Input"></param>
        private void ProcessHyphenSeparatedInput(string Input)
        {
            //Method to process the input if it is hyphen separated
            string[] HyphenSeparatedParts = Input.Split('-');
            //Execute the copy service to the range of saves
            for (int i = int.Parse(HyphenSeparatedParts[0]) - 1; i <= int.Parse(HyphenSeparatedParts[1]) - 1; i++)
            {
                SaveService.ExecuteCopy(ListSaveModel[i], ListSaveModel, StateManagerList);
            }
        }

        /// <summary>
        /// Method to show the logs
        /// </summary>
        public void ShowLogs()
        {
            //Method to show the logs
            loggerService.OpenLogFile();

        }

        /// <summary>
        /// Method to show the state file
        /// </summary>
        public void ShowStateFile()
        {
            //Method to show the state file
            stateManagerService.OpenStateFile();
        }

        /// <summary>
        /// Method to show the saves schedule
        /// </summary>
        public void ShowSavesSchedule()
        {
            //Method to show the saves schedule
            foreach (SaveModel item in ListSaveModel)
            {
                SaveService.ShowInfo(item);
            }
            DisplayService.SetForegroundColor("Gray", "\n"+DisplayService.GetResource("EnterExit")!);
        }

        
    }
}
