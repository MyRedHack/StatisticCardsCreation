using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using StatCardApp.Model.CardFields;

namespace StatCardApp
{
    public class TreeViewFolderItem : Base
    {
        public TreeViewFolderItem Parent { get; set; }

        public string Title { get; set; }

        public string Path { get; set; }

        public string ImagePath { get; set; }

        public Boolean IsCase { get; set; }

        public DataTemplate HeaderTemplate { get; set; }

        private Boolean sentToServer;
        public Boolean SentToServer {
            get => sentToServer;
            set
            {
                sentToServer = value;
                SentToServerImagePath = sentToServer ? "Resources\\check.png" : "Resources\\cross.png";
                OnPropertyChanged("SentToServer");
            }

        }

        private string sentToServerImagePath;
        public string SentToServerImagePath
        {
            get => sentToServerImagePath;
            set
            {
                sentToServerImagePath = value;
                OnPropertyChanged("SentToServerImagePath");
            }
        }

        public ObservableCollection<StatCardVisualData> CardList { get; set; }
        public ObservableCollection<TreeViewFolderItem> Items { get; set; }

        public TreeViewFolderItem()
        {
            Items = new ObservableCollection<TreeViewFolderItem>();
            CardList = new ObservableCollection<StatCardVisualData>();
        }

        
    }
}
