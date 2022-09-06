using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticProceeding
{
    public class CaseDescription : Base
    {
        public int pNum { get; set; }
        public string CaseNumber { get; set; }
        public string CriminalProceedings { get; set; }
        public string Suspects { get; set; }

        private string crimeInfo;
        public string CrimeInfo
        {
            get => crimeInfo;
            set
            {
                crimeInfo = value;
                OnPropertyChanged("CrimeInfo");
            }
        }
        public string CriminalProductions { get; set; }
        public string InvestigationTerm { get; set; }
        public string GuardTerm { get; set; }

        private string investigationActions;
        public string InvestigationActions
        {
            get => investigationActions;
            set
            {
                investigationActions = value;
                OnPropertyChanged("InvestigationActions");
            }
        }
    }
}
