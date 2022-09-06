using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //Информация о нарушениях для таблицы пункта 2.3.е ///////////////////////////////////////////////
    public class ViolationInfo : Base
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

        //код нарушения
        private int? violationCode;
        public int? ViolationCode
        {
            get { return violationCode; }
            set
            {
                violationCode = value;
                OnPropertyChanged("ViolationCode");
            }
        }

        //описание нарушителя
        private Person person;
        public Person Person
        {
            get { return person; }
            set
            {
                person = value;
                OnPropertyChanged("Person");
            }
        }
    }
}
