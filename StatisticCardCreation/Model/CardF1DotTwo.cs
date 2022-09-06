using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using System.Windows;
using StatCardApp.Global;

namespace StatCardApp.Model
{
    public class CardF1DotTwo : StatisticCard
    {
        //место службы лица
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

        //порядковый номер подозреваемого
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
        private FIO fio = new FIO(table: "Suspects");
        public FIO FIO
        {
            get => fio;
            set
            {
                fio = value;
                OnPropertyChanged("FIO");
            }
        }

        //Должность
        private string position;
        public string Position
        {
            get => position;
            set
            {
                position = value;
                OnPropertyChanged("Position");
                StatCardFileTree.AddChangedField(new ChangedField("position", $"'{value}'", "Suspects"));
            }
        }

        //Звание
        private string rank;
        public string Rank
        {
            get => rank;
            set
            {
                rank = value;
                OnPropertyChanged("Rank");
                StatCardFileTree.AddChangedField(new ChangedField("rank", $"'{value}'", "Suspects"));
            }
        }

        //Местный житель
        private Boolean local;
        public Boolean Local
        {
            get => local;
            set
            {
                local = value;
                OnPropertyChanged("Local");
                StatCardFileTree.AddChangedField(new ChangedField("local", $"{Convert.ToInt32(value)}", "Suspects"));
            }
        }

        //Личный номер военнослужащего
        private string individNumber;
        public string IndividNumber
        {
            get => individNumber;
            set
            {
                individNumber = value;
                OnPropertyChanged("IndividNumber");
                StatCardFileTree.AddChangedField(new ChangedField("individNumber", $"'{value}'", "Suspects"));
            }
        }

        //Пол
        private int? sex;
        public int? Sex
        {
            get => sex;
            set
            {
                sex = value;
                OnPropertyChanged("Sex");
                StatCardFileTree.AddChangedField(new ChangedField("sex", $"{value}", "Suspects"));
            }
        }

        //Дата рождения
        private DateTime? birthday;
        public DateTime? Birthday
        {
            get => birthday;
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
                StatCardFileTree.AddChangedField(new ChangedField("birthday", $"'{value}'", "Suspects"));
            }
        }

        //Возраст на момент совершения преступления
        private int? age;
        public int? Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged("Age");
                StatCardFileTree.AddChangedField(new ChangedField("age", $"{value}", "Suspects"));
            }
        }

        //Место рождения
        private Place birthPlace = new Place("birth", "Suspects");
        public Place BirthPlace
        {
            get => birthPlace;
            set
            {
                birthPlace = value;
                OnPropertyChanged("BirthPlace");
            }
        }

        //Место жительства
        private Place livePlace = new Place("live", "Suspects");
        public Place LivePlace
        {
            get => livePlace;
            set
            {
                livePlace = value;
                OnPropertyChanged("LivePlace");
            }
        }

        //Воспитание
        private int? upbringing;
        public int? Upbringing
        {
            get => upbringing;
            set
            {
                upbringing = value;
                OnPropertyChanged("Upbringing");
                StatCardFileTree.AddChangedField(new ChangedField("upbringing", $"{value}", "Suspects"));
            }
        }

        //Социальный статус
        private int? socialStatus;
        public int? SocialStatus
        {
            get => socialStatus;
            set
            {
                socialStatus = value;
                OnPropertyChanged("SocialStatus");
                StatCardFileTree.AddChangedField(new ChangedField("socialStatus", $"{value}", "Suspects"));
            }
        }

        //Количество находящихся на иждивении детей
        private int? children;
        public int? Children
        {
            get => children;
            set
            {
                children = value;
                OnPropertyChanged("Children");
                StatCardFileTree.AddChangedField(new ChangedField("children", $"{value}", "Suspects"));
            }
        }

        //Место призыва/учебное заведение
        private string _RVKinstitute;
        public string RVKinstitute
        {
            get => _RVKinstitute;
            set
            {
                _RVKinstitute = value;
                OnPropertyChanged("RVKinstitute");
                StatCardFileTree.AddChangedField(new ChangedField("RVKinstitute", $"'{value}'", "Suspects"));
            }
        }

        //республика, край, область, Москва,  С.-Петербург
        private string instancePlace;
        public string InstancePlace
        {
            get => instancePlace;
            set
            {
                instancePlace = value;
                OnPropertyChanged("InstancePlace");
                StatCardFileTree.AddChangedField(new ChangedField("instancePlace", $"'{value}'", "Suspects"));
            }
        }

        //Дата призыва/окончания учебного заведения
        private DateTime? callDate;
        public DateTime? CallDate
        {
            get => callDate;
            set
            {
                callDate = value;
                OnPropertyChanged("CallDate");
                StatCardFileTree.AddChangedField(new ChangedField("callDate", $"'{value}'", "Suspects"));
            }
        }

        //Образование
        private int? education;
        public int? Education
        {
            get => education;
            set
            {
                education = value;
                OnPropertyChanged("Education");
                StatCardFileTree.AddChangedField(new ChangedField("education", $"{value}", "Suspects"));
            }
        }

        //Гражданство
        private int? citizenship;
        public int? Citizenship
        {
            get => citizenship;
            set
            {
                citizenship = value;
                OnPropertyChanged("Citizenship");
                StatCardFileTree.AddChangedField(new ChangedField("citizenship", $"{value}", "Suspects"));
            }
        }

        //Страна по гражданству
        private string country;
        public string Country
        {
            get => country;
            set
            {
                country = value;
                OnPropertyChanged("Country");
                StatCardFileTree.AddChangedField(new ChangedField("country", $"'{value}'", "Suspects"));
            }
        }

        //Национальность
        private string nationality;
        public string Nationality
        {
            get => nationality;
            set
            {
                nationality = value;
                OnPropertyChanged("Nationality");
                StatCardFileTree.AddChangedField(new ChangedField("nationality", $"'{value}'", "Suspects"));
            }
        }

        //Семейное положение
        private int? familyStatus;
        public int? FamilyStatus
        {
            get => familyStatus;
            set
            {
                familyStatus = value;
                OnPropertyChanged("FamilyStatus");
                StatCardFileTree.AddChangedField(new ChangedField("familyStatus", $"{value}", "Suspects"));
            }
        }

        //Должностной статус
        public ObservableCollection<El> OfficialStatus { get; set; } = new ObservableCollection<El>();

        //Дата возбуждения уголовного дела
        private DateTime? criminalProceedingDate;
        public DateTime? CriminalProceedingDate
        {
            get => criminalProceedingDate;
            set
            {
                criminalProceedingDate = value;
                OnPropertyChanged("CriminalProceedingDate");
            }
        }

        //Дата задержания
        private DateTime? arrestDate;
        public DateTime? ArrestDate
        {
            get => arrestDate;
            set
            {
                arrestDate = value;
                OnPropertyChanged("ArrestDate");
            }
        }

        //Дата первичного обвинения
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

        //Дата объявления в розыск
        private DateTime? wantedDate;
        public DateTime? WantedDate
        {
            get => wantedDate;
            set
            {
                wantedDate = value;
                OnPropertyChanged("WantedDate");
            }
        }

        //Дата применения меры процессуального принуждения
        private DateTime? coersiveMeasureDate;
        public DateTime? CoersiveMeasureDate
        {
            get => coersiveMeasureDate;
            set
            {
                coersiveMeasureDate = value;
                OnPropertyChanged("CoersiveMeasureDate");
                switch(CoersiveMeasure)
                {
                    case 14: 
                        CriminalProceedingDate = CoersiveMeasureDate;
                        ArrestDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        break;
                    case 11:
                        ArrestDate = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        break;
                    case 12:
                        AccusationDate = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        break;
                    case 31:
                        ArrestTimeStart = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        AccusationDate = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        break;
                    case 13:
                        WantedDate = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        CoersiveMeasureDateGeneral = null;
                        break;
                    default:
                        CoersiveMeasureDateGeneral = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        break;
                }
                StatCardFileTree.AddChangedField(new ChangedField("coerciveMeasureDate", $"'{value}'", "PreventiveMeasures"));
            }
        }

        private DateTime? coersiveMeasureDateGeneral;
        public DateTime? CoersiveMeasureDateGeneral
        {
            get => coersiveMeasureDateGeneral;
            set
            {
                coersiveMeasureDateGeneral = value;
                OnPropertyChanged("CoersiveMeasureDateGeneral");
            }
        }
        //Вид применения меры процессуального принуждения
        private int? coersiveMeasure;
        public int? CoersiveMeasure
        {
            get => coersiveMeasure;
            set
            {
                
                coersiveMeasure = value;
                OnPropertyChanged("CoersiveMeasure");
                switch (value)
                {
                    case 14:
                        CriminalProceedingDate = CoersiveMeasureDate;
                        ArrestDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        CoersiveMeasureGeneral = null;
                        break;
                    case 11:
                        ArrestDate = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        CoersiveMeasureGeneral = null;
                        break;
                    case 12:
                        AccusationDate = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        CoersiveMeasureGeneral = null;
                        break;
                    case 31:
                        ArrestTimeStart = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        AccusationDate = null;
                        WantedDate = null;
                        CoersiveMeasureDateGeneral = null;
                        CoersiveMeasureGeneral = null;
                        break;
                    case 13:
                        WantedDate = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        CoersiveMeasureDateGeneral = null;
                        CoersiveMeasureGeneral = null;
                        break;
                    default:
                        CoersiveMeasureDateGeneral = CoersiveMeasureDate;
                        CriminalProceedingDate = null;
                        ArrestDate = null;
                        AccusationDate = null;
                        ArrestTimeStart = null;
                        WantedDate = null;
                        CoersiveMeasureGeneral = value;
                        break;
                }

                if (value != 31)
                {
                    ArrestTimeEnd = null;
                    ArrestDuration = null;
                }

                StatCardFileTree.AddChangedField(new ChangedField("coerciveMeasure", $"{value}", "PreventiveMeasures"));
            }
        }

        private int? coersiveMeasureGeneral;
        public int? CoersiveMeasureGeneral
        {
            get => coersiveMeasureGeneral;
            set
            {
                coersiveMeasureGeneral = value;
                OnPropertyChanged("CoersiveMeasureGeneral");
            }
        }

        //Дата начала продления ареста
        private DateTime? arrestTimeStart;
        public DateTime? ArrestTimeStart
        {
            get => arrestTimeStart;
            set
            {
                arrestTimeStart = value;
                OnPropertyChanged("ArrestTimeStart");
            }
        }

        //Дата окончания продления ареста
        private DateTime? arrestTimeEnd;
        public DateTime? ArrestTimeEnd
        {
            get => arrestTimeEnd;
            set
            {
                arrestTimeEnd = value;
                OnPropertyChanged("ArrestTimeEnd");
                StatCardFileTree.AddChangedField(new ChangedField("arrestExtensionEnd", $"'{value}'", "PreventiveMeasures"));
            }
        }

        //Длительность ареста
        private int? arrestDuration;
        public int? ArrestDuration
        {
            get
            {
                return arrestDuration;
            }
            set
            {
                arrestDuration = value;
                OnPropertyChanged("ArrestDuration");
                StatCardFileTree.AddChangedField(new ChangedField("arrestExtension", $"{value}", "PreventiveMeasures"));
            }
        }
        public ObservableCollection<Crime> Crimes { get; set; } = new ObservableCollection<Crime>();

        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F12 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F12Window w = new F12Window((CardF1DotTwo)F12, FilePath);
            w.Show();
        }
        #endregion

        public static CardF1DotTwo operator +(CardF1DotTwo F12Main, CardF1DotTwo F12Changed)
        {
            CardF1DotTwo F12Result = (CardF1DotTwo)F12Main.Clone();

            F12Result.militaryUnit = !string.IsNullOrWhiteSpace(F12Changed.MilitaryUnit) ? F12Changed.MilitaryUnit : F12Main.MilitaryUnit;
            F12Result.suspectNumber = F12Changed.suspectNumber.HasValue ? F12Changed.suspectNumber : F12Main.suspectNumber;
            F12Result.FIO.Name = !string.IsNullOrWhiteSpace(F12Changed.FIO.Name) ? F12Changed.FIO.Name : F12Main.FIO.Name;
            F12Result.FIO.Surname = !string.IsNullOrWhiteSpace(F12Changed.FIO.Surname) ? F12Changed.FIO.Surname : F12Main.FIO.Surname;
            F12Result.FIO.Patronymic = !string.IsNullOrWhiteSpace(F12Changed.FIO.Patronymic) ? F12Changed.FIO.Patronymic : F12Main.FIO.Patronymic;
            F12Result.position = !string.IsNullOrWhiteSpace(F12Changed.Position) ? F12Changed.Position : F12Main.Position;
            F12Result.rank = !string.IsNullOrWhiteSpace(F12Changed.rank) ? F12Changed.rank : F12Main.rank;
            F12Result.local = F12Changed.local ? F12Changed.local : F12Main.local;
            F12Result.individNumber = !string.IsNullOrWhiteSpace(F12Changed.individNumber) ? F12Changed.individNumber : F12Main.individNumber;
            F12Result.sex = F12Changed.sex.HasValue ? F12Changed.sex : F12Main.sex;
            F12Result.birthday = F12Changed.birthday.HasValue ? F12Changed.birthday : F12Main.birthday;
            F12Result.age = F12Changed.age.HasValue ? F12Changed.age : F12Main.age;
            F12Result.BirthPlace.Address = !string.IsNullOrWhiteSpace(F12Changed.BirthPlace.Address) ? F12Changed.BirthPlace.Address : F12Main.BirthPlace.Address;
            F12Result.BirthPlace.Area = !string.IsNullOrWhiteSpace(F12Changed.BirthPlace.Area) ? F12Changed.BirthPlace.Area : F12Main.BirthPlace.Area;
            F12Result.BirthPlace.Region = !string.IsNullOrWhiteSpace(F12Changed.BirthPlace.Region) ? F12Changed.BirthPlace.Region : F12Main.BirthPlace.Region;
            F12Result.BirthPlace.Locality = !string.IsNullOrWhiteSpace(F12Changed.BirthPlace.Locality) ? F12Changed.BirthPlace.Locality : F12Main.BirthPlace.Locality;
            F12Result.LivePlace.Address = !string.IsNullOrWhiteSpace(F12Changed.LivePlace.Address) ? F12Changed.LivePlace.Address : F12Main.LivePlace.Address;
            F12Result.LivePlace.Area = !string.IsNullOrWhiteSpace(F12Changed.LivePlace.Area) ? F12Changed.LivePlace.Area : F12Main.LivePlace.Area;
            F12Result.LivePlace.Region = !string.IsNullOrWhiteSpace(F12Changed.LivePlace.Region) ? F12Changed.LivePlace.Region : F12Main.LivePlace.Region;
            F12Result.LivePlace.Locality = !string.IsNullOrWhiteSpace(F12Changed.LivePlace.Locality) ? F12Changed.LivePlace.Locality : F12Main.LivePlace.Locality;
            F12Result.upbringing = F12Changed.upbringing.HasValue ? F12Changed.upbringing : F12Main.upbringing;
            F12Result.socialStatus = F12Changed.socialStatus.HasValue ? F12Changed.socialStatus : F12Main.socialStatus;
            F12Result.children = F12Changed.children.HasValue ? F12Changed.children : F12Main.children;
            F12Result._RVKinstitute = !string.IsNullOrWhiteSpace(F12Changed.RVKinstitute) ? F12Changed.RVKinstitute : F12Main.RVKinstitute;
            F12Result.instancePlace = !string.IsNullOrWhiteSpace(F12Changed.instancePlace) ? F12Changed.instancePlace : F12Main.instancePlace;
            F12Result.callDate = F12Changed.callDate.HasValue ? F12Changed.callDate : F12Main.callDate;
            F12Result.education = F12Changed.education.HasValue ? F12Changed.education : F12Main.education;
            F12Result.citizenship = F12Changed.citizenship.HasValue ? F12Changed.citizenship : F12Main.citizenship;
            F12Result.nationality = !string.IsNullOrWhiteSpace(F12Changed.nationality) ? F12Changed.nationality : F12Main.nationality;
            F12Result.familyStatus = F12Changed.familyStatus.HasValue ? F12Changed.familyStatus : F12Main.familyStatus;
            F12Result.criminalProceedingDate = F12Changed.criminalProceedingDate.HasValue ? F12Changed.criminalProceedingDate : F12Main.criminalProceedingDate;
            F12Result.arrestDate = F12Changed.arrestDate.HasValue ? F12Changed.arrestDate : F12Main.arrestDate;
            F12Result.accusationDate = F12Changed.accusationDate.HasValue ? F12Changed.accusationDate : F12Main.accusationDate;
            F12Result.wantedDate = F12Changed.wantedDate.HasValue ? F12Changed.wantedDate : F12Main.wantedDate;
            F12Result.coersiveMeasure = F12Changed.coersiveMeasure.HasValue ? F12Changed.coersiveMeasure : F12Main.coersiveMeasure;
            F12Result.coersiveMeasureGeneral = F12Changed.coersiveMeasureGeneral.HasValue ? F12Changed.coersiveMeasureGeneral : F12Main.coersiveMeasureGeneral;
            F12Result.coersiveMeasureDate = F12Changed.coersiveMeasureDate.HasValue ? F12Changed.coersiveMeasureDate : F12Main.coersiveMeasureDate;
            F12Result.coersiveMeasureDateGeneral = F12Changed.coersiveMeasureDateGeneral.HasValue ? F12Changed.coersiveMeasureDateGeneral : F12Main.coersiveMeasureDateGeneral;
            F12Result.arrestTimeStart = F12Changed.arrestTimeStart.HasValue ? F12Changed.arrestTimeStart : F12Main.arrestTimeStart;
            F12Result.arrestTimeEnd = F12Changed.arrestTimeEnd.HasValue ? F12Changed.arrestTimeEnd : F12Main.arrestTimeEnd;
            F12Result.arrestDuration = F12Changed.arrestDuration.HasValue ? F12Changed.arrestDuration : F12Main.arrestDuration;
            if (F12Changed.Crimes.Count > 0)
            {
                F12Result.Crimes = new ObservableCollection<Crime>();
                foreach (Crime item in F12Changed.Crimes) F12Result.Crimes.Add(item);
            }
            else
            {
                F12Result.Crimes = new ObservableCollection<Crime>();
                foreach (Crime item in F12Main.Crimes) F12Result.Crimes.Add(item);
            }

            if (F12Changed.OfficialStatus.Count > 0)
            {
                F12Result.OfficialStatus = new ObservableCollection<El>();
                foreach (El item in F12Changed.OfficialStatus) F12Result.OfficialStatus.Add(item);
            }
            else
            {
                F12Result.OfficialStatus = new ObservableCollection<El>();
                foreach (El item in F12Main.OfficialStatus) F12Result.OfficialStatus.Add(item);
            }
            return F12Result;
        }
    }
}
