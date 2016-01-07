namespace DesktopWidgets
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
        MouseAndKeyboard,
        Hidden
    }

    public enum AnimationMode
    {
        Show,
        Hide
    }

    public enum AnimationType
    {
        Fade,
        Slide,
        None
    }

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
    }

    public enum ImageScalingMode
    {
        HighQuality,
        LowQuality,
        NearestNeighbor
    }

    public enum ShortcutContentMode
    {
        Icon,
        Text,
        Both
    }

    public enum ScrollBarVisibility
    {
        Auto,
        Visible,
        Hidden
    }

    public enum ShortcutOrientation
    {
        Vertical,
        Horizontal
    }

    public enum UpdateBranch
    {
        Stable,
        Beta
    }

    public enum DefaultShortcutsMode
    {
        DontChange,
        Preset,
        Taskbar
    }

    public enum MouseBoundsDetectionAxis
    {
        Both,
        Vertical,
        Horizontal
    }
}