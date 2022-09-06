using System.IO;
using Microsoft.Win32;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using System.Threading;
using System.Windows;
using System;
using StatCardApp.Global;
using StatCardApp.BaseClass;

namespace StatCardApp.ViewModel
{
    #region VM КАРТОЧКИ Ф-1
    public class ViewModelF1 : ViewModel
    {

        public ViewModelF1(DirectoryInfo casePathInfo, CardF1 F1 = null) : base(F1)
        {
            #region Инициализация свойств карточки Ф-1
            
            this.casePathInfo = casePathInfo;
            if (F1 != null) this.F1 = F1;

            this.F1.CardType = CardType.F1;
            this.F1.TypeOfCard = this.F1.GetType();
            ChangeState.IsChanged = false;

            Card = this.F1;
            Card.ClearFields += ClearFieldsF1;

            this.F1.ExtraInfo.CollectionChanged += this.F1.SetNationalProgramm;

            CaseHierarchy = StatCardFunc.GetCaseHierarchy();
            #endregion
        }

        //Карточка Ф-1
        private CardF1 _F1 = new CardF1();
        public CardF1 F1
        {
            get { return _F1; }
            set
            {
                _F1 = value;
                OnPropertyChanged("F1");
            }
        }

        //Элемент для добавления в список собственности
        private OE_Own ownToAdd = new OE_Own();
        public OE_Own OwnToAdd
        {
            get => ownToAdd;
            set
            {
                ownToAdd = value;
                OnPropertyChanged("OwnToAdd");
            }
        }

        //Элемент для добавления в список оружия
        private OE_Weapon weaponToAdd = new OE_Weapon();
        public OE_Weapon WeaponToAdd
        {
            get => weaponToAdd;
            set
            {
                weaponToAdd = value;
                OnPropertyChanged("WeaponToAdd");
            }
        }

        //Элемент для добавления в список изъятых вещей
        private OE_ThingWithdrawed thingToAdd = new OE_ThingWithdrawed();
        public OE_ThingWithdrawed ThingToAdd
        {
            get => thingToAdd;
            set
            {
                thingToAdd = value;
                OnPropertyChanged("ThingToAdd");
            }
        }

        //Элемент для добавления в список характеристики пострадавшего
        private El victimToAdd = new El();
        public El VictimToAdd
        {
            get => victimToAdd;
            set
            {
                victimToAdd = value;
                OnPropertyChanged("VictimToAdd");
            }
        }

        //Элемент для добавления в список дополнительной информации
        private El extraInfoToAdd = new El();
        public El ExtraInfoToAdd
        {
            get => extraInfoToAdd;
            set
            {
                extraInfoToAdd = value;
                OnPropertyChanged("ExtraInfoToAdd");
            }
        }

        //Элемент для добавления в список участников осмотра места происшествия
        private El sceneInspectionInvolvedPersonToAdd = new El();
        public El SceneInspectionInvolvedPersonToAdd
        {
            get => sceneInspectionInvolvedPersonToAdd;
            set
            {
                sceneInspectionInvolvedPersonToAdd = value;
                OnPropertyChanged("SceneInspectionInvolvedPersonToAdd");
            }
        }

        //Элемент для добавления в список временных обстоятельств
        private El tempConditionToAdd = new El();
        public El TempConditionToAdd
        {
            get => tempConditionToAdd;
            set
            {
                tempConditionToAdd = value;
                OnPropertyChanged("TempConditionToAdd");
            }
        }

        private CaseHierarchy caseHierarchy = new CaseHierarchy();
        public CaseHierarchy CaseHierarchy
        {
            get => caseHierarchy;
            set
            {
                caseHierarchy = value;
                OnPropertyChanged("CaseHierarchy");
            }
        }

        #region Команда добавления объекта в список собственности
        private RelayCommand addToOwnList;
        public RelayCommand AddToOwnList
        {
            get
            {
                return addToOwnList ?? (
                    addToOwnList = new RelayCommand(obj =>
                    {
                        F1.owns.Add(OwnToAdd);
                        OwnToAdd = new OE_Own();
                    }));
            }
        }
        #endregion

        #region Команда добавления объекта в список оружия
        private RelayCommand addToWeaponList;
        public RelayCommand AddToWeaponList
        {
            get
            {
                return addToWeaponList ?? (
                    addToWeaponList = new RelayCommand(obj =>
                    {
                        F1.weapons.Add(WeaponToAdd);
                        WeaponToAdd = new OE_Weapon();
                    }));
            }
        }
        #endregion

        #region Команда добавления объекта в список изъятых вещей
        private RelayCommand addToThingList;
        public RelayCommand AddToThingList
        {
            get
            {
                return addToThingList ?? (
                    addToThingList = new RelayCommand(obj =>
                    {
                        F1.things.Add(ThingToAdd);
                        ThingToAdd = new OE_ThingWithdrawed();
                    }));
            }
        }
        #endregion

        #region Команда добавления объекта в список характеристики пострадавшего
        private RelayCommand addToVictimList;
        public RelayCommand AddToVictimList
        {
            get
            {
                return addToVictimList ?? (
                    addToVictimList = new RelayCommand(obj =>
                    {

                        if (VictimToAdd != null)
                            F1.VictimCharacter.Add(VictimToAdd);
                        VictimToAdd = new El();
                    }));
            }
        }
        #endregion

        #region Команда добавления объекта в список доп.информации
        private RelayCommand addToExtraInfoList;
        public RelayCommand AddToExtraInfoList
        {
            get
            {
                return addToExtraInfoList ?? (
                    addToExtraInfoList = new RelayCommand(obj =>
                    {
                        F1.ExtraInfo.Add(ExtraInfoToAdd);
                        ExtraInfoToAdd = new El();
                    }));
            }
        }
        #endregion

        #region Команда добавления объекта в список участника осмотра места происшествия
        private RelayCommand addToSceneInvolvedList;
        public RelayCommand AddToSceneInvolvedList
        {
            get
            {
                return addToSceneInvolvedList ?? (
                    addToSceneInvolvedList = new RelayCommand(obj =>
                    {
                        F1.SceneInspectionInvolvedPerson.Add(SceneInspectionInvolvedPersonToAdd);
                        SceneInspectionInvolvedPersonToAdd = new El();
                    }));
            }
        }
        #endregion

        #region Команда добавления объекта в список временных обстоятельств
        private RelayCommand addToTempConditionList;
        public RelayCommand AddToTempConditionList
        {
            get
            {
                return addToTempConditionList ?? (
                    addToTempConditionList = new RelayCommand(obj =>
                    {
                        F1.TempCondition.Add(TempConditionToAdd);
                        TempConditionToAdd = new El();
                    }));
            }
        }
        #endregion


        #region Команда удаления объекта из списка характеристики пострадавшего
        private RelayCommand removeFromViictimList;
        public RelayCommand RemoveFromVictimList
        {
            get
            {
                return removeFromViictimList ?? (
                    removeFromViictimList = new RelayCommand(obj =>
                    {
                        F1.VictimCharacter.Remove((El)ItemToRemove);
                    }));
            }
        }
        #endregion

        #region Команда удаления объекта из списка оружия
        private RelayCommand removeFromWeaponList;
        public RelayCommand RemoveFromWeaponList
        {
            get
            {
                return removeFromWeaponList ?? (
                    removeFromWeaponList = new RelayCommand(obj =>
                    {
                        F1.weapons.Remove((OE_Weapon)ItemToRemove);
                    }));
            }
        }
        #endregion

        #region Команда удаления объекта из списка собственности
        private RelayCommand removeFromOwnList;
        public RelayCommand RemoveFromOwnList
        {
            get
            {
                return removeFromOwnList ?? (
                    removeFromOwnList = new RelayCommand(obj =>
                    {
                        F1.owns.Remove((OE_Own)ItemToRemove);
                    }));
            }
        }
        #endregion

        #region Команда удаления объекта из списка вещей
        private RelayCommand removeFromThingList;
        public RelayCommand RemoveFromThingList
        {
            get
            {
                return removeFromThingList ?? (
                    removeFromThingList = new RelayCommand(obj =>
                    {
                        F1.things.Remove((OE_ThingWithdrawed)ItemToRemove);
                    }));
            }
        }
        #endregion

        #region Команда удаления объекта из списка участников осмотра места происшествия
        private RelayCommand removeFromSceneInvolvedList;
        public RelayCommand RemoveFromSceneInvolvedList
        {
            get
            {
                return removeFromSceneInvolvedList ?? (
                    removeFromSceneInvolvedList = new RelayCommand(obj =>
                    {
                        F1.SceneInspectionInvolvedPerson.Remove((El)ItemToRemove);
                    }));
            }
        }
        #endregion

        #region Команда удаления объекта из списка временных обстоятельств
        private RelayCommand removeFromTempConditionList;
        public RelayCommand RemoveFromTempConditionList
        {
            get
            {
                return removeFromTempConditionList ?? (
                    removeFromTempConditionList = new RelayCommand(obj =>
                    {
                        F1.TempCondition.Remove((El)ItemToRemove);
                    }));
            }
        }
        #endregion

        #region Команда удаления объекта из списка доп. информации
        private RelayCommand removeFromExtraInfoList;
        public RelayCommand RemoveFromExtraInfoList
        {
            get
            {
                return removeFromExtraInfoList ?? (
                    removeFromExtraInfoList = new RelayCommand(obj =>
                    {
                        F1.ExtraInfo.Remove((El)ItemToRemove);
                    }));
            }
        }
        #endregion

        private void ClearFieldsF1()
        {
            if (this.F1.Accounting == 9)
            {
                int? crimeNumber = this.F1.CrimeNumber;
                DateTime creationDate = this.F1.CardCreationDate;
                this.F1 = new CardF1();
                this.F1.CardCreationDate = creationDate;
                this.F1.CrimeNumber = crimeNumber;
                this.F1.Accounting = 9;
                this.F1.CardType = CardType.F1;
                this.F1.TypeOfCard = this.F1.GetType();

                Card = this.F1;
            }

        }

    }
    #endregion
}
