using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.Widgets.LatencyMonitor
{
    public class ViewModel : WidgetViewModelBase
    {
        private bool _scanLatency;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            LatencyHistory = new ObservableCollection<string>();
            _scanLatency = true;
            StartLatencyReader();
        }

        public Settings Settings { get; }

        public ObservableCollection<string> LatencyHistory { get; }

        private void StartLatencyReader()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (_scanLatency)
                {
                    var latencyText = GetLatencyText();
                    Application.Current.Dispatcher.BeginInvoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            LatencyHistory.Add(latencyText);
                            while (LatencyHistory.Count >= Settings.MaxHistory)
                                LatencyHistory.RemoveAt(0);
                        }));
                    Thread.Sleep(Settings.PingInterval);
                }
            }).Start();
        }

        private string GetLatencyText()
        {
            if (string.IsNullOrWhiteSpace(Settings.HostAddress))
                return string.Empty;
            var latencyText = string.Empty;
            var ping = new Ping();
            var reply = ping.Send(Settings.HostAddress, Settings.Timeout);
            if (reply != null)
            {
                var stringBuilder = new StringBuilder();
                if (Settings.ShowTime)
                    stringBuilder.Append($"{DateTime.Now.ToString(Settings.DateTimeFormat)}: ");
                stringBuilder.Append($"{reply.RoundtripTime.ToString().PadLeft(Settings.LatencyPadding, '0')}ms");
                if (Settings.ShowStatus)
                    stringBuilder.Append($" ({reply.Status})");
                latencyText = stringBuilder.ToString();
            }
            return latencyText;
        }

        public override void OnClose()
        {
            base.OnClose();
            _scanLatency = false;
        }
    }
}