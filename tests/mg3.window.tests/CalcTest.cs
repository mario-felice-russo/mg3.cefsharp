using mg3.drivers;
using mg3.pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace mg3.tests
{
    [TestClass]
    public class CalcTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Driver = new WindowApplicationDriver().Get(host, port, application, operativesystem);
        }

        private string host = "http://127.0.0.1";
        private int port = 4723;
        private string application = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
        private string operativesystem = "Windows 10";

        private WindowsDriver<WindowsElement> Driver;
        private CalcPage Page;

        [TestMethod]
        public void AdditionTest()
        {
            bool bResult = false;
            int op1 = 8, op2 = 9;

            using (Page = new CalcPage(Driver))
                if (Page != null)
                {
                    bResult = Page.PerformAddition(op1, op2);
                }

            Assert.IsTrue(bResult, "Il valore dell'operazione {0} + {1} <> {2}, è diverso da {2}!", op1, op2, (op1 + op2));
        }

        [TestMethod]
        public void DivisionTest()
        {
            bool bResult = false;
            int op1 = 8, op2 = 1;

            using (Page = new CalcPage(Driver))
                if (Page != null)
                {
                    bResult = Page.PerformDivision(op1, op2);
                }

            Assert.IsTrue(bResult, "Il valore dell'operazione {0} / {1} <> {2}, è diverso da {2}!", op1, op2, (op1 / op2));
        }

        [TestMethod]
        public void MultiplicationTest()
        {
            bool bResult = false;
            int op1 = 9, op2 = 2;

            using (Page = new CalcPage(Driver))
                if (Page != null)
                {
                    bResult = Page.PerformMultiplication(op1, op2);
                }

            Assert.IsTrue(bResult, "Il valore dell'operazione {0} * {1} <> {2}, è diverso da {2}!", op1, op2, (op1 * op2));
        }

        [TestMethod]
        public void SubtractionTest()
        {
            bool bResult = false;
            int op1 = 8, op2 = 9;

            using (Page = new CalcPage(Driver))
                if (Page != null)
                {
                    bResult = Page.PerformSubtraction(op1, op2);
                }

            Assert.IsTrue(bResult, "Il valore dell'operazione {0} - {1} <> {2}, è diverso da {2}!", op1, op2, (op1 - op2));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
                Driver = null;
            }
        }        
    }
}
