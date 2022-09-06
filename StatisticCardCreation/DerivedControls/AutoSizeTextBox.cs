using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace StatCardApp.DerivedControls
{
    public class AutoSizeTextBox : TextBox
    {
        public AutoSizeTextBox() : base()
        {
            this.Loaded += ChangeFont;
            this.IsReadOnly = true;
            this.BorderBrush = Brushes.White;
            this.BorderThickness = new Thickness(0, 0, 0, 0);
            this.TextWrapping = TextWrapping.Wrap;
            this.FontSize = 16.32;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Right;
            this.Margin = new Thickness(0, 2, 8, 2);
        }

        private void ChangeFont(object sender, RoutedEventArgs arg)
        {
            TextBox tb = (TextBox)sender;
            while (tb.ExtentHeight > (tb.ActualHeight - tb.Padding.Top - tb.Padding.Bottom))
            {
                if (tb.FontSize > 1)
                {
                    tb.FontSize--;
                    tb.UpdateLayout();
                }
                else return;
            }
        }

    }
}
