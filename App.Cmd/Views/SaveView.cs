using App.Cmd.ViewModels;
using System.Resources;
using System.Globalization;


namespace App.Cmd.Views
{
    public class SaveView
    { 
        public SaveViewModel ViewModel;

        private readonly ResourceManager Resources;
        private readonly CultureInfo culture = CultureInfo.CurrentCulture;


        public SaveView()
        {
            ViewModel = new SaveViewModel();
            //Create test saves for the first run
            ViewModel.TestSaves();
            //Set the culture
            string ResourceFileName = culture.Name == "fr-FR" ? "ResourcesFR-FR" : "ResourcesEN-UK";
            //Load the resources
            Resources = new ResourceManager("App.Cmd."+ResourceFileName, typeof(SaveView).Assembly);
        }

        public bool Initialize()
        {
            //Display the main menu
            string? Output;
            Console.Clear();
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
            Output = Resources.GetString("Menu1");
            Console.WriteLine(Output);
            Output = Resources.GetString("Menu2");
            Console.WriteLine(Output);
            Output = Resources.GetString("Menu3");
            Console.WriteLine(Output);
            Output = Resources.GetString("Menu4");
            Console.WriteLine(Output);
            Output = Resources.GetString("Menu5");
            Console.WriteLine(Output);
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.DarkRed;   
            Output = Resources.GetString("Menu6");
            Console.Write(Output);
            Console.ResetColor();
            Console.WriteLine("          | ");
            Console.WriteLine("|                                                           |");
            Console.WriteLine("+-----------------------------------------------------------+");
            Console.WriteLine("");

            //User choice
            try {
                int UserEntry = Convert.ToInt32(Console.ReadLine());

                switch (UserEntry)
                {
                    case 1:
                        Console.Clear();
                        Output = Resources.GetString("PlanSave");
                        Console.WriteLine(Output);
                        return ViewModel.CreateSave(Resources);
                    case 2:
                        Console.Clear();
                        Output = Resources.GetString("RunSave");
                        Console.WriteLine(Output);
                        ViewModel.RunSave(Resources);
                        return true;
                    case 3:
                        Console.Clear();
                        Output = Resources.GetString("ShowLogs");
                        Console.WriteLine(Output);
                        ViewModel.ShowLogs();
                        return true;
                    case 4:
                        Console.Clear();
                        Output = Resources.GetString("ShowStateFile");
                        Console.WriteLine(Output);
                        ViewModel.ShowStateFile();
                        return true;
                    case 5:
                        Console.Clear();
                        Output = Resources.GetString("ShowSchedule");
                        Console.WriteLine(Output);
                        ViewModel.ShowSavesSchedule(Resources);
                        while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                        return true;
                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Output = Resources.GetString("Exit");
                        Console.WriteLine(Output);  
                        System.Threading.Thread.Sleep(1000);
                        Console.ResetColor();
                        //Exit the program
                        System.Environment.Exit(0);
                        return false;
                    default:
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Output = Resources.GetString("InvalidChoice");
                        Console.WriteLine(Output);
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(1500);
                        //Return to the main menu
                        return false;
                }
            }    
            catch (System.FormatException)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                Output = Resources.GetString("InvalidChoice");
                Console.WriteLine(Output);
                Console.ResetColor();
                System.Threading.Thread.Sleep(1500);
                return true;
            }
        }


    }
}
