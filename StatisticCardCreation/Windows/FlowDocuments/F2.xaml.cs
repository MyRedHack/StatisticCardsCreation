using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF2.xaml
    /// </summary>
    public partial class DocumentViewerF2 : Window
    {
        private DocViewerF2ViewModel docViewerViewModel;

        public DocumentViewerF2(CardF2 F2, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F2.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF2ViewModel(F2, regData, CasePath);
            if (F2.IsExported)
            {
                docViewerViewModel.CaseInfo = F2.CaseInfo;
                docViewerViewModel.RegData = F2.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}