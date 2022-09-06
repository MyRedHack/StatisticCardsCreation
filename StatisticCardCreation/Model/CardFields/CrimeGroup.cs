using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Global;

namespace StatCardApp.Model.CardFields
{
    public class CrimeGroup : Base
    {
        //Характеристика группы
        private int? characteristic;

        public int? Characteristic
        {
            get { return characteristic; }
            set
            {
                characteristic = value;
                OnPropertyChanged("Characteristic");
                StatCardFileTree.AddChangedField(new ChangedField("characteristicOfGroup", $"{value}"));
            }
        }
        //Связи
        private int? links;
        public int? Links
        {
            get { return links; }
            set
            {
                links = value;
                OnPropertyChanged("Links");
                StatCardFileTree.AddChangedField(new ChangedField("linksOfGroup", $"{value}"));
            }
        }
        //Соучастники
        private int? accomplices;
        public int? Accomplices
        {
            get { return accomplices; }
            set
            {
                accomplices = value;
                OnPropertyChanged("Accomplices");
                StatCardFileTree.AddChangedField(new ChangedField("accomplicesOfGroup", $"{value}"));
            }
        }

        public string StringView
        {
            get { return characteristic + " " + links + " " + accomplices; }
        }
    }
}
