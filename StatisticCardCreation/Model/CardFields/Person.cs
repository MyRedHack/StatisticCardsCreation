using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Global;

namespace StatCardApp.Model.CardFields
{
    public class Person : Base
    {
        #region должность
        private string position;
        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged("Position");
                StatCardFileTree.AddChangedField(new ChangedField("position", $"'{value}'"));
            }
        }
        #endregion

        #region звание
        private string rank;
        public string Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
                StatCardFileTree.AddChangedField(new ChangedField("rank", $"'{value}'"));
            }
        }
        #endregion

        #region фамилия и инициалы
        private string fio;
        public string FIO
        {
            get { return fio; }
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
                StatCardFileTree.AddChangedField(new ChangedField("fio", $"'{value}'"));
            }
        }
        #endregion

        public string StringView
        {
            get { return position + " " + rank + " " + fio; }
        }
    }
}
