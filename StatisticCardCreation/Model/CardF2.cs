using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public class CardF2 : StatisticCard
    {
        //УСЛОВНОЕ наименование воинской части (со
        //единения), учреждения, организации
        private string militaryUnit;
        public string MilitaryUnit
        {
            get => militaryUnit;
            set
            {
                militaryUnit = value;
                OnPropertyChanged("MilitaryUnit");
                StatCardFileTree.AddChangedField(new ChangedField("militaryUnit", $"'{value}'", "Suspects"));
            }
        }

        //наименования военной прокуратуры, за которой числится преступление
        private string listedProsecutorOffice;
        public string ListedProsecutorOffice
        {
            get => listedProsecutorOffice;
            set
            {
                listedProsecutorOffice = value;
                OnPropertyChanged("ListedProsecutorOffice");
            }
        }

        //Порядковый номер лица в уголовном деле
        private int? suspectNumber;
        public int? SuspectNumber
        {
            get => suspectNumber;
            set
            {
                suspectNumber = value;
                OnPropertyChanged("SuspectNumber");
            }
        }

        //Ф.И.О.
        private FIO fio = new FIO();
        public FIO FIO
        {
            get => fio;
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        //Лицо, совершившее преступление, ранее привлекалось к уголовной ответственности (сколько раз)
        private int? criminalCount;
        public int? CriminalCount
        {
            get => criminalCount;
            set
            {
                criminalCount = value;
                OnPropertyChanged("CriminalCount");
                StatCardFileTree.AddChangedField(new ChangedField("criminlCount", $"{value}", "Suspects"));

            }
        }

        //Ст. УК РФ (РСФСР), по которым лицо ранее привлекалось к ответственности
        private string pastArticles;
        public string PastArticles
        {
            get => pastArticles;
            set
            {
                pastArticles = value;
                OnPropertyChanged("PastArticles");
                StatCardFileTree.AddChangedField(new ChangedField("pastArticles", $"{value}", "Suspects"));
            }
        }

        //Судимость
        private int? conviction;
        public int? Conviction
        {
            get => conviction;
            set
            {
                conviction = value;
                OnPropertyChanged("Conviction");
                StatCardFileTree.AddChangedField(new ChangedField("conviction", $"{value}", "Suspects"));

            }
        }

        //Рецидив преступления:
        private int? crimeRelapse;
        public int? CrimeRelapse
        {
            get => crimeRelapse;
            set
            {
                crimeRelapse = value;
                OnPropertyChanged("CrimeRelapse");
                StatCardFileTree.AddChangedField(new ChangedField("crimeRelapse", $"{value}", "Suspects"));
            }
        }

        //Ранее совершалось 
        private int? earlyCases;
        public int? EarlyCases
        {
            get => earlyCases;
            set
            {
                earlyCases = value;
                OnPropertyChanged("EarlyCases");
                StatCardFileTree.AddChangedField(new ChangedField("earlyCases", $"{value}", "Suspects"));
            }
        }

        //Обстоятельства лица на момент преступления
        private int? personState;
        public int? PersonState
        {
            get => personState;
            set
            {
                personState = value;
                OnPropertyChanged("PersonState");
                StatCardFileTree.AddChangedField(new ChangedField("personState", $"{value}", "Suspects"));
            }
        }

        //Преступление совершено лицом (при положении)
        private int? personSituation;
        public int? PersonSituation
        {
            get => personSituation;
            set
            {
                personSituation = value;
                OnPropertyChanged("PersonSituation");
                StatCardFileTree.AddChangedField(new ChangedField("personSituation", $"{value}", "Suspects"));
            }
        }

        //Решение, принятое в отношении лица
        private int? onPersonDecision;
        public int? OnPersonDecision
        {
            get => onPersonDecision;
            set
            {
                onPersonDecision = value;
                OnPropertyChanged("OnPersonDecision");
                StatCardFileTree.AddChangedField(new ChangedField("onPersonDecision", $"{value}", "Suspects"));
            }
        }

        //Причины прекращения уголовного дела
        private int? caseStopReason;
        public int? CaseStopReason
        {
            get => caseStopReason;
            set
            {
                caseStopReason = value;
                OnPropertyChanged("CaseStopReason");
                StatCardFileTree.AddChangedField(new ChangedField("caseStopReason", $"{value}", "Suspects"));
            }
        }

        //Список преступлений
        private ObservableCollection<Crime> _crimes = new ObservableCollection<Crime>();
        public ObservableCollection<Crime> Crimes
        {
            get => _crimes;
            set
            {
                _crimes = value;
                OnPropertyChanged("Crimes");
            }
        }

        //Сумма ареста имущества
        private long? ownArrest;
        public long? OwnArrest
        {
            get => ownArrest;
            set
            {
                ownArrest = value;
                OnPropertyChanged("OwnArrest");
                StatCardFileTree.AddChangedField(new ChangedField("ownArrest", $"{value}", "Suspects"));
            }
        }
        //Дата обвинения
        private DateTime? accusationDate;
        public DateTime? AccusationDate
        {
            get => accusationDate;
            set
            {
                accusationDate = value;
                OnPropertyChanged("AccusationDate");
            }
        }

        //Мера процессуального принуждения
        private int? coerciveMeasureCode;
        public int? CoerciveMeasureCode
        {
            get => coerciveMeasureCode;
            set
            {
                coerciveMeasureCode = value;
                OnPropertyChanged("CoerciveMeasureCode");
                StatCardFileTree.AddChangedField(new ChangedField("coerciveMeasure", $"{value}", "PreventiveMeasures"));
            }
        }

        //Дата меры процессуального принуждения
        private DateTime? coerciveMeasureDate;
        public DateTime? CoerciveMeasureDate
        {
            get => coerciveMeasureDate;
            set
            {
                coerciveMeasureDate = value;
                OnPropertyChanged("CoerciveMeasureDate");
                StatCardFileTree.AddChangedField(new ChangedField("coerciveMeasureDate", $"'{value}'", "PreventiveMeasures"));
            }
        }

        public DataForF6Card F6Data { get; set; } = new DataForF6Card();

        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F2 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F2Window w = new F2Window((CardF2)F2, FilePath);
            w.Show();
        }
        #endregion

        public static CardF2 operator +(CardF2 F2Main, CardF2 F2Changed)
        {
            CardF2 F2Result = (CardF2)F2Main.Clone();

            F2Result.militaryUnit = !string.IsNullOrWhiteSpace(F2Changed.militaryUnit) ? F2Changed.militaryUnit : F2Main.militaryUnit;
            F2Result.listedProsecutorOffice = !string.IsNullOrWhiteSpace(F2Changed.listedProsecutorOffice) ? F2Changed.listedProsecutorOffice : F2Main.listedProsecutorOffice;
            F2Result.suspectNumber = F2Changed.suspectNumber.HasValue ? F2Changed.suspectNumber : F2Main.suspectNumber;
            F2Result.fio.Name = !string.IsNullOrWhiteSpace(F2Changed.fio.Name) ? F2Changed.fio.Name : F2Main.fio.Name;
            F2Result.fio.Surname = !string.IsNullOrWhiteSpace(F2Changed.fio.Surname) ? F2Changed.fio.Surname : F2Main.fio.Surname;
            F2Result.fio.Patronymic = !string.IsNullOrWhiteSpace(F2Changed.fio.Patronymic) ? F2Changed.fio.Patronymic : F2Main.fio.Patronymic;
            F2Result.criminalCount = F2Changed.criminalCount.HasValue ? F2Changed.criminalCount : F2Main.criminalCount;
            F2Result.pastArticles = !string.IsNullOrWhiteSpace(F2Changed.pastArticles) ? F2Changed.pastArticles : F2Main.pastArticles;
            F2Result.conviction = F2Changed.conviction.HasValue ? F2Changed.conviction : F2Main.conviction;
            F2Result.crimeRelapse = F2Changed.crimeRelapse.HasValue ? F2Changed.crimeRelapse : F2Main.crimeRelapse;
            F2Result.earlyCases = F2Changed.earlyCases.HasValue ? F2Changed.earlyCases : F2Main.earlyCases;
            F2Result.personState = F2Changed.personState.HasValue ? F2Changed.personState : F2Main.personState;
            F2Result.personSituation = F2Changed.personSituation.HasValue ? F2Changed.personSituation : F2Main.personSituation;
            F2Result.onPersonDecision = F2Changed.onPersonDecision.HasValue ? F2Changed.onPersonDecision : F2Main.onPersonDecision;
            F2Result.caseStopReason = F2Changed.caseStopReason.HasValue ? F2Changed.caseStopReason : F2Main.caseStopReason;
            F2Result.ownArrest = F2Changed.ownArrest.HasValue ? F2Changed.ownArrest : F2Main.ownArrest;
            F2Result.accusationDate = F2Changed.accusationDate.HasValue ? F2Changed.accusationDate : F2Main.accusationDate;
            F2Result.coerciveMeasureCode = F2Changed.coerciveMeasureCode.HasValue ? F2Changed.coerciveMeasureCode : F2Main.coerciveMeasureCode;
            F2Result.coerciveMeasureDate = F2Changed.coerciveMeasureDate.HasValue ? F2Changed.coerciveMeasureDate : F2Main.coerciveMeasureDate;
            F2Result.F6Data.Birthday = F2Changed.F6Data.Birthday.HasValue ? F2Changed.F6Data.Birthday : F2Main.F6Data.Birthday;
            F2Result.F6Data.BirthPlace.Address = !string.IsNullOrWhiteSpace(F2Changed.F6Data.BirthPlace.Address) ? F2Changed.F6Data.BirthPlace.Address : F2Main.F6Data.BirthPlace.Address;
            F2Result.F6Data.BirthPlace.Region = !string.IsNullOrWhiteSpace(F2Changed.F6Data.BirthPlace.Region) ? F2Changed.F6Data.BirthPlace.Region : F2Main.F6Data.BirthPlace.Region;
            F2Result.F6Data.BirthPlace.Area = !string.IsNullOrWhiteSpace(F2Changed.F6Data.BirthPlace.Area) ? F2Changed.F6Data.BirthPlace.Area : F2Main.F6Data.BirthPlace.Area;
            F2Result.F6Data.BirthPlace.Locality = !string.IsNullOrWhiteSpace(F2Changed.F6Data.BirthPlace.Locality) ? F2Changed.F6Data.BirthPlace.Locality : F2Main.F6Data.BirthPlace.Locality;

            return F2Result;

        }
    }
}
