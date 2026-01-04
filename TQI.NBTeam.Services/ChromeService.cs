using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using TQI.NBTeam.Commons;

namespace TQI.NBTeam.Services;

public class ChromeService
{
	public enum StatusOpenChrome
	{
		Opened,
		Closed
	}

	public Process Process { get; set; }

	public Process ProcessId { get; set; }

	public int IndexChrome { get; set; }

	public bool HideBrowser { get; set; }

	public bool DisableImage { get; set; } = false;

	public bool DisableSound { get; set; }

	public bool Incognito { get; set; }

	public bool IsUseProfile { get; set; } = false;

	public string Proxy { get; set; }

	public string ProfilePath { get; set; }

	public string App { get; set; }

	public string UserAgent { get; set; }

	public string SessionId { get; set; }

	public string Port { get; set; }

	public Point Size { get; set; } = new Point(520, 770);

	public Point Position { get; set; }

	public ChromeDriver chromeDriver { get; set; }

	public (StatusOpenChrome, string) OpenChrome()
	{
		try
		{
			ChromeOptions chromeOptions = new ChromeOptions();
			chromeOptions.AddArguments("--window-size=" + Size.X + "," + Size.Y, "--window-position=" + Position.X + "," + Position.Y, "--disable-blink-features=AutomationControlled", "--disable-infobars", "--no-sandbox", "--disable-web-security", "--ignore-certificate-errors", "--disable-popup-blocking", "--no-default-browser-check", "--no-first-run", "--lang=en_US", "--hide-crash-restore-bubble", "--disk-cache-size=0", "--media-cache-size=0", "--disable-3d-apis", "--disable-webgl");
			if (DisableSound)
			{
				chromeOptions.AddArgument("--mute-audio");
			}
			if (HideBrowser)
			{
				chromeOptions.AddArgument("--headless=new");
			}
			if (DisableImage)
			{
				chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 2);
				chromeOptions.AddArgument("--blink-settings=imagesEnabled=false");
			}
			else
			{
				chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.images", 1);
			}
			if (!string.IsNullOrEmpty(UserAgent))
			{
				chromeOptions.AddArgument("--user-agent=" + UserAgent);
			}
			if (Incognito)
			{
				chromeOptions.AddArgument("--incognito");
			}
			chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
			chromeOptions.AddArgument("--disable-features=DisableLoadExtensionCommandLineSwitch");
			chromeOptions.AddAdditionalChromeOption("useAutomationExtension", true);
			chromeOptions.AddExcludedArgument("enable-automation");
			chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
			chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
			chromeOptions.AddUserProfilePreference("profile.default_content_setting_values.geolocation", 0);
			chromeOptions.AddUserProfilePreference("extensions.ui.developer_mode", true);
			chromeOptions.AddArgument("--disk-cache-size=0");
			chromeOptions.AddArgument("--media-cache-size=0");
			chromeOptions.BrowserVersion = "137";
			if (IsUseProfile && !string.IsNullOrEmpty(ProfilePath))
			{
				if (!Directory.Exists(ProfilePath))
				{
					Directory.CreateDirectory(ProfilePath);
				}
				chromeOptions.AddArgument("--user-data-dir=" + ProfilePath);
			}
			string extFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "lib", "Extensions");
			if (Directory.Exists(extFolder))
			{
				List<string> subDirs = (from d in Directory.GetDirectories(extFolder)
					where Directory.GetFiles(d, "*.crx", SearchOption.TopDirectoryOnly).Any() || Directory.GetFiles(d, "manifest.json", SearchOption.TopDirectoryOnly).Any()
					select d).ToList();
				if (subDirs.Any())
				{
					string joined = string.Join(",", subDirs);
					chromeOptions.AddArgument("--load-extension=" + joined);
				}
				else
				{
					string[] crxFiles = Directory.GetFiles(extFolder, "*.crx");
					if (crxFiles.Any())
					{
						string joined2 = string.Join(",", crxFiles);
						chromeOptions.AddArgument("--load-extension=" + joined2);
					}
				}
			}
			if (!string.IsNullOrEmpty(Proxy))
			{
				Proxy proxy = new Proxy
				{
					Kind = ProxyKind.Manual,
					IsAutoDetect = false,
					HttpProxy = Proxy,
					SslProxy = Proxy
				};
				chromeOptions.Proxy = proxy;
			}
			ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
			chromeDriverService.HideCommandPromptWindow = true;
			chromeDriverService.DisableBuildCheck = true;
			chromeDriver = new ChromeDriver(chromeDriverService, chromeOptions, TimeSpan.FromMinutes(3.0));
			ProcessId = Process.GetProcessById(chromeDriverService.ProcessId);
			chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(3.0);
			chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120.0);
			chromeDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(60.0);
			SessionId = chromeDriver.SessionId.ToString();
			Port = chromeDriverService.Port.ToString();
		}
		catch (Exception ex)
		{
			if (ex.ToString().Contains("session timed out after"))
			{
				Common.KillProcess("chrome");
				Common.KillProcess("chromedriver");
				return (StatusOpenChrome.Closed, "session timed out after");
			}
		}
		return (StatusOpenChrome.Opened, "");
	}

	public async Task<bool> QuitAsync()
	{
		try
		{
			await Task.Run(delegate
			{
				try
				{
					chromeDriver.Quit();
				}
				catch
				{
				}
			}).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch
		{
		}
		try
		{
			if (Process != null && !Process.HasExited)
			{
				Process.Kill();
			}
			if (ProcessId != null && !ProcessId.HasExited)
			{
				ProcessId.Kill();
			}
		}
		catch
		{
		}
		Process = null;
		ProcessId = null;
		return true;
	}

	public void RefreshWebPage()
	{
		chromeDriver.Navigate().Refresh();
	}

	public string GetURL()
	{
		return chromeDriver.Url;
	}

	public int GotoURL(string url)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			chromeDriver.Navigate().GoToUrl(url);
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public void AddCookie(string name, string value, string domain, string path = "/", DateTime? expiry = null, bool isSecure = false, bool isHttpOnly = false)
	{
		try
		{
			Cookie cookie = new Cookie(name, value, domain, path, expiry, isSecure, isHttpOnly, "Lax");
			chromeDriver.Manage().Cookies.AddCookie(cookie);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error adding cookie: " + ex.Message);
		}
	}

	public void AddSimpleCookie(string name, string value, string domain)
	{
		AddCookie(name, value, domain);
	}

	public int OpenNewTab(string url, bool switchToLastTab = true)
	{
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			chromeDriver.ExecuteScript("window.open('" + url + "', '_blank').focus();");
			WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(10.0));
			wait.Until((IWebDriver d) => d.WindowHandles.Count > 1);
			if (switchToLastTab)
			{
				chromeDriver.SwitchTo().Window(chromeDriver.WindowHandles.Last());
			}
			wait.Until((IWebDriver d) => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");
			return 1;
		}
		catch (Exception)
		{
			return 0;
		}
	}

	public string FindElement(string xPath)
	{
		if (IsXPath(xPath))
		{
			xPath = xPath.Replace("'", "\\'");
			return "document.evaluate('" + xPath + "', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue";
		}
		string text = "";
		string text2 = "";
		int num = 0;
		int num2 = 0;
		string[] array = xPath.Split('|');
		switch (array.Length)
		{
		case 1:
			text = array[0].Trim();
			break;
		case 2:
			text = array[0].Trim();
			num = Convert.ToInt32(array[1].Trim());
			break;
		case 3:
			text = array[0].Trim();
			num = Convert.ToInt32(array[1].Trim());
			text2 = array[2].Trim();
			break;
		case 4:
			text = array[0].Trim();
			num = Convert.ToInt32(array[1].Trim());
			text2 = array[2].Trim();
			num2 = Convert.ToInt32(array[3].Trim());
			break;
		}
		if (num2 == 0)
		{
			return string.Format("document.querySelectorAll('{0}')[{1}]", text.Replace("'", "\\'"), num);
		}
		return string.Format("document.querySelectorAll('{0}')[{1}].querySelectorAll('{2}')[{3}]", text.Replace("'", "\\'"), num, text2.Replace("'", "\\'"), num2);
	}

	public int Click(int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0, int times = 1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		for (int i = 0; i < times; DelayTime(1.0), i++)
		{
			try
			{
				if (subTypeAttribute == 0)
				{
					switch (typeAttribute)
					{
					case 1:
						chromeDriver.FindElements(By.Id(attributeValue))[index].Click();
						break;
					case 2:
						chromeDriver.FindElementsByName(attributeValue)[index].Click();
						break;
					case 3:
						chromeDriver.FindElementsByXPath(attributeValue)[index].Click();
						break;
					case 4:
						chromeDriver.FindElementsByCssSelector(attributeValue)[index].Click();
						break;
					}
				}
				else
				{
					switch (typeAttribute)
					{
					case 1:
						chromeDriver.FindElementsById(attributeValue)[index].FindElements(By.Id(subAttributeValue))[subIndex].Click();
						break;
					case 2:
						chromeDriver.FindElementsByName(attributeValue)[index].FindElements(By.Name(subAttributeValue))[subIndex].Click();
						break;
					case 3:
						chromeDriver.FindElementsByXPath(attributeValue)[index].FindElements(By.XPath(subAttributeValue))[subIndex].Click();
						break;
					case 4:
						chromeDriver.FindElementsByCssSelector(attributeValue)[index].FindElements(By.CssSelector(subAttributeValue))[subIndex].Click();
						break;
					}
				}
				flag = true;
			}
			catch (Exception)
			{
				continue;
			}
			break;
		}
		return flag ? 1 : 0;
	}

	public bool ExecuteJSClick(string xPath)
	{
		if (!IsLive())
		{
			return false;
		}
		try
		{
			ExecuteScript(FindElement(xPath) + ".click()");
			return true;
		}
		catch
		{
		}
		return false;
	}

	public int ClickWithAction(int typeAttribute, string attributeValue, int index = 0, int subTypeAttribute = 0, string subAttributeValue = "", int subIndex = 0)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (subTypeAttribute != 0)
			{
				switch (typeAttribute)
				{
				case 1:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsById(attributeValue)[index].FindElements(By.Id(subAttributeValue))[subIndex]).Perform();
					break;
				case 2:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsByName(attributeValue)[index].FindElements(By.Name(subAttributeValue))[subIndex]).Perform();
					break;
				case 3:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsByXPath(attributeValue)[index].FindElements(By.XPath(subAttributeValue))[subIndex]).Perform();
					break;
				case 4:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsByCssSelector(attributeValue)[index].FindElements(By.CssSelector(subAttributeValue))[subIndex]).Perform();
					break;
				}
			}
			else
			{
				switch (typeAttribute)
				{
				case 1:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsById(attributeValue)[index]).Perform();
					break;
				case 2:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsByName(attributeValue)[index]).Perform();
					break;
				case 3:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsByXPath(attributeValue)[index]).Perform();
					break;
				case 4:
					new Actions(chromeDriver).Click(chromeDriver.FindElementsByCssSelector(attributeValue)[index]).Perform();
					break;
				}
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public bool TryClickElement(string selector)
	{
		if (!IsLive())
		{
			return false;
		}
		try
		{
			IWebElement webElement = FindElementByCustomSelector(selector);
			if (webElement == null)
			{
				return false;
			}
			webElement?.Click();
			return true;
		}
		catch
		{
		}
		return false;
	}

	public bool ScrollAndClick(string selector)
	{
		if (CheckChromeClosed())
		{
			return false;
		}
		try
		{
			ScrollAndWait(selector);
			return TryClickElement(selector);
		}
		catch (Exception)
		{
		}
		return false;
	}

	public int SendKeys(int typeAttribute, string attributeValue, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (isClick)
			{
				Click(typeAttribute, attributeValue);
				DelayTime(timeDelayAfterClick);
			}
			switch (typeAttribute)
			{
			case 1:
				chromeDriver.FindElementById(attributeValue).SendKeys(content);
				break;
			case 2:
				chromeDriver.FindElementByName(attributeValue).SendKeys(content);
				break;
			case 3:
				chromeDriver.FindElementByXPath(attributeValue).SendKeys(content);
				break;
			case 4:
				chromeDriver.FindElementByCssSelector(attributeValue).SendKeys(content);
				break;
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendKeys(int typeAttribute, string attributeValue, int index, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (isClick)
			{
				Click(typeAttribute, attributeValue);
				DelayTime(timeDelayAfterClick);
			}
			switch (typeAttribute)
			{
			case 1:
				chromeDriver.FindElementsById(attributeValue)[index].SendKeys(content);
				break;
			case 2:
				chromeDriver.FindElementsByName(attributeValue)[index].SendKeys(content);
				break;
			case 3:
				chromeDriver.FindElementsByXPath(attributeValue)[index].SendKeys(content);
				break;
			case 4:
				chromeDriver.FindElementsByCssSelector(attributeValue)[index].SendKeys(content);
				break;
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendKeys(int typeAttribute, string attributeValue, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (isClick)
			{
				Click(typeAttribute, attributeValue);
				DelayTime(timeDelayAfterClick);
			}
			for (int i = 0; i < content.Length; i++)
			{
				switch (typeAttribute)
				{
				case 1:
					chromeDriver.FindElementById(attributeValue).SendKeys(content[i].ToString());
					break;
				case 2:
					chromeDriver.FindElementByName(attributeValue).SendKeys(content[i].ToString());
					break;
				case 3:
					chromeDriver.FindElementByXPath(attributeValue).SendKeys(content[i].ToString());
					break;
				case 4:
					chromeDriver.FindElementByCssSelector(attributeValue).SendKeys(content[i].ToString());
					break;
				}
				if (timeDelay_Second > 0.0)
				{
					int num = Convert.ToInt32(timeDelay_Second * 1000.0);
					if (num < 100)
					{
						num = 100;
					}
					Thread.Sleep(new Random().Next(num, num + 50));
				}
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendKeys(int typeAttribute, string attributeValue, int index, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (isClick)
			{
				Click(typeAttribute, attributeValue);
				DelayTime(timeDelayAfterClick);
			}
			for (int i = 0; i < content.Length; i++)
			{
				switch (typeAttribute)
				{
				case 1:
					chromeDriver.FindElementsById(attributeValue)[index].SendKeys(content[i].ToString());
					break;
				case 2:
					chromeDriver.FindElementsByName(attributeValue)[index].SendKeys(content[i].ToString());
					break;
				case 3:
					chromeDriver.FindElementsByXPath(attributeValue)[index].SendKeys(content[i].ToString());
					break;
				case 4:
					chromeDriver.FindElementsByCssSelector(attributeValue)[index].SendKeys(content[i].ToString());
					break;
				}
				if (timeDelay_Second > 0.0)
				{
					int num = Convert.ToInt32(timeDelay_Second * 1000.0);
					if (num < 100)
					{
						num = 100;
					}
					Thread.Sleep(new Random().Next(num, num + 50));
				}
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendKeys(Random rd, int typeAttribute, string attributeValue, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (isClick)
			{
				Click(typeAttribute, attributeValue);
				DelayTime(timeDelayAfterClick);
			}
			int num = 0;
			int num2 = rd.Next(1, 1000) % 3;
			if (content.Length < 3)
			{
				num2 = 2;
			}
			else
			{
				num = rd.Next(1, content.Length * 3 / 4);
				switch (num2)
				{
				case 0:
				{
					string content4 = content.Substring(0, num);
					SendKeys(typeAttribute, attributeValue, content4, Convert.ToDouble(rd.Next(10, 100)) / 1000.0);
					DelayTime(rd.Next(1, 3));
					int num5 = rd.Next(1, num);
					for (int i = 0; i < num5; i++)
					{
						SendBackspace(typeAttribute, attributeValue);
						DelayTime(Convert.ToDouble(rd.Next(1000, 2000)) / 10000.0);
					}
					string text = "";
					switch (typeAttribute)
					{
					case 1:
						text = "#" + attributeValue;
						break;
					case 2:
						text = "[name=\"" + attributeValue + "\"]";
						break;
					case 4:
						text = attributeValue;
						break;
					}
					content4 = content.Substring(chromeDriver.ExecuteScript("return document.querySelector('" + text + "').value+''").ToString().Length);
					DelayTime(rd.Next(1, 3));
					SendKeys(typeAttribute, attributeValue, content4, Convert.ToDouble(rd.Next(100, 300)) / 1000.0, isClick: false);
					DelayTime(rd.Next(1, 3));
					break;
				}
				case 1:
				{
					string content2 = content.Substring(0, num);
					string content3 = content.Substring(num);
					SendKeys(typeAttribute, attributeValue, content2, Convert.ToDouble(rd.Next(10, 100)) / 1000.0);
					DelayTime(rd.Next(1, 3));
					SendKeys(typeAttribute, attributeValue, content3, Convert.ToDouble(rd.Next(100, 300)) / 1000.0, isClick: false);
					DelayTime(rd.Next(1, 3));
					break;
				}
				}
			}
			SendKeys(typeAttribute, attributeValue, content, Convert.ToDouble(rd.Next(100, 200)) / 1000.0);
			DelayTime(rd.Next(1, 3));
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendKeysv2(int typeAttribute, string attributeValue, int index, int subTypeAttribute, string subAttributeValue, int subIndex, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			if (isClick)
			{
				Click(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex);
				DelayTime(timeDelayAfterClick);
			}
			if (subTypeAttribute == 0)
			{
				switch (typeAttribute)
				{
				case 1:
					chromeDriver.FindElementsById(attributeValue)[index].SendKeys(content);
					break;
				case 2:
					chromeDriver.FindElementsByName(attributeValue)[index].SendKeys(content);
					break;
				case 3:
					chromeDriver.FindElementsByXPath(attributeValue)[index].SendKeys(content);
					break;
				case 4:
					chromeDriver.FindElementsByCssSelector(attributeValue)[index].SendKeys(content);
					break;
				}
			}
			else
			{
				switch (typeAttribute)
				{
				case 1:
					chromeDriver.FindElementsById(attributeValue)[index].FindElements(By.Id(subAttributeValue))[subIndex].SendKeys(content);
					break;
				case 2:
					chromeDriver.FindElementsByName(attributeValue)[index].FindElements(By.Name(subAttributeValue))[subIndex].SendKeys(content);
					break;
				case 3:
					chromeDriver.FindElementsByXPath(attributeValue)[index].FindElements(By.XPath(subAttributeValue))[subIndex].SendKeys(content);
					break;
				case 4:
					chromeDriver.FindElementsByCssSelector(attributeValue)[index].FindElements(By.CssSelector(subAttributeValue))[subIndex].SendKeys(content);
					break;
				}
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendKeysWithSpeed(int tocDo, int typeAttribute, string attributeValue, string content, double timeDelay_Second, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		if (!IsLive())
		{
			return -2;
		}
		int result = 0;
		switch (tocDo)
		{
		case 0:
			result = SendKeys(new Random(), typeAttribute, attributeValue, content, timeDelay_Second, isClick, timeDelayAfterClick);
			break;
		case 1:
			result = SendKeys(typeAttribute, attributeValue, content, timeDelay_Second, isClick, timeDelayAfterClick);
			break;
		case 2:
			result = SendKeys(typeAttribute, attributeValue, content, isClick, timeDelayAfterClick);
			break;
		}
		return result;
	}

	public int SendKeysWithSpeedv2(int tocDo, int typeAttribute, string attributeValue, int index, int subTypeAttribute, string subAttributeValue, int subIndex, string content, bool isClick = true, double timeDelayAfterClick = 0.1)
	{
		if (!IsLive())
		{
			return -2;
		}
		int result = 0;
		switch (tocDo)
		{
		case 0:
			result = SendKeysv2(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex, content);
			break;
		case 1:
			result = SendKeysv2(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex, content);
			break;
		case 2:
			result = SendKeysv2(typeAttribute, attributeValue, index, subTypeAttribute, subAttributeValue, subIndex, content);
			break;
		}
		return result;
	}

	public int CheckExistElement(string querySelector, double timeWait_Second = 0.0)
	{
		bool flag = true;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			int tickCount = Environment.TickCount;
			while (ExecuteScript("return document.querySelectorAll('" + querySelector.Replace("'", "\\'") + "').length+''") == "0")
			{
				if (!((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0))
				{
					if (!IsLive())
					{
						Thread.Sleep(1000);
						continue;
					}
					return -2;
				}
				flag = false;
				break;
			}
		}
		catch (Exception)
		{
			flag = false;
		}
		return flag ? 1 : 0;
	}

	public int CheckExistElements(double timeWait_Second = 0.0, params string[] querySelectors)
	{
		int index = 0;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			int tickCount = Environment.TickCount;
			while (true)
			{
				index = Convert.ToInt32(ExecuteScript("var arr='" + string.Join("|", querySelectors) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; }return (output + ''); "));
				if (index > 0)
				{
					return index;
				}
				if (index != 2)
				{
					if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
					{
						break;
					}
					DelayTime(1.0);
					continue;
				}
				return -2;
			}
		}
		catch (Exception)
		{
		}
		return index;
	}

	public async Task<string> CheckExistElements(double timeWait_Second, List<string> querySelectors)
	{
		if (CheckChromeClosed())
		{
			return "-2";
		}
		try
		{
			string attributeScript = ((!IsXPath(querySelectors[0])) ? ("var arr='" + string.Join("|", querySelectors) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; }return (output + ''); ") : ("var arr='" + string.Join("|", querySelectors) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.evaluate(arr[i], document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue != null) { output = i + 1; break;}; } return (output + '');"));
			int tickCount = Environment.TickCount;
			while (true)
			{
				int index = Convert.ToInt32(ExecuteScript(attributeScript));
				if (index > 0)
				{
					return querySelectors[index - 1];
				}
				if (index != 2)
				{
					if ((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0)
					{
						break;
					}
					await Task.Delay(1000);
					continue;
				}
				return "-2";
			}
		}
		catch
		{
		}
		return "";
	}

	public int CheckExistElements(double timeWait_Second, Dictionary<int, List<string>> dic)
	{
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			int tickCount = Environment.TickCount;
			while (true)
			{
				foreach (KeyValuePair<int, List<string>> item in dic)
				{
					if (Convert.ToInt32(chromeDriver.ExecuteScript("var arr='" + string.Join("|", item.Value) + "'.split('|');var output=0;for(i=0;i<arr.length;i++){ if (document.querySelectorAll(arr[i]).length > 0) { output = i + 1; break;}; } return (output + ''); ")) != 0)
					{
						return item.Key;
					}
				}
				if (!((double)(Environment.TickCount - tickCount) > timeWait_Second * 1000.0))
				{
					Thread.Sleep(1000);
					continue;
				}
				break;
			}
		}
		catch
		{
		}
		return 0;
	}

	public IWebElement FindElementByCustomSelector(string selector)
	{
		if (IsXPath(selector))
		{
			return chromeDriver.FindElement(By.XPath(selector));
		}
		string cssSelectorToFind = "";
		string text = "";
		int index = 0;
		int index2 = 0;
		string[] array = selector.Split('|');
		switch (array.Length)
		{
		case 1:
			cssSelectorToFind = array[0].Trim();
			break;
		case 2:
			cssSelectorToFind = array[0].Trim();
			index = Convert.ToInt32(array[1].Trim());
			break;
		case 3:
			cssSelectorToFind = array[0].Trim();
			index = Convert.ToInt32(array[1].Trim());
			text = array[2].Trim();
			break;
		case 4:
			cssSelectorToFind = array[0].Trim();
			index = Convert.ToInt32(array[1].Trim());
			text = array[2].Trim();
			index2 = Convert.ToInt32(array[3].Trim());
			break;
		}
		if (text == "")
		{
			ReadOnlyCollection<IWebElement> elements = chromeDriver.FindElements(By.CssSelector(cssSelectorToFind));
			if (elements.Count == 0)
			{
				return null;
			}
			return elements[index];
		}
		return chromeDriver.FindElements(By.CssSelector(cssSelectorToFind))[index].FindElements(By.CssSelector(text))[index2];
	}

	public IWebElement SetAttribute(IWebElement element, string name, string value)
	{
		IWebDriver driver = ((IWrapsDriver)element).WrappedDriver;
		IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
		jsExecutor.ExecuteScript("arguments[0].setAttribute(arguments[1], arguments[2]);", element, name, value);
		return element;
	}

	public string GetPageSource()
	{
		if (!IsLive())
		{
			return "-2";
		}
		try
		{
			return chromeDriver.PageSource;
		}
		catch (Exception)
		{
		}
		return "";
	}

	public string GetCssSelector(string querySelector, string attributeName, string attributeValue)
	{
		string result = "";
		if (!IsLive())
		{
			return "-2";
		}
		try
		{
			result = ExecuteScript("function GetSelector(el){let path=[],parent;while(parent=el.parentNode){path.unshift(`${el.tagName}:nth-child(${[].indexOf.call(parent.children, el)+1})`);el=parent}return `${path.join('>')}`.toLowerCase()}; function GetCssSelector(selector, attribute, value){var c = document.querySelectorAll(selector); for (i = 0; i < c.length; i++) { if (c[i].getAttribute(attribute)!=null && c[i].getAttribute(attribute).includes(value)) { return GetSelector(c[i])} }; return '';}; return GetCssSelector('" + querySelector + "','" + attributeName + "','" + attributeValue + "')").ToString();
		}
		catch (Exception)
		{
		}
		return result;
	}

	public int SendEnter(int typeAttribute, string attributeValue)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			switch (typeAttribute)
			{
			case 1:
				chromeDriver.FindElementById(attributeValue).SendKeys(OpenQA.Selenium.Keys.Enter);
				break;
			case 2:
				chromeDriver.FindElementByName(attributeValue).SendKeys(OpenQA.Selenium.Keys.Enter);
				break;
			case 3:
				chromeDriver.FindElementByXPath(attributeValue).SendKeys(OpenQA.Selenium.Keys.Enter);
				break;
			case 4:
				chromeDriver.FindElementByCssSelector(attributeValue).SendKeys(OpenQA.Selenium.Keys.Enter);
				break;
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public int SendBackspace(int typeAttribute, string attributeValue)
	{
		bool flag = false;
		if (!IsLive())
		{
			return -2;
		}
		try
		{
			switch (typeAttribute)
			{
			case 1:
				chromeDriver.FindElementById(attributeValue).SendKeys(OpenQA.Selenium.Keys.Backspace);
				break;
			case 2:
				chromeDriver.FindElementByName(attributeValue).SendKeys(OpenQA.Selenium.Keys.Backspace);
				break;
			case 3:
				chromeDriver.FindElementByXPath(attributeValue).SendKeys(OpenQA.Selenium.Keys.Backspace);
				break;
			case 4:
				chromeDriver.FindElementByCssSelector(attributeValue).SendKeys(OpenQA.Selenium.Keys.Backspace);
				break;
			}
			flag = true;
		}
		catch (Exception)
		{
		}
		return flag ? 1 : 0;
	}

	public bool ScrollAndWait(string selector, int waitSeconds = 1)
	{
		if (CheckChromeClosed())
		{
			return false;
		}
		try
		{
			WaitLoading();
			if (!ScrollElementIntoView(selector))
			{
				return false;
			}
			DelayTime(waitSeconds);
			return true;
		}
		catch (Exception)
		{
		}
		return false;
	}

	public string GetCookies(string domain = "facebook")
	{
		string text = "";
		try
		{
			Cookie[] array = chromeDriver.Manage().Cookies.AllCookies.ToArray();
			Cookie[] array2 = array;
			Cookie[] array3 = array2;
			foreach (Cookie cookie in array3)
			{
				if (cookie.Domain.Contains(domain))
				{
					text = text + cookie.Name + "=" + cookie.Value + ";";
				}
			}
		}
		catch (Exception)
		{
		}
		return text;
	}

	public bool ScrollElementIntoView(string selector)
	{
		if (CheckChromeClosed())
		{
			return false;
		}
		ExecuteScript(ToJSDom(selector) + ".scrollIntoView({ behavior: 'smooth', block: 'center'});");
		return true;
	}

	public void WaitLoading()
	{
		WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(30.0));
		Func<IWebDriver, bool> waitLoading = delegate(IWebDriver Web)
		{
			try
			{
				IWebElement webElement = Web.FindElement(By.Id("TQISoft"));
				return false;
			}
			catch
			{
				return true;
			}
		};
		try
		{
			wait.Until(waitLoading);
		}
		catch
		{
		}
	}

	public void WaitElement(By by)
	{
		WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(30.0));
		Func<IWebDriver, bool> waitLoading = delegate(IWebDriver Web)
		{
			try
			{
				IWebElement webElement = Web.FindElement(by);
				return false;
			}
			catch
			{
				return true;
			}
		};
		try
		{
			wait.Until(waitLoading);
		}
		catch
		{
		}
	}

	public int GetElementCount(string selector)
	{
		if (CheckChromeClosed())
		{
			return 0;
		}
		int result = 0;
		try
		{
			result = ((!IsXPath(selector)) ? Convert.ToInt32(chromeDriver.ExecuteScript("return document.querySelectorAll('" + selector.Replace("'", "\\'") + "').length+''").ToString()) : Convert.ToInt32(chromeDriver.ExecuteScript("return document.evaluate('count(" + selector.Replace("'", "\\'") + ")', document, null, XPathResult.NUMBER_TYPE, null).numberValue").ToString()));
		}
		catch (Exception)
		{
		}
		return result;
	}

	public string ToJSDom(string selector)
	{
		selector = ReplaceCssSelector(selector);
		return "document.querySelector('" + selector + "')";
	}

	public bool IsLive()
	{
		return !CheckChromeClosed();
	}

	public bool CheckChromeClosed()
	{
		if (Process != null)
		{
			return Process.HasExited;
		}
		if (chromeDriver == null)
		{
			return true;
		}
		bool isClose = true;
		try
		{
			_ = chromeDriver.Title;
			isClose = false;
		}
		catch (Exception)
		{
		}
		return isClose;
	}

	public bool GetProcess()
	{
		string title;
		try
		{
			title = chromeDriver.CurrentWindowHandle;
			return true;
		}
		catch
		{
			title = "TEMP_" + Guid.NewGuid().ToString("N").Substring(0, 15);
		}
		if (string.IsNullOrEmpty(title))
		{
			return false;
		}
		try
		{
			((IJavaScriptExecutor)chromeDriver).ExecuteScript("document.title = '" + title + "';", Array.Empty<object>());
		}
		catch
		{
		}
		DelayTime(0.5);
		Process = Process.GetProcessesByName("chrome").FirstOrDefault((Process p) => p.MainWindowTitle != null && p.MainWindowTitle.Contains(title));
		if (Process != null)
		{
			return true;
		}
		return false;
	}

	public string ExecuteScript(string script)
	{
		try
		{
			return chromeDriver.ExecuteScript(script).ToString();
		}
		catch
		{
		}
		return "";
	}

	private bool IsXPath(string selector)
	{
		selector = ReplaceCssSelector(selector);
		if (selector.StartsWith("("))
		{
			selector = selector.TrimStart('(');
		}
		return selector.StartsWith("/");
	}

	private string ReplaceCssSelector(string selector)
	{
		selector = selector.Replace("\\", "\\\\\\\\");
		selector = selector.Replace("'", "\\'");
		selector = selector.Replace("\n", "\\\\n");
		return selector;
	}

	public void DelayTime(double timeDelays)
	{
		timeDelays = Math.Floor(timeDelays);
		for (int i = 0; (double)i < timeDelays; i++)
		{
			Common.Sleep(1.0);
		}
	}
}
