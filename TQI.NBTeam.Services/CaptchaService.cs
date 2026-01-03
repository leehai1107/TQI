using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TQI.NBTeam.Commons;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Services;

public class CaptchaService
{
	public enum StatusCreateTask
	{
		Success,
		Failure
	}

	public enum StatusResultTask
	{
		Success,
		Failure,
		Processing
	}

	private CaptchaDto _captchaDto;

	private HttpClient _httpClient;

	private const string baseUrl = "https://api.gemcaptcha.com";

	public CaptchaService(CaptchaDto captchaDto)
	{
		_captchaDto = captchaDto;
		(_httpClient, _) = HttpClientCommon.CreateHttpClient();
	}

	public async Task<(StatusCreateTask, string)> CreateRecaptchaV2Task()
	{
		var payload = new
		{
			clientKey = _captchaDto.CaptchaKey,
			task = new
			{
				type = "RecaptchaV2TokenTask",
				websiteURL = _captchaDto?.WebsiteUrl,
				websiteKey = _captchaDto?.WebsiteKey
			}
		};
		string payloadStr = JsonConvert.SerializeObject(payload);
		MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json")
		{
			CharSet = "UTF-8"
		};
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.gemcaptcha.com/v2/createTask")
		{
			Content = (HttpContent)new StringContent(payloadStr)
		};
		httpRequestMessage.Content.Headers.ContentType = contentType;
		JObject responseObject = JObject.Parse(await (await _httpClient.SendAsync(httpRequestMessage)).Content.ReadAsStringAsync());
		if (string.IsNullOrEmpty(responseObject["errorId"]?.ToString()) || responseObject["errorId"]?.ToString() == "1")
		{
			return (StatusCreateTask.Failure, responseObject["errorDescription"]?.ToString());
		}
		string taskId = responseObject["taskId"].ToString();
		return (StatusCreateTask.Success, taskId);
	}

	public async Task<(StatusResultTask, string)> GetTaskResult(string taskId)
	{
		var payload = new
		{
			clientKey = _captchaDto.CaptchaKey,
			taskId = taskId
		};
		string payloadStr = JsonConvert.SerializeObject(payload);
		HttpRequestMessage getTaskResultMessage = new HttpRequestMessage(HttpMethod.Post, "https://api.gemcaptcha.com/v2/getTaskResult");
		getTaskResultMessage.Content = (HttpContent)new StringContent(payloadStr);
		MediaTypeHeaderValue contentType = new MediaTypeHeaderValue("application/json")
		{
			CharSet = "UTF-8"
		};
		getTaskResultMessage.Content.Headers.ContentType = contentType;
		JObject responseObject = JObject.Parse(await (await _httpClient.SendAsync(getTaskResultMessage)).Content.ReadAsStringAsync());
		if (string.IsNullOrEmpty(responseObject["errorId"]?.ToString()) || responseObject["errorId"]?.ToString() == "1")
		{
			return (StatusResultTask.Failure, responseObject["errorDescription"]?.ToString());
		}
		if (responseObject["errorId"]?.ToString() == "0" && responseObject["status"]?.ToString() == "ready")
		{
			string recaptchaResponse = responseObject["solution"]?["gRecaptchaResponse"]?.ToString();
			return (StatusResultTask.Success, recaptchaResponse);
		}
		return (StatusResultTask.Processing, null);
	}
}
