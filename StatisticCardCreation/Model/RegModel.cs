
using System;
using System.Collections.Generic;
using System.Text;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public class RegModel: Base, ICloneable
    {

        #region код министерства
        private int ministryCode;
        public int MinistryCode
        {
            get { return ministryCode; }
            set
            {
                ministryCode = value;
                OnPropertyChanged("MinistryCode");
            }
        }
        #endregion

        #region код направления
        private int directionCode;
        public int DirectionCode
        {
            get { return directionCode; }
            set
            {
                directionCode = value;
                OnPropertyChanged("DirectionCode");
            }
        }
        #endregion

        #region код управления
        private int managementCode;
        public int ManagementCode
        {
            get { return managementCode; }
            set
            {
                managementCode = value;
                OnPropertyChanged("ManagementCode");
            }
        }
        #endregion

        #region код отдела
        private int departmentCode;
        public int DepartmentCode
        {
            get { return departmentCode; }
            set
            {
                departmentCode = value;
                OnPropertyChanged("DepartmentCode");
            }
        }
        #endregion

        public OperatingPerson User { get; set; } = new OperatingPerson(true);

        public OperatingPerson VSOchef { get; set; }
        public OperatingPerson MilitaryProsecutor { get; set; }
        public Boolean Match(RegModel RegModel)
        {
            Boolean result = true;

            if (this.ministryCode != RegModel.ministryCode) result = false;
            if (this.directionCode != RegModel.directionCode) result = false;
            if (this.managementCode != RegModel.managementCode) result = false;
            if (this.departmentCode != RegModel.departmentCode) result = false;
            if (this.User.Position != RegModel.User.Position) result = false;
            if (this.User.Rank != RegModel.User.Rank) result = false;
            if (this.User.FIO != RegModel.User.FIO) result = false;
            
            return result;
        }

        public object Clone()
        {
            var result = (RegModel)this.MemberwiseClone();
            result.User = (OperatingPerson)this.User.Clone();
            result.VSOchef = (OperatingPerson)this.VSOchef.Clone();
            result.MilitaryProsecutor = (OperatingPerson)this.MilitaryProsecutor.Clone();

            return result;
        }
    }

    public class OperatingPerson : Base, ICloneable
    {
        bool addingToDataBase;
        public OperatingPerson(bool addingToDataBase = false)
        {
            this.addingToDataBase = addingToDataBase;
        }

        private int? position;
        public int? Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged("Position");
                if (addingToDataBase) StatCardFileTree.AddChangedField(new ChangedField("investigatorPosition", $"'{value}'"));
            }
        }

        private int? rank;
        public int? Rank
        {
            get => rank;
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
                if (addingToDataBase) StatCardFileTree.AddChangedField(new ChangedField("investigatorRank", $"'{value}'"));
            }
        }

        private string fio;
        public string FIO
        {
            get => fio;
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
                if (addingToDataBase) StatCardFileTree.AddChangedField(new ChangedField("investigatorFIO", $"'{value}'"));
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
