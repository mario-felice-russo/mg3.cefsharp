using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.IO;

namespace mg3.behaviours
{
    public class Behaviour
    {
        public Behaviour(WindowsDriver<WindowsElement> driver)
        {
            Driver = driver;
        }

        public WindowsDriver<WindowsElement> Driver { get; set; }
        
        public void CaptureScreenshot(string test, string step, int number, string extension, string imagestore = @".\images\")
        {
            try
            {
                Screenshot screenshot = ((ITakesScreenshot)Driver).GetScreenshot();
                string path = Path.GetFullPath(imagestore);

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                string filename = GetScreenShotFile(test, step, number, extension, imagestore);
                screenshot.SaveAsFile(filename, ScreenshotImageFormat.Png);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public string GetScreenShotFile(string test, string step, int number, string extension, string imagestore)
        {
            return imagestore + test + "-" + step + number + "." + extension;
        }

        public string Capitalize(string str)
        {
            if (str == null)
                return null;

            if (str.Length > 1)
                return char.ToUpper(str[0]) + str.Substring(1);

            return str.ToUpper();
        }
    }
}
