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
        private static MediaPlayer musiqueFond, musiqueSon;
        private double volumeMusique;

        public MenuJeu()
        {
            InitializeComponent();
            InitMusique();
            InitSon();
        }
        public void InitMusique()
        {
            musiqueFond = new MediaPlayer();
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "son/MusiqueFond.mp3"));
            musiqueFond.MediaEnded += RelanceMusique;
            musiqueFond.Play();
            musiqueFond.Volume = 0.1;
        }
        public void InitSon()
        {
            musiqueSon = new MediaPlayer();
            musiqueSon.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "son/musiqueSon.mp3"));
            musiqueSon.MediaEnded += RelanceSon;
            musiqueSon.Play();
            musiqueSon.Volume = 0.3;
        }
        public void VolumeMusique(double volumeMusique)
        {
            if (musiqueFond != null)
            {
                musiqueFond.Volume = volumeMusique / 100; 
            }
        }
        public void VolumeSon(double VolumeSon)
        {
            if ( musiqueSon != null)
            {
                musiqueSon.Volume = VolumeSon / 100;
            }
        }
        public static void RelanceMusique(object? sender, EventArgs e)
        {
            musiqueFond.Position = TimeSpan.Zero;
            musiqueFond.Play();
        }
        public static void RelanceSon(object? sender, EventArgs e)
        {
            musiqueSon.Position = TimeSpan.Zero;
            musiqueSon.Play();
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
            double valeurSliderSon = sliderVolumeSonMenuJeu.Value;
            labVolumeSonMenuJeu.Content = ("Volume Son: " + valeurSliderSon + "%");
            VolumeSon(valeurSliderSon);
        }
        public void sliderVolumeMusiqueMenuJeu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double valeurSliderMusique = sliderVolumeMusiqueMenuJeu.Value;
            labVolumeMusiqueMenuJeu.Content = ("Volume Musique: " + valeurSliderMusique + "%");
            VolumeMusique(valeurSliderMusique);
        }
    }
}