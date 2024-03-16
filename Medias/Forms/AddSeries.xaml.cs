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

namespace Medias.Forms
{
    /// <summary>
    /// Interaction logic for AddSeries.xaml
    /// </summary>
    public partial class AddSeries : Window
    {
       // private List<Genre> GenresList = new();

        private AuthorsController authorController = new();

        private GenresCotnroller genreController = new();

        private SeasonsController seasonsController = new();

        private ActorsController actorsController = new();
        public Series NewMovie { get; set; }

        public AddSeries()
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));
            comboStatus.SelectedItem = WatchStatus.NotSelected;
        }
        public AddSeries(Series series)
        {


            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));

            NewMovie = (Series)series;
            //NewMovie = new Series();
            //NewMovie.Id = series.Id; 

            txtName.Text = series.Name;
            txtDescription.Text = series.Description;
            txtRating.Text = series.Rating.ToString();

            // GenresList = series.Genres;

            comboStatus.SelectedItem = series.Status;
            //txtAvgEpisodeLength.Text = (series.Duration.TotalMinutes / series.SeriesCount).ToString();
            //txtEpisodesCount.Text = series.SeriesCount.ToString();
            genreController = new(series.Genres);
            seasonsController = new(series.Seasons);
            actorsController = new(series.Actors);
            authorController = new( series.Authors); 
            LoadGenresToList();
            LoadAuthorsToList();
            LoadSeasonsToList();
            LoadActorsToList();

        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)GenresComboBox.SelectedItem;

            if (selectedItem == null)
            {
                ShowErrorDialog("Select a genre from the dropdown list.");
                return;
            }

            try
            {
                genreController.Add(new Genre(selectedItem.Content.ToString()));

                //update list
                LoadGenresToList();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex);
            }

        }
        private void DeleteGenre_Click(object sender, RoutedEventArgs e)
        {
            if (canDeleteGenre())DeleteGenre((string)GenresListBox.SelectedItem);


            
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
            if (authorController.Count ()== 1)
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
            Save();
        }
        private void AddSeason_Click(object sender, RoutedEventArgs e)
        {
            string num = txtNumber.Text;
            string name = txtSeasonName.Text;
            string description = txtSeasonDescription.Text;
            string episodes = txtEpisodesCount.Text;
            string message = checkSeasonFields(num, name, description, episodes);

            if (message.Length != 0)
            {
                ShowErrorDialog(message);
                return;
            }
            Season season = new Season(Convert.ToInt32(num), name, description, Convert.ToInt32(episodes));

            try
            {
                seasonsController.Add(season);
                LoadSeasonsToList();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex.Message.ToString());
            }

        }
        private void DeleteSeason_Click(object sender, RoutedEventArgs e)
        {
            if (canDeleteSeason())
            {
                DeleteSeason((Season)SeasonsListBox.SelectedItem);
            }
        }
        private bool canDeleteSeason()
        {
            if (seasonsController.IsEmpty())
            {
                ShowErrorDialog("Seasons list is empty.");
                return false;
            }

            if (SeasonsListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select season to delete.");
                return false;
            }
            return true;
        }
        private void ClearSeasons_Click(object sender, RoutedEventArgs e)
        {
            if (seasonsController.IsEmpty())
            {
                MessageBox.Show($"Season list is empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {

                MessageBoxResult result = MessageBox.Show($"Are you sure you want to clear seasons list?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Check dialog result
                if (result == MessageBoxResult.Yes)
                {
                    // Delete item if user confirm
                    seasonsController.Clear();
                    LoadSeasonsToList();
                }
            }

        }
        private void AddActor_Click(object sender, RoutedEventArgs e)
        {
            var addActorWindow = new AddActor();
            addActorWindow.ShowDialog();
            if (addActorWindow.NewActor != null)
            {
                //MediaItem = addMovieWindow.NewMovie;
                //MessageBox.Show($"{addActorWindow.NewActor.Name} {addActorWindow.NewActor.CharacterName}");
                //ActorsList.Add(addActorWindow.NewActor);
                actorsController.Add(addActorWindow.NewActor);
                LoadActorsToList();
            }
        }
        private void DeleteActor_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show(ActorsListBox.SelectedItem.ToString());
            if (canDeleteActor()) DeleteActor((Actor)ActorsListBox.SelectedItem);

        }
        private bool canDeleteActor()
        {
            if (actorsController.IsEmpty())
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
            actorsController.Delete(selectedItem);
            LoadActorsToList();
        }
        private void DeleteSeason(Season selectedItem)
        {
            seasonsController.Remove(selectedItem);
            LoadSeasonsToList();
        }
        private void Save()
        {

            string movieName = txtName.Text;
            string description = txtDescription.Text;
            WatchStatus watchStatus = (WatchStatus)comboStatus.SelectedItem;
            List<Season>? seasons = seasonsController.Seasons;
            List<Genre>? genres = genreController.Genres;
            List<Author>? authors = authorController.Authors;
            List<Actor>? actors = actorsController.Actors;
            string _rating = txtRating.Text;

            string message = CheckFields(
                movieName,
                description,
                genreController.IsEmpty(),
                authorController.IsEmpty(),
                seasonsController.IsEmpty(),
                actorsController.IsEmpty(),
                _rating,
                watchStatus
                );

            if (message.Length != 0)
            {
                ShowErrorDialog(message);
                return;
            }

            int id = 0;
            if (NewMovie != null) id = NewMovie.Id;
            int rating = Int32.Parse(_rating);
            NewMovie = new Series(id, movieName, rating, description, genres, authors,actors, watchStatus, seasons);
            MessageBox.Show(NewMovie.ToString());
            Close();

        }
        private string CheckFields(string txtName, string txtDescription, bool genresEmpty, bool authorsEmpty,bool seasonsEmpty, bool actorsEmpty, string rating, object selectedItem)
        {
            StringBuilder sb = new();

            if (txtName.Length == 0) sb.Append("Movie name cannot be empty").Append("\n");
            //if(txtDescription.Length == 0)
            if (genresEmpty) sb.Append("Genres list is empty").Append("\n");
            if (authorsEmpty) sb.Append("Authors list is empty").Append("\n");
            if (seasonsEmpty) sb.Append("Seasons list is empty").Append("\n");
            if (actorsEmpty) sb.Append("Actors list is empty").Append("\n");

            if (selectedItem.Equals(WatchStatus.NotSelected)) sb.Append("Watch status is not selected").Append("\n");

            bool resAvgLength;
            int a;
            resAvgLength = int.TryParse(rating, out a);
            if (!resAvgLength || a <= 0) sb.Append($"Wrong rating value: {rating}").Append("\n");


            return sb.ToString();
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
            IEnumerable<Actor> actors;
            if(actorsController.IsEmpty())actors = new ObservableCollection<Actor>();
            else actors =
                    new ObservableCollection<Actor>(actorsController.Actors);

            ActorsListBox.ItemsSource = actors;
        }
        private void LoadSeasonsToList()
        {
            SeasonsListBox.ItemsSource = new ObservableCollection<Season>(seasonsController.Seasons);
        }
        private string checkSeasonFields(string id, string seasonName, string seasonDescription, string episodesCount)
        {
            StringBuilder sb = new();
            if (seasonName.Length == 0 || seasonName is null) sb.Append("Season name cannot be empty.").Append("\n");

            bool num;
            int a;
            num = int.TryParse(id, out a);
            if (!num || a <= 0) sb.Append($"Wrong season number value: {id}.").Append("\n");


            bool epCount;
            int b;
            epCount = int.TryParse(episodesCount, out b);
            if (!epCount || b <= 0) sb.Append($"Wrong episodes count value: {episodesCount}.").Append("\n");


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


    }
}
