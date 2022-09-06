using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //Описание преступления для таблицы пункта 2.3 //////////////////////////////////////////////
    public class CrimeDescription : Base
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

        //квалификация
        private CrimeQualification qualification;
        public CrimeQualification Qualification
        {
            get { return qualification; }
            set
            {
                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }

        //преступная группа
        private CrimeGroup group;
        public CrimeGroup Group
        {
            get { return group; }
            set
            {
                group = value;
                OnPropertyChanged("Group");
            }
        }

        //мотив преступления
        public int? crimeMotive;
        public int? CrimeMotive
        {
            get { return crimeMotive; }
            set
            {
                crimeMotive = value;
                OnPropertyChanged("CrimeMotive");
            }
        }

        //преступления раскрыто (каким путем)
        public int? detection;
        public int? Detection
        {
            get { return detection; }
            set
            {
                detection = value;
                OnPropertyChanged("Detection");
            }
        }
    }
}
