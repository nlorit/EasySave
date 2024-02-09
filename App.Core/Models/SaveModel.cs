
namespace App.Core.Models
{
    public class SaveModel
    {
        public string InPath { get; set; } = "";
        public string OutPath { get; set; } = "";
        public bool Type { get; set; } = false; // false = Complete, true = Sequentiel
        public string SaveName { get; set; } = "";
        public DateTime Date { get; set; }
        public StateManagerModel StateManager { get; set; } = new StateManagerModel();
    }
}
