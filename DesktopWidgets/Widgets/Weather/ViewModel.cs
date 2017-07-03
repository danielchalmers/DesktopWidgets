using System;
using System.Net;
using System.Threading.Tasks;
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
        private string _lastApiKey;
        private double _temperature;
        private double _temperatureMax;
        private double _temperatureMin;
        private bool _didWeatherDataFail;
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
            _updateTimer.Tick += async (sender, args) => await UpdateWeatherAsync();
            _updateTimer.Start();

            Task.Run(async () => await UpdateWeatherAsync());
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

        public bool DidWeatherDataFail
        {
            get { return _didWeatherDataFail; }
            set
            {
                if (_didWeatherDataFail != value)
                {
                    _didWeatherDataFail = value;
                    RaisePropertyChanged();
                }
            }
        }

        private async Task<OpenWeatherMapApiResult> GetWeatherDataAsync()
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
                using (var webClient = new WebClient())
                {
                    var result = await webClient.DownloadStringTaskAsync(
                        new Uri($"{Resources.OpenWeatherMapDomain}data/2.5/weather?zip={Settings.ZipCode}&units={unitType}&appid={Settings.ApiKey}"));
                    return JsonConvert.DeserializeObject<OpenWeatherMapApiResult>(result);
                }
            }
            catch
            {
                // ignored
            }
            return null;
        }

        private async Task UpdateWeatherAsync()
        {
            _lastZipCode = Settings.ZipCode;
            _lastApiKey = Settings.ApiKey;

            if (string.IsNullOrEmpty(Settings.ZipCode) ||
                string.IsNullOrEmpty(Settings.ApiKey))
            {
                return;
            }

            var weatherData = await GetWeatherDataAsync();
            DidWeatherDataFail = (weatherData?.main?.temp == null || weatherData.weather.Count == 0);
            if (!DidWeatherDataFail)
            {
                Temperature = weatherData.main.temp;
                Description = weatherData.weather[0].description;
                TemperatureMin = weatherData.main.temp_min;
                TemperatureMax = weatherData.main.temp_max;
                IconUrl = $"{Resources.OpenWeatherMapDomain}img/w/{weatherData.weather[0].icon}.png";
            }
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
            if (_lastZipCode != Settings.ZipCode ||
                _lastApiKey != Settings.ApiKey)
            {
                Task.Run(async () => await UpdateWeatherAsync());
            }
        }
    }
}