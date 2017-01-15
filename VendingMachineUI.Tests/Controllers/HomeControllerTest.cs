using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using VendingMachineUI;
using VendingMachineUI.Controllers;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;


namespace VendingMachineUI.Tests
{
	[TestFixture]
	public class HomeControllerTest
	{
	    private IWebDriver ffDriver;

	    [SetUp]
	    public void SetUp()
	    {
	        ffDriver = new FirefoxDriver();

	    }

	    [Test]
	    public void Test1()
	    {
	        ffDriver.Navigate().GoToUrl("http://localhost/RhinoCoffee/");
            ffDriver.FindElement(By.Id("coffeeButton")).Click();
            Assert.That(ffDriver.FindElement(By.Id("coffeePrice")).Text, Is.EqualTo("coffee - $ 2.25"));
	    }
	}
}
