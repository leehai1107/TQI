using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OtpNet;

namespace TQI.NBTeam.Commons;

public class Common
{
	private static int getWidthScreen = Screen.PrimaryScreen.WorkingArea.Width;

	private static int getHeightScreen = Screen.PrimaryScreen.WorkingArea.Height;

	public static string GetCode(string key2fa)
	{
		byte[] secretKey = Base32Encoding.ToBytes(key2fa.Trim().Replace(" ", ""));
		Totp totp = new Totp(secretKey);
		return totp.ComputeTotp(DateTime.UtcNow);
	}

	public static bool IsDateTimeOffset(string input)
	{
		DateTimeOffset result;
		return DateTimeOffset.TryParse(input, out result);
	}

	public static bool IsUnixTimestamp(string input)
	{
		if (long.TryParse(input, out var timestamp))
		{
			return timestamp > 0 && timestamp < 253402300799L;
		}
		return false;
	}

	private static List<string> GetListHoEN()
	{
		return new List<string>
		{
			"Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson", "Moore", "Taylor",
			"Anderson", "Thomas", "Jackson", "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson",
			"Clark", "Rodriguez", "Lewis", "Lee", "Walker", "Hall", "Allen", "Young", "Hernandez", "King",
			"Wright", "Lopez", "Hill", "Scott", "Green", "Adams", "Baker", "Gonzalez", "Nelson", "Carter",
			"Mitchell", "Perez", "Roberts", "Turner", "Phillips", "Campbell", "Parker", "Evans", "Edwards", "Collins",
			"Stewart", "Sanchez", "Morris", "Rogers", "Reed", "Cook", "Morgan", "Bell", "Murphy", "Bailey",
			"Rivera", "Cooper", "Richardson", "Cox", "Howard", "Ward", "Torres", "Peterson", "Gray", "Ramirez",
			"James", "Watson", "Brooks", "Kelly", "Sanders", "Price", "Bennett", "Wood", "Barnes", "Ross",
			"Henderson", "Coleman", "Jenkins", "Perry", "Powell", "Long", "Patterson", "Hughes", "Flores", "Washington",
			"Butler", "Simmons", "Foster", "Gonzales", "Bryant", "Alexander"
		};
	}

	private static List<string> GetListTenEN()
	{
		return new List<string>
		{
			"James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles",
			"Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul", "Steven", "Andrew", "Kenneth",
			"Joshua", "George", "Kevin", "Brian", "Edward", "Ronald", "Timothy", "Jason", "Jeffrey", "Ryan",
			"Jacob", "Gary", "Nicholas", "Eric", "Stephen", "Jonathan", "Larry", "Justin", "Scott", "Brandon",
			"Frank", "Benjamin", "Gregory", "Samuel", "Raymond", "Patrick", "Alexander", "Jack", "Dennis", "Jerry",
			"Tyler", "Aaron", "Jose", "Henry", "Douglas", "Adam", "Peter", "Nathan", "Zachary", "Walter",
			"Kyle", "Harold", "Carl", "Jeremy", "Keith", "Roger", "Gerald", "Ethan", "Arthur", "Terry",
			"Christian", "Sean", "Lawrence", "Austin", "Joe", "Noah", "Jesse", "Albert", "Bryan", "Billy",
			"Bruce", "Willie", "Jordan", "Dylan", "Alan", "Ralph", "Gabriel", "Roy", "Juan", "Wayne",
			"Eugene", "Logan", "Randy", "Louis", "Russell", "Vincent", "Philip", "Bobby", "Johnny", "Bradley"
		};
	}

	public static string GetRandomFName()
	{
		Random rd = new Random();
		List<string> listHo = GetListHoEN();
		return listHo[rd.Next(0, listHo.Count)];
	}

	public static string GetRandomLName()
	{
		Random rd = new Random();
		List<string> lstTen = GetListTenEN();
		return lstTen[rd.Next(0, lstTen.Count)];
	}

	public static string CreateRandomNumber(int leng, Random rd = null)
	{
		string text = "";
		if (rd == null)
		{
			rd = new Random();
		}
		string text2 = "0123456789";
		for (int i = 0; i < leng; i++)
		{
			text += text2[rd.Next(0, text2.Length)];
		}
		return text;
	}

	public static string CreateRandomString(int length, string characters = null, Random random = null)
	{
		if (length < 0)
		{
			throw new ArgumentOutOfRangeException("length", "Length must be non-negative.");
		}
		characters = (string.IsNullOrEmpty(characters) ? "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789" : characters);
		if (string.IsNullOrEmpty(characters))
		{
			throw new ArgumentException("Character set cannot be empty or null.", "characters");
		}
		StringBuilder result = new StringBuilder(length);
		if (random != null)
		{
			for (int i = 0; i < length; i++)
			{
				result.Append(characters[random.Next(0, characters.Length)]);
			}
		}
		else
		{
			byte[] randomBytes = new byte[length];
			using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(randomBytes);
			}
			for (int j = 0; j < length; j++)
			{
				int index = randomBytes[j] % characters.Length;
				result.Append(characters[index]);
			}
		}
		return result.ToString();
	}

	public static async Task DelayAsync(double seconds, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (seconds < 0.0)
		{
			throw new ArgumentOutOfRangeException("seconds", "Delay duration cannot be negative.");
		}
		if (seconds < 1.0)
		{
			await Task.Delay(TimeSpan.FromSeconds(seconds), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			return;
		}
		int totalMilliseconds = (int)(seconds * 1000.0);
		for (int elapsed = 0; elapsed < totalMilliseconds; elapsed += 1000)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1000, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public static void KillProcess(string nameProcess)
	{
		string contents = "TASKKILL /F /IM " + nameProcess + ".exe /T";
		try
		{
			File.WriteAllText("kill.bat", contents);
			ProcessStartInfo startInfo = new ProcessStartInfo("kill.bat")
			{
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				UseShellExecute = false
			};
			Process.Start(startInfo);
		}
		catch
		{
			File.WriteAllText("kill.bat", contents);
			Process.Start("kill.bat");
		}
	}

	public static void Sleep(double timeSleep)
	{
		Application.DoEvents();
		Thread.Sleep(Convert.ToInt32(timeSleep * 1000.0));
	}

	public static List<T> ShuffleList<T>(List<T> list)
	{
		Random rng = new Random();
		List<T> newList = new List<T>(list);
		int n = newList.Count;
		while (n > 1)
		{
			n--;
			int k = rng.Next(n + 1);
			T value = newList[k];
			newList[k] = newList[n];
			newList[n] = value;
		}
		return newList;
	}

	public static int ReserveNextAvailablePosition(ref List<int> positions)
	{
		lock (positions)
		{
			int index = positions.IndexOf(0);
			if (index >= 0)
			{
				positions[index] = 1;
				return index;
			}
			return 0;
		}
	}

	public static void ReleasePosition(ref List<int> positions, int index)
	{
		lock (positions)
		{
			positions[index] = 0;
		}
	}

	public static Point CalculateChromeWindowSize(int columns, int rows)
	{
		int width = 320;
		int height = 570;
		int windowWidth = width / columns + 15;
		int windowHeight = height / rows + 10;
		return new Point(windowWidth, windowHeight);
	}

	public static Point GetPointFromIndexPosition(int indexPos, int column, int row)
	{
		int screenWidth = getWidthScreen;
		int screenHeight = getHeightScreen;
		int normalizedIndex = indexPos % (column * row);
		int rowIndex = normalizedIndex / column;
		int columnIndex = normalizedIndex % column;
		int yPosition = 0;
		if (row >= 1 && row <= 5)
		{
			yPosition = screenHeight / row * rowIndex;
		}
		int xPosition = columnIndex * (screenWidth / column) - 10;
		return new Point(xPosition, yPosition);
	}

	public static void DeleteFile(string filePath)
	{
		if (Directory.Exists(filePath))
		{
			Directory.Delete(filePath, recursive: true);
		}
	}

	public static List<string> GetFiles(string folderPath)
	{
		if (Directory.Exists(folderPath))
		{
			return Directory.GetDirectories(folderPath).ToList();
		}
		return new List<string>();
	}
}
