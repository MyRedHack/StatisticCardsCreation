using System;
using System.Windows;

namespace StatCardApp.DerivedControls
{
    public class DerivedControlEventArgs : RoutedEventArgs
    {
        public Boolean HasError { get; }
        public ErrorAdded ErrorAdded { get; }

        public DerivedControlEventArgs(Boolean err, ErrorAdded add) : base()
        {
            HasError = err;
            ErrorAdded = add;
        }
    }
}