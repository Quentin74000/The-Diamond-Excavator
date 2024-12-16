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
        public static double VOLUME_MUSIQUE;
        public static double VOLUME_SON;

        public MenuJeu()
        {
            InitializeComponent();
            MainWindow.InitMusique();
            MainWindow.InitSon();
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
            VOLUME_SON = sliderVolumeSonMenuJeu.Value;
            labVolumeSonMenuJeu.Content = ("Volume Son: " + VOLUME_SON + "%");
            MainWindow.VolumeSon(VOLUME_SON);
        }
        public void sliderVolumeMusiqueMenuJeu_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VOLUME_MUSIQUE = sliderVolumeMusiqueMenuJeu.Value;
            labVolumeMusiqueMenuJeu.Content = ("Volume Musique: " + VOLUME_MUSIQUE + "%");
            MainWindow.VolumeMusique(VOLUME_MUSIQUE);
        }
    }
}