using NUnit.Framework;
using GoogleMantisQTest.Framework;

namespace GoogleMantisTests
{
    [TestFixture]
	public class GoogleMantisTests : QTestFramework
	{

		string urlGoogle = "http://google.pl";
		string urlMantisOnSF = "https://sourceforge.net/projects/mantisbt/";
		string urlMantisDownloads = "https://sourceforge.net/projects/mantisbt/files/mantis-stable/";
		string googlePageLocator = "gs_htif0";

        [OneTimeSetUp]
		public void TestSetup()
		{
			initializeDriverForChrome();
		}

        [OneTimeTearDown]
		public void Cleanup()
		{
			closeDriver();
		}

		[TestCase]
		public void OpenGoogleAndFindMantis()
		{
			OpenTestPageAndWait(urlGoogle);
			WaitForGooglePageElementToBeLoaded(googlePageLocator);
			SearchForPhrase("Mantis");
			FindCorrectResultAndOpenPage("MantisBT | SourceForge.net", urlMantisOnSF);
		}

		[TestCase]
		public void CheckMantisMostDownladedVersion()
		{
			OpenTestPageAndWait(urlMantisOnSF);
			FindDownloadStatisticsAndOpenPage("mantis-stable", urlMantisDownloads);
			SearchForMostDownloadedVersion("2.4.1");
		}
	}
}
