using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DVR_UI_WPF
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
        List<DeviceItem> deviceListAllShow = new List<DeviceItem>();
		public MainWindow()
		{
			this.InitializeComponent();
            for (int i = 0; i < 32; i++)
            {
                DeviceItem di = new DeviceItem(i+1);

                //double marginLeft = (this.Width - 4 * di.Width) / 5;
                //di.Margin = new Thickness(marginLeft, 5, 0, 5);
                //di.Padding = new Thickness(0, 12, 0, 12);

                di.HorizontalAlignment = HorizontalAlignment.Right;
                di.DeviceID += di.DeviceID + i.ToString();
                deviceListAllShow.Add(di);
            }
			// 在此点下面插入创建对象所需的代码。
		}

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var item in deviceListAllShow)
            {
                InsertDeviceItem(item);
            }
        }

        private int InsertDeviceItem(DeviceItem di)
        {
            return this.wrapPanel_DeviceList.Children.Add(di);
        }

        private void wrapPanel_DeviceList_TouchMove(object sender, TouchEventArgs e)
        {
            //e.
            MessageBox.Show("未处理触摸");
        }

        private void wrapPanel_DeviceList_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            //this.wrapPanel_DeviceList.Margin.Top += e.Delta;
        }

        private void button_min_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button_min_MouseEnter(object sender, MouseEventArgs e)
        {
            this.button_min.Opacity = 1;
        }

        private void button_min_MouseLeave(object sender, MouseEventArgs e)
        {
            this.button_min.Opacity = 0.8;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="deviceIndex">区间 0-31</param>
        /// <param name="devicePath"></param>
        private void deviceConnected(string deviceID, int deviceIndex,string devicePath)
        {
            if (deviceIndex>-1 && deviceIndex< deviceListAllShow.Count)
            {
                this.deviceListAllShow[deviceIndex].DeviceID = deviceID;
                this.deviceListAllShow[deviceIndex].DevicePath = devicePath;
                this.deviceListAllShow[deviceIndex].IsConnected = true; 
            }
        }

        private void deviceDisConnected(string deviceID, int deviceIndex, string devicePath)
        {
            if (deviceIndex>-1 && deviceIndex < deviceListAllShow.Count)
            {
                this.deviceListAllShow[deviceIndex].IsConnected = false;
            }
        }



        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            int rNum = rd.Next(-20, 50);
            deviceConnected(DateTime.Now.Millisecond.ToString(), rNum, DateTime.Now.Second.ToString());
            rNum = rd.Next(-20, 50);
            deviceDisConnected(DateTime.Now.Millisecond.ToString(), rNum, DateTime.Now.Second.ToString());
        }
	}
}