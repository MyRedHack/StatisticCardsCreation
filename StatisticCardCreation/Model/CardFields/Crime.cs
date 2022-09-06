using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //--------------ПРЕСТУПЛЕНИЯ, СОВЕРШЕННЫЕ ЛИЦОМ----------------//
    public class Crime : Base
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
        private CrimeQualification qualification = new CrimeQualification();
        public CrimeQualification Qualification
        {
            get => qualification;
            set
            {
                qualification = value;
                OnPropertyChanged("Qualification");
            }
        }

        //соучастие
        private int? complicity;
        public int? Complicity
        {
            get => complicity;
            set
            {
                complicity = value;
                OnPropertyChanged("Complicity");
            }
        }

        //состояние
        private int? situation;
        public int? Situation
        {
            get => situation;
            set
            {
                situation = value;
                OnPropertyChanged("Situation");
            }
        }

        //Сумма судебного штрафа
        private long? judicialFine;
        public long? JudicialFine
        {
            get => judicialFine;
            set
            {
                judicialFine = value;
                OnPropertyChanged("JudicialFine");
            }
        }

        //Стадия совершения преступления
        private int? crimeStage;
        public int? CrimeStage
        {
            get => crimeStage;
            set
            {
                crimeStage = value;
                OnPropertyChanged("CrimeStage");
            }
        }
        //Форма соучастия
        private int? complicityForm;
        public int? ComplicityForm
        {
            get => complicityForm;
            set
            {
                complicityForm = value;
                OnPropertyChanged("ComplicityForm");
            }
        }
    }
}
