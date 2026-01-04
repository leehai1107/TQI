using System;
using System.Windows.Forms;
using TQI.NBTeam.Services;

namespace TQI.NBTeam;

internal static class Program
{
	private static UpdateManager _updateManager;
	
	public static UpdateManager UpdateManager => _updateManager;

	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		
		// Khởi tạo UpdateManager để auto-check updates
		try
		{
			_updateManager = new UpdateManager();
			_updateManager.Initialize();
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[Program] Lỗi khởi tạo UpdateManager: {ex.Message}");
		}
		
		Application.Run(new frmMain());
		
		// Cleanup
		_updateManager?.Dispose();
	}
}
