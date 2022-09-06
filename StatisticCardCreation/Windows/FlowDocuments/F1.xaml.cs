using StatCardApp.Model;
using StatCardApp.ViewModel;
using System.Windows;
using System.Windows.Documents;
using StatCardApp.Function;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewer.xaml
    /// </summary>
    public partial class DocumentViewerF1 : Window
    {
        private DocViewerF1ViewModel docViewerViewModel;

        public DocumentViewerF1(CardF1 F1, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F1.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF1ViewModel(F1, regData, CasePath);
            DataContext = docViewerViewModel;
            if (F1.IsExported)
            {
                docViewerViewModel.CaseInfo = F1.CaseInfo;
                docViewerViewModel.RegData = F1.RegData;
                docViewerViewModel.init();
            }
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }

       
    }
}