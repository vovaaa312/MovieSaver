using Medias.Forms;
using MovieSaver.Controller;
using MovieSaver.Model;
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
                    1,
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

            InitializeComponent();
            LoadMoviesToDataGrid();

        }



        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addMovieOrSeries = new MediaSelect();
            addMovieOrSeries.ShowDialog();
            if (addMovieOrSeries.MediaItem!=null) 
            {
                AddMediaItem(addMovieOrSeries.MediaItem);
            }
            
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Delete was clicked");
            if (dataGrid.SelectedItem != null)
            {
               
                MediaItem selectedItem = (MediaItem)dataGrid.SelectedItem;
                DeleteMediaItem(selectedItem);
            }
            else MessageBox.Show("Select item to delete.");

        }



        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                MediaItem selectedItem = (MediaItem)dataGrid.SelectedItem;
                
                MessageBox.Show($"Edit item clicked.\nItem={selectedItem.ToString()}");
                
                if (selectedItem is Movie) EditMovieItem(selectedItem);
                else if (selectedItem is Series) EditSeriesItem();
                else MessageBox.Show("Error: selected item is not movie or series");

            }
            else MessageBox.Show("Select item to edit.");
        }

        private void EditMovieItem(MediaItem selectedItem)
        {
            var editMovieWindow = new AddMovie(selectedItem);
            editMovieWindow.ShowDialog();
        }

        private void EditSeriesItem()
        {

        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save was clicked");
        }
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Load was clicked");

        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clear was clicked");

        }


        private void AddMediaItem(MediaItem mediaItem)
        {
            controller.AddMovie(mediaItem);
            LoadMoviesToDataGrid();
        }
        private void DeleteMediaItem(MediaItem selectedItem)
        {
            controller.RemoveMovie(selectedItem);
            LoadMoviesToDataGrid();
        }
        private void LoadMoviesToDataGrid()
        {
            dataGrid.Items.Clear();
            foreach (var movie in controller.Movies)
            {
                dataGrid.Items.Add(movie);
            }

        }
    }
}
