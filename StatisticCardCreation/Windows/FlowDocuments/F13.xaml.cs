using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF13.xaml
    /// </summary>
    public partial class DocumentViewerF13 : Window
    {
        private DocViewerF13ViewModel docViewerViewModel;

        public DocumentViewerF13(CardF1DotThree F13, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F13.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF13ViewModel(F13, regData, CasePath);
            if (F13.IsExported)
            {
                docViewerViewModel.CaseInfo = F13.CaseInfo;
                docViewerViewModel.RegData = F13.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}