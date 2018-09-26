using mg3.behaviours;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace mg3.pages
{
    public enum OperationsEnum { none, sum, subtract, equals, multiply, divide };

    public class CalcPage : BasePage
    {
        public CalcPage(WindowsDriver<WindowsElement> driver) : base(driver)
        {
            Behaviour = new WindowApplicationBehaviour(driver);
            screenShotIndex = 0;
        }

        public WindowsElement btnZero => Driver.FindElementByName("Zero");
        public WindowsElement btnOne => Driver.FindElementByName("Uno");
        public WindowsElement btnTwo => Driver.FindElementByName("Due");
        public WindowsElement btnThree => Driver.FindElementByName("Tre");
        public WindowsElement btnFour => Driver.FindElementByName("Quattro");
        public WindowsElement btnFive => Driver.FindElementByName("Cinque");
        public WindowsElement btnSix => Driver.FindElementByName("Sei");
        public WindowsElement btnSeven => Driver.FindElementByName("Sette");
        public WindowsElement btnEight => Driver.FindElementByName("Otto");
        public WindowsElement btnNine => Driver.FindElementByName("Nove");

        public WindowsElement btnPlus => Driver.FindElementByName("Più");
        public WindowsElement btnMinus => Driver.FindElementByName("Meno");
        public WindowsElement btnMultiply => Driver.FindElementByName("Moltiplicazione");
        public WindowsElement btnDivision => Driver.FindElementByName("Divisione");

        public WindowsElement btnEquals => Driver.FindElementByName("Uguale");

        public WindowsElement txCalculatorResults => Driver.FindElementByAccessibilityId("CalculatorResults");

        public WindowApplicationBehaviour Behaviour { get; private set; }

        public bool PerformAddition(int op1, int op2)
        {
            PerformOperations(op1, OperationsEnum.sum, op2);
            return CheckOperation(op1, OperationsEnum.sum, op2);
        }

        public bool PerformDivision(int op1, int op2)
        {
            PerformOperations(op1, OperationsEnum.divide, op2, 40);
            return CheckOperation(op1, OperationsEnum.divide, op2);
        }

        public bool PerformMultiplication(int op1, int op2)
        {
            PerformOperations(op1, OperationsEnum.multiply, op2, 40);
            return CheckOperation(op1, OperationsEnum.multiply, op2);
        }

        public bool PerformSubtraction(int op1, int op2)
        {
            PerformOperations(op1, OperationsEnum.subtract, op2);
            return CheckOperation(op1, OperationsEnum.subtract, op2);
        }

        private void PerformOperations(int op1, OperationsEnum operation, int op2, int ms = 40)
        {
            GetButton(op1).Click();
            Thread.Sleep(ms);

            switch (operation)
            {
                case OperationsEnum.sum:
                    btnPlus.Click();
                    break;
                case OperationsEnum.subtract:
                    btnMinus.Click();
                    break;
                case OperationsEnum.equals:
                    btnEquals.Click();
                    break;
                case OperationsEnum.multiply:
                    btnMultiply.Click();
                    break;
                case OperationsEnum.divide:
                    btnDivision.Click();
                    break;
                default:
                    throw new NotSupportedException($"Not Supported operation = {operation}");
            }
            Thread.Sleep(ms);

            GetButton(op2).Click();
            Thread.Sleep(ms);

            btnEquals.Click();
            Thread.Sleep(ms);

            string strOperation = Behaviour.Capitalize(operation.ToString());
            Behaviour.CaptureScreenshot("Perform-" + strOperation + "-Test", "Execute" + strOperation, BasePage.screenShotIndex++, "png");
        }

        private bool CheckOperation(int op1, OperationsEnum op, int op2)
        {
            bool bResult = true;
            string calcresult = GetCalculatorResult();
            string opresult = "";

            switch (op)
            {
                case OperationsEnum.sum:
                    opresult = (op1 + op2).ToString();
                    break;
                case OperationsEnum.subtract:
                    opresult = (op1 - op2).ToString();
                    break;
                case OperationsEnum.multiply:
                    opresult = (op1 * op2).ToString();
                    break;
                case OperationsEnum.divide:
                    opresult = (op1 / op2).ToString();
                    break;
            }

            bResult = calcresult.Equals(opresult);
            return bResult;
        }

        private WindowsElement GetButton(int number)
        {
            WindowsElement element = null;

            switch (number)
            {
                case 0:
                    element = btnZero;
                    break;
                case 1:
                    element = btnOne;
                    break;
                case 2:
                    element = btnTwo;
                    break;
                case 3:
                    element = btnThree;
                    break;
                case 4:
                    element = btnFour;
                    break;
                case 5:
                    element = btnFive;
                    break;
                case 6:
                    element = btnSix;
                    break;
                case 7:
                    element = btnSeven;
                    break;
                case 8:
                    element = btnEight;
                    break;
                case 9:
                    element = btnNine;
                    break;
            }

            return element;
        }

        private string GetCalculatorResult()
        {
            return txCalculatorResults.Text.Replace("Display is ", string.Empty).Replace("Lo schermo è ", "").Trim();
        }
    }
}