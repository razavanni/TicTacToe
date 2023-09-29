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

namespace _03_TicTacToe_Dynamisch
{
    /// <summary>
    /// Interaktionslogik für StartModal.xaml
    /// </summary>
    public partial class StartModal : Window
    {
        public Int32 Spielgroesse { get; set; }
        public string SpielerName_1 { get; set; }
        public string SpielerName_2 { get; set; }
        public Int32 ZeitProSpieler { get; set; }

        public StartModal()
        {
            InitializeComponent();
        }

        private void Start_button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Spielgroesse = Convert.ToInt32(spielgroesse_input.Text);
                ZeitProSpieler = Convert.ToInt32(zeit_input.Text);

                if (Spielgroesse < 3 || Spielgroesse > 8)
                {
                    throw new Exception("Die Spielfeldgröße muss zwischen 3 und 8 sein!");
                }
                else
                {
                    SpielerName_1 = spieler_1_input.Text;
                    SpielerName_2 = spieler_2_input.Text;
                    DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler: " + ex.Message, "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // GOT FOCUS METHODEN
        private void Spielgroesse_input_GotFocus(object sender, RoutedEventArgs e)
        {
            spielgroesse_input.Text = " ";
        }
        private void Spieler_1_input_GotFocus(object sender, RoutedEventArgs e)
        {
            spieler_1_input.Text = " ";
        }
        private void Spieler_2_input_GotFocus(object sender, RoutedEventArgs e)
        {
            spieler_2_input.Text = " ";
        }
        private void Zeit_input_GotFocus(object sender, RoutedEventArgs e)
        {
            zeit_input.Text = "";
        }

        // LOST FOCUS METHODEN
        private void Spielgroesse_input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(spielgroesse_input.Text))
            {
                spielgroesse_input.Text = "3";
            }
        }
        private void Spieler_1_input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(spieler_1_input.Text))
            {
                spieler_1_input.Text = "Hans";
            }
        }
        private void Spieler_2_input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(spieler_2_input.Text))
            {
                spieler_2_input.Text = "Franz";
            }
        }
        private void Zeit_input_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(zeit_input.Text))
            {
                zeit_input.Text = "Franz";
            }
        }
    }
}
