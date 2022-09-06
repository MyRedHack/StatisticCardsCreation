using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для CaseProductionWindow.xaml
    /// </summary>
    public partial class CaseProductionWindow : Window
    {
        CaseproductionViewModel ViewModel = new CaseproductionViewModel();
        public CaseProductionWindow(CaseInfoModel caseInfo)
        {
            InitializeComponent();
            ViewModel.ProductionInfo.CaseNumber = caseInfo.FullCaseNumber;

            this.DataContext = ViewModel;
        }

        private void SendCaseProductionToServer(object sender, RoutedEventArgs e)
        {
            ViewModel.TransferData();
        }
    }
}
