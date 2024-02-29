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

        //private List<Season> SeasonList = new();

        private SeasonsController seasonsController = new();
        public Series NewMovie { get; set; }

        public AddSeries()
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));
            comboStatus.SelectedItem = WatchStatus.NotSelected;
        }
        //public AddSeries(Series series)
        //{

        //    InitializeComponent();
        //    comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));

        //    NewMovie = (Series)series;
        //    //NewMovie = new Series();
        //    //NewMovie.Id = series.Id; 

        //    txtName.Text = series.Name;
        //    txtDescription.Text = series.Description;
        //    GenresList = series.Genres;
        //    AuthorsList = series.Authors;
        //    comboStatus.SelectedItem = series.Status;
        //    txtAvgEpisodeLength.Text = (series.Duration.TotalMinutes / series.SeriesCount).ToString();
        //    txtEpisodesCount.Text = series.SeriesCount.ToString();

        //    LoadGenresToList();
        //    LoadAuthorsToList();

        //}



        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)GenresComboBox.SelectedItem;
            
            if (selectedItem == null)
            {
                ShowError("Select a genre from the dropdown list.");
                return;
            }

            string genreName = selectedItem.Content.ToString();
            if (GenresList.Any(g => g.Name == genreName))
            {
                ShowError($"Genre '{genreName}' already  in the list.");
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
                ShowError("Genre list is empty.");
                return;
            }
            if (GenresListBox.SelectedItem == null)
            {
                ShowError("Please select a genre to delete.");
                return;
            }
            if (GenresList.Count == 1)
            {
                ShowError("Movie must have at least 1 genre.");
                return;
            }


            if (GenresList.Count == 0)
            {
                ShowError("Genre list is empty.");
                return;
            }

            string selectedGenre = (string)GenresListBox.SelectedItem;
            GenresList.RemoveAll(g => g.Name == selectedGenre);
            LoadGenresToList();


        }
        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            //string authName = AuthorNameTextBox.Text;
            //if (authName.Length > 0)
            //{
            //    Author auth = new(authName);

            //    if (!AuthorsList.Any(g => g.Name == authName))  // if new author not in the list...
            //    {
            //        //then add new author to list
            //        AuthorsList.Add(auth);
            //        //update list
            //        LoadAuthorsToList();
            //    }
            //    else
            //    {
            //        MessageBox.Show("This author already exists in the list.");
            //    }

            //}
            //else MessageBox.Show("Enter author name.");


            string authName = AuthorNameTextBox.Text;
            if (!(authName.Length > 0))
            {
                ShowError("Enter author name.");
                return;
            }

            if (AuthorsList.Any(g => g.Name == authName))
            {
                ShowError($"Autor '{authName}' already in the list.");
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
                ShowError("Author list is empty.");
                return;
            }
            if (AuthorsListBox.SelectedItem == null) 
            {
                ShowError("Select author to delete.");
                return;
            }
            if (AuthorsList.Count == 1)
            {
                ShowError("Movie must have at least 1 author.");
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

            if (message.Length != 0 ) 
            {
                ShowError(message);
                return;
            }
            Season season = new Season(0, name, description, Convert.ToInt32(episodes));
            seasonsController.AddSeason(season);
            LoadSeasonsToList();
        }



        private void DeleteSeason_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Delete was clicked");
            //if (SeasonsListBox.SelectedItem != null)
            //{
            //    // Get selected item
            //    Season selectedItem = (Season)SeasonsListBox.SelectedItem;

            //    // Show dialog
            //    MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedItem.Name}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            //    // Check dialog result
            //    if (result == MessageBoxResult.Yes)
            //    {
            //        // Delete item if user confirm
            //        DeleteSeason(selectedItem);
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("Select an item to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    //MessageBox.Show("Select an item to delete.");
            //}


            if (seasonsController.IsEmpty())
            {
                ShowError("Seasons list is empty.");
                return;
            }

            if (SeasonsListBox.SelectedItem == null) 
            {
                ShowError("Select season to delete.");
                return;
            }

            Season selectedItem = (Season)SeasonsListBox.SelectedItem;

            // Show dialog
            //MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete '{selectedItem.Name}'?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            bool confirm = ShowConfirmationDialog($"Are you sure you want to delete '{selectedItem.Name}'?");
            // Check dialog result
            if (confirm)
            {
                // Delete item if user confirm
                DeleteSeason(selectedItem);
            }
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
            //int avgEpisodeLength; // Используем double для парсинга времени
            //int.TryParse(txtAvgEpisodeLength.Text, out avgEpisodeLength); // Парсим как Double

            //double episodesCount = 0; // Используем double для парсинга времени
            //double.TryParse(txtEpisodesCount.Text, out episodesCount); // Парсим как Double


            //string movieName = txtName.Text;
            //string description = txtDescription.Text;
            //WatchStatus statusSelected = (WatchStatus)comboStatus.SelectedItem;
            //TimeSpan movieLength = TimeSpan.FromMinutes(avgEpisodeLength); // Преобразуем в TimeSpan

            //string message = CheckFields(
            //    txtName.Text,
            //    txtDescription.Text,
            //    GenresList,
            //    AuthorsList,
            //    comboStatus.SelectedItem,
            //    txtAvgEpisodeLength.Text,
            //    txtEpisodesCount.Text
            //    );

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

        private void ShowError(string message)
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
