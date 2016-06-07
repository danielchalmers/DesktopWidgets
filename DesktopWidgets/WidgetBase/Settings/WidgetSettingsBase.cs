using System;
using System.ComponentModel;
using System.Windows;
using DesktopWidgets.WidgetBase.Styles;

namespace DesktopWidgets.WidgetBase.Settings
{
    public class WidgetSettingsBase
    {
        [Browsable(false)]
        [DisplayName("Disabled")]
        public bool Disabled { get; set; } = false;

        [Browsable(false)]
        [DisplayName("Internal Identifier")]
        public WidgetId Identifier { get; set; } = new WidgetId();

        [Browsable(false)]
        [DisplayName("Package Info")]
        public WidgetPackageInfo PackageInfo { get; set; }

        [Browsable(false)]
        [DisplayName("Active Time End")]
        public DateTime ActiveTimeEnd { get; set; } = DateTime.Now;

        [Category("Style")]
        public WidgetStyle Style { get; set; } = new WidgetStyle();

        [Category("Style")]
        public WidgetActionBarStyle ActionBarStyle { get; set; } = new WidgetActionBarStyle();

        [Category("General")]
        [DisplayName("Name")]
        public string Name { get; set; } = "Untitled";

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

        [Category("General")]
        [DisplayName("Screen Bounds")]
        public Rect ScreenBounds { get; set; } = Rect.Empty;

        [Category("Behavior")]
        [DisplayName("Stay On Top")]
        public bool Topmost { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Force Stay On Top")]
        public bool ForceTopmost { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Snap To Edges")]
        public bool SnapToScreenEdges { get; set; } = true;

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

        [Category("Behavior (Hideable)")]
        [DisplayName("Activate On Show")]
        public bool ActivateOnShow { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Force On Top Interval (ms)")]
        public int ForceOnTopInterval { get; set; } = 500;

        [Category("Behavior")]
        [DisplayName("Intro Duration (ms)")]
        public int IntroDuration { get; set; } = 3000;

        [Category("Behavior")]
        [DisplayName("Unclickable")]
        public bool Unclickable { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Drag Widget To Move")]
        public bool DragToMove { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Drag Action Bar To Move")]
        public bool DragActionBarToMove { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Custom Mouse Detection Bounds")]
        public Rect CustomMouseDetectionBounds { get; set; } = new Rect();

        [Category("Dock")]
        [DisplayName("Offset")]
        public Point DockOffset { get; set; } = new Point();

        [Category("Behavior (Hideable)")]
        [DisplayName("Stay Open With Mouse Focus")]
        public bool StayOpenIfMouseFocus { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Show If Foreground Fullscreen")]
        public bool FullscreenActivation { get; set; } = false;

        [Browsable(false)]
        [DisplayName("Scroll Horizontal Offset")]
        public double ScrollHorizontalOffset { get; set; }

        [Browsable(false)]
        [DisplayName("Scroll Vertical Offset")]
        public double ScrollVerticalOffset { get; set; }

        [Category("Behavior (Hideable)")]
        [DisplayName("Full Mouse Bounds On Center Dock")]
        public bool CenterBoundsOnNonSidedDock { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Bounds Detection Axis")]
        public MouseBoundsDetectionAxis MouseBoundsDetectionAxis { get; set; } = MouseBoundsDetectionAxis.Both;

        [Category("Behavior")]
        [DisplayName("Auto Detect Screen Bounds")]
        public bool AutoDetectScreenBounds { get; set; } = true;

        [Category("Behavior (Hideable)")]
        [DisplayName("Ignore Screen Corner Size")]
        public int IgnoreScreenCornerSize { get; set; } = 1;

        [Category("Behavior")]
        [DisplayName("Use Screen Size as Max Size")]
        public bool MaxSizeUseScreen { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Ignore AppBars")]
        public bool IgnoreAppBars { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Force Hidden")]
        public bool ForceHide { get; set; }

        [Category("Behavior (Idle)")]
        [DisplayName("Idle Duration")]
        public TimeSpan IdleDuration { get; set; } = TimeSpan.FromSeconds(3);

        [Category("Behavior (Idle)")]
        [DisplayName("Detect Idle")]
        public bool DetectIdle { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Refocus Main Element On Show")]
        public bool RefocusMainElementOnShow { get; set; } = true;

        [Category("Behavior (Idle)")]
        [DisplayName("Detect Mouse Movement")]
        public bool UseMouseMoveIdleDetection { get; set; } = true;

        [Category("Behavior")]
        [DisplayName("Show Intro On Launch")]
        public bool ShowIntroOnLaunch { get; set; } = true;

        [Category("Dock")]
        [DisplayName("Dock As AppBar")]
        public bool IsAppBar { get; set; } = false;

        [Category("Behavior")]
        [DisplayName("Ignore Mute")]
        public bool IgnoreMute { get; set; } = false;

        [Browsable(false)]
        [DisplayName("Mute End Time")]
        public DateTime MuteEndTime { get; set; } = DateTime.MinValue;

        [Category("Behavior (Hideable)")]
        [DisplayName("Mouse Detection Offset")]
        public Point MouseDetetcionOffset { get; set; } = new Point(0, 0);
    }
}