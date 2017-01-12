using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        private long _lastDownloadUsage;
        private long _lastLatency;
        private long _lastUploadUsage;
        private bool _scanLatency;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
            {
                return;
            }
            LatencyHistory = new ObservableCollection<TextBlock>();
            _lastLatency = -1;
            _lastDownloadUsage = GetDownloadedBytes();
            _lastUploadUsage = GetUploadedBytes();
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
                using (var ping = new Ping())
                {
                    while (_scanLatency)
                    {
                        try
                        {
                            var reply = GetLatency(ping);
                            if (reply == null)
                            {
                                continue;
                            }
                            var downloadedBytes = GetDownloadedBytes();
                            var uploadedBytes = GetUploadedBytes();
                            Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Background,
                                new Action(() =>
                                {
                                    try
                                    {
                                        var latencyTextBlock = new TextBlock
                                        {
                                            Text =
                                                GetLatencyText(reply, downloadedBytes - _lastDownloadUsage,
                                                    uploadedBytes - _lastUploadUsage),
                                            Foreground = new SolidColorBrush(GetLatencyBrush(reply))
                                        };
                                        while (LatencyHistory.Count + 1 > Settings.MaxHistory)
                                        {
                                            LatencyHistory.RemoveAt(0);
                                        }
                                        LatencyHistory.Add(latencyTextBlock);
                                    }
                                    catch
                                    {
                                        // ignored
                                    }
                                }));
                            _lastLatency = reply.RoundtripTime;
                            _lastDownloadUsage = GetDownloadedBytes();
                            _lastUploadUsage = GetUploadedBytes();
                            Thread.Sleep(Settings.PingInterval);
                        }
                        catch
                        {
                            // ignored
                        }
                    }
                }
            }).Start();
        }

        private PingReply GetLatency(Ping ping)
        {
            if (string.IsNullOrWhiteSpace(Settings.HostAddress))
            {
                return null;
            }
            try
            {
                var reply = ping.Send(Settings.HostAddress, Settings.Timeout);
                return reply;
            }
            catch (PingException)
            {
                return null;
            }
        }

        private long GetDownloadedBytes()
            => NetworkInterface.GetAllNetworkInterfaces().Select(x => x.GetIPStatistics().BytesReceived).Sum();

        private long GetUploadedBytes()
            => NetworkInterface.GetAllNetworkInterfaces().Select(x => x.GetIPStatistics().BytesSent).Sum();

        private string GetLatencyText(PingReply reply, long downloadUsage, long uploadUsage)
        {
            if (reply == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            if (Settings.ShowTime)
            {
                stringBuilder.Append($"{DateTime.Now.ToString(Settings.DateTimeFormat)}:");
            }
            if (Settings.ShowDownloadUsage)
            {
                stringBuilder.Append(
                    $" {StringHelper.BytesToString(downloadUsage, Settings.BandwidthDecimalPlaces).PadLeft(Settings.BandwidthPadding, ' ')}");
            }
            if (Settings.ShowUploadUsage)
            {
                stringBuilder.Append(
                    $" {StringHelper.BytesToString(uploadUsage, Settings.BandwidthDecimalPlaces).PadLeft(Settings.BandwidthPadding, ' ')}");
            }
            stringBuilder.Append($" {reply.RoundtripTime.ToString().PadLeft(Settings.LatencyPadding, '0')}ms");
            if (Settings.ShowStatus)
            {
                stringBuilder.Append($" ({reply.Status})");
            }
            return stringBuilder.ToString();
        }

        private Color GetLatencyBrush(PingReply reply)
        {
            if (!Settings.ColorCoding)
            {
                return Settings.LatencyDefaultColor;
            }
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