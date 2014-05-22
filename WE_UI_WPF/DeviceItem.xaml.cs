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

namespace DVR_UI_WPF
{
    /// <summary>
    /// DeviceItem.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceItem : UserControl
    {

        private string defaultDeviceID = "未连接";
        //private string defaultDevicePath = "无路径";
        private string defaultIndex = "00";
        public DeviceItem(int index)
        {
            InitializeComponent();
            //this.textBlock_Index.Text = index.ToString(defaultIndex);
            this.Width = 180;
            this.Height = 26;
            this.textBlock_DeviceID.Text = defaultDeviceID;
            //this.textBlock_DevicePath.Text = defaultDevicePath;
        }

        private bool isConnected = false;
        public bool IsConnected
        {
            get { return isConnected; }
            set { isConnected = value;
                    this.ellipse_isConnected.Fill = isConnected ? Brushes.LightGreen : Brushes.Gray;
                    if (!value)
                    {
                        //this.textBlock_DeviceID.Text = defaultDeviceID;
                        //this.textBlock_DevicePath.Text = defaultDevicePath;
                    }
            }
        }

        public string DeviceID
        {
          get { return this.textBlock_DeviceID.Text; }
          set { this.textBlock_DeviceID.Text = value; }
        }

        public string DevicePath
        {
            get { 
                //return this.textBlock_DevicePath.Text; 
                return null; 
            }
            set { 
                //this.textBlock_DevicePath.Text = value;
            }
        }
        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Opacity = 1;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Opacity = 0.9;
        }
    }
}
