using System.Collections.Generic;

namespace TQI.NBTeam.Models;

public class AccountDto
{
	public string Uid { get; set; }

	public string Password { get; set; }

	public string Key2FA { get; set; }

	public string Email { get; set; }

	public string Cookie { get; set; }

	public string Token { get; set; }

	public string DTSGToken { get; set; }

	public string LSDToken { get; set; }

	public string AdAccountId { get; set; }

	public List<BusinessManagermentDto> BusinessManagerments { get; set; }
}
