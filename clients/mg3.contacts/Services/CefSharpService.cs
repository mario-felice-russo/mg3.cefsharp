using CefSharp;
using CefSharp.Wpf;
using Handlers;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Services
{
    public class CefSharpService
    {
        public CefSharpService()
        {
        }

        public CefSharpConfiguration Config { get; set; }

        public void ConfigureBrowser(string schemename)
        {
            Config = new CefSharpConfiguration()
            {
                Address = string.Format("{0}:{0}/index.html", schemename),
                // Address = "chrome://version",
                SchemeList = new List<string>() { schemename },
                SchemeName = schemename,
                ShowDevtools = false
            };

            // CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            CefSettings Settings = new CefSettings();
            Settings.RemoteDebuggingPort = 8088;
            // Settings.CefCommandLineArgs.Add("renderer-process-limit", "1");
            Settings.LogSeverity = LogSeverity.Default;
            Settings.RegisterScheme(new CefCustomScheme()
            {
                SchemeName = Config.SchemeName,
                SchemeHandlerFactory = new SchemeHandlerFactory(Config.SchemeList),
                IsSecure = true, // treated with the same security rules as those applied to "https" URLs
                IsCorsEnabled = true,
                IsCSPBypassing = true
            });
            Settings.PackLoadingDisabled = false;
            Settings.CachePath = AppDomain.CurrentDomain.BaseDirectory + "cache";
            Settings.CefCommandLineArgs.Add("disable-application-cache", "1");
            Settings.CefCommandLineArgs.Add("disable-session-storage", "1");

            if (!Cef.IsInitialized) Cef.Initialize(Settings);
        }
    }
}
