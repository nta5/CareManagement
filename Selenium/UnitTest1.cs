using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Selenium
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestService()
        {
            string urlServices = "https://localhost:7121/Services";
            string urlQualification = "https://localhost:7121/Qualifications";
            ChromeDriver driver = new ChromeDriver();

            // This redirects to the Qualifications URL
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlQualification);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests creating a qualification
            driver.FindElement(By.Id("CreateQualification")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("QualificationDescription")).SendKeys("QD1");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();


            // This redirects to the Services URL
            driver.Navigate().GoToUrl(urlServices);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests creating a service
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("Hours")).SendKeys("8");
            driver.FindElement(By.Id("Rate")).SendKeys("1");
            driver.FindElement(By.Id("Type")).SendKeys("ServiceTypeTest");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();

            // This tests editing a service
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("Hours")).Clear();
            driver.FindElement(By.Id("Rate")).Clear();
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Hours")).SendKeys("9");
            driver.FindElement(By.Id("Rate")).SendKeys("1");
            driver.FindElement(By.Id("Type")).SendKeys("TypeEdited");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();


            // This tests viewing a service
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Details")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();

            // This tests deleting a service
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Delete")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
        }



        [TestMethod]
        public void TestQualification()
        {
            string urlQualification = "https://localhost:7121/Qualifications";
            ChromeDriver driver = new ChromeDriver();

            // This redirects to the Qualifications URL
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlQualification);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests creating a qualification
            driver.FindElement(By.Id("CreateQualification")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("QualificationDescription")).SendKeys("QD2");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();

            // This tests editing a qualification
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("QualificationDescription")).Clear();
            driver.FindElement(By.Id("QualificationDescription")).SendKeys("QD3");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();

            // This tests viewing a qualification
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Details")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();

            // This tests deleting a qualification
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Delete")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
        }


        [TestMethod]
        public void TestInvoice()
        {
            string urlInvoice = "https://localhost:7121/Invoices";
            ChromeDriver driver = new ChromeDriver();

            // This redirects to the Invoice URL
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlInvoice);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests creating an invoice
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Send date and time to the DateTime elements
            IWebElement startDate = driver.FindElement(By.Id("StartDate"));
            driver.FindElement(By.Id("StartDate")).Click();
            // Fill date as yyyy/mm/dd
            startDate.SendKeys("2023\t03-01");  // Use tab to shift to the next value
            startDate.SendKeys("0245PM");
            IWebElement endDate = driver.FindElement(By.Id("EndDate"));
            driver.FindElement(By.Id("EndDate")).Click();
            endDate.SendKeys("2023\t04-01");
            endDate.SendKeys("0245PM");
            IWebElement datePaid = driver.FindElement(By.Id("DatePaid"));
            driver.FindElement(By.Id("DatePaid")).Click();
            datePaid.SendKeys("2023\t04-01");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);


            // This redirects to the Invoice URL
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlInvoice);

            // This tests viewing a invoice
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Details")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();

            // This tests editing a invoice
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();

            if (driver.FindElement(By.LinkText("Delete")) == null)
            {
                return;
            }
            // This tests deleting a invoice
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Delete")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
        }


        [TestMethod]
        public void TestReport()
        {
            string urlReport = "https://localhost:7121/Report";
            ChromeDriver driver = new ChromeDriver();

            // This redirects to the report URL
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlReport);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests searching for reports by searchRenter
            driver.FindElement(By.Name("searchRenter")).Clear();
            driver.FindElement(By.Name("searchRenter")).SendKeys("Albert");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests searching for reports by searchService
            driver.FindElement(By.Name("searchService")).Clear();
            driver.FindElement(By.Name("searchService")).SendKeys("Nursing");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests searching for reports by time range
            IWebElement startTime = driver.FindElement(By.Name("startTime"));
            // Fill date as yyyy/mm/ddhhmm
            startTime.SendKeys("2023\t04-010700AM");  // Use tab to shift to the next value
            IWebElement endTime = driver.FindElement(By.Name("endTime"));
            endTime.SendKeys("2023\t04-031100PM");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }


        [TestMethod]
        public void TestSchedule()
        {
            string urlSchedule = "https://localhost:7121/Schedules";
            ChromeDriver driver = new ChromeDriver();

            // This redirects to the schedule URL
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(urlSchedule);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // This tests creating a schedule
            driver.FindElement(By.LinkText("Create a new schedule")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Send date and time to the DateTime elements
            IWebElement startTime = driver.FindElement(By.Id("StartTime"));
            driver.FindElement(By.Id("StartTime")).Click();
            // Fill date as yyyy/mm/dd
            startTime.SendKeys("2023\t03-01");  // Use tab to shift to the next value
            startTime.SendKeys("0245PM");
            IWebElement endTime = driver.FindElement(By.Id("EndTime"));
            driver.FindElement(By.Id("EndTime")).Click();
            endTime.SendKeys("2023\t04-01");
            endTime.SendKeys("0245PM");

            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Navigate().GoToUrl(urlSchedule);


            // This tests editing a schedule
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            // Send date and time to the DateTime elements
            IWebElement startTimeEdit = driver.FindElement(By.Id("StartTime"));
            driver.FindElement(By.Id("StartTime")).Click();
            // Fill date as yyyy/mm/dd
            startTimeEdit.SendKeys("2023\t03-02");  // Use tab to shift to the next value
            startTimeEdit.SendKeys("0246PM");
            IWebElement endTimeEdit = driver.FindElement(By.Id("EndTime"));
            driver.FindElement(By.Id("EndTime")).Click();
            endTimeEdit.SendKeys("2023\t04-02");
            endTimeEdit.SendKeys("0246PM");
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);


            // This tests viewing a schedule
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Details")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Back to List")).Click();

            // This tests deleting a schedule
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.LinkText("Delete")).Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.XPath("//Input[@type='submit']")).Click();
        }
    }
}

