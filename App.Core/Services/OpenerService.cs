using App.Core.Models;

namespace App.Core.Services
{
    public class OpenerService
    {
        private readonly OpenerModel openerModel = new();

        public void OpenLogFile()
        {
            // Open the log file in notepad
            System.Diagnostics.Process.Start("notepad.exe", this.openerModel.LogPath);
        }

        public void OpenStateFile()
        {
            // Open the state file in notepad
            System.Diagnostics.Process.Start("notepad.exe", this.openerModel.StatePath);
        }
    }
}
