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
    /// Logique d'interaction pour Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        //public static MediaPlayer musiqueFond, musiqueSon; // pour initialiser la musique
        public static double VOLUME_MUSIQUE;
        public static double VOLUME_SON;
        public static int NB_TOUR = 0;

        public Options()
        {
            InitializeComponent();
            if(NB_TOUR == 0)
            {
                MainWindow.InitMusique();
                MainWindow.InitSon();
            }
            NB_TOUR++;
        }
        private void butValider_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void sliderVolumeSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.musiqueSon != null)
            {
                VOLUME_SON = sliderVolumeSon.Value;
                labVolumeSon.Content = ("Volume Son: " + VOLUME_SON + "%");
                MainWindow.VolumeSon(VOLUME_SON);
            }
        }

        private void sliderVolumeMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.musiqueFond != null)
            {
                VOLUME_MUSIQUE = sliderVolumeMusique.Value;
                labVolumeMusique.Content = ("Volume Son: " + VOLUME_MUSIQUE + "%");
                MainWindow.VolumeMusique(VOLUME_MUSIQUE);
            }
        }
    }
}
