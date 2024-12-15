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
        private static MediaPlayer musiqueFond;
        private double volume;

        public MenuJeu()
        {
            InitializeComponent();
        }
        public void InitMusique()
        {
            musiqueFond = new MediaPlayer();
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "son/MusiqueFond.mp3"));
            musiqueFond.MediaEnded += RelanceMusique;
            musiqueFond.Play();
        }
        public void Volume(double volume)
        {
            musiqueFond.Volume = volume / 100;
        }
        public static void RelanceMusique(object? sender, EventArgs e)
        {
            musiqueFond.Position = TimeSpan.Zero;
            musiqueFond.Play();
        }
        private void butJouer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void butQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void sliderVolumeSonMenuJeu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double valeurSliderMusique = 0;
            valeurSliderMusique = sliderVolumeSonMenuJeu.Value;
            labVolumeSonMenuJeu.Content = ("Volume Son: " + valeurSliderMusique + "%");
        }
        public void sliderVolumeMusiqueMenuJeu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double valeurSliderMusique = 0.5 ;
            valeurSliderMusique = sliderVolumeMusiqueMenuJeu.Value;
            labVolumeMusiqueMenuJeu.Content = ("Volume Musique: " + valeurSliderMusique + "%");
            if (musiqueFond != null)
            {
                InitMusique();
            }
            musiqueFond.Volume = valeurSliderMusique;
            //Volume(valeurSliderMusique);
            Console.WriteLine(valeurSliderMusique);
        }
    }
}
