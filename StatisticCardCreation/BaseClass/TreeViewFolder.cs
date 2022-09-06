using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using StatCardApp.Function;
using StatCardApp.Model;
using StatCardApp.Model.CardFields;
using StatCardApp.Global;

namespace StatCardApp
{
    public class TreeViewFolder : Base
    {
        public TreeViewFolderItem root { get; set; } = new TreeViewFolderItem();
        public TreeViewFolder(string Path, string Title)
        {
            root.Path = Path;
            root.Title = Title;
            root.ImagePath = "Resources\\folder.png";
            AddDirectories(root);
        }


        private void AddDirectories(TreeViewFolderItem item)
        {
            string[] nestedDirectories = Directory.GetDirectories(item.Path);

            foreach (string directoryStr in nestedDirectories)
            {
                DirectoryInfo directory = new DirectoryInfo(directoryStr);
                TreeViewFolderItem subItem = new TreeViewFolderItem() {
                    Title = directory.Name,
                    Path = directory.FullName,
                    ImagePath = "Resources\\folder.png",
                    Parent = item
                };
                AddDirectories(subItem);
                subItem.IsCase = CheckDirectoryIsCase(subItem);
                item.Items.Add(subItem);
            }

            AddFiles(item);
        }

        private void AddFiles(TreeViewFolderItem item)
        {
            string[] nestedFiles = Directory.GetFiles(item.Path);

            foreach (string fileStr in nestedFiles)
            {
                FileInfo file = new FileInfo(fileStr);
                
                if (file.FullName.Contains(".json"))
                {
                    StatisticCard card = StatCardFunc.FromJson1(file.FullName, typeof(StatisticCard));
                    item.CardList.Add(card.VisualData);
                }
            }
        }

        private bool CheckDirectoryIsCase(TreeViewFolderItem item)
        {
            string[] nestedFiles = Directory.GetFiles(item.Path);
            bool result = false;
            foreach (string fileStr in nestedFiles)
            {
                FileInfo file = new FileInfo(fileStr);

                if (file.FullName.Contains("caseInfo.conf"))
                    result = true;
            }

            return result;
        }
    }
    

}
