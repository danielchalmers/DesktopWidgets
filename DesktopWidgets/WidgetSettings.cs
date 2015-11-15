﻿using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace DesktopWidgets
{
    public class WidgetConfig
    {
        public int Version { get; set; } = 1;
        public ObservableCollection<WidgetSettings> Widgets { get; set; }
    }

    public class WidgetSettings
    {
        public Thickness Padding { get; set; } = new Thickness(2);
        //public Point ScreenDpi = new Point(96, 96);
        public string Name { get; set; } = "";
        public bool Disabled { get; set; } = false;
        public Guid Guid { get; set; } = Guid.NewGuid();
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");
        public Color TextColor { get; set; } = Colors.Black;
        public Color BackgroundColor { get; set; } = Colors.White;
        public Color BorderColor { get; set; } = Colors.DimGray;
        public double BackgroundOpacity { get; set; } = 1;
        public double BorderOpacity { get; set; } = 1;
        public double Width { get; set; } = double.NaN;
        public double Height { get; set; } = double.NaN;
        //public int ShowDelay { get; set; } = 0;
        //public int HideDelay { get; set; } = 0;
        //public int AnimationTime { get; set; } = 150;
        //public int Monitor { get; set; } = -1;
        public int FontSize { get; set; } = 16;
        public bool OnTop { get; set; } = true;
        public bool ForceOnTop { get; set; } = true;
        public bool BorderEnabled { get; set; } = true;
        public bool SnapToScreenEdges { get; set; } = true;
        //public bool AnimationEase { get; set; } = true;

        public override string ToString()
        {
            return WidgetHelper.GetWidgetName(Guid);
        }
    }

    public class WidgetClockSettings : WidgetSettings
    {
        public TimeSpan TickInterval { get; set; } = TimeSpan.FromMilliseconds(100);
        public string TimeFormat { get; set; }
    }

    public class WidgetTimeClockSettings : WidgetClockSettings
    {
    }

    public class WidgetCountdownClockSettings : WidgetClockSettings
    {
        public WidgetCountdownClockSettings()
        {
            TimeFormat = "hh:mm:ss";
        }

        public DateTime EndDateTime { get; set; } = DateTime.Now;
    }
}