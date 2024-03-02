using Medias.Forms;
using Medias.MediaIO;
using Medias.Model.Enum;
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
        private List<Genre> genres = new();
        private ObservableCollection<MediaItem> displayedMovies = new();// Обновленное свойство

        public MainWindow()
        {
            controller = new MovieController();

            //controller.AddMovie(
            //    new Movie(
            //        0,
            //        "Movie1",
            //        "Good movie",
            //        new List<Genre>() {
            //    new Genre("action"),new Genre("comedy")
            //                            },
            //        new List<Author>()
            //                            {
            //    new Author("John Doe")
            //                            },
            //        WatchStatus.WATCHED,
            //        TimeSpan.FromMinutes(120)

            //        ));

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
            MovieTypeComboBox.ItemsSource = Enum.GetValues(typeof(MovieType));
            MovieTypeComboBox.SelectedItem = MovieType.ANY;


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

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            EditMediaItem();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void Save()
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

            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Binary Files (*.dat)|*.dat|All Files (*.*)|*.*"; // Filter for files

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

        private void ActionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Action"));
            LoadMoviesToDataGrid();
        }
        private void ComedyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Comedy"));
            LoadMoviesToDataGrid();


        }
        private void DramaCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Drama"));
            LoadMoviesToDataGrid();

        }
        private void FantasyCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Fantasy"));
            LoadMoviesToDataGrid();

        }
        private void HorrorCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Horror"));
            LoadMoviesToDataGrid();
        }
        private void MysteryCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Mystery"));
            LoadMoviesToDataGrid();

        }
        private void RomanceCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Romance"));
            LoadMoviesToDataGrid();

        }
        private void ThrillerCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            genres.Add(new Genre("Thriller"));
            LoadMoviesToDataGrid();

        }
        private void ActionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Action"));
            LoadMoviesToDataGrid();
        }
        private void ComedyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Comedy"));
            LoadMoviesToDataGrid();


        }
        private void DramaCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Drama"));
            LoadMoviesToDataGrid();

        }
        private void FantasyCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Fantasy"));
            LoadMoviesToDataGrid();

        }
        private void HorrorCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Horror"));
            LoadMoviesToDataGrid();
        }
        private void MysteryCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Mystery"));
            LoadMoviesToDataGrid();

        }
        private void RomanceCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Romance"));
            LoadMoviesToDataGrid();

        }
        private void ThrillerCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            genres.Remove(new Genre("Thriller"));
            LoadMoviesToDataGrid();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (controller.IsEmpty()) return;

            bool? confirm = ShowConfirmationCancelDialog("Do you want save changes?");
            if (confirm.HasValue) // Check,if was pressed button "Yes" or "No"
            {
                if (confirm == true)
                {
                    Save();
                }
                // If cancel button pressed do nothing
            }
            else // If cancel button pressed
            {
                e.Cancel = true; // Cancel window closing
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Ваш код для обработки изменений в SearchTextBox
            // Например, можно выполнить поиск и обновить отображаемые данные в DataGrid
            LoadMoviesToDataGrid();
        }
        //private void LoadMoviesToDataGrid()
        //{

        //    dataGrid.ItemsSource = new ObservableCollection<MediaItem>(controller.Movies);

        //}

        //private void LoadMoviesToDataGrid()
        //{
        //    List<MediaItem> nameFilteredItems = new List<MediaItem>();
        //    string searchText = SearchTextBox.Text.ToLower();
        //    if (!string.IsNullOrEmpty(searchText))
        //    {
        //        foreach (MediaItem item in controller.Movies)
        //        {
        //            if (item.Name.ToLower().Contains(searchText))
        //            {
        //                nameFilteredItems.Add(item);
        //            }
        //        }
        //    }


        //    List<MediaItem> genreFilteredItems = new List<MediaItem>();

        //    List<MediaItem> items = controller.Movies;
        //    if (nameFilteredItems.Count > 0) items = nameFilteredItems;
        //    // Filter movies by selected genres
        //    if (genres.Count > 0)
        //    {
        //        foreach (MediaItem item in items)
        //        {
        //            bool containsAllGenres = true;
        //            foreach (Genre genre in genres)
        //            {
        //                if (!item.Genres.Contains(genre))
        //                {
        //                    containsAllGenres = false;
        //                    break;
        //                }
        //            }
        //            if (containsAllGenres) genreFilteredItems.Add(item);

        //        }
        //    }
        //    else genreFilteredItems.AddRange(controller.Movies);


        //    //string searchText = SearchTextBox.Text.ToLower();

        //    //if (searchText != "") 
        //    //{
        //    //    foreach (MediaItem item in filteredItems) 
        //    //    {
        //    //        if (!item.Name.ToLower().Contains(searchText)) 
        //    //        {
        //    //            filteredItems.Remove(item);
        //    //        }
        //    //    }
        //    //}


        //    dataGrid.ItemsSource = new ObservableCollection<MediaItem>(genreFilteredItems);
        //}

        private void MovieTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Ваш код для обработки изменений выбранного элемента в ComboBox
            LoadMoviesToDataGrid();
        }

        private void ClearFilter_Click(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = string.Empty;
            MovieTypeComboBox.SelectedItem = MovieType.ANY;
            UncheckCheckboxes();
            LoadMoviesToDataGrid();
        }

        private void UncheckCheckboxes()
        {
            ActionCheckBox.IsChecked = false;
            ComedyCheckBox.IsChecked = false;
            DramaCheckBox.IsChecked = false;
            FantasyCheckBox.IsChecked = false;
            HorrorCheckBox.IsChecked = false;
            MysteryCheckBox.IsChecked = false;
            RomanceCheckBox.IsChecked = false;
            ThrillerCheckBox.IsChecked = false;

        }

        private void LoadMoviesToDataGrid()
        {
            List<MediaItem> movieTypeFilteredItems = new List<MediaItem>();

            if ((MovieType)MovieTypeComboBox.SelectedItem == MovieType.MOVIE)
            {
                //movieTypeFilteredItems.AddRange(controller.Movies.Where(movie => movie is Movie));
                foreach (MediaItem item in controller.Movies) if (item is Movie) movieTypeFilteredItems.Add(item);

            }
            else if ((MovieType)MovieTypeComboBox.SelectedItem == MovieType.SERIES)
            {
                //movieTypeFilteredItems.AddRange(controller.Movies.Where(movie => movie is Series));
                foreach (MediaItem item in controller.Movies)
                {
                    if (item is Series) movieTypeFilteredItems.Add(item);
                }
            }
            else movieTypeFilteredItems = controller.Movies;


            List<MediaItem> nameFilteredItems = new List<MediaItem>();
            string searchText = SearchTextBox.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                foreach (MediaItem item in movieTypeFilteredItems)
                {
                    if (item.Name.ToLower().Contains(searchText))
                    {
                        nameFilteredItems.Add(item);
                    }
                }
            }
            else nameFilteredItems = movieTypeFilteredItems;

            List<MediaItem> genreFilteredItems = new List<MediaItem>();


            List<MediaItem> items = controller.Movies;
            if (nameFilteredItems.Count > 0) items = nameFilteredItems;
            // Filter movies by selected genres
            if (genres.Count > 0)
            {
                foreach (MediaItem item in nameFilteredItems)
                {
                    bool containsAllGenres = true;
                    foreach (Genre genre in genres)
                    {
                        if (!item.Genres.Contains(genre))
                        {
                            containsAllGenres = false;
                            break;
                        }
                    }
                    if (containsAllGenres) genreFilteredItems.Add(item);

                }
            }
            else genreFilteredItems.AddRange(nameFilteredItems);

            displayedMovies.Clear(); // Очистка данных
            foreach (var item in genreFilteredItems)
            {
                displayedMovies.Add(item); // Добавление отфильтрованных данных
            }

            dataGrid.ItemsSource = displayedMovies;
        }



        private void AddMediaItem(MediaItem mediaItem)
        {
            controller.AddMovie(mediaItem);
            LoadMoviesToDataGrid();
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
        private void DeleteMediaItem(MediaItem selectedItem)
        {
            controller.DeleteMovie(selectedItem);
            LoadMoviesToDataGrid();
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
        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private bool ShowConfirmationDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private bool? ShowConfirmationCancelDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }
            else // Если пользователь нажал "Отмена"
            {
                return null;
            }
        }


    }
}
