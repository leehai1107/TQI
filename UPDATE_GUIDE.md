# Hướng dẫn Auto-Update với NetSparkle

## Tổng quan

Ứng dụng TQI.NBTeam đã tích hợp NetSparkle và workflow GitHub Actions để tự động build & phát hành nhị phân.

## Cách hoạt động

### 1. Khi khởi động ứng dụng

- Ứng dụng **tự động** kiểm tra update qua `appcast.xml` tải từ GitHub Releases (asset mới nhất)
- Nếu có phiên bản mới, **tự động download** và **tự động restart** với bản mới
- User chỉ cần mở app, không phải làm gì thêm

### 2. File appcast.xml (được workflow sinh tự động)

- URL: `https://github.com/leehai1107/TQI/releases/latest/download/appcast.xml`
- Workflow tạo appcast và upload vào Release, luôn trỏ tới asset mới nhất

## Quy trình release version mới

### Quy trình release mới (tự động qua GitHub Actions)

1. **Bump version** trong `Properties/AssemblyInfo.cs` (ví dụ 1.0.2.0) cho đồng bộ `ProductVersion` hiển thị.
2. **Tạo tag** theo chuẩn: `v1.0.2` (khớp số version ở trên).

```bash
git commit -am "Bump version 1.0.2"
git tag v1.0.2
git push origin main --tags
```

3. **Workflow chạy tự động** (file `.github/workflows/release.yml`):

   - `dotnet publish` win-x64
   - Đóng gói zip: `TQI.NBTeam-v1.0.2-win-x64.zip`
   - Sinh `appcast.xml` với đúng version, pubDate, length
   - Tạo GitHub Release từ tag và upload cả zip + appcast

4. **Client cũ** khi mở app sẽ lấy `appcast.xml` ở release mới nhất và tự động update.

## Cấu trúc URL quan trọng

### URL appcast.xml

```
https://github.com/leehai1107/TQI/releases/latest/download/appcast.xml
```

- URL này đã được cấu hình trong `UpdateManager.cs`
- App luôn lấy appcast từ release mới nhất

### URL download installer

```
https://github.com/leehai1107/TQI/releases/download/v1.0.2/TQI.NBTeam-v1.0.2-win-x64.zip
```

Format: `https://github.com/{owner}/{repo}/releases/download/{tag}/{filename}` (workflow đã chuẩn hoá tên file)

## Troubleshooting

## Cách test nhanh sau khi tạo tag

1. Tạo tag mới (ví dụ `v1.0.2`) và push: `git push origin v1.0.2`
2. Chờ GitHub Actions hoàn tất (check tab Actions và Release đã có asset zip + appcast.xml)
3. Chạy app phiên bản cũ (ví dụ v1.0.1) trên máy người dùng
4. Quan sát: app tự tải bản mới, cài đặt và tự restart; tiêu đề sẽ hiển thị version mới

### Ứng dụng không phát hiện update

- Kiểm tra `appcast.xml` tại: https://github.com/leehai1107/TQI/releases/latest/download/appcast.xml
- Kiểm tra version trong appcast có lớn hơn version hiện tại không
- Xem Debug output trong Visual Studio (log `[UpdateManager]`)

### Download failed

- Kiểm tra URL download trong `appcast.xml`
- Kiểm tra asset zip có tồn tại trong Release đúng tag không
- Kiểm tra `length` có khớp kích thước file zip không

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
