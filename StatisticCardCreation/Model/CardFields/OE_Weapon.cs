using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class OE_Weapon : Base
    {
        #region Оружие
        private int? weapon;
        public int? Weapon
        {
            get { return weapon; }
            set
            {
                weapon = value;
                OnPropertyChanged("Weapon");
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
