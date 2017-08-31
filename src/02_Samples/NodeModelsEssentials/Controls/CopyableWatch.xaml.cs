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

namespace NodeModelsEssentials.Controls
{
    /// <summary>
    /// Interaction logic for CopyableWatch.xaml
    /// </summary>
    public partial class CopyableWatch : UserControl
    {
        private string currentText = "";

        public CopyableWatch()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(currentText);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentText = (sender as TextBox).Text;
        }
    }
}
