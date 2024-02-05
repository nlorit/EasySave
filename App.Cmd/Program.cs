using App.Cmd.Views;

namespace App.Cmd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting EasySave ...");
            var view = new SaveView();

            while (view.Initialize()) ;
            
        }
    }
}