using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-2

    public class ViewModelF2 : ViewModel
    {
        public ViewModelF2(DirectoryInfo casePathInfo, CardF2 F2 = null) : base(F2)
        {
            #region Инициализация свойств карточки

            this.casePathInfo = casePathInfo;

            if (F2 != null) this.F2 = F2;
            this.F2.CardType = CardType.F2;
            this.F2.TypeOfCard = this.F2.GetType();
            ChangeState.IsChanged = false;
            Card = this.F2;
            this.F2.ClearFields += ClearF2;

            if (!this.F2.IsExported) F12List = StatCardFunc.GetStatCardList<CardF1DotTwo>(casePathInfo.FullName, CardType.F12);
            List<CardF1DotTwo> MainF12List = F12List.FindAll(StatCardFunc.MainCardPredicate);
            List<CardF1DotTwo> ChangedF12List = F12List.FindAll(StatCardFunc.ChangedCardPredicate);
            List<CardF1DotTwo> UpdatedF12List = new List<CardF1DotTwo>();

            F12List.Clear();
            foreach (CardF1DotTwo MainF12 in MainF12List)
            {
                CardF1DotTwo F12ToAdd = MainF12;
                foreach (CardF1DotTwo ChangedF12 in ChangedF12List)
                {
                    if (ChangedF12.CardCreationDate == MainF12.CardCreationDate)
                        F12ToAdd += ChangedF12;
                }

                F12List.Add(F12ToAdd);
            }
            CrimeLists = new ObservableCollection<Crime>[F12List.Count];
            Accusations = new DateTime?[F12List.Count];
            Measures = new int?[F12List.Count];
            MeasuresDates = new DateTime?[F12List.Count];
            FIO = new FIO[F12List.Count];
            Places = new Place[F12List.Count];
            Birthdays = new DateTime?[F12List.Count];
            Data = new ListsData();

            #endregion Инициализация свойств карточки

            #region Заполнение массивов значениями

            int i = 0;
            foreach (CardF1DotTwo F12 in F12List)
            {
                if (F12.ChangeNumber == 0)
                {
                    Suspects.Add(new El(F12.SuspectNumber.Value, $"{F12.FIO.Surname} {F12.FIO.Name} {F12.FIO.Patronymic}"));
                    FIO[i] = new FIO();
                    FIO[i].Name = F12.FIO.Name;
                    FIO[i].Surname = F12.FIO.Surname;
                    FIO[i].Patronymic = F12.FIO.Patronymic;

                    CrimeLists[i] = F12.Crimes;
                    Accusations[i] = F12.AccusationDate;
                    Measures[i] = F12.CoersiveMeasure;
                    MeasuresDates[i] = F12.CoersiveMeasureDate;
                    Places[i] = F12.BirthPlace;
                    Birthdays[i] = F12.Birthday;
                    i++;
                }
            }

            #endregion Заполнение массивов значениями
        }
        //Карточка Ф-2
        private CardF2 _F2 = new CardF2();

        public CardF2 F2
        {
            get => _F2;
            set
            {
                _F2 = value;
                OnPropertyChanged("F2");
            }
        }

        //Данные списков
        public ListsData Data { get; set; }

        //Объект для добавления в список преступлений
        private Crime crime;
        public Crime Crime
        {
            get => crime;
            set
            {
                crime = value;
                OnPropertyChanged("Crime");
            }
        }

        //Список с карточками Ф-1.2
        public List<CardF1DotTwo> F12List { get; set; } = new List<CardF1DotTwo>();

        #region Списки данных карточки Ф-1.2

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        //Подозреваемые
        public List<El> Suspects { get; set; } = new List<El>();

        //Текущий выбранный подозреваемый
        private int currentSuspect = -1;

        public int CurrentSuspect
        {
            get => currentSuspect;
            set
            {
                currentSuspect = value;
                OnPropertyChanged("CurrentSuspect");
                if (currentSuspect >= 0)
                {
                    F2.Crimes = CrimeLists[currentSuspect];
                    F2.FIO = FIO[currentSuspect];
                    F2.F6Data.Birthday = Birthdays[currentSuspect];
                    F2.F6Data.BirthPlace = Places[currentSuspect];
                }
                else
                {
                    F2.Crimes = new ObservableCollection<Crime>();
                    F2.FIO = new Model.CardFields.FIO();
                    F2.F6Data.Birthday = null;
                    F2.F6Data.BirthPlace = new Place();
                }
            }
        }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        //Список списков преступлений
        public ObservableCollection<Crime>[] CrimeLists { get; set; }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        //Даты обвинений
        public DateTime?[] Accusations { get; set; }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //Меры принуждений
        public int?[] Measures { get; set; }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //Даты применения мер принуждения
        public DateTime?[] MeasuresDates { get; set; }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //Даты применения мер принуждения
        public FIO[] FIO { get; set; }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //Места рождения
        public Place[] Places { get; set; }

        //||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
        //Даты рождения
        public DateTime?[] Birthdays { get; set; }

        #endregion Списки данных карточки Ф-1.2

        private void ClearF2()
        {
            if (this.F2.Accounting == 9)
            {
                int? suspectNumber = this.F2.SuspectNumber;
                DateTime creationDate = this.F2.CardCreationDate;

                this.F2 = new CardF2();
                
                this.F2.SuspectNumber = suspectNumber;
                this.F2.Accounting = 9;
                this.F2.CardType = CardType.F2;
                this.F2.TypeOfCard = this.F2.GetType();
                this.F2.CardCreationDate = creationDate;

                Card = this.F2;
            }

        }

    }

    #endregion VM КАРТОЧКИ Ф-2
}