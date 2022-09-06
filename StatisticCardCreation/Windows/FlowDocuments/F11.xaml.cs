using StatCardApp.Model;
using StatCardApp.ViewModel;
using System.Windows;
using System.Windows.Documents;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF11.xaml
    /// </summary>
    public partial class DocumentViewerF11 : Window
    {
        private DocViewerF11ViewModel docViewerViewModel;

        public DocumentViewerF11(CardF1DotOne F11, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F11.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF11ViewModel(F11, regData, CasePath);
            if (F11.IsExported)
            {
                docViewerViewModel.CaseInfo = F11.CaseInfo;
                docViewerViewModel.RegData = F11.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}