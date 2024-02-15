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
            ViewModel.TestSaves();
        }

        public bool Initialize()
        {
            //Display the main menu
            DisplayService.DisplayMenu();
            //User choice
            try
            {
                return ViewModel.UserChoice(Convert.ToInt32(Console.ReadLine()));
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
