using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Core.Services
{
    /// <summary>
    /// Class related to all interactions with the console
    /// </summary>
    public class DisplayService
    {
        /// <summary>
        /// Method to display the text in the specified color
        /// </summary>
        /// <param name="color"></param>
        /// <param name="text"></param>
        public static void SetForegroundColor(string color, string text)
        {
            Console.ForegroundColor = color switch
            {
                "Red" => ConsoleColor.Red,
                "Green" => ConsoleColor.Green,
                "Yellow" => ConsoleColor.Yellow,
                "Blue" => ConsoleColor.Blue,
                "Cyan" => ConsoleColor.Cyan,
                "Magenta" => ConsoleColor.Magenta,
                "White" => ConsoleColor.White,
                "DarkRed" => ConsoleColor.DarkRed,
                "DarkGreen" => ConsoleColor.DarkGreen,
                "DarkYellow" => ConsoleColor.DarkYellow,
                "DarkBlue" => ConsoleColor.DarkBlue,
                "DarkCyan" => ConsoleColor.DarkCyan,
                "DarkMagenta" => ConsoleColor.DarkMagenta,
                "Gray" => ConsoleColor.Gray,
                "DarkGray" => ConsoleColor.DarkGray,
                "Black" => ConsoleColor.Black,
                _ => ConsoleColor.White,
            };
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void SetBackgrounfColor(string color, string text)
        {
            Console.BackgroundColor = color switch
            {
                "Red" => ConsoleColor.Red,
                "Green" => ConsoleColor.Green,
                "Yellow" => ConsoleColor.Yellow,
                "Blue" => ConsoleColor.Blue,
                "Cyan" => ConsoleColor.Cyan,
                "Magenta" => ConsoleColor.Magenta,
                "White" => ConsoleColor.White,
                "DarkRed" => ConsoleColor.DarkRed,
                "DarkGreen" => ConsoleColor.DarkGreen,
                "DarkYellow" => ConsoleColor.DarkYellow,
                "DarkBlue" => ConsoleColor.DarkBlue,
                "DarkCyan" => ConsoleColor.DarkCyan,
                "DarkMagenta" => ConsoleColor.DarkMagenta,
                "Gray" => ConsoleColor.Gray,
                "DarkGray" => ConsoleColor.DarkGray,
                "Black" => ConsoleColor.Black,
                _ => ConsoleColor.White,
            };
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Method to display the welcome message
        /// </summary>
        public static void DisplayWelcomeMessage()
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
        }
        /// <summary>
        /// Display the progress bar
        /// </summary>
        public static void DisplayProgressBar()
        {
            // Simulate loading with a progress bar
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n\n\n                                   Loading:");
            Console.CursorVisible = false;

            // Progress bar
            int ProgressBarWidth = 35;
            int ProgressBarCenter = (Console.WindowWidth - ProgressBarWidth) / 2;
            Console.SetCursorPosition(ProgressBarCenter, Console.CursorTop);
            Console.Write("[");
            for (int i = 0; i <= 45; i++)
            {
                Console.Write("#");
                System.Threading.Thread.Sleep(10);
            }
            Console.WriteLine("]");
            Console.CursorVisible = true;

            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n\n\nLoaded successfully!");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1000);
        }

        /// <summary>
        /// Get the ressource from the ressource file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string? GetResource(string key)
        {
            string resourceFileName = CultureInfo.CurrentCulture.Name == "fr-FR" ? "ResourcesFR-FR" : "ResourcesEN-UK";
            string baseName = "App.Core.Resources." + resourceFileName;

            // Load the resources
            ResourceManager resources = new ResourceManager(baseName, typeof(DisplayService).Assembly);
            return resources.GetString(key);
        }

        
        /// <summary>
        /// Method to display the main menu
        /// </summary>
        public static void DisplayMenu()
        {
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
            Output = DisplayService.GetResource("Menu1");
            Console.WriteLine(Output);
            Output = DisplayService.GetResource("Menu2");
            Console.WriteLine(Output);
            Output = DisplayService.GetResource("Menu3");
            Console.WriteLine(Output);
            Output = DisplayService.GetResource("Menu4");
            Console.WriteLine(Output);
            Output = DisplayService.GetResource("Menu5");
            Console.WriteLine(Output);
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Output = DisplayService.GetResource("Menu6");
            Console.Write(Output);
            Console.ResetColor();
            Console.WriteLine("          | ");
            Console.WriteLine("|                                                           |");
            Console.WriteLine("+-----------------------------------------------------------+");
            Console.WriteLine("");
        }
    }
}
