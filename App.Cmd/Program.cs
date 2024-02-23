using App.Cmd.Views;
using App.Core.Services;
using static System.Net.Mime.MediaTypeNames;

namespace App.Cmd
{
    internal class Program
    {
        //Mutex to have app in mono-instance
        static Mutex mutex = new Mutex(true, "{EasySaveMutex}");
        static void Main(string[] args)
        {
            if (!mutex.WaitOne(TimeSpan.Zero, true))
            {
                return;
            }

            mutex.ReleaseMutex();
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