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
using System.IO;

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
                    deviceDic.Add(de.DeviceID, de);
                    di.IsConnected = true;
                }));
            }
            else
            {
                deviceDic[de.DeviceID] = de;
                wrapPanel_DeviceList.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var item in wrapPanel_DeviceList.Children)
                    {
                        DeviceItem itemDI = item as DeviceItem;
                        if (itemDI.DeviceID == de.DeviceID)
                        {
                            itemDI.IsConnected = true;
                            break;
                        }
                    }
                }));


                
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
            
        }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="deviceIndex">区间 0-31</param>
        /// <param name="devicePath"></param>
        public void deviceConnected(DeviceEntity device)
        {
            ShowMessage(DeviceDetectorMessage.DeviceConnected,string.Empty);

            device.onDeviceMessage +=  this.ShowMessage;
            device.onDeviceInitFinished += new EventHandler(DeviceInitFinished);
            //device.onDeviceDataIn += new DeviceEntity.DeviceDataIn(device_onDeviceDataIn);
            device.onDeviceGetFileInfoFinished += new EventHandler(device_onDeviceGetFileInfoFinished);
            device.onDeviceGetFilePartFinished += new EventHandler(device_onDeviceGetFilePartFinished);
            device.onDeviceGetFileFinished += new EventHandler(device_onDeviceGetFileFinished);
            //device.onDeviceInitFinished += new EventHandler(device_onDeviceInitFinished);
            device.onDeviceNetLost += new EventHandler(device_onDeviceNetLost);


            device.SendGetVersion();
        }

        void device_onDeviceNetLost(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 这里需要保存一个固定的数据路径 用ClickOnece运行路径会变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void device_onDeviceGetFileFinished(object sender, EventArgs e)
        {
            //复制文件
            DeviceEntity device = sender as DeviceEntity;
            if (device != null)
            {
                string deviceFilePathRoot = System.IO.Path.Combine(Environment.CurrentDirectory, "DataFiles", device.DeviceID);
                if (!Directory.Exists(deviceFilePathRoot))
                {
                    try
                    {
                        Directory.CreateDirectory(deviceFilePathRoot);
                    }
                    catch (Exception ex)
                    {

                        // UI 异常处理
                        throw;
                    }
                    try
                    {
                        File.Move(device.tempFilePath, System.IO.Path.Combine(deviceFilePathRoot, device.ReceiveFileName));
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }

            }

        }

        void device_onDeviceInitFinished(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void device_onDeviceGetFilePartFinished(object sender, EventArgs e)
        {
            DeviceEntity device = sender as DeviceEntity;
            if (device != null)
            {
                device.SendGetFileByte(device.NextPartWriteCount, device.ReceiveFileName);
            }
        }

        void device_onDeviceGetFileInfoFinished(object sender, EventArgs e)
        {
            DeviceEntity device = sender as DeviceEntity;
            if (device!=null)
            {
                device.SendGetFileByte(device.NextPartWriteCount, device.ReceiveFileName);
            }
        }

        void device_onDeviceDataIn(byte[] dataIn)
        {
            throw new NotImplementedException();
        }


        void DeviceInitFinished(object sender,EventArgs e)
        {
            InsertDeviceItem(sender as DeviceEntity);
            DeviceEntity device = sender as DeviceEntity;
            if (device!=null)
            {
                device.SendGetFileInfo();
            }
        }


        public void deviceDisConnected(DeviceEntity device)
        {
            if (!deviceDic.ContainsKey(device.DeviceID))
            {
                //this.wrapPanel_DeviceList.Dispatcher.Invoke(new Action(() =>
                //{
                //    DeviceItem di = new DeviceItem(0);
                //    di.HorizontalAlignment = HorizontalAlignment.Right;
                //    deviceListAllShow.Add(di);
                //    di.DeviceID = device.DeviceID;

                //    this.wrapPanel_DeviceList.Children.Add(di);
                //    deviceDic.Add(device.DeviceID, device);
                //    di.IsConnected = true;
                //}));
            }
            else
            {
                wrapPanel_DeviceList.Dispatcher.Invoke(new Action(() =>
                {
                    foreach (var item in wrapPanel_DeviceList.Children)
                    {
                        DeviceItem itemDI = item as DeviceItem;
                        if (itemDI.DeviceID == device.DeviceID)
                        {

                            itemDI.IsConnected = false;
                            break;
                        }
                    }
                }));


            }
            ShowMessage(DeviceDetectorMessage.DeviceDisConnected,string.Empty);


        }



        private void button_test_Click(object sender, RoutedEventArgs e)
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            int rNum = rd.Next(-20, 50);
            //deviceConnected(DateTime.Now.Millisecond.ToString(), rNum, DateTime.Now.Second.ToString());
            rNum = rd.Next(-20, 50);
            //deviceDisConnected(DateTime.Now.Millisecond.ToString(), rNum, DateTime.Now.Second.ToString());
        }

        public void ShowMessage(DeviceDetectorMessage messageArg,string errorMessage)
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
                    case DeviceDetectorMessage.DeviceDisConnected:
                        textArg="已断开";
                        break;

                    default:
                        textArg = "未定义的信息";
                        break;
                }
                textArg += ":"+errorMessage;
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


        private void label_Close_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                this.Close();
            }
        }

        private void label_min_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                this.WindowState = WindowState.Minimized;
            }
            
        }

        private void label_Menu_SetSSID_PWD_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in this.deviceDic.Values)
            {
                item.SendSetWifiSSID_PWD();
            }
        }

        private void label_Menue_SetTime_MouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var item in this.deviceDic.Values)
            {
                item.SendSetTime();
            }
        }

        //private void button_min_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        //{
        //    // 在此处添加事件处理程序实现。
        //}
	}

    public enum WEErrorLevel
	{
        Success,
        Message,
        Warnning,
        Error,
	}


}