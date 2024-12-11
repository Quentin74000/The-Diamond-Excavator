using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace The_Diamond_Excavator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static bool gauche, droite, creuse;
        private static int vitesseJoueur = 5;
        private static BitmapImage pelleteuseGauche, pelleteuseDroite, pelleteuseCreuse1, pelleteuseCreuse2, pelleteuseCreuse3;
        private static DispatcherTimer timer;
        private static int test=0;
        public MainWindow()
        {
            InitializeComponent();
            InitialisationImages();
            InitialisationTimer();
        }
        private void joueur_ToucheEnfoncee(object sender, KeyEventArgs e)
        {
            if (Key.Left == e.Key)
            {
                gauche = true;
            }
            if (Key.Right == e.Key)
            {
                droite = true;
            }
            if (Key.Down == e.Key)
            {
                creuse = true;
            }
        }
        private void joueur_ToucheRelachee(object sender, KeyEventArgs e)
        {
            if (Key.Left == e.Key)
            {
                gauche = false;
            }
            if (Key.Right == e.Key)
            {
                droite = false;
            }
            if (Key.Down == e.Key)
            {
                creuse = false;
            }
        }
        private void InitialisationImages()
        {
            {
                pelleteuseGauche = new BitmapImage(new Uri($"pack://application:,,,/img/PelleteuseGauche.png"));
                pelleteuseDroite = new BitmapImage(new Uri($"pack://application:,,,/img/PelleteuseDroite.png"));
                pelleteuseCreuse1 = new BitmapImage(new Uri($"pack://application:,,,/img/PelleteuseCreuse1.png"));
                pelleteuseCreuse2 = new BitmapImage(new Uri($"pack://application:,,,/img/PelleteuseCreuse2.png"));
                pelleteuseCreuse3 = new BitmapImage(new Uri($"pack://application:,,,/img/PelleteuseCreuse3.png"));
            }
        }
        private void InitialisationTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += Jeu;
            timer.Start();
        }

        private void Jeu(object? sender, EventArgs e)
        {
            test++;
            TEST.Content = "test : " + test;
            Rect JoueurCollision = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
            if (gauche == true && droite == false)
            {
                Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) - vitesseJoueur);
                joueur.Source = pelleteuseGauche;
            }
            if (droite == true && gauche == false)
            {
                Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) + vitesseJoueur);
                joueur.Source = pelleteuseDroite;
            }
            if (creuse == true && droite == false && gauche == false)
            {
                joueur.Source = pelleteuseCreuse1;
                TimeSpan.FromSeconds(1);
                joueur.Source = pelleteuseCreuse2;
                TimeSpan.FromSeconds(1);
                joueur.Source = pelleteuseCreuse3;
                TimeSpan.FromSeconds(1);
                joueur.Source = pelleteuseDroite;
            }
        }    
    }
}