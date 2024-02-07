using App.Cmd.Views;

namespace App.Cmd
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("\n\n\n\n\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" \r\n                                 ______                          _____                        " +
                              " \r\n                                |  ____|                        / ____|                       " +
                              " \r\n                                | |__      __ _   ___   _   _  | (___     __ _  __   __   ___ " +
                              " \r\n                                |  __|    / _` | / __| | | | |  \\___ \\   / _` | \\ \\ / /  / _ \\" +
                              " \r\n                                | |____  | (_| | \\__ \\ | |_| |  ____) | | (_| |  \\ V /  |  __/" +
                              " \r\n                                |______|  \\__,_| |___/  \\__, | |_____/   \\__,_|   \\_/    \\___|" +
                              " \r\n                                                         __/ |                                " +
                              " \r\n                                                        |___/                                 " +
                              " \r\n                               ");
            Console.WriteLine("                                                          Developped by \n                    ");
            Console.WriteLine("                                Louis JAGUENEAU  - Nathan LORIT - Julien DESPEZ - Paul PESCHEL");
            Console.ResetColor();


            // Simulate loading with a progress bar
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n\n\n                                   Loading:");
            Console.CursorVisible = false; 

            int progressBarWidth = 35; 
            int progressBarCenter = (Console.WindowWidth - progressBarWidth) / 2; 

            Console.SetCursorPosition(progressBarCenter, Console.CursorTop); 

            Console.Write("["); 

            for (int i = 0; i <= 45; i++)
            {
                Console.Write("#"); 
                System.Threading.Thread.Sleep(10); 
            }

            Console.WriteLine("]"); 
            Console.CursorVisible = true;

            Console.ResetColor();
            Console.ForegroundColor= ConsoleColor.DarkGreen;
            Console.WriteLine("\n\n\nLoaded successfully!");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1000);

            var view = new SaveView();

            while (view.Initialize())
            {
                
            }

        }
    }
}