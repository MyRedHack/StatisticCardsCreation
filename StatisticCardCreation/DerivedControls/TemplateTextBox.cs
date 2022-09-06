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
    public class TemplateTextBox : TextBox
    {
        public TemplateTextBox() : base()
        {
            LostFocus += SetError;
        }

        static TemplateTextBox()
        {
            HasErrorProperty = DependencyProperty.Register(
                "HasError",
                typeof(Boolean),
                typeof(TemplateTextBox));
        }

        public delegate void ErrorEvent(object sender, DerivedControlEventArgs e);

        public event ErrorEvent ErrorChanged;

        private ErrorAdded ErrorAdded;

        public static readonly DependencyProperty HasErrorProperty;
        public Boolean HasError
        {
            get { return (Boolean)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        private void SetError(object sender, EventArgs e)
        {
            Validate();
            ChangeFieldColor();
        }

        private void ChangeFieldColor()
        {
            if (HasError)
            {
                this.BorderBrush = Brushes.Red;
                this.ToolTip = "Значение не соответствует формату";
            }
            else
            {
                this.BorderBrush = Brushes.DarkGray;
                this.ToolTip = null;
            }
        }

        private void Validate()
        {
            Regex template = new Regex(@"^[А-Я]{2}-[0-9]{6}$");
            MatchCollection matches = template.Matches(this.Text);

            if (matches.Count == 0)
            {
                if (!HasError)
                {
                    HasError = true;
                    ErrorAdded = ErrorAdded.Added;
                }
                else
                {
                    ErrorAdded = ErrorAdded.None;
                }
                ErrorChanged?.Invoke(this, new DerivedControlEventArgs(HasError, ErrorAdded));
            }
            else
            {
                if (HasError)
                {
                    HasError = false;
                    ErrorAdded = ErrorAdded.Removed;
                }
                else
                {
                    ErrorAdded = ErrorAdded.None;
                }
                ErrorChanged?.Invoke(this, new DerivedControlEventArgs(HasError, ErrorAdded));
            }
        }
    }
}
