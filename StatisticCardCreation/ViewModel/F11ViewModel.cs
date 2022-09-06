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
    #region VM КАРТОЧКИ Ф-1.1

    public class ViewModelF1DotOne : ViewModel
    {

        public ViewModelF1DotOne(DirectoryInfo casePathInfo, CardF1DotOne F11 = null) : base(F11)
        {
            #region Инициализация свойств карточки

            this.casePathInfo = casePathInfo;
            if (F11 != null) this.F11 = F11;
            this.F11.CardType = CardType.F11;
            this.F11.TypeOfCard = this.F11.GetType();
            ChangeState.IsChanged = false;
            Card = this.F11;
            this.F11.ClearFields += ClearFieldsF11;
            #endregion Инициализация свойств карточки

            if (!this.F11.IsExported)
            {
                #region инициализация массивов объектов карточки Ф-1.1
                F1List = StatCardFunc.GetStatCardList<CardF1>(casePathInfo.FullName, CardType.F1);

                List<CardF1> MainF1List = F1List.FindAll(StatCardFunc.MainCardPredicate);
                List<CardF1> ChangedF1List = F1List.FindAll(StatCardFunc.ChangedCardPredicate);
                List<CardF1> UpdatedF1List = new List<CardF1>();

                F1List.Clear();
                foreach(CardF1 MainF1 in MainF1List)
                {
                    CardF1 F1ToAdd = MainF1;
                    foreach(CardF1 ChangedF1 in ChangedF1List)
                    {
                        if (ChangedF1.CardCreationDate == MainF1.CardCreationDate)
                            F1ToAdd += ChangedF1;
                    }

                    F1List.Add(F1ToAdd);
                }

                foreach (CardF1 F1 in F1List)
                {
                    if (F1.ChangeNumber == 0)
                    {
                        this.F11.Crimes = new List<CrimeDescription>();
                        this.F11.ViolationInfos = new List<ViolationInfo>();
                        this.F11.MaterialDamageInfos = new List<MaterialDamageInfo>();
                        this.F11.Things = new List<OE_ThingWithdrawed>();

                        //Список преступлений
                        CrimeDescription crime = new CrimeDescription();
                        crime.CrimeNumber = F1.CrimeNumber;
                        crime.CrimeMotive = F1.CrimeMotive;
                        crime.Detection = F1.DetectionRequirement;
                        crime.Group = F1.CrimeGroup;
                        crime.Qualification = F1.CrimeQualification;

                        this.F11.Crimes.Add(crime);

                        //Список нарушений
                        ViolationInfo violationInfo = new ViolationInfo();
                        violationInfo.CrimeNumber = F1.CrimeNumber;
                        violationInfo.ViolationCode = F1.ViolationInfo;
                        violationInfo.Person = F1.CrimePersDescription;

                        this.F11.ViolationInfos.Add(violationInfo);

                        //Список материального ущерба
                        if (F1.owns == null) F1.owns = new ObservableCollection<OE_Own>();
                        foreach (OE_Own el in F1.owns)
                        {
                            MaterialDamageInfo damageInfo = new MaterialDamageInfo();
                            damageInfo.CrimeNumber = F1.CrimeNumber;
                            damageInfo.Own = el;
                            this.F11.MaterialDamageInfos.Add(damageInfo);
                        }

                        //Список изъятых предметов
                        try
                        {
                            this.F11.Things.AddRange(F1.things);
                        }
                        catch { }
                    }
                }

                #endregion инициализация массивов объектов карточки Ф-1.1
            }
        }
        //Карточка Ф-1.1
        private CardF1DotOne _F11 = new CardF1DotOne();

        public CardF1DotOne F11
        {
            get { return _F11; }
            set
            {
                _F11 = value;
                OnPropertyChanged("F11");
            }
        }

        //Список с карточками Ф-1
        public List<CardF1> F1List;

        //Элемент для добавления в список гражданский иск
        private CivilAction civilActionToAdd = new CivilAction();

        public CivilAction CivilActionToAdd
        {
            get => civilActionToAdd;
            set
            {
                civilActionToAdd = value;
                OnPropertyChanged("CivilActionToAdd");
            }
        }

        //Элемент для добавления в список научно-технические средства
        private SciTechMeanUse sciTechMeanToAdd = new SciTechMeanUse();

        public SciTechMeanUse SciTechMeanToAdd
        {
            get => sciTechMeanToAdd;
            set
            {
                sciTechMeanToAdd = value;
                OnPropertyChanged("SciTechMeanToAdd");
            }
        }

        //Элемент для добавления в список экспертиз
        private Expertize expertizeToAdd = new Expertize();

        public Expertize ExpertizeToAdd
        {
            get => expertizeToAdd;
            set
            {
                expertizeToAdd = value;
                OnPropertyChanged("ExpertizeToAdd");
            }
        }

        //Элемент для добавления в список информации о мерах
        private ActionInfo actionInfoToAdd = new ActionInfo();

        public ActionInfo ActionInfoToAdd
        {
            get => actionInfoToAdd;
            set
            {
                actionInfoToAdd = value;
                OnPropertyChanged("ActionInfoToAdd");
            }
        }

        #region Команда добавления объекта в список гражданского иска

        private RelayCommand addToCivilActionList;

        public RelayCommand AddToCivilActionList
        {
            get
            {
                return addToCivilActionList ?? (
                    addToCivilActionList = new RelayCommand(obj =>
                    {
                        F11.CivilActions.Add(CivilActionToAdd);
                        CivilActionToAdd = new CivilAction();
                    }));
            }
        }

        #endregion Команда добавления объекта в список гражданского иска

        #region Команда добавления объекта в список научно-технических средства

        private RelayCommand addToSciTechMeansList;

        public RelayCommand AddToSciTechMeansList
        {
            get
            {
                return addToSciTechMeansList ?? (
                    addToSciTechMeansList = new RelayCommand(obj =>
                    {
                        F11.SciTechMeanUses.Add(SciTechMeanToAdd);
                        SciTechMeanToAdd = new SciTechMeanUse();
                    }));
            }
        }

        #endregion Команда добавления объекта в список научно-технических средства

        #region Команда добавления объекта в список экспертиз

        private RelayCommand addToExpertizeList;

        public RelayCommand AddToExpertizeList
        {
            get
            {
                return addToExpertizeList ?? (
                    addToExpertizeList = new RelayCommand(obj =>
                    {
                        F11.Expertizes.Add(ExpertizeToAdd);
                        ExpertizeToAdd = new Expertize();
                    }));
            }
        }

        #endregion Команда добавления объекта в список экспертиз

        #region Команда добавления объекта в список информации о мерах

        private RelayCommand addToActionInfoList;

        public RelayCommand AddToActionInfoList
        {
            get
            {
                return addToActionInfoList ?? (
                    addToActionInfoList = new RelayCommand(obj =>
                    {
                        F11.ActionInfos.Add(ActionInfoToAdd);
                        ActionInfoToAdd = new ActionInfo();
                    }));
            }
        }

        #endregion Команда добавления объекта в список информации о мерах

        #region Команда удаления объекта из списка гражданского иска

        private RelayCommand removeFromCivilActionList;

        public RelayCommand RemoveFromCivilActionList
        {
            get
            {
                return removeFromCivilActionList ?? (
                    removeFromCivilActionList = new RelayCommand(obj =>
                    {
                        F11.CivilActions.Remove((CivilAction)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка гражданского иска

        #region Команда удаления объекта из списка научно-технических средств

        private RelayCommand removeFromSciTechMeansList;

        public RelayCommand RemoveFromSciTechMeansList
        {
            get
            {
                return removeFromSciTechMeansList ?? (
                    removeFromSciTechMeansList = new RelayCommand(obj =>
                    {
                        F11.SciTechMeanUses.Remove((SciTechMeanUse)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка научно-технических средств

        #region Команда удаления объекта из списка экспертиз

        private RelayCommand removeFromExpertizeList;

        public RelayCommand RemoveFromExpertizeList
        {
            get
            {
                return removeFromExpertizeList ?? (
                    removeFromExpertizeList = new RelayCommand(obj =>
                    {
                        F11.Expertizes.Remove((Expertize)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка экспертиз

        #region Команда удаления объекта из списка информации о мерах

        private RelayCommand removeFromActionInfoList;

        public RelayCommand RemoveFromActionInfoList
        {
            get
            {
                return removeFromActionInfoList ?? (
                    removeFromActionInfoList = new RelayCommand(obj =>
                    {
                        F11.ActionInfos.Remove((ActionInfo)ItemToRemove);
                    }));
            }
        }

        #endregion Команда удаления объекта из списка информации о мерах

        private void ClearFieldsF11()
        {
            if (this.F11.Accounting == 9)
            {
                this.F11 = new CardF1DotOne();

                this.F11.Accounting = 9;
                this.F11.CardType = CardType.F11;
                this.F11.TypeOfCard = this.F11.GetType();

                Card = this.F11;
            }

        }
    }

    #endregion VM КАРТОЧКИ Ф-1.1
}