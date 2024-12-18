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

namespace The_Diamond_Excavator
{
    /// <summary>
    /// Logique d'interaction pour Gagne.xaml
    /// </summary>
    public partial class Gagne : Window
    {
        MediaPlayer sonVictoire;
        public Gagne()
        {
            InitializeComponent();
            InitSon();
        }
        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public static void InitSon()
        {

            MediaPlayer sonVictoire = new MediaPlayer();
            sonVictoire.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "son/Victoire.mp3"));
        }
    }
}
