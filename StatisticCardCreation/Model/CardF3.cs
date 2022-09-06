using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;

namespace StatCardApp.Model
{
    public class CardF3 : StatisticCard
    {
        //Список решений по делу
        public ObservableCollection<CaseDecision> CaseDecisions { get; set; } = new ObservableCollection<CaseDecision>();

        //требования прокурора (количество всего)
        private int? demandsTotal;
        public int? DemandsTotal
        {
            get { return demandsTotal; }
            set
            {
                demandsTotal = value;
                OnPropertyChanged("DemandsTotal");
            }
        }

        //требования прокурора (количество удовлетворено)
        private int? demandsAccepted;
        public int? DemandsAccepted
        {
            get { return demandsAccepted; }
            set
            {
                demandsAccepted = value;
                OnPropertyChanged("DemandsAccepted");
            }
        }

        //Срок предв следствия продлен на сколько месяцев 
        private int? termExtension;
        public int? TermExtension
        {
            get { return termExtension; }
            set
            {
                termExtension = value;
                OnPropertyChanged("TermExtension");
            }
        }

        //Срок предв следствия продлен до какой даты
        private DateTime? termExtensionDate;
        public DateTime? TermExtensionDate
        {
            get { return termExtensionDate; }
            set
            {
                termExtensionDate = value;
                OnPropertyChanged("TermExtensionDate");
            }
        }

        //Срок предв следствия продлен до какой даты
        private int? caseExtendReason;
        public int? CaseExtendReason
        {
            get { return caseExtendReason; }
            set
            {
                caseExtendReason = value;
                OnPropertyChanged("CaseExtendReason");
            }
        }


        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F3 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F3Window w = new F3Window((CardF3)F3, FilePath);
            w.Show();
        }
        #endregion
    }
}
