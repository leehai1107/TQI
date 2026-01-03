namespace TQI.NBTeam.Services;

public class ProxyService
{
	private readonly object _lock = new object();

	public string Proxy { get; private set; }

	public string UrlApi { get; private set; }

	public string Port { get; private set; }

	public int CurrentUsage { get; private set; }

	public int LastUsage { get; private set; }

	public bool IsReset { get; private set; }

	public int TypeProxy { get; private set; }

	public int LimitThreadUse { get; }

	public bool TryAcquireProxy()
	{
		lock (_lock)
		{
			if (CurrentUsage < LimitThreadUse && !IsReset && LastUsage < LimitThreadUse)
			{
				CurrentUsage++;
				return true;
			}
			return false;
		}
	}

	public void ReleaseProxy()
	{
		lock (_lock)
		{
			if (CurrentUsage > 0)
			{
				CurrentUsage--;
				LastUsage++;
			}
		}
	}

	public bool TryReset()
	{
		if (CurrentUsage != 0 || LastUsage < LimitThreadUse)
		{
			return false;
		}
		lock (_lock)
		{
			if (CurrentUsage != 0 || LastUsage < LimitThreadUse)
			{
				return false;
			}
			IsReset = true;
		}
		bool result;
		try
		{
			result = true;
		}
		finally
		{
			lock (_lock)
			{
				LastUsage = 0;
				IsReset = false;
			}
		}
		return result;
	}
}
