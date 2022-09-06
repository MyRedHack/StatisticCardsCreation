using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Global;

namespace StatCardApp.Model.CardFields
{
    //--------------Ф.И.О.----------------//
    public class FIO : Base, ICloneable
    {
        public FIO() { }
        public FIO(string table) 
        {
            DBTableName = table;
        }
        string DBTableName;
        //Имя
        private string name;
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
                StatCardFileTree.AddChangedField(new ChangedField("name", $"'{value}'", DBTableName));
            }
        }

        //Фамилия
        private string surname;
        public string Surname
        {
            get => surname;
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
                StatCardFileTree.AddChangedField(new ChangedField("surname", $"'{value}'", DBTableName));
            }

        }

        //Отчество
        private string patronymic;
        public string Patronymic
        {
            get => patronymic;
            set
            {
                patronymic = value;
                OnPropertyChanged("Patronymic");
                StatCardFileTree.AddChangedField(new ChangedField("patronymic", $"'{value}'", DBTableName));
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
