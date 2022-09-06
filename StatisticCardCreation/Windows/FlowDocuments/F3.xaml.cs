using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF3.xaml
    /// </summary>
    public partial class DocumentViewerF3 : Window
    {
        private DocViewerF3ViewModel docViewerViewModel;

        public DocumentViewerF3(CardF3 F3, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F3.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF3ViewModel(F3, regData, CasePath);
            if (F3.IsExported)
            {
                docViewerViewModel.CaseInfo = F3.CaseInfo;
                docViewerViewModel.RegData = F3.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}