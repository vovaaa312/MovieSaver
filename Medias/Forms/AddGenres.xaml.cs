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
    /// Interaction logic for AddGenres.xaml
    /// </summary>
    public partial class AddGenres : Window
    {
        // public bool SaveChange { get; }
        public List<Genre> GenresList { get; set; }
        public AddGenres()
        {           
            //if (AddMovie.Genres.Count != 0) GenresList = AddMovie.Genres;
            GenresList = new List<Genre>();  // init genres list
            InitializeComponent();

        }
        public AddGenres(List<Genre> genres)
        {
            GenresList = genres;  // init genres list
            InitializeComponent();
            LoadGenresToList();
        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {

            if (GenresComboBox.SelectedItem != null)
            {
                //get selected item
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
            else
            {
                MessageBox.Show("Please select a genre.");
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

        private void LoadGenresToList()
        {
            GenresListBox.Items.Clear();
            foreach (var g in GenresList)
            {
                GenresListBox.Items.Add(g.Name);
            }

        }
        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }

        
    }
}
