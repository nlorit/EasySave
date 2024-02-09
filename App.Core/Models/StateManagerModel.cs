namespace App.Core.Models
{
    public class StateManagerModel
    {
        public string SaveName { get; set; } = "";
        public string SourceFilePath { get; set; } = "";
        public string TargetFilePath { get; set; } = "";
        public string State { get; set; } = "END"; 
        public int TotalFilesToCopy { get; set; } = 0;
        public long TotalFilesSize { get; set; } = 0;
        public int NbFilesLeftToDo { get; set; } = 0;
        public float Progression { get; set; } = 0;

    }
}
