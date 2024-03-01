using Medias.Forms;
using Medias.MediaIO;
using MovieSaver.Controller;
using MovieSaver.Model;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Medias
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MovieController controller;
        public MainWindow()
        {
            controller = new MovieController();

            controller.AddMovie(
                new Movie(
                    0,
                    "Movie1",
                    "Good movie",
                    new List<Genre>() {
                new Genre("action"),new Genre("comedy")
                                        },
                    new List<Author>()
                                        {
                new Author("John Doe")
                                        },
                    WatchStatus.WATCHED,
                    TimeSpan.FromMinutes(120)

                    ));

            //   controller.AddMovie(
            //    new Series(
            //   0,
            //   "Series1",
            //   "Good series",
            //   new List<Genre>() {
            //       new Genre("action"),new Genre("comedy")
            //                    },
            //    new List<Author>()
            //                    {
            //       new Author("John Doe")
            //                    },
            //    WatchStatus.WATCHED, 15,
            //   TimeSpan.FromMinutes(30)

            //));

            InitializeComponent();
            // dataGrid.ItemsSource = new ObservableCollection<MediaItem>();

            LoadMoviesToDataGrid();



        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addMovieOrSeries = new MediaSelect();
            addMovieOrSeries.ShowDialog();
            if (addMovieOrSeries.MediaItem != null)
            {
                AddMediaItem(addMovieOrSeries.MediaItem);
            }

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            DeleteMediaItem();
        }
        private void DeleteMediaItem()
        {
            if (controller.IsEmpty())
            {
                ShowErrorDialog("Movie list is empty.");
                return;
            }
            if (dataGrid.SelectedItem == null)
            {
                ShowErrorDialog("Select movie to delete.");
                return;
            }

            // Get selected item
            MediaItem selectedItem = (MediaItem)dataGrid.SelectedItem;

            // Show dialog
            bool result = ShowConfirmationDialog($"Are you sure you want to delete '{selectedItem.Name}'?");
            // Check dialog result
            if (result)
            {
                // Delete item if user confirm
                DeleteMediaItem(selectedItem);
            }

        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditMediaItem();
        }

        private void EditMediaItem()
        {
            if (dataGrid.SelectedItem == null)
            {
                ShowErrorDialog("Select movie to edit.");
                return;
            }

            MediaItem selectedItem = (MediaItem)dataGrid.SelectedItem;

            if (selectedItem is Movie) EditMovieItem(selectedItem);
            else if (selectedItem is Series) EditSeriesItem(selectedItem);
            else ShowErrorDialog("Edit movie error.");

        }

        private void EditMovieItem(MediaItem selectedItem)
        {
            Movie selectedMovie = (Movie)selectedItem;
            var editMovieWindow = new AddMovie(selectedMovie);
            editMovieWindow.ShowDialog();
            MediaItem editedMovie = editMovieWindow.NewMovie;

            controller.EditMovie(editedMovie);
            LoadMoviesToDataGrid();


        }

        private void EditSeriesItem(MediaItem selectedItem)
        {
            Series selectedSeries = (Series)selectedItem;
            var editSeriesWindow = new AddSeries(selectedSeries);
            editSeriesWindow.ShowDialog();
            Series editedSeries = editSeriesWindow.NewMovie;

            controller.EditMovie(editedSeries);
            LoadMoviesToDataGrid();



        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (controller.IsEmpty())
            {
                ShowErrorDialog("Movie list is empty.");
                return;
            }

            // Create a dialog box for selecting a save file
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog.Filter = "Binary Files (*.dat)|*.dat|All Files (*.*)|*.*"; // Фильтр для файлов
            saveFileDialog.DefaultExt = ".dat"; // Расширение файла по умолчанию

            // Open the dialog box and get the result
            bool? result = saveFileDialog.ShowDialog();

            // Checking whether the file was selected and whether the "Save" button was clicked
            if (result == true)
            {
                // Getting the path to the selected file
                string filePath = saveFileDialog.FileName;

                try
                {
                    Serializer.SaveToFile(controller.Movies, filePath);
                    MessageBox.Show($"Data will be saved to: {filePath}");
                }
                catch (IOException ex)
                {
                    ShowErrorDialog(ex.Message);
                }

            }
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Load was clicked");
            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Binary Files (*.dat)|*.dat|All Files (*.*)|*.*"; // Фильтр для файлов

            // Open the dialog box and get the result
            bool? result = openFileDialog.ShowDialog();

            // Checking whether the file was selected and whether the "Open" button was clicked
            if (result == true)
            {
                // Getting the path to the selected file
                string filePath = openFileDialog.FileName;

                try
                {
                    List<MediaItem> readedItems = Deserializer.ReadFile(filePath);

                    // Clear existing data
                    controller = new(readedItems);

                    LoadMoviesToDataGrid();
                    MessageBox.Show($"Data loaded from: {filePath}");
                }
                catch (IOException ex)
                {
                    ShowErrorDialog(ex.Message);
                }
            }

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (controller.IsEmpty())
            {
                ShowErrorDialog("Movie list is empty.");
                return;
            }

            bool result = ShowConfirmationDialog($"Are you sure you want to delete all items?");

            // Check dialog result
            if (result)
            {
                // Delete item if user confirm
                controller.ClearAll();
                LoadMoviesToDataGrid();
            }

        }


        private void AddMediaItem(MediaItem mediaItem)
        {
            controller.AddMovie(mediaItem);
            LoadMoviesToDataGrid();
        }
        private void DeleteMediaItem(MediaItem selectedItem)
        {
            controller.DeleteMovie(selectedItem);
            LoadMoviesToDataGrid();
        }
        private void LoadMoviesToDataGrid()
        {

            dataGrid.ItemsSource = new ObservableCollection<MediaItem>(controller.Movies);


            //foreach (var movie in controller.Movies)
            //{
            //    dataGrid.Items.Add(movie);
            //}

        }


        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem != null)
            {
                // Приведение выбранного элемента к вашему типу данных, если это необходимо
                //var item = (MediaItem)grid.SelectedItem;

                // Выполнение действия с выбранным элементом
                //MessageBox.Show($"Double click on item: {item.ToString()}");
                EditMediaItem();
            }
        }

        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private bool ShowConfirmationDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }
    }
}
