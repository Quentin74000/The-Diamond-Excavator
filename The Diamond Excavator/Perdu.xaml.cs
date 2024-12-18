using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
    /// Logique d'interaction pour Perdu.xaml
    /// </summary>
    public partial class Perdu : Window
    {
        public Perdu()
        {
            InitializeComponent();
        }

        private void Quitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}