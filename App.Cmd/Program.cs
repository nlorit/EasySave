using App.Cmd.Views;

namespace App.Cmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\r\n  ______                          _____                        \r\n |  ____|                        / ____|                       \r\n | |__      __ _   ___   _   _  | (___     __ _  __   __   ___ \r\n |  __|    / _` | / __| | | | |  \\___ \\   / _` | \\ \\ / /  / _ \\\r\n | |____  | (_| | \\__ \\ | |_| |  ____) | | (_| |  \\ V /  |  __/\r\n |______|  \\__,_| |___/  \\__, | |_____/   \\__,_|   \\_/    \\___|\r\n                          __/ |                                \r\n                         |___/                                 \r\n");
            System.Threading.Thread.Sleep(1500);
            var view = new SaveView();


            while (view.Initialize())
            {
                
            }
            
            
        }
    }
}