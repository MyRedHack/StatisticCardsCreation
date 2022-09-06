using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;

namespace StatCardApp.Model
{
    public class CardF1DotThree : StatisticCard
    {
        //Список мер принуждений
        public ObservableCollection<OnSuspectMeasure> Measures { get; set; } = new ObservableCollection<OnSuspectMeasure>();

        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F13 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F13Window w = new F13Window((CardF1DotThree)F13, FilePath);
            w.Show();
        }
        #endregion
    }
}
