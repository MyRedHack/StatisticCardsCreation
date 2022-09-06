using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatisticProceeding
{
    public class CrimeQualification
    {
        #region  статья
        private int? article;
        public int? Article
        {
            get { return article; }
            set
            {
                article = value;
            }
        }
        #endregion

        #region знак
        private string sign;
        public string Sign
        {
            get { return sign; }
            set
            {
                sign = value;
            }
        }
        #endregion

        #region часть
        private string part;
        public string Part
        {
            get { return part; }
            set
            {
                part = value;
            }
        }
        #endregion

        #region пункт
        private string paragraph;
        public string Paragraph
        {
            get { return paragraph; }
            set
            {
                paragraph = value;
            }
        }
        #endregion

    }
}
