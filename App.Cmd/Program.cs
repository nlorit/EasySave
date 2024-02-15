using App.Cmd.Views;
using App.Core.Services;

namespace App.Cmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DisplayService displayService = new();
            // Check if the arguments are null
            ArgumentNullException.ThrowIfNull(args);

            // Display the welcome message
            DisplayService.DisplayWelcomeMessage();

            DisplayService.DisplayProgressBar();

            SaveView View = new();
            // Initialize the view
            while (View.Initialize());
        }
    }
}