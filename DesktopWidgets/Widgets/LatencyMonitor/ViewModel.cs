using System;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using DesktopWidgets.Helpers;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;

namespace DesktopWidgets.Widgets.LatencyMonitor
{
    public class ViewModel : WidgetViewModelBase
    {
        private long _lastLatency = -1;
        private bool _scanLatency;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            LatencyHistory = new ObservableCollection<TextBlock>();
            _scanLatency = true;
            StartLatencyReader();
        }

        public Settings Settings { get; }

        public ObservableCollection<TextBlock> LatencyHistory { get; }

        private void StartLatencyReader()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (_scanLatency)
                {
                    var reply = GetLatency();
                    if (reply == null)
                        continue;
                    Application.Current.Dispatcher.Invoke(
                        DispatcherPriority.Background,
                        new Action(() =>
                        {
                            try
                            {
                                var latencyTextBlock = new TextBlock
                                {
                                    Text = GetLatencyText(reply),
                                    Foreground = new SolidColorBrush(GetLatencyBrush(reply))
                                };
                                while (LatencyHistory.Count + 1 > Settings.MaxHistory)
                                    LatencyHistory.RemoveAt(0);
                                LatencyHistory.Add(latencyTextBlock);
                            }
                            catch
                            {
                                // ignored
                            }
                        }));
                    _lastLatency = reply.RoundtripTime;
                    Thread.Sleep(Settings.PingInterval);
                }
            }).Start();
        }

        private PingReply GetLatency()
        {
            if (string.IsNullOrWhiteSpace(Settings.HostAddress))
                return null;
            var ping = new Ping();
            var reply = ping.Send(Settings.HostAddress, Settings.Timeout);
            return reply;
        }

        private string GetLatencyText(PingReply reply)
        {
            if (reply == null)
                return null;
            var stringBuilder = new StringBuilder();
            if (Settings.ShowTime)
                stringBuilder.Append($"{DateTime.Now.ToString(Settings.DateTimeFormat)}: ");
            stringBuilder.Append($"{reply.RoundtripTime.ToString().PadLeft(Settings.LatencyPadding, '0')}ms");
            if (Settings.ShowStatus)
                stringBuilder.Append($" ({reply.Status})");
            return stringBuilder.ToString();
        }

        private Color GetLatencyBrush(PingReply reply)
        {
            if (!Settings.ColorCoding)
                return Settings.LatencyDefaultColor;
            return reply == null || reply.Status != IPStatus.Success || (reply.RoundtripTime > Settings.LatencyGoodMax) ||
                   (_lastLatency > 0 && Math.Abs(reply.RoundtripTime - _lastLatency) > Settings.LatencyGoodSinceLast)
                ? Settings.LatencyBadColor
                : Settings.LatencyGoodColor;
        }

        public override void OnClose()
        {
            base.OnClose();
            _scanLatency = false;
        }
    }
}