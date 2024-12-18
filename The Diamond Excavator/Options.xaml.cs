using System.Windows;
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
        private void ButValider_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void SliderVolumeSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.musiqueSon != null)
            {
                VOLUME_SON = SliderVolumeSon.Value;
                LabVolumeSon.Content = ("Volume Son: " + VOLUME_SON + "%");
                MainWindow.VolumeSon(VOLUME_SON);
            }
        }

        private void SliderVolumeMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (MainWindow.musiqueFond != null)
            {
                VOLUME_MUSIQUE = SliderVolumeMusique.Value;
                LabVolumeMusique.Content = ("Volume Son: " + VOLUME_MUSIQUE + "%");
                MainWindow.VolumeMusique(VOLUME_MUSIQUE);
            }
        }
    }
}
