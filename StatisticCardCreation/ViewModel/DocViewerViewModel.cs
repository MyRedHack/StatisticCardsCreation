using StatCardApp.Function;
using StatCardApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Controls;

namespace StatCardApp.ViewModel
{
    public abstract class DocViewerViewModel
    {
        public RegModel RegData { get; set; } = new RegModel();
        public CaseInfoModel CaseInfo { get; set; } = new CaseInfoModel();
        public string FullCaseNumber { get; set; }

        public string Today { get; set; }

        public string MinistryCode { get; set; }
        public string DirectionCode { get; set; }
        public string ManagementCode { get; set; }
        public string DepartmentCode { get; set; }

        public string caseNumStr;

        public CommandList Commands { get; set; } = new CommandList();

        public string caseYear;

        public DocViewerViewModel(RegModel regData, string CasePath = "")
        {
            RegData = regData;
            if (CasePath != "")
            {
                CaseInfo = (CaseInfoModel)StatCardFunc.FromJson<CaseInfoModel>(CasePath + @"\caseInfo.conf");
                init();
            }
        }

        public void init()
        {
            caseNumStr = StatCardFunc.ConvertSizeValue(CaseInfo.CaseNumber, 6);

            caseYear = CaseInfo.RegDate.Value.Year.ToString().Substring(2, 2);

            MinistryCode = StatCardFunc.ConvertSizeValue(RegData.MinistryCode, 2);
            DirectionCode = StatCardFunc.ConvertSizeValue(RegData.DirectionCode, 2);
            ManagementCode = StatCardFunc.ConvertSizeValue(RegData.ManagementCode, 2);
            DepartmentCode = StatCardFunc.ConvertSizeValue(RegData.DepartmentCode, 2);

            FullCaseNumber = CaseInfo.CaseType + "." + caseYear + "." + MinistryCode + DirectionCode + "." +
                                ManagementCode + DepartmentCode + "." + caseNumStr;
            Today = DateTime.Today.ToShortDateString().Remove(6, 2);
        }
    }

    public class DocViewerF1ViewModel : DocViewerViewModel
    {
        public CardF1 F1 { get; set; } = new CardF1();

        public DocViewerF1ViewModel(CardF1 F1, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F1 = F1;
        }
    }

    public class DocViewerF11ViewModel : DocViewerViewModel
    {
        public CardF1DotOne F11 { get; set; }

        public DocViewerF11ViewModel(CardF1DotOne F11, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F11 = F11;
        }
    }

    public class DocViewerF12ViewModel : DocViewerViewModel
    {
        public CardF1DotTwo F12 { get; set; }

        public DocViewerF12ViewModel(CardF1DotTwo F12, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F12 = F12;
        }
    }

    public class DocViewerF5ViewModel : DocViewerViewModel
    {
        public CardF5 F5 { get; set; }

        public DocViewerF5ViewModel(CardF5 F5, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F5 = F5;
        }
    }

    public class DocViewerF13ViewModel : DocViewerViewModel
    {
        public CardF1DotThree F13 { get; set; }

        public DocViewerF13ViewModel(CardF1DotThree F13, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F13 = F13;
        }
    }

    public class DocViewerF2ViewModel : DocViewerViewModel
    {
        public CardF2 F2 { get; set; }

        public DocViewerF2ViewModel(CardF2 F2, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F2 = F2;
        }
    }

    public class DocViewerF3ViewModel : DocViewerViewModel
    {
        public CardF3 F3 { get; set; }

        public DocViewerF3ViewModel(CardF3 F3, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F3 = F3;
        }
    }

    public class DocViewerF55ViewModel : DocViewerViewModel
    {
        public CardF5DotFive F55 { get; set; }

        public DocViewerF55ViewModel(CardF5DotFive F55, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F55 = F55;
        }
    }

    public class DocViewerF6ViewModel : DocViewerViewModel
    {
        public CardF6 F6 { get; set; } = new CardF6();
        public F6Qualification[] Qualifications { get; set; } = new F6Qualification[6];
        public char[] OrganCodeChar { get; set; } = new char[4];
        public char[] CaseNumberChar { get; set; } = new char[3];
        public char[] BirthDayChar { get; set; } = new char[8];
        public char[] SendingToCourtChar { get; set; } = new char[8];

        private Dictionary<int, int> complicityFormConvertationList = new Dictionary<int, int>()
        {
            {0, 10},
            {2, 20 },
            {1, 30 },
            {8, 40 }
        };

        private Dictionary<int, int> complicityCategoryConvertationList = new Dictionary<int, int>()
        {
            {4, 1 },
            {1, 2 },
            {2, 3 },
            {3, 4 }
        };

        public DocViewerF6ViewModel(CardF6 F6, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F6 = F6;
            InitializeChars();
        }

        private void InitializeChars()
        {
            var F6CrimeArray = F6.Qualifications.ToArray();
            for (int i = 0; i < F6CrimeArray.Count(); i++)
            {
                F6Qualification item = new F6Qualification();

                item.CrimeStage = StatCardFunc.ConvertSizeValue(F6CrimeArray[i].CrimeStage, 2).ToArray();
                try
                {
                    item.Complicity = StatCardFunc.ConvertSizeValue(complicityFormConvertationList[F6CrimeArray[i].ComplicityForm.Value] + complicityCategoryConvertationList[F6CrimeArray[i].Complicity.Value], 2).ToArray();
                }
                catch(Exception)
                {
                    item.Complicity = " ".ToArray();
                }

                if(F6CrimeArray[i].Qualification.Article.HasValue)
                {
                    item.Article = StatCardFunc.ConvertSizeValue(F6CrimeArray[i].Qualification.Article.Value, 3).ToArray();
                }
                else
                {
                    item.Article = "   ".ToArray();
                }

                if (F6CrimeArray[i].Qualification.Sign != null)
                {
                    item.Sign = StatCardFunc.ConvertSizeValue(Convert.ToInt32(F6CrimeArray[i].Qualification.Sign), 2).ToArray();
                }
                else
                {
                    item.Sign = "  ".ToArray();
                }

                if(F6CrimeArray[i].Qualification.Part != null)
                {
                    item.Part = F6CrimeArray[i].Qualification.Part[0];
                }
                else
                {
                    item.Part = ' ';
                }

                if(F6CrimeArray[i].Qualification.Paragraph != null)
                {
                    item.Paragraph = F6CrimeArray[i].Qualification.Paragraph.ToArray();
                }
                else
                {
                    item.Paragraph = " ".ToArray();
                }

                Qualifications[i] = item;
            }

            OrganCodeChar = StatCardFunc.ConvertSizeValue(RegData.DepartmentCode, 4).ToArray();
            CaseNumberChar = StatCardFunc.ConvertSizeValue(CaseInfo.CaseNumber, 3).ToArray();
            if (F6.Birthday.HasValue)
            {
                string TempYear = StatCardFunc.ConvertSizeValue(F6.Birthday.Value.Year, 4);
                string TempMonth = StatCardFunc.ConvertSizeValue(F6.Birthday.Value.Month, 2);
                string TempDay = StatCardFunc.ConvertSizeValue(F6.Birthday.Value.Day, 2);

                BirthDayChar = (TempYear + TempMonth + TempDay).ToArray();
            }

            if (F6.SendingToCourt.HasValue)
            {
                string TempYear = StatCardFunc.ConvertSizeValue(F6.SendingToCourt.Value.Year, 4);
                string TempMonth = StatCardFunc.ConvertSizeValue(F6.SendingToCourt.Value.Month, 2);
                string TempDay = StatCardFunc.ConvertSizeValue(F6.SendingToCourt.Value.Day, 2);

                SendingToCourtChar = (TempYear + TempMonth + TempDay).ToArray();
            }
        }
    }

    public class DocViewerF31ViewModel : DocViewerViewModel
    {
        public CardF3DotOne F31 { get; set; }

        public DocViewerF31ViewModel(CardF3DotOne F31, RegModel regData, string CasePath = "") : base(regData, CasePath)
        {
            this.F31 = F31;

        }
    }

    public class F6Qualification
    {
        public char[] CrimeStage { get; set; } = new char[2];
        public char[] Complicity { get; set; } = new char[2];
        public char[] Article { get; set; } = new char[3];
        public char[] Sign { get; set; } = new char[2];
        public char Part { get; set; }
        public char[] Paragraph { get; set; } = new char[5];
    }
}