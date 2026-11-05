# Selenium WebDriver với C# và NUnit

Dự án mẫu này minh họa cách xây dựng bộ kiểm thử giao diện theo hướng có thể mở rộng: Page Object Model, khởi tạo WebDriver tập trung, cấu hình qua biến môi trường, chờ tường minh và lưu ảnh chụp màn hình khi test thất bại.

## Yêu cầu

- .NET SDK 10
- Chrome, Firefox hoặc Microsoft Edge
- Kết nối Internet khi chạy test mẫu

Selenium Manager tự tìm hoặc tải WebDriver tương thích ở lần chạy đầu tiên. Test mẫu sử dụng trang thực hành công khai [The Internet](https://the-internet.herokuapp.com/login), tài khoản `tomsmith` / `SuperSecretPassword!`; vì vậy kết quả phụ thuộc vào dịch vụ bên ngoài.

## Cấu trúc

```text
Configuration/TestSettings.cs  Đọc và kiểm tra cấu hình từ biến môi trường
Driver/DriverFactory.cs        Tạo và cấu hình Chrome, Firefox hoặc Edge
Pages/BasePage.cs              Các thao tác và explicit wait dùng chung
Pages/LoginPage.cs             Page Object cho trang đăng nhập mẫu
Tests/BaseTest.cs              Vòng đời WebDriver và ảnh chụp khi thất bại
Tests/LoginTests.cs             Kịch bản đăng nhập thành công và thất bại
```

Mỗi page kế thừa `BasePage`; mỗi test fixture kế thừa `BaseTest`. Khi thêm nghiệp vụ, tạo Page Object tương ứng và viết test theo hành vi người dùng, không đặt locator hay thao tác Selenium trực tiếp trong test nếu có thể đóng gói chúng trong page.

## Chạy kiểm thử

```powershell
dotnet restore
dotnet test
```

Chọn cấu hình bằng biến môi trường trong PowerShell:

```powershell
$env:BROWSER = "chrome"       # chrome, firefox, edge
$env:HEADLESS = "true"        # true hoặc false; mặc định false
$env:BASE_URL = "https://the-internet.herokuapp.com"
$env:WAIT_SECONDS = "15"
dotnet test
```

Khi test thất bại, ảnh được lưu dưới `artifacts/screenshots` trong thư mục làm việc của test và được đính kèm vào kết quả NUnit. Nếu muốn xem trình duyệt, đặt `HEADLESS=false` hoặc bỏ biến này.

## Quy ước mở rộng

- Dùng locator ổn định (ưu tiên `id`, `name`, thuộc tính dành cho test) và explicit wait; tránh `Thread.Sleep`.
- Giữ dữ liệu test tách khỏi thao tác UI khi bộ test lớn dần; không đưa mật khẩu thật vào repository.
- Chia fixture theo tính năng, đặt tên test theo điều kiện và kết quả mong đợi.
- Chạy test trên trình duyệt/URL đã cấu hình trong CI, đồng thời xuất kết quả test để truy vết lỗi.

## Dùng AI có trách nhiệm

AI có thể hỗ trợ tạo ý tưởng test theo yêu cầu, đề xuất edge case, tạo khung Page Object, giải thích lỗi và rà soát mã. Luôn yêu cầu AI nêu giả định, kiểm tra locator/API, chạy test thực tế và tự đánh giá thay đổi trước khi đưa vào nhánh chính. Không gửi token, thông tin đăng nhập hoặc dữ liệu khách hàng vào prompt.

Ví dụ prompt:

> Từ yêu cầu [mô tả], đề xuất các kịch bản happy path, negative và boundary. Sau đó viết test NUnit theo Page Object hiện có; không dùng Thread.Sleep, không hard-code bí mật, và giải thích cách xác minh kết quả.

## Git và GitHub

Tạo repository GitHub trống, sau đó chạy tại thư mục dự án:

```powershell
git init
git add .
git commit -m "Build Selenium C# test automation framework"
git branch -M main
git remote add origin https://github.com/<username>/<repository>.git
git push -u origin main
```

Tạo nhánh riêng cho mỗi tính năng, commit nhỏ có nội dung rõ ràng và mở Pull Request trước khi gộp. Với portfolio xin việc, ghim repository này trên profile GitHub; bổ sung README có mục tiêu, cách chạy, cấu trúc, ảnh/báo cáo test và liên kết demo. Chỉ mô tả những gì repository thực sự có, không đưa thông tin xác thực vào lịch sử Git.
