using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticCardServer
{
    public class CriminalProceedingsInfo
    {
        public int Status { get; set; }
        public string Date { get; set; }
        public string Position { get; set; }
        public string Rank { get; set; }
        public string FIO { get; set; }
        public string Qualification { get; set; }

        public string StringView
        {
            get
            {
                string status = string.Empty;
                if (Status == 2) status = "Приостановлено";
                if (Status == 3) status = "Прекращено";
                if (Status == 6) status = "Возобновлено";
                return $"{status}\n{Date}\n{Position} ВСО по ВГ\n{Rank}\n{FIO}\n{Qualification}\n\n";
            }
        }
    }
}
