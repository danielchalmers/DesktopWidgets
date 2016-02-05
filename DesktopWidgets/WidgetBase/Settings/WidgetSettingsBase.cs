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
        public bool Disabled { get; set; }

        [DisplayName("Internal Identifier")]
        public WidgetId Identifier { get; set; }

        [DisplayName("Active Time End")]
        public DateTime ActiveTimeEnd { get; set; }

        [Category("Style")]
        [DisplayName("Padding")]
        public Thickness Padding { get; set; }

        [Category("General")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Category("Style (Text)")]
        [DisplayName("Font Family")]
        public FontFamily FontFamily { get; set; }

        [Category("Style (Text)")]
        [DisplayName("Text Color")]
        public Color TextColor { get; set; }

        [Category("Style (Background)")]
        [DisplayName("Color")]
        public Color BackgroundColor { get; set; }

        [Category("Style (Border)")]
        [DisplayName("Border Color")]
        public Color BorderColor { get; set; }

        [Category("Style (Background)")]
        [DisplayName("Opacity")]
        public double BackgroundOpacity { get; set; }

        [Category("Style (Border)")]
        [DisplayName("Border Opacity")]
        public double BorderOpacity { get; set; }

        [Category("Style (Size)")]
        [DisplayName("Width (px)")]
        public double Width { get; set; }

        [Category("Style (Size)")]
        [DisplayName("Height (px)")]
        public double Height { get; set; }

        [Category("Style (Size)")]
        [DisplayName("Minimum Width (px)")]
        public double MinWidth { get; set; }

        [Category("Style (Size)")]
        [DisplayName("Minimum Height (px)")]
        public double MinHeight { get; set; }

        [Category("Style (Size)")]
        [DisplayName("Maximum Width (px)")]
        public double MaxWidth { get; set; }

        [Category("Style (Size)")]
        [DisplayName("Maximum Height (px)")]
        public double MaxHeight { get; set; }

        [Category("Style (Position)")]
        [DisplayName("Left Position (px)")]
        public double Left { get; set; }

        [Category("Style (Position)")]
        [DisplayName("Top Position (px)")]
        public double Top { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Delay (ms)")]
        public int ShowDelay { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide Delay (ms)")]
        public int HideDelay { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Animation Duration (ms)")]
        public int AnimationTime { get; set; }

        [Category("General")]
        [DisplayName("Screen Bounds")]
        public Rect ScreenBounds { get; set; }

        [Category("Style (Text)")]
        [DisplayName("Font Size")]
        public int FontSize { get; set; }

        [Category("Behavior")]
        [DisplayName("Stay On Top")]
        public bool OnTop { get; set; }

        [Category("Behavior")]
        [DisplayName("Force On Top")]
        public bool ForceOnTop { get; set; }

        [Category("Style (Border)")]
        [DisplayName("Visible")]
        public bool BorderEnabled { get; set; }

        [Category("Behavior")]
        [DisplayName("Snap To Edges")]
        public bool SnapToScreenEdges { get; set; }

        [Category("Style")]
        [DisplayName("Ease Animation")]
        public bool AnimationEase { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Mode")]
        public OpenMode OpenMode { get; set; }

        [Category("Dock")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        [Category("Dock")]
        [DisplayName("Vertical Alignment")]
        public VerticalAlignment VerticalAlignment { get; set; }

        [Category("Dock")]
        [DisplayName("Docked")]
        public bool IsDocked { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Detection Distance (px)")]
        public int MouseBounds { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Stretch Mouse Bounds")]
        public bool StretchBounds { get; set; }

        [Category("Style")]
        [DisplayName("Animation Type")]
        public AnimationType AnimationType { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Hotkey")]
        public Key HotKey { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Hotkey Modifiers")]
        public ModifierKeys HotKeyModifiers { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Activate On Show")]
        public bool ActivateOnShow { get; set; }

        [Category("Behavior")]
        [DisplayName("Force On Top Refresh Interval (ms)")]
        public int ForceOnTopInterval { get; set; }

        [Category("Style")]
        [DisplayName("Intro Duration (ms)")]
        public int IntroDuration { get; set; }

        [Category("Behavior")]
        [DisplayName("Unclickable")]
        public bool Unclickable { get; set; }

        [Category("Behavior")]
        [DisplayName("Drag Widget To Move")]
        public bool DragToMove { get; set; }

        [Category("Behavior")]
        [DisplayName("Drag Titlebar To Move")]
        public bool DragTitlebarToMove { get; set; }

        [Category("Style (Background)")]
        [DisplayName("Image Path")]
        public string BackgroundImagePath { get; set; }

        [Category("Style (Background)")]
        [DisplayName("Image Opacity")]
        public double BackgroundImageOpacity { get; set; }

        [Category("Style")]
        [DisplayName("Context Menu Enabled")]
        public bool ShowContextMenu { get; set; }

        [Category("Style")]
        [DisplayName("Scrollbar Visibility")]
        public ScrollBarVisibility ScrollBarVisibility { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Custom Mouse Detection Bounds")]
        public Rect CustomMouseDetectionBounds { get; set; }

        [Category("Dock")]
        [DisplayName("Offset")]
        public Point DockOffset { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Sound Path")]
        public string ShowSoundPath { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Sound Volume")]
        public double ShowSoundVolume { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide Sound Path")]
        public string HideSoundPath { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Hide Sound Volume")]
        public double HideSoundVolume { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open With Mouse Focus")]
        public bool StayOpenIfMouseFocus { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show If Foreground Fullscreen")]
        public bool FullscreenActivation { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Visible")]
        public TitlebarVisibilityMode TitlebarVisibilityMode { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Reload Button Visible")]
        public bool ShowTitlebarReloadButton { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Menu Button Visible")]
        public bool ShowTitlebarMenuButton { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Dismiss Button Visible")]
        public bool ShowTitlebarDismissButton { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Background Color")]
        public Color TitlebarBackgroundColor { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Background Opacity")]
        public double TitlebarBackgroundOpacity { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Titlebar Name Visible")]
        public bool ShowTitlebarName { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Font Family")]
        public FontFamily TitlebarNameFontFamily { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Font Weight")]
        public FontWeight TitlebarNameFontWeight { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Font Size")]
        public int TitlebarNameFontSize { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Color")]
        public Color TitlebarNameTextColor { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment TitlebarNameHorizontalAlignment { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Corner Radius")]
        public CornerRadius TitlebarCornerRadius { get; set; }

        [Category("Style")]
        [DisplayName("Corner Radius")]
        public CornerRadius CornerRadius { get; set; }

        [Category("Style (Titlebar Name)")]
        [DisplayName("Allow Editing")]
        public bool TitlebarNameAllowEditing { get; set; }

        [DisplayName("Scroll Horizontal Offset")]
        public double ScrollHorizontalOffset { get; set; }

        [DisplayName("Scroll Vertical Offset")]
        public double ScrollVerticalOffset { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Full Mouse Bounds On Center Dock")]
        public bool CenterBoundsOnNonSidedDock { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Bounds Detection Axis")]
        public MouseBoundsDetectionAxis MouseBoundsDetectionAxis { get; set; }

        [Category("Style (Text)")]
        [DisplayName("Font Weight")]
        public FontWeight FontWeight { get; set; }

        [Category("Style (Border)")]
        [DisplayName("Thickness")]
        public Thickness BorderThickness { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Toggle Intro On Hotkey")]
        public bool ToggleIntroOnHotkey { get; set; }

        [Category("Behavior")]
        [DisplayName("Auto Detect Screen Bounds")]
        public bool AutoDetectScreenBounds { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Ignore Screen Corner Size")]
        public int IgnoreScreenCornerSize { get; set; }

        [Category("Behavior")]
        [DisplayName("Use Screen Size as Max Size")]
        public bool MaxSizeUseScreen { get; set; }

        [Category("Behavior")]
        [DisplayName("Ignore AppBars")]
        public bool IgnoreAppBars { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Button Font Size")]
        public int TitlebarButtonFontSize { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Button Size (px)")]
        public double TitlebarButtonSize { get; set; }

        [Category("Style")]
        [DisplayName("Horizontal Alignment")]
        public HorizontalAlignment ViewHorizontalAlignment { get; set; }

        [Category("Style")]
        [DisplayName("Vertical Alignment")]
        public VerticalAlignment ViewVerticalAlignment { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open On Show Hotkey")]
        public bool StayOpenOnShowHotkey { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Show Hotkey Duration (ms)")]
        public int ShowHotkeyDuration { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Dismiss Hotkey")]
        public Key HideHotKey { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Dismiss Hotkey Modifiers")]
        public ModifierKeys HideHotKeyModifiers { get; set; }

        [DisplayName("Show Hotkey Identifier")]
        public Guid ShowHotkeyIdentifier { get; set; }

        [DisplayName("Hide Hotkey Identifier")]
        public Guid HideHotkeyIdentifier { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Height (px)")]
        public double TitlebarHeight { get; set; }

        [Category("Style (Titlebar)")]
        [DisplayName("Width (px)")]
        public double TitlebarWidth { get; set; }

        [Category("Behavior")]
        [DisplayName("Force Hidden")]
        public bool ForceHide { get; set; }

        [Category("Behavior")]
        [DisplayName("Idle Duration")]
        public TimeSpan IdleDuration { get; set; }

        [Category("Behavior")]
        [DisplayName("Detect Idle")]
        public bool DetectIdle { get; set; }

        [Category("Behavior")]
        [DisplayName("Refocus Main Element On Show")]
        public bool RefocusMainElementOnShow { get; set; }

        [Category("Style (Frame)")]
        [DisplayName("Enable Top Frame")]
        public bool ShowTopFrame { get; set; }

        [Category("Style (Frame)")]
        [DisplayName("Enable Bottom Frame")]
        public bool ShowBottomFrame { get; set; }

        [Category("Style (Frame)")]
        [DisplayName("Enable Left Frame")]
        public bool ShowLeftFrame { get; set; }

        [Category("Style (Frame)")]
        [DisplayName("Enable Right Frame")]
        public bool ShowRightFrame { get; set; }

        [Category("Behavior")]
        [DisplayName("Use Mouse Movement For Idle Detection")]
        public bool UseMouseMoveIdleDetection { get; set; }

        [Category("Behavior")]
        [DisplayName("Show Intro On Launch")]
        public bool ShowIntroOnLaunch { get; set; }

        public virtual void SetDefaults()
        {
            Disabled = false;
            Identifier = new WidgetId();
            ActiveTimeEnd = DateTime.Now;
            Padding = new Thickness(3, 2, 3, 2);
            Name = "Untitled";
            FontFamily = new FontFamily("Segoe UI");
            TextColor = Colors.Black;
            BackgroundColor = Colors.White;
            BorderColor = Colors.Gray;
            BackgroundOpacity = 0.95;
            BorderOpacity = 0.5;
            Width = double.NaN;
            Height = double.NaN;
            MinWidth = double.NaN;
            MinHeight = double.NaN;
            MaxWidth = double.NaN;
            MaxHeight = double.NaN;
            Left = double.NaN;
            Top = double.NaN;
            ShowDelay = 0;
            HideDelay = 0;
            AnimationTime = 250;
            ScreenBounds = Rect.Empty;
            FontSize = 14;
            OnTop = true;
            ForceOnTop = false;
            BorderEnabled = true;
            SnapToScreenEdges = true;
            AnimationEase = true;
            OpenMode = OpenMode.AlwaysOpen;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            IsDocked = false;
            MouseBounds = 8;
            StretchBounds = false;
            AnimationType = AnimationType.Fade;
            HotKey = Key.None;
            HotKeyModifiers = ModifierKeys.None;
            ActivateOnShow = true;
            ForceOnTopInterval = 500;
            IntroDuration = 3000;
            Unclickable = false;
            DragToMove = true;
            DragTitlebarToMove = true;
            BackgroundImagePath = "";
            BackgroundImageOpacity = 1.0;
            ShowContextMenu = true;
            ScrollBarVisibility = ScrollBarVisibility.Auto;
            CustomMouseDetectionBounds = new Rect();
            DockOffset = new Point();
            ShowSoundPath = "";
            ShowSoundVolume = 1.0;
            HideSoundPath = "";
            HideSoundVolume = 1.0;
            StayOpenIfMouseFocus = true;
            FullscreenActivation = false;
            TitlebarVisibilityMode = TitlebarVisibilityMode.AlwaysVisible;
            ShowTitlebarReloadButton = false;
            ShowTitlebarMenuButton = true;
            ShowTitlebarDismissButton = false;
            TitlebarBackgroundColor = Color.FromRgb(32, 32, 32);
            TitlebarBackgroundOpacity = 0.5;
            ShowTitlebarName = true;
            TitlebarNameFontFamily = new FontFamily("Segoe UI");
            TitlebarNameFontWeight = FontWeights.Normal;
            TitlebarNameFontSize = 12;
            TitlebarNameTextColor = Colors.WhiteSmoke;
            TitlebarNameHorizontalAlignment = HorizontalAlignment.Center;
            TitlebarCornerRadius = new CornerRadius(4);
            CornerRadius = new CornerRadius(4);
            TitlebarNameAllowEditing = true;
            ScrollHorizontalOffset = 0;
            ScrollVerticalOffset = 0;
            CenterBoundsOnNonSidedDock = true;
            MouseBoundsDetectionAxis = MouseBoundsDetectionAxis.Both;
            FontWeight = FontWeights.Normal;
            BorderThickness = new Thickness(1);
            ToggleIntroOnHotkey = true;
            AutoDetectScreenBounds = true;
            IgnoreScreenCornerSize = 1;
            MaxSizeUseScreen = true;
            IgnoreAppBars = false;
            TitlebarButtonFontSize = 12;
            TitlebarButtonSize = 16;
            ViewHorizontalAlignment = HorizontalAlignment.Stretch;
            ViewVerticalAlignment = VerticalAlignment.Stretch;
            StayOpenOnShowHotkey = false;
            ShowHotkeyDuration = 5000;
            HideHotKey = Key.None;
            HideHotKeyModifiers = ModifierKeys.None;
            ShowHotkeyIdentifier = Guid.NewGuid();
            HideHotkeyIdentifier = Guid.NewGuid();
            TitlebarHeight = double.NaN;
            TitlebarWidth = double.NaN;
            ForceHide = false;
            IdleDuration = TimeSpan.FromSeconds(5);
            DetectIdle = true;
            RefocusMainElementOnShow = true;
            ShowTopFrame = true;
            ShowBottomFrame = true;
            ShowLeftFrame = true;
            ShowRightFrame = true;
            UseMouseMoveIdleDetection = false;
            ShowIntroOnLaunch = true;
        }
    }
}