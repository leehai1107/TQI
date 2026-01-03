using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using Microsoft.Graph;
using MimeKit;
using Newtonsoft.Json;
using TQI.NBTeam.Commons;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Helpers;

public class HotmailHelper
{
	public const string Server = "outlook.office365.com";

	public const string UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 16_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.4 Mobile/15E148 Safari/604.1";

	public HotmailDto HotmailDto { get; }

	private HttpClient _client { get; set; }

	private CookieContainer _cookieContainer { get; set; }

	public HotmailHelper(HotmailDto hotmailDto)
	{
		HotmailDto = hotmailDto;
		(_client, _cookieContainer) = HttpClientCommon.CreateHttpClient("", "", "Mozilla/5.0 (iPhone; CPU iPhone OS 16_4_1 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.4 Mobile/15E148 Safari/604.1");
	}

	public async Task<List<MailMessageDto>> ReadMailMessageOAuth2Ver2()
	{
		string accessToken = await GetAccessToken();
		if (string.IsNullOrEmpty(accessToken))
		{
			return null;
		}
		DelegateAuthenticationProvider authProvider = new DelegateAuthenticationProvider(async delegate(HttpRequestMessage request)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		});
		GraphServiceClient graphClient = new GraphServiceClient((IAuthenticationProvider)authProvider, (IHttpProvider)null);
		IUserMessagesCollectionPage messages = await graphClient.Me.Messages.Request().Top(10).GetAsync();
		List<MailMessageDto> mailmessages = new List<MailMessageDto>();
		foreach (Message msg in messages)
		{
			string bodyHtml = msg.Body.Content;
			Recipient sender = msg.Sender;
			string subject = msg.Subject;
			MailMessageDto mailMessage = new MailMessageDto
			{
				Sender = sender.EmailAddress.Address,
				Message = bodyHtml,
				Subject = subject,
				ReceiveTime = msg.ReceivedDateTime.Value.ToString("dd/MM/yyyy HH:mm:ss"),
				Code = Regex.Match(bodyHtml, "\\d{6,8}").Value
			};
			mailmessages.Add(mailMessage);
		}
		return mailmessages;
	}

	public async Task<int> DeleteAllMessagesAsync(string folderId = "Inbox")
	{
		string accessToken = await GetAccessToken();
		if (string.IsNullOrEmpty(accessToken))
		{
			return -1;
		}
		DelegateAuthenticationProvider authProvider = new DelegateAuthenticationProvider(async delegate(HttpRequestMessage request)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		});
		GraphServiceClient graphClient = new GraphServiceClient((IAuthenticationProvider)authProvider, (IHttpProvider)null);
		int deleted = 0;
		int top = 0;
		IMailFolderMessagesCollectionPage messages = await graphClient.Me.MailFolders[folderId].Messages.Request().Top(top).Select("id")
			.GetAsync();
		int messagesCount = messages.Count;
		List<Task> tasks = new List<Task>();
		int i;
		for (i = 0; i < messagesCount; i++)
		{
			tasks.Add(Task.Run(async delegate
			{
				Message message = messages[i];
				try
				{
					await graphClient.Me.Messages[message.Id].Request().DeleteAsync();
					int num = deleted;
					deleted = num + 1;
				}
				catch
				{
				}
			}));
		}
		return deleted;
	}

	public async Task<List<MailMessageDto>> GetLinkInvite()
	{
		string accessToken = await GetAccessToken2();
		if (string.IsNullOrEmpty(accessToken))
		{
			return null;
		}
		DelegateAuthenticationProvider authProvider = new DelegateAuthenticationProvider(async delegate(HttpRequestMessage request)
		{
			request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
		});
		GraphServiceClient graphClient = new GraphServiceClient((IAuthenticationProvider)authProvider, (IHttpProvider)null);
		int top = 0;
		List<MailMessageDto> mailmessages = new List<MailMessageDto>();
		IUserMessagesCollectionPage messages;
		do
		{
			messages = await graphClient.Me.Messages.Request().Top(25).Skip(top)
				.GetAsync();
			top += 25;
			foreach (Message msg in messages)
			{
				if (msg.Sender.EmailAddress.Address == "notification@facebookmail.com" && (msg.Subject.Contains("invite") || msg.Sender.EmailAddress.Name.Contains("Facebook") || msg.Sender.EmailAddress.Name.Contains("Meta for Business") || msg.Subject.Contains("Meta Business Suite")))
				{
					string link = GetLinkBM(msg.Body.Content);
					if (!string.IsNullOrEmpty(link))
					{
						string bodyHtml = msg.Body.Content;
						Recipient sender = msg.Sender;
						string subject = msg.Subject;
						MailMessageDto mailMessage = new MailMessageDto
						{
							Sender = sender.EmailAddress.Address,
							Message = bodyHtml,
							Subject = subject,
							ReceiveTime = msg.ReceivedDateTime.Value.ToString("dd/MM/yyyy HH:mm:ss"),
							Code = Regex.Match(bodyHtml, "\\d{6,8}").Value,
							BusinessLink = link
						};
						mailmessages.Add(mailMessage);
					}
				}
			}
		}
		while (messages.NextPageRequest != null && mailmessages.Count == 0);
		return mailmessages;
	}

	public async Task<List<MailMessageDto>> GetLinkInvite2()
	{
		string accessToken = await GetAccessToken();
		if (string.IsNullOrEmpty(accessToken))
		{
			return null;
		}
		List<MailMessageDto> mailmessages = new List<MailMessageDto>();
		using (ImapClient client = new ImapClient())
		{
			SaslMechanismOAuth2 oauth2 = new SaslMechanismOAuth2(HotmailDto.Username, accessToken);
			client.Connect("outlook.office365.com", 993, SecureSocketOptions.SslOnConnect);
			client.Authenticate(oauth2);
			IMailFolder inbox = client.Inbox;
			inbox.Open(FolderAccess.ReadWrite);
			inbox.Search(MailKit.Search.SearchQuery.NotSeen);
			IList<IMessageSummary> messages = inbox.Fetch(0, 100, MessageSummaryItems.Envelope | MessageSummaryItems.Flags);
			List<string> validSenders = new List<string> { "security@facebookmail.com", "security@account.meta.com", "notification@facebookmail.com" };
			foreach (IMessageSummary message in messages.OrderByDescending((IMessageSummary m) => m.Index))
			{
				MessageFlags? flags = message.Flags;
				if (flags.HasValue && flags.GetValueOrDefault().HasFlag(MessageFlags.Seen))
				{
					continue;
				}
				inbox.AddFlags(message.Index, MessageFlags.Seen, silent: true);
				if (IsFromValidSender(message, validSenders))
				{
					MimeMessage messageStr = inbox.GetMessage(message.Index);
					string link = GetLinkBM(messageStr.HtmlBody);
					if (!string.IsNullOrEmpty(link))
					{
						string bodyHtml = messageStr.HtmlBody;
						InternetAddressList sender = message.Envelope.Sender;
						string subject = message.NormalizedSubject;
						MailMessageDto mailMessage = new MailMessageDto
						{
							Sender = sender.ToString(),
							Message = bodyHtml,
							Subject = subject,
							ReceiveTime = message.Date.ToString("dd/MM/yyyy HH:mm:ss"),
							Code = Regex.Match(bodyHtml, "\\d{6,8}").Value,
							BusinessLink = link
						};
						mailmessages.Add(mailMessage);
					}
				}
			}
			client.Disconnect(quit: true);
			client.Dispose();
		}
		return mailmessages;
	}

	private bool IsFromValidSender(IMessageSummary message, List<string> validSenders)
	{
		string sender = message.Envelope.From.ToString();
		return validSenders.Any(sender.Contains);
	}

	private string GetLinkBM(string body)
	{
		return string.IsNullOrEmpty(Regex.Match(body, "(https://fb.me/(.+?))\"").Groups[1].Value) ? Regex.Match(body, "(https://business.facebook.com/invitation/?(.+?))\"").Groups[1].Value : Regex.Match(body, "(https://fb.me/(.+?))\"").Groups[1].Value;
	}

	private async Task<string> GetAccessToken()
	{
		string accessToken = string.Empty;
		try
		{
			Dictionary<string, string> payload = new Dictionary<string, string>
			{
				{ "client_id", HotmailDto.ClientId },
				{ "refresh_token", HotmailDto.RefreshToken },
				{ "grant_type", "refresh_token" }
			};
			dynamic json = JsonConvert.DeserializeObject(await (await _client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload))).Content.ReadAsStringAsync());
			accessToken = json.access_token;
			HotmailDto.AccessToken = accessToken;
		}
		catch
		{
		}
		return accessToken;
	}

	private async Task<string> GetAccessToken2()
	{
		string accessToken = string.Empty;
		try
		{
			Dictionary<string, string> payload = new Dictionary<string, string>
			{
				{ "client_id", HotmailDto.ClientId },
				{ "refresh_token", HotmailDto.RefreshToken },
				{ "grant_type", "refresh_token" },
				{ "scope", "https://graph.microsoft.com/.default offline_access" }
			};
			dynamic json = JsonConvert.DeserializeObject(await (await _client.PostAsync("https://login.microsoftonline.com/common/oauth2/v2.0/token", (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)payload))).Content.ReadAsStringAsync());
			accessToken = json.access_token;
			HotmailDto.AccessToken = accessToken;
		}
		catch
		{
		}
		return accessToken;
	}
}
