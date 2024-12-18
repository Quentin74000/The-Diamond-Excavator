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
        public static MediaPlayer musiqueFond, musiqueSon; // pour initialiser la musique

        private static bool gauche, droite, creuse, saute, pause, triche, gagne, perdu;
        private static int vitesseJoueur = 6;
        private static int gravite = 15;
        private static int saut = 125;
        private static int chrono = 0;
        private static int nbVie = 3;
        private static int nbDiamant = 0;
        private static int decalageBloc = 64;
        private static BitmapImage pelleteuseGauche, pelleteuseDroite, pelleteuseCreuse1, pelleteuseCreuse2, pelleteuseCreuse3;

        // DÉCLARATION DES CHRONOMETRES
        private static DispatcherTimer minuterie;
        private static DispatcherTimer collision;
        private static DispatcherTimer chronometre;
        private static DispatcherTimer valeurAffichage;

        // DÉCLARATION DES LISTES
        private List<Rectangle> blocs = new List<Rectangle>();
        private List<Rectangle> bombes = new List<Rectangle>();
        private List<Rectangle> diamants = new List<Rectangle>();
        private List<Rectangle> vies = new List<Rectangle>();

        // DÉCLARATION FENETRES
        MenuJeu fenetreMenu = new MenuJeu();
        Niveaux fenetreNiveaux = new Niveaux();
        Options fenetreOptions = new Options();
        MenuPause fenetrePause = new MenuPause();

        // DÉCLARATION DES DIFFÉRENTES VARIABLES (BOMBES,DIAMANTS,MINES)
        public static int NB_BOMBES = 0;
        public static int NB_MINES = 0;
        public static int NB_DIAMANTS = 0;


        private bool peutSauter = true; // Variable pour vérifier si le joueur peut sauter
        private DispatcherTimer timerSaut; // Timer pour gérer le délai de saut

        public MainWindow()
        {
            fenetreMenu.ShowDialog();
            fenetreNiveaux.ShowDialog();
            InitializeComponent();
            InitialisationImages();
            InitialisationMinuterie();
            InitialisationVie();
            CreationDiamants();
            CreationBombes();
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
            if (Key.Up == e.Key)
            {
                saute = true;
            }
            if (Key.Escape == e.Key)
            {
                pause = true;
            }
            if (Key.P == e.Key)
            {
                triche = true;
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
            if (Key.P == e.Key)
            {
                triche = false;
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

            valeurAffichage = new DispatcherTimer();
            valeurAffichage.Interval = TimeSpan.FromMilliseconds(16);
            valeurAffichage.Tick += AffichageValeur;
            valeurAffichage.Start();

            timerSaut = new DispatcherTimer();
            timerSaut.Interval = TimeSpan.FromSeconds(1); // Délai d'1 seconde entre les sauts
            timerSaut.Tick += (s, e) =>
            {
                peutSauter = true; // Réautorise le saut après le délai
                timerSaut.Stop();   // Arrête le timer
            };
        }
        public void InitialisationVie()
        {
            int ajouterVie = 0, totalDecalage = 0;
            do
            {
                Rectangle nouvelleVie = new Rectangle
                {
                    Tag = "nouvelleVie",
                    Height = vie.Height,
                    Width = vie.Width,
                    Stroke = vie.Stroke,
                    Fill = vie.Fill,
                };
                totalDecalage += decalageBloc;
                Canvas.SetLeft(nouvelleVie, Canvas.GetLeft(vie) + totalDecalage);
                Canvas.SetTop(nouvelleVie, Canvas.GetTop(vie));
                zoneJeu.Children.Add(nouvelleVie);
                vies.Add(nouvelleVie);
                ajouterVie += 1;
            } while (ajouterVie < nbVie);
        }
        public void CreationBlocs()
        {
            int totalDecalageVertical = 0;
            for (int i = 0; i < 10; i++)
            {
                int totalDecalage = 0;
                for (int j = 0; j < 17; j++)
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
            int decalage = 64, nbBombe = 0;

            do
            {
                // Génération aléatoire de la position
                int ligne = random.Next(1, 10), colonne = random.Next(1, 18);
                Rectangle nouvelleBombe = new Rectangle
                {
                    Tag = "nouvelleBombe",
                    Height = bombe.Height,
                    Width = bombe.Width,
                    Stroke = bombe.Stroke,
                    Fill = bombe.Fill,
                };

                // Premier placement de la bombe
                Canvas.SetLeft(nouvelleBombe, Canvas.GetLeft(bombe) + colonne * decalage);
                Canvas.SetTop(nouvelleBombe, Canvas.GetTop(bombe) + ligne * decalage);

                // Initialisation des tests pour le chevauchement entre bombes et diamants
                bool bombeDejaExistante = false;
                bool positionSurDiamant = false;

                // Test pour savoir si une bombe est déjà présent dans l'emplacement
                foreach (Rectangle bombeExistante in bombes)
                {
                    if (Canvas.GetLeft(bombeExistante) == Canvas.GetLeft(nouvelleBombe) &&
                        Canvas.GetTop(bombeExistante) == Canvas.GetTop(nouvelleBombe))
                    {
                        bombeDejaExistante = true;
                        break;
                    }
                }

                // Test pour savoir si un diamant est déjà présent dans l'emplacement
                foreach (Rectangle diamantExistant in diamants)
                {
                    if (Canvas.GetLeft(diamantExistant) == Canvas.GetLeft(nouvelleBombe) &&
                        Canvas.GetTop(diamantExistant) == Canvas.GetTop(nouvelleBombe))
                    {
                        positionSurDiamant = true;
                        break;
                    }
                }

                // Ajout de la bombe si la position est valide
                if (!bombeDejaExistante && !positionSurDiamant)
                {
                    zoneJeu.Children.Add(nouvelleBombe);
                    bombes.Add(nouvelleBombe);
                    nbBombe += 1;
                }

            } while (nbBombe < NB_BOMBES);
        }
        
        private void CreationDiamants()
        {
            Random random = new Random();
            int decalage = 64, nbDiamants = 0;
            do
            {
                // Génération aléatoire de la position
                int ligne = random.Next(1, 10), colonne = random.Next(1, 18);
                Rectangle nouveauDiamant = new Rectangle
                {
                    Tag = "nouveauDiamant",
                    Height = diamant.Height,
                    Width = diamant.Width,
                    Stroke = diamant.Stroke,
                    Fill = diamant.Fill,
                };

                // Premier placement du diamant
                Canvas.SetLeft(nouveauDiamant, Canvas.GetLeft(diamant) + colonne * decalage);
                Canvas.SetTop(nouveauDiamant, Canvas.GetTop(diamant) + ligne * decalage);

                // Initialisation du test pour éviter le chevauchement avec un autre diamant
                bool dejaExistant = false;

                // Vérification de l'emplacement libre ou non
                foreach (Rectangle diamantExistant in diamants)
                {
                    if (Canvas.GetLeft(diamantExistant) == Canvas.GetLeft(nouveauDiamant) && Canvas.GetTop(diamantExistant) == Canvas.GetTop(nouveauDiamant))
                    {
                        dejaExistant = true;
                        break;
                    }
                }

                // S'il n'y a pas de diamant ici alors le placer
                if (!dejaExistant)
                {
                    zoneJeu.Children.Add(nouveauDiamant);
                    diamants.Add(nouveauDiamant);
                    nbDiamants += 1;
                }
                else
                {
                    continue;
                }
            } while (nbDiamants < NB_DIAMANTS);
        }

        // Méthode pour cliquer sur les blocs
        private void BlocClique(object sender, MouseButtonEventArgs e)
        {
            // Vérifie si l'objet cliqué est un rectangle
            Rectangle blocClique = sender as Rectangle;
            if (blocClique == null)
                return;

            // Créer un Rect pour le bloc cliqué
            Rect blocRect = new Rect(Canvas.GetLeft(blocClique), Canvas.GetTop(blocClique), blocClique.Width, blocClique.Height);

            // Vérifie la collision avec la zone du joueur
            Rect joueurRect = new Rect(Canvas.GetLeft(joueur)-joueur.Width/2, Canvas.GetTop(joueur)-joueur.Height/2, joueur.Width * 2, joueur.Height * 2);

            // Si le bloc n'est pas en collision avec la zone du joueur, ignorer le clic
            if (!joueurRect.IntersectsWith(blocRect))
            {
                return;
            }

            // Initialisation des variables pour déterminer si une bombe ou un diamant est présent
            Rectangle bombeAssociee = null;
            Rectangle diamantAssociee = null;

            // Vérifie si une bombe est cachée sous ce bloc
            foreach (Rectangle nouvelleBombe in bombes)
            {
                if (Canvas.GetLeft(nouvelleBombe) == Canvas.GetLeft(blocClique) && Canvas.GetTop(nouvelleBombe) == Canvas.GetTop(blocClique))
                {
                    bombeAssociee = nouvelleBombe;
                    break;
                }
            }
            foreach (Rectangle nouveauDiamant in diamants)
            {
                if (Canvas.GetLeft(nouveauDiamant) == Canvas.GetLeft(blocClique) && Canvas.GetTop(nouveauDiamant) == Canvas.GetTop(blocClique))
                {
                    diamantAssociee = nouveauDiamant;
                    break;
                }
            }

            // Si une bombe est trouvée sous le bloc
            if (bombeAssociee != null)
            {
                // Retire la dernière vie a être apparue
                Rectangle viePerdue = vies[^1];
                zoneJeu.Children.Remove(viePerdue);
                vies.RemoveAt(vies.Count - 1);
                nbVie -= 1;
                
                // Retire la bombe de la liste pour qu'elle puisse rester affichée sans compter
                bombes.Remove(bombeAssociee);
                NB_BOMBES -= 1;
            }

            if (diamantAssociee != null)
            {

                // Retire le diamant de la liste pour qu'il puisse rester affiché sans compter
                diamants.Remove(diamantAssociee);
                nbDiamant += 1;
                
            }

            // Supprime le bloc cliqué du canvas et de la liste
            blocs.Remove(blocClique);
            zoneJeu.Children.Remove(blocClique);
        }
        private void AffichageValeur(object sender, EventArgs e)
        {
            diamantTrouve.Content = "Diamants trouvés : " + nbDiamant + "/" + NB_DIAMANTS;
            bombeRestante.Content = "Bombes restantes : " + NB_BOMBES;
        }
        // Méthode pour les collisions
        private void Collision(object sender, EventArgs e)
        {
            // Initialisation des rectangles qui permettront de détecter une collision
            Rect joueurCollision = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);
            Rect solGaucheCollision = new Rect(Canvas.GetLeft(solGauche), Canvas.GetTop(solGauche), solGauche.Width, 5);
            Rect solDroitCollision = new Rect(Canvas.GetLeft(solDroit), Canvas.GetTop(solDroit), solDroit.Width, 5);
            Rect solBasCollision = new Rect(Canvas.GetLeft(solBas), Canvas.GetTop(solBas), solBas.Width, solBas.Height);

            bool collisionDetectee = false;

            // Détecte si le joueur est sur un bloc
            foreach (Rectangle nouveauBloc in blocs)
            {
                Rect blocCollision = new Rect(Canvas.GetLeft(nouveauBloc) + 16, Canvas.GetTop(nouveauBloc), 32, 5);

                if (joueurCollision.IntersectsWith(blocCollision))
                {
                    gravite = 0;
                    collisionDetectee = true;
                    Canvas.SetTop(joueur, Canvas.GetTop(nouveauBloc) - joueur.ActualHeight);
                    break;
                }
            }

            // Collision entre joueur et sol de gauche
            if (joueurCollision.IntersectsWith(solGaucheCollision))
            {
                collisionDetectee = true;
                gravite = 0;
                Canvas.SetTop(joueur, Canvas.GetTop(solGauche) - joueur.ActualHeight);
            }
            // Collision entre joueur et sol de droite
            else if (joueurCollision.IntersectsWith(solDroitCollision))
            {
                collisionDetectee = true;
                gravite = 0;
                Canvas.SetTop(joueur, Canvas.GetTop(solDroit) - joueur.ActualHeight);
            }
            // Collision entre joueur et sol du bas
            else if (joueurCollision.IntersectsWith(solBasCollision))
            {
                collisionDetectee = true;
                gravite = 0;
                Canvas.SetTop(joueur, Canvas.GetTop(solBas) - joueur.ActualHeight);
            }
            // S'il n'y a aucune collision détectée alors la gravité a sa valeur normale
            if (!collisionDetectee)
            {
                gravite = 15;
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) + gravite);
            }
        }

        // Méthode pour afficher un chronomètre
        private void Chronometre(object? sender, EventArgs e)
        {
            chrono++;
            lab_chronometre.Content = "Temps : " + chrono;
        }

        // Méthode Jeu
        private void Jeu(object? sender, EventArgs e)
        {
            bool gagne = false;
            bool perdu = false;
            bool bloqueGauche = false;
            bool bloqueDroite = false;
            bool BloqueBas = false;
            Rect joueurCollision = new Rect(Canvas.GetLeft(joueur), Canvas.GetTop(joueur), joueur.Width, joueur.Height);

            // Gestion de la collision entre le joueur est les blocs de façon horizontale
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
                if (Canvas.GetTop(joueur) <= Canvas.GetTop(solDroit) || (Canvas.GetLeft(joueur) <= solDroit.ActualWidth + solGauche.ActualWidth + 405 && Canvas.GetLeft(joueur) < this.ActualWidth - joueur.ActualWidth))
                {
                    if (Canvas.GetLeft(joueur) + vitesseJoueur <= this.ActualWidth - joueur.ActualWidth)
                    {
                        Canvas.SetLeft(joueur, Canvas.GetLeft(joueur) + vitesseJoueur);
                    }
                }
                joueur.Source = pelleteuseDroite;
            }
            if (saute && gravite == 0 && peutSauter)
            {
                // Exécute le saut
                Canvas.SetTop(joueur, Canvas.GetTop(joueur) - saut);
                peutSauter = false; // Désactive le saut jusqu'à la fin du délai
                timerSaut.Start();  // Lance le timer pour réautoriser le saut
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
            if (triche)
            {
                foreach (Rectangle nouveauBloc in blocs)
                {
                    nouveauBloc.Opacity = 0;
                }
            }
            else if (!triche)
            {
                foreach (Rectangle nouveauBloc in blocs)
                {
                    nouveauBloc.Opacity = 100;
                }
            }
            if (fenetrePause.DialogResult == true)
            {
                InitialisationMinuterie();
            }
            if (nbVie <= 0)
            {
                perdu = true;
            }
            if (nbDiamant == NB_DIAMANTS)
            {
                gagne = true;
            }
            if (gagne == true)
            {
                foreach (Rectangle nouveauBloc in blocs)
                {
                    nouveauBloc.Opacity = 0;
                }
                minuterie.Stop();
                chronometre.Stop();
                Gagne fenetreGagne = new Gagne();
                fenetreGagne.ShowDialog();
            }
            else if (perdu == true)
            {
                foreach (Rectangle nouveauBloc in blocs)
                {
                    nouveauBloc.Opacity = 0;
                }
                minuterie.Stop();
                chronometre.Stop();
                Perdu fenetrePerdu = new Perdu();
                fenetrePerdu.ShowDialog();
            }
        }
        // NIVEAUX CHOISIT
        public static void ChoixNiveaux(double niveaux)
        {
            if (niveaux == 1)
            {
                Console.WriteLine("Niveau 1");
                NB_BOMBES = 5;
                NB_MINES = 7;
                NB_DIAMANTS = 6;
                //Console.WriteLine($"{NB_BOMBES},{NB_DIAMANTS},{NB_MINES}");
            }
            else if (niveaux == 2)
            {
                Console.WriteLine("Niveau 2");
                NB_BOMBES = 10;
                NB_MINES = 7;
                NB_DIAMANTS = 6;
                //Console.WriteLine($"{NB_BOMBES},{NB_DIAMANTS},{NB_MINES}");
            }
            else
            {
                Console.WriteLine("Niveau 3");
                NB_BOMBES = 15;
                NB_MINES = 7;
                NB_DIAMANTS = 6;
                //Console.WriteLine($"{NB_BOMBES},{NB_DIAMANTS},{NB_MINES}");
            }
        }
                                // PARTIE MUSIQUE / SON

        // INITTIALISATION DE LA MUSIQUE / SON
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
        // PERMET DE RELANCER LA MUSIQUE / LE SON QUAND ILS SONT FINIT
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
          // PERMET DE MODIFIER LE VOLUME DE LA MUSIQUE / SON
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