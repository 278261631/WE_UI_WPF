using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecordView
{
    /// <summary>
    /// MessageText.xaml 的交互逻辑
    /// </summary>
    public partial class MessageText : UserControl
    {
        public MessageText()
        {
            InitializeComponent();
        }

        private SolidColorBrush TextColor
        {
            //get;
            set
            {
                this.textBoxMessage.Foreground = value;
            }
        }

        private string TextString
        {
            set
            {
                this.textBoxMessage.Content = string.Empty;
                this.textBoxMessage.Content = value;
            }
        }

        public void ShowMessage(string textArg,SolidColorBrush colorBrushArg)
        {
            TextColor = colorBrushArg;
            TextString= textArg;
            this.MessageShow_BeginStoryboard1.Storyboard.Begin();
            //this.textBoxMessage.TextInput
        }
    }
}
