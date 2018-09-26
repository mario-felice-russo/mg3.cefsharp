using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using System;

namespace mg3.behaviours
{
    public class WindowApplicationBehaviour : Behaviour
    {
        public WindowApplicationBehaviour(WindowsDriver<WindowsElement> driver) : base(driver)
        {
        }
        
        public WindowsElement WaitUntil(string elementAccessibilityId, int seconds)
        {
            WindowsElement element = null;

            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(Driver)
            {
                Timeout = TimeSpan.FromSeconds(seconds),
                PollingInterval = TimeSpan.FromSeconds(1)
            };
            wait.IgnoreExceptionTypes(typeof(InvalidOperationException), typeof(WebDriverTimeoutException));

            try
            {
                element = wait.Until(
                    driver =>
                    {
                        WindowsElement e = null;

                        try
                        {
                            driver.SwitchTo().Window(driver.WindowHandles[0]);
                            e = driver.FindElementByAccessibilityId(elementAccessibilityId);
                        }
                        catch (WebDriverTimeoutException) { }
                        catch (Exception) { }

                        return e;
                    }
                );
            }
            catch (WebDriverTimeoutException) { }

            return element;
        }
    }
}
