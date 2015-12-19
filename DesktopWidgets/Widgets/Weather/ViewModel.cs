using System;
using System.Net;
using System.Windows.Threading;
using DesktopWidgets.Classes;
using DesktopWidgets.Helpers;
using DesktopWidgets.Properties;
using DesktopWidgets.ViewModelBase;
using Newtonsoft.Json;

namespace DesktopWidgets.Widgets.Weather
{
    public class ViewModel : WidgetViewModelBase
    {
        private string _description;
        private string _iconUrl;

        private bool _showHelp;

        private double _temperature;

        private double _temperatureMax;

        private double _temperatureMin;
        public DispatcherTimer UpdateTimer;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            UpdateTimer = new DispatcherTimer();
            UpdateTimer.Interval = Settings.RefreshInterval.TotalMinutes < 30
                ? TimeSpan.FromMinutes(30)
                : Settings.RefreshInterval;
            UpdateTimer.Tick += UpdateWeather;
            UpdateTimer.Start();
            if (Settings.ZipCode == 0)
            {
                ShowHelp = true;
            }
            else
            {
                UpdateWeather();
            }
        }

        public Settings Settings { get; }

        public bool ShowHelp
        {
            get { return _showHelp; }
            set
            {
                if (_showHelp != value)
                {
                    _showHelp = value;
                    RaisePropertyChanged(nameof(ShowHelp));
                }
            }
        }


        public double Temperature
        {
            get { return _temperature; }
            set
            {
                if (_temperature != value)
                {
                    _temperature = value;
                    RaisePropertyChanged(nameof(Temperature));
                }
            }
        }

        public double TemperatureMin
        {
            get { return _temperatureMin; }
            set
            {
                if (_temperatureMin != value)
                {
                    _temperatureMin = value;
                    RaisePropertyChanged(nameof(TemperatureMin));
                }
            }
        }

        public double TemperatureMax
        {
            get { return _temperatureMax; }
            set
            {
                if (_temperatureMax != value)
                {
                    _temperatureMax = value;
                    RaisePropertyChanged(nameof(TemperatureMax));
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
                    RaisePropertyChanged(nameof(Description));
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
                    RaisePropertyChanged(nameof(IconUrl));
                }
            }
        }

        private void UpdateWeather(object sender = null, EventArgs eventArgs = null)
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
            string json;
            using (var wc = new WebClient())
                json =
                    wc.DownloadString(
                        $"{Resources.OpenWeatherMapDomain}data/2.5/weather?zip={Settings.ZipCode}&units={unitType}&appid={Resources.OpenWeatherMapAPIKey}");
            var data = JsonConvert.DeserializeObject<OpenWeatherMapApiResult>(json);

            if (data?.main?.temp == null)
                return;

            Temperature = data.main.temp;
            Description = data.weather[0].description;
            TemperatureMin = data.main.temp_min;
            TemperatureMax = data.main.temp_max;
            IconUrl = $"{Resources.OpenWeatherMapDomain}img/w/{data.weather[0].icon}.png";
        }
    }
}