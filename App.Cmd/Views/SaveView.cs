using App.Cmd.ViewModels;
using System.Resources;
using System.Globalization;
using App.Core.Services;

namespace App.Cmd.Views
{
    public class SaveView
    { 
        public SaveViewModel ViewModel;


        public SaveView()
        {
            ViewModel = new SaveViewModel();
            //Create test saves for the first run
            //ViewModel.TestSaves();
        }

        /// <summary>
        /// Method to initialize the view
        /// </summary>
        /// <returns></returns>
        public bool Initialize()
        {
            //Display the main menu
            DisplayService.DisplayMenu();
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
                DisplayService.SetBackForeColor("Black", "DarkRed", DisplayService.GetResource("InvalidChoice")!);
                System.Threading.Thread.Sleep(1500);
                return true;
            }
        }
    }
}
