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
            MessageBox.Show("App is running");
        }

        private void LoadMoviesToDataGrid()
        {
            List<MediaItem> mediaItems = controller.Movies;
            dataGrid.Items.Clear();

            foreach (var movie in mediaItems)
            {
                dataGrid.Items.Add(movie);
            }

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            var addMovieWindow = new MediaSelect();
            addMovieWindow.Show();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save was clicked");
        }
    }
}
