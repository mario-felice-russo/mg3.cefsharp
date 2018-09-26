using mg3.behaviours;
using mg3.drivers;
using mg3.pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace mg3.tests
{
    [TestClass]
    public class CalcPageTest
    {
        [TestMethod]
        public void InstantiatePageTest()
        {
            CalcPage page = TestFactory.Instance.Create<CalcPage>();
            Assert.IsTrue(page != null, "Pagina CalcPage not created.");
        }

        [TestMethod]
        public void InstantiateBehaviourTest()
        {
            WindowApplicationBehaviour behaviour = TestFactory.Instance.Create<WindowApplicationBehaviour>();
            Assert.IsTrue(behaviour != null, "Pagina CalcPage not created.");
        }
    }
}
