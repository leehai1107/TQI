using System.Collections.Generic;
using TQI.NBTeam.Models;

namespace TQI.NBTeam.Containers;

public class FacebookHandlerContainer
{
	private static FacebookHandlerContainer _instance;

	private static readonly object _lock = new object();

	private List<FacebookHandlerDto> _facebookHandlerProfile { get; set; }

	public List<FacebookHandlerDto> FacebookHandlerProfile => _facebookHandlerProfile;

	public static FacebookHandlerContainer Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (_lock)
				{
					if (_instance == null)
					{
						_instance = new FacebookHandlerContainer();
					}
				}
			}
			return _instance;
		}
	}

	public FacebookHandlerContainer()
	{
		_facebookHandlerProfile = new List<FacebookHandlerDto>();
	}

	public void AddFacebookHandlerProfile(FacebookHandlerDto facebookHandlerDto)
	{
		lock (_lock)
		{
			FacebookHandlerDto exist = _facebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Uid.Equals(facebookHandlerDto.Account.Uid));
			if (exist != null)
			{
				int index = _facebookHandlerProfile.IndexOf(exist);
				_facebookHandlerProfile[index] = facebookHandlerDto;
			}
			else
			{
				_facebookHandlerProfile.Add(facebookHandlerDto);
			}
		}
	}

	public void UpdateFacebookHandlerProfile(FacebookHandlerDto updatedFacebookHandlerDto)
	{
		lock (_lock)
		{
			FacebookHandlerDto exist = _facebookHandlerProfile.Find((FacebookHandlerDto x) => x.Equals(updatedFacebookHandlerDto));
			if (exist != null)
			{
				int index = _facebookHandlerProfile.IndexOf(exist);
				_facebookHandlerProfile[index] = updatedFacebookHandlerDto;
			}
			else
			{
				_facebookHandlerProfile.Add(updatedFacebookHandlerDto);
			}
		}
	}

	public void DeleteFacebookHandlerProfile(string uid)
	{
		lock (_lock)
		{
			FacebookHandlerDto exist = _facebookHandlerProfile.Find((FacebookHandlerDto x) => x.Account.Equals(uid));
			if (exist != null)
			{
				_facebookHandlerProfile.Remove(exist);
			}
		}
	}
}
