using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;

namespace StatCardApp.DerivedControls
{
    class FontAdjustTextBlock : TextBlock
    {
        double TempHeight;

        public FontAdjustTextBlock() : base()
        {
            TempHeight = this.ActualHeight;
            this.SizeChanged += ReduceFontSize;
        }

        static FontAdjustTextBlock()
        {

        }
  
        private void ReduceFontSize(object sender, EventArgs e)
        {
            
            if (TempHeight < ActualHeight)
            {
                this.FontSize--;
            }

            TempHeight = ActualHeight;
        }
    }
}
