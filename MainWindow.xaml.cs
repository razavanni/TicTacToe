using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace _03_TicTacToe_Dynamisch
{
    public partial class MainWindow : Window
    {
        // Deklaration eines 2D-Arrays für das Spielfeld
        Rechteck[,] spielfeld;

        // Zähler für den aktuellen Spieler
        int spieler;

        // Statische Größe des Spielfelds
        int spielfeldgroesse = 3;
        // Neue Variable für richtige Formatierung des Spielfelds
        int spielfeldgroessePlusAbstand;

        // Name des Aktuellen spielers
        private string currentPlayerName;
        // Array für Spielernamen
        private string[] playerNames;

        // Index des aktuellen Spielers im playerNames-Array
        private int currentPlayerIndex;
        // Array für verbleibende Zeit pro Spieler
        private int[] remainingTimePerPlayer;
        // Array für verbleibende Zeit pro Spieler
        private DispatcherTimer timer;

        // Modales Fenster für die Spieleinstellungen
        StartModal modal;


        public MainWindow()
        {
            // Initialisierung der verbleibenden Zeit pro Spieler und Timer
            remainingTimePerPlayer = new int[2];
            currentPlayerIndex = 0;
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += Timer_Tick;

            // Initialisierung des modalen Fensters für die Spieleinstellungen
            modal = new StartModal();

            // Zeigt das modale Fenster an und prüft, ob "OK" ausgewählt wurde
            if ((bool)modal.ShowDialog())
            {
                InitializeComponent();
            }
            else
            {
                // Schließt das Fenster, wenn "Abbrechen" ausgewählt wurde
                Close();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialisiert das Spielfeld und Spielerinformationen
            Init();

            // Speichert die Spielernamen im playerNames-Array
            playerNames = new string[]
            {
                modal.SpielerName_1,
                modal.SpielerName_2
            };
            currentPlayerName = playerNames[0];
            UpdateCurrentPlayerLabel();

            // Setzt die verbleibende Zeit für beide Spieler und startet den Timer
            remainingTimePerPlayer[0] = modal.ZeitProSpieler * 60;
            remainingTimePerPlayer[1] = modal.ZeitProSpieler * 60;
            UpdateRemainingTimeLabel();

            timer.Start();
        }

        private void Init()
        {
            // Setzt die Spielfeldgröße und Abstände für die Zellen
            spielfeldgroesse = modal.Spielgroesse;
            spielfeldgroessePlusAbstand = spielfeldgroesse + 2;

            // Setzt die Spielerlabels auf die Namen aus dem modalen Fenster
            spielername_1_label_zeit.Content = modal.SpielerName_1;
            spielername_2_label_zeit.Content = modal.SpielerName_2;
            // Spieler mit der ID 1 fängt an
            spieler = 1;

            // Initialisiert das 2D-Array für das Spielfeld
            spielfeld = new Rechteck[spielfeldgroesse, spielfeldgroesse];
            for (int i = 0; i < spielfeldgroesse; i++)
            {
                for (int j = 0; j < spielfeldgroesse; j++)
                {
                    spielfeld[i, j] = new Rechteck(canvas.ActualWidth / spielfeldgroessePlusAbstand, canvas.ActualHeight / spielfeldgroessePlusAbstand, i, j);
                    spielfeld[i, j].Zeichne(canvas);
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Durchläuft alle Zellen im Spielfeld
            for (int i = 0; i < spielfeldgroesse; i++)
            {
                for (int j = 0; j < spielfeldgroesse; j++)
                {
                    // Überprüft, ob die Maus über einer Zelle ist
                    if (spielfeld[i, j].Rect.IsMouseOver)
                    {
                        // Überprüft, ob die Zelle bereits geklickt wurde
                        if (!spielfeld[i, j].WurdeGeklickt)
                        {
                            spielfeld[i, j].WurdeGeklickt = true;

                            // Setzt die Farbe und den Besitzer der Zelle basierend auf dem aktuellen Spieler
                            if (spieler == 1)
                            {
                                spielfeld[i, j].Rect.Fill = Brushes.Red;
                                spielfeld[i, j].Owner = 1;
                                spieler = 2;
                            }
                            else if (spieler == 2)
                            {
                                spielfeld[i, j].Rect.Fill = Brushes.Blue;
                                spielfeld[i, j].Owner = 2;
                                spieler = 1;
                            }
                        }
                    }
                }
            }

            // Aktualisiert den aktuellen Spieler und die verbleibende Zeit
            currentPlayerIndex = spieler - 1;
            currentPlayerName = playerNames[spieler - 1];
            UpdateCurrentPlayerLabel();

            remainingTimePerPlayer[currentPlayerIndex]--;
            if (remainingTimePerPlayer[currentPlayerIndex] <= 0)
            {
                timer.Stop();
                MessageBox.Show("Zeit abgelaufen für Spieler " + playerNames[currentPlayerIndex]);
                Close();
            }
            UpdateRemainingTimeLabel();

            // Überprüfe, ob Spieler 1 gewonnen hat
            if (CheckWin(1))
            {
                // Spielername = erster name im Array
                currentPlayerName = playerNames[0];
                timer.Stop();
                UpdateCurrentPlayerLabel();
                MessageBox.Show("Spieler 1 (" + playerNames[0] + ") hat gewonnen!");
                Close();
            }

            // Überprüfe, ob Spieler 2 gewonnen hat
            if (CheckWin(2))
            {
                // Spielername = zweiter name im Array
                currentPlayerName = playerNames[1];
                timer.Stop();
                UpdateCurrentPlayerLabel();
                MessageBox.Show("Spieler 2 (" + playerNames[1] + ") hat gewonnen!");
                Close();
            }

            // Überprüfe auf ein Unentschieden
            if (IsTie())
            {
                timer.Stop();
                MessageBox.Show("Unentschieden!");
                Close();
            }
        }

        private bool IsTie()
        {
            bool isTie = true;

            for (int i = 0; i < spielfeldgroesse; i++)
            {
                for (int j = 0; j < spielfeldgroesse; j++)
                {
                    if (spielfeld[i, j].Owner == 0)
                    {
                        isTie = false;
                        break;
                    }
                }
                if (!isTie)
                {
                    break;
                }
            }

            return isTie;
        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            for (int i = 0; i < spielfeldgroesse; i++)
            {
                for (int j = 0; j < spielfeldgroesse; j++)
                {
                    double sx;
                    double sy;

                    try
                    {
                        sx = e.NewSize.Width / e.PreviousSize.Width;
                        sy = e.NewSize.Height / e.PreviousSize.Height;

                        spielfeld[i, j].Grosse(sx, sy);
                    }
                    catch { }

                }
            }
        }

        private bool CheckWin(int player)
        {
            // Überprüfe die Reihen
            for (int i = 0; i < spielfeldgroesse; i++)
            {
                // Annahme, dass der Spieler gewonnen hat, bis das Gegenteil bewiesen ist
                bool rowWin = true;
                for (int j = 0; j < spielfeldgroesse; j++)
                {
                    if (spielfeld[i, j].Owner != player)
                    {
                        // Setze die Annahme auf falsch, wenn ein Feld nicht dem Spieler gehört
                        rowWin = false;
                        // Breche die Schleife ab weil Gewinn nicht möglich ist
                        break;
                    }
                }
                if (rowWin)
                {
                    // Wenn die Annahme wahr ist, gibt es einen Gewinn in dieser Reihe
                    return true;
                }
            }

            // Überprüfe die Spalten
            for (int j = 0; j < spielfeldgroesse; j++)
            {
                // Annahme, dass der Spieler gewonnen hat, bis das Gegenteil bewiesen ist
                bool columnWin = true;
                for (int i = 0; i < spielfeldgroesse; i++)
                {
                    if (spielfeld[i, j].Owner != player)
                    {
                        // Setze die Annahme auf falsch, wenn ein Feld nicht dem Spieler gehört
                        columnWin = false;
                        // Breche die Schleife ab, da der Gewinn nicht möglich ist
                        break;
                    }
                }
                if (columnWin)
                {
                    // Wenn die Annahme wahr ist, gibt es einen Gewinn in dieser Spalte
                    return true;
                }
            }

            // Überprüfe Hauptdiagonale
            // Annahme, dass der Spieler gewonnen hat, bis das Gegenteil bewiesen ist
            bool mainDiagonalWin = true;
            for (int i = 0; i < spielfeldgroesse; i++)
            {
                if (spielfeld[i, i].Owner != player)
                {
                    // Setze die Annahme auf falsch, wenn ein Feld nicht dem Spieler gehört
                    mainDiagonalWin = false;
                    // Breche die Schleife ab, da der Gewinn nicht möglich ist
                    break;
                }
            }
            if (mainDiagonalWin)
            {
                // Wenn die Annahme wahr ist, gibt es einen Gewinn in der Hauptdiagonale
                return true;
            }

            // Überprüfe Nebendiagonale
            // Annahme, dass der Spieler gewonnen hat, bis das Gegenteil bewiesen ist
            bool secondaryDiagonalWin = true;
            for (int i = 0; i < spielfeldgroesse; i++)
            {
                if (spielfeld[i, spielfeldgroesse - 1 - i].Owner != player)
                {
                    // Setze die Annahme auf falsch, wenn ein Feld nicht dem Spieler gehört
                    secondaryDiagonalWin = false;
                    // Breche die Schleife ab, da der Gewinn nicht möglich ist
                    break;
                }
            }
            if (secondaryDiagonalWin)
            {
                // Wenn die Annahme wahr ist, gibt es einen Gewinn in der Nebendiagonale
                return true;
            }
            // Kein Gewinn wurde gefunden
            return false;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // Setzt die Spielfeldgröße und zeigt eine Nachricht an
            spielfeldgroesse = modal.Spielgroesse;
            MessageBox.Show("Spiel hat gestartet");
        }

        private void Ende_Click(object sender, RoutedEventArgs e)
        {
            // Schließt das Fenster
            Close();
        }

        private void Parameter_Click(object sender, RoutedEventArgs e)
        {
            // Öffnet das modale Fenster für die Spieleinstellungen
            modal = new StartModal();

            // Überprüft, ob "OK" ausgewählt wurde
            if ((bool)modal.ShowDialog())
            {
                // Setzt die Spielfeldgröße und initialisiert das Spielfeld
                spielfeldgroesse = modal.Spielgroesse;
                Init();
            }
        }

        private void UpdateCurrentPlayerLabel()
        {
            currentPlayerLabel.Content = "Aktueller Spieler: " + currentPlayerName;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            remainingTimePerPlayer[currentPlayerIndex]--;

            if (remainingTimePerPlayer[currentPlayerIndex] <= 0)
            {
                timer.Stop();
                MessageBox.Show("Zeit abgelaufen für Spieler " + playerNames[currentPlayerIndex]);
                Close();
            }

            UpdateRemainingTimeLabel();
        }

        private void UpdateRemainingTimeLabel()
        {
            zeit_spieler_1.Content = TimeSpan.FromSeconds(remainingTimePerPlayer[0]).ToString(@"mm\:ss");
            zeit_spieler_2.Content = TimeSpan.FromSeconds(remainingTimePerPlayer[1]).ToString(@"mm\:ss");
        }
    }
}
