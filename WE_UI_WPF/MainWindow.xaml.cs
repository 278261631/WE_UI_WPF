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
using PnPDetectorDLL;

namespace DVR_UI_WPF
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
        List<DeviceItem> deviceListAllShow = new List<DeviceItem>();
        public event EventHandler onWindowsDispose;

		public MainWindow()
		{
			this.InitializeComponent();
            //for (int i = 0; i < 32; i++)
            //{
            //    DeviceItem di = new DeviceItem(i+1);

            //    //double marginLeft = (this.Width - 4 * di.Width) / 5;
            //    //di.Margin = new Thickness(marginLeft, 5, 0, 5);
            //    //di.Padding = new Thickness(0, 12, 0, 12);

            //    di.HorizontalAlignment = HorizontalAlignment.Right;
            //    di.DeviceID += di.DeviceID + i.ToString();
            //    deviceListAllShow.Add(di);
            //}
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
                //InsertDeviceItem(item);
            }
        }

        //private int InsertDeviceItem(DeviceItem di)
        //private void InsertDeviceItem(DeviceItem di)
        /// <summary>
        /// 历史连接列表：key DeviceID  Value DeviceEntity
        /// </summary>
        Dictionary<string, DeviceEntity> deviceDic = new Dictionary<string, DeviceEntity>();
        private void InsertDeviceItem(DeviceEntity de)
        {
            //return this.wrapPanel_DeviceList.Children.Add(di);

            if (!deviceDic.ContainsKey(de.DeviceID))
            {
                this.wrapPanel_DeviceList.Dispatcher.Invoke(new Action(() =>
                {
                    DeviceItem di = new DeviceItem(0);
                    di.HorizontalAlignment = HorizontalAlignment.Right;
                    deviceListAllShow.Add(di);
                    di.DeviceID = de.DeviceID;

                    this.wrapPanel_DeviceList.Children.Add(di);
                }));
            }
            else
            {

            }

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
        public void deviceConnected(DeviceEntity device)
        {
            //if (deviceIndex>-1 && deviceIndex< deviceListAllShow.Count)
            //{
            //    this.deviceListAllShow[deviceIndex].DeviceID = deviceID;
            //    this.deviceListAllShow[deviceIndex].DevicePath = devicePath;
            //    this.deviceListAllShow[deviceIndex].IsConnected = true; 
            //}

            //DeviceItem di = new DeviceItem(0);
            //di.HorizontalAlignment = HorizontalAlignment.Right;
            //deviceListAllShow.Add(di);
            //InsertDeviceItem(di);

            //--- Check if Exist
            //clone entity to own list
            ShowMessage(DeviceDetectorMessage.DeviceConnected);

            device.onDeviceMessage +=  this.ShowMessage;
            device.onDeviceInitFinished += new EventHandler(DeviceInitFinished);
            //device.GetInitDevice();
        }


        void DeviceInitFinished(object sender,EventArgs e)
        {
            InsertDeviceItem(sender as DeviceEntity);
        }


        public void deviceDisConnected(DeviceEntity device)
        {
            //if (this.deviceListAllShow.)
            //{
            //    this.deviceListAllShow[deviceIndex].IsConnected = false;
            //}


        }



        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            int rNum = rd.Next(-20, 50);
            //deviceConnected(DateTime.Now.Millisecond.ToString(), rNum, DateTime.Now.Second.ToString());
            rNum = rd.Next(-20, 50);
            //deviceDisConnected(DateTime.Now.Millisecond.ToString(), rNum, DateTime.Now.Second.ToString());
        }

        public void ShowMessage(DeviceDetectorMessage messageArg)
        {
            this.messageTextBox.Dispatcher.Invoke(new Action(() => {
                string textArg = string.Empty;
                switch (messageArg)
                {
                    case DeviceDetectorMessage.IPEndPointError:
                        textArg = "IP地址错误";
                        break;
                    case DeviceDetectorMessage.ConnectionError:
                        textArg = "网络连接错误";
                        break;
                    case DeviceDetectorMessage.DeviceConnected:
                        textArg = "已经连接";
                        break;
                    default:
                        textArg = "未定义的错误";
                        break;
                }
                //SolidColorBrush scb = Brushes.Black;
                //this.messageTextBox.ShowMessage(textArg, scb);
                this.messageTextBox.ShowMessage(textArg, Brushes.Black);
            }));
            
        }

        public void ShowMessage(string textArg,WEErrorLevel errorArg)
        {
            SolidColorBrush scb = Brushes.Black;
            switch (errorArg)
            {
                case WEErrorLevel.Success:
                    scb = Brushes.DarkOliveGreen;
                    break;
                case WEErrorLevel.Message:
                    scb = Brushes.Black;
                    break;
                case WEErrorLevel.Warnning:
                    scb = Brushes.LightGoldenrodYellow;
                    break;
                case WEErrorLevel.Error:
                    scb = Brushes.DarkRed;
                    break;
                default:
                    break;
            }
            this.messageTextBox.ShowMessage(textArg,scb);
        }

        public void ShowMessage(string textArg)
        {
            this.messageTextBox.Dispatcher.Invoke(new Action(() =>
            {
                this.messageTextBox.ShowMessage(textArg, Brushes.Black);
            }));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.onWindowsDispose!=null)
            {
                this.onWindowsDispose(null, null);
            }
        }
	}

    public enum WEErrorLevel
	{
        Success,
        Message,
        Warnning,
        Error,
	}


}