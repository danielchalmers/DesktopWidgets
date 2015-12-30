using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DesktopWidgets.Classes
{
    public class WidgetSettingsBase
    {
        [DisplayName("Disable")]
        public bool Disabled { get; set; } = false;

        [DisplayName("Internal Identifier")]
        public WidgetId Identifier { get; } = new WidgetId();

        [Category("Style")]
        [DisplayName("Outer Padding")]
        public Thickness Padding { get; set; } = new Thickness(3,2,3,2);

        [Category("General")]
        [DisplayName("Name")]
        public string Name { get; set; } = "Untitled";

        [Category("Style")]
        [DisplayName("Font Family")]
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");

        [Category("Style")]
        [DisplayName("Text Color")]
        public Color TextColor { get; set; } = Colors.Black;

        [Category("Style (Background)")]
        [DisplayName("Color")]
        public Color BackgroundColor { get; set; } = Colors.White;

        [Category("Style (Border)")]
        [DisplayName("Border Color")]
        public Color BorderColor { get; set; } = Colors.Gray;

        [Category("Style (Background)")]
        [DisplayName("Opacity")]
        public double BackgroundOpacity { get; set; } = 0.95;

        [Category("Style (Border)")]
        [DisplayName("Border Opacity")]
        public double BorderOpacity { get; set; } = 0.5;

        [Category("Style (Size)")]
        [DisplayName("Width (px)")]
        public double Width { get; set; } = double.NaN;

        [Category("Style (Size)")]
        [DisplayName("Height (px)")]
        public double Height { get; set; } = double.NaN;

        [Category("Style (Size)")]
        [DisplayName("Minimum Width (px)")]
        public double MinWidth { get; set; } = double.NaN;

        [Category("Style (Size)")]
        [DisplayName("Minimum Height (px)")]
        public double MinHeight { get; set; } = double.NaN;

        [Category("Style (Size)")]
        [DisplayName("Maximum Width (px)")]
        public double MaxWidth { get; set; } = double.NaN;

        [Category("Style (Size)")]
        [DisplayName("Maximum Height (px)")]
        public double MaxHeight { get; set; } = double.NaN;

        [DisplayName("Left Position (px)")]
        public double Left { get; set; } = double.NaN;

        [DisplayName("Top Position (px)")]
        public double Top { get; set; } = double.NaN;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Delay (ms)")]
        public int ShowDelay { get; set; } = 0;

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide Delay (ms)")]
        public int HideDelay { get; set; } = 0;

        [Category("Behavior (Hideable)")]
        [DisplayName("Animation Duration (ms)")]
        public int AnimationTime { get; set; } = 250;

        [Category("Dock")]
        [DisplayName("Screen")]
        public int Monitor { get; set; } = -1;

        [Category("Style")]
        [DisplayName("Font Size")]
        public int FontSize { get; set; } = 14;

        [Category("Style")]
        [DisplayName("Stay On Top")]
        public bool OnTop { get; set; } = true;

        [Category("Style")]
        [DisplayName("Force On Top")]
        public bool ForceOnTop { get; set; } = false;

        [Category("Style (Border)")]
        [DisplayName("Show")]
        public bool BorderEnabled { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Snap To Edges")]
        public bool SnapToScreenEdges { get; set; } = true;

        [Category("Style")]
        [DisplayName("Ease Animation")]
        public bool AnimationEase { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open Mode")]
        public OpenMode OpenMode { get; set; } = OpenMode.AlwaysOpen;

        [Category("Dock")]
        [DisplayName("Position")]
        public ScreenDockPosition DockPosition { get; set; } = ScreenDockPosition.None;

        [Category("Dock")]
        [DisplayName("Alignment")]
        public ScreenDockAlignment DockAlignment { get; set; } = ScreenDockAlignment.Center;

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Detection Distance (px)")]
        public int MouseBounds { get; set; } = 8;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stretch Mouse Bounds")]
        public bool StretchBounds { get; set; } = false;

        [Category("Style")]
        [DisplayName("Animation Type")]
        public AnimationType AnimationType { get; set; } = AnimationType.Fade;

        [Category("Style")]
        [DisplayName("Ignore Corners")]
        public bool IgnoreCorners { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Open Hotkey")]
        public Key HotKey { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Open Hotkey Modifiers")]
        public ModifierKeys HotKeyModifiers { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Activate On Show")]
        public bool ActivateOnShow { get; set; } = true;

        [Category("General")]
        [DisplayName("Force On Top Refresh Interval (ms)")]
        public int ForceOnTopInterval { get; set; } = 500;

        [Category("Style")]
        [DisplayName("Corner Size (px)")]
        public int CornerSize { get; set; } = 16;

        [Category("Style")]
        [DisplayName("Intro Duration (ms)")]
        public int IntroDuration { get; set; } = 3000;

        [Category("Style")]
        [DisplayName("Unclickable")]
        public bool Unclickable { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Drag Widget To Move")]
        public bool DragWidgetToMove { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Drag Titlebar To Move")]
        public bool DragTitlebarToMove { get; set; } = true;

        [Category("Style (Background)")]
        [DisplayName("Image Path")]
        public string BackgroundImagePath { get; set; }

        [Category("Style (Background)")]
        [DisplayName("Image Opacity")]
        public double BackgroundImageOpacity { get; set; } = 1.0;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Intro")]
        public bool ShowIntro { get; set; } = true;

        [Category("Style")]
        [DisplayName("Show Context Menu")]
        public bool ShowContextMenu { get; set; } = true;

        [Category("Style")]
        [DisplayName("Scrollbar Visibility")]
        public ScrollBarVisibility ScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;

        [Category("Behavior (Hideable)")]
        [DisplayName("Custom Mouse Detection Bounds")]
        public Rect CustomMouseDetectionBounds { get; set; } = new Rect();

        [Category("Dock")]
        [DisplayName("Offset")]
        public Point DockOffset { get; set; } = new Point();

        [Category("Behavior (Hideable)")]
        [DisplayName("Ignore 0,0 Cursor Position")]
        public bool Ignore00XY { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Sound Path")]
        public string ShowSoundPath { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Sound Volume")]
        public double ShowSoundVolume { get; set; } = 1.0;

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide Sound Path")]
        public string HideSoundPath { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide Sound Volume")]
        public double HideSoundVolume { get; set; } = 1.0;

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Bounds Use Dock Offset")]
        public bool MouseBoundsUseDockOffset { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open With Mouse Focus")]
        public bool StayOpenIfMouseFocus { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show If Foreground Fullscreen")]
        public bool FullscreenActivation { get; set; } = false;

        [Category("Style (Titlebar)")]
        [DisplayName("Show Titlebar")]
        public bool ShowTitlebar { get; set; } = true;

        [Category("Style (Titlebar)")]
        [DisplayName("Show Menu Button")]
        public bool ShowTitlebarMenuButton { get; set; } = true;

        [Category("Style (Titlebar)")]
        [DisplayName("Background Color")]
        public Color TitlebarBackgroundColor { get; set; } = Color.FromRgb(32, 32, 32);

        [Category("Style (Titlebar)")]
        [DisplayName("Background Opacity")]
        public double TitlebarBackgroundOpacity { get; set; } = 0.5;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Show Titlebar Name")]
        public bool ShowTitlebarName { get; set; } = true;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Font Family")]
        public FontFamily TitlebarNameFontFamily { get; set; } = new FontFamily("Segoe UI");

        [Category("Style (Titlebar Name)")]
        [DisplayName("Font Weight")]
        public FontWeight TitlebarNameFontWeight { get; set; } = FontWeights.Normal;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Font Size")]
        public int TitlebarNameFontSize { get; set; } = 12;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Color")]
        public Color TitlebarNameTextColor { get; set; } = Colors.WhiteSmoke;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Alignment")]
        public TextAlignment TitlebarNameAlignment { get; set; } = TextAlignment.Center;

        [Category("Style (Titlebar)")]
        [DisplayName("Rounded Corners")]
        public bool TitlebarRoundedCorners { get; set; } = true;

        [Category("Style")]
        [DisplayName("Rounded Corners")]
        public bool RoundedCorners { get; set; } = true;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Allow Editing")]
        public bool TitlebarNameAllowEditing { get; set; } = true;
    }
}