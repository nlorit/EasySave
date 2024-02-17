namespace App.Core.Models
{
    public class SaveModel
    {
        public string InPath { get; set; } = "";                                      // Path of the source file
        public string OutPath { get; set; } = "";                                     // Path of the target file
        public string Type { get; set; } = "";                                         // Type of the save // false = Complete, true = Sequentiel          
        public string SaveName { get; set; } = "" ;                                   // Name of the save
        public DateTime Date { get; } = DateTime.Now;                // Date of the save
    }
}
