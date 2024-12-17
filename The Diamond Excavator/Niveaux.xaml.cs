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
    /// Logique d'interaction pour Niveaux.xaml
    /// </summary>
    public partial class Niveaux : Window
    {
        public static double NIV_FACILE = 1;
        public static double NIV_MOYEN = 2;
        public static double NIV_DIFFICILE = 3;

        public Niveaux()
        {
            InitializeComponent();
        }

        private void ButFacile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ChoixNiveaux(NIV_FACILE);
            DialogResult = true;
        }

        private void ButMoyen_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ChoixNiveaux(NIV_MOYEN);
            DialogResult = true;
        }

        private void ButDifficile__Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ChoixNiveaux(NIV_DIFFICILE);
            DialogResult = true;
        }
    }
}
