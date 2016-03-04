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
        Both,
        Name,
        Path,
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
        Both,
        Text,
        Icon
    }

    public enum ScrollBarVisibility
    {
        Auto,
        Visible,
        Hidden
    }

    public enum ShortcutOrientation
    {
        Horizontal,
        Vertical
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
        Horizontal,
        Vertical
    }

    public enum DirectoryChange
    {
        NewFile,
        FileChanged
    }

    public enum FileType
    {
        None,
        Text,
        Image,
        Audio,
        Other
    }

    public enum TitlebarVisibilityMode
    {
        AlwaysVisible,
        OnHover,
        Hidden
    }

    public enum YesNoAny
    {
        Any,
        Yes,
        No
    }

    public enum StringMatchMode
    {
        Any,
        Equals,
        Contains
    }

    public enum FileWriteMode
    {
        Overwrite,
        Append
    }

    public enum InsertMode
    {
        None,
        Prefix,
        Suffix
    }

    public enum InputMode
    {
        Clipboard,
        File,
        Text
    }

    public enum ResumeOnStartMode
    {
        Auto,
        Resume,
        DontChange
    }

    public enum MuteMode
    {
        Toggle,
        Mute,
        Unmute
    }

    public enum MuteEventMode
    {
        Mute,
        Unmute,
        Both
    }
}