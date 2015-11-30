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

        private double _temperature;
        public DispatcherTimer UpdateTimer;

        public ViewModel(WidgetId id) : base(id)
        {
            Settings = id.GetSettings() as Settings;
            if (Settings == null)
                return;
            UpdateTimer = new DispatcherTimer();
            UpdateTimer.Interval = Settings.RefreshInterval;
            UpdateTimer.Tick += UpdateWeather;
            UpdateTimer.Start();
            UpdateWeather();
        }

        public Settings Settings { get; }


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
            IconUrl = $"{Resources.OpenWeatherMapDomain}img/w/{data.weather[0].icon}.png";
        }
    }
}