using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg3.cef.selenium
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the ChromeDriver executable.
            System.setProperty("webdriver.chrome.driver", "c:/temp/chromedriver.exe");

            //For setting and defining variables
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @"..\", EnvironmentVariableTarget.User);
            System.Environment.SetEnvironmentVariable("DBname", comboBoxDataBaseName.Text, EnvironmentVariableTarget.User);

            //For getting
            string Pathsave = System.Environment.GetEnvironmentVariable("PathDB", EnvironmentVariableTarget.User);
            string DBselect = System.Environment.GetEnvironmentVariable("DBname", EnvironmentVariableTarget.User);

            ChromeOptions options = new ChromeOptions();
            // Path to the CEF executable.
            options.setBinary("c:/temp/cef_binary_3.2171.1979_windows32_client/Release/cefclient.exe");
            // Port to communicate on. Required starting with ChromeDriver v2.41.
            options.addArguments("remote-debugging-port=12345");

            WebDriver driver = new ChromeDriver(options);
            driver.get("http://www.google.com/xhtml");
            sleep(3000);  // Let the user actually see something!
            WebElement searchBox = driver.findElement(By.name("q"));
            searchBox.sendKeys("ChromeDriver");
            searchBox.submit();
            sleep(3000);  // Let the user actually see something!
            driver.quit();
        }
    }
}
