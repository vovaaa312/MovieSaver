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
    /// Interaction logic for AddMovie.xaml
    /// </summary>
    public partial class AddMovie : Window
    {
        public AddMovie()
        {
            InitializeComponent();
            comboStatus.ItemsSource = Enum.GetValues(typeof(WatchStatus));

        }

        private void AddGenre_Click(object sender, RoutedEventArgs e)
        {
            var addGenreWindow = new AddGenres();
            addGenreWindow.ShowDialog();
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            var addAuthorsWindow = new AddAuthors();
            addAuthorsWindow.ShowDialog();
        }
    }
}
