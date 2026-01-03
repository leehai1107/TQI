using System;
using System.Threading;
using System.Threading.Tasks;

namespace TQI.NBTeam;

public static class TaskExtensions
{
	public static async Task WithTimeout(this Task task, TimeSpan timeout)
	{
		using CancellationTokenSource cts = new CancellationTokenSource();
		Task delayTask = Task.Delay(timeout, cts.Token);
		if (await Task.WhenAny(task, delayTask) == delayTask)
		{
			throw new TimeoutException("The operation timed out.");
		}
		cts.Cancel();
		await task;
	}
}
