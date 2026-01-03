using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TQI.NBTeam.Commons;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Services;

public class ViotpService
{
	private ViotpDto _viotpDto;

	private HttpClient _httpClient;

	private const string baseUrl = "https://api.viotp.com";

	public ViotpService(ViotpDto viotpDto)
	{
		_viotpDto = viotpDto;
		(_httpClient, _) = HttpClientCommon.CreateHttpClient();
	}

	public async Task<ViotpDataPhone> CreateTask()
	{
		HttpResponseMessage response = await _httpClient.GetAsync("https://api.viotp.com/request/getv2?token=" + _viotpDto.ApiKey + "&serviceId=" + _viotpDto.ServiceId);
		if (!response.IsSuccessStatusCode)
		{
			return null;
		}
		string responseStr = await response.Content.ReadAsStringAsync();
		if (!responseStr.Contains("status_code\":200"))
		{
			return null;
		}
		JObject responseObject = JObject.Parse(responseStr);
		string phoneNumber = responseObject["data"]["phone_number"].ToString();
		string requestId = responseObject["data"]["request_id"].ToString();
		string countryISO = responseObject["data"]["countryISO"].ToString();
		return new ViotpDataPhone
		{
			PhoneNumber = phoneNumber,
			RequestId = requestId,
			CountryISO = countryISO
		};
	}

	public async Task<SmsMessageDto> GetCodeData(string requestId)
	{
		HttpResponseMessage response = await _httpClient.GetAsync("https://api.viotp.com/session/getv2?requestId=" + requestId + "&token=" + _viotpDto.ApiKey);
		if (!response.IsSuccessStatusCode)
		{
			return null;
		}
		string responseStr = await response.Content.ReadAsStringAsync();
		if (!responseStr.Contains("status_code\":200"))
		{
			return null;
		}
		JObject responseObject = JObject.Parse(responseStr);
		string dataJson = responseObject["data"].ToString();
		return JsonConvert.DeserializeObject<SmsMessageDto>(dataJson);
	}
}
