using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
    /// Logique d'interaction pour Menu_Jeux.xaml
    /// </summary>
    public partial class MenuJeu : Window
    {
        private static SoundPlayer sonDeFond;
        private static MediaPlayer musique;
        public MenuJeu()
        {
            InitializeComponent();
            SonDeFond();
        }

        private void butJouer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void butQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void sliderVolumeSonMenuJeu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string valeurSliderMusique = "";
            valeurSliderMusique = sliderVolumeSonMenuJeu.Value.ToString();
            labVolumeSonMenuJeu.Content = ("Volume Son: " + valeurSliderMusique + "%");
        }

        private void sliderVolumeMusiqueMenuJeu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string valeurSliderMusique = "";
            valeurSliderMusique = sliderVolumeMusiqueMenuJeu.Value.ToString();
            labVolumeMusiqueMenuJeu.Content = ("Volume Musique: " + valeurSliderMusique + "%");
          
        }
        private void SonDeFond()
        {
            sonDeFond = new SoundPlayer(Application.GetResourceStream(new Uri("pack://application:,,,/The Diamond Excavator/son/MusiqueFond.wav")).Stream);

            // Lire le fichier en arrière-plan (asynchrone)
            sonDeFond.PlayLooping();  // Pour lire en boucle
                                      // player.Play();      // Pour lire une seule fois

        }
        private void RelanceMusique(object? sender, EventArgs e)
        {
            musique.Position = TimeSpan.Zero;
            musique.Play();
        }

    }
}
