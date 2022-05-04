using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SoundListMx
{
    /// <summary>
    /// Логика взаимодействия для Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        public Configuration()
        {
            InitializeComponent();
        }

        private void BrowserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CommonFileDialog.IsPlatformSupported)
            {
                using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
                {
                    dialog.IsFolderPicker = true;
                    dialog.Multiselect = false;
                    if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        TextPath.Text = dialog.FileName;
                    }
                }
            }
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string path
        {
            get { return TextPath.Text; }
        }
    }
}
