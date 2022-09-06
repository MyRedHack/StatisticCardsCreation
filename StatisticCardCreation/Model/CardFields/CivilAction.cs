using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //Предъявление гражданского иска //////////////////////////////////////////////
    public class CivilAction : Base
    {
        //код иска
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

        //сумма
        private long? sum;
        public long? Sum
        {
            get { return sum; }
            set
            {
                sum = value;
                OnPropertyChanged("Sum");
            }
        }

    }
}
