# ECOS - Hệ thống Quản lý Hợp đồng Đơn hàng trong Quan trắc Môi trường

## 📖 Tổng quan dự án
**ECOS (Environmental Contract Oversight System)** là hệ thống phần mềm được xây dựng nhằm số hóa và tự động hóa quy trình quản lý hợp đồng quan trắc môi trường. Hệ thống cung cấp giải pháp quản lý toàn diện từ khâu ký kết hợp đồng, lập kế hoạch quan trắc, thu thập dữ liệu hiện trường và phòng thí nghiệm, cho đến việc xuất báo cáo kết quả. Đặc biệt, dự án còn tích hợp Trí tuệ nhân tạo (AI) để dự đoán khả năng tái ký hợp đồng và mức độ ô nhiễm môi trường.

## ✨ Các tính năng nổi bật
* **Quản lý Hợp đồng & Khách hàng:** Tạo, theo dõi và quản lý vòng đời hợp đồng theo chu kỳ (quý/6 tháng).
* **Phân quyền & Xác thực bảo mật:** Hỗ trợ đăng nhập bằng mật khẩu, nhận diện khuôn mặt (Face ID) và cấp lại mật khẩu qua OTP/SMS.
* **Lập kế hoạch & Nhập liệu Quan trắc:** Phân công nhiệm vụ cụ thể cho Phòng Hiện trường (HT) và Phòng Thí nghiệm (PTN). Hệ thống tự động kiểm tra và cảnh báo nếu các thông số môi trường vượt ngưỡng QCVN.
* **Hệ thống cảnh báo tự động:** Tự động gửi Email/SMS thông báo khi đơn hàng sắp quá hạn hoặc nhắc nhở gia hạn hợp đồng.
* **Tích hợp Trí tuệ Nhân tạo (AI):** Phân tích và dự báo khả năng tái ký hợp đồng của khách hàng và dự báo tình trạng ô nhiễm môi trường bằng công nghệ ML.NET.
* **Tiện ích mở rộng:** Hỗ trợ tìm kiếm bằng giọng nói (Voice Search), xuất báo cáo đa định dạng (PDF, Excel) và lưu trữ Audit trail đầy đủ.

## 🛠️ Công nghệ sử dụng
Hệ thống được phát triển dựa trên kiến trúc **3-Layer (GUI, BLL, DAL)** với các công nghệ:
* **Ngôn ngữ & Framework:** C#, ASP.NET Core 8.0, Windows Forms (WinForms).
* **Cơ sở dữ liệu:** SQL Server 2022.
* **AI & Machine Learning:** ML.NET.
* **Dịch vụ tích hợp:** SendGrid (Gửi Email), Twilio (Gửi SMS), SpeechRecognition API (Tìm kiếm giọng nói).
* **Bảo mật:** Mã hóa dữ liệu AES-256.

## 🚀 Hướng dẫn cài đặt và chạy dự án

### Yêu cầu hệ thống:
* Visual Studio 2022 (hỗ trợ .NET 8.0).
* SQL Server 2022.
* Git.

### Các bước thực hiện:
1. **Clone mã nguồn về máy:**
   ```bash
   git clone https://github.com/your-username/your-repo-name.git
Thiết lập Cơ sở dữ liệu:
Mở SQL Server Management Studio (SSMS).
Chạy file script SQL đính kèm trong thư mục Database (hoặc tạo database QuanLyHopDongQuanTrac và chạy các câu lệnh tạo bảng, thủ tục được cung cấp).
Cấu hình chuỗi kết nối (Connection String):
Mở project bằng Visual Studio.
Tìm file cấu hình (ví dụ: appsettings.json hoặc cấu hình trong tầng DAL) và thay đổi chuỗi kết nối cho phù hợp với SQL Server của bạn.
Chạy ứng dụng:
Set project chứa giao diện (GUI) làm Startup Project.
Nhấn F5 hoặc nút Start trên Visual Studio để chạy phần mềm.
Tài khoản mặc định:
Hệ thống không hỗ trợ người dùng tự đăng ký, tài khoản sẽ do Admin cấp. Bạn có thể sử dụng tài khoản Admin mặc định đã thiết lập trong database để đăng nhập.
📁 Kiến trúc thư mục (3-Tier Architecture)
GUI (Presentation Layer): Chứa các form giao diện người dùng (WinForms), xử lý sự kiện và validation dữ liệu đầu vào.
BLL (Business Logic Layer): Chứa các class xử lý quy tắc nghiệp vụ, tính toán và liên kết giữa GUI và DAL.
DAL (Data Access Layer): Chịu trách nhiệm kết nối, truy vấn và thao tác trực tiếp với cơ sở dữ liệu SQL Server thông qua các Stored Procedures.
👥 Đội ngũ phát triển (Nhóm 19 - STech)
Họ và tên                   Vai trò
Nguyễn Hoàng Sơn            Project Manager, Tester
Phan Đức Tài                Business Analyst, Tester
Tôn Quốc Thái               Developer, Designer
Trần Quang Thái             Developer, Designer
Phan Trí Tâm                Developer, Designer
📄 Giấy phép và Tài liệu tham khảo
Phần mềm tuân thủ các quy định hiện hành về Quan trắc môi trường (QCVN).
Tài liệu Hướng dẫn sử dụng (User Manual) và Sơ đồ thiết kế chi tiết được đính kèm trong dự án.
