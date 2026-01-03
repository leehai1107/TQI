using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Commons;

public static class HttpClientCommon
{
	public static void SetDefaultHeader(this HttpClient client)
	{
		((HttpHeaders)client.DefaultRequestHeaders).Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
		((HttpHeaders)client.DefaultRequestHeaders).Add("dpr", "1");
		((HttpHeaders)client.DefaultRequestHeaders).Add("priority", "u=0, i");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-ch-prefers-color-scheme", "dark");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-ch-ua-mobile", "?0");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-ch-ua-model", "");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-ch-ua-platform", "\"Windows\"");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-ch-ua-platform-version", "\"19.0.0\"");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-fetch-site", "same-origin");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-fetch-mode", "navigate");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-fetch-user", "?1");
		((HttpHeaders)client.DefaultRequestHeaders).Add("upgrade-insecure-requests", "1");
		((HttpHeaders)client.DefaultRequestHeaders).Add("sec-fetch-dest", "empty");
		((HttpHeaders)client.DefaultRequestHeaders).Add("cache-control", "max-age=0");
		((HttpHeaders)client.DefaultRequestHeaders).Add("Accept-Language", "en-GB,en;q=0.9,en-US;q=0.8");
	}

	public static async Task<HttpResponseMessage> SendRequest(this HttpClient httpClient, HttpRequestMessage requestMessage, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await ((HttpMessageInvoker)httpClient).SendAsync(requestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static async Task<HttpResponseMessage> SendRequestWithRetry(this HttpClient httpClient, HttpRequestMessage requestMessage, AccountDto account, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await ExecuteWithRetry<HttpResponseMessage>(async delegate
		{
			HttpResponseMessage response = await ((HttpMessageInvoker)httpClient).SendAsync(requestMessage, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (response.IsSuccessStatusCode)
			{
				if (response.IsAutomationCheckpoint())
				{
					MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
					string contentType2 = ((contentType != null) ? contentType.CharSet : null);
					if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
					{
						response.Content.Headers.ContentType.CharSet = "utf-8";
					}
					await ResolveCheckpointAutomate(responseStr: await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), client: httpClient, account: account).ConfigureAwait(continueOnCapturedContext: false);
					return await httpClient.SendRequestWithRetry(requestMessage, account, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				return response;
			}
			throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
		}, 3, 200, delegate(Exception ex, int attempt)
		{
			Console.WriteLine($"Lần thử thứ {attempt} thất bại. Lỗi: {ex.Message}");
		}, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static (HttpClient, CookieContainer) CreateHttpClient(string cookies = "", string proxyStr = "", string useragent = "", string domain = "facebook.com")
	{
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		CookieContainer cookieContainer = new CookieContainer();
		WebProxy proxy = null;
		if (!string.IsNullOrEmpty(proxyStr))
		{
			string[] proxyRaw = proxyStr.Split(':');
			proxy = ((proxyRaw.Length >= 3) ? new WebProxy
			{
				Address = new Uri("http://" + proxyRaw[0] + ":" + proxyRaw[1]),
				BypassProxyOnLocal = false,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(proxyRaw[2], proxyRaw[3])
			} : new WebProxy
			{
				Address = new Uri("http://" + proxyRaw[0] + ":" + proxyRaw[1]),
				BypassProxyOnLocal = false,
				UseDefaultCredentials = false
			});
		}
		if (!string.IsNullOrEmpty(cookies))
		{
			string[] cookieJar = cookies.Split(';');
			string[] array = cookieJar;
			foreach (string cookie in array)
			{
				string[] cookieItem = cookie.Split('=');
				if (cookieItem.Length >= 2)
				{
					try
					{
						cookieContainer.Add(new Cookie(cookieItem[0].Trim(), cookieItem[1].Trim(), "/", domain));
					}
					catch
					{
					}
				}
			}
			cookieContainer.Add(new Cookie("dpr", "1", "/", ".facebook.com"));
			cookieContainer.Add(new Cookie("m_pixel_ratio", "0.8", "/", ".facebook.com"));
			cookieContainer.Add(new Cookie("locale", "en_SG", "/", ".facebook.com"));
		}
		HttpClientHandler clientHandler = new HttpClientHandler
		{
			CookieContainer = cookieContainer,
			UseCookies = true,
			UseDefaultCredentials = false,
			AllowAutoRedirect = true,
			AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate),
			SslProtocols = (SslProtocols.Tls12 | SslProtocols.Tls13),
			ServerCertificateCustomValidationCallback = (HttpRequestMessage sender, X509Certificate2 cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true
		};
		if (proxy != null)
		{
			clientHandler.Proxy = proxy;
			clientHandler.UseProxy = true;
		}
		HttpClient client = new HttpClient((HttpMessageHandler)(object)clientHandler);
		client.Timeout = TimeSpan.FromSeconds(60.0);
		if (!string.IsNullOrEmpty(useragent))
		{
			client.DefaultRequestHeaders.UserAgent.ParseAdd(useragent);
		}
		client.SetDefaultHeader();
		return (client, cookieContainer);
	}

	public static Task<string> CookieToString(this CookieContainer cookieContainer, string domain = "https://www.facebook.com/")
	{
		return Task.Run(delegate
		{
			CookieCollection cookies = cookieContainer.GetCookies(new Uri(domain));
			string text = string.Empty;
			foreach (Cookie cookie in cookies)
			{
				text = text + cookie.Name + "=" + cookie.Value + "; ";
			}
			return text;
		});
	}

	public static async Task<HttpResponseMessage> PostWithRetry(this HttpClient client, string url, HttpContent content, AccountDto account, int maxRetries = 3, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await ExecuteWithRetry<HttpResponseMessage>(async delegate
		{
			HttpResponseMessage response = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (response.IsSuccessStatusCode)
			{
				if (response.IsAutomationCheckpoint())
				{
					MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
					string contentType2 = ((contentType != null) ? contentType.CharSet : null);
					if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
					{
						response.Content.Headers.ContentType.CharSet = "utf-8";
					}
					await ResolveCheckpointAutomate(responseStr: await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), client: client, account: account).ConfigureAwait(continueOnCapturedContext: false);
					return await client.PostWithRetry(url, content, account, maxRetries, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				return response;
			}
			if (response.StatusCode == HttpStatusCode.Found)
			{
				return response;
			}
			throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
		}, maxRetries, 200, delegate(Exception ex, int attempt)
		{
			Console.WriteLine($"Lần thử thứ {attempt} thất bại. Lỗi: {ex.Message}");
		}, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static async Task<HttpResponseMessage> GetWithRetry(this HttpClient client, string url, AccountDto account, int maxRetries = 3, CancellationToken cancellationToken = default(CancellationToken))
	{
		return await ExecuteWithRetry<HttpResponseMessage>(async delegate
		{
			HttpResponseMessage response = await client.GetAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (response.IsSuccessStatusCode)
			{
				if (response.IsAutomationCheckpoint())
				{
					MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
					string contentType2 = ((contentType != null) ? contentType.CharSet : null);
					if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
					{
						response.Content.Headers.ContentType.CharSet = "utf-8";
					}
					await ResolveCheckpointAutomate(responseStr: await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), client: client, account: account).ConfigureAwait(continueOnCapturedContext: false);
					return await client.GetAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
				}
				return response;
			}
			if (response.StatusCode == HttpStatusCode.Found)
			{
				return response;
			}
			response = await client.GetAsync("https://www.facebook.com/me", cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (response.IsSuccessStatusCode)
			{
				if (response.IsAutomationCheckpoint())
				{
					MediaTypeHeaderValue contentType3 = response.Content.Headers.ContentType;
					string contentType4 = ((contentType3 != null) ? contentType3.CharSet : null);
					if (!string.IsNullOrEmpty(contentType4) && contentType4.Contains("\"utf-8\""))
					{
						response.Content.Headers.ContentType.CharSet = "utf-8";
					}
					await ResolveCheckpointAutomate(responseStr: await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false), client: client, account: account).ConfigureAwait(continueOnCapturedContext: false);
				}
				return await client.GetAsync(url, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
			throw new HttpRequestException($"Request failed with status code: {response.StatusCode}");
		}, maxRetries, 200, delegate(Exception ex, int attempt)
		{
			Console.WriteLine($"Lần thử thứ {attempt} thất bại. Lỗi: {ex.Message}");
		}, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static bool IsAutomationCheckpoint(this HttpResponseMessage response)
	{
		MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
		string contentType2 = ((contentType != null) ? contentType.CharSet : null);
		if (!string.IsNullOrEmpty(contentType2) && contentType2.Contains("\"utf-8\""))
		{
			response.Content.Headers.ContentType.CharSet = "utf-8";
		}
		string responseStr = ((response.Content != null) ? response.Content.ReadAsStringAsync().GetAwaiter().GetResult() : string.Empty);
		if (response.RequestMessage.RequestUri.AbsolutePath == "/checkpoint/601051028565049/" || (!string.IsNullOrEmpty(responseStr) && responseStr.Contains("601051028565049")))
		{
			return true;
		}
		return false;
	}

	public static async Task ResolveCheckpointAutomate(HttpClient client, string responseStr, AccountDto account)
	{
		if (string.IsNullOrEmpty(account.DTSGToken))
		{
			account.DTSGToken = Regex.Match(responseStr, "DTSGInitialData\",\\[],{\"token\":\"(.*?)\"").Groups[1].Value;
		}
		Dictionary<string, string> payload = new Dictionary<string, string>
		{
			{ "av", account.Uid },
			{ "__user", account.Uid },
			{ "__a", "1" },
			{ "__req", "5" },
			{ "__hs", "20025.HYP:comet_pkg.2.1..2.1" },
			{ "dpr", "1" },
			{ "__ccg", "EXCELLENT" },
			{ "__rev", "1017759046" },
			{ "__s", "mwvlhs:4ve1ex:dagn50" },
			{ "__hsi", "7431233835583946046" },
			{ "__dyn", "7xeUmwlEnwn8K2Wmh0no6u5U4e0yoW3q32360CEbo19oe8hw2nVE4W099w8G1Dz81s8hwnU2lwv89k2C1Fwc60D8vwRwlE-U2zxe2GewbS361qw8Xwn82Lx-0lK3qazo720Bo2ZwrU6C0hq1Iwqo35wvodo7u2-2K0UE" },
			{ "__csr", "iNCvuV4h4DGUHGXUSeZ4DjrwzyGyJbzpUO2G59E4-exaU4vwbu4o1ao4i0nS2u0gi2W1Jw3wE4m0EU1eU0zK00oeC034S00XgE" },
			{ "__comet_req", "15" },
			{ "fb_dtsg", account.DTSGToken },
			{ "jazoest", "25401" },
			{ "lsd", "_SUj0S8K_WcCtiwQFwev4Y" },
			{ "__spin_r", "1017759046" },
			{ "__spin_b", "trunk" },
			{ "__spin_t", "1730218957" },
			{ "fb_api_caller_class", "RelayModern" },
			{ "fb_api_req_friendly_name", "FBScrapingWarningMutation" },
			{ "variables", "{}" },
			{ "server_timestamps", "true" },
			{ "doc_id", "6339492849481770" }
		};
		await client.PostAsync("https://www.facebook.com/api/graphql/", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload)).ConfigureAwait(continueOnCapturedContext: false);
	}

	private static async Task<T> ExecuteWithRetry<T>(Func<Task<T>> action, int maxRetries = 2, int delayMilliseconds = 1000, Action<Exception, int> onRetry = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		int attempt = 0;
		Exception lastException = null;
		while (attempt < maxRetries)
		{
			cancellationToken.ThrowIfCancellationRequested();
			try
			{
				return await action().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (SocketException ex)
			{
				throw ex;
			}
			catch (Exception ex2)
			{
				Exception ex3 = ex2;
				lastException = ex3;
				int num = attempt;
				attempt = num + 1;
				try
				{
					if (onRetry != null)
					{
						await Task.Run(delegate
						{
							onRetry(ex3, attempt);
						}, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					}
				}
				catch (Exception ex4)
				{
					Exception retryEx = ex4;
					Console.WriteLine("Lỗi trong onRetry: " + retryEx.Message);
				}
				if (attempt >= maxRetries)
				{
					throw new InvalidOperationException($"Thử lại thất bại sau {maxRetries} lần.", ex3);
				}
				await Task.Delay(delayMilliseconds, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		throw lastException;
	}
}
