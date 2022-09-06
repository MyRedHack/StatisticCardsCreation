using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Linq;

namespace StatCardApp.Model
{
    public class CaseInfoModel : Base, ICloneable
    {
        #region вид дела
        private int? caseType;
        public int? CaseType
        {
            get { return caseType; }
            set
            {
                caseType = value;
                OnPropertyChanged("CaseType");
            }
        }
        #endregion

        #region дата регистрации
        private DateTime? regDate;
        public DateTime? RegDate
        {
            get { return regDate; }
            set
            {
                regDate = value;
                OnPropertyChanged("RegDate");
            }
        }
        #endregion

        #region номер дела
        private int? caseNumber;
        public int? CaseNumber
        {
            get { return caseNumber; }
            set
            {
                caseNumber = value;
                OnPropertyChanged("CaseNumber");
            }
        }
        #endregion

        #region полный номер дела
        public long FullCaseNumber { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion


    }
}
