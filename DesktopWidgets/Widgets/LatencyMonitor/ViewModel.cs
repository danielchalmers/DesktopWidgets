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
                            if (reply != null)
                            {
                                var downloadedBytes = GetDownloadedBytes();
                                var uploadedBytes = GetUploadedBytes();
                                var downloadDifference = downloadedBytes - _lastDownloadUsage;
                                var uploadDifference = uploadedBytes - _lastUploadUsage;
                                _lastLatency = reply.RoundtripTime;
                                _lastDownloadUsage = GetDownloadedBytes();
                                _lastUploadUsage = GetUploadedBytes();
                                Application.Current.Dispatcher.BeginInvoke(
                                    DispatcherPriority.Background,
                                    new Action(() =>
                                    {
                                        try
                                        {
                                            var latencyTextBlock = new TextBlock
                                            {
                                                Text =
                                                    GetLatencyText(reply, downloadDifference, uploadDifference),
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
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                        Thread.Sleep(Settings.PingInterval);
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
                    $" {BytesToReadableString(downloadUsage, Settings.BandwidthDecimalPlaces).PadLeft(Settings.BandwidthPadding, ' ')}");
            }
            if (Settings.ShowUploadUsage)
            {
                stringBuilder.Append(
                    $" {BytesToReadableString(uploadUsage, Settings.BandwidthDecimalPlaces).PadLeft(Settings.BandwidthPadding, ' ')}");
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

        // https://stackoverflow.com/a/11124118
        private static string BytesToReadableString(long bytes, int decimalPlaces)
        {
            if (bytes <= 0)
            {
                return "0B";
            }

            string suffix;
            double readable;
            if (bytes >= 0x1000000000000000) // Exabyte.
            {
                suffix = "EB";
                readable = (bytes >> 50);
            }
            else if (bytes >= 0x4000000000000) // Petabyte.
            {
                suffix = "PB";
                readable = (bytes >> 40);
            }
            else if (bytes >= 0x10000000000) // Terabyte.
            {
                suffix = "TB";
                readable = (bytes >> 30);
            }
            else if (bytes >= 0x40000000) // Gigabyte.
            {
                suffix = "GB";
                readable = (bytes >> 20);
            }
            else if (bytes >= 0x100000) // Megabyte.
            {
                suffix = "MB";
                readable = (bytes >> 10);
            }
            else if (bytes >= 0x400) // Kilobyte.
            {
                suffix = "KB";
                readable = bytes;
            }
            else
            {
                return bytes.ToString("0B"); // Byte.
            }

            // Divide by 1024 to get fractional value.
            readable = Math.Round(readable / 1024, decimalPlaces);

            return readable.ToString("0") + suffix;
        }
    }
}