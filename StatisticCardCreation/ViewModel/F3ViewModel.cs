using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.Collections.ObjectModel;
using System.IO;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-3

    public class ViewModelF3 : ViewModel
    {
        public ViewModelF3(DirectoryInfo casePathInfo, CardF3 F3 = null) : base(F3)
        {
            this.casePathInfo = casePathInfo;

            if (F3 != null) this.F3 = F3;
            this.F3.CardType = CardType.F3;
            this.F3.TypeOfCard = this.F3.GetType();
            ChangeState.IsChanged = false;
            Card = this.F3;
            CaseDecToAdd = new CaseDecision();
        }

        //Карточка Ф-3
        private CardF3 _F3 = new CardF3();

        public CardF3 F3
        {
            get => _F3;
            set
            {
                _F3 = value;
                OnPropertyChanged("F3");
            }
        }

        //Элемент решения для добавления в список
        private CaseDecision caseDecToAdd;

        public CaseDecision CaseDecToAdd
        {
            get => caseDecToAdd;
            set
            {
                caseDecToAdd = value;
                OnPropertyChanged("CaseDecToAdd");
            }
        }

        #region Команда добавления объекта в список решений по делу

        private RelayCommand addToCaseDecList;

        public RelayCommand AddToCaseDecList
        {
            get
            {
                return addToCaseDecList ?? (
                    addToCaseDecList = new RelayCommand(obj =>
                    {
                        F3.CaseDecisions.Add(CaseDecToAdd);

                        CaseDecToAdd = new CaseDecision();
                    }));
            }
        }

        #endregion Команда добавления объекта в список решений по делу

        #region Команда удаления объекта из списка решений по делу

        private RelayCommand removeFromCaseDecList;

        public RelayCommand RemoveFromCaseDecList
        {
            get
            {
                return removeFromCaseDecList ?? (
                    removeFromCaseDecList = new RelayCommand(obj =>
                    {
                        F3.CaseDecisions.Remove((CaseDecision)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка решений по делу
    }

    #endregion VM КАРТОЧКИ Ф-3
}