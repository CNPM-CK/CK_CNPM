# 🌱 ECOS - Environmental Contract Oversight System

> Hệ thống Quản lý Hợp đồng Đơn hàng trong Quan trắc Môi trường

---

## 📖 Giới thiệu dự án

**ECOS (Environmental Contract Oversight System)** là hệ thống phần mềm được xây dựng nhằm số hóa và tự động hóa quy trình quản lý hợp đồng quan trắc môi trường.

Hệ thống hỗ trợ quản lý toàn diện từ:

- Quản lý khách hàng và hợp đồng
- Lập kế hoạch quan trắc
- Thu thập dữ liệu hiện trường và phòng thí nghiệm
- Xuất báo cáo kết quả
- Theo dõi tiến độ thực hiện
- Cảnh báo hợp đồng sắp hết hạn

Ngoài ra, hệ thống còn tích hợp **Trí tuệ nhân tạo (AI)** nhằm:
- Dự đoán khả năng tái ký hợp đồng
- Phân tích và dự báo mức độ ô nhiễm môi trường

---

# ✨ Tính năng nổi bật

## 📋 Quản lý Hợp đồng & Khách hàng
- Quản lý thông tin khách hàng
- Theo dõi vòng đời hợp đồng
- Quản lý hợp đồng theo chu kỳ:
  - Theo quý
  - Theo 6 tháng
- Theo dõi trạng thái đơn hàng

---

## 🔐 Phân quyền & Xác thực bảo mật
- Đăng nhập bằng tài khoản/mật khẩu
- Xác thực bằng Face ID
- Cấp lại mật khẩu qua OTP/SMS
- Phân quyền theo vai trò người dùng

---

## 🧪 Lập kế hoạch & Nhập liệu Quan trắc
- Phân công nhiệm vụ cho:
  - Phòng Hiện trường (HT)
  - Phòng Thí nghiệm (PTN)
- Nhập dữ liệu quan trắc
- Kiểm tra thông số môi trường
- Tự động cảnh báo khi vượt ngưỡng QCVN

---

## 🚨 Hệ thống cảnh báo tự động
- Gửi Email nhắc hạn hợp đồng
- Gửi SMS cảnh báo đơn hàng sắp quá hạn
- Thông báo gia hạn hợp đồng

---

## 🤖 Tích hợp Trí tuệ nhân tạo (AI)
- Dự đoán khả năng tái ký hợp đồng
- Dự báo mức độ ô nhiễm môi trường
- Ứng dụng Machine Learning với ML.NET

---

## ⚙️ Tiện ích mở rộng
- Voice Search (Tìm kiếm bằng giọng nói)
- Xuất báo cáo:
  - PDF
  - Excel
- Lưu trữ Audit Trail
- Theo dõi lịch sử thao tác người dùng

---

# 🛠️ Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Ngôn ngữ lập trình | C# |
| Framework | ASP.NET Core 8.0, WinForms |
| Kiến trúc | 3-Layer Architecture |
| Cơ sở dữ liệu | SQL Server 2022 |
| AI & Machine Learning | ML.NET |
| Gửi Email | SendGrid |
| Gửi SMS | Twilio |
| Voice Search | SpeechRecognition API |
| Bảo mật | AES-256 Encryption |

---

# 🏗️ Kiến trúc hệ thống

Hệ thống được phát triển theo mô hình **3-Layer Architecture**:

## 🖥️ GUI (Presentation Layer)
- Chứa giao diện người dùng
- Xử lý sự kiện
- Kiểm tra dữ liệu đầu vào

---

## 🧠 BLL (Business Logic Layer)
- Xử lý nghiệp vụ
- Tính toán dữ liệu
- Liên kết GUI và DAL

---

## 🗄️ DAL (Data Access Layer)
- Kết nối SQL Server
- Thực hiện truy vấn dữ liệu
- Làm việc với Stored Procedures

---

# 🚀 Hướng dẫn cài đặt

## 📌 Yêu cầu hệ thống

- Visual Studio 2022
- .NET 8.0 SDK
- SQL Server 2022
- Git

---

## 📥 Clone project

```bash
git clone https://github.com/your-username/your-repo-name.git
```

---

## 🗄️ Thiết lập cơ sở dữ liệu

### Bước 1:
Mở **SQL Server Management Studio (SSMS)**

### Bước 2:
Tạo database:

```sql
QuanLyHopDongQuanTrac
```

### Bước 3:
Chạy file script SQL trong thư mục:

```bash
/Database
```

---

## ⚙️ Cấu hình Connection String

Mở file:

```bash
appsettings.json
```

Hoặc cấu hình trong tầng:

```bash
DAL
```

Sau đó chỉnh sửa chuỗi kết nối SQL Server phù hợp với máy của bạn.

---

## ▶️ Chạy ứng dụng

### Bước 1:
Mở solution bằng **Visual Studio 2022**

### Bước 2:
Set project giao diện làm:

```bash
Startup Project
```

### Bước 3:
Nhấn:

```bash
F5
```

hoặc:

```bash
Start
```

để chạy chương trình.

---

# 👤 Tài khoản mặc định

Hệ thống không hỗ trợ người dùng tự đăng ký tài khoản.

Tài khoản sẽ được Admin cấp sẵn trong cơ sở dữ liệu.

---

# 📁 Cấu trúc thư mục

```bash
ECOS/
│
├── GUI/        # Giao diện người dùng
├── BLL/        # Business Logic Layer
├── DAL/        # Data Access Layer
├── Database/   # Script SQL
├── Assets/     # Hình ảnh, tài nguyên
└── README.md
```

---

# 👥 Đội ngũ phát triển

## Nhóm 19 - STech

| Họ và tên | Vai trò |
|---|---|
| Nguyễn Hoàng Sơn | Project Manager, Tester |
| Phan Đức Tài | Business Analyst, Tester |
| Tôn Quốc Thái | Developer, Designer |
| Trần Quang Thái | Developer, Designer |
| Phan Trí Tâm | Developer, Designer |

---

# 📄 Tài liệu & Quy chuẩn

- Phần mềm tuân thủ các quy chuẩn QCVN về Quan trắc môi trường
- Tài liệu hướng dẫn sử dụng (User Manual) được đính kèm trong dự án
- Bao gồm:
  - Sơ đồ thiết kế hệ thống
  - Tài liệu phân tích nghiệp vụ
  - Tài liệu triển khai

---

# 📌 Ghi chú

Đây là dự án học phần được xây dựng nhằm:
- Nghiên cứu quy trình quản lý quan trắc môi trường
- Ứng dụng AI trong phân tích dữ liệu
- Xây dựng hệ thống quản lý theo mô hình doanh nghiệp thực tế

---

# ⭐ ECOS - Smart Environmental Monitoring Management System
