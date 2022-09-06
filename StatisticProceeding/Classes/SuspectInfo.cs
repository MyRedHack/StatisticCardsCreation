using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticProceeding
{
    public class SuspectInfo
    {
        public string ServicePlace { get; set; }
        public string Rank { get; set; }
        public string FIO { get; set; }

        public string StringView
        {
            get
            {
                return $"{ServicePlace}\n {Rank}\n {FIO}\n\n";
            }
        }
    }
}
