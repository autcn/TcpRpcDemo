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

namespace RpcClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window, IClientNotify
    {
        private TcpRpcClient _rpcClient;
        public MainWindow()
        {
            InitializeComponent();
            _rpcClient = new TcpRpcClient(true);
            _rpcClient.RegisterRemoteServiceProxy<IWeatherService>();
            _rpcClient.RegisterRemoteServiceProxy<ITicketService>();
            _rpcClient.AddLocalService<IClientNotify>(this);
            _rpcClient.MessageReceived += _rpcClient_MessageReceived;
        }

        private void _rpcClient_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            byte[] data = e.MessageRawData.ToArray();
            string strMsg = Encoding.UTF8.GetString(data);
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                tbxLog.AppendText(strMsg + "\r\n");
            }));
        }

        private void ButtonGetWeather_Click(object sender, RoutedEventArgs e)
        {
            var weatherService = _rpcClient.GetRemoteServiceProxy<IWeatherService>();
            var weather = weatherService.GetWeather(DateTime.Now);
            MessageBox.Show($"今天的天气：温度 {weather.Temperature}度  {weather.Description}");
        }

        private void ButtonConnectServer_Click(object sender, RoutedEventArgs e)
        {
            _rpcClient.Connect("127.0.0.1", 5000);
        }

        private void ButtonSearchTicket_Click(object sender, RoutedEventArgs e)
        {
            var ticketService = _rpcClient.GetRemoteServiceProxy<ITicketService>();
            try
            {
                bool hasTicket = ticketService.HasTicket("K200");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Alert(string message)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                tbxLog.AppendText(message + "\r\n");
            }));
        }

        private void ButtonSendNormalMsg_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxInput.Text))
                _rpcClient.SendText(tbxInput.Text);
        }
    }
}
