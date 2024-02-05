using App.Core.Models;
using App.Core.Services;
using System.Text.RegularExpressions;

namespace App.Cmd.ViewModels
{
    public class SaveViewModel
    {
        private readonly Core.Services.SaveService service;
        private readonly Core.Services.StringService stringService;
        public SaveModel model { get; set; }
    
        public SaveViewModel()
        {
            service = new Core.Services.SaveService();
            model = new SaveModel();
            stringService = new Core.Services.StringService();
        }

        public void Save()
        {
            Console.WriteLine("Fichier d'entrée");
            model.IN_PATH = Console.ReadLine();

            Console.WriteLine("Fichier de sortie");
            model.OUT_PATH = Console.ReadLine();

            Console.WriteLine("Type de Sauvegarde : ");
            Console.WriteLine("1 - Complète");
            Console.WriteLine("2 - Séquentielle");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    model.TYPE = false;
                    break;
                case 2:
                    model.TYPE = true;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Nom de la sauvegarde");
            model.SAVE_NAME = Console.ReadLine();
            //TODO Gérer l'exception du service
            if (string.IsNullOrEmpty(model.IN_PATH) ||
                               string.IsNullOrEmpty(model.OUT_PATH) ||
                                              string.IsNullOrEmpty(model.SAVE_NAME))
                throw new System.InvalidOperationException();

            service.Create(model);
        }

        public void Run()
        {
            Console.WriteLine("Que voulez exécuter / What do you want to execute ?");
            Console.WriteLine("1-3 => 1 à/at 3 ");
            Console.WriteLine("1,3 => 1 et/and 3 ");
            String input = Console.ReadLine();
            switch (stringService.IsCommaSeparatedOrHyphen(input))
            {
                case true:
                    Console.WriteLine("Comma");
                    string[] commaSeparatedParts = input.Split(',');
                    Console.WriteLine(commaSeparatedParts[0] + " à " + commaSeparatedParts[1]);
                    break;
                case false:
                    Console.WriteLine("Hyphen");
                    string[] hyphenSeparatedParts = input.Split('-');
                    Console.WriteLine(hyphenSeparatedParts[0] + " et " + hyphenSeparatedParts[1]);
                    break;
            }

            service.Run();




        }
        public void ShowLogs()
        {

        }

        public void ShowStateFile()
        {

        }

        public void ShowSchedule()
        {

        }
    }


}
