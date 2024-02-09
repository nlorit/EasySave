
namespace App.Core.Models
{
    public class OpenerModel
    {
        public string LogPath { get; set; } = "logs.json";          // Path of the log file
        public string StatePath { get; set; } = "state.json";       // Path of the state file
    }
}
