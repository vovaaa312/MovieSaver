using Medias.Model;
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
    /// Interaction logic for AddActor.xaml
    /// </summary>
    /// 

    public partial class AddActor : Window
    {
        public Actor NewActor { get; set; } = null;
        public AddActor()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string actorName = txtName.Text;
            string characterName = txtCharacterName.Text;
            string message = check(actorName, characterName);
            if (!(message.Length == 0))
            {
                ShowErrorDialog(message);
                return;
            }

            NewActor = new Actor(actorName, characterName);
            Close();
        }

        private string check(string actorName, string characterName)
        {
            StringBuilder sb = new StringBuilder();
            if (actorName == "") sb.Append("Actor name cannot be empty.").Append("\n");
            else if (actorName.Length < 2) sb.Append($"Actor name must contain more than 2 characters: {actorName}").Append("\n");

            if (characterName == "") sb.Append("Character name name cannot be empty.").Append("\n");
            else if (characterName.Length < 2) sb.Append($"Character name must contain more than 2 characters: {characterName}").Append("\n");

            return sb.ToString();
        }

        private void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
