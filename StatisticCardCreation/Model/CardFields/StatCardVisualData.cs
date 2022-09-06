using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class StatCardVisualData : Base
    {
        public StatCardVisualData()
        {
            Types.Add(typeof(CardF1), "Ф-1");
            Types.Add(typeof(CardF1DotOne), "Ф-1.1");
            Types.Add(typeof(CardF1DotTwo), "Ф-1.2");
            Types.Add(typeof(CardF1DotThree), "Ф-1.3");
            Types.Add(typeof(CardF2), "Ф-2");
            Types.Add(typeof(CardF3), "Ф-3");
            Types.Add(typeof(CardF3DotOne), "Ф-3.1");
            Types.Add(typeof(CardF5), "Ф-5");
            Types.Add(typeof(CardF5DotFive), "Ф-5.5");
            Types.Add(typeof(CardF6), "Ф-6");
        }

        public string CardTypeStr { get; set; }

        private Type cardType;
        public Type CardType
        {
            get => cardType;
            set
            {
                cardType = value;
                CardTypeStr = Types[value];
            }
        }
        public DateTime Date { get; set; }

        private Boolean sentToServer;
        public Boolean SentToServer
        {
            get => sentToServer;
            set
            {
                sentToServer = value;
                OnPropertyChanged("SentToServer");

                SentToServerImagePath = sentToServer ? "Resources\\check.png" : "Resources\\cross.png";
            }
        }

        private string sentToServerImagePath;
        public string SentToServerImagePath
        {
            get => sentToServerImagePath;
            set
            {
                sentToServerImagePath = value;
                OnPropertyChanged("SentToServerImagePath");
            }
        }
        private string id;
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Path { get; set; }

        private Dictionary<Type, string> Types = new Dictionary<Type, string>();
        private void GetStringType()
        {
            
        }
    }
}
