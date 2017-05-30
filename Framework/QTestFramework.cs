using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Linq;
using System.Collections.Generic;


namespace GoogleMantisQTest.Framework

{
	public class QTestFramework
	{
		IWebDriver driver;
		int timeOut = 3; //time for Google page to load before doing any search
						 //int timeOutLong = 15;

		public void initializeDriverForChrome()
		{
			driver = new ChromeDriver();
		}

		public void closeDriver()
		{
			driver.Quit();

		}

		public void SearchForMostDownloadedVersion(string expectedVersion)
		{
			string TableID = "files_list";
			IWebElement baseTable = driver.FindElement(By.Id(TableID));
			IList<IWebElement> colDownloads = baseTable.FindElements(By.XPath("//td[@headers='files_downloads_h']"));
			var maxDownloads = colDownloads
				.Select(x => int.Parse(x.Text.Trim().Replace(",", string.Empty)))
				.OrderByDescending(x => x).ElementAt(1); //to avoid the sum of all downloads
			var maxDownloadsString = maxDownloads.ToString(@"#\,###");
			IWebElement rowOfMostDownloads = driver.FindElement(By.XPath("//tr/*[text()[contains(.,'" + maxDownloadsString + "')]]/parent::tr"));
			string versionOfMostDownloads = rowOfMostDownloads.Text.Substring(0, 5);
			Console.WriteLine("The maximum number of downloads is: {0} of the version: {1}", maxDownloadsString, versionOfMostDownloads);
			Assert.AreEqual(versionOfMostDownloads, expectedVersion);
		}

		public void OpenTestPageAndWait(string urlToTest)
		{
			driver.Navigate().GoToUrl(urlToTest);

		}

		public void WaitForGooglePageElementToBeLoaded(string elementToWait)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
					   .Until(ExpectedConditions.ElementExists(By.Id(elementToWait)));
		}

		public void WaitForMantisPageElementToBeLoaded(string elementToWait)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
					   .Until(ExpectedConditions.ElementExists(By.ClassName(elementToWait)));
		}

		public void SearchForPhrase(string phrase)
		{
			driver.FindElement(By.Id("lst-ib")).SendKeys(phrase);
		}

		public void FindCorrectResultAndOpenPage(string result, string resultUrl)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
				.Until(ExpectedConditions.ElementExists((By.XPath("//a[@aria-label='Page 3']"))));
			driver.FindElement(By.XPath("//a[@aria-label='Page 3']")).Click();
			new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
			  .Until(ExpectedConditions.ElementExists(By.LinkText(result)));
			driver.FindElement(By.LinkText(result)).Click();
			driver.Url.Equals(resultUrl);
		}

		public void FindDownloadStatisticsAndOpenPage(string result, string resultUrl)
		{
			new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
				.Until(ExpectedConditions.ElementExists((By.XPath("//a[text()='Files']"))));
			driver.FindElement(By.XPath("//a[text()='Files']")).Click();
			new WebDriverWait(driver, TimeSpan.FromSeconds(timeOut))
			  .Until(ExpectedConditions.ElementExists(By.LinkText(result)));
			driver.FindElement(By.LinkText(result)).Click();
			driver.Url.Equals(resultUrl);
		}

	}
}
