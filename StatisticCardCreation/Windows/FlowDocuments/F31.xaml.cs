using System.Windows;
using System.Windows.Documents;
using StatCardApp.ViewModel;
using StatCardApp.Model;

namespace StatCardApp
{
    /// <summary>
    /// Логика взаимодействия для DockViewerF31.xaml
    /// </summary>
    public partial class DocumentViewerF31 : Window
    {
        private DocViewerF31ViewModel docViewerViewModel;

        public DocumentViewerF31(CardF3DotOne F31, RegModel regData, string CasePath)
        {
            InitializeComponent();
            if (F31.IsExported) CasePath = string.Empty;

            docViewerViewModel = new DocViewerF31ViewModel(F31, regData, CasePath);

            if (F31.IsExported)
            {
                docViewerViewModel.CaseInfo = F31.CaseInfo;
                docViewerViewModel.RegData = F31.RegData;
                docViewerViewModel.init();
            }
            DataContext = docViewerViewModel;
            //Разделение документа на страницы
            ((IDocumentPaginatorSource)docViewer).DocumentPaginator.ComputePageCount();
        }
    }
}