using Medias.Controller;
using Medias.Model;
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
        private List<Genre> GenresList = new();

        private List<Author> AuthorsList = new();

        private SeasonsController seasonsController = new();
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
            GenresList = series.Genres;
            AuthorsList = series.Authors;
            comboStatus.SelectedItem = series.Status;
            //txtAvgEpisodeLength.Text = (series.Duration.TotalMinutes / series.SeriesCount).ToString();
            //txtEpisodesCount.Text = series.SeriesCount.ToString();

            seasonsController = new(series.Seasons);
            LoadGenresToList();
            LoadAuthorsToList();
            LoadSeasonsToList();

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
            Save();
        }
        private void AddSeason_Click(object sender, RoutedEventArgs e)
        {
            string name = txtSeasonName.Text;
            string description = txtSeasonDescription.Text;
            string episodes = txtEpisodesCount.Text;
            string message = checkSeasonFields(name, description, episodes);

            if (message.Length != 0)
            {
                ShowErrorDialog(message);
                return;
            }
            Season season = new Season(0, name, description, Convert.ToInt32(episodes));
            seasonsController.AddSeason(season);
            LoadSeasonsToList();
        }
        private void DeleteSeason_Click(object sender, RoutedEventArgs e)
        {
            if (seasonsController.IsEmpty())
            {
                ShowErrorDialog("Seasons list is empty.");
                return;
            }

            if (SeasonsListBox.SelectedItem == null)
            {
                ShowErrorDialog("Select season to delete.");
                return;
            }

            Season selectedItem = (Season)SeasonsListBox.SelectedItem;

            DeleteSeason(selectedItem);

            //bool confirm = ShowConfirmationDialog($"Are you sure you want to delete '{selectedItem.Name}'?");
            //// Check dialog result
            //if (confirm)
            //{
            //    // Delete item if user confirm
            //    DeleteSeason(selectedItem);
            //}
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
        private void DeleteSeason(Season selectedItem)
        {
            seasonsController.DeleteSeason(selectedItem);
            LoadSeasonsToList();
        }
        private void Save()
        {

            string movieName = txtName.Text;
            string description = txtDescription.Text;
            WatchStatus watchStatus = (WatchStatus)comboStatus.SelectedItem;
            List<Season>? seasons = seasonsController.Seasons;
            List<Genre>? genres = GenresList;
            List<Author>? authors = AuthorsList;

            string message = CheckFields(
                movieName,
                description,
                genres,
                authors,
                seasons,
                watchStatus
                );

            if (message.Length != 0)
            {
                ShowErrorDialog(message);
                return;
            }

            int id = 0;
            if (NewMovie != null) id = NewMovie.Id;

            NewMovie = new Series(id, movieName, description, genres, authors, watchStatus, seasons);
            //MessageBox.Show($"private void Save_Click: NewMovie= {NewMovie.ToString()}");
            Close();


            //if (message.Length == 0)
            //{
            //    int id = 0;
            //    if (NewMovie != null) id = NewMovie.Id;

            //    NewMovie = new Series(id, movieName, description, GenresList, AuthorsList, statusSelected, Convert.ToInt32(episodesCount), movieLength);
            //    //MessageBox.Show($"private void Save_Click: NewMovie= {NewMovie.ToString()}");
            //    Close();

            //}
            //else MessageBox.Show($"Errors: \n\n{message}");
            MessageBox.Show("Save was clicked");
        }
        private string CheckFields(string txtName, string txtDescription, List<Genre>? genresList, List<Author>? authorsList, List<Season>? seasonsList, object selectedItem)
        {
            StringBuilder sb = new();

            if (txtName.Length == 0) sb.Append("Movie name cannot be empty").Append("\n");
            //if(txtDescription.Length == 0)
            if (genresList.Count == 0) sb.Append("Genres list is empty").Append("\n");
            if (authorsList.Count == 0) sb.Append("Authors list is empty").Append("\n");
            if (seasonsList.Count == 0) sb.Append("Seasons list is empty").Append("\n");

            if (selectedItem.Equals(WatchStatus.NotSelected)) sb.Append("Watch status is not selected").Append("\n");

            //bool resAvgLength;
            //int a;
            //resAvgLength = int.TryParse(txtAvgEpisodeLength, out a);
            //if (!resAvgLength || a <= 0) sb.Append($"Wrong avg episode length value: {txtAvgEpisodeLength}").Append("\n");

            //bool resEpCount;
            //int b;
            //resEpCount = int.TryParse(txtEpisodesCount, out b);
            //if (!resEpCount || b <= 0) sb.Append($"Wrong episodes count value: {txtEpisodesCount}").Append("\n");


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
        private void LoadSeasonsToList()
        {
            SeasonsListBox.ItemsSource = new ObservableCollection<Season>(seasonsController.Seasons);

            //SeasonsListBox.Items.Clear();
            //foreach (var g in seasonsController.Seasons)
            //{
            //    SeasonsListBox.Items.Add(g.ToString());
            //}
        }
        private string checkSeasonFields(string seasonName, string seasonDescription, string episodesCount)
        {
            StringBuilder sb = new();
            if (seasonName.Length == 0 || seasonName is null) sb.Append("Season name cannot be empty").Append("\n");

            bool epCount;
            int a;
            epCount = int.TryParse(episodesCount, out a);
            if (!epCount || a <= 0) sb.Append($"Wrong avg episode length value: {episodesCount}").Append("\n");


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
