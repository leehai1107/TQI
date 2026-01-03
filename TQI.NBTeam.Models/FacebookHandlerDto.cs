using TQI.NBTeam.Handlers;

namespace TQI.NBTeam.Models;

public class FacebookHandlerDto
{
	public FacebookHandler FacebookHandler { get; set; }

	public AccountDto Account { get; set; }

	public ChromeHandler ChromeHandler { get; set; }

	public bool IsLoggedIn { get; set; }
}
