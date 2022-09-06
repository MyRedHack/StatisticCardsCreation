using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class DataForF6Card
    {
        public FIO FIO { get; set; } = new FIO();
        public Place BirthPlace { get; set; } = new Place();
        public DateTime? Birthday { get; set; }
    }
}
