using System.Collections.Generic;

namespace DesktopWidgets.Classes
{
    public abstract class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public abstract class Weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public abstract class Main
    {
        public double temp { get; set; }
        public double pressure { get; set; }
        public int humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double sea_level { get; set; }
        public double grnd_level { get; set; }
    }

    public abstract class Wind
    {
        public double speed { get; set; }
        public double deg { get; set; }
    }

    public abstract class Rain
    {
        public double __invalid_name__3h { get; set; }
    }

    public abstract class Clouds
    {
        public int all { get; set; }
    }

    public abstract class Sys
    {
        public double message { get; set; }
        public string country { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
    }

    public abstract class OpenWeatherMapApiResult
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public Rain rain { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }
}