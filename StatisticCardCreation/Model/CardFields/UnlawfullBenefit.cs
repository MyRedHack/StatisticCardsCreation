using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Global;

namespace StatCardApp.Model.CardFields
{
    public class UnlawfullBenefit : Base
    {
        private int? valueCode;
        public int? ValueCode
        {
            get { return valueCode; }
            set
            {
                valueCode = value;
                OnPropertyChanged("ValueCode");
                StatCardFileTree.AddChangedField(new ChangedField("unlawfullBenefit", $"{value}"));
            }
        }

        private string valueStr;
        public string ValueStr
        {
            get { return valueStr; }
            set
            {
                valueStr = value;
                OnPropertyChanged("ValueStr");
            }
        }

        private int? sum;
        public int? Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                OnPropertyChanged("Sum");
                StatCardFileTree.AddChangedField(new ChangedField("unlawfullBenefitSum", $"{value}"));
            }
        }
    }
}
