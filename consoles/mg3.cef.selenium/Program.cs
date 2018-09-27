using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Threading;

namespace mg3.cef.selenium
{
    class Program
    {
        static void Main(string[] args)
        {
            string spath = Environment.CurrentDirectory;

            // --Environment.SetEnvironmentVariable("Path", spath + @"\drivers\chromedriver.exe");

            ChromeOptions options = new ChromeOptions();

            options.AcceptInsecureCertificates = true;
            
            // options.BinaryLocation = spath + @"\drivers\chromedriver.exe";
            options.AddArgument("verbose");
            options.AddArgument("remote-debugging-port=8088");
            options.AddArgument("log-path=chromedriver.log");
            options.AddArgument("--log-level=ALL");

            options.BinaryLocation = @"E:\Works\mg3.cefsharp\clients\mg3.contacts\bin\x64\Debug\mg3.contacts.exe";

            /*
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--disable-web-security");
            options.AddArgument("--allow-insecure-localhost");
            options.AddArgument("--allow-running-insecure-content");
            options.AddArgument("--acceptInsecureCerts=true");
            options.AddArgument("--proxy-server='direct://'");
            options.AddArgument("--proxy-bypass-list=*");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-infobars");
            options.AddArgument("--window-size=1920,1080");
            options.AddArgument("--incognito");
            options.AddArgument("--headless");
            */

            ChromeDriverService service = ChromeDriverService.CreateDefaultService(@"E:\Works\mg3.cefsharp\consoles\mg3.cef.selenium\drivers\");

            options.SetLoggingPreference(OpenQA.Selenium.LogType.Driver, OpenQA.Selenium.LogLevel.All);
            options.SetLoggingPreference(LogType.Browser, LogLevel.All);
            options.SetLoggingPreference(LogType.Driver, LogLevel.All);
            // options.SetLoggingPreference(LogType.Client, LogLevel.All);
            // options.SetLoggingPreference(LogType.Profiler, LogLevel.All);
            // options.SetLoggingPreference(LogType.Server, LogLevel.All);

            // ChromeDriver driver = new ChromeDriver(spath + @"\drivers\", options);
            ChromeDriver driver = new ChromeDriver(service, options);
            driver.Navigate().GoToUrl("https://www.google.it/");
            
            driver.Quit();
        }
    }
}
