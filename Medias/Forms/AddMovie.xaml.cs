using Medias.Controller;
using Medias.Model;
using Medias.Model.Enum;
using MovieSaver.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private GenresCotnroller genreController = new();
        private AuthorsController authorController = new();
        private ActorsController actorController = new();

        public bool SaveClicked { get; private set; } = false;

        public Movie NewMovie { get; set; }

        private Movie movieCopy;
        public AddMovie()
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));
            comboStatus.SelectedItem = WatchStatus.NotSelected;
            // Closing += AddMovie_Closing;

        }
        public AddMovie(Movie movie)
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));


            int id = movie.Id;
            string name = movie.Name;
            int rating = movie.Rating;
            string description = movie.Description;
            List<Genre> genres = movie.Genres;
            List<Author> authors = movie.Authors;
            List<Actor> actors = movie.Actors;
            WatchStatus status = movie.Status;
            TimeSpan movieLength = movie.MovieLength;

            //NewMovie = (Movie)movie;
            NewMovie = new Movie(
               id,
              name,
               rating,
               description,
             genres,
              authors,
              actors,
               status,
              movieLength
               );

            movieCopy = new Movie(
                  id,
              name,
               rating,
               description,
             genres,
              authors,
              actors,
               status,
               movieLength
               );

            txtName.Text = NewMovie.Name;
            txtDescription.Text = NewMovie.Description;

            genreController = new(genres);
            actorController = new(actors);
            authorController = new(authors);

            comboStatus.SelectedItem = movie.Status;
            txtMovieLength.Text = movie.MovieLength.TotalMinutes.ToString();
            txtRating.Text = movie.Rating.ToString();

            LoadGenresToList();
            LoadAuthorsToList();
            LoadActorsToList();
            Closing += AddMovie_Closing;

            // movieCopy.Id = movie.Id;


            //movieCopy.Name = movie.Name;
            //movieCopy.Description = movie.Description;
            //movieCopy.Actors = movie.Actors;
            //movieCopy.Genres = movie.Genres;
            //movieCopy.Authors = movie.Authors; 
            //movieCopy.Status = movie.Status;
            //movieCopy.Rating = movie.Rating;


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
            //if (GenresList.Any(g => g.Name == genreName))


            if (genreController.Contains(new Genre(genreName)))
            {
                ShowErrorDialog($"Genre '{genreName}' already  in the list.");
                return;
            }

            //add new genre to list
            genreController.Add(new Genre(genreName));
            //update list
            LoadGenresToList();
        }
        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {


            if (canDeleteGenre()) DeleteGenre((string)GenresListBox.SelectedItem);

        }

        private void DeleteGenre(string selectedGenre)
        {
            //GenresList.RemoveAll(g => g.Name == selectedGenre);
            try
            {
                genreController.Remove(new Genre(selectedGenre));
                LoadGenresToList();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex);
            }

        }

        private bool canDeleteGenre()
        {
            if (genreController.IsEmpty())
            {
                ShowErrorDialog("Genre list is empty.");
                return false;
            }
            if (GenresListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select a genre to delete.");
                return false;
            }
            if (genreController.Count() == 1)
            {
                ShowErrorDialog("Movie must have at least 1 genre.");
                return false;
            }
            return true;
        }
        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {


            string authName = AuthorNameTextBox.Text;
            if (!(authName.Length > 0))
            {
                ShowErrorDialog("Enter author name.");
                return;
            }

            Author auth = new(authName);

            try
            {
                // add new author to list
                authorController.Add(auth);
                //clear text box
                AuthorNameTextBox.Clear();
                //update list
                LoadAuthorsToList();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex);
            }
        }
        private void DeleteAuthor_Click(object sender, RoutedEventArgs e)
        {


            if (canDeleteAuthor()) DeleteAuthor((string)AuthorsListBox.SelectedItem);

        }

        private bool canDeleteAuthor()
        {
            if (authorController.IsEmpty())
            {
                ShowErrorDialog("Author list is empty.");
                return false;
            }
            if (AuthorsListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select author to delete.");
                return false;
            }
            if (authorController.Count() == 1)
            {
                ShowErrorDialog("Movie must have at least 1 author.");
                return false;
            }
            return true;
        }

        private void DeleteAuthor(string author)
        {
            //AuthorsList.RemoveAll(g => g.Name == author);
            try
            {

                authorController.Remove(new Author(author));
                LoadAuthorsToList();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex);
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            double length;
            double.TryParse(txtMovieLength.Text, out length);

            string movieName = txtName.Text;
            string description = txtDescription.Text;
            WatchStatus watchStatus = (WatchStatus)comboStatus.SelectedItem;
            // convert to TimeSpan
            string movieLength = txtMovieLength.Text;
            List<Genre>? genres = genreController.Genres;
            List<Author>? authors = authorController.Authors;
            List<Actor>? actors = actorController.Actors;
            string _rating = txtRating.Text;


            string message = CheckFields(
                movieName,
                description,
                genreController.IsEmpty(),
                authorController.IsEmpty(),
                actorController.IsEmpty(),
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
            int rating = Int32.Parse(_rating);

            NewMovie = new Movie(id, movieName, rating, description, genres, authors, actors, watchStatus, movieLengthTS);
            MessageBox.Show($"NewMovie = {NewMovie}", "AddMovie.Save_Click");
            SaveClicked = true;
            Close();


        }
        private void LoadGenresToList()
        {
            GenresListBox.Items.Clear();
            foreach (var g in genreController.Genres)
            {
                GenresListBox.Items.Add(g.Name);
            }
        }
        private void LoadAuthorsToList()
        {
            AuthorsListBox.Items.Clear();
            foreach (var g in authorController.Authors)
            {
                AuthorsListBox.Items.Add(g.Name);
            }
        }
        private void LoadActorsToList()
        {
            ActorsListBox.ItemsSource = new ObservableCollection<Actor>(actorController.Actors);
        }
        private string CheckFields(string txtName, string txtDescription, bool genresEmpty, bool authorsEmpty, bool actorsEmpty, object selectedItem, string txtMovieLength)
        {
            StringBuilder sb = new();

            if (txtName.Length == 0) sb.Append("Movie name cannot be empty").Append("\n");
            //if(txtDescription.Length == 0)
            if (genresEmpty) sb.Append("Genres list is empty").Append("\n");
            if (authorsEmpty) sb.Append("Authors list is empty").Append("\n");
            if (actorsEmpty) sb.Append("Actors list is empty").Append("\n");

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

        private void ShowErrorDialog(Exception ex)
        {
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private bool ShowConfirmationDialog(string message)
        {
            MessageBoxResult result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        private void AddActor_Click(object sender, RoutedEventArgs e)
        {
            var addActorWindow = new AddActor();
            addActorWindow.ShowDialog();
            if (addActorWindow.NewActor != null)
            {

                actorController.Add(addActorWindow.NewActor);
                LoadActorsToList();
            }
        }

        private void DeleteActor_Click(object sender, RoutedEventArgs e)
        {
            if (canDeleteActor()) DeleteActor((Actor)ActorsListBox.SelectedItem);

        }
        private bool canDeleteActor()
        {
            if (actorController.IsEmpty())
            {
                ShowErrorDialog("Actors list is empty.");
                return false;
            }
            if (ActorsListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select actor to delete.");
                return false;
            }
            return true;

        }
        private void DeleteActor(Actor selectedItem)
        {
            actorController.Delete(selectedItem);
            LoadActorsToList();
        }


        private void AddMovie_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //MessageBox.Show(movieCopy.ToString(), "AddMovie.xaml.cs.AddMovie_Closing") ;
            if (SaveClicked == false) NewMovie = movieCopy;
            SaveClicked = false;

        }
    }
}
