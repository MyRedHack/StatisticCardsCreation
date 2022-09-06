using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для F6.xaml
    /// </summary>
    public partial class DocumentViewerF6 : Window
    {
        private DocViewerF6ViewModel docViewerViewModel;

        public DocumentViewerF6(CardF6 F6, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F6.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF6ViewModel(F6, regData, CasePath);
            if (F6.IsExported)
            {
                docViewerViewModel.CaseInfo = F6.CaseInfo;
                docViewerViewModel.RegData = F6.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}