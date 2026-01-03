using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TQI.NBTeam.Commons;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Services;

public class InstagramService
{
	public enum StatusCookieIG
	{
		Live,
		Die,
		Checkpoint,
		Error
	}

	public enum StatusLoginIG
	{
		Success,
		Failed,
		Checkpoint,
		Error
	}

	private HttpClient _client;

	private CookieContainer _cookieContainer;

	private InstagramAccountDto _accountDto;

	private string _userAgent;

	public const string baseUrl = "https://www.instagram.com";

	public InstagramService(InstagramAccountDto accountDto)
	{
		_accountDto = accountDto;
		_userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36";
	}

	private void InitializeHttpClient(string proxy, int typeLogin = 0)
	{
		if (typeLogin == 0)
		{
			string cookie = _accountDto.Cookie;
			string userAgent = _userAgent;
			(_client, _cookieContainer) = HttpClientCommon.CreateHttpClient(cookie, proxy, userAgent, ".instagram.com");
		}
		else
		{
			string userAgent = _userAgent;
			(_client, _cookieContainer) = HttpClientCommon.CreateHttpClient("", proxy, userAgent, ".instagram.com");
		}
	}

	private async Task<StatusCookieIG> CheckStatusCookie()
	{
		HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://www.instagram.com/api/v1/accounts/edit/web_form_data/");
		((HttpHeaders)requestMessage.Headers).Add("X-Ig-App-Id", "936619743392459");
		HttpResponseMessage response = await _client.SendAsync(requestMessage);
		if (!response.IsSuccessStatusCode)
		{
			return StatusCookieIG.Error;
		}
		MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
		string contentType2 = ((contentType != null) ? contentType.CharSet : null);
		if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
		{
			response.Content.Headers.ContentType.CharSet = "utf-8";
		}
		string responseStr = await response.Content.ReadAsStringAsync();
		if (response.RequestMessage.RequestUri.AbsolutePath.Contains("/login/"))
		{
			return StatusCookieIG.Die;
		}
		if (responseStr.Contains("trusted_username"))
		{
			response = await _client.GetAsync("https://www.instagram.com");
			MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
			contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
			if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
			{
				response.Content.Headers.ContentType.CharSet = "utf-8";
			}
			string dtsgToken = Regex.Match(await response.Content.ReadAsStringAsync(), "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
			if (!string.IsNullOrEmpty(dtsgToken))
			{
				_accountDto.DTSGToken = dtsgToken;
			}
			return StatusCookieIG.Live;
		}
		return StatusCookieIG.Checkpoint;
	}

	public async Task<StatusLoginIG> LoginInstagram(int typeLogin = 0)
	{
		_accountDto.DTSGToken = string.Empty;
		_accountDto.Token = string.Empty;
		InitializeHttpClient("");
		if (typeLogin == 0)
		{
			return await CheckStatusCookie() switch
			{
				StatusCookieIG.Live => StatusLoginIG.Success, 
				StatusCookieIG.Die => StatusLoginIG.Failed, 
				StatusCookieIG.Error => StatusLoginIG.Error, 
				StatusCookieIG.Checkpoint => StatusLoginIG.Checkpoint, 
				_ => throw new NotImplementedException(), 
			};
		}
		throw new NotImplementedException();
	}

	public async Task ConvertAccount()
	{
		Dictionary<string, string> payload = new Dictionary<string, string>
		{
			{ "category_id", "2214" },
			{ "create_business_id", "true" },
			{ "entry_point", "ig_web_settings" },
			{ "set_public", "true" },
			{ "should_bypass_contact_check", "true" },
			{ "should_show_category", "0" },
			{ "to_account_type", "2" }
		};
		_accountDto.CsrfToken = Regex.Match(_accountDto.Cookie, "csrftoken=(.*?);").Groups[1].Value;
		HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.instagram.com/api/v1/business/account/convert_account/");
		((HttpHeaders)requestMessage.Headers).Add("X-CSRFToken", _accountDto.CsrfToken);
		requestMessage.Content = (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload);
		HttpResponseMessage response = await _client.SendAsync(requestMessage);
		if (response.IsSuccessStatusCode)
		{
			MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
			string contentType2 = ((contentType != null) ? contentType.CharSet : null);
			if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
			{
				response.Content.Headers.ContentType.CharSet = "utf-8";
			}
			await response.Content.ReadAsStringAsync();
		}
	}

	public async Task<bool> CheckKich()
	{
		HttpResponseMessage response = await _client.GetAsync("https://www.instagram.com/ad_tools/?");
		MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
		string contentType2 = ((contentType != null) ? contentType.CharSet : null);
		if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
		{
			response.Content.Headers.ContentType.CharSet = "utf-8";
		}
		string responseStr = await response.Content.ReadAsStringAsync();
		_ = Regex.Match(responseStr, "\"versioningID\":\"(.*?)\"").Groups[1].Value;
		string lsdToken = Regex.Match(responseStr, "LSD\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
		StringContent content = new StringContent("fb_dtsg=" + _accountDto.DTSGToken + "&jazoest=26161&lsd=-" + lsdToken + "&params={\\\"params\\\":\\\"{\\\\\\\"server_params\\\\\\\":{\\\\\\\"entry_point\\\\\\\":\\\\\\\"ads_manager_suggested_post\\\\\\\",\\\\\\\"flow\\\\\\\":\\\\\\\"pro2pro_framework_boost_on_web_flow\\\\\\\",\\\\\\\"INTERNAL__latency_qpl_marker_id\\\\\\\":36707139,\\\\\\\"INTERNAL__latency_qpl_instance_id\\\\\\\":\\\\\\\"157747658400040\\\\\\\"}}\\\"}", (Encoding)null, "application/x-www-form-urlencoded");
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://www.instagram.com/async/wbloks/fetch/?appid=com.bloks.www.pro_to_pro.framework.boost.async.controller.igba_mutation&type=action&__bkv=fb51dcf4bf4da3cf8bfc28f96ee156f67b02f1fc66bc2885d40cd36dcaf75a04");
		request.Content = (HttpContent)(object)content;
		((HttpHeaders)request.Headers).Add("accept", "*/*");
		((HttpHeaders)request.Headers).Add("accept-language", "en-US,en;q=0.9");
		((HttpContent)content).Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
		((HttpContent)content).Headers.ContentType.CharSet = "UTF-8";
		((HttpHeaders)request.Headers).Add("origin", "https://www.instagram.com");
		((HttpHeaders)request.Headers).Add("priority", "u=1, i");
		((HttpHeaders)request.Headers).Add("referer", "https://www.instagram.com/ad_tools/?context=main_navigation");
		((HttpHeaders)request.Headers).Add("sec-ch-prefers-color-scheme", "dark");
		((HttpHeaders)request.Headers).Add("sec-ch-ua", "\"Google Chrome\";v=\"143\", \"Chromium\";v=\"143\", \"Not A(Brand\";v=\"24\"");
		((HttpHeaders)request.Headers).Add("sec-ch-ua-full-version-list", "");
		((HttpHeaders)request.Headers).Add("sec-ch-ua-mobile", "?0");
		((HttpHeaders)request.Headers).Add("sec-ch-ua-model", "\"\"");
		((HttpHeaders)request.Headers).Add("sec-ch-ua-platform", "\"Windows\"");
		((HttpHeaders)request.Headers).Add("sec-ch-ua-platform-version", "\"\"");
		((HttpHeaders)request.Headers).Add("sec-fetch-dest", "empty");
		((HttpHeaders)request.Headers).Add("sec-fetch-mode", "cors");
		((HttpHeaders)request.Headers).Add("sec-fetch-site", "same-origin");
		response = await _client.SendAsync(request);
		MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
		contentType2 = ((contentType3 != null) ? contentType3.CharSet : null);
		if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
		{
			response.Content.Headers.ContentType.CharSet = "utf-8";
		}
		responseStr = await response.Content.ReadAsStringAsync();
		if (responseStr.Contains("<title>Error</title>"))
		{
			return false;
		}
		if (responseStr.Contains("Without a Facebook Ad Account"))
		{
			return true;
		}
		return false;
	}
}
