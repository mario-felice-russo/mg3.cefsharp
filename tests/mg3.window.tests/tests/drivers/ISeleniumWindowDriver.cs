using System.Diagnostics;
using OpenQA.Selenium.Appium.Windows;

namespace mg3.drivers
{
    public interface ISeleniumWindowDriver
    {
        WindowsDriver<WindowsElement> Get(string host, int port, string application, string onsystem);

        bool Close();
        bool IsNotAlreadyRunning();

        Process RunProcess(string filename, string filepath);
    }
}