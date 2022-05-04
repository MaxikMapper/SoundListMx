using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;

namespace SoundListMx
{    public class TreeViewFolder
    {
        public string CheckSoundsAddons(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var dir in dirInfo.GetDirectories())
            {
                if (dir.Name == "sound")
                {
                    return dir.FullName;
                }
            }
            return "";
        }

        public List<TreeViewItem> GetItems(string path)
        {
            var items = new List<TreeViewItem>();

            var dirInfo = new DirectoryInfo(path);

            foreach (var dir in dirInfo.GetDirectories())
            {
                var checkPath = dir.FullName;
                if (path.EndsWith("addons"))
                {
                    checkPath = CheckSoundsAddons(dir.FullName);
                    if (checkPath == "") 
                        continue;

                }
                var item = new TreeViewItem();
                item.Header = dir.Name;
                item.ToolTip = checkPath;
                item.ItemsSource = GetItems(checkPath);

                items.Add(item);
            }

            return items;
        }
    }
}
