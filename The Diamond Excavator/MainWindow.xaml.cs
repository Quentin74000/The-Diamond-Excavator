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
        private static int vitesseJoueur = 6;
        private static int gravite = 10;
        private static int saut = 100;
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

                    // Ajouter l'événement clic au bloc
                    nouveauBloc.MouseDown += BlocClique;

                    zoneJeu.Children.Add(nouveauBloc);
                    blocs.Add(nouveauBloc);
                }
                totalDecalageVertical += decalageBloc;
            }
        }
        private void BlocClique(object sender, MouseButtonEventArgs e)
        {
            Rectangle blocClique = sender as Rectangle;
        
            if (blocClique != null)
            {
                Rect joueurRect = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
                Rect blocRect = new Rect(Canvas.GetLeft(blocClique), Canvas.GetTop(blocClique), blocClique.Width, blocClique.Height);
                if (joueurRect.IntersectsWith(blocRect))
                {
                    zoneJeu.Children.Remove(blocClique);
                    blocs.Remove(blocClique);
                }
            }
        }
        private void Collision(object sender, EventArgs e)
        {
            Rect joueurCollision = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);

            // Rectangles de collision corrigés
            Rect solGaucheCollision = new Rect(Canvas.GetLeft(solGauche), Canvas.GetTop(solGauche), solGauche.Width, solGauche.Height);
            Rect solDroitCollision = new Rect(Canvas.GetLeft(solDroit), Canvas.GetTop(solDroit), solDroit.Width, solDroit.Height);
            Rect solBasCollision = new Rect(Canvas.GetLeft(solBas), Canvas.GetTop(solBas), solBas.Width, solBas.Height);

            bool collisionDetectee = false;

            // Collision avec les blocs
            foreach (Rectangle nouveauBloc in blocs)
            {
                Rect blocCollision = new Rect(Canvas.GetLeft(nouveauBloc) + 16, Canvas.GetTop(nouveauBloc), 32, 5);
                if (joueurCollision.IntersectsWith(blocCollision))
                {
                    if (creuse)
                    {
                        zoneJeu.Children.Remove(nouveauBloc);
                        blocs.Remove(nouveauBloc);
                        break;
                    }
                    testCollision.Content = "Collision avec un bloc";
                    gravite = 0;
                    collisionDetectee = true;
                    break;
                }
            }

            // Collision avec les sols
            if (joueurCollision.IntersectsWith(solGaucheCollision))
            {
                testCollision.Content = "Collision avec le sol gauche";
                collisionDetectee = true;
                gravite = 0;
            }
            else if (joueurCollision.IntersectsWith(solDroitCollision))
            {
                testCollision.Content = "Collision avec le sol droit";
                collisionDetectee = true;
                gravite = 0;
            }
            else if (joueurCollision.IntersectsWith(solBasCollision))
            {
                testCollision.Content = "Collision avec le sol bas";
                collisionDetectee = true;
                gravite = 0;
            }

            // Appliquer la gravité si aucune collision n'est détectée
            if (!collisionDetectee)
            {
                testCollision.Content = "Pas de collision";
                gravite = 10;
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) + gravite);
            }
        }

        private void Jeu(object? sender, EventArgs e)
        {
            bool bloqueGauche = false;
            bool bloqueDroite = false;
            Rect joueurCollision = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
            foreach (Rectangle bloc in blocs)
            {
                Rect blocRect = new Rect(Canvas.GetLeft(bloc), Canvas.GetTop(bloc), bloc.Width, bloc.Height);
                if (joueurCollision.IntersectsWith(blocRect) && joueurCollision.Right >= blocRect.Left && joueurCollision.Left < blocRect.Left && joueurCollision.Bottom > blocRect.Top)
                {
                    bloqueDroite = true;
                }
                if (joueurCollision.IntersectsWith(blocRect) && joueurCollision.Left <= blocRect.Right && joueurCollision.Right > blocRect.Right && joueurCollision.Bottom > blocRect.Top)
                {
                    bloqueGauche = true;
                }
            }
            if (gauche && !droite && !bloqueGauche)
            {
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solGauche) || Canvas.GetLeft(joueur) >= solGauche.ActualWidth)
                {
                    Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) - vitesseJoueur);
                }
                joueur.Source = pelleteuseGauche;
            }
            if (droite && !gauche && !bloqueDroite)
            {
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solDroit) || Canvas.GetLeft(joueur) <= solDroit.ActualWidth + solGauche.ActualWidth + 140)
                {
                    Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) + vitesseJoueur);
                }
                joueur.Source = pelleteuseDroite;
            }
            if (saute && gravite == 0)
            {
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) - saut);
            }
        }
    }
}