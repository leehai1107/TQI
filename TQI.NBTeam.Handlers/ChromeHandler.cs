using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using TQI.NBTeam.Models;
using TQI.NBTeam.Services;

namespace TQI.NBTeam.Handlers;

public class ChromeHandler
{
	private ChromeService _chromeService;

	private AccountDto _accountDto;

	public ChromeHandler(ChromeService chromeService, AccountDto accountDto)
	{
		_chromeService = chromeService;
		_accountDto = accountDto;
	}

	public bool LoginFacebook()
	{
		try
		{
			_chromeService.GotoURL("https://www.facebook.com/login/");
			if (string.IsNullOrEmpty(_accountDto.Cookie))
			{
				return false;
			}
			if (_chromeService?.chromeDriver == null)
			{
				return false;
			}
			_chromeService.chromeDriver.Manage().Cookies.DeleteAllCookies();
			string[] cookieJar = _accountDto.Cookie.Split(';');
			string[] array = cookieJar;
			foreach (string cookie in array)
			{
				try
				{
					string[] cookieItem = cookie.Split('=');
					if (cookieItem.Length >= 2)
					{
						_chromeService.AddSimpleCookie(cookieItem[0].Trim(), cookieItem[1].Trim(), ".facebook.com");
					}
				}
				catch
				{
				}
			}
			_chromeService.RefreshWebPage();
			return true;
		}
		catch
		{
		}
		return false;
	}

	public bool IsLive()
	{
		if (_chromeService == null || !_chromeService.GetProcess())
		{
			return false;
		}
		return true;
	}

	public bool OpenNewTab(string url, bool switchToLastTab)
	{
		return _chromeService.OpenNewTab(url, switchToLastTab) == 1;
	}

	public void RemoveBlock()
	{
		ReadOnlyCollection<IWebElement> verificationBlockElements = _chromeService.chromeDriver.FindElements(By.XPath("//span[@data-surface='/bizweb:business_users']"));
		if (verificationBlockElements.Count > 1)
		{
			((IJavaScriptExecutor)_chromeService.chromeDriver).ExecuteScript("arguments[0].remove();", new object[1] { verificationBlockElements[1] });
		}
	}

	public bool CheckLoginSuccess(string currentUrl = "", string html = "")
	{
		if (currentUrl == "")
		{
			currentUrl = _chromeService.GetURL();
		}
		List<string> lstKerword = new List<string> { "facebook.com/home.php" };
		if (CheckStringContainKeyword(currentUrl, lstKerword))
		{
			return true;
		}
		if (html == "")
		{
			html = _chromeService.GetPageSource();
		}
		List<string> lstKerword2 = new List<string> { "/friends/", "/logout.php?button_location=settings&amp;button_name=logout" };
		if (CheckStringContainKeyword(html, lstKerword2))
		{
			return true;
		}
		List<string> list = new List<string> { "a[href*=\"/friends/\"]", "[action=\"/logout.php?button_location=settings&button_name=logout\"]" };
		if (_chromeService.CheckExistElements(0.0, list.ToArray()) > 0)
		{
			return true;
		}
		return false;
	}

	private bool CheckStringContainKeyword(string content, List<string> lstKerword)
	{
		int num = 0;
		while (true)
		{
			if (num < lstKerword.Count)
			{
				if (Regex.IsMatch(content, lstKerword[num]) || content.Contains(lstKerword[num]))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}
}
