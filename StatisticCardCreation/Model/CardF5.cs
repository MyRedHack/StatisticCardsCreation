using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public class CardF5 : CardF1DotTwo
    {
        //Порядковый номер пострадавшего
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

        //Список преступлений
        public ObservableCollection<VictimCrime> VictimCrimes { get; set; } = new ObservableCollection<VictimCrime>();

        //Преступление спровоцировано неправомерными действиями потерпевшего и связано
        private int? provocativeActions;
        public int? ProvocativeActions
        {
            get => provocativeActions;
            set
            {
                provocativeActions = value;
                OnPropertyChanged("ProvocativeActions");
                StatCardFileTree.AddChangedField(new ChangedField("provocativeActions", $"{value}"));
            }
        }

        //Потерпевший по отношению к совершившему преступление являлся
        private int? crimePersonsRelation;
        public int? CrimePersonsRelation
        {
            get => crimePersonsRelation;
            set
            {
                crimePersonsRelation = value;
                OnPropertyChanged("CrimePersonsRelation");
                StatCardFileTree.AddChangedField(new ChangedField("crimePersonsRelation", $"{value}"));
            }
        }

        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F5 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F5Window w = new F5Window((CardF5)F5, FilePath);
            w.Show();
        }
        #endregion
    }
}
