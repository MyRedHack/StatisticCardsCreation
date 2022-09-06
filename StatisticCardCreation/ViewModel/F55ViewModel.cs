using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.Collections.Generic;
using System.IO;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-5.5

    public class ViewModelF5DotFive : ViewModel
    {
        public ViewModelF5DotFive(DirectoryInfo casePathInfo, CardF5DotFive F55 = null) : base(F55)
        {
            #region Инициализация свойств карточки
            this.F55 = new CardF5DotFive();
            OwnCrimeToAdd = new OwnCrime();
            this.casePathInfo = casePathInfo;
            if (F55 != null) this.F55 = F55;
            this.F55.CardType = CardType.F55;
            this.F55.TypeOfCard = this.F55.GetType();
            this.F55.ClearFields += ClearFieldsF55;
            ChangeState.IsChanged = false;
            Card = this.F55;
            if (!this.F55.IsExported) F1List = StatCardFunc.GetStatCardList<CardF1>(casePathInfo.FullName, CardType.F1);
            List<CardF1> MainF1List = F1List.FindAll(StatCardFunc.MainCardPredicate);
            List<CardF1> ChangedF1List = F1List.FindAll(StatCardFunc.ChangedCardPredicate);
            List<CardF1> UpdatedF1List = new List<CardF1>();

            F1List.Clear();
            foreach (CardF1 MainF1 in MainF1List)
            {
                CardF1 F1ToAdd = MainF1;
                foreach (CardF1 ChangedF1 in ChangedF1List)
                {
                    if (ChangedF1.CardCreationDate == MainF1.CardCreationDate)
                        F1ToAdd += ChangedF1;
                }

                F1List.Add(F1ToAdd);
            }
            qualifications = new CrimeQualification[F1List.Count];

            #endregion Инициализация свойств карточки

            #region Инициализация списков модели

            int i = 0;
            foreach (CardF1 F1 in F1List)
            {
                if (F1.ChangeNumber == 0)
                {
                    CrimeNumbersList.Add(new El(F1.CrimeNumber.Value, F1.CrimeNumber.Value.ToString()));
                    qualifications[i] = F1.CrimeQualification;
                    i++;
                }
            }

            #endregion Инициализация списков модели
        }

        //Карточка Ф5.5
        private CardF5DotFive _F55 = new CardF5DotFive();

        public CardF5DotFive F55
        {
            get => _F55;
            set
            {
                _F55 = value;
                OnPropertyChanged("F55");
            }
        }

        //Список с карточками Ф-1
        public List<CardF1> F1List { get; set; } = new List<CardF1>();

        //Список с номерами преступлений
        public List<El> CrimeNumbersList { get; set; } = new List<El>();

        //Текущий выбранный номер преступления
        private int currentCrimeNumber = -1;

        public int CurrentCrimeNumber
        {
            get => currentCrimeNumber;
            set
            {
                currentCrimeNumber = value;
                OnPropertyChanged("CurrentCrimeNumber");

                if (currentCrimeNumber >= 0)
                {
                    OwnCrimeToAdd.Qualification = qualifications[currentCrimeNumber];
                }
            }
        }

        //Список с квалификациями преступлений
        private CrimeQualification[] qualifications;

        //Элемент списка добавления в список преступлений
        private OwnCrime ownCrimeToAdd;

        public OwnCrime OwnCrimeToAdd
        {
            get => ownCrimeToAdd;
            set
            {
                ownCrimeToAdd = value;
                OnPropertyChanged("OwnCrimeToAdd");
            }
        }

        #region Команда добавления объекта в список преступлений

        private RelayCommand addToOwnCrimeList;

        public RelayCommand AddToOwnCrimeList
        {
            get
            {
                return addToOwnCrimeList ?? (
                    addToOwnCrimeList = new RelayCommand(obj =>
                    {
                        F55.OwnCrimes.Add(OwnCrimeToAdd);
                        OwnCrimeToAdd = new OwnCrime();
                    },
                    (obj)=>currentCrimeNumber>=0));
            }
        }

        #endregion Команда добавления объекта в список преступлений

        #region Команда удаления объекта из списка преступлений

        private RelayCommand removeFromOwnCrimecList;

        public RelayCommand RemoveFromOwnCrimecList
        {
            get
            {
                return removeFromOwnCrimecList ?? (
                    removeFromOwnCrimecList = new RelayCommand(obj =>
                    {
                        F55.OwnCrimes.Remove((OwnCrime)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка преступлений

        private void ClearFieldsF55()
        {
            if (this.F55.Accounting == 9)
            {
                int? victimNumber = this.F55.VictimNumber;
                this.F55 = new CardF5DotFive();
                this.F55.VictimNumber = victimNumber;
                this.F55.Accounting = 9;
                this.F55.CardType = CardType.F55;
                this.F55.TypeOfCard = this.F55.GetType();

                Card = this.F55;
            }

        }
    }

    #endregion VM КАРТОЧКИ Ф-5.5
}