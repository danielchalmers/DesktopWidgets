using System;
using System.Net;
using System.Windows.Threading;
using DesktopWidgets.ApiClasses;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.WidgetBase;
using DesktopWidgets.WidgetBase.ViewModel;
using Newtonsoft.Json;

namespace DesktopWidgets.Widgets.Weather
{
    public class ViewModel : WidgetViewModelBase
    {
        private string _description;
        private string _iconUrl;
        private string _lastZipCode;
        private double _temperature;
        private double _temperatureMax;
        private double _temperatureMin;
        private DispatcherTimer _updateTimer;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
            {
                return;
            }
            _updateTimer = new DispatcherTimer();
            UpdateTimerInterval();
            _updateTimer.Tick += (sender, args) => UpdateWeather();

            UpdateWeather();
            _updateTimer.Start();
        }

        public Settings Settings { get; }

        public double Temperature
        {
            get { return _temperature; }
            set
            {
                if (value.IsEqual(_temperature))
                {
                    _temperature = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double TemperatureMin
        {
            get { return _temperatureMin; }
            set
            {
                if (value.IsEqual(_temperatureMin))
                {
                    _temperatureMin = value;
                    RaisePropertyChanged();
                }
            }
        }

        public double TemperatureMax
        {
            get { return _temperatureMax; }
            set
            {
                if (value.IsEqual(_temperatureMax))
                {
                    _temperatureMax = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string IconUrl
        {
            get { return _iconUrl; }
            set
            {
                if (_iconUrl != value)
                {
                    _iconUrl = value;
                    RaisePropertyChanged();
                }
            }
        }

        private void DownloadWeatherData(Action<OpenWeatherMapApiResult> finishAction)
        {
            string unitType;
            switch (Settings.UnitType)
            {
                default:
                case TemperatureUnitType.Celsius:
                    unitType = "metric";
                    break;
                case TemperatureUnitType.Fahrenheit:
                    unitType = "imperial";
                    break;
                case TemperatureUnitType.Kelvin:
                    unitType = "kelvin";
                    break;
            }

            try
            {
                using (var wc = new WebClient())
                {
                    wc.DownloadStringCompleted +=
                        (sender, args) =>
                            finishAction(JsonConvert.DeserializeObject<OpenWeatherMapApiResult>(args.Result));
                    wc.DownloadStringAsync(
                        new Uri(
                            $"{Resources.OpenWeatherMapDomain}data/2.5/weather?zip={Settings.ZipCode}&units={unitType}&appid={Resources.OpenWeatherMapAPIKey}"));
                }
            }
            catch
            {
                // ignored
            }
        }

        private void UpdateWeather()
        {
            _lastZipCode = Settings.ZipCode;

            if (string.IsNullOrEmpty(Settings.ZipCode))
            {
                return;
            }

            DownloadWeatherData(data =>
            {
                if (data?.main?.temp != null && data.weather.Count > 0)
                {
                    Temperature = data.main.temp;
                    Description = data.weather[0].description;
                    TemperatureMin = data.main.temp_min;
                    TemperatureMax = data.main.temp_max;
                    IconUrl = $"{Resources.OpenWeatherMapDomain}img/w/{data.weather[0].icon}.png";
                }
            });
        }

        private void UpdateTimerInterval()
        {
            _updateTimer.Interval = Settings.RefreshInterval.TotalMinutes < 30
                ? TimeSpan.FromMinutes(30)
                : Settings.RefreshInterval;
        }

        public override void OnClose()
        {
            base.OnClose();
            _updateTimer?.Stop();
            _updateTimer = null;
        }

        public override void OnRefresh()
        {
            base.OnRefresh();
            UpdateTimerInterval();
            if (_lastZipCode != Settings.ZipCode)
            {
                UpdateWeather();
            }
        }
    }
}