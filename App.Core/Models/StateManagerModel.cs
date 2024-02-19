namespace App.Core.Models
{

    public class StateManagerModel
    {
        public string SaveName { get; set; } = string.Empty;                                // Name of the save
        public string SourceFilePath { get; set; } = string.Empty;                          // Path of the source file
        public string TargetFilePath { get; set; } = string.Empty;                          // Path of the target file
        public string State { get; set; } = "END";                                          // State of the save
        public long TotalFilesToCopy { get; set; } = 0;                                      // Total files to copy
        public long TotalFilesSize { get; set; } = 0;                                       // Total size of the files to copy
        public long NbFilesLeftToDo { get; set; } = 0;                                       // Number of files left to copy
        public float Progression { get; set; } = 0;                                         // Progression of the save

    }
}
