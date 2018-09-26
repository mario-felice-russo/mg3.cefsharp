using mg3.behaviours;
using mg3.drivers;
using mg3.pages;
using mg3.ioc;
using OpenQA.Selenium.Appium.Windows;

namespace mg3.tests
{
    public class TestFactory : TestContainer
    {
        private static readonly TestFactory instance = new TestFactory();

        // Explicit static constructor to tell C# compiler not to mark type as beforefieldinit
        static TestFactory()
        {
        }

        private TestFactory()
        {
            RegisterComponents();
        }

        public static TestFactory Instance
        {
            get
            {
                return instance;
            }
        }

        public WindowsDriver<WindowsElement> Driver
        {
            get
            {
                if (_driver is null)
                {
                    _driver = new WindowApplicationDriver().Get(
                        GetConfiguration<string>("window.driver.host"),
                        GetConfiguration<int>("window.driver.port"),
                        GetConfiguration<string>("window.driver.app"),
                        GetConfiguration<string>("window.driver.device")
                    );
                }
                return _driver;
            }
        }
        private WindowsDriver<WindowsElement> _driver;

        public void RegisterComponents()
        {
            Configuration["window.driver.host"] = "http://127.0.0.1";
            Configuration["window.driver.port"] = 4723;
            Configuration["window.driver.app"] = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
            Configuration["window.driver.device"] = "Windows 10";

            Register<WindowApplicationDriver>(
                delegate
                {
                    return new WindowApplicationDriver();
                }
            );

            Register<WindowApplicationBehaviour>(
                delegate
                {
                    return new WindowApplicationBehaviour(Driver);
                }
            );

            Register<CalcPage>(
                delegate
                {
                    return new CalcPage(Driver);
                }
            );
        }
    }
}
