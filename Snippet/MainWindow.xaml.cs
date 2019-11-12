using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml;
using System.Xml.Serialization;

namespace Snippet
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        const string SnippetHeader = @"
#SingleInstance, force
";
        Process AhkProcess;
        private void RunScripts()
        {
            AhkProcess?.Kill();
            var buider = new StringBuilder();
            buider.Append(SnippetHeader);
            foreach (var snippetPanel in SnippetContent.Children)
            {
                if (!(snippetPanel is SnippetPanel sp)) continue;
                var si = sp.SnippetItem;
                buider.Append(si.ToString());
            }
            File.WriteAllText("snippet.ahk", buider.ToString());
            AhkProcess = Process.Start("autohotkey.exe", "snippet.ahk");
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (Run.IsChecked)
            {
                RunScripts();
                SaveScripts();

                foreach (var item in SnippetContent.Children)
                {
                    switch (item)
                    {
                        case SnippetPanel sp:
                            sp.EditMod = false;
                            break;
                        case Button button:
                            button.Visibility = Visibility.Collapsed;
                            break;
                        default:
                            break;
                    }
                }
                Run.IsChecked = false;
            }
            else
            {
                foreach (var item in SnippetContent.Children)
                {
                    switch (item)
                    {
                        case SnippetPanel sp:
                            sp.EditMod = true;
                            break;
                        case Button button:
                            button.Visibility = Visibility.Visible;
                            break;
                        default:
                            break;
                    }
                }
                Run.IsChecked = true;
            }
        }

        private void SaveScripts()
        {
            var list = new List<SnippetItem>();
            foreach (var item in SnippetContent.Children)
            {
                switch (item)
                {
                    case SnippetPanel sp:
                        list.Add(sp.SnippetItem);
                        break;
                    default:
                        break;
                }
            }
            if (File.Exists("config.xml")) File.Delete("config.xml");
            var xml = new XmlSerializer(typeof(List<SnippetItem>));
            using (var writer = File.OpenWrite("config.xml"))
            {
                xml.Serialize(writer, list);
            }

        }

        private void AlwaysOnTop_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !AlwaysOnTop.IsChecked;
            AlwaysOnTop.IsChecked = !AlwaysOnTop.IsChecked;
        }

        private void AddSnippetButton_Click(object sender, RoutedEventArgs e)
        {
            var sp = new SnippetPanel();
            sp.EditMod = true;
            SnippetContent.Children.Insert(SnippetContent.Children.Count - 1, sp);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!File.Exists("config.xml")) return;
            var xml = new XmlSerializer(typeof(List<SnippetItem>));
            using (var reader = File.OpenRead("config.xml"))
            {
                var items = (List<SnippetItem>)xml.Deserialize(reader);
                foreach (var item in items)
                {
                    SnippetContent.Children.Insert(SnippetContent.Children.Count - 1, new SnippetPanel(item));
                }
            }
            Run_Click(null, null);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            AhkProcess.Kill();
        }
    }
}
