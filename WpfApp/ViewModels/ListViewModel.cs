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
        private SaveModel _selected;
        public SaveModel Selected
        {
            get => _selected;
            set
            {
                if (_selected != value)
                {
                    _selected = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<SaveModel> Items { get; } = new ObservableCollection<SaveModel>();

        private readonly SaveService _service = new SaveService();

        public event PropertyChangedEventHandler PropertyChanged;

        public RelayCommand RefreshCommand { get; }
        public RelayCommand CreateCommand { get; }
        public RelayCommand ExecuteCommand { get; }

        public ListViewModel()
        {
            RefreshCommand = new RelayCommand(Refresh);
            CreateCommand = new RelayCommand(Create);
            ExecuteCommand = new RelayCommand(Execute);
            Refresh();
        }

        public void Execute()
        {
            if (Selected != null)
            {
                SaveService.ExecuteSave(Selected);
            }
            else
            {
                // Handle case where no item is selected
            }
        }

        public void Create()
        {
            // Create a new instance of SaveModel (assuming SaveModel has a default constructor)
            var newSaveModel = new SaveModel();

            // Add the new SaveModel instance to the Items collection
            Items.Add(newSaveModel);

            // Optionally, select the newly added item
            Selected = newSaveModel;
        }

        public void Refresh()
        {
            Items.Clear();
            foreach (var item in _service.LoadSave())
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
