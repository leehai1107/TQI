using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NetSparkleUpdater;
using NetSparkleUpdater.AssemblyAccessors;
using NetSparkleUpdater.Configurations;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;

namespace TQI.NBTeam.Services;

/// <summary>
/// Quản lý auto-update cho ứng dụng thông qua NetSparkle
/// </summary>
public class UpdateManager : IDisposable
{
	private SparkleUpdater _sparkle;
	private bool _disposed;

	/// <summary>
	/// URL appcast trỏ tới asset mới nhất trên GitHub Releases
	/// Workflow sẽ upload appcast.xml vào release asset với tên appcast.xml
	/// </summary>
	private const string AppCastUrl = "https://github.com/leehai1107/TQI/releases/latest/download/appcast.xml";

	/// <summary>
	/// Khởi tạo UpdateManager và cấu hình NetSparkle
	/// </summary>
	public void Initialize()
	{
		try
		{
			// Tạo custom configuration để tránh lỗi Registry (dùng JSON file)
			var assemblyAccessor = new AssemblyReflectionAccessor("TQI.NBTeam.exe");
			var appDataPath = Path.Combine(
				Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
				"TQI",
				"NBTeam"
			);
			Directory.CreateDirectory(appDataPath);
			var configPath = Path.Combine(appDataPath, "sparkle_config.json");
			
			// Tạo SparkleUpdater với chế độ tự động hoàn toàn
			_sparkle = new SparkleUpdater(
				AppCastUrl,
				new DSAChecker(SecurityMode.Unsafe) // Unsafe mode - không verify signature (có thể thay bằng DSA key để bảo mật hơn)
			)
			{
				UIFactory = null, // Tắt UI - update hoàn toàn tự động
				RelaunchAfterUpdate = true, // Tự động restart app sau khi update
				CustomInstallerArguments = "/VERYSILENT /NORESTART", // Silent install, không restart từ installer
				Configuration = new JSONConfiguration(assemblyAccessor, configPath) // Dùng JSON thay vì Registry
			};

			// Log events để debug
			_sparkle.UpdateDetected += (sender, e) =>
			{
				Debug.WriteLine($"[UpdateManager] Phát hiện version mới!");
				Debug.WriteLine($"[UpdateManager] Next action: {e.NextAction}");
			};

			_sparkle.UpdateCheckFinished += (sender, status) =>
			{
				Debug.WriteLine($"[UpdateManager] Kiểm tra update hoàn tất. Status: {status}");
				if (status == UpdateStatus.UpdateAvailable)
				{
					Debug.WriteLine("[UpdateManager] Có bản cập nhật mới - đang tự động download và install");
				}
				else if (status == UpdateStatus.UpdateNotAvailable)
				{
					Debug.WriteLine("[UpdateManager] Đang sử dụng phiên bản mới nhất");
				}
			};

			// Cấu hình update check behavior - tự động kiểm tra và cài đặt
			_sparkle.StartLoop(true, true); // Check ngay lập tức và lặp lại định kỳ
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"[UpdateManager] Lỗi khởi tạo: {ex.Message}");
		}
	}

	/// <summary>
	/// Kiểm tra update thủ công
	/// </summary>
	public void CheckForUpdatesManually()
	{
		_sparkle?.CheckForUpdatesAtUserRequest();
	}

	/// <summary>
	/// Kiểm tra update ngầm (không hiện dialog nếu không có update)
	/// </summary>
	public void CheckForUpdatesQuietly()
	{
		_sparkle?.CheckForUpdatesQuietly();
	}

	/// <summary>
	/// Dispose resources
	/// </summary>
	public void Dispose()
	{
		if (_disposed)
			return;

		_sparkle?.Dispose();
		_sparkle = null;

		_disposed = true;
	}
}
