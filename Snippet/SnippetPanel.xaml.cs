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

namespace Snippet
{
    /// <summary>
    /// SnippetPanel.xaml 的交互逻辑
    /// </summary>
    public partial class SnippetPanel : UserControl
    {
        public SnippetPanel()
        {
            InitializeComponent();
            SnippetItem = new SnippetItem();
        }

        public SnippetPanel(SnippetItem item)
        {
            InitializeComponent();
            SnippetItem = item;
        }

        Visibility VisibilityMod = Visibility.Collapsed;

        public bool EditMod
        {
            get
            {
                return VisibilityMod != Visibility.Collapsed;
            }
            set
            {
                VisibilityMod = value ? Visibility.Visible : Visibility.Collapsed;
                DeletItemButton.Visibility = VisibilityMod;
                Trigger.IsEnabled = value;
                Script.IsEnabled = value;
                IsHotKey.Visibility = VisibilityMod;
                IsSnippet.Visibility = VisibilityMod;
                ClipboardOrSend.Visibility = VisibilityMod;
            }
        }

        public SnippetItem SnippetItem
        {
            get
            {
                return new SnippetItem()
                {
                    Trigger = Trigger.Text,
                    Script = Script.Text,
                    IsHotKey = IsHotKey.IsChecked ?? false,
                    IsSnippet = IsSnippet.IsChecked ?? false,
                    ClipboardOrSend = ClipboardOrSend.IsChecked ?? false
                };
            }
            set
            {
                Trigger.Text = value.Trigger;
                Script.Text = value.Script;
                IsHotKey.IsChecked = value.IsHotKey;
                IsSnippet.IsChecked = value.IsSnippet;
                ClipboardOrSend.IsChecked = value.ClipboardOrSend;
            }
        }

        private void DeletItemButton_Click(object sender, RoutedEventArgs e)
        {
            var panel = Parent as Panel;
            if (panel != null)
            {
                panel.Children.Remove(this);
            }
        }
    }
}
