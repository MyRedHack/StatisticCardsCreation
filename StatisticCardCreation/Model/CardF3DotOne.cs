using System;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Function;
using System.Collections.Generic;
using StatCardApp.Model.CardFields;

namespace StatCardApp.Model
{
    public class CardF3DotOne : StatisticCard
    {
        private string caseDestination;
        public string CaseDestination
        {
            get => caseDestination;
            set
            {
                caseDestination = value;
                OnPropertyChanged("CaseDestination");
            }
        }

        private string outgoingNumber;
        public string OutgoingNumber
        {
            get => outgoingNumber;
            set
            {
                outgoingNumber = value;
                OnPropertyChanged("OutgoingNumber");
            }
        }

        private DateTime date = DateTime.Today;
        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F31 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F31Window w = new F31Window((CardF3DotOne)F31, FilePath);
            w.Show();
        }
        #endregion

        public List<F13Crime> CrimeTable { get; set; } = new List<F13Crime>();
        public List<F13Crime> CrimeTable2 { get; set; } = new List<F13Crime>();
    }

    public class F13Crime
    {
        public int? CrimeNumber { get; set; }
        public CrimeQualification Qualification { get; set; } = new CrimeQualification();
        public DateTime? RegistrationDate { get; set; }
    }
}
