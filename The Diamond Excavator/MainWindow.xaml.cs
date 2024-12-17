﻿using System;
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
        public static MediaPlayer musiqueFond, musiqueSon; // pour initialiser la musique

        private static bool gauche, droite, creuse, saute, pause;
        private static int vitesseJoueur = 6;
        private static int gravite = 15;
        private static int saut = 100;
        private static int chrono = 0;
        private static BitmapImage pelleteuseGauche, pelleteuseDroite, pelleteuseCreuse1, pelleteuseCreuse2, pelleteuseCreuse3;
        private static DispatcherTimer minuterie;
        private static DispatcherTimer collision;
        private static DispatcherTimer chronometre;
        private static int decalageBloc = 64;
        private List<Rectangle> blocs = new List<Rectangle>();
        private List<Rectangle> bombes = new List<Rectangle>();
        MenuJeu fenetreMenu = new MenuJeu();
        Options fenetreOptions = new Options();
        MenuPause fenetrePause = new MenuPause();



        public MainWindow()
        {

            fenetreMenu.ShowDialog();
            
            InitializeComponent();

            InitialisationImages();
            InitialisationMinuterie();
            CreationBlocs();
            CreationBombes();
          
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
            if (Key.Up == e.Key)
            {
                saute = true;
            }
            if (Key.Escape == e.Key)
            {
                pause = true;
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

            chronometre = new DispatcherTimer();
            chronometre.Interval = TimeSpan.FromSeconds(1);
            chronometre.Tick += Chronometre;
            chronometre.Start();
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
        private void CreationBombes()
        {
            Random random = new Random();
            int nbBombe = 0;
            do
            {
                int decalage = 64, ligne = random.Next(1, 14), colonne = random.Next(1, 13);
                Rectangle nouvelleBombe = new Rectangle
                {
                    Tag = "nouvelleBombe",
                    Height = bombe.Height,
                    Width = bombe.Width,
                    Stroke = bombe.Stroke,
                    Fill = bombe.Fill,
                };
                Canvas.SetLeft(nouvelleBombe, Canvas.GetLeft(bombe) + colonne * decalage);
                Canvas.SetTop(nouvelleBombe, Canvas.GetTop(bombe) + ligne * decalage);
                zoneJeu.Children.Add(nouvelleBombe);
                bombes.Add(nouvelleBombe);
                nbBombe++;
            } while (nbBombe >= 10);
        }
        private void BlocClique(object sender, MouseButtonEventArgs e)
        {
            Rectangle blocClique = sender as Rectangle;
            if (blocClique != null)
            {
                Rect joueurRect = new Rect(Canvas.GetLeft(joueur)-joueur.Width, Canvas.GetTop(joueur)-joueur.Height, joueur.Width*3, joueur.Height*3);
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
            Rect solGaucheCollision = new Rect(Canvas.GetLeft(solGauche), Canvas.GetTop(solGauche), solGauche.Width, 5);
            Rect solDroitCollision = new Rect(Canvas.GetLeft(solDroit), Canvas.GetTop(solDroit), solDroit.Width, 5);
            Rect solBasCollision = new Rect(Canvas.GetLeft(solBas), Canvas.GetTop(solBas), solBas.Width, solBas.Height);

            bool collisionDetectee = false;

            foreach (Rectangle nouveauBloc in blocs)
            {
                Rect blocCollision = new Rect(Canvas.GetLeft(nouveauBloc) + 16, Canvas.GetTop(nouveauBloc), 32, 5);

                if (joueurCollision.IntersectsWith(blocCollision))
                {
                    testCollision.Content = "Collision avec un bloc";
                    gravite = 0;
                    collisionDetectee = true;
                    Canvas.SetTop(joueur, Canvas.GetTop(nouveauBloc) - joueur.ActualHeight);
                    break;
                }
            }
            if (joueurCollision.IntersectsWith(solGaucheCollision))
            {
                testCollision.Content = "Collision avec le sol gauche";
                collisionDetectee = true;
                gravite = 0;
                Canvas.SetTop(joueur, Canvas.GetTop(solGauche) - joueur.ActualHeight);
            }
            else if (joueurCollision.IntersectsWith(solDroitCollision))
            {
                testCollision.Content = "Collision avec le sol droit";
                collisionDetectee = true;
                gravite = 0;
                Canvas.SetTop(joueur, Canvas.GetTop(solDroit) - joueur.ActualHeight);
            }
            else if (joueurCollision.IntersectsWith(solBasCollision))
            {
                testCollision.Content = "Collision avec le sol bas";
                collisionDetectee = true;
                gravite = 0;
                Canvas.SetTop(joueur, Canvas.GetTop(solBas) - joueur.ActualHeight);
            }
            if (!collisionDetectee)
            {
                testCollision.Content = "Pas de collision";
                gravite = 15;
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) + gravite);
            }
        }

        private void Chronometre(object? sender, EventArgs e)
        {
            chrono++;
            lab_chronometre.Content = chrono;
        }

        private void Jeu(object? sender, EventArgs e)
        {
            bool bloqueGauche = false;
            bool bloqueDroite = false;
            bool enSaut = false;
            int enSautValeur = 0;
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
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solGauche) || (Canvas.GetLeft(joueur) >= solGauche.ActualWidth && Canvas.GetLeft(joueur) > 0))
                {
                    if (Canvas.GetLeft(joueur) - vitesseJoueur >= 0)
                    {
                        Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) - vitesseJoueur);
                    }
                }
                joueur.Source = pelleteuseGauche;
            }

            if (droite && !gauche && !bloqueDroite)
            {
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solDroit) || (Canvas.GetLeft(joueur) <= solDroit.ActualWidth + solGauche.ActualWidth + 140 && Canvas.GetLeft(joueur) < this.ActualWidth - joueur.ActualWidth))
                {
                    if (Canvas.GetLeft(joueur) + vitesseJoueur <= this.ActualWidth - joueur.ActualWidth)
                    {
                        Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) + vitesseJoueur);
                    }
                }
                joueur.Source = pelleteuseDroite;
            }
            if (saute && gravite == 0)
            {
                enSaut = true;
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) - saut);
            }
            if (pause)
            {
                if (minuterie.IsEnabled && chronometre.IsEnabled)
                {
                    minuterie.Stop();
                    chronometre.Stop();
                }
                fenetrePause.ShowDialog();
            }
            if (fenetrePause.DialogResult == true)
            {
                InitialisationMinuterie();
            }
        }
        // PARTIE MUSIQUE / SON
        public static void InitMusique()
        {
            musiqueFond = new MediaPlayer();
            musiqueFond.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "son/MusiqueFond.mp3"));
            musiqueFond.MediaEnded += RelanceMusique;
            musiqueFond.Play();
            musiqueFond.Volume = 0.1;
        }
        public static void InitSon()
        {
            musiqueSon = new MediaPlayer();
            musiqueSon.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + "son/musiqueSon.mp3"));
            musiqueSon.MediaEnded += RelanceSon;
            musiqueSon.Play();
            musiqueSon.Volume = 0.3;
        }
        public static void RelanceMusique(object? sender, EventArgs e)
        {
            musiqueFond.Position = TimeSpan.Zero;
            musiqueFond.Play();
        }
        public static void RelanceSon(object? sender, EventArgs e)
        {
            musiqueSon.Position = TimeSpan.Zero;
            musiqueSon.Play();
        }
        public static void VolumeMusique(double volumeMusique)
        {
            if (musiqueFond != null)
            {
                musiqueFond.Volume = volumeMusique / 100;
            }
        }
        public static void VolumeSon(double VolumeSon)
        {
            if (musiqueSon != null)
            {
                musiqueSon.Volume = VolumeSon / 100;
            }
        }

    }
}