// ==============================================================================
// 
//   Copyright (C) 2014-2020, GitHub/Codeplex user AlphaCentaury
//   All rights reserved.
// 
//     See 'LICENSE.MD' file (or 'license.txt' if missing) in the project root
//     for complete license information.
// 
//   http://www.alphacentaury.org/movistartv
//   https://github.com/AlphaCentaury
// 
// ==============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using IpTviewr.Common.Telemetry;
using IpTviewr.Native;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace IpTviewr.Telemetry
{
    public sealed class VsAppCenter : ITelemetryProvider
    {
        #region Implementation of ITelemetryProvider

        public bool Enabled
        {
            get => AppCenter.IsEnabledAsync().Result;
            set => AppCenter.SetEnabledAsync(value).Wait();
        } // Enabled

        public void Start(IReadOnlyDictionary<string, string> properties)
        {
            if (properties == null) throw new InvalidOperationException();

            var countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            var exe = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            WindowsDesktop.GetDpi(out var dpiX, out var dpiY);

#if DEBUG
            AppCenter.LogLevel = LogLevel.Verbose;
#endif
            AppCenter.SetEnabledAsync(AppTelemetry.Enabled).Wait();
            Analytics.SetEnabledAsync(AppTelemetry.Enabled).Wait();
            Crashes.SetEnabledAsync(AppTelemetry.Enabled).Wait();

            AppCenter.SetCountryCode(countryCode);
            AppCenter.Start(properties["AppSecret"], typeof(Analytics), typeof(Crashes));
            Analytics.TrackEvent($"{exe}:Start", new Dictionary<string, string>
            {
                {"CurrentCulture", CultureInfo.CurrentCulture.Name},
                {"CurrentUICulture", CultureInfo.CurrentUICulture.Name},
                {"InstalledUICulture", CultureInfo.InstalledUICulture.Name},
                {"MonitorCount", SystemInformation.MonitorCount.ToString(CultureInfo.InvariantCulture)},
                {"DpiX", dpiX.ToString(CultureInfo.InvariantCulture) },
                {"DpiY", dpiY.ToString(CultureInfo.InvariantCulture) },
            });
        } // Start

        public void End()
        {
            var exe = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            Analytics.TrackEvent($"{exe}:End");
        } // End

        public void ScreenEvent(string eventName, string screenName, string data = null, IEnumerable<KeyValuePair<string, string>> properties = null)
        {
            Analytics.TrackEvent("Screen:" + eventName, new Dictionary<string, string>
            {
                {"Screen", screenName },
                {"Data", data }
            });
        } // ScreenEvent

        public void Exception(Exception ex, string screenName, string message = null, IEnumerable<KeyValuePair<string, string>> properties = null)
        {
            Crashes.TrackError(ex, new Dictionary<string, string>
            {
                {"Screen", screenName },
                {"Message", message }
            }, ErrorAttachmentLog.AttachmentWithText(ex.StackTrace, "ex.StackTrace"));
        } // Exception

        public void CustomEvent(string eventName, string action, string screenName, string data = null, IEnumerable<KeyValuePair<string, string>> properties = null)
        {
            Analytics.TrackEvent(eventName, new Dictionary<string, string>
            {
                {"Event", action },
                {"Data", data },
                {"Screen", screenName },
            });
        } // CustomEvent

        #endregion
    } // class VsAppCenter
} // namespace
