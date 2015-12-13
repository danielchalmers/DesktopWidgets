using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using DesktopWidgets.Helpers;

namespace DesktopWidgets.Classes
{
    public class WidgetSettingsBase
    {
        public bool Disabled { get; set; } = false;
        public WidgetId Identifier { get; } = new WidgetId();

        [Category("Style")]
        [DisplayName("Outer Padding")]
        public Thickness Padding { get; set; } = new Thickness(2);

        [DisplayName("Name")]
        public string Name { get; set; } = "";

        [Category("Name Style")]
        [DisplayName("Show Name")]
        public bool ShowName { get; set; } = true;

        [Category("Style")]
        [DisplayName("Font Family")]
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");

        [Category("Style")]
        [DisplayName("Text Color")]
        public Color TextColor { get; set; } = Colors.Black;

        [Category("Style")]
        [DisplayName("Background Color")]
        public Color BackgroundColor { get; set; } = Colors.White;

        [Category("Style")]
        [DisplayName("Border Color")]
        public Color BorderColor { get; set; } = Colors.DimGray;

        [Category("Style")]
        [DisplayName("Background Opacity")]
        public double BackgroundOpacity { get; set; } = 1;

        [Category("Style")]
        [DisplayName("Border Opacity")]
        public double BorderOpacity { get; set; } = 1;

        [Category("Size")]
        [DisplayName("Width")]
        public double Width { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Height")]
        public double Height { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Minimum Width")]
        public double MinWidth { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Minimum Height")]
        public double MinHeight { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Maximum Width")]
        public double MaxWidth { get; set; } = double.NaN;

        [Category("Size")]
        [DisplayName("Maximum Height")]
        public double MaxHeight { get; set; } = double.NaN;

        [DisplayName("Left Position")]
        public double Left { get; set; } = double.NaN;

        [DisplayName("Top Position")]
        public double Top { get; set; } = double.NaN;

        [Category("Behavior")]
        [DisplayName("Show Delay")]
        public int ShowDelay { get; set; } = 0;

        [Category("Behavior")]
        [DisplayName("Hide Delay")]
        public int HideDelay { get; set; } = 0;

        [Category("Behavior")]
        [DisplayName("Animation Duration")]
        public int AnimationTime { get; set; } = 150;

        [Category("Dock")]
        [DisplayName("Dock Monitor")]
        public int Monitor { get; set; } = -1;

        [Category("Style")]
        [DisplayName("Font Size")]
        public int FontSize { get; set; } = 16;

        [Category("Style")]
        [DisplayName("Stay On Top")]
        public bool OnTop { get; set; } = true;

        [Category("Style")]
        [DisplayName("Force On Top")]
        public bool ForceOnTop { get; set; } = false;

        [Category("Style")]
        [DisplayName("Enable Border")]
        public bool BorderEnabled { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Snap to Edges")]
        public bool SnapToScreenEdges { get; set; } = true;

        [Category("Style")]
        [DisplayName("Ease Animation")]
        public bool AnimationEase { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Open Mode")]
        public OpenMode OpenMode { get; set; } = OpenMode.AlwaysOpen;

        [Category("Dock")]
        [DisplayName("Dock Position")]
        public ScreenDockPosition DockPosition { get; set; } = ScreenDockPosition.None;

        [Category("Dock")]
        [DisplayName("Dock Alignment")]
        public ScreenDockAlignment DockAlignment { get; set; } = ScreenDockAlignment.Center;

        [Category("Behavior")]
        [DisplayName("Mouse Detection Distance")]
        public int MouseBounds { get; set; } = 8;

        [Category("Behavior")]
        [DisplayName("Stretch Mouse Bounds")]
        public bool StretchBounds { get; set; } = false;

        [Category("Style")]
        [DisplayName("Animation Type")]
        public AnimationType AnimationType { get; set; } = AnimationType.Fade;

        [Category("Style")]
        [DisplayName("Ignore Corners")]
        public bool IgnoreCorners { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Hotkey")]
        public Key HotKey { get; set; } = Key.CapsLock;

        [Category("Behavior")]
        [DisplayName("Hotkey Modifiers")]
        public ModifierKeys HotKeyModifiers { get; set; }

        [Category("Behavior")]
        [DisplayName("Activate on Show")]
        public bool ActivateOnShow { get; set; } = true;

        [Category("Style")]
        [DisplayName("Force On Top Refresh Interval")]
        public int ForceOnTopInterval { get; set; } = 500;

        [Category("Style")]
        [DisplayName("Corner Size")]
        public int CornerSize { get; set; } = 16;

        [Category("Style")]
        [DisplayName("Intro Duration")]
        public int IntroDuration { get; set; } = 3000;

        [Category("Style")]
        [DisplayName("Unclickable")]
        public bool Unclickable { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Drag to Move")]
        public bool DragToMove { get; set; } = true;

        [Category("Style")]
        [DisplayName("Background Image Path")]
        public string BackgroundImagePath { get; set; }

        [Category("Style")]
        [DisplayName("Background Image Opacity")]
        public double BackgroundImageOpacity { get; set; } = 1.0;

        [Category("Name Style")]
        [DisplayName("Name Font Family")]
        public FontFamily NameFontFamily { get; set; } = new FontFamily("Segoe UI");

        [Category("Name Style")]
        [DisplayName("Name Font Weight")]
        public FontWeight NameFontWeight { get; set; } = FontWeights.Bold;

        [Category("Name Style")]
        [DisplayName("Name Font Size")]
        public int NameFontSize { get; set; } = 14;

        [Category("Name Style")]
        [DisplayName("Name Text Color")]
        public Color NameTextColor { get; set; } = Colors.Black;

        [Category("Name Style")]
        [DisplayName("Name Background Color")]
        public Color NameBackgroundColor { get; set; } = Colors.White;

        [Category("Name Style")]
        [DisplayName("Name Background Opacity")]
        public double NameBackgroundOpacity { get; set; } = 1;

        [Category("Name Style")]
        [DisplayName("Name Alignment")]
        public TextAlignment NameAlignment { get; set; } = TextAlignment.Center;

        [Category("Behavior")]
        [DisplayName("Show Intro")]
        public bool ShowIntro { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Enable Move Hotkeys")]
        public bool MoveHotkeys { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Move Hotkeys Distance")]
        public int MoveDistance { get; set; } = 5;

        [Category("Style")]
        [DisplayName("Show Context Menu")]
        public bool ShowContextMenu { get; set; } = true;

        [Category("Size")]
        [DisplayName("Automatically Decide Max Size")]
        public bool AutoMaxSize { get; set; } = true;

        [Category("Style")]
        [DisplayName("Scrollbar Visibility")]
        public ScrollBarVisibility ScrollBarVisibility { get; set; } = ScrollBarVisibility.Auto;

        public override string ToString()
        {
            return Identifier.GetName();
        }
    }
}