using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //Сведения о материальном ущербе для таблицы пункта 2.4 ////////////////////////////////////
    public class MaterialDamageInfo : Base
    {
        //номер преступления
        private int? crimeNumber;
        public int? CrimeNumber
        {
            get { return crimeNumber; }
            set
            {
                crimeNumber = value;
                OnPropertyChanged("CrimeNumber");
            }
        }

        //собственность
        private OE_Own own;
        public OE_Own Own
        {
            get { return own; }
            set
            {
                own = value;
                OnPropertyChanged("Own");
            }
        }

        //сумма ущерба
        private long? damageSum;
        public long? DamageSum
        {
            get { return damageSum; }
            set
            {
                damageSum = value;
                OnPropertyChanged("DamageSum");
            }
        }

        //сумма возмещения
        private long? repayment;
        public long? Repayment
        {
            get { return repayment; }
            set
            {
                repayment = value;
                OnPropertyChanged("Repayment");
            }
        }

        //изъято
        private long? withdrawed;
        public long? Withdrawed
        {
            get { return withdrawed; }
            set
            {
                withdrawed = value;
                OnPropertyChanged("Withdrawed");
            }
        }
    }
}
