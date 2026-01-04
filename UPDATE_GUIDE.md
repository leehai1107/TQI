# Hướng dẫn Auto-Update với NetSparkle

## Tổng quan

Ứng dụng TQI.NBTeam hiện đã được tích hợp NetSparkle để tự động cập nhật từ GitHub.

## Cách hoạt động

### 1. Khi khởi động ứng dụng

- Ứng dụng **TỰ ĐỘNG** kiểm tra update từ GitHub (file `appcast.xml`)
- Nếu có phiên bản mới, **TỰ ĐỘNG DOWNLOAD** ngầm không cần hỏi user
- Sau khi download xong, **TỰ ĐỘNG RESTART** với phiên bản mới
- **User không cần làm gì cả** - mở app là có version mới nhất!

### 2. File appcast.xml

File này cần được đặt trong repository GitHub của bạn (nhánh `main`) và accessible qua URL:

```
https://raw.githubusercontent.com/leehai1107/TQI/main/appcast.xml
```

## Quy trình release version mới

### Bước 1: Cập nhật version trong code

1. Mở file `Properties/AssemblyInfo.cs`
2. Thay đổi version:

```csharp
[assembly: AssemblyFileVersion("1.0.1.0")]
[assembly: AssemblyVersion("1.0.1.0")]
```

### Bước 2: Build ứng dụng

```bash
dotnet build -c Release
```

### Bước 3: Tạo installer

Bạn cần tạo file installer (Setup.exe) cho ứng dụng. Có thể dùng các công cụ:

- **Inno Setup** (khuyên dùng - miễn phí)
- **WiX Toolset**
- **Advanced Installer**

#### Ví dụ với Inno Setup:

1. Download và cài Inno Setup: https://jrsoftware.org/isinfo.php
2. Tạo file script `setup.iss`:

```iss
[Setup]
AppName=TQI.NBTeam
AppVersion=1.0.1
DefaultDirName={pf}\TQI.NBTeam
DefaultGroupName=TQI.NBTeam
OutputDir=.
OutputBaseFilename=TQI.NBTeam-1.0.1-Setup
Compression=lzma2
SolidCompression=yes

[Files]
Source: "bin\Release\net481\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Icons]
Name: "{group}\TQI.NBTeam"; Filename: "{app}\TQI.NBTeam.exe"
Name: "{commondesktop}\TQI.NBTeam"; Filename: "{app}\TQI.NBTeam.exe"

[Run]
Filename: "{app}\TQI.NBTeam.exe"; Description: "Launch TQI.NBTeam"; Flags: postinstall nowait skipifsilent
```

3. Build installer:

```bash
iscc setup.iss
```

### Bước 4: Tạo GitHub Release

1. Vào repository GitHub: https://github.com/leehai1107/TQI
2. Chọn "Releases" → "Create a new release"
3. Điền thông tin:
   - **Tag**: `v1.0.1` (phải khớp với version trong code)
   - **Title**: `Version 1.0.1`
   - **Description**: Liệt kê các tính năng mới và bug fixes
4. Upload file installer: `TQI.NBTeam-1.0.1-Setup.exe`
5. Click "Publish release"

### Bước 5: Cập nhật appcast.xml

1. Mở file `appcast.xml`
2. Thêm entry mới cho version 1.0.1 (ở đầu file, trước version cũ):

```xml
<item>
  <title>Version 1.0.1</title>
  <sparkle:releaseNotesLink>
    https://github.com/leehai1107/TQI/releases/tag/v1.0.1
  </sparkle:releaseNotesLink>
  <pubDate>Sat, 04 Jan 2026 00:00:00 +0700</pubDate>
  <enclosure
    url="https://github.com/leehai1107/TQI/releases/download/v1.0.1/TQI.NBTeam-1.0.1-Setup.exe"
    sparkle:version="1.0.1"
    sparkle:os="windows"
    length="12345678"
    type="application/octet-stream" />
</item>
```

**Chú ý quan trọng:**

- `url`: Phải là đường dẫn download chính xác của file installer trên GitHub Release
- `sparkle:version`: Version mới (ví dụ: `1.0.1`)
- `length`: Kích thước file installer tính bằng bytes (xem trong Properties của file)
- `pubDate`: Ngày phát hành theo format RFC 822

3. Commit và push file `appcast.xml` lên nhánh `main`:

```bash
git add appcast.xml
git commit -m "Update appcast.xml for version 1.0.1"
git push origin main
```

### Bước 6: Kiểm tra

1. Chạy ứng dụng version cũ (1.0.0)
2. Ứng dụng sẽ tự động phát hiện version mới (1.0.1)
3. Dialog update sẽ xuất hiện
4. Click "Update" để test quá trình cập nhật

## Cấu trúc URL quan trọng

### URL appcast.xml

```
https://raw.githubusercontent.com/leehai1107/TQI/main/appcast.xml
```

- URL này được cấu hình trong `UpdateManager.cs`
- GitHub sẽ serve file XML trực tiếp

### URL download installer

```
https://github.com/leehai1107/TQI/releases/download/v1.0.1/TQI.NBTeam-1.0.1-Setup.exe
```

Format: `https://github.com/{owner}/{repo}/releases/download/{tag}/{filename}`

## Troubleshooting

### Ứng dụng không phát hiện update

- Kiểm tra xem `appcast.xml` có accessible từ URL trên không
- Mở trình duyệt và test URL: https://raw.githubusercontent.com/leehai1107/TQI/main/appcast.xml
- Kiểm tra version trong `appcast.xml` có lớn hơn version hiện tại không
- Xem Debug output trong Visual Studio (có log từ UpdateManager)

### Download failed

- Kiểm tra URL download trong `appcast.xml` có đúng không
- Kiểm tra file installer có tồn tại trong GitHub Release không
- Kiểm tra kích thước file (`length` attribute) có đúng không

### Update không tự động restart

- Đảm bảo `RelaunchAfterUpdate = true` trong `UpdateManager.cs`
- Kiểm tra quyền Administrator nếu cần thiết

## Tính năng nâng cao

### Thêm digital signature (khuyên dùng cho production)

Để bảo mật hơn, nên dùng DSA signature thay vì `SecurityMode.Unsafe`:

1. Generate DSA key pair:

```bash
netsparkle-dsa-helper generate
```

2. Sign installer:

```bash
netsparkle-dsa-helper sign TQI.NBTeam-1.0.1-Setup.exe
```

3. Thêm signature vào `appcast.xml`:

```xml
<enclosure
  ...
  sparkle:dsaSignature="MC0CFQCx..." />
```

4. Update `UpdateManager.cs`:

```csharp
new DSAChecker(SecurityMode.Strict, "YOUR_PUBLIC_KEY")
```

### Kiểm tra update thủ công

Nếu muốn thêm button "Check for Updates" trong UI:

```csharp
// Trong form hoặc menu item
private void btnCheckUpdate_Click(object sender, EventArgs e)
{
    var updateManager = new UpdateManager();
    updateManager.Initialize();
    updateManager.CheckForUpdatesManually();
}
```

## Checklist cho mỗi release

- [ ] Cập nhật version trong `AssemblyInfo.cs`
- [ ] Build project ở mode Release
- [ ] Tạo installer với Inno Setup hoặc tool tương tự
- [ ] Test installer trên máy sạch
- [ ] Tạo GitHub Release với tag đúng format (`v1.0.x`)
- [ ] Upload installer vào GitHub Release
- [ ] Cập nhật `appcast.xml` với URL và thông tin chính xác
- [ ] Commit và push `appcast.xml` lên nhánh `main`
- [ ] Test auto-update từ version cũ lên version mới

## Tài liệu tham khảo

- NetSparkle Documentation: https://github.com/NetSparkleUpdater/NetSparkle
- Inno Setup: https://jrsoftware.org/isinfo.php
- GitHub Releases: https://docs.github.com/en/repositories/releasing-projects-on-github
