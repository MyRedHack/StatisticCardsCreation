using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //Мера принуждения к лицу
    public class OnSuspectMeasure : Base
    {

        public OnSuspectMeasure()
        {
            CoerciveMeasureDateStart = null;
            CoerciveMeasureDateEnd = null;
        }

        //Номер лица
        private int? suspectNumber;
        public int? SuspectNumber
        {
            get => suspectNumber;
            set
            {
                suspectNumber = value;
                OnPropertyChanged("SuspectNumber");
            }
        }

        //Фамилия, имя, отчество
        private string fio;
        public string FIO
        {
            get => fio;
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        //Вид меры принуждения
        private int? coerciveMeasureCode;
        public int? CoerciveMeasureCode
        {
            get => coerciveMeasureCode;
            set
            {
                coerciveMeasureCode = value;
                OnPropertyChanged("CoerciveMeasureCode");
            }
        }

        //Дата применения меры (начало)
        private DateTime? coerciveMeasureDateStart;
        public DateTime? CoerciveMeasureDateStart
        {
            get => coerciveMeasureDateStart;
            set
            {
                coerciveMeasureDateStart = value;
                OnPropertyChanged("CoerciveMeasureDateStart");
            }
        }

        //Дата окончания действия меры
        private DateTime? coerciveMeasureDateEnd;
        public DateTime? CoerciveMeasureDateEnd
        {
            get => coerciveMeasureDateEnd;
            set
            {
                coerciveMeasureDateEnd = value;
                OnPropertyChanged("CoerciveMeasureDateEnd");
            }
        }

        //Длительность ареста
        private int? measureDuration;
        public int? MeasureDuration
        {
            get
            {
                return measureDuration;
            }
            set
            {
                measureDuration = value;
                OnPropertyChanged("MeasureDuration");
            }
        }
    }
}
