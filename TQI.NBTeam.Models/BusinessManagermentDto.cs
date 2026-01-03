using System.Collections.Generic;

namespace TQI.NBTeam.Models;

public class BusinessManagermentDto
{
	public BusinessInfomationDto BusinessInfo { get; set; }

	public List<BusinessUserDto> BusinessUsers { get; set; }
}
