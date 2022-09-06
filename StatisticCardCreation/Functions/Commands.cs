using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Documents;
using System.Printing;
using System.Windows.Media;
namespace StatCardApp
{

    //Шаблон команды
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }

    public class CommandList
    {
        //Печать первой страницы
        private RelayCommand printFirstPage;
        public RelayCommand PrintFirstPage
        {
            get
            {
                return printFirstPage ?? (printFirstPage = new RelayCommand(obj =>
                {

                    FlowDocumentScrollViewer scrollViewer = (FlowDocumentScrollViewer)obj;
                    FlowDocument docViewer = scrollViewer.Document;
                    PrintDialog printDialog = new PrintDialog();

                    try
                    {
                        printDialog.PrintVisual(((IDocumentPaginatorSource)docViewer).DocumentPaginator.GetPage(0).Visual, "Печатай");
                    }
                    catch (ArgumentNullException e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    scrollViewer.Height = SystemParameters.PrimaryScreenHeight - SystemParameters.PrimaryScreenHeight*0.1;

                }));
            }
        }

        //Печать второй страницы
        private RelayCommand printSecondPage;
        public RelayCommand PrintSecondPage
        {
            get
            {
                return printSecondPage ?? (printSecondPage = new RelayCommand(obj =>
                {
                    FlowDocumentScrollViewer scrollViewer = (FlowDocumentScrollViewer)obj;
                    FlowDocument docViewer = scrollViewer.Document;
                    PrintDialog printDialog = new PrintDialog();

                    DocumentPage page = ((IDocumentPaginatorSource)docViewer).DocumentPaginator.GetPage(1);
                    

                    try
                    {
                        printDialog.PrintVisual(page.Visual, "Печатай");
                    }
                    catch (ArgumentNullException e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    scrollViewer.Height = SystemParameters.PrimaryScreenHeight - SystemParameters.PrimaryScreenHeight * 0.1;
                }));
            }
        }

        //Полна печать
        private RelayCommand printFullDocument;
        public RelayCommand PrintFullDocument
        {
            get
            {
                return printFullDocument ?? (printFullDocument = new RelayCommand(obj =>
                {
                    FlowDocumentScrollViewer scrollViewer = (FlowDocumentScrollViewer)obj;
                    FlowDocument docViewer = scrollViewer.Document;
                    PrintDialog printDialog = new PrintDialog();

                    printDialog.PrintTicket.Duplexing = Duplexing.TwoSidedShortEdge;
                   

                    try
                    {
                        printDialog.PrintDocument(((IDocumentPaginatorSource)docViewer).DocumentPaginator, "Печатай");
                    }
                    catch (ArgumentNullException e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    scrollViewer.Height = SystemParameters.PrimaryScreenHeight - SystemParameters.PrimaryScreenHeight * 0.1;
                }));
            }
        }

        //Настраиваемая печать
        private RelayCommand printWithDialog;
        public RelayCommand PrintWithDialog
        {
            get
            {
                return printWithDialog ?? (printWithDialog = new RelayCommand(obj =>
                {
                    FlowDocumentScrollViewer scrollViewer = (FlowDocumentScrollViewer)obj;
                    FlowDocument docViewer = scrollViewer.Document;
                    PrintDialog printDialog = new PrintDialog();

                    printDialog.UserPageRangeEnabled = true;

                    Nullable<Boolean> print = printDialog.ShowDialog();

                    if (print == true)
                    {
                        printDialog.PageRangeSelection = PageRangeSelection.UserPages;

                        try
                        {
                            printDialog.PrintDocument(((IDocumentPaginatorSource)docViewer).DocumentPaginator, "Печатай");
                        }
                        catch (ArgumentNullException e)
                        {
                            MessageBox.Show(e.Message);
                        }

                        scrollViewer.Height = SystemParameters.PrimaryScreenHeight - SystemParameters.PrimaryScreenHeight * 0.1;
                    }

                }));
            }
        }





    }
}
