using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DocumentViewerF55.xaml
    /// </summary>
    public partial class DocumentViewerF55 : Window
    {
        private DocViewerF55ViewModel docViewerViewModel;

        public DocumentViewerF55(CardF5DotFive F55, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F55.IsExported) CasePath = string.Empty;
            docViewerViewModel = new DocViewerF55ViewModel(F55, regData, CasePath);
            if (F55.IsExported)
            {
                docViewerViewModel.CaseInfo = F55.CaseInfo;
                docViewerViewModel.RegData = F55.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}