using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.Collections.Generic;
using System.IO;
using System;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-1.2

    public class ViewModelF1DotTwo : ViewModel
    {
        public ViewModelF1DotTwo(DirectoryInfo casePathInfo, CardF1DotTwo F12 = null) : base(F12)
        {
            #region Инициализация свойств карточки

            this.casePathInfo = casePathInfo;
            if (F12 != null) this.F12 = F12;

            this.F12.CardType = CardType.F12;
            this.F12.TypeOfCard = this.F12.GetType();
            ChangeState.IsChanged = false;
            Card = this.F12;
            this.F12.ClearFields += ClearF12;
            if (!this.F12.IsExported) F1List = StatCardFunc.GetStatCardList<CardF1>(casePathInfo.FullName, CardType.F1);

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
            crimeStages = new int?[F1List.Count];
            complicityForms = new int?[F1List.Count];

            #endregion Инициализация свойств карточки

            #region Инициализация списков модели

            int i = 0;
            foreach (CardF1 F1 in F1List)
            {
                if (F1.ChangeNumber == 0)
                {
                    CrimeNumbersList.Add(new El(F1.CrimeNumber.Value, F1.CrimeNumber.Value.ToString()));
                    qualifications[i] = F1.CrimeQualification;
                    crimeStages[i] = F1.FinishedCompositionSign;
                    complicityForms[i] = F1.CrimeGroup.Characteristic;
                    i++;
                }
            }

            #endregion Инициализация списков модели
        }

        //Карточка Ф-1.2
        private CardF1DotTwo _F12 = new CardF1DotTwo();
        public CardF1DotTwo F12
        {
            get => _F12;
            set
            {
                _F12 = value;
                OnPropertyChanged("F12");
            }
        }

        //Список с карточками Ф-1
        public List<CardF1> F1List { get; set; } = new List<CardF1>();

        //Список с номерами преступлений
        public List<El> CrimeNumbersList { get; set; } = new List<El>();

        //Список с квалификациями преступлений
        private CrimeQualification[] qualifications;

        private int?[] crimeStages;
        private int?[] complicityForms;

        //Элемент для добавления в список пресутплений
        private Crime crimeToAdd = new Crime();
        public Crime CrimeToAdd
        {
            get => crimeToAdd;
            set
            {
                crimeToAdd = value;
                OnPropertyChanged("CrimeToAdd");
            }
        }

        private El officialStatusToAdd = new El();
        public El OfficialStatusToAdd
        {
            get => officialStatusToAdd;
            set
            {
                officialStatusToAdd = value;
                OnPropertyChanged("OfficialStatusToAdd");
            }
        }

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
                    CrimeToAdd.Qualification = qualifications[currentCrimeNumber];
                }
            }
        }

        #region Команда добавления объекта в список преступлений

        private RelayCommand addToCrimeList;

        public RelayCommand AddToCrimeList
        {
            get
            {
                return addToCrimeList ?? (
                    addToCrimeList = new RelayCommand(obj =>
                    {
                        CrimeToAdd.CrimeStage = crimeStages[currentCrimeNumber];
                        CrimeToAdd.ComplicityForm = complicityForms[currentCrimeNumber];
                        F12.Crimes.Add(CrimeToAdd);
                        CrimeToAdd = new Crime();
                    },
                    (obj)=>currentCrimeNumber>=0));
            }
        }

        #endregion Команда добавления объекта в список преступлений

        #region Команда удаления объекта из списка преступлений

        private RelayCommand removeFromCrimeList;

        public RelayCommand RemoveFromCrimeList
        {
            get
            {
                return removeFromCrimeList ?? (
                    removeFromCrimeList = new RelayCommand(obj =>
                    {
                        F12.Crimes.Remove((Crime)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка преступлений

        #region Команда добавления объекта в должностных положений
        private RelayCommand addToOfficialStatusList;

        public RelayCommand AddToOfficialStatusList
        {
            get
            {
                return addToOfficialStatusList ?? (
                    addToOfficialStatusList = new RelayCommand(obj =>
                    {
                        F12.OfficialStatus.Add(OfficialStatusToAdd);
                        OfficialStatusToAdd = new El();
                    }));
            }
        }
        #endregion


        #region Команда удаления объекта из списка должностных положений

        private RelayCommand removeFromOfficialStatusList;

        public RelayCommand RemoveFromOfficialStatusList
        {
            get
            {
                return removeFromOfficialStatusList ?? (
                    removeFromOfficialStatusList = new RelayCommand(obj =>
                    {
                        F12.OfficialStatus.Remove((El)ItemToRemove);
                        OfficialStatusToAdd = new El();
                    }));
            }
        }

        #endregion Команда удаления объекта из списка должностных положений
        private void ClearF12()
        {
            if (this.F12.Accounting == 9)
            {
                int? suspectNumber = this.F12.SuspectNumber;
                DateTime creationDate = this.F12.CardCreationDate;

                this.F12 = new CardF1DotTwo();
                this.F12.SuspectNumber = suspectNumber;
                this.F12.Accounting = 9;
                this.F12.CardType = CardType.F12;
                this.F12.TypeOfCard = this.F12.GetType();
                this.F12.CardCreationDate = creationDate;

                Card = this.F12;
            }

        }
    }

    
    #endregion VM КАРТОЧКИ Ф-1.2
}