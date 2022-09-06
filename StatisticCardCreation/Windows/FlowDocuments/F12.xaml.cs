using StatCardApp.Model;
using StatCardApp.ViewModel;
using System.Windows;
using System.Windows.Documents;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF12.xaml
    /// </summary>
    public partial class DocumentViewerF12 : Window
    {
        private DocViewerF12ViewModel docViewerViewModel;

        public DocumentViewerF12(CardF1DotTwo F12, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F12.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF12ViewModel(F12, regData, CasePath);
            if (F12.IsExported)
            {
                docViewerViewModel.CaseInfo = F12.CaseInfo;
                docViewerViewModel.RegData = F12.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}