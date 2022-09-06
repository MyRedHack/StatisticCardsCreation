using System.ComponentModel;
using System.Runtime.CompilerServices;
using StatCardApp.Global;

namespace StatCardApp
{
    public class Base : INotifyPropertyChanged
    {
        #region реализация интерфейса

        public event PropertyChangedEventHandler PropertyChanged;

        public void  OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                ChangeState.IsChanged = true;
            }
        }

        #endregion реализация интерфейса
    }
}