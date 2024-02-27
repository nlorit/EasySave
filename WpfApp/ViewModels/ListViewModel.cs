using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using App.Core.Models;
using App.Core.Services;

namespace WpfApp.ViewModels
{
    class ListViewModel 
    {
        public ObservableCollection<SaveModel> Selected
        {
            get => Selected;
            set
            {
                if (Selected != value)
                {
                    Selected = value;
                }
            }
        }


        //public ObservableCollection<StateManagerModel> listStateManager { get; set; } = new ObservableCollection<StateManagerModel>();
        //public ObservableCollection<SaveModel> listSaveModel{ get; set; } = new ObservableCollection<SaveModel>();
        //public ObservableCollection<SaveModel> Items { get; set; } = new ObservableCollection<SaveModel>();


        //private SaveService? saveService;


        //public event PropertyChangedEventHandler PropertyChanged;

        //public RelayCommand RefreshCommand { get; }
        //public RelayCommand CreateCommand { get; }
        //public RelayCommand ExecuteCommand { get; }
        //public RelayCommand DeleteCommand { get; }

        //public ListViewModel()
        //{
        //    saveService = new()
        //    {
        //        ListStateManager = this.listStateManager,
        //        ListSaveModel = this.listSaveModel
        //    };

        //    saveService.LoadSave();
        //    this.listSaveModel = saveService.ListSaveModel;
        //    this.listStateManager = saveService.ListStateManager;
        //    foreach (var item in listSaveModel)
        //    {
        //        Items.Add(item);
        //    }
        //    RefreshCommand = new RelayCommand(Refresh);
        //    CreateCommand = new RelayCommand(Create);
        //    ExecuteCommand = new RelayCommand(Execute);
        //    DeleteCommand = new RelayCommand(Delete);

        //}

        //public void Execute()
        //{
        //    if (Selected != null)
        //    {
        //        Task.Run(() => saveService!.ExecuteSave(Selected));
        //    }
        //    else
        //    {
        //        // Handle case where no item is selected
        //    }
        //}

        //public void Create()
        //{
        //    // Create a new instance of SaveModel (assuming SaveModel has a default constructor)
        //    var newSaveModel = new ObservableCollection<SaveModel>();

        //    // Add the new SaveModel instance to the Items collection
        //    Items.Add(newSaveModel);

        //    // Optionally, select the newly added item
        //    newSaveModel = Selected;

        //    saveService!.CreateSave(newSaveModel.InPath, newSaveModel.OutPath, newSaveModel.Type, newSaveModel.SaveName);
        //    Thread.Sleep(500);
        //    Refresh();
        //}

        //public void Refresh()
        //{
        //    Items.Clear();
        //    foreach (var item in listSaveModel)
        //    {
        //        Items.Add(item);
        //    }
        //}

        //public void Delete()
        //{
        //    if (Selected != null)
        //    {
        //        saveService!.DeleteSave(Selected);
        //        Refresh();
        //    }
        //    else
        //    {
        //        // Handle case where no item is selected
        //    }
        //}

        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
