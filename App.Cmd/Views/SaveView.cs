using App.Cmd.ViewModels;
using System.Resources;
using System.Globalization;


namespace App.Cmd.Views
{
    public class SaveView
    { 
        public SaveViewModel ViewModel;

        public ResourceManager Resources;
        public CultureInfo cultureInfo = CultureInfo.CurrentCulture;


        public SaveView()
        {
            ViewModel = new SaveViewModel();
            ViewModel.Test();
            string nomFichierRessources = cultureInfo.Name == "fr-FR" ? "ResourcesFR-FR" : "ResourcesEN-UK";
            Resources = new ResourceManager("App.Cmd."+nomFichierRessources, typeof(SaveView).Assembly);
        }

        public bool Initialize()
        {
            string? print;
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
            print = Resources.GetString("Menu1");
            Console.WriteLine(print);
            print = Resources.GetString("Menu2");
            Console.WriteLine(print);
            print = Resources.GetString("Menu3");
            Console.WriteLine(print);
            print = Resources.GetString("Menu4");
            Console.WriteLine(print);
            print = Resources.GetString("Menu5");
            Console.WriteLine(print);
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.DarkRed;   
            print = Resources.GetString("Menu6");
            Console.Write(print);
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
                        Console.Clear();
                        print = Resources.GetString("PlanSave");
                        Console.WriteLine(print);
                        return ViewModel.Save(Resources);
                    case 2:
                        Console.Clear();
                        print = Resources.GetString("RunSave");
                        Console.WriteLine(print);
                        ViewModel.Run(Resources);
                        return true;
                    case 3:
                        Console.Clear();
                        print = Resources.GetString("ShowLogs");
                        Console.WriteLine(print);
                        ViewModel.ShowLogs();
                        return true;
                    case 4:
                        Console.Clear();
                        print = Resources.GetString("ShowStateFile");
                        Console.WriteLine(print);
                        ViewModel.ShowStateFile();
                        return true;
                    case 5:
                        Console.Clear();
                        print = Resources.GetString("ShowSchedule");
                        Console.WriteLine(print);
                        ViewModel.ShowSchedule(Resources);
                        while (Console.ReadKey().Key != ConsoleKey.Enter) ;
                        return true;
                    case 6:
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        print = Resources.GetString("Exit");
                        Console.WriteLine(print);  
                        System.Threading.Thread.Sleep(1000);
                        Console.ResetColor();
                        System.Environment.Exit(0);
                        return false;
                    default:
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        print = Resources.GetString("InvalidChoice");
                        Console.WriteLine(print);
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(1500);
                        return false;
                }
            }    
            catch (System.FormatException)
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Black;
                print = Resources.GetString("InvalidChoice");
                Console.WriteLine(print);
                Console.ResetColor();
                System.Threading.Thread.Sleep(1500);
                return true;
            }
        }


    }
}
