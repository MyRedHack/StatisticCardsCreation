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
    #region VM КАРТОЧКИ Ф-6

    public class ViewModelF6 : ViewModel
    {
        public ViewModelF6(DirectoryInfo casePathInfo, CardF6 F6 = null) : base(F6)
        {
            this.casePathInfo = casePathInfo;
            if (F6 != null) this.F6 = F6;
            this.F6.CardType = CardType.F6;
            this.F6.TypeOfCard = this.F6.GetType();
            ChangeState.IsChanged = false;
            Card = this.F6;
            
            CaseInfo = (CaseInfoModel)StatCardFunc.FromJson<CaseInfoModel>(casePathInfo.FullName + @"\caseInfo.conf");
            if (!this.F6.IsExported)
            {
                F2List = StatCardFunc.GetStatCardList<CardF2>(casePathInfo.FullName, CardType.F2);
                List<CardF2> MainF2List = F2List.FindAll(StatCardFunc.MainCardPredicate);
                List<CardF2> ChangedF2List = F2List.FindAll(StatCardFunc.ChangedCardPredicate);
                List<CardF2> UpdatedF12List = new List<CardF2>();

                F2List.Clear();
                foreach (CardF2 MainF2 in MainF2List)
                {
                    CardF2 F2ToAdd = MainF2;
                    foreach (CardF2 ChangedF2 in ChangedF2List)
                    {
                        if (ChangedF2.CardCreationDate == MainF2.CardCreationDate)
                            F2ToAdd += ChangedF2;
                    }

                    F2List.Add(F2ToAdd);
                }
                fio = new FIO[F2List.Count];
                birthPlaces = new Place[F2List.Count];
                birthdays = new DateTime?[F2List.Count];
                QualificationsF6 = new ObservableCollection<Crime>[F2List.Count];

                #region Заполнение массивов значениями

                int i = 0;
                foreach (CardF2 F2 in F2List)
                {
                    if (F2.ChangeNumber == 0)
                    {
                        Suspects.Add(new El(F2.SuspectNumber.Value, $"{F2.FIO.Surname} {F2.FIO.Name} {F2.FIO.Patronymic}"));
                        fio[i] = new FIO();
                        fio[i].Name = F2.FIO.Name;
                        fio[i].Surname = F2.FIO.Surname;
                        fio[i].Patronymic = F2.FIO.Patronymic;

                        birthPlaces[i] = F2.F6Data.BirthPlace;
                        birthdays[i] = F2.F6Data.Birthday;
                        QualificationsF6[i] = F2.Crimes;
                        i++;
                    }
                }

                #endregion Заполнение массивов значениями
            }

        }

        //Карточка Ф-6
        private CardF6 _F6 = new CardF6();

        public CardF6 F6
        {
            get => _F6;
            set
            {
                _F6 = value;
                OnPropertyChanged("F6");
            }
        }

        private CaseInfoModel CaseInfo = new CaseInfoModel();

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
                    F6.FIO = fio[currentSuspect];
                    F6.BirthPlace = birthPlaces[currentSuspect];
                    F6.Birthday = birthdays[currentSuspect];
                    F6.Qualifications = QualificationsF6[currentSuspect];
                }
            }
        }

        public ListsData Data { get; set; } = new ListsData();

        private List<CardF2> F2List;

        private FIO[] fio;
        private Place[] birthPlaces;
        private DateTime?[] birthdays;
        private ObservableCollection<Crime>[] QualificationsF6;

    }

    #endregion VM КАРТОЧКИ Ф-6
}