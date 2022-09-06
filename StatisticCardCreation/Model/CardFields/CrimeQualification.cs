using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Global;

namespace StatCardApp.Model.CardFields
{
    public class CrimeQualification : Base
    {
        #region  статья
        private int? article;
        public int? Article
        {
            get { return article; }
            set
            {
                article = value;
                OnPropertyChanged("Article");
                StatCardFileTree.AddChangedField(new ChangedField("article", $"{value}"));
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
                OnPropertyChanged("Sign");
                StatCardFileTree.AddChangedField(new ChangedField("sign", $"{value}"));
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
                OnPropertyChanged("Part");
                StatCardFileTree.AddChangedField(new ChangedField("part", $"'{value}'"));
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
                OnPropertyChanged("Paragraph");
                StatCardFileTree.AddChangedField(new ChangedField("paragraph", $"'{value}'"));
            }
        }
        #endregion

        public string StringView
        {
            get { return $"ст. {article}, ч. {part}, п. {paragraph}"; }
        }
    }
}
