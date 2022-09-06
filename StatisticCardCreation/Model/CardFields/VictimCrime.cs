using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class VictimCrime : Base
    {
        //номер преступления
        private int? crimeNumber;
        public int? CrimeNumber
        {
            get => crimeNumber;
            set
            {
                crimeNumber = value;
                OnPropertyChanged("CrimeNumber");
            }
        }

        //квалификация преступления
        private CrimeQualification qualification;
        public CrimeQualification Qualification
        {
            get => qualification;
            set
            {
                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }

        //Характер последствий
        private int? consequencesNature;
        public int? ConsequencesNature
        {
            get => consequencesNature;
            set
            {
                consequencesNature = value;
                OnPropertyChanged("ConsequencesNature");
            }
        }

        //состояние
        private int? condition;
        public int? Condition
        {
            get => condition;
            set
            {
                condition = value;
                OnPropertyChanged("Condition");
                if (condition == 1)
                {
                    HelplessCondition = 1;
                }
                else
                {
                    HelplessCondition = null;
                }
            }
        }

        //Беспомощное состояние
        public int? HelplessCondition { get; set; }
    }
}
