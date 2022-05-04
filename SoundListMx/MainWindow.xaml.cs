using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections.Generic;
using System.Windows.Media;

namespace SoundListMx
{
    public partial class MainWindow : Window
    {
        private string pathGmod = Properties.Settings.Default.pathGame;

        private string soundPath;
        private MediaPlayer player = new MediaPlayer();

        private bool CheckPathToGame()
        {
            try
            {
                string pathGmodContent = pathGmod + @"garrysmod";

                if (Directory.Exists(pathGmodContent + "\\sound") && Directory.Exists(pathGmodContent + "\\addons"))
                {
                    Properties.Settings.Default.pathGame = pathGmod;
                    Properties.Settings.Default.Save();
                    return true;
                }
                else
                {
                    throw new System.Exception();
                }
            }
            catch (System.Exception)
            {
                Configuration configMenu = new Configuration();

                if (configMenu.ShowDialog() == true)
                {
                    if (configMenu.path.EndsWith("\\"))
                        pathGmod = configMenu.path;
                    else
                        pathGmod = configMenu.path + "\\";
                    return CheckPathToGame();
                }
            }
            return false;
        }

        public MainWindow()
        {
            util.Resolver.RegisterDependencyResolver();
            InitializeComponent();
            if (CheckPathToGame())
            {
                string pathGmodSounds = pathGmod + @"garrysmod\sound";
                string pathGmodAddons = pathGmod + @"garrysmod\addons";

                var treeViewFolder = new TreeViewFolder();

                var folderSound = new TreeViewItem();
                folderSound.Header = "Sounds";


                var folderAddons = new TreeViewItem();
                folderAddons.Header = "Addons";

                var sounds = treeViewFolder.GetItems(pathGmodSounds);
                var addons = treeViewFolder.GetItems(pathGmodAddons);


                foreach (var sound in sounds)
                {
                    folderSound.Items.Add(sound);
                }

                foreach (var addon in addons)
                {
                    folderAddons.Items.Add(addon);
                }

                ElementTreeViewFolder.Items.Add(folderSound);
                ElementTreeViewFolder.Items.Add(folderAddons);
            }
            else
            {
                Close();
            }
        }

        private void TreeViewFolderItem_Click(object sender, MouseButtonEventArgs e)
        {
            var item = (TreeViewItem)sender;
            var path = (string)item.ToolTip;

            if (path != null)
            {
                var dirInfo = new DirectoryInfo(path);
                var items = new List<ListBoxFolderItem>();
                var files = dirInfo.GetFiles();

                for (int i = 0; i < files.Length; i++)
                {
                    string color = "White";
                    if (i % 2 == 0) color = "#e0e0e0";

                    if (files[i].Name.EndsWith(".wav") || files[i].Name.EndsWith(".mp3"))
                    {

                        items.Add(new ListBoxFolderItem()
                        {
                            Sound = files[i].Name,
                            SoundPath = files[i].FullName.Substring(pathGmod.Length + 10),
                            Background = color,
                        });
                    }
                }
                ListBoxFolder.ItemsSource = items;

                item.Focusable = true;
                item.Focus();
                item.Focusable = false;
                e.Handled = true;
            }
        }

        private void ButtonCopyPath_Click(object sender, RoutedEventArgs e)
        {
            if (soundPath != null)
            {
                Clipboard.SetText(soundPath);
            }
        }

        private void ButtonStopSound_Click(object sender, RoutedEventArgs e)
        {
            player.Stop();
        }

        private void ListBoxFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var item = (ListBoxFolderItem)listBox.SelectedItem;
            if (item != null)
            {
                string path = pathGmod + "garrysmod\\" + item.SoundPath;
                player.Stop();
                player.Open(new System.Uri(path, System.UriKind.Absolute));
                player.Play();

                string soundFolder = "sound\\";
                int posSoundFolder = item.SoundPath.IndexOf(soundFolder);
                soundPath = item.SoundPath.Substring(posSoundFolder + soundFolder.Length).Replace("\\", "/");
            }
        }
    }
}
