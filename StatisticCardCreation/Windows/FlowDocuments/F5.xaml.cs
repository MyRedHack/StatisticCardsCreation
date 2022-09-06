using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF5.xaml
    /// </summary>
    public partial class DocumentViewerF5 : Window
    {
        private DocViewerF5ViewModel docViewerViewModel;

        public DocumentViewerF5(CardF5 F5, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F5.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF5ViewModel(F5, regData, CasePath);
            if (F5.IsExported)
            {
                docViewerViewModel.CaseInfo = F5.CaseInfo;
                docViewerViewModel.RegData = F5.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}