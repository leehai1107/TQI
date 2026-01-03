using System;
using System.Collections.Generic;
using System.Threading;
using TQI.NBTeam.Services;

namespace TQI.NBTeam.Manages;

public class ProxyManager
{
	private readonly List<ProxyService> _proxyServices;

	private readonly object _lock = new object();

	private int _nextProxyIndex = 0;

	public ProxyManager(List<ProxyService> proxies)
	{
		_proxyServices = proxies ?? throw new ArgumentNullException("proxies");
		if (_proxyServices.Count == 0)
		{
			throw new ArgumentException("Danh sách proxy không được rỗng.");
		}
	}

	public ProxyService AcquireProxy()
	{
		while (true)
		{
			lock (_lock)
			{
				for (int i = 0; i < _proxyServices.Count; i++)
				{
					ProxyService proxy = _proxyServices[_nextProxyIndex];
					_nextProxyIndex = (_nextProxyIndex + 1) % _proxyServices.Count;
					if (proxy.TryAcquireProxy())
					{
						Console.WriteLine($"[Acquire] {proxy.Proxy} (Usage: {proxy.CurrentUsage})");
						return proxy;
					}
				}
			}
			Thread.Sleep(100);
		}
	}

	public void ReleaseProxy(ProxyService proxy)
	{
		if (proxy != null)
		{
			proxy.ReleaseProxy();
			Console.WriteLine($"[Release] {proxy.Proxy} (Usage: {proxy.CurrentUsage})");
			if (proxy.CurrentUsage == 0 && proxy.TryReset())
			{
				Console.WriteLine("[Reset] Proxy " + proxy.Proxy + " đã được đổi.");
			}
		}
	}
}
