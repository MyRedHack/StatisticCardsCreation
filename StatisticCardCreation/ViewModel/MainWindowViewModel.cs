using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StatCardApp.Client;
using StatCardApp.Model;
using System.Windows.Controls;
using StatCardApp.Model.CardFields;
using System.Windows;
using StatCardApp.Function;

namespace StatCardApp.ViewModel
{
    public class MainWindowViewModel : Base
    {
        private TreeViewFolderItem statCardList = new TreeViewFolderItem();
        public TreeViewFolderItem StatCardList
        {
            get => statCardList;
            set
            {
                statCardList = value;
                OnPropertyChanged("StatCardList");
            }
        }

        private CardAvaliability cardAvaliability = new CardAvaliability();
        public CardAvaliability CardAvaliability
        {
            get => cardAvaliability;
            set
            {
                cardAvaliability = value;
                OnPropertyChanged("CardAvaliability");
            }
        }

        private CaseInfoViewModel caseInfoVM = new CaseInfoViewModel();
        public CaseInfoViewModel CaseInfoVM
        {
            get => caseInfoVM;
            set
            {
                caseInfoVM = value;
                OnPropertyChanged("CaseInfoVM");
            }
        }

        public void OnCardNameFilter(object sender, System.Windows.Data.FilterEventArgs e)
        {

        }
    }
}
