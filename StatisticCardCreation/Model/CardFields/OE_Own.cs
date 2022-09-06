using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    public class OE_Own : Base
    {
        #region Форма собственности
        private int? ownForm;
        public int? OwnForm
        {
            get { return ownForm; }
            set
            {
                ownForm = value;
                OnPropertyChanged("OwnForm");
            }
        }
        #endregion

        #region Тип имущества
        private int? ownType;
        public int? OwnType
        {
            get { return ownType; }
            set
            {
                ownType = value;
                OnPropertyChanged("OwnType");
            }
        }
        #endregion

        #region Сумма в рублях
        private long? sumRub;
        public long? SumRub
        {
            get { return sumRub; }
            set
            {
                sumRub = value;
                OnPropertyChanged("SumRub");
            }
        }
        #endregion

    }
}
