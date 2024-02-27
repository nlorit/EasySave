using App.Core.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp.ViewModels;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel? viewModel;
        public ObservableCollection<SaveModel> Saves { get; set; } = new ObservableCollection<SaveModel>();

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            List_Save.Items.Clear();
            LoadSave();
            List_Save.ItemsSource = this.Saves;
        }

        private void QuitBtns_CLick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void LangBtns_Click(object sender, RoutedEventArgs e)
        {
            SetLang(((Button)sender).Tag.ToString());
        }

        private void SetLang(string lang)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);

            Application.Current.Resources.MergedDictionaries.Clear();
            ResourceDictionary resdict = new ResourceDictionary()
            {
                Source = new Uri($"/Dictionary-{lang}.xaml", UriKind.Relative)
            };
            Application.Current.Resources.MergedDictionaries.Add(resdict);

            EnglishBtn.IsEnabled = true;
            FrenchBtn.IsEnabled = true;

            switch (lang)
            {
                case "en-US":
                    EnglishBtn.IsEnabled = false;
                    break;
                case "fr-FR":
                    FrenchBtn.IsEnabled = false;
                    break;
                default:
                    break;
            }

        }

        private void LoadSave()
        {
            if (viewModel!.IsLoadCorrectly)
            {
                this.Saves = viewModel!.saves;
                MessageBox.Show("Saves loaded successfully", "Succes" ,MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //error message box
                MessageBox.Show("Error loading saves", "Fatal Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void addSave_Click(object sender, RoutedEventArgs e)
        {
            SaveModel save = new SaveModel
            {
                SaveName = SaveName.Text,
                InPath = SourceName.Text,
                OutPath = DestinationName.Text,
                Type = IsComplete.IsChecked != null && (bool)IsComplete.IsChecked ? "Complete" : "Incremental",
                EncryptChoice = IsEncrypted.IsChecked != null && (bool)IsEncrypted.IsChecked ? "True" : "False"
            };
            try
            {
                if (false)
                {
                    MessageBox.Show("Please fill in all the fields");
                    return;
                }

                viewModel!.AddSave(save);
                
                this.Saves = viewModel!.LoadSave();
                List_Save.ItemsSource = this.Saves;
                MessageBox.Show("Save added successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


            }
            catch
            {
                // TODO : Handle exception
            }
            DestinationName.Text = string.Empty;
            SourceName.Text = string.Empty;
            SaveName.Text = string.Empty;
        }


        private void SourcePopup_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog(); //Declaration of the method to open the window to choose the folder path.
            dialog.DefaultDirectory = "";
            if ((bool)dialog.ShowDialog())
            {
                SourceName.Text = dialog.FolderName;
            }


        }

        private void DestinationPopup_Click(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new OpenFolderDialog(); //Declaration of the method to open the window to choose the folder path.
            dialog.DefaultDirectory = "";
            if ((bool)dialog.ShowDialog())
            {
                DestinationName.Text = dialog.FolderName;
            }
        }

        private void StartSave_Click(object sender, RoutedEventArgs e)
        {
            if (List_Save.SelectedItem != null)
            {
                // Start the save operation asynchronously
                var selectedItem = (SaveModel)List_Save.SelectedItem;
                var row_init = (DataGridRow)List_Save.ItemContainerGenerator.ContainerFromItem(selectedItem);
                if (row_init != null)
                {
                    row_init.Background = Brushes.Green;
                }
                viewModel!.LaunchSave(selectedItem);
                
            }
            else
            {
                // Handle case where no item is selected
            }
        }

        private void DeleteSave_Click(object sender, RoutedEventArgs e)
        {
            if (List_Save.SelectedItem != null)
            {
                // Start the save operation
                viewModel!.DeleteSave((SaveModel)List_Save.SelectedItem);
                this.Saves = viewModel!.saves;
                List_Save.ItemsSource = this.Saves;
            }
        }

        private void PlaySave_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (SaveModel)List_Save.SelectedItem;
            var row_init = (DataGridRow)List_Save.ItemContainerGenerator.ContainerFromItem(selectedItem);
            if (row_init != null)
            {
                row_init.Background = Brushes.Green;
            }
            viewModel!.PlaySave(selectedItem);
        }

        private void PauseSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (SaveModel)List_Save.SelectedItem;
            var row_init = (DataGridRow)List_Save.ItemContainerGenerator.ContainerFromItem(selectedItem);
            if (row_init != null)
            {
                row_init.Background = Brushes.Orange;
            }
            viewModel!.PauseSave(selectedItem);
        }


        private void StopSave_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (SaveModel)List_Save.SelectedItem;
            var row_init = (DataGridRow)List_Save.ItemContainerGenerator.ContainerFromItem(selectedItem);
            if (row_init != null)
            {
                row_init.Background = Brushes.Red;
            }
            viewModel!.StopSave(selectedItem);
        }
    }
}