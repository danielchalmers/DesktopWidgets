﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DesktopWidgets.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    public sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool MustUpgrade {
            get {
                return ((bool)(this["MustUpgrade"]));
            }
            set {
                this["MustUpgrade"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Widgets {
            get {
                return ((string)(this["Widgets"]));
            }
            set {
                this["Widgets"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool EnableAdvancedMode {
            get {
                return ((bool)(this["EnableAdvancedMode"]));
            }
            set {
                this["EnableAdvancedMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("550")]
        public double ManageWidgetsHeight {
            get {
                return ((double)(this["ManageWidgetsHeight"]));
            }
            set {
                this["ManageWidgetsHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public double ManageWidgetsWidth {
            get {
                return ((double)(this["ManageWidgetsWidth"]));
            }
            set {
                this["ManageWidgetsWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("00:00:30")]
        public global::System.TimeSpan SaveDelay {
            get {
                return ((global::System.TimeSpan)(this["SaveDelay"]));
            }
            set {
                this["SaveDelay"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool RunOnStartup {
            get {
                return ((bool)(this["RunOnStartup"]));
            }
            set {
                this["RunOnStartup"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("100")]
        public int MouseBoundsPollingInterval {
            get {
                return ((int)(this["MouseBoundsPollingInterval"]));
            }
            set {
                this["MouseBoundsPollingInterval"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("550")]
        public double ManageShortcutsHeight {
            get {
                return ((double)(this["ManageShortcutsHeight"]));
            }
            set {
                this["ManageShortcutsHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public double ManageShortcutsWidth {
            get {
                return ((double)(this["ManageShortcutsWidth"]));
            }
            set {
                this["ManageShortcutsWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool LaunchProcessAsync {
            get {
                return ((bool)(this["LaunchProcessAsync"]));
            }
            set {
                this["LaunchProcessAsync"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("All")]
        public string UpdateDays {
            get {
                return ((string)(this["UpdateDays"]));
            }
            set {
                this["UpdateDays"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Version ForgetUpdateVersion {
            get {
                return ((global::System.Version)(this["ForgetUpdateVersion"]));
            }
            set {
                this["ForgetUpdateVersion"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime LastUpdateCheck {
            get {
                return ((global::System.DateTime)(this["LastUpdateCheck"]));
            }
            set {
                this["LastUpdateCheck"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Stable")]
        public global::DesktopWidgets.UpdateBranch UpdateBranch {
            get {
                return ((global::DesktopWidgets.UpdateBranch)(this["UpdateBranch"]));
            }
            set {
                this["UpdateBranch"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("95")]
        public int BetaVersion {
            get {
                return ((int)(this["BetaVersion"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AutoUpdate {
            get {
                return ((bool)(this["AutoUpdate"]));
            }
            set {
                this["AutoUpdate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool CheckForUpdates {
            get {
                return ((bool)(this["CheckForUpdates"]));
            }
            set {
                this["CheckForUpdates"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("180")]
        public int UpdateCheckIntervalMinutes {
            get {
                return ((int)(this["UpdateCheckIntervalMinutes"]));
            }
            set {
                this["UpdateCheckIntervalMinutes"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool AllowMultiInstance {
            get {
                return ((bool)(this["AllowMultiInstance"]));
            }
            set {
                this["AllowMultiInstance"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("600")]
        public double WidgetEditorWidth {
            get {
                return ((double)(this["WidgetEditorWidth"]));
            }
            set {
                this["WidgetEditorWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("650")]
        public double WidgetEditorHeight {
            get {
                return ((double)(this["WidgetEditorHeight"]));
            }
            set {
                this["WidgetEditorHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double OptionsIndex {
            get {
                return ((double)(this["OptionsIndex"]));
            }
            set {
                this["OptionsIndex"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("200")]
        public double PropertyGridNameColumnWidth {
            get {
                return ((double)(this["PropertyGridNameColumnWidth"]));
            }
            set {
                this["PropertyGridNameColumnWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ShortcutPropertiesIsExpanded {
            get {
                return ((bool)(this["ShortcutPropertiesIsExpanded"]));
            }
            set {
                this["ShortcutPropertiesIsExpanded"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int MaxConcurrentMediaPlayers {
            get {
                return ((int)(this["MaxConcurrentMediaPlayers"]));
            }
            set {
                this["MaxConcurrentMediaPlayers"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int SnapMargin {
            get {
                return ((int)(this["SnapMargin"]));
            }
            set {
                this["SnapMargin"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("600")]
        public double OptionsWidth {
            get {
                return ((double)(this["OptionsWidth"]));
            }
            set {
                this["OptionsWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("550")]
        public double OptionsHeight {
            get {
                return ((double)(this["OptionsHeight"]));
            }
            set {
                this["OptionsHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string ChangelogCache {
            get {
                return ((string)(this["ChangelogCache"]));
            }
            set {
                this["ChangelogCache"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Version UpdateWaiting {
            get {
                return ((global::System.Version)(this["UpdateWaiting"]));
            }
            set {
                this["UpdateWaiting"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("01:00:00")]
        public global::System.TimeSpan MuteDuration {
            get {
                return ((global::System.TimeSpan)(this["MuteDuration"]));
            }
            set {
                this["MuteDuration"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime MuteEndTime {
            get {
                return ((global::System.DateTime)(this["MuteEndTime"]));
            }
            set {
                this["MuteEndTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool DisableChangelog {
            get {
                return ((bool)(this["DisableChangelog"]));
            }
            set {
                this["DisableChangelog"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1E-09")]
        public double DoubleComparisonTolerance {
            get {
                return ((double)(this["DoubleComparisonTolerance"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int ChangelogDownloadPages {
            get {
                return ((int)(this["ChangelogDownloadPages"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("500")]
        public double ManageEventsWidth {
            get {
                return ((double)(this["ManageEventsWidth"]));
            }
            set {
                this["ManageEventsWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("550")]
        public double ManageEventsHeight {
            get {
                return ((double)(this["ManageEventsHeight"]));
            }
            set {
                this["ManageEventsHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("800")]
        public double EventActionPairEditorWidth {
            get {
                return ((double)(this["EventActionPairEditorWidth"]));
            }
            set {
                this["EventActionPairEditorWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("650")]
        public double EventActionPairEditorHeight {
            get {
                return ((double)(this["EventActionPairEditorHeight"]));
            }
            set {
                this["EventActionPairEditorHeight"] = value;
            }
        }
    }
}
