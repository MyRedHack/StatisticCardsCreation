using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class OE_ThingWithdrawed : Base
    {
        #region Предмет
        private int? thing;
        public int? Thing
        {
            get { return thing; }
            set
            {
                thing = value;
                OnPropertyChanged("Thing");
            }
        }
        #endregion

        #region Количество
        private long? wCount;
        public long? WCount
        {
            get { return wCount; }
            set
            {
                wCount = value;
                OnPropertyChanged("WCount");
            }
        }
        #endregion
    }
}
