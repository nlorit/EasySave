﻿using App.Core.Models;
using App.Core.Services;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace App.Cmd.ViewModels
{
    public class SaveViewModel
    {
        private readonly SaveService service;
        private readonly StringService stringService;
        public SaveModel model { get; set; }
        public List<SaveModel> ListSave { get; set; } = [];
    
        public SaveViewModel()
        {
            
            stringService = new StringService();
            service = new SaveService();
            
        }

        public void add()
        {
            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/safran";
            model.OutPath = "C:/Users/Nathan/Desktop/safran2";
            model.Type = false;
            model.SaveName = "Save1";

            service.Create(model);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/Portfolio";
            model.OutPath = "C:/Users/Nathan/Desktop/Portfolio2";
            model.Type = false;
            model.SaveName = "Save2";

            service.Create(model);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Utilisateur/Documents/Projet/IN";
            model.OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT";
            model.Type = false;
            model.SaveName = "Save3";

            service.Create(model);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Utilisateur/Documents/Projet/IN";
            model.OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT";
            model.Type = false;
            model.SaveName = "Save4";

            service.Create(model);
            ListSave.Add(model);

        }

        public void Save()
        {
            model = new SaveModel();

            Console.WriteLine("Fichier d'entrée");
            model.InPath = Console.ReadLine();

            Console.WriteLine("Fichier de sortie");
            model.OutPath = Console.ReadLine();

            Console.WriteLine("Type de Sauvegarde : ");
            Console.WriteLine("1 - Complète");
            Console.WriteLine("2 - Séquentielle");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    model.Type = false;
                    break;
                case 2:
                    model.Type = true;
                    break;
                default:
                    break;
            }
            Console.WriteLine("Nom de la sauvegarde");
            model.SaveName = Console.ReadLine();
            //TODO Gérer l'exception du service
            if (string.IsNullOrEmpty(model.InPath) ||
                               string.IsNullOrEmpty(model.OutPath) ||
                                              string.IsNullOrEmpty(model.SaveName))
                throw new System.InvalidOperationException();


            service.Create(model);
            ListSave.Add(model);
        }

        public void Run()
        {
            Console.WriteLine("Que voulez-vous exécuter / What do you want to execute ?");
            Console.WriteLine("1-3 => 1 à/at 3 ");
            Console.WriteLine("1,3 => 1 et/and 3 ");

            string input = Console.ReadLine();

            bool isCommaSeparatedOrHyphen = stringService.IsCommaSeparatedOrHyphen(input);

            switch (isCommaSeparatedOrHyphen)
            {
                case true:
                    Console.WriteLine("Comma");
                    ProcessCommaSeparatedInput(input);
                    break;

                case false:
                    Console.WriteLine("Hyphen");
                    ProcessHyphenSeparatedInput(input);
                    break;
            }
        }

        private void ProcessCommaSeparatedInput(string input)
        {
            string[] commaSeparatedParts = input.Split(',');
            Console.WriteLine(commaSeparatedParts[0] + " à " + commaSeparatedParts[1]);

            int start = int.Parse(commaSeparatedParts[0]);
            int end = int.Parse(commaSeparatedParts[1]);

            service.Run(ListSave[start - 1]);
            service.Run(ListSave[end - 1]);
        }

        private void ProcessHyphenSeparatedInput(string input)
        {
            string[] hyphenSeparatedParts = input.Split('-');
            Console.WriteLine(hyphenSeparatedParts[0] + " à " + hyphenSeparatedParts[1]);

            for (int i = int.Parse(hyphenSeparatedParts[0]) - 1; i <= int.Parse(hyphenSeparatedParts[1]) - 1; i++)
            {
                service.Run(ListSave[i]);
            }

            
        }

        public void ShowLogs()
        {

        }

        public void ShowStateFile()
        {

        }

        public void ShowSchedule()
        {
            foreach (var item in ListSave)
            {
                Console.WriteLine("- " + service.ShowInfo(item));
            }
        }
    }


}