using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _03_TicTacToe_Dynamisch
{
    class Rechteck
    {
        public Rectangle rect = new Rectangle();
        bool wurdeGeklickt;
        int owner;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public bool WurdeGeklickt
        {
            get { return wurdeGeklickt; }
            set { wurdeGeklickt = value; }
        }

        public int Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        public Rechteck(double breite = 100, double hoehe = 100, int i = 100, int j = 100)
        {
            rect.Width = breite;
            rect.Height = hoehe;

            Canvas.SetLeft(rect, (i + 1) * breite);
            Canvas.SetTop(rect, (j + 1) * hoehe);

            rect.Fill = Brushes.Aquamarine;
            rect.Stroke = Brushes.SteelBlue;
            rect.StrokeThickness = 2;

            wurdeGeklickt = false;

        }

        public void Zeichne(Canvas c)
        {
            if (!c.Children.Contains(rect)) // Wenn das Canvas KEINE rechtecke bereits hat...
            {
                c.Children.Add(rect);       // Füge die rechtecke hinzu
            }
        }

        public void Loesche(Canvas c)
        {
            if (c.Children.Contains(rect))
            {
                c.Children.Remove(rect);
            }
        }

        public void Grosse(double sx, double sy)
        {
            rect.Width = sx * rect.Width;
            rect.Height = sy * rect.Height;

            Canvas.SetLeft(Rect, sx * Canvas.GetLeft(Rect));
            Canvas.SetTop(Rect, sy * Canvas.GetTop(Rect));
        }

        

        
    }
}
