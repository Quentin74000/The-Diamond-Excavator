using System;
using System.Windows;

namespace The_Diamond_Excavator
{
    public partial class MenuJeu : Window
    {
        private Options fenetreOptions;

        public MenuJeu()
        {
            InitializeComponent();
            fenetreOptions = new Options();
            fenetreOptions.Closing += FenetreOptions_Closing; // Ajouter un événement pour gérer la fermeture de la fenêtre
        }

        private void FenetreOptions_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true; // Annule la fermeture de la fenêtre
            fenetreOptions.Hide(); // Cache la fenêtre au lieu de la fermer
        }

        private void ButJouer_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void ButQuitter_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButOptions_Click(object sender, RoutedEventArgs e)
        {
            fenetreOptions.Show();
        }
    }
}