using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Global;

namespace StatCardApp.Model.CardFields
{
    //-----------------ОПИСАНИЕ МЕСТА---------------------------//
    public class Place : Base, ICloneable
    {
        public Place() { }
        public Place(string str, string table = "")
        {
            variableParent = str;
            DBTableName = table;
        }
        string DBTableName;
        string variableParent = string.Empty;
        string DBFieldName;

        private string locality;
        public string Locality
        {
            get => locality;
            set
            {
                locality = value;
                OnPropertyChanged("Locality");
                if (variableParent == "birth") DBFieldName = "birthLocality";
                if (variableParent == "live") DBFieldName = "liveLocality";
                if (string.IsNullOrEmpty(variableParent)) DBFieldName = "Locality";
                StatCardFileTree.AddChangedField(new ChangedField(DBFieldName, $"'{value}'", DBTableName));
            }
        }

        private string area;
        public string Area
        {
            get => area;
            set
            {
                area = value;
                OnPropertyChanged("Area");
                if (variableParent == "birth") DBFieldName = "birthArea";
                if (variableParent == "live") DBFieldName = "liveArea";
                if (string.IsNullOrEmpty(variableParent)) DBFieldName = "Area";
                StatCardFileTree.AddChangedField(new ChangedField(DBFieldName, $"'{value}'", DBTableName));
            }

        }

        private string region;
        public string Region
        {
            get => region;
            set
            {
                region = value;
                OnPropertyChanged("Region");
                if (variableParent == "birth") DBFieldName = "birthRegion";
                if (variableParent == "live") DBFieldName = "liveRegion";
                if (string.IsNullOrEmpty(variableParent)) DBFieldName = "Region";
                StatCardFileTree.AddChangedField(new ChangedField(DBFieldName, $"'{value}'", DBTableName));
            }
        }

        private string address;
        public string Address
        {
            get => address;
            set
            {
                address = value;
                OnPropertyChanged("Address");
                if (variableParent == "live") DBFieldName = "liveAddress";
                StatCardFileTree.AddChangedField(new ChangedField(DBFieldName, $"'{value}'", DBTableName));
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
