using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;

namespace StatCardApp.Model
{
    public class CardF6 : StatisticCard
    {
        //Ф.И.О.
        private FIO fio = new FIO();
        public FIO FIO
        {
            get => fio;
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        //Дата рождения
        private DateTime? birthday;
        public DateTime? Birthday
        {
            get => birthday;
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        //Место рождения
        private Place birthPlace = new Place();
        public Place BirthPlace
        {
            get => birthPlace;
            set
            {
                birthPlace = value;
                OnPropertyChanged("BirthPlace");
            }
        }

        //Дата направления карточки в суд
        private DateTime? sendingToCourt;
        public DateTime? SendingToCourt
        {
            get => sendingToCourt;
            set
            {
                sendingToCourt = value;
                OnPropertyChanged("SendingToCourt");
            }
        }
        //Место направления карточки
        private CardDestination destination = new CardDestination();
        public CardDestination Destination
        {
            get => destination;
            set
            {
                destination = value;
                MessageBox.Show("SET");
                OnPropertyChanged("Destination");
            }
        }

        private ObservableCollection<Crime> _Qualifications;
        public ObservableCollection<Crime> Qualifications
        {
            get => _Qualifications;
            set
            {
                _Qualifications = value;
                OnPropertyChanged("Qualifications");
            }
        }
        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F6 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F6Window w = new F6Window((CardF6)F6, FilePath);
            w.Show();
        }
        #endregion
    }
}
