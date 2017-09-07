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
using System.Threading;
using System.Windows.Threading;

namespace NodeModelsEssentials.Controls
{
    /// <summary>
    /// Interaction logic for CopyableWatch.xaml
    /// </summary>
    public partial class CopyableWatch : UserControl
    {
        private string currentText = "";
        private TextBox textWatch;
        private bool isCopyingText = false;

        public CopyableWatch()
        {
            InitializeComponent();
            textWatch = this.FindName("TextWatch") as TextBox;
        }

        private void CopyToClipboard()
        {
            if (isCopyingText) return;

            Clipboard.SetText(currentText);
            var _currentText = textWatch.Text;
            textWatch.Text = "Copied to clipboard!";
            isCopyingText = true;

            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(750) };
            timer.Start();
            timer.Tick += (_sender, _args) => {
                timer.Stop();
                this.Dispatcher.Invoke(() => {
                    textWatch.Text = _currentText;
                });
                isCopyingText = false;
            };
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            currentText = (sender as TextBox).Text;
        }

        private void TextWatch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.CopyToClipboard();
        }
    }
}
