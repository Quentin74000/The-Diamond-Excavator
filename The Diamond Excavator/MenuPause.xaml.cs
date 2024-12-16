﻿using System;
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
using System.Windows.Threading;

namespace The_Diamond_Excavator
{
    /// <summary>
    /// Logique d'interaction pour menu_Pause.xaml
    /// </summary>

    public partial class MenuPause : Window
    {
        private static DispatcherTimer chronmetre;
        public static double VOLUME_SON;
        public static double VOLUME_MUSIQUE;


        public MenuPause()
        {
            InitializeComponent();
            //valeuSliderSon = V;
            //Chronometre();
        }

        //private void Chronometre(object sender, RoutedEventArgs e)
        //{
        //    chronmetre = new DispatcherTimer();
        //    chronmetre.Interval = TimeSpan.FromMilliseconds(16);
        //    chronmetre.Tick += Raccourci;
        //    chronmetre.Start();
        //}

        public void butReprendrePause_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
           
        }

        private void butQuitterPause_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Raccourci(object sender, KeyEventArgs e)
        {
            if (Key.Escape == e.Key)
            {
                DialogResult = true;
            }
        }
        public void sliderVolumeSon_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VOLUME_SON = sliderVolumeSon.Value;
            labVolumeSon.Content = ("Volume Son: " + VOLUME_SON + "%");
            MainWindow.VolumeSon(VOLUME_SON);
        }

        public void sliderVolumeMusique_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            VOLUME_MUSIQUE = sliderVolumeMusique.Value;
            labVolumeMusique.Content = ("Volume Musique: " + VOLUME_MUSIQUE + "%");
            MainWindow.VolumeMusique(VOLUME_MUSIQUE);
        }


    }
}
