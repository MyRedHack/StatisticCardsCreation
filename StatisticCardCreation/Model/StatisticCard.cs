using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Linq;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using MaterialDesignThemes.Wpf;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public enum CardType { F1 = 1, F11 = 2, F12 = 3, F13 = 4, F2 = 5, F3 = 6, F31 = 7, F5 = 8, F55 = 9, F6 = 10, F61 = 11 }
    public class StatisticCard : Base, ICloneable
    {
        public StatCardVisualData VisualData { get; set; } = new StatCardVisualData();

        private Boolean sentToServer;
        public Boolean SentToServer
        {
            get => sentToServer;
            set
            {
                sentToServer = value;
                VisualData.SentToServer = value;
            }
        }
        public CardType CardType { get; set; }
        public Boolean InheritedPropertiesChanged { get; set; }
        public DateTime CardCreationDate { get; set; }
        public RegModel RegData { get; set; }
        public CaseInfoModel CaseInfo { get; set; }
        public Boolean DependentDataChanged { get; set; }
        public long CaseNumber
        {
            get
            {
                if(RegData != null && CaseInfo != null)
                {
                    string number = string.Empty;
                    number += CaseInfo.CaseType;
                    number += CaseInfo.RegDate.Value.Year.ToString().Substring(2, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.MinistryCode, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.DirectionCode, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.ManagementCode, 2);
                    number += StatCardFunc.ConvertSizeValue(RegData.DepartmentCode, 2);
                    number += StatCardFunc.ConvertSizeValue(CaseInfo.CaseNumber, 6);

                    return Convert.ToInt64(number);

                }
                else
                {
                    return 0;
                }
            }
        }
        public string docVer { get; set; }
        //учесть
        private int? accounting;

        public Type TypeOfCard { get; set; }
        //учесть
        public int? Accounting
        {
            get { return accounting; }
            set
            {
                accounting = value;
                ClearFields?.Invoke();
                if (value == 9) StatCardFileTree.ChangedFields = new List<ChangedField>();

                OnPropertyChanged("Accounting");
            }
        }

        public Boolean IsExported { get; set; } = false;
        //Номер изменения
        public long ChangeNumber { get; set; }
        public DateTime ChangeDate { get; set; }
        public Boolean ChangedLast { get; set; } = true;
        //дата направления карточки прокурору
        private DateTime? transCardDate;
        public DateTime? TransCardDate
        {
            get { return transCardDate; }
            set
            {
                transCardDate = value;
                OnPropertyChanged("TransCardDate");
            }
        }

        private bool directorIsTemporary;
        public bool DirectorIsTemporary
        {
            get => directorIsTemporary;
            set
            {
                directorIsTemporary = value;
                OnPropertyChanged("DirectorIsTemporary");
            }
        }

        private bool directorIsMain;
        public bool DirectorIsMain
        {
            get => directorIsMain;
            set
            {
                directorIsMain = value;
                OnPropertyChanged("DirectorIsMain");
            }
        }


        public delegate void StatisticCardHandler();
        public event StatisticCardHandler ClearFields;

        public virtual void CallWindow(FileInfo casePath)
        {
            MessageBox.Show("");
        }

        public object Clone()
        {
            var result = (StatisticCard)this.MemberwiseClone();

            result.RegData = (RegModel)this.RegData.Clone();
            result.CaseInfo = (CaseInfoModel)this.CaseInfo.Clone();

            return result;

        }

        public List<ChangedField> ChangedFields { get; set; }

    }

    public class ChangedField
    {
        public string Field { get; set; }

        public string Value { get; set; }

        public string Table { get; set; }
        public ChangedField() { }
        public ChangedField(string field, string value, string table = "")
        {
            Field = field;
            Value = value;
            Table = table;
        }

    }
}
