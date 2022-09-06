using System;
using System.Collections.ObjectModel;
using System.IO;
using Newtonsoft.Json;
using StatCardApp.Model.CardFields;
using StatCardApp.Function;
using System.Windows;
using StatCardApp.Global;
using System.Collections.Specialized;

namespace StatCardApp.Model
{
    public class CardF1 : StatisticCard
    {
        #region сущность карточки Ф-1

        #region Поля
        //-----объявление полей----



        //вид документа
        private int? documentType;

        //номер документа
        private long? documentNumber;

        //за кем числится преступление
        private string listedCrimeObject;

        //наименования военной прокуратуры, за которой числится преступление
        private string listedProsecutorOffice;

        //порядковый номер преступления в уголовном деле
        private int? crimeNumber;

        //номер регистрации заявления
        private int? stateRegNumber;

        //дата регистрации
        private DateTime? registrationDate;

        //сведения о нарушениях
        private int? violationInfo;

        //кем допущены нарушения
        private int? crimePerson;

        //описание, кем допущено нарушение
        private Person _crimePersDesc = new Person();
        public Person CrimePersDescription
        {
            get { return _crimePersDesc; }
            set
            {
                _crimePersDesc = value;
                OnPropertyChanged("CrimePersDescription");
            }
        }

        //вид требования раскрытия преступления
        private int? detectionRequirement;

        //время и дата совершения преступления
        private DateTime? crimeDate;
        private DateTime? crimeTime;

        //кем обнаружено преступление
        private int? crimeDetectedObject;

        //поводы для возбуждения уголовного дела
        private int? crimePretext;

        //кем возбуждено уголовное дело
        private int? caseStartedObject;

        //форма предварительного расследования
        private int? preInvestigationForm;

        //Дата направления материалов военным прокурорам
        private DateTime? transMaterialsDate;

        //дата возбуждения уголовного дела
        private DateTime? startCaseDate;

        //дата принятия дела к производству следователем
        private DateTime? acceptCaseForProccess;

        //откуда поступило дело
        private int? caseFrom;

        //описание события
        private string eventDescription;

        //соединенное уголовное дело
        private long? connectedCase;
        public long? ConnectedCase
        {
            get => connectedCase;
            set
            {
                connectedCase = value;
                OnPropertyChanged("ConnectedCase");
                StatCardFileTree.AddChangedField(new ChangedField("connectedCase", $"{value}"));
            }
        }
        //квалификация преступления
        private CrimeQualification crimeQualification = new CrimeQualification();

        public CrimeQualification CrimeQualification
        {
            get { return crimeQualification; }
            set
            {
                crimeQualification = value;
                OnPropertyChanged("CrimeQualification");
            }
        }

        //Незаконная выгода и иной вред имущественного характера
        private UnlawfullBenefit unlawfullBenefit = new UnlawfullBenefit();

        public UnlawfullBenefit UnlawfullBenefit
        {
            get { return unlawfullBenefit; }
            set
            {
                unlawfullBenefit = value;
                OnPropertyChanged("UnlawfullBenefit");
            }
        }

        //квалифицирующие признаки преступления
        private string qualCrimeAttribute;

        //мотив преступления
        private int? crimeMotive;

        //сумма взятки
        private long? bribeSum;

        //статья УК, предшествовавшие легализации денежных средств и иного имущества
        private string articlesBeforeLegalization;

        //квалификация связанных преступлений
        private string relatedCrimesQualification;

        //Признак оконченного состава
        private int? finishedCompositionSign;

        //описание группы преступников
        private CrimeGroup crimeGroup = new CrimeGroup();
        public CrimeGroup CrimeGroup
        {
            get { return crimeGroup; }
            set
            {
                crimeGroup = value;
                OnPropertyChanged("CrimeGroup");
            }
        }

        //характер происшествия
        private int? crimeCharacter;

        //Обстоятельства гибели и травмирования
        private int? truamaCondition;

        //объекты посягательства:
        //на имущество


        public ObservableCollection<OE_Own> owns { get; set; } = new ObservableCollection<OE_Own>();

        //на оружие
        public ObservableCollection<OE_Weapon> weapons { get; set; } = new ObservableCollection<OE_Weapon>();

        //изъято предметов
        public ObservableCollection<OE_ThingWithdrawed> things { get; set; } = new ObservableCollection<OE_ThingWithdrawed>();

        //характеристика пострадавшего
        public ObservableCollection<El> VictimCharacter { get; set; } = new ObservableCollection<El>();

        //обстоятельства временного характера
        public ObservableCollection<El> TempCondition { get; set; } = new ObservableCollection<El>();

        //дополнительные сведения
        public ObservableCollection<El> ExtraInfo { get; set; } = new ObservableCollection<El>();

        private Boolean extraInfoHasNationalProgramm;
        public Boolean ExtraInfoHasNationalProgramm
        {
            get 
            {
                return extraInfoHasNationalProgramm;
            }
            set
            {
                extraInfoHasNationalProgramm = value;
                OnPropertyChanged("ExtraInfoHasNationalProgramm");
            }
        }

        //нац программы
        private string nationalProgramm;

        //место совершения преступления
        private int? crimePlace;

        //Субъект РФ или государство преступления
        private string crimeState;

        //дислокация воинской части
        private int? militaryUntDislocation;

        //общественное место
        private int? publicPlace;

        //орудия и средства преступления
        private int? crimeMeans;

        //осмотр места происшествия провёл
        private int? sceneInspectionPerson;

        //осмотр места происшествия произведен с участием
        public ObservableCollection<El> SceneInspectionInvolvedPerson { get; set; } = new ObservableCollection<El>();


        #endregion

        #region Свойства
        //-----объявление свойств----


        //вид документ
        public int? DocumentType
        {
            get { return documentType; }
            set
            {
                documentType = value;
                OnPropertyChanged("DocumentType");
            }
        }

        //номер документа
        public long? DocumentNumber
        {
            get { return documentNumber; }
            set
            {
                documentNumber = value;
                OnPropertyChanged("DocumentNumber");
            }
        }

        //за кем числится преступление
        public string ListedCrimeObject
        {
            get { return listedCrimeObject; }
            set
            {
                listedCrimeObject = value;
                OnPropertyChanged("ListedCrimeObject");
                StatCardFileTree.AddChangedField(new ChangedField("listedCrimeObject", $"'{value}'"));
            }
        }

        //наименования военной прокуратуры, за которой числится преступление
        public string ListedProsecutorOffice
        {
            get { return listedProsecutorOffice; }
            set
            {
                listedProsecutorOffice = value;
                OnPropertyChanged("ListedProsecutorOffice");
                StatCardFileTree.AddChangedField(new ChangedField("listedProsecutorOffice", $"'{value}'"));
            }
        }

        //порядковый номер преступления в уголовном деле
        public int? CrimeNumber
        {
            get { return crimeNumber; }
            set
            {
                crimeNumber = value;
                OnPropertyChanged("CrimeNumber");
            }
        }

        //номер регистрации заявления
        public int? StateRegNumber
        {
            get { return stateRegNumber; }
            set
            {
                stateRegNumber = value;
                OnPropertyChanged("StateRegNumber");
                StatCardFileTree.AddChangedField(new ChangedField("stateRegNumber", $"{value}"));
            }
        }

        //дата регистрации
        public DateTime? RegistrationDate
        {
            get { return registrationDate; }
            set
            {
                registrationDate = value;
                OnPropertyChanged("RegistrationDate");
                StatCardFileTree.AddChangedField(new ChangedField("registrationDate", $"'{value}'"));
            }
        }



        //сведения о нарушениях
        public int? ViolationInfo
        {
            get { return violationInfo; }
            set
            {
                violationInfo = value;
                OnPropertyChanged("ViolationInfo");
                StatCardFileTree.AddChangedField(new ChangedField("violationInfo", $"{value}"));
            }
        }

        //кем допущены нарушения
        public int? CrimePerson
        {
            get { return crimePerson; }
            set
            {
                crimePerson = value;
                OnPropertyChanged("CrimePerson");
                StatCardFileTree.AddChangedField(new ChangedField("crimePerson", $"{value}"));
            }
        }

        //вид требования раскрытия преступления
        public int? DetectionRequirement
        {
            get { return detectionRequirement; }
            set
            {
                detectionRequirement = value;
                OnPropertyChanged("DetectionRequirement");
                StatCardFileTree.AddChangedField(new ChangedField("detectionRequirement", $"{value}"));
            }
        }

        //время и дата совершения преступления
        public DateTime? CrimeDate
        {
            get { return crimeDate; }
            set
            {
                crimeDate = value;
                OnPropertyChanged("CrimeDate");
                StatCardFileTree.AddChangedField(new ChangedField("crimeDate", $"'{value}'"));
            }
        }

        public DateTime? CrimeTime
        {
            get { return crimeTime; }
            set
            {
                crimeTime = value;
                OnPropertyChanged("CrimeTime");
            }
        }

        //кем обнаружено преступление
        public int? CrimeDetectedObject
        {
            get { return crimeDetectedObject; }
            set
            {
                crimeDetectedObject = value;
                OnPropertyChanged("CrimeDetectedObject");
                StatCardFileTree.AddChangedField(new ChangedField("crimeDetectedObject", $"{value}"));
            }
        }

        //поводы для возбуждения уголовного дела
        public int? CrimePretext
        {
            get { return crimePretext; }
            set
            {
                crimePretext = value;
                OnPropertyChanged("CrimePretext");
                StatCardFileTree.AddChangedField(new ChangedField("crimePretext", $"{value}"));
            }
        }

        //кем возбуждено уголовное дело
        public int? CaseStartedObject
        {
            get { return caseStartedObject; }
            set
            {
                caseStartedObject = value;
                OnPropertyChanged("CaseStartedObject");
                StatCardFileTree.AddChangedField(new ChangedField("caseStartedObject", $"{value}"));
            }
        }

        //форма предварительного расследования
        public int? PreInvestigationForm
        {
            get { return preInvestigationForm; }
            set
            {
                preInvestigationForm = value;
                OnPropertyChanged("PreInvestigationForm");
                StatCardFileTree.AddChangedField(new ChangedField("preInvestigationForm", $"{value}"));
            }
        }

        //Дата направления материалов военным прокурорам
        public DateTime? TransMaterialsDate
        {
            get { return transMaterialsDate; }
            set
            {
                transMaterialsDate = value;
                OnPropertyChanged("TransMaterialsDate");
                StatCardFileTree.AddChangedField(new ChangedField("transMaterialsDate", $"'{value}'"));
            }
        }

        //дата возбуждения уголовного дела
        public DateTime? StartCaseDate
        {
            get { return startCaseDate; }
            set
            {
                startCaseDate = value;
                OnPropertyChanged("StartCaseDate");
                StatCardFileTree.AddChangedField(new ChangedField("startCaseDate", $"'{value}'"));
            }
        }

        //дата принятия дела к производству следователем
        public DateTime? AcceptCaseForProccess
        {
            get { return acceptCaseForProccess; }
            set
            {
                acceptCaseForProccess = value;
                OnPropertyChanged("AcceptCaseForProccess");
                StatCardFileTree.AddChangedField(new ChangedField("acceptCaseForProccess", $"'{value}'"));
            }
        }

        //откуда поступило дело
        public int? CaseFrom
        {
            get { return caseFrom; }
            set
            {
                caseFrom = value;
                OnPropertyChanged("CaseFrom");
                StatCardFileTree.AddChangedField(new ChangedField("caseFrom", $"{value}"));
            }
        }

        //описание события
        public string EventDescription
        {
            get { return eventDescription; }
            set
            {
                eventDescription = value;
                OnPropertyChanged("EventDescription");
                StatCardFileTree.AddChangedField(new ChangedField("eventDescription", $"'{value}'"));
            }
        }


        //квалифицирующие признаки преступления
        public string QualCrimeAttribute
        {
            get { return qualCrimeAttribute; }
            set
            {
                qualCrimeAttribute = value;
                OnPropertyChanged("QualCrimeAttribute");
                StatCardFileTree.AddChangedField(new ChangedField("qualCrimeAttribute", $"'{value}'"));
            }
        }

        //мотив преступления
        public int? CrimeMotive
        {
            get { return crimeMotive; }
            set
            {
                crimeMotive = value;
                OnPropertyChanged("CrimeMotive");
                StatCardFileTree.AddChangedField(new ChangedField("crimeMotive", $"{value}"));
            }
        }

        //сумма взятки
        public long? BribeSum
        {
            get { return bribeSum; }
            set
            {
                bribeSum = value;
                OnPropertyChanged("BribeSum");
                StatCardFileTree.AddChangedField(new ChangedField("bribeSum", $"{value}"));
            }
        }

        //статья УК, предшествовавшие легализации денежных средств и иного имущества
        public string ArticlesBeforeLegalization
        {
            get { return articlesBeforeLegalization; }
            set
            {
                articlesBeforeLegalization = value;
                OnPropertyChanged("ArticlesBeforeLegalization");
                StatCardFileTree.AddChangedField(new ChangedField("articlesBeforeLegalization", $"'{value}'"));
            }
        }

        //квалификация связанных преступлений
        public string RelatedCrimesQualification
        {
            get => relatedCrimesQualification;
            set
            {
                relatedCrimesQualification = value;
                OnPropertyChanged("RelatedCrimesQualification");
                StatCardFileTree.AddChangedField(new ChangedField("relatedCrimesQualification", $"'{value}'"));
            }
        }
        //Признак оконченного состава
        public int? FinishedCompositionSign
        {
            get { return finishedCompositionSign; }
            set
            {
                finishedCompositionSign = value;
                OnPropertyChanged("FinishedCompositionSign");
                StatCardFileTree.AddChangedField(new ChangedField("finishedCompositionSign", $"{value}"));
            }
        }


        //характер происшествия
        public int? CrimeCharacter
        {
            get { return crimeCharacter; }
            set
            {
                crimeCharacter = value;
                OnPropertyChanged("CrimeCharacter");
                StatCardFileTree.AddChangedField(new ChangedField("crimeCharacter", $"{value}"));
            }
        }

        //Обстоятельства гибели и травмирования
        public int? TruamaCondition
        {
            get { return truamaCondition; }
            set
            {
                truamaCondition = value;
                OnPropertyChanged("TruamaCondition");
                StatCardFileTree.AddChangedField(new ChangedField("truamaCondition", $"{value}"));
            }
        }

        //место совершения преступления
        public int? CrimePlace
        {
            get { return crimePlace; }
            set
            {
                crimePlace = value;
                OnPropertyChanged("CrimePlace");
                StatCardFileTree.AddChangedField(new ChangedField("crimePlace", $"{value}"));
            }
        }

        //Субъект РФ или государство преступления
        public string CrimeState
        {
            get { return crimeState; }
            set
            {
                crimeState = value;
                OnPropertyChanged("CrimeState");
                StatCardFileTree.AddChangedField(new ChangedField("crimeState", $"'{value}'"));
            }
        }

        //дислокация воинской части
        public int? MilitaryUntDislocation
        {
            get { return militaryUntDislocation; }
            set
            {
                militaryUntDislocation = value;
                OnPropertyChanged("MilitaryUntDislocation");
                StatCardFileTree.AddChangedField(new ChangedField("militaryUntDislocation", $"{value}"));
            }
        }

        //общественное место
        public int? PublicPlace
        {
            get { return publicPlace; }
            set
            {
                publicPlace = value;
                OnPropertyChanged("PublicPlace");
                StatCardFileTree.AddChangedField(new ChangedField("publicPlace", $"{value}"));
            }
        }

        //орудия и средства преступления
        public int? CrimeMeans
        {
            get { return crimeMeans; }
            set
            {
                crimeMeans = value;
                OnPropertyChanged("CrimeMeans");
                StatCardFileTree.AddChangedField(new ChangedField("crimeMeans", $"{value}"));
            }
        }

        //осмотр места происшествия провёл
        public int? SceneInspectionPerson
        {
            get { return sceneInspectionPerson; }
            set
            {
                sceneInspectionPerson = value;
                OnPropertyChanged("SceneInspectionPerson");
                StatCardFileTree.AddChangedField(new ChangedField("sceneInspectionPerson", $"{value}"));
            }
        }

        public string NationalProgramm
        {
            get { return nationalProgramm; }
            set
            {
                nationalProgramm = value;
                OnPropertyChanged("NationalProgramm");
                StatCardFileTree.AddChangedField(new ChangedField("nationalProgramm", $"'{value}'"));
            }
        }
        #endregion
        #endregion


        #region Вызов формы карточки
        public override void CallWindow(FileInfo FilePath)
        {
            string textFromFile = StatCardFunc.GetFileText(FilePath.FullName);
            var F1 = JsonConvert.DeserializeObject(textFromFile, this.GetType());
            F1Window w = new F1Window((CardF1)F1, FilePath);
            w.Show();
        }
        #endregion

        public void SetNationalProgramm(object sender, NotifyCollectionChangedEventArgs args)
        {
            ExtraInfoHasNationalProgramm = false;
            foreach (El item in (ObservableCollection<El>)sender)
            {
                if (item.Num == 70) ExtraInfoHasNationalProgramm = true;
            }
        }

        public static CardF1 operator +(CardF1 Main, CardF1 Changed)
        {
            if (!string.IsNullOrWhiteSpace(Changed.listedCrimeObject)) Main.listedCrimeObject = Changed.listedCrimeObject;
            if (!string.IsNullOrWhiteSpace(Changed.listedProsecutorOffice)) Main.listedProsecutorOffice = Changed.listedProsecutorOffice;
            if (Changed.crimeNumber.HasValue) Main.crimeNumber = Changed.crimeNumber;
            if (Changed.stateRegNumber.HasValue) Main.stateRegNumber = Changed.stateRegNumber;
            if (Changed.registrationDate.HasValue) Main.registrationDate = Changed.registrationDate;
            if (Changed.violationInfo.HasValue) Main.violationInfo = Changed.violationInfo;
            if (Changed.crimePerson.HasValue) Main.crimePerson = Changed.crimePerson;
            if (!string.IsNullOrWhiteSpace(Changed.CrimePersDescription.FIO)) Main.CrimePersDescription.FIO = Changed.CrimePersDescription.FIO;
            if (!string.IsNullOrWhiteSpace(Changed.CrimePersDescription.Position)) Main.CrimePersDescription.Position = Changed.CrimePersDescription.Position;
            if (!string.IsNullOrWhiteSpace(Changed.CrimePersDescription.Rank)) Main.CrimePersDescription.Rank = Changed.CrimePersDescription.Rank;
            if (Changed.detectionRequirement.HasValue) Main.detectionRequirement = Changed.detectionRequirement;
            if (Changed.crimeDate.HasValue) Main.crimeDate = Changed.crimeDate;
            if (Changed.crimeTime.HasValue) Main.crimeTime = Changed.crimeTime;
            if (Changed.crimeDetectedObject.HasValue) Main.crimeDetectedObject = Changed.crimeDetectedObject;
            if (Changed.crimePretext.HasValue) Main.crimePretext = Changed.crimePretext;
            if (Changed.caseStartedObject.HasValue) Main.caseStartedObject = Changed.caseStartedObject;
            if (Changed.preInvestigationForm.HasValue) Main.preInvestigationForm = Changed.preInvestigationForm;
            if (Changed.transMaterialsDate.HasValue) Main.transMaterialsDate = Changed.transMaterialsDate;
            if (Changed.startCaseDate.HasValue) Main.startCaseDate = Changed.startCaseDate;
            if (Changed.acceptCaseForProccess.HasValue) Main.acceptCaseForProccess = Changed.acceptCaseForProccess;
            if (Changed.caseFrom.HasValue) Main.caseFrom = Changed.caseFrom;
            if (!string.IsNullOrWhiteSpace(Changed.eventDescription)) Main.eventDescription = Changed.eventDescription;
            if (Changed.crimeQualification.Article.HasValue) Main.crimeQualification.Article = Changed.crimeQualification.Article;
            if (!string.IsNullOrWhiteSpace(Changed.crimeQualification.Sign)) Main.crimeQualification.Sign = Changed.crimeQualification.Sign;
            if (!string.IsNullOrWhiteSpace(Changed.crimeQualification.Paragraph)) Main.crimeQualification.Paragraph = Changed.crimeQualification.Paragraph;
            if (!string.IsNullOrWhiteSpace(Changed.crimeQualification.Part)) Main.crimeQualification.Part = Changed.crimeQualification.Part;
            if (Changed.unlawfullBenefit.ValueCode.HasValue) Main.unlawfullBenefit.ValueCode = Changed.unlawfullBenefit.ValueCode;
            if (Changed.unlawfullBenefit.Sum.HasValue) Main.unlawfullBenefit.Sum = Changed.unlawfullBenefit.Sum;
            if (!string.IsNullOrWhiteSpace(Changed.qualCrimeAttribute)) Main.qualCrimeAttribute = Changed.qualCrimeAttribute;
            if (Changed.crimeMotive.HasValue) Main.crimeMotive = Changed.crimeMotive;
            if (Changed.bribeSum.HasValue) Main.bribeSum = Changed.bribeSum;
            if (!string.IsNullOrWhiteSpace(Changed.articlesBeforeLegalization)) Main.articlesBeforeLegalization = Changed.articlesBeforeLegalization;
            if (!string.IsNullOrWhiteSpace(Changed.relatedCrimesQualification)) Main.relatedCrimesQualification = Changed.relatedCrimesQualification;
            if (Changed.finishedCompositionSign.HasValue) Main.finishedCompositionSign = Changed.finishedCompositionSign;
            if (Changed.crimeGroup.Accomplices.HasValue) Main.crimeGroup.Accomplices = Changed.crimeGroup.Accomplices;
            if (Changed.crimeGroup.Characteristic.HasValue) Main.crimeGroup.Characteristic = Changed.crimeGroup.Characteristic;
            if (Changed.crimeGroup.Links.HasValue) Main.crimeGroup.Links = Changed.crimeGroup.Links;
            if (Changed.crimeCharacter.HasValue) Main.crimeCharacter = Changed.crimeCharacter;
            if (Changed.truamaCondition.HasValue) Main.truamaCondition = Changed.truamaCondition;
            if (Changed.crimePlace.HasValue) Main.crimePlace = Changed.crimePlace;
            if (!string.IsNullOrWhiteSpace(Changed.crimeState)) Main.crimeState = Changed.crimeState;
            if (Changed.militaryUntDislocation.HasValue) Main.militaryUntDislocation = Changed.militaryUntDislocation;
            if (Changed.publicPlace.HasValue) Main.publicPlace = Changed.publicPlace;
            if (Changed.crimeMeans.HasValue) Main.crimeMeans = Changed.crimeMeans;
            if (Changed.sceneInspectionPerson.HasValue) Main.sceneInspectionPerson = Changed.sceneInspectionPerson;
            if (Changed.owns.Count > 0) 
            {
                Main.owns.Clear();
                foreach (OE_Own own in Changed.owns) Main.owns.Add(own);
            }
            if (Changed.weapons.Count > 0)
            {
                Main.weapons.Clear();
                foreach (OE_Weapon weapon in Changed.weapons) Main.weapons.Add(weapon);
            }
            if (Changed.things.Count > 0)
            {
                Main.things.Clear();
                foreach (OE_ThingWithdrawed thing in Changed.things) Main.things.Add(thing);
            }
            if (Changed.VictimCharacter.Count > 0)
            {
                Main.VictimCharacter.Clear();
                foreach (El item in Changed.VictimCharacter) Main.VictimCharacter.Add(item);
            }
            if (Changed.TempCondition.Count > 0)
            {
                Main.TempCondition.Clear();
                foreach (El item in Changed.TempCondition) Main.TempCondition.Add(item);
            }
            if (Changed.ExtraInfo.Count > 0)
            {
                Main.ExtraInfo.Clear();
                foreach (El item in Changed.ExtraInfo) Main.ExtraInfo.Add(item);
            }
            if (Changed.VictimCharacter.Count > 0)
            {
                Main.VictimCharacter.Clear();
                foreach (El item in Changed.VictimCharacter) Main.VictimCharacter.Add(item);
            }
            if (Changed.SceneInspectionInvolvedPerson.Count > 0)
            {
                Main.SceneInspectionInvolvedPerson.Clear();
                foreach (El item in Changed.SceneInspectionInvolvedPerson) Main.SceneInspectionInvolvedPerson.Add(item);
            }

            return Main;
        }


    }
}
