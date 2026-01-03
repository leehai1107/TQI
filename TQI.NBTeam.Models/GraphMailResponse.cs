namespace TQI.NBTeam.Models;

public class GraphMailResponse
{
	public string email { get; set; }

	public string password { get; set; }

	public bool status { get; set; }

	public string code { get; set; }

	public dynamic messages { get; set; }
}
