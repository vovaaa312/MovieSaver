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
using System.Xml.Linq;

namespace Medias.Forms
{
    /// <summary>
    /// Interaction logic for AddMovie.xaml
    /// </summary>
    public partial class AddMovie : Window
    {
        private List<Genre> GenresList = new();
        private List<Author> AuthorsList = new();

        public Movie NewMovie { get; set; }
        public AddMovie()
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));
            comboStatus.SelectedItem = WatchStatus.NotSelected;
        }
        public AddMovie(Movie movie)
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));

            NewMovie = (Movie)movie;

            txtName.Text = movie.Name;
            txtDescription.Text = movie.Description;
            GenresList = movie.Genres;
            AuthorsList = movie.Authors;
            comboStatus.SelectedItem = movie.Status;
            txtMovieLength.Text = movie.MovieLength.TotalMinutes.ToString();

            LoadGenresToList();
            LoadAuthorsToList();

        }
        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {

            ComboBoxItem selectedItem = (ComboBoxItem)GenresComboBox.SelectedItem;

            if (selectedItem == null)
            {
                ShowErrorDialog("Select a genre from the dropdown list.");
                return;
            }

            string genreName = selectedItem.Content.ToString();
            if (GenresList.Any(g => g.Name == genreName))
            {
                ShowErrorDialog($"Genre '{genreName}' already  in the list.");
                return;
            }

            //add new genre to list
            GenresList.Add(new Genre(genreName));
            //update list
            LoadGenresToList();
        }
        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (GenresList.Count == 0)
            {
                ShowErrorDialog("Genre list is empty.");
                return;
            }
            if (GenresListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select a genre to delete.");
                return;
            }
            if (GenresList.Count == 1)
            {
                ShowErrorDialog("Movie must have at least 1 genre.");
                return;
            }
            string selectedGenre = (string)GenresListBox.SelectedItem;
            GenresList.RemoveAll(g => g.Name == selectedGenre);
            LoadGenresToList();
        }
        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            string authName = AuthorNameTextBox.Text;
            if (!(authName.Length > 0))
            {
                ShowErrorDialog("Enter author name.");
                return;
            }

            if (AuthorsList.Any(g => g.Name == authName))
            {
                ShowErrorDialog($"Autor '{authName}' already in the list.");
                return;
            }

            Author auth = new(authName);
            // add new author to list
            AuthorsList.Add(auth);
            //update list
            LoadAuthorsToList();
            //clear text box
            AuthorNameTextBox.Clear();
        }
        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (AuthorsList.Count == 0)
            {
                ShowErrorDialog("Author list is empty.");
                return;
            }
            if (AuthorsListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select author to delete.");
                return;
            }
            if (AuthorsList.Count == 1)
            {
                ShowErrorDialog("Movie must have at least 1 author.");
                return;
            }

            string selectedAuthor = (string)AuthorsListBox.SelectedItem;
            AuthorsList.RemoveAll(g => g.Name == selectedAuthor);
            LoadAuthorsToList();
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            double length; // Используем double для парсинга времени
            double.TryParse(txtMovieLength.Text, out length); // Парсим как Double

            string movieName = txtName.Text;
            string description = txtDescription.Text;
            WatchStatus watchStatus = (WatchStatus)comboStatus.SelectedItem;
             // Преобразуем в TimeSpan
            string movieLength = txtMovieLength.Text;
            List<Genre>? genres = GenresList;
            List<Author>? authors = AuthorsList;

            string message = CheckFields(
                movieName,
                description,
                genres,
                authors,
                watchStatus,
                movieLength
                );


            if (message.Length != 0)
            {
                ShowErrorDialog(message);
                return;
            }


            int id = 0;
            if (NewMovie != null) id = NewMovie.Id;
            TimeSpan movieLengthTS = TimeSpan.FromMinutes(length);
            NewMovie = new Movie(id, movieName, description, genres, authors, watchStatus, movieLengthTS);
            Close();


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
        private string CheckFields(string txtName, string txtDescription, List<Genre> genresList, List<Author> authorsList, object selectedItem, string txtMovieLength)
        {
            StringBuilder sb = new();

            if (txtName.Length == 0) sb.Append("Movie name cannot be empty").Append("\n");
            //if(txtDescription.Length == 0)
            if (genresList.Count == 0) sb.Append("Genres list is empty").Append("\n");
            if (authorsList.Count == 0) sb.Append("Author list is empty").Append("\n");
            if (selectedItem.Equals(WatchStatus.NotSelected)) sb.Append("Watch status is not selected").Append("\n");

            bool res;
            int a;
            res = int.TryParse(txtMovieLength, out a);

            if (!res || a <= 0) sb.Append($"Wrong movie length value: {txtMovieLength}").Append("\n");

            return sb.ToString();
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
