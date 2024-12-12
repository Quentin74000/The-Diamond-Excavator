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
    /// Logique d'interaction pour menu_Pause.xaml
    /// </summary>
    public partial class menu_Pause : Window
    {
        public menu_Pause()
        {
            InitializeComponent();
        }

        private void butReprendrePause_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;

        }

        private void butQuitterPause_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void sliderVolumeMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string valeurSliderMusique = "";
            valeurSliderMusique = sliderVolumeMusique.Value.ToString();
            labVolumeMusique.Content = ("Volume Musique: " + valeurSliderMusique +"%");
        }

        private void sliderVolumeSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            string valeurSliderSon = "";
            valeurSliderSon = sliderVolumeSon.Value.ToString();
            labVolumeSon.Content = ("Volume Musique: " + valeurSliderSon + "%");
        }
    }
}
