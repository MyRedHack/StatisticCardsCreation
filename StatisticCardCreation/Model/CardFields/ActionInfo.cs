using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //информация о принятии мер ////////////////////////////////////////////
    public class ActionInfo : Base
    {
        //код
        private int? code;
        public int? Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        //кому отправлено
        private string destination;
        public string Destination
        {
            get { return destination; }
            set
            {
                destination = value;
                OnPropertyChanged("Destination");
            }
        }

        //дата отправления
        private DateTime? date;
        public DateTime? Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

    }
}
