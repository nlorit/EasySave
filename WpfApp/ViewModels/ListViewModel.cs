using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Cmd.ViewModels;
using App.Core.Models;
using App.Core.Services;

namespace WpfApp.ViewModels
{
    class ListViewModel : INotifyPropertyChanged
    {
        public RelayCommand RefreshCommand { get; set; }
        public RelayCommand CreateCommand { get; set; }
        public RelayCommand ExecuteCommand { get; set; }
        public ObservableCollection<SaveModel> Items { get; set; }

        private SaveModel selected;
        private readonly SaveService service;

        public event PropertyChangedEventHandler? PropertyChanged;

        public SaveModel Selected
        {
            get => selected;
            set
            {
                if (selected != value)
                {
                    selected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
                }
            }
        }

        public ListViewModel()
        {
            service = new SaveService();
            Items = new ObservableCollection<SaveModel>();

            RefreshCommand = new RelayCommand(Refresh);
            CreateCommand = new RelayCommand(Create);
            ExecuteCommand = new RelayCommand(Execute);
            Refresh();
        }

        public void Execute()
        {
            if (selected != null)
            {
                SaveService.ExecuteCopy(selected);
            }
            else
            {
                // Handle case where no item is selected
            }
        }

        public void Create()
        {
            // Implement create logic
        }

        public void Refresh()
        {
            Items.Clear();
            foreach (var item in service.LoadSave())
            {
                Items.Add(item);
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
