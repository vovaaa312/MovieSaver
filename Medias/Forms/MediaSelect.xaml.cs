﻿using MovieSaver.Model;
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
    /// Interaction logic for MediaSelect.xaml
    /// </summary>
    public partial class MediaSelect : Window
    {
        public MediaItem? MediaItem = null;
        public MediaSelect()
        {
            InitializeComponent();
        }

        private void Movie_Click(object sender, RoutedEventArgs e)
        {
            Close();
            var addMovieWindow = new AddMovie();
            addMovieWindow.ShowDialog();
            if (addMovieWindow.NewMovie != null) 
            {
                MediaItem = addMovieWindow.NewMovie;
            }
        }

        private void Series_Click(object sender, RoutedEventArgs e)
        {
            Close();
            var addSeriesWindow = new AddSeries();
            addSeriesWindow.ShowDialog();
            if (addSeriesWindow.NewMovie != null)
            {
                MediaItem = addSeriesWindow.NewMovie;
            }
        }


    }
}
