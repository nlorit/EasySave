﻿using App.Core.Models;
using App.Core.Services;
using System;
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

        public void add() //Jeux de test
        {
            model = new SaveModel();


            model.InPath = "R:/FILMS/1917 (2019)/QTZ 1917 (2019) Bluray-2160p.mkv";
            model.OutPath = "C:/Users/Nathan/Desktop/safran3/QTZ 1917 (2019) Bluray-2160p.mkv";
            model.Type = true;
            model.SaveName = "Save1";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            service.Create(model);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/safran2";
            model.OutPath = "C:/Users/Nathan/Desktop/safran3";
            model.Type = false;
            model.SaveName = "Save2";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            service.Create(model);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Nathan/Desktop/safran3/1. Lettre d'engagement.docx";
            model.OutPath = "C:/Users/Nathan/Desktop/safran3/1. Lettre d'engagement 2.docx";
            model.Type = false;
            model.SaveName = "Save3";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            service.Create(model);
            ListSave.Add(model);

            model = new SaveModel();

            model.InPath = "C:/Users/Utilisateur/Documents/Projet/IN";
            model.OutPath = "C:/Users/Utilisateur/Documents/Projet/OUT";
            model.Type = false;
            model.SaveName = "Save4";
            model.Date = DateTime.Parse("02/05/2024 10:00:00");

            service.Create(model);
            ListSave.Add(model);
        }

        public void Save()
        {
           if (ListSave.Count < 5)
             {
              model = new SaveModel();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("1- ");
              Console.ResetColor();
              Console.WriteLine("Répertoire d'entrée / Source Directory ? |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.InPath = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("   2- ");
              Console.ResetColor();
              Console.WriteLine("Fichier de sortie / Target file ?     |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.OutPath = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("    3- ");
              Console.ResetColor();
              Console.WriteLine("Type de Sauvegarde / Save type ?     |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              Console.WriteLine("1 - Complète / Complet");
              Console.WriteLine("2 - Séquentielle / sequential");
              Console.WriteLine("");
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

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("   4- ");
              Console.ResetColor();
              Console.WriteLine("Nom de la sauvegarde / Save name ?    |");
              Console.WriteLine("+---------------------------------------------+");
              Console.WriteLine("");
              model.SaveName = Console.ReadLine();

              Console.WriteLine("");
              Console.WriteLine("+---------------------------------------------+");
              Console.Write("| ");
              Console.ForegroundColor = ConsoleColor.Cyan;
              Console.Write("  5- ");
              Console.ResetColor();
              Console.WriteLine("Date de la sauvegarde / Date name ?    |");
              Console.WriteLine("+---------------------------------------------+");
              model.Date = DateTime.Parse(Console.ReadLine());
              //TODO Gérer l'exception du service
              if (string.IsNullOrEmpty(model.InPath) ||
                                 string.IsNullOrEmpty(model.OutPath) ||
                                                string.IsNullOrEmpty(model.SaveName))
                  throw new System.InvalidOperationException();


              service.Create(model);
              ListSave.Add(model);
           }
           else
           {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erreur : Vous avez atteint le nombre maximum de sauvegardes / Error : You have reached the maximum number of saves");
                Console.ResetColor();
           }

        }

        public void Run()
        {
            Console.WriteLine("");
            Console.WriteLine("+----------------------------------------------------------+");
            Console.WriteLine("| Que voulez-vous exécuter / What do you want to execute ? |");
            Console.WriteLine("|                                                          |");
            Console.Write("| ");
            Console.ForegroundColor= ConsoleColor.Cyan;
            Console.Write("Une liste / A list:");
            Console.ResetColor();
            Console.WriteLine(" 1-3 => 1 à/to 3                      |");
            Console.Write("| ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Certains éléments / Some element:");
            Console.ResetColor();
            Console.WriteLine("  1,3 => 1 et/and 3     |");
            Console.WriteLine("|                                                          |");
            Console.WriteLine("+----------------------------------------------------------+");


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
                //Console.WriteLine(service.ShowInfo(item));*
                service.ShowInfo(item);
            }
        }
    }
}
