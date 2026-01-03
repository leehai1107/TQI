using System.Collections.ObjectModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TQI.NBTeam.Commons;

public static class ChromeDriverCommon
{
	public static IWebElement FindElementById(this ChromeDriver chromeDriver, string id)
	{
		return chromeDriver.FindElement(By.Id(id));
	}

	public static IWebElement FindElementByName(this ChromeDriver chromeDriver, string name)
	{
		return chromeDriver.FindElement(By.Name(name));
	}

	public static IWebElement FindElementByXPath(this ChromeDriver chromeDriver, string xPath)
	{
		return chromeDriver.FindElement(By.XPath(xPath));
	}

	public static IWebElement FindElementByCssSelector(this ChromeDriver chromeDriver, string cssSelector)
	{
		return chromeDriver.FindElement(By.CssSelector(cssSelector));
	}

	public static ReadOnlyCollection<IWebElement> FindElementsById(this ChromeDriver chromeDriver, string id)
	{
		return chromeDriver.FindElements(By.Id(id));
	}

	public static ReadOnlyCollection<IWebElement> FindElementsByName(this ChromeDriver chromeDriver, string name)
	{
		return chromeDriver.FindElements(By.Name(name));
	}

	public static ReadOnlyCollection<IWebElement> FindElementsByXPath(this ChromeDriver chromeDriver, string xPath)
	{
		return chromeDriver.FindElements(By.XPath(xPath));
	}

	public static ReadOnlyCollection<IWebElement> FindElementsByCssSelector(this ChromeDriver chromeDriver, string cssSelector)
	{
		return chromeDriver.FindElements(By.CssSelector(cssSelector));
	}
}
