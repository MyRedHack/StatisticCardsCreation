using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StatisticProceeding
{
    public class Base : INotifyPropertyChanged
    {
        #region реализация интерфейса

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion реализация интерфейса
    }
}
