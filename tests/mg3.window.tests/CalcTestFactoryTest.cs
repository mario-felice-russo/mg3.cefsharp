using mg3.drivers;
using mg3.pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace mg3.tests
{
    [TestClass]
    public class CalcTestFactoryTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx) { }

        private WindowsDriver<WindowsElement> Driver;
        private CalcPage Page;

        [TestMethod]
        public void AdditionTest()
        {
            bool bResult = false;
            int op1 = 8, op2 = 9;

            using (Page = TestFactory.Instance.Create<CalcPage>())
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

            using (Page = TestFactory.Instance.Create<CalcPage>())
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

            using (Page = TestFactory.Instance.Create<CalcPage>())
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

            using (Page = TestFactory.Instance.Create<CalcPage>())
            {
                bResult = Page.PerformSubtraction(op1, op2);
            }

            Assert.IsTrue(bResult, "Il valore dell'operazione {0} - {1} <> {2}, è diverso da {2}!", op1, op2, (op1 - op2));
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (TestFactory.Instance.Driver != null)
            {
                TestFactory.Instance.Driver.Quit();
            }
        }
    }
}
