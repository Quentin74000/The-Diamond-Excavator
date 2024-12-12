using System;
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
    public partial class MainWindow : Window
    {
        private static bool gauche, droite, creuse, saute;
        private static int vitesseJoueur = 5;
        private static int gravite = 10;
        private static int saut = 70;
        private static BitmapImage pelleteuseGauche, pelleteuseDroite, pelleteuseCreuse1, pelleteuseCreuse2, pelleteuseCreuse3;
        private static DispatcherTimer minuterie;
        private static DispatcherTimer collision;
        private static int decalageBloc = 64;
        private List<Rectangle> blocs = new List<Rectangle>();

        public MainWindow()
        {
            MenuJeu fenetreMenu = new MenuJeu();
            fenetreMenu.ShowDialog();

            InitializeComponent();
            InitialisationImages();
            InitialisationMinuterie();
            CreationBlocs();
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
            if (Key.Up == e.Key)
            {
                saute = true;
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
            if (Key.Up == e.Key)
            {
                saute = false;
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

        private void InitialisationMinuterie()
        {
            minuterie = new DispatcherTimer();
            minuterie.Interval = TimeSpan.FromMilliseconds(16);
            minuterie.Tick += Jeu;
            minuterie.Start();

            collision = new DispatcherTimer();
            collision.Interval = TimeSpan.FromMilliseconds(16);
            collision.Tick += Collision;
            collision.Start();
        }

        public void CreationBlocs()
        {
            int totalDecalageVertical = 0;
            for (int i = 0; i < 13; i++)
            {
                int totalDecalage = 0;
                for (int j = 0; j < 12; j++)
                {
                    Rectangle nouveauBloc = new Rectangle
                    {
                        Tag = "nouveauBloc",
                        Height = bloc.Height,
                        Width = bloc.Width,
                        Stroke = bloc.Stroke,
                        Fill = bloc.Fill,
                    };
                    totalDecalage += decalageBloc;
                    Canvas.SetLeft(nouveauBloc, Canvas.GetLeft(bloc) + totalDecalage);
                    Canvas.SetTop(nouveauBloc, Canvas.GetTop(bloc) + totalDecalageVertical);
                    zoneJeu.Children.Add(nouveauBloc);
                    blocs.Add(nouveauBloc);
                }
                totalDecalageVertical += decalageBloc;
            }
        }

        private void Collision(object sender, EventArgs e)
        {
            Rect joueurCollision = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
            Rect solGaucheCollision = new Rect(Canvas.GetLeft(solGauche), Canvas.GetTop(solGauche), solGauche.Width, solGauche.Height);
            Rect solDroitCollision = new Rect(Canvas.GetLeft(solDroit), Canvas.GetTop(solDroit), solDroit.Width, solDroit.Height);
            Rect solBasCollision = new Rect(Canvas.GetLeft(solBas), Canvas.GetTop(solBas), solBas.Width, solBas.Height);
            bool collisionDetectee = false;

            foreach (Rectangle nouveauBloc in blocs)
            {
                Rect blocCollision = new Rect(Canvas.GetLeft(nouveauBloc), Canvas.GetTop(nouveauBloc), nouveauBloc.Width, nouveauBloc.Height);
                if (joueurCollision.IntersectsWith(blocCollision))
                {
                        if (creuse == true)
                        {
                            zoneJeu.Children.Remove(nouveauBloc);
                            blocs.Remove(nouveauBloc);
                        }
                    testCollision.Content = "Collision";
                    gravite = 0;
                    collisionDetectee = true;
                    break;
                }
            }
            if (!collisionDetectee)
            {
                testCollision.Content = "Pas de collision";
                gravite = 10;
            }
            if (joueurCollision.IntersectsWith(solGaucheCollision) || joueurCollision.IntersectsWith(solDroitCollision) || joueurCollision.IntersectsWith(solBasCollision))
            {
                testCollision.Content = "Collision";
                collisionDetectee = true;
                gravite = 0;
            }
            Canvas.SetTop(joueur, Canvas.GetTop(joueur) + gravite);
        }

        private void Jeu(object? sender, EventArgs e)
        {
            if (gauche == true && droite == false)
            {
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solGauche) || Canvas.GetLeft(joueur) >= solGauche.ActualWidth)
                {
                    Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) - vitesseJoueur);
                }
                joueur.Source = pelleteuseGauche;
            }
            if (droite == true && gauche == false)
            {
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solDroit) || Canvas.GetLeft(joueur) <= this.ActualWidth - solGauche.ActualWidth)
                {
                    Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) + vitesseJoueur);
                }
                joueur.Source = pelleteuseDroite;
            }
            if (saute == true && gravite == 0)
            {
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) - saut);
            }
        }
    }
}