using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public class CardF1DotOne : StatisticCard
    {

        //вид документа
        private int? documentType;
        public int? DocumentType
        {
            get { return documentType; }
            set
            {
                documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }

        //описания преступлений по пункту 2.3
        public List<CrimeDescription> Crimes { get; set; } = new List<CrimeDescription>();
        //сведения о нарушениях
        public List<ViolationInfo> ViolationInfos { get; set; } = new List<ViolationInfo>();
        //описание нарушителя
        public List<MaterialDamageInfo> MaterialDamageInfos { get; set; } = new List<MaterialDamageInfo>();

        //изъято имущества
        public List<OE_ThingWithdrawed> Things { get; set; } = new List<OE_ThingWithdrawed>();

        //статьи УПК
        private int? _UPKArticle;
        public int? UPKArticle
        {
            get { return _UPKArticle; }
            set
            {
                _UPKArticle = value;
                OnPropertyChanged("UPKArticle");
                StatCardFileTree.AddChangedField(new ChangedField("UPKArticle", $"'{value}'"));
            }
        }

        //Дата направления в суд
        private DateTime? _UPKArticleDate;
        public DateTime? UPKArticleDate
        {
            get { return _UPKArticleDate; }
            set
            {
                _UPKArticleDate = value;
                OnPropertyChanged("UPKArticleDate");
                StatCardFileTree.AddChangedField(new ChangedField("UPKArticleDate", $"'{value}'"));
            }
        }

        //где принято решение
        private int? decisionAccept;
        public int? DecisionAccept
        {
            get { return decisionAccept; }
            set
            {
                decisionAccept = value;
                OnPropertyChanged("DecisionAccept");
                StatCardFileTree.AddChangedField(new ChangedField("DecisionAccept", $"{value}"));
            }
        }

        //орган, в который направлено дело
        private string caseSendTo;
        public string CaseSendTo
        {
            get { return caseSendTo; }
            set
            {
                caseSendTo = value;
                OnPropertyChanged("CaseSendTo");
                StatCardFileTree.AddChangedField(new ChangedField("referralBody", $"'{value}'"));
            }
        }

        //дата направления дела в орган
        private DateTime? caseSendDate;
        public DateTime? CaseSendDate
        {
            get { return caseSendDate; }
            set
            {
                caseSendDate = value;
                OnPropertyChanged("CaseSendDate");
                StatCardFileTree.AddChangedField(new ChangedField("referralDate", $"'{value}'"));
            }
        }

        //исходящий номер направления дела
        private string caseSendOutNumber;
        public string CaseSendOutNumber
        {
            get { return caseSendOutNumber; }
            set
            {
                caseSendOutNumber = value;
                OnPropertyChanged("CaseSendOutNumber");
                StatCardFileTree.AddChangedField(new ChangedField("referralOutNum", $"'{value}'"));
            }
        }

        //№ дела, с которым соединено
        private long? connectCase;
        public long? ConnectCase
        {
            get { return connectCase; }
            set
            {
                connectCase = value;
                OnPropertyChanged("ConnectCase");
                StatCardFileTree.AddChangedField(new ChangedField("connectedCaseNum", $"{value}"));
            }
        }

        //в чьих интересах предъявлен гражданский иск
        public ObservableCollection<CivilAction> CivilActions { get; set; } = new ObservableCollection<CivilAction>();

        //применение научно-технических средств криминалистики
        public ObservableCollection<SciTechMeanUse> SciTechMeanUses { get; set; } = new ObservableCollection<SciTechMeanUse>();

        //проведено экспертиз
        public ObservableCollection<Expertize> Expertizes { get; set; } = new ObservableCollection<Expertize>();

        //о принятии мер по устранению обстоятельств, способствовавших преступлению
        public ObservableCollection<ActionInfo> ActionInfos { get; set; } = new ObservableCollection<ActionInfo>();

        //требования прокурора (количество всего)
        private int? demandsTotal;
        public int? DemandsTotal
        {
            get { return demandsTotal; }
            set
            {
                demandsTotal = value;
                OnPropertyChanged("DemandsTotal");
                StatCardFileTree.AddChangedField(new ChangedField("demandsTotal", $"{value}"));
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
                StatCardFileTree.AddChangedField(new ChangedField("demandsAccepted", $"{value}"));
            }
        }

        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F11 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F11Window w = new F11Window((CardF1DotOne)F11, FilePath);
            w.Show();
        }
        #endregion
    }
}
