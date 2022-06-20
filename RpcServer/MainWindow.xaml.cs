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
using Quick.Communication;
using RpcProtocol;

namespace RpcServer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IWeatherService
    {
        private TcpRpcServer _server;
        public MainWindow()
        {
            InitializeComponent();
            _server = new TcpRpcServer();
            _server.AddLocalService<IWeatherService>(this);
            _server.AddLocalService<ITicketService>(new TicketService());
            _server.RegisterRemoteServiceProxy<IClientNotify>();
            //_server.ClientStatusChanged += _server_ClientStatusChanged;
            _server.MessageReceived += _server_MessageReceived;
            _server.Start(5000);
        }

        private void _server_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            byte[] data = e.MessageRawData.ToArray();
            string strMsg = Encoding.UTF8.GetString(data);
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                tbxLog.AppendText(strMsg + "\r\n");
            }));
        }

        public Weather GetWeather(DateTime day)
        {
            string strDay = day.ToString("yyyy-MM-dd");
            if (strDay == "2022-06-17")
            {
                return new Weather()
                {
                    Temperature = 30,
                    Description = "晴"
                };
            }
            else if (strDay == "2022-06-18")
            {
                return new Weather()
                {
                    Temperature = 27,
                    Description = "中雨"
                };
            }
            throw new Exception("不知道！");
        }

        private void ButtonSendAlert_Click(object sender, RoutedEventArgs e)
        {
            //用于获取所有客户端的服务代理
            var clientsNotifies = _server.GetAllRemoteServiceProxy<IClientNotify>();
            foreach (var clientNotify in clientsNotifies)
            {
                Task.Run(() => clientNotify.Alert("今晚19点有台风，请大家注意防范"));
            }

            //获取第一个客户端的服务代理
            //try
            //{
            //    IClientNotify clientNotify = _server.GetFirstClientServiceProxy<IClientNotify>();
            //    clientNotify.Alert("今晚19点有台风，请大家注意防范");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void ButtonSendNormalMsg_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxInput.Text))
                _server.BroadcastText(tbxInput.Text);
        }
    }
}
