namespace DesktopWidgets.Classes
{
    public enum TemperatureUnitType
    {
        Celsius,
        Fahrenheit,
        Kelvin
    }

    public enum OpenMode
    {
        AlwaysOpen,
        Mouse,
        Keyboard,
        MouseAndKeyboard
    }

    public enum ScreenDockPosition
    {
        None,
        Left,
        Right,
        Top,
        Bottom
    }

    public enum ScreenDockAlignment
    {
        Center,
        Top,
        Bottom,
        Stretch
    }

    public enum AnimationMode
    {
        Show,
        Hide
    };

    public enum AnimationType
    {
        Fade,
        Slide,
        None
    };

    public enum IconPosition
    {
        Left,
        Right
    }

    public enum ToolTipType
    {
        Name,
        Path,
        Both,
        None
    };

    public enum ShortcutAlignment
    {
        Top,
        Center,
        Bottom
    };

    public enum ImageScalingMode
    {
        HighQuality,
        LowQuality,
        NearestNeighbor
    };

    public enum ShortcutContentMode
    {
        Icon,
        Text,
        Both
    };

    public enum ScrollBarVisibility
    {
        Auto,
        Visible,
        Hidden
    };

    public enum ShortcutOrientation
    {
        Auto,
        Vertical,
        Horizontal
    };
}