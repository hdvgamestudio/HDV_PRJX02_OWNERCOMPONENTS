using HDV.FntFontToWPK;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FntFontToWPK
{

    public class FontHolder
    {
        public string Font
        {
            set;
            get;
        }

        public List<string> TextureFiles
        {
            set;
            get;
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private FontHolder m_currentFontHolder;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnImportFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fnt Files (*.fnt)|*.fnt|All Files (*.*)|*.*";
            bool? isOpenned = openFileDialog.ShowDialog(this);

            if (isOpenned != null && isOpenned.Value)
            {
                string filePath = openFileDialog.FileName;

                FntFont font = new FntFont(filePath);

                List<string> textureFiles = new List<string>();

                string directory = System.IO.Path.GetDirectoryName(filePath);
                foreach (var texturePage in font.Pages.Values)
                {
                    string textureFileName = System.IO.Path.Combine(directory, texturePage.TextureFile);
                    if (!System.IO.File.Exists(textureFileName))
                    {
                        MessageBox.Show(this, 
                            "Has not texture file in same directory", "Error", 
                            MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    textureFiles.Add(textureFileName);
                }

                this.m_currentFontHolder = new FontHolder
                {
                    Font = filePath,
                    TextureFiles = textureFiles,
                };
                this.Title = string.Format("{0}", font.FontName);

            }
        }

        static readonly string serviceName = "HDV.MadVirus.Entities.GUI.FntFont v1.0.0.0";
        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (m_currentFontHolder == null)
                return;

            string fntFileName = m_currentFontHolder.Font;
            string directory = System.IO.Path.GetDirectoryName(fntFileName);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(fntFileName);

            Exporter.Export(
                System.IO.Path.Combine(directory, fileName + ".wpk"), 
                serviceName, 
                fntFileName, 
                m_currentFontHolder.TextureFiles.ToArray());

            MessageBox.Show("Done");
        }
    }
}
