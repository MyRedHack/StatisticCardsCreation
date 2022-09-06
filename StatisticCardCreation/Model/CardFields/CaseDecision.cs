using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatCardApp.Model.CardFields
{
    //Решение по делу
    public class CaseDecision : Base
    {
        //Решение по делу
        private int? decision;
        public int? Decision
        {
            get => decision;
            set
            {
                decision = value;
                OnPropertyChanged("Decision");
            }
        }

        //Дата решения
        private DateTime? date;
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        //Где принято решение
        private int? source;
        public int? Source
        {
            get => source;
            set
            {
                source = value;
                OnPropertyChanged("Source");
            }
        }

        //Срок продления (установки) следствия (дознания)
        private int? termExtension;
        public int? TermExtension
        {
            get => termExtension;
            set
            {
                termExtension = value;
                OnPropertyChanged("TermExtension");
            }
        }

        //Дата окончания продления (установки) следствия (дознания)
        private DateTime? termExtensionDate;
        public DateTime? TermExtensionDate
        {
            get => termExtensionDate;
            set
            {
                termExtensionDate = value;
                OnPropertyChanged("TermExtensionDate");
            }
        }

        //Причина остановки следствия
        private int? caseExtendReason;
        public int? CaseExtendReason
        {
            get => caseExtendReason;
            set
            {
                caseExtendReason = value;
                OnPropertyChanged("CaseExtendReason");
            }
        }
    }
}
