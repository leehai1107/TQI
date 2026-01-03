using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using TQI.NBTeam.Commons;

namespace TQI.NBTeam.Helpers;

public class MailHelper
{
	public HttpClient _httpClient;

	public MailHelper()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		_httpClient = new HttpClient();
	}

	public static async Task<string> GetLinkFViaInboxes(string username)
	{
		HttpClient httpClient = HttpClientCommon.CreateHttpClient("", "", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36").Item1;
		HttpResponseMessage response = await httpClient.GetAsync("https://fviainboxes.com/");
		if (!response.IsSuccessStatusCode)
		{
			return null;
		}
		MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
		string contentType2 = ((contentType != null) ? contentType.CharSet : null);
		if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
		{
			response.Content.Headers.ContentType.CharSet = "utf-8";
		}
		string linkGet = Regex.Match(await response.Content.ReadAsStringAsync(), " src=\"(.*?)\">").Groups[1].Value;
		response = await httpClient.GetAsync("https://fviainboxes.com/" + linkGet);
		if (!response.IsSuccessStatusCode)
		{
			return null;
		}
		MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
		contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
		if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
		{
			response.Content.Headers.ContentType.CharSet = "utf-8";
		}
		string author = Regex.Match(await response.Content.ReadAsStringAsync(), "headers:{Authorization:\"(.*?)\"}").Groups[1].Value;
		int countTime = 3;
		while (countTime-- > 0)
		{
			HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, new Uri("https://fviainboxes.com/messages?username=" + username + "&domain=fviainboxes.com"));
			((HttpHeaders)requestMessage.Headers).Add("Accept", "application/json, text/plain, */*");
			((HttpHeaders)requestMessage.Headers).Add("sec-fetch-site", "same-site");
			((HttpHeaders)requestMessage.Headers).Add("sec-fetch-dest", "empty");
			((HttpHeaders)requestMessage.Headers).Add("accept-language", "en-US,en;q=0.9");
			((HttpHeaders)requestMessage.Headers).Add("sec-fetch-mode", "cors");
			((HttpHeaders)requestMessage.Headers).Add("referer", "https://fviainboxes.com/");
			((HttpHeaders)requestMessage.Headers).Add("authorization", author);
			response = await httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			MediaTypeHeaderValue contentType4 = response.Content.Headers.ContentType;
			contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
			if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
			{
				response.Content.Headers.ContentType.CharSet = "utf-8";
			}
			MatchCollection listMail = Regex.Matches(await response.Content.ReadAsStringAsync(), "id\":\"(.*?)\",\"subject\":\"(.*?)\",\"from\":\"(.*?)\",\"seenAt\":0");
			foreach (Match match in listMail)
			{
				string idMail = match.Groups[1].Value;
				string from = match.Groups[3].Value;
				if (from.Contains("facebook"))
				{
					HttpRequestMessage requestMessage2 = new HttpRequestMessage(HttpMethod.Get, new Uri("https://fviainboxes.com/message?username=" + username + "&domain=fviainboxes.com&id=" + idMail));
					((HttpHeaders)requestMessage2.Headers).Add("Accept", "application/json, text/plain, */*");
					((HttpHeaders)requestMessage2.Headers).Add("sec-fetch-site", "same-site");
					((HttpHeaders)requestMessage2.Headers).Add("sec-fetch-dest", "empty");
					((HttpHeaders)requestMessage2.Headers).Add("accept-language", "en-US,en;q=0.9");
					((HttpHeaders)requestMessage2.Headers).Add("sec-fetch-mode", "cors");
					((HttpHeaders)requestMessage2.Headers).Add("referer", "https://fviainboxes.com/");
					((HttpHeaders)requestMessage2.Headers).Add("authorization", author);
					HttpResponseMessage responseGetLink = await httpClient.SendRequest(requestMessage2);
					if (!responseGetLink.IsSuccessStatusCode)
					{
						return null;
					}
					MediaTypeHeaderValue contentType5 = responseGetLink.Content.Headers.ContentType;
					contentType2 = ((contentType5 != null) ? contentType5.CharSet : null);
					if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
					{
						responseGetLink.Content.Headers.ContentType.CharSet = "utf-8";
					}
					string linkInviteBM = GetLinkBM(await responseGetLink.Content.ReadAsStringAsync());
					if (!string.IsNullOrEmpty(linkInviteBM))
					{
						return linkInviteBM;
					}
					await Task.Delay(1000);
					countTime--;
				}
			}
		}
		return "";
	}

	public static async Task<string> GetLinkMailNgon(string username)
	{
		HttpClient httpClient = HttpClientCommon.CreateHttpClient("", "", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36").Item1;
		string link = string.Empty;
		int retry = 3;
		do
		{
			HttpResponseMessage response = await httpClient.GetAsync("https://mailngon.top/checkmail.php?mail=" + username + "@mailngon.top&latest_id=0");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
			JObject responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
			JArray emailJArr = responseObject["emails"].ToObject<JArray>();
			if (emailJArr.Count != 0)
			{
				string messageId = emailJArr.FirstOrDefault()["id"].ToString();
				response = await httpClient.GetAsync("https://mailngon.top/viewmail.php?id=" + messageId);
				if (!response.IsSuccessStatusCode)
				{
					return null;
				}
				responseObject = JObject.Parse(await response.Content.ReadAsStringAsync());
				string body = responseObject["email"]["body"].ToString();
				link = GetLinkBM(body);
			}
		}
		while (string.IsNullOrEmpty(link) && retry-- > 0);
		return link;
	}

	public async Task<string> GetMailMoakt()
	{
		try
		{
			Dictionary<string, string> payload = new Dictionary<string, string>
			{
				{ "domain", "teml.net" },
				{ "username", "" },
				{ "random", "Get a Random Address" },
				{ "preferred_domain", "" }
			};
			HttpResponseMessage response = await _httpClient.PostAsync("https://www.moakt.com/en/inbox", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
			MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
			string contentType2 = ((contentType != null) ? contentType.CharSet : null);
			if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
			{
				response.Content.Headers.ContentType.CharSet = "utf-8";
			}
			string responseStr = await response.Content.ReadAsStringAsync();
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(responseStr);
			string email = document.DocumentNode.SelectSingleNode("//div[@id='email-address']").InnerText;
			if (!string.IsNullOrEmpty(email))
			{
				return email;
			}
		}
		catch
		{
		}
		return null;
	}

	public async Task ChangeMailMoakt(string username)
	{
		Dictionary<string, string> payload = new Dictionary<string, string> { { "username", username } };
		await _httpClient.PostAsync("https://moakt.com/en/inbox/change", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload));
	}

	public async Task<string> ReadInboxMoakt()
	{
		try
		{
			HttpResponseMessage response = await _httpClient.GetAsync("https://www.moakt.com/en/inbox");
			MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
			string contentType2 = ((contentType != null) ? contentType.CharSet : null);
			if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
			{
				response.Content.Headers.ContentType.CharSet = "utf-8";
			}
			string responseStr = await response.Content.ReadAsStringAsync();
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(responseStr);
			HtmlNodeCollection mailMessages = document.DocumentNode.SelectNodes("//div[@id='email_message_list']/div/table/tr");
			string link = string.Empty;
			foreach (HtmlNode mailMessage in mailMessages.Skip(1))
			{
				HtmlNodeCollection sub = mailMessage.SelectNodes("./td/a");
				if (sub == null)
				{
					continue;
				}
				string title = mailMessage.SelectSingleNode("./td/a").InnerText;
				string senders = mailMessage.SelectSingleNode("./td/span").InnerText;
				if (!title.Contains("invite") && !senders.Contains("Facebook") && !senders.Contains("Meta for Business") && !title.Contains("Meta Business Suite"))
				{
					continue;
				}
				string subUrl = mailMessage.SelectSingleNode("./td/a").Attributes["href"].Value;
				HttpResponseMessage responseMessage = await _httpClient.GetAsync("https://www.moakt.com" + subUrl + "/content");
				if (responseMessage.IsSuccessStatusCode)
				{
					MediaTypeHeaderValue contentType3 = responseMessage.Content.Headers.ContentType;
					contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
					if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
					{
						responseMessage.Content.Headers.ContentType.CharSet = "utf-8";
					}
					link = GetLinkBM(await responseMessage.Content.ReadAsStringAsync());
				}
			}
			return link;
		}
		catch
		{
		}
		return null;
	}

	private static string GetLinkBM(string body)
	{
		return string.IsNullOrEmpty(Regex.Match(body, "(https://fb.me/(.+?))\"").Groups[1].Value) ? Regex.Match(body, "(https://business.facebook.com/invitation/?(.+?))\"").Groups[1].Value : Regex.Match(body, "(https://fb.me/(.+?))\"").Groups[1].Value;
	}

	public async Task<string> GetCodeIG()
	{
		try
		{
			HttpResponseMessage response = await _httpClient.GetAsync("https://www.moakt.com/en/inbox");
			MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
			string contentType2 = ((contentType != null) ? contentType.CharSet : null);
			if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
			{
				response.Content.Headers.ContentType.CharSet = "utf-8";
			}
			string responseStr = await response.Content.ReadAsStringAsync();
			HtmlDocument document = new HtmlDocument();
			document.LoadHtml(responseStr);
			HtmlNodeCollection mailMessages;
			for (mailMessages = document.DocumentNode.SelectNodes("//div[@id='email_message_list']/div/table/tr"); mailMessages == null; mailMessages = document.DocumentNode.SelectNodes("//div[@id='email_message_list']/div/table/tr"))
			{
				response = await _httpClient.GetAsync("https://www.moakt.com/en/inbox");
				MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
				contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
				if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
				{
					response.Content.Headers.ContentType.CharSet = "utf-8";
				}
				responseStr = await response.Content.ReadAsStringAsync();
				document = new HtmlDocument();
				document.LoadHtml(responseStr);
			}
			string link = string.Empty;
			foreach (HtmlNode mailMessage in mailMessages.Skip(1))
			{
				HtmlNodeCollection sub = mailMessage.SelectNodes("./td/a");
				if (sub == null)
				{
					continue;
				}
				_ = mailMessage.SelectSingleNode("./td/a").InnerText;
				string senders = mailMessage.SelectSingleNode("./td/span").InnerText;
				if (!senders.Contains("Instagram"))
				{
					continue;
				}
				string subUrl = mailMessage.SelectSingleNode("./td/a").Attributes["href"].Value;
				HttpResponseMessage responseMessage = await _httpClient.GetAsync("https://www.moakt.com" + subUrl + "/content");
				if (responseMessage.IsSuccessStatusCode)
				{
					MediaTypeHeaderValue contentType4 = responseMessage.Content.Headers.ContentType;
					contentType2 = ((contentType4 != null) ? contentType4.CharSet : null);
					if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
					{
						responseMessage.Content.Headers.ContentType.CharSet = "utf-8";
					}
					link = Regex.Match(await responseMessage.Content.ReadAsStringAsync(), "(?<=>)\\d{6}(?=<)").Value;
				}
			}
			return link;
		}
		catch
		{
		}
		return null;
	}
}
