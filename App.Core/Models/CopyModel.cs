

using System.ComponentModel;

namespace App.Core.Models
{
    public class CopyModel : INotifyPropertyChanged
    {
        public string SourcePath { get; set; } = string.Empty;                              // Path of the source file
        public string TargetPath { get; set; } = string.Empty;                              // Path of the target file

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
