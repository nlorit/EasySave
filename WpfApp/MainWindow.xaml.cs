using App.Core.Models;
using Microsoft.Win32;
using System.Collections.ObjectModel;
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

        private void LoadSave()
        {
            this.Saves = viewModel!.saves;
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
                if (string.IsNullOrEmpty(SaveName.Text) || SourceName.Text == "" || DestinationName.Text == "" || Destination.Text == "Destination" || Source.Text == "Source" )
                {
                    MessageBox.Show("Please fill in all the fields");
                    return;
                }

                viewModel!.AddSave(save);
                MessageBox.Show("Save added successfully");
            }
            catch
            {
                // TODO : Handle exception
            }
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
                // Start the save operation
                viewModel!.StartSave((SaveModel)List_Save.SelectedItem);

                // Change the visual appearance of the selected item to indicate that it's running
                var selectedItem = List_Save.SelectedItem as SaveModel;
                if (selectedItem != null)
                {
                    // Get the row corresponding to the selected item
                    var row = (DataGridRow)List_Save.ItemContainerGenerator.ContainerFromItem(selectedItem);
                    if (row != null)
                    {
                        // Change the background color of the row to green
                        row.Background = Brushes.Green;

                        // Wait for the save operation to complete
                        Task.Run(() =>
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                while (viewModel!.IsSaveRunning((SaveModel)List_Save.SelectedItem))
                                {
                                    Thread.Sleep(1000); // Sleep for 1 second before checking again
                                }

                                // Update the row color to red when the save operation is completed
                                row.Dispatcher.Invoke(() =>
                                {
                                    row.Background = Brushes.Red;
                                });
                            });
                        });
                    }
                }
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
            }
        }

        private void PlaySave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PauseSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StopSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}