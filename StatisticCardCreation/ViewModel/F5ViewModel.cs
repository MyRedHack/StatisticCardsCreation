using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.Collections.Generic;
using System.IO;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-5

    public class ViewModelF5 : ViewModel
    {
        public ViewModelF5(DirectoryInfo casePathInfo, CardF5 F5 = null) : base(F5)
        {
            #region Инициализация свойств карточки

            VictimCrimeToAdd = new VictimCrime();
            this.casePathInfo = casePathInfo;
            if (F5 != null) this.F5 = F5;

            this.F5.SuspectNumber = 0;
            this.F5.CardType = CardType.F5;
            this.F5.TypeOfCard = this.F5.GetType();
            this.F5.ClearFields += ClearFieldsF5;
            ChangeState.IsChanged = false;
            Card = this.F5;
            if (!this.F5.IsExported) F1List = StatCardFunc.GetStatCardList<CardF1>(casePathInfo.FullName, CardType.F1);
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

        //Карточка Ф-1.2
        private CardF5 _F5 = new CardF5();

        public CardF5 F5
        {
            get => _F5;
            set
            {
                _F5 = value;
                OnPropertyChanged("F5");
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
                    VictimCrimeToAdd.Qualification = qualifications[currentCrimeNumber];
                }
            }
        }

        //Список с квалификациями преступлений
        private CrimeQualification[] qualifications;

        //Элемент списка добавления в список преступлений
        private VictimCrime vicCrimeToAdd;

        public VictimCrime VictimCrimeToAdd
        {
            get => vicCrimeToAdd;
            set
            {
                vicCrimeToAdd = value;
                OnPropertyChanged("VictimCrimeToAdd");
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

        #region Команда добавления объекта в список преступлений

        private RelayCommand addToVicCrimeList;

        public RelayCommand AddToVicCrimeList
        {
            get
            {
                return addToVicCrimeList ?? (
                    addToVicCrimeList = new RelayCommand(obj =>
                    {
                        F5.VictimCrimes.Add(VictimCrimeToAdd);
                        VictimCrimeToAdd = new VictimCrime();
                    },
                    (obj) => currentCrimeNumber >= 0));
            }
        }

        #endregion Команда добавления объекта в список преступлений

        #region Команда удаления объекта из списка преступлений

        private RelayCommand removeFromVicCrimecList;

        public RelayCommand RemoveFromVicCrimecList
        {
            get
            {
                return removeFromVicCrimecList ?? (
                    removeFromVicCrimecList = new RelayCommand(obj =>
                    {
                        F5.VictimCrimes.Remove((VictimCrime)ItemToRemove);
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
                        F5.OfficialStatus.Add(OfficialStatusToAdd);
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
                        F5.OfficialStatus.Remove((El)ItemToRemove);
                        OfficialStatusToAdd = new El();
                    }));
            }
        }

        #endregion Команда удаления объекта из списка должностных положений

        private void ClearFieldsF5()
        {
            if (this.F5.Accounting == 9)
            {
                int? victimNumber = this.F5.VictimNumber;
                this.F5 = new CardF5();
                this.F5.VictimNumber = victimNumber;
                this.F5.Accounting = 9;
                this.F5.CardType = CardType.F5;
                this.F5.TypeOfCard = this.F5.GetType();

                Card = this.F5;
            }

        }
    }


    #endregion VM КАРТОЧКИ Ф-5
}