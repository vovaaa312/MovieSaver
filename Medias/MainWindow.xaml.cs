﻿using Medias.Forms;
using MovieSaver.Controller;
using MovieSaver.Model;
using System.Collections.ObjectModel;
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
                    0,
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

            controller.AddMovie(
             new Series(
            0,
            "Series1",
            "Good series",
            new List<Genre>() {
                new Genre("action"),new Genre("comedy")
                             },
             new List<Author>()
                             {
                new Author("John Doe")
                             },
             WatchStatus.WATCHED, 15,
            TimeSpan.FromMinutes(30)

         ));

            InitializeComponent();
           // dataGrid.ItemsSource = new ObservableCollection<MediaItem>();

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

                //MessageBox.Show($"Edit item clicked.\nItem={selectedItem.ToString()}");

                if (selectedItem is Movie) EditMovieItem(selectedItem);
                else if (selectedItem is Series) EditSeriesItem(selectedItem);
                else MessageBox.Show("Error: selected item is not movie or series");

            }
            else MessageBox.Show("Select item to edit.");
        }

        private void EditMovieItem(MediaItem selectedItem)
        {
            var editMovieWindow = new AddMovie((Movie)selectedItem);
            editMovieWindow.ShowDialog();
            MediaItem mi = editMovieWindow.NewMovie;

            bool itemsEqual = selectedItem.Equals(editMovieWindow.NewMovie);

            //if (!mi.Equals(editMovieWindow.NewMovie)) 
            if (!itemsEqual)
            {
                MessageBox.Show("!selectedItem.Equals(editMovieWindow.NewMovie)");
                controller.EditMovie(mi);
                LoadMoviesToDataGrid();
            }
        }

        private void EditSeriesItem(MediaItem selectedItem)
        {
            MessageBox.Show("Edit series was clicked");
            var editSeriesWindow = new AddSeries((Series)selectedItem);
            editSeriesWindow.ShowDialog();
            Series mi = editSeriesWindow.NewMovie;

            bool itemsEqual = selectedItem.Equals(editSeriesWindow.NewMovie);

            //if (!mi.Equals(editMovieWindow.NewMovie)) 
            if (!itemsEqual)
            {
                controller.EditMovie(mi);
                LoadMoviesToDataGrid();
            }

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
            if (!controller.isEmpty())
            {
                controller.ClearAll();
                LoadMoviesToDataGrid();
            }
            else MessageBox.Show("Movie list is empty.");

        }
        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("DataGrid_MouseDoubleClicked");
            if (sender is DataGrid dataGrid)
            {
                if (dataGrid.SelectedItem != null)
                {
                    dataGrid.BeginEdit();
                }
            }
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
            
            dataGrid.ItemsSource = new ObservableCollection<MediaItem>(controller.Movies);


            //foreach (var movie in controller.Movies)
            //{
            //    dataGrid.Items.Add(movie);
            //}

        }

        //private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        //{
        //    //// Получите элемент DataGridRow, на который был совершен двойной щелчок
        //    //DataGridRow row = sender as DataGridRow;

        //    //// Проверьте, что щелчок произошел на реальной строке данных, а не на заголовке или пустой области
        //    //if (row != null && row.Item != null)
        //    //{
        //    //    // Получите объект, связанный с этой строкой данных (в вашем случае это MediaItem)
        //    //    MediaItem selectedItem = row.Item as MediaItem;

        //    //    // Выполните необходимые действия с выбранным элементом (например, редактирование)
        //    //    EditMediaItem(selectedItem);
        //    //}

        //    MessageBox.Show("Item double clicked");
        //}

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid grid && grid.SelectedItem != null)
            {
                // Приведение выбранного элемента к вашему типу данных, если это необходимо
                var item = (MediaItem)grid.SelectedItem;

                // Выполнение действия с выбранным элементом
                MessageBox.Show($"Двойной клик на элемент: {item.ToString()}");
            }
        }

    }
}
