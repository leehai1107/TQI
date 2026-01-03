namespace TQI.NBTeam.Models;

public class SmsMessageDto
{
	public string Id { get; set; }

	public string ServiceId { get; set; }

	public string ServiceName { get; set; }

	public int Status { get; set; }

	public long Price { get; set; }

	public string Phone { get; set; }

	public string SmsContent { get; set; }

	public bool IsSound { get; set; }

	public string CreatedTime { get; set; }

	public string Code { get; set; }

	public string PhoneOriginal { get; set; }

	public string CountryISO { get; set; }

	public string CountryCode { get; set; }
}
