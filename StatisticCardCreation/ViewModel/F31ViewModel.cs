using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.IO;
using System.Collections.Generic;
using System;
using StatCardApp.Global;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-3.1

    public class ViewModelF3DotOne : ViewModel
    {
        public ViewModelF3DotOne(DirectoryInfo casePathInfo, CardF3DotOne F31 = null) : base(F31)
        {
            this.casePathInfo = casePathInfo;
            if (F31 != null) this.F31 = F31;
            this.F31.CardType = CardType.F31;
            this.F31.TypeOfCard = this.F31.GetType();
            ChangeState.IsChanged = false;
            Card = this.F31;


            if (!this.F31.IsExported)
            {
                this.F31.CrimeTable = new List<F13Crime>();
                this.F31.CrimeTable2 = new List<F13Crime>();

                F1List = StatCardFunc.GetStatCardList<CardF1>(casePathInfo.FullName, CardType.F1);

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

                foreach (CardF1 F1 in F1List)
                {
                    F13Crime crime = new F13Crime();
                    crime.CrimeNumber = F1.CrimeNumber;
                    crime.Qualification = F1.CrimeQualification;
                    crime.RegistrationDate = F1.CardCreationDate;

                    this.F31.CrimeTable.Add(crime);

                    if (this.F31.CrimeTable.Count == 15)
                    {
                        for (int i = 15; i < F1List.Count; i++)
                        {
                            F13Crime crime1 = new F13Crime();
                            crime1.CrimeNumber = F1List[i].CrimeNumber;
                            crime1.Qualification = F1List[i].CrimeQualification;
                            crime1.RegistrationDate = F1List[i].CardCreationDate;
                            this.F31.CrimeTable2.Add(crime1);
                        }

                        break;
                    }
                }

                int CrimeTableCount = this.F31.CrimeTable.Count;
                for (int i = 0; i < 15 - CrimeTableCount; i++)
                {
                    this.F31.CrimeTable.Add(new F13Crime());
                }

                int CrimeTable2Count = this.F31.CrimeTable2.Count;
                for (int i = 0; i < 15 - CrimeTable2Count; i++)
                {
                    this.F31.CrimeTable2.Add(new F13Crime());
                }
            }


           
        }

        public ListsData Data { get; set; } = new ListsData();

        private CardF3DotOne _F31 = new CardF3DotOne();
        
        public CardF3DotOne F31
        {
            get => _F31;
            set
            {
                _F31 = value;
                OnPropertyChanged("F31");
            }
        }

        public List<CardF1> F1List { get; set; } = new List<CardF1>();

    }



    #endregion VM КАРТОЧКИ Ф-3.1
}