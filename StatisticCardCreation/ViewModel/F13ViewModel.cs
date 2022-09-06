using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-1.3

    public class ViewModelF1DotThree : ViewModel
    {
        public ViewModelF1DotThree(DirectoryInfo casePathInfo, CardF1DotThree F13 = null) : base(F13)
        {
            this.casePathInfo = casePathInfo;
            if (F13 != null) this.F13 = F13;

            this.F13.CardType = CardType.F13;
            this.F13.TypeOfCard = this.F13.GetType();
            ChangeState.IsChanged = false;
            Card = this.F13;
            MeasureToAdd = new OnSuspectMeasure();

            F12List = StatCardFunc.GetStatCardList<CardF1DotTwo>(casePathInfo.FullName, CardType.F12);

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

            foreach (CardF1DotTwo F12 in F12List)
            {
                FIOList.Add(new El(F12.SuspectNumber.Value, $"{F12.FIO.Surname} {F12.FIO.Name} {F12.FIO.Patronymic}"));
            }
        }

        //Карточка Ф-1.3
        private CardF1DotThree _F13 = new CardF1DotThree();

        public CardF1DotThree F13
        {
            get => _F13;
            set
            {
                _F13 = value;
                OnPropertyChanged("F13");
            }
        }

        //Объект класса процессуальной меры к лицу
        private OnSuspectMeasure measure;

        public OnSuspectMeasure MeasureToAdd
        {
            get => measure;
            set
            {
                measure = value;
                OnPropertyChanged("MeasureToAdd");
            }
        }

        //Данные списков
        public ListsData Data { get; set; } = new ListsData();

        //Список Ф.И.О. подозреваемых
        public List<El> FIOList { get; set; } = new List<El>();

        public List<CardF1DotTwo> F12List { get; set; } = new List<CardF1DotTwo>();


        #region Команда добавления объекта в список мер принуждения

        private RelayCommand addToMeasureList;

        public RelayCommand AddToMeasureList
        {
            get
            {
                return addToMeasureList ?? (
                    addToMeasureList = new RelayCommand(obj =>
                    {
                        F13.Measures.Add(MeasureToAdd);
                        MeasureToAdd = new OnSuspectMeasure();
                    }));
            }
        }

        #endregion Команда добавления объекта в список мер принуждения

        #region Команда удаления объекта из списка мер принуждений

        private RelayCommand removeFromMeasureList;

        public RelayCommand RemoveFromMeasureList
        {
            get
            {
                return removeFromMeasureList ?? (
                    removeFromMeasureList = new RelayCommand(obj =>
                    {
                        F13.Measures.Remove((OnSuspectMeasure)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка мер принуждений
    }

    #endregion VM КАРТОЧКИ Ф-1.3
}