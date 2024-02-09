
namespace App.Core.Models
{
    public class SaveModel
    {
        public string InPath { get; set; } = string.Empty;                                      // Path of the source file
        public string OutPath { get; set; } = string.Empty;                                     // Path of the target file
        public bool Type { get; set; } = false;                                                 // Type of the save // false = Complete, true = Sequentiel          
        public string SaveName { get; set; } = string.Empty ;                                   // Name of the save
        public DateTime Date { get; set; }                                                      // Date of the save
        public StateManagerModel StateManager { get; set; } = new();                            // State of the save
    }
}
