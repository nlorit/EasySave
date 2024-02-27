using System;
using System.ComponentModel;

namespace App.Core.Models
{
    public class SaveModel : INotifyPropertyChanged
    {
        private int _percentage;

        public string InPath { get; set; } = "";            // Path of the source file
        public string OutPath { get; set; } = "";           // Path of the target file
        public string Type { get; set; } = "";              // Type of the save (false = Complete, true = Sequential)          
        public string SaveName { get; set; } = "";          // Name of the save
        public string EncryptChoice { get; set; } = "";
        public DateTime Date { get; } = DateTime.Now;       // Date of the save

        public float percentage
        {
            get { return _percentage; }
            set
            {
                if (_percentage != value)
                {
                    _percentage = (int)value;
                    OnPropertyChanged(nameof(percentage));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
