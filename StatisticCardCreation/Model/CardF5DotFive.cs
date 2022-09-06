using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public class CardF5DotFive : StatisticCard
    {
        //Порядковый номер пострадавшего лица
        private int? victimNumber;
        public int? VictimNumber
        {
            get => victimNumber;
            set
            {
                victimNumber = value;
                OnPropertyChanged("VictimNumber");
                
            }
        }

        //Наименование организации
        private string organizationName;
        public string OrganizationName
        {
            get => organizationName;
            set
            {
                organizationName = value;
                OnPropertyChanged("OrganizationName");
                StatCardFileTree.AddChangedField(new ChangedField("name", $"'{value}'"));
            }
        }

        //Форма собственности
        private int? ownForm;
        public int? OwnForm
        {
            get => ownForm;
            set
            {
                ownForm = value;
                OnPropertyChanged("OwnForm");
                StatCardFileTree.AddChangedField(new ChangedField("ownForm", $"{value}"));
            }
        }

        //Место расположения
        private Place place = new Place();
        public Place OrgPlace
        {
            get => place;
            set
            {
                place = value;
                OnPropertyChanged("OrgPlace");
            }
        }

        //Список преступлений 
        public ObservableCollection<OwnCrime> OwnCrimes { get; set; } = new ObservableCollection<OwnCrime>();

        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F55 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F55Window w = new F55Window((CardF5DotFive)F55, FilePath);
            w.Show();
        }
        #endregion
    }
}
