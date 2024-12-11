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
    /// Logique d'interaction pour Menu_Jeux.xaml
    /// </summary>
    public partial class Menu_Jeux : Window
    {
        public Menu_Jeux()
        {
            InitializeComponent();
        }

        private void butJouer_Click(object sender, RoutedEventArgs e)
        {

        }

        private void butQuit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
