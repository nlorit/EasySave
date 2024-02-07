using App.Cmd.Views;

namespace App.Cmd
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("\r\n  ______                          _____                        \r\n |  ____|                        / ____|                       \r\n | |__      __ _   ___   _   _  | (___     __ _  __   __   ___ \r\n |  __|    / _` | / __| | | | |  \\___ \\   / _` | \\ \\ / /  / _ \\\r\n | |____  | (_| | \\__ \\ | |_| |  ____) | | (_| |  \\ V /  |  __/\r\n |______|  \\__,_| |___/  \\__, | |_____/   \\__,_|   \\_/    \\___|\r\n                          __/ |                                \r\n                         |___/                                 \r\n");
            System.Threading.Thread.Sleep(1500);
            var view = new SaveView();


            Task<bool> initializationTask = Task.Run(() => view.Initialize());

            // Wait for initialization to complete
            bool initialized = await initializationTask;

            // Continue updating if initialized successfully
            while (initialized)
            {
                await view.Initialize(); // Initialize the view asynchronously
                await view.UpdateAsync(); // Update the view asynchronously
                await Task.Delay(500); // Delay for 500 milliseconds
            }


        }
    }
}