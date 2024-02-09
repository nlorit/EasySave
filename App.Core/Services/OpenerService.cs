using App.Core.Models;

namespace App.Core.Services
{
    public class OpenerService
    {
        private readonly OpenerModel model = new();

        public void OpenLogFile()
        {
            System.Diagnostics.Process.Start("notepad.exe", this.model.LogPath);
        }

        public void OpenStateFile()
        {
            System.Diagnostics.Process.Start("notepad.exe", this.model.StatePath);
        }
    }
}
