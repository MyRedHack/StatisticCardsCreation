using StatCardApp.Model;

namespace StatCardApp.ViewModel
{
    public class RegViewModel : Base
    {
        public ListsData Data { get; set; } = new ListsData();

        private RegModel regInfo = new RegModel();

        public RegModel RegInfo
        {
            get { return regInfo; }
            set
            {
                regInfo = value;
                OnPropertyChanged("RegInfo");
            }
        }

        public RegViewModel()
        {
            RegInfo.VSOchef = new OperatingPerson();
            RegInfo.MilitaryProsecutor = new OperatingPerson();

            RegInfo.VSOchef.FIO = "Иванов И.И.";
            RegInfo.VSOchef.Rank = 1;
            RegInfo.VSOchef.Position = 10;

            RegInfo.MilitaryProsecutor.FIO = "Петров П.П.";
            RegInfo.MilitaryProsecutor.Rank = 1;
        }
    }
}