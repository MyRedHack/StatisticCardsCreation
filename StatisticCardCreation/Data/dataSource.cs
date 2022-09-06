using WpfApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using WpfApp1.Data;

namespace WpfApp1.Data
{
    class dataSource :Base
    {
        ListsData data = new ListsData();

        private List<El> violationInfo;
        public List<El> ViolationInfo 
        { 
            get { return violationInfo; }
            
            set
            {
                violationInfo = value;
                OnPropertyChanged("ViolationInfo");
            }

        }

    }

   
    
}
