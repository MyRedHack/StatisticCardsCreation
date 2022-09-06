using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace StatCardApp.DerivedControls
{
    public enum ErrorAdded { Added = 1, Removed = 2, None = 3 }

    public class FiltredComboBox : ComboBox
    {
        public delegate void ErrorEvent(object sender, DerivedControlEventArgs e);

        public event ErrorEvent ErrorChanged;

        public ICollectionView MyCollection { get; set; }

        public static readonly DependencyProperty HasErrorProperty;

        public Boolean HasError
        {
            get { return (Boolean)GetValue(HasErrorProperty); }
            set { SetValue(HasErrorProperty, value); }
        }

        private ErrorAdded ErrorAdded;

        public string TBtext { get; set; }

        System.Collections.IEnumerable DefaultCollection;
        #region Логика фильтрации

        public bool Filter(object emp)
        {
            El element = emp as El;
            //MessageBox.Show(element.Str.ToLower().Contains(TBtext).ToString());
            return element.FullValue.Trim(' ').ToLower().Contains(this.Text.Trim(' ').ToLower());
        }

        #endregion Логика фильтрации

        public FiltredComboBox() : base()
        {
            this.KeyUp += UpdateText;
            this.LostKeyboardFocus += SetCBValue;
            this.StaysOpenOnEdit = true;
            this.DropDownOpened += SelectFullCollection;
            this.SelectionChanged += UpdateAfterSelection;
            this.DropDownClosed += LoseFocus;
        }

        static FiltredComboBox()
        {
            HasErrorProperty = DependencyProperty.Register(
                "HasError",
                typeof(Boolean),
                typeof(FiltredComboBox));
        }

        private void TextValidation()
        {
            
            if (this.Items.Count == 0)
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

        private void UpdateText(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Up && e.Key != Key.Down )
            {
                if (e.Key == Key.Enter)
                {
                    this.IsDropDownOpen = false;
                }
                else
                {
                    if (MyCollection == null)
                    {
                        MyCollection = CollectionViewSource.GetDefaultView(this.ItemsSource);
                        DefaultCollection = this.ItemsSource;
                        this.ItemsSource = MyCollection;
                        
                    }

                    if (this.SelectedItem != null)
                    {
                        TBtext = this.Text;
                        this.SelectedItem = null;
                        this.Text = TBtext;
                    }
                    else
                    {
                        MyCollection.Filter += Filter;
                    }

                    TextValidation();
                    if (HasError == true)
                    {
                        this.ToolTip = "Данного значения нет в списке";
                        this.BorderBrush = Brushes.Red;
                        this.IsDropDownOpen = false;
                    }
                    else
                    {
                        this.ToolTip = null;
                        this.BorderBrush = Brushes.DarkGray;
                        this.IsDropDownOpen = true;
                        Right();
                    }
                }
            }

        }

        private void DropDown(object sender, KeyEventArgs e)
        {
            if (!HasError)
            {
                if (this.IsDropDownOpen == false)
                    this.IsDropDownOpen = true;
            }
        }

        private void SetCBValue(object sender, EventArgs e)
        {
            if (!HasError && this.SelectedItem == null && this.Text != String.Empty)
                this.SelectedItem = this.Items[0];
        }

        private void SelectionClear(object sender, EventArgs e)
        {
            Right();
        }

        private void Right()
        {
            var key = Key.Right;
            var target = Keyboard.FocusedElement;
            var routedEvent = Keyboard.KeyDownEvent;

            target.RaiseEvent(
              new KeyEventArgs(
                Keyboard.PrimaryDevice,
                PresentationSource.FromVisual((Visual)target),
                0,
                key)
              { RoutedEvent = routedEvent }
            );
        }

        private void SelectFullCollection(object sender, EventArgs e)
        {

            if (MyCollection != null)
            {
                this.MyCollection.Filter = null;
            }
        }

        private void UpdateAfterSelection(object sender, SelectionChangedEventArgs args)
        {
            TextValidation();
            this.ToolTip = null;
            this.BorderBrush = Brushes.DarkGray;

        }

        private void LoseFocus(object sender, EventArgs e)
        {
            Keyboard.ClearFocus();
        }

    }
}