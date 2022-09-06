using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class CardDestination : Base
    {
        //название органа
        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        //Индекс
        private int? index;
        public int? Index
        {
            get => index;
            set
            {
                index = value;
                OnPropertyChanged("Index");
            }
        }

        //почтовый адрес
        private string mailingAddress;
        public string MailingAddress
        {
            get => mailingAddress;
            set
            {
                mailingAddress = value;
                OnPropertyChanged("MailingAddress");
            }
        }
    }
}
