﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IpTviewr.ChannelList.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SetUiCulture {
            get {
                return ((string)(this["SetUiCulture"]));
            }
            set {
                this["SetUiCulture"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastSelectedServiceProvider {
            get {
                return ((string)(this["LastSelectedServiceProvider"]));
            }
            set {
                this["LastSelectedServiceProvider"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string LastSelectedService {
            get {
                return ((string)(this["LastSelectedService"]));
            }
            set {
                this["LastSelectedService"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection MruChannels {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["MruChannels"]));
            }
            set {
                this["MruChannels"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Telemetry_GoogleAnalyticsClientId {
            get {
                return ((string)(this["Telemetry_GoogleAnalyticsClientId"]));
            }
            set {
                this["Telemetry_GoogleAnalyticsClientId"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::IpTviewr.Common.Telemetry.TelemetryConfiguration Telemetry {
            get {
                return ((global::IpTviewr.Common.Telemetry.TelemetryConfiguration)(this["Telemetry"]));
            }
            set {
                this["Telemetry"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.Collections.Specialized.StringCollection PushIgnoreList {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["PushIgnoreList"]));
            }
            set {
                this["PushIgnoreList"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string SetCulture {
            get {
                return ((string)(this["SetCulture"]));
            }
            set {
                this["SetCulture"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::System.DateTime LastCheckedForUpdates {
            get {
                return ((global::System.DateTime)(this["LastCheckedForUpdates"]));
            }
            set {
                this["LastCheckedForUpdates"] = value;
            }
        }
    }
}
