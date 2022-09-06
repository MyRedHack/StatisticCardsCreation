using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //экспертиза /////////////////////////////////////////////////
    public class Expertize : Base
    {
        //код экспертизы
        private int? expertizeCode;
        public int? ExpertizeCode
        {
            get { return expertizeCode; }
            set
            {
                expertizeCode = value;
                OnPropertyChanged("ExpertizeCode");
            }
        }
        //Дргуая экспертиза (00)
        private string otherExpertize;
        public string OtherExpertize
        {
            get => otherExpertize;
            set
            {
                otherExpertize = value;
                OnPropertyChanged("OtherExpertize");
            }
        }
        //код учреждения
        private int? instituteCode;
        public int? InstituteCode
        {
            get { return instituteCode; }
            set
            {
                instituteCode = value;
                OnPropertyChanged("InstituteCode");
            }
        }

        //количество проведенных экспертиз
        private int? count;
        public int? Count
        {
            get { return count; }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        //стоимость экспертиз
        private long? cost;
        public long? Cost
        {
            get { return cost; }
            set
            {
                cost = value;
                OnPropertyChanged("Cost");
            }
        }
    }
}
