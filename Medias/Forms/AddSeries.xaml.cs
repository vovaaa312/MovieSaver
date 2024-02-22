using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Medias.Forms
{
    /// <summary>
    /// Interaction logic for AddSeries.xaml
    /// </summary>
    public partial class AddSeries : Window
    {
        private List<Genre> GenresList = new();
        private List<Author> AuthorsList = new();

        public Series NewMovie { get; set; }

        public AddSeries()
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));
            comboStatus.SelectedItem = WatchStatus.NotSelected;
        }

        public AddSeries(Series series) {

            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));

            //NewMovie = (Series)series;
            NewMovie = new Series();
            NewMovie.Id = series.Id; 

            txtName.Text = series.Name;
            txtDescription.Text = series.Description;
            GenresList = series.Genres;
            AuthorsList = series.Authors;
            comboStatus.SelectedItem = series.Status;
            txtAvgEpisodeLength.Text = series.Duration.TotalMinutes.ToString();
            txtEpisodesCount.Text = series.SeriesCount.ToString();
            
            LoadGenresToList();
            LoadAuthorsToList();
        }

    

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresComboBox.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)GenresComboBox.SelectedItem;
                string genreName = selectedItem.Content.ToString();
                if (!GenresList.Any(g => g.Name == genreName))  // if selected genre not in the list...
                {
                    //then add new genre to list
                    GenresList.Add(new Genre(genreName));
                    //update list
                    LoadGenresToList();
                }
                else
                {
                    MessageBox.Show("This genre already exists in the list.");
                }

            }
        }

        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresListBox.SelectedItem != null)
            {
                string selectedGenre = (string)GenresListBox.SelectedItem;
                GenresList.RemoveAll(g => g.Name == selectedGenre);
                LoadGenresToList();
            }
            else
            {
                MessageBox.Show("Please select a genre to delete.");
            }
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            string authName = AuthorNameTextBox.Text;
            if (authName.Length > 0)
            {
                Author auth = new(authName);

                if (!AuthorsList.Any(g => g.Name == authName))  // if new author not in the list...
                {
                    //then add new author to list
                    AuthorsList.Add(auth);
                    //update list
                    LoadAuthorsToList();
                }
                else
                {
                    MessageBox.Show("This author already exists in the list.");
                }

            }
            else MessageBox.Show("Enter author name.");

        }

        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsListBox.SelectedItem != null)
            {
                string selectedAuthor = (string)AuthorsListBox.SelectedItem;
                AuthorsList.RemoveAll(g => g.Name == selectedAuthor);
                LoadAuthorsToList();
            }
            else
            {
                MessageBox.Show("Please select a author to delete.");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            double avgEpisodeLength; // Используем double для парсинга времени
            double.TryParse(txtAvgEpisodeLength.Text, out avgEpisodeLength); // Парсим как Double

            double episodesCount = 0; // Используем double для парсинга времени
            double.TryParse(txtEpisodesCount.Text, out avgEpisodeLength); // Парсим как Double


            string movieName = txtName.Text;
            string description = txtDescription.Text;
            WatchStatus statusSelected = (WatchStatus)comboStatus.SelectedItem;
            TimeSpan movieLength = TimeSpan.FromMinutes(avgEpisodeLength); // Преобразуем в TimeSpan

            string message = CheckFields(
                txtName.Text,
                txtDescription.Text,
                GenresList,
                AuthorsList,
                comboStatus.SelectedItem,
                txtAvgEpisodeLength.Text,
                txtEpisodesCount.Text
                );

            if (message.Length == 0)
            {
                int id = 0;
                if (NewMovie != null) id = NewMovie.Id;

                NewMovie = new Series(id, movieName, description, GenresList, AuthorsList, statusSelected, Convert.ToInt32(episodesCount),movieLength);
                //MessageBox.Show($"private void Save_Click: NewMovie= {NewMovie.ToString()}");
                Close();

            }
            else MessageBox.Show($"Errors: \n\n{message}");
        }

        private string CheckFields(string txtName, string txtDescription, List<Genre> genresList, List<Author> authorsList, object selectedItem, string txtAvgEpisodeLength, string txtEpisodesCount)
        {
            StringBuilder sb = new();

            if (txtName.Length == 0) sb.Append("Movie name cannot be empty").Append("\n");
            //if(txtDescription.Length == 0)
            if (genresList.Count == 0) sb.Append("Genres list is empty").Append("\n");
            if (authorsList.Count == 0) sb.Append("Author list is empty").Append("\n");
            if (selectedItem.Equals(WatchStatus.NotSelected)) sb.Append("Watch status is not selected").Append("\n");

            bool resAvgLength;
            int a;
            resAvgLength = int.TryParse(txtAvgEpisodeLength, out a);
            if (!resAvgLength || a <= 0) sb.Append($"Wrong avg episode length value: {txtAvgEpisodeLength}").Append("\n");

            bool resEpCount;
            int b;
            resEpCount = int.TryParse(txtEpisodesCount, out b);
            if (!resEpCount || b <= 0) sb.Append($"Wrong episodes count value: {txtEpisodesCount}").Append("\n");


            return sb.ToString();
        }

        private void LoadGenresToList()
        {
            GenresListBox.Items.Clear();
            foreach (var g in GenresList)
            {
                GenresListBox.Items.Add(g.Name);
            }
        }
        private void LoadAuthorsToList()
        {
            AuthorsListBox.Items.Clear();
            foreach (var g in AuthorsList)
            {
                AuthorsListBox.Items.Add(g.Name);
            }
        }
    }
}
