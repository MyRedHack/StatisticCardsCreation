using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticProceeding
{
    public class CriminalProductionInfo
    {
        public string Date { get; set; }
        public string Position { get; set; }
        public string Rank { get; set; }
        public string FIO { get; set; }

        public string StringView
        {
            get
            {
                return $"{Date}\n {Position} ВСО по ВГ\n {Rank}\n {FIO}\n\n";
            }
        }
    }
}
