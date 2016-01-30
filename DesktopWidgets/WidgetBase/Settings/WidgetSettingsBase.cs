using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DesktopWidgets.WidgetBase.Settings
{
    public class WidgetSettingsBase
    {
        [DisplayName("Disabled")]
        public bool Disabled { get; set; } = false;

        [DisplayName("Internal Identifier")]
        public WidgetId Identifier { get; } = new WidgetId();

        [DisplayName("Active Time End")]
        public DateTime ActiveTimeEnd { get; set; } = DateTime.Now;

        [Category("Style")]
        [DisplayName("Padding")]
        public Thickness Padding { get; set; } = new Thickness(3, 2, 3, 2);

        [Category("General")]
        [DisplayName("Name")]
        public string Name { get; set; } = "Untitled";

        [Category("Style (Text)")]
        [DisplayName("Font Family")]
        public FontFamily FontFamily { get; set; } = new FontFamily("Segoe UI");

        [Category("Style (Text)")]
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

        [Category("Style (Position)")]
        [DisplayName("Left Position (px)")]
        public double Left { get; set; } = double.NaN;

        [Category("Style (Position)")]
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
        [DisplayName("Screen Bounds")]
        public Rect ScreenBounds { get; set; }

        [Category("Style (Text)")]
        [DisplayName("Font Size")]
        public int FontSize { get; set; } = 14;

        [Category("Behavior")]
        [DisplayName("Stay On Top")]
        public bool OnTop { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Force On Top")]
        public bool ForceOnTop { get; set; } = false;

        [Category("Style (Border)")]
        [DisplayName("Visible")]
        public bool BorderEnabled { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Snap To Edges")]
        public bool SnapToScreenEdges { get; set; } = true;

        [Category("Style")]
        [DisplayName("Ease Animation")]
        public bool AnimationEase { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Mode")]
        public OpenMode OpenMode { get; set; } = OpenMode.AlwaysOpen;

        [Category("Dock")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        [Category("Dock")]
        [DisplayName("Vertical Alignment")]
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;

        [Category("Dock")]
        [DisplayName("Docked")]
        public bool IsDocked { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Detection Distance (px)")]
        public int MouseBounds { get; set; } = 8;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stretch Mouse Bounds")]
        public bool StretchBounds { get; set; } = false;

        [Category("Style")]
        [DisplayName("Animation Type")]
        public AnimationType AnimationType { get; set; } = AnimationType.Fade;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Hotkey")]
        public Key HotKey { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Hotkey Modifiers")]
        public ModifierKeys HotKeyModifiers { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Activate On Show")]
        public bool ActivateOnShow { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Force On Top Refresh Interval (ms)")]
        public int ForceOnTopInterval { get; set; } = 500;

        [Category("Style")]
        [DisplayName("Intro Duration (ms)")]
        public int IntroDuration { get; set; } = 3000;

        [Category("Behavior")]
        [DisplayName("Unclickable")]
        public bool Unclickable { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Drag Widget To Move")]
        public bool DragToMove { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Drag Titlebar To Move")]
        public bool DragTitlebarToMove { get; set; } = true;

        [Category("Style (Background)")]
        [DisplayName("Image Path")]
        public string BackgroundImagePath { get; set; }

        [Category("Style (Background)")]
        [DisplayName("Image Opacity")]
        public double BackgroundImageOpacity { get; set; } = 1.0;

        [Category("Style")]
        [DisplayName("Context Menu Enabled")]
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
        [DisplayName("Stay Open With Mouse Focus")]
        public bool StayOpenIfMouseFocus { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show If Foreground Fullscreen")]
        public bool FullscreenActivation { get; set; } = false;

        [Category("Style (Titlebar)")]
        [DisplayName("Visible")]
        public bool ShowTitlebar { get; set; } = true;

        [Category("Style (Titlebar)")]
        [DisplayName("Reload Button Visible")]
        public bool ShowTitlebarReloadButton { get; set; } = false;

        [Category("Style (Titlebar)")]
        [DisplayName("Menu Button Visible")]
        public bool ShowTitlebarMenuButton { get; set; } = true;

        [Category("Style (Titlebar)")]
        [DisplayName("Dismiss Button Visible")]
        public bool ShowTitlebarDismissButton { get; set; } = false;

        [Category("Style (Titlebar)")]
        [DisplayName("Background Color")]
        public Color TitlebarBackgroundColor { get; set; } = Color.FromRgb(32, 32, 32);

        [Category("Style (Titlebar)")]
        [DisplayName("Background Opacity")]
        public double TitlebarBackgroundOpacity { get; set; } = 0.5;

        [Category("Style (Titlebar Name)")]
        [DisplayName("Titlebar Name Visible")]
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
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment TitlebarNameHorizontalAlignment { get; set; } = HorizontalAlignment.Center;

        [Category("Style (Titlebar)")]
        [DisplayName("Corner Radius")]
        public CornerRadius TitlebarCornerRadius { get; set; } = new CornerRadius(4);

        [Category("Style")]
        [DisplayName("Corner Radius")]
        public CornerRadius CornerRadius { get; set; } = new CornerRadius(4);

        [Category("Style (Titlebar Name)")]
        [DisplayName("Allow Editing")]
        public bool TitlebarNameAllowEditing { get; set; } = true;

        [DisplayName("Scroll Horizontal Offset")]
        public double ScrollHorizontalOffset { get; set; }

        [DisplayName("Scroll Vertical Offset")]
        public double ScrollVerticalOffset { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Full Mouse Bounds On Center Dock")]
        public bool CenterBoundsOnNonSidedDock { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Bounds Detection Axis")]
        public MouseBoundsDetectionAxis MouseBoundsDetectionAxis { get; set; } = MouseBoundsDetectionAxis.Both;

        [Category("Style (Text)")]
        [DisplayName("Font Weight")]
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;

        [Category("Style (Border)")]
        [DisplayName("Thickness")]
        public Thickness BorderThickness { get; set; } = new Thickness(1);

        [Category("Behavior (Hideable)")]
        [DisplayName("Toggle Intro On Hotkey")]
        public bool ToggleIntroOnHotkey { get; set; } = true;

        [Category("General")]
        [DisplayName("Auto Detect Screen Bounds")]
        public bool AutoDetectScreenBounds { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Ignore Screen Corner Size")]
        public int IgnoreScreenCornerSize { get; set; } = 1;

        [Category("Style")]
        [DisplayName("Resize Mode")]
        public ResizeMode ResizeMode { get; set; } = ResizeMode.NoResize;

        [Category("Behavior")]
        [DisplayName("Use Screen Size as Max Size")]
        public bool MaxSizeUseScreen { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Ignore AppBars")]
        public bool IgnoreAppBars { get; set; } = false;

        [Category("Style (Titlebar)")]
        [DisplayName("Button Font Size")]
        public int TitlebarButtonFontSize { get; set; } = 12;

        [Category("Style (Titlebar)")]
        [DisplayName("Button Size (px)")]
        public double TitlebarButtonSize { get; set; } = 16;

        [Category("Style")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment ViewHorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;

        [Category("Style")]
        [DisplayName("Vertical Alignment")]
        public VerticalAlignment ViewVerticalAlignment { get; set; } = VerticalAlignment.Stretch;

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On Show Hotkey")]
        public bool StayOpenOnShowHotkey { get; set; } = false;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Hotkey Duration (ms)")]
        public int ShowHotkeyDuration { get; set; } = 5000;

        [Category("Behavior (Hideable)")]
        [DisplayName("Dismiss Hotkey")]
        public Key HideHotKey { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Dismiss Hotkey Modifiers")]
        public ModifierKeys HideHotKeyModifiers { get; set; }

        [DisplayName("Show Hotkey Identifier")]
        public Guid ShowHotkeyIdentifier { get; } = Guid.NewGuid();

        [DisplayName("Hide Hotkey Identifier")]
        public Guid HideHotkeyIdentifier { get; } = Guid.NewGuid();

        [Category("Style (Titlebar)")]
        [DisplayName("Height (px)")]
        public double TitlebarHeight { get; set; } = double.NaN;

        [Category("Style (Titlebar)")]
        [DisplayName("Width (px)")]
        public double TitlebarWidth { get; set; } = double.NaN;

        [Category("Behavior")]
        [DisplayName("Force Hidden")]
        public bool ForceHide { get; set; }

        [Category("Behavior")]
        [DisplayName("Idle Duration")]
        public TimeSpan IdleDuration { get; set; } = TimeSpan.FromSeconds(5);

        [Category("Behavior")]
        [DisplayName("Detect Idle")]
        public bool DetectIdle { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Refocus Main Element On Show")]
        public bool RefocusMainElementOnShow { get; set; } = true;

        [Category("Style (Frame)")]
        [DisplayName("Enable Top Frame")]
        public bool ShowTopFrame { get; set; } = true;

        [Category("Style (Frame)")]
        [DisplayName("Enable Bottom Frame")]
        public bool ShowBottomFrame { get; set; } = true;

        [Category("Style (Frame)")]
        [DisplayName("Enable Left Frame")]
        public bool ShowLeftFrame { get; set; } = true;

        [Category("Style (Frame)")]
        [DisplayName("Enable Right Frame")]
        public bool ShowRightFrame { get; set; } = true;
    }
}