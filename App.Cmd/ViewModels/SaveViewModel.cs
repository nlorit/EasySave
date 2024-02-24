using App.Core.Models;
using App.Core.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;

namespace App.Cmd.ViewModels
{

    public partial class SaveViewModel
    {
        private readonly SaveService? saveService;
        private readonly StateManagerService? stateManagerService;
        private readonly LoggerService? loggerService;
        private readonly CopyService? copyService;
        private readonly DisplayService displayService;

        [GeneratedRegex(@"^\d+;\d+$")]
        private static partial Regex MyRegex();



        private ObservableCollection<StateManagerModel> listState = [];
        private ObservableCollection<SaveModel> ListSaveModel = [];

        public SaveViewModel()
        {
            stateManagerService = new()
            {
                listStateModel = listState
            };
            
            loggerService = new();
            copyService = new(stateManagerService);
            displayService = new();

            saveService = new()
            {
                ListStateManager = listState,
                ListSaveModel = ListSaveModel
            };

            saveService.LoadSave();
            ListSaveModel = saveService.ListSaveModel;
            saveService.ListStateManager = listState;
        }




        public enum TypeOfSave
        {
            Sequential,
            Complete
        }


        /// <summary>
        /// Method to get the user choice
        /// </summary>
        /// <param name="UserEntry"></param>
        /// <returns></returns>
        public bool UserChoice(int UserEntry)
        {
            saveService!.ListSaveModel = ListSaveModel;
            saveService.ListStateManager = listState;

            switch (UserEntry)
            {
                case 1:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("PlanSave"));
                    Thread.Sleep(500);

                    Console.Clear();
                    if (ListSaveModel.Count < 5)
                    {
                        //Create a new save

                        string InPath = "";
                        bool exist = false;
                        while (exist == false)
                        {
                            Console.WriteLine("\n+---------------------------------------------+");
                            Console.Write("| ");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write(DisplayService.GetResource("Number1"));
                            Console.ResetColor();
                            Console.WriteLine(DisplayService.GetResource("SourceDirectory"));
                            Console.WriteLine("+---------------------------------------------+\n");
                            InPath = Console.ReadLine()!;
                            if (!Directory.Exists(InPath))
                            {
                                Console.WriteLine("Invalid Path");
                                exist = false;
                            }
                            else
                            {
                                exist = true;
                            }
                        }

                        string OutPath = "";
                        Console.WriteLine("\n+---------------------------------------------+");
                        Console.Write("| ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(DisplayService.GetResource("Number2"));
                        Console.ResetColor();
                        Console.WriteLine(DisplayService.GetResource("TargetFile"));
                        Console.WriteLine("+---------------------------------------------+\n");

                        //Get the target directory
                        do
                        {
                            try
                            {
                                OutPath = Console.ReadLine()!;
                            }
                            catch (ArgumentNullException)
                            {
                                DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                                System.Threading.Thread.Sleep(1500);
                                continue; // Go back to the start of the loop to prompt for input again
                            }
                        } while (OutPath == null || OutPath.Trim() == "");

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
                        string type = "";
                        switch (int.Parse(Console.ReadLine()!))
                        {
                            case 1:
                                type = TypeOfSave.Complete.ToString();
                                break;
                            case 2:
                                type = TypeOfSave.Sequential.ToString();
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

                        string SaveName = "";
                        //TODO : AJouter exception dans le cas ou l'utilisateur ne rentre pas de nom
                        do
                        {
                            try
                            {
                                SaveName = Console.ReadLine()!;
                            }
                            catch (ArgumentNullException)
                            {
                                DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                                System.Threading.Thread.Sleep(1500);
                                continue; // Go back to the start of the loop to prompt for input again
                            }
                        } while (SaveName == null || SaveName.Trim() == "");



                        Console.WriteLine("");
                        Console.WriteLine("+---------------------------------------------+");
                        Console.Write("| ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(DisplayService.GetResource("Number5WithoutSpace"));
                        Console.ResetColor();
                        Console.Write(DisplayService.GetResource("EncryptChoice"));
                        Console.WriteLine("       |");
                        Console.WriteLine("+---------------------------------------------+");
                        Console.Write(DisplayService.GetResource("Number1WithoutSpace"));
                        Console.WriteLine(DisplayService.GetResource("Yes"));
                        Console.Write(DisplayService.GetResource("Number2WithoutSpace"));
                        Console.WriteLine(DisplayService.GetResource("No"));
                        Console.WriteLine("\n");

                      

                        switch (int.Parse(Console.ReadLine()!))
                        {
                            case 1:
                                saveService.copyService!.isEncrypted = true;
                                break;
                            case 2:
                                saveService.copyService!.isEncrypted = false;
                                break;
                            default:
                                break;
                        }

                        //TODO : Ajouter message de confirmation de save créé avec le try catch ...

                        saveService!.CreateSave( InPath, OutPath, type, SaveName);

                        //stateManagerService!.UpdateStateFile();
                        ListSaveModel = saveService.ListSaveModel;
                        listState = stateManagerService!.listStateModel!;
                        
                        DisplayService.SetForegroundColor("Green", "\nSave Created\n");
                        Thread.Sleep(500);

                    }
                    else
                    {
                        //If the list of saves is full
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(DisplayService.GetResource("MaxSaveError"));
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(2000);
                    }


                    //Return to the main menu
                    DisplayService.SetForegroundColor("Gray", "\n" + DisplayService.GetResource("EnterExit")!);
                    while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                    return true;
                
                
                
                
                case 2:
                    Console.Clear();
                    Console.WriteLine(DisplayService.GetResource("RunSave"));

                    try
                    {

                        int i = 0;
                        foreach (var item in ListSaveModel!)
                        {
                            Console.WriteLine($"{i}- " + saveService!.ShowInfo(item));
                            i++;
                        }
                        Console.WriteLine("\n 1;3 => 1 et 3 \n 1-3 => 1 à 3");

                        string UserChoice = Console.ReadLine()!;

                        if (MyRegex().IsMatch(UserChoice))
                        {
                            if (!saveService!.IsSoftwareRunning())
                            {

                                string[] parts = UserChoice.Split(';');
                                int z1 = int.Parse(parts[0]);
                                int z2 = int.Parse(parts[1]);
                                DisplayService.SetForegroundColor("Green", $"Save {z1} is running");

                                saveService!.ExecuteSave(ListSaveModel[z1]);
                                DisplayService.SetForegroundColor("Green", $"Save {z1} Executed");
                                Thread.Sleep(1000);
                                DisplayService.SetForegroundColor("Green", $"Save {z2} is running");
                                saveService!.ExecuteSave(ListSaveModel[z2]);
                                DisplayService.SetForegroundColor("Green", $"Save {z2} Executed");
                                Thread.Sleep(1000);


                            }
                            else
                            {
                                DisplayService.SetBackForeColor("Black", "DarkRed", "Software is running save cannot be executed...");
                                System.Threading.Thread.Sleep(1500);
                            }

                        }
                        else
                        {
                            //TODO : Ajouter un try catch pour la gestion des entrées de l'utilisateur
                            if (!saveService!.IsSoftwareRunning())
                            {

                                string[] parts = UserChoice.Split('-');
                                int min = int.Parse(parts[0]);
                                int max = int.Parse(parts[1]);
                                for (int x = min; x <= max; x++)
                                {
                                    DisplayService.SetForegroundColor("Green", $"Save {x} is running");
                                    saveService!.ExecuteSave(ListSaveModel[x]);
                                    DisplayService.SetForegroundColor("Green", $"Save {x} Executed");
                                    Thread.Sleep(1000);
                                }

                            }
                            else
                            {
                                DisplayService.SetBackForeColor("Black", "DarkRed", "Software is running save cannot be executed...");
                                System.Threading.Thread.Sleep(1500);
                            }


                        }
                        //return true;
                    }                    
                    catch (ArgumentOutOfRangeException)
                    {
                        //TODO : Quand save est vide
                        DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                        System.Threading.Thread.Sleep(1500);
                        return true;
                    }

                    //TODO : Run Save
                    DisplayService.SetForegroundColor("Gray", "\n" + DisplayService.GetResource("EnterExit")!);
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
            stateManagerService!.OpenStateFile();
        }

        /// <summary>
        /// Method to show the saves schedule
        /// </summary>
        public void ShowSavesSchedule()
        {
            if (File.Exists("saves.json"))
            {
                FileInfo fileInfo = new("saves.json");

            if (fileInfo.Length > 0)
            {
                string json = File.ReadAllText("saves.json");
                ListSaveModel = JsonConvert.DeserializeObject<ObservableCollection<SaveModel>>(json) ?? [];
            }
            else
            {
                // Fichier vide, initialiser la liste sans désérialiser
                ListSaveModel = [];
            }
        }
        else
        {
            // Créer le fichier s'il n'existe pas
            File.WriteAllText("saves.json", "[]");
        }
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("");
        int i = 0;
        foreach (SaveModel item in ListSaveModel)
        {

            Console.WriteLine($"{i} "+saveService!.ShowInfo(item));
            i++;
        }

        Console.WriteLine("\n"+DisplayService.GetResource("EnterExit"));
        Console.ResetColor();


        }
    }
}
