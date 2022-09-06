using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StatCardApp.BaseClass
{
    public class CaseHierarchy : Base
    {
        public List<int> Years { get; set; } = new List<int>();

        public Dictionary<int, List<long>> Cases { get; set; } = new Dictionary<int, List<long>>();

        private int currentYear;
        public int CurrentYear 
        {
            get => currentYear;
            set
            {
                
                if (!Years.Contains(value))
                {
                    
                    CurrentCaseList = new List<long>();
                    return;
                }
                currentYear = value;
                OnPropertyChanged("CurrentYear");
                CurrentCaseList = Cases[currentYear];
            }
        }


        private List<long> currentCaseList = new List<long>();
        public List<long> CurrentCaseList
        {
            get => currentCaseList;
            set
            {
                currentCaseList = value;
                OnPropertyChanged("currentCaseList");
            }
        }
    }
}
