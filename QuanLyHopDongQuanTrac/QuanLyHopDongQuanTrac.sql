use master
go

IF EXISTS (SELECT name 
           FROM sys.databases 
           WHERE name = 'QuanLyHopDongQuanTrac')
BEGIN
    DROP DATABASE QuanLyHopDongQuanTrac;
END
go

create database QuanLyHopDongQuanTrac;
go
use QuanLyHopDongQuanTrac;
go

create table TaiKhoan (
	tenTK varchar(30) PRIMARY KEY,
	matKhau varchar(100) NOT NULL,
	vaiTro bit NOT NULL--0 là nhân viên, 1 là admin--
);
go

create table PhongBan (
	maPhong varchar(15) PRIMARY KEY,
	tenPhong nvarchar(30) NOT NULL,
	truongPhong varchar(15)
);

go

create table NhanVien (
	maNV varchar(15) PRIMARY KEY,
	tenTK varchar(30) NOT NULL,
	maPhong varchar(15) NOT NULL,
	hoTen nvarchar(60) NOT NULL,
	ngaySinh date,
	gioiTinh bit, --0 là nam, 1 là nữ--
	diaChi nvarchar(150),
	soDienThoai varchar(10) NOT NULL,
	email varchar(50),
	ngayTao date
);
go

create table KhachHang (
	maKH varchar(15) PRIMARY KEY,
	tenDoanhNghiep nvarchar(100) NOT NULL,
	kyHieuDN nvarchar(20),
	diaChi nvarchar(150) NOT NULL,
	nguoiDaiDien nvarchar(50) NOT NULL,
	soDienThoaiKH varchar(10) NOT NULL
)
go

create table HopDong (
	maHD varchar(15) PRIMARY KEY,
	maKH varchar(15) NOT NULL,
	ngayKy date NOT NULL,
	ngayDuKien date NOT NULL,
	ngayThucTe date,
	trangThai bit --0 là không trễ hạn, 1 là trễ hạn--
)

create table DonHang (
	maDH varchar(15) PRIMARY KEY,
	maHD varchar(15) NOT NULL,
	noiDung text, --chứa các thông tin cần quan trắc--
	dotQuanTrac nvarchar(20) NOT NULL --6 tháng/1 lần hoặc theo quý--
)

create table NenMau (
	maNen varchar(15) PRIMARY KEY,
	maDH varchar(15) NOT NULL,
	moTa text NOT NULL
)

create table ChiTietDonHang (
	maDH varchar(15) NOT NULL,
	maNen varchar(15) NOT NULL
)
 
create table ThongSoMoiTruong (
	maTS varchar(15) PRIMARY KEY,
	tenTS nvarchar(30) NOT NULL,
	donVi nvarchar(15) NOT NULL,
	giaTriToiDa int,
	giaTriToiThieu int
)

create table ChiTietQuanTrac (
	maNen varchar(15) NOT NULL,
	maTS varchar(15) NOT NULL,
	maPhong varchar(15) NOT NULL
)

create table KetQua (
	maKQ varchar(15) PRIMARY KEY,
	maNen varchar(15) NOT NULL,
	maTS varchar(15) NOT NULL,
	nhanVienNhap varchar(15) NOT NULL,
	maBC varchar(15) NOT NULL,
	ngayDo date NOT NULL,
	giaTriDoDuoc int NOT NULL,
	ghiChu text
)

create table BaoCaoKetQua (
	maBC varchar(15) PRIMARY KEY,
	maDH varchar(15) NOT NULL,
	nguoiXuat varchar(15) NOT NULL,
	ngayXuat date NOT NULL,
	fileBaoCao varchar(50) NOT NULL
)


--CONSTRAINT--
--NhanVien--
alter table NhanVien add constraint fk_NhanVien_TaiKhoan foreign key (tenTK) references TaiKhoan(tenTK);
alter table NhanVien add constraint fk_NhanVien_PhongBan foreign key (maPhong) references PhongBan(maPhong);
--PhongBan--
alter table PhongBan add constraint fk_PhongBan_NhanVien foreign key (truongPhong) references NhanVien(maNV);
--HopDong--
alter table HopDong add constraint fk_HopDong_KhachHang foreign key (maKH) references KhachHang(maKH);
--DonHang--
alter table DonHang add constraint fk_DonHang_HopDong foreign key (maHD) references HopDong(maHD);
--NenMau--
alter table NenMau add constraint fk_NenMau_DonHang foreign key (maDH) references DonHang(maDH);
--ChiTietDonHang--
alter table ChiTietDonHang add constraint fk_ChiTietDonHang_DonHang foreign key (maDH) references DonHang(maDH);
alter table ChiTietDonHang add constraint fk_ChiTietDonHang_NenMau foreign key (maNen) references NenMau(maNen);
alter table ChiTietDonHang add constraint pk_ChiTietDonHang primary key (maDH, maNen);
--ChiTietQuanTrac--
alter table ChiTietQuanTrac add constraint fk_ChiTietQuanTrac_NenMau foreign key (maNen) references NenMau(maNen);
alter table ChiTietQuanTrac add constraint fk_ChiTietQuanTrac_ThongSoMoiTruong foreign key (maTS) references ThongSoMoiTruong(maTS);
alter table ChiTietQuanTrac add constraint pk_ChiTietQuanTrac primary key (maNen, maTS);
--KetQua--
alter table KetQua add constraint fk_KetQua_NenMau foreign key (maNen) references NenMau(maNen);
alter table KetQua add constraint fk_KetQua_ThongSoMoiTruong foreign key (maTS) references ThongSoMoiTruong(maTS);
alter table KetQua add constraint fk_KetQua_NhanVien foreign key (nhanVienNhap) references NhanVien(maNV);
alter table KetQua add constraint fk_KetQua_BaoCaoKetQua foreign key (maBC) references BaoCaoKetQua(maBC);
--BaoCaoKetQua--
alter table BaoCaoKetQua add constraint fk_BaoCaoKetQua_DonHang foreign key (MaDH) references DonHang(maDH);
alter table BaoCaoKetQua add constraint fk_BaoCaoKetQua_NhanVien foreign key (nguoiXuat) references NhanVien(maNV);
go

--PROCEDURES--
--proc lấy tài khoản--
CREATE PROCEDURE layTaikhoan 
    @tenTK NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tenTK, matKhau, vaiTro
    FROM TaiKhoan
    WHERE tenTK = @tenTK;
END
go
---Chèn dữ liệu để kiểm tra 
--Dữ liệu phòng ban
insert into PhongBan(maPhong, tenPhong, truongPhong)
values 
('P001', N'Phòng kinh doanh ', null),
('P002', N'Phòng kế hoạch ', null),
('P003', N'Phòng hiện trường ', null),
('P004', N'Phòng thí nghiệm  ', null),
('P005', N'Phòng kết quả ', null),
('P006', N'Phòng quan trắc', null);
go
-- Tài khoản admin đã có rồi
-- Thêm tài khoản cho nhân viên

insert into TaiKhoan(tenTK, matKhau, vaiTro)
values
('nv001@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 0),
('nv002@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555',  0),
('nv003@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 0),
('nv004@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 0),
('nv005@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 0),
('nv006@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 0);
go

-- Dữ liệu Nhân viên
insert into NhanVien(maNV, tenTK, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email)
values
('NV001', 'nv001@company.com', 'P001', N'Nguyễn Văn A', '1990-05-12', 0, N'Hà Nội', '0901234567', 'nv001@company.com'),
('NV002', 'nv002@company.com', 'P004', N'Trần Thị B', '1992-09-20', 1, N'Hà Nội', '0902345678', 'nv002@company.com'),
('NV003', 'nv003@company.com', 'P003', N'Lê Văn C', '1988-03-15', 0, N'Hải Phòng', '0913456789', 'nv003@company.com'),
('NV004', 'nv004@company.com', 'P002', N'Phạm Thị D', '1995-07-25', 1, N'Đà Nẵng', '0914567890', 'nv004@company.com'),
('NV005', 'nv005@company.com', 'P005', N'Hoàng Văn E', '1993-11-02', 0, N'Hồ Chí Minh', '0925678901', 'nv005@company.com'),
('NV006', 'nv006@company.com', 'P006', N'Vũ Thị F', '1996-01-10', 1, N'Cần Thơ', '0936789012', 'nv006@company.com');
go
--Procedure lấy danh sách nhân viên 
create procedure layDanhSachNhanVien
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        nv.maNV,
		 nv.tenTK,
		 nv.maPhong,
        nv.hoTen,
        nv.ngaySinh,
        CASE nv.gioiTinh WHEN 0 THEN N'Nam' ELSE N'Nữ' END AS gioiTinh,
        nv.diaChi,
        nv.soDienThoai,
		nv.email,
        pb.tenPhong
    FROM NhanVien nv
    LEFT JOIN PhongBan pb ON nv.maPhong = pb.maPhong;
END
GO
<<<<<<< HEAD


-- TẠO TỪ 4/10


-- TẠO BẢNG OTP VERIFICATION
CREATE TABLE OTPVerification (
    ID INT PRIMARY KEY IDENTITY(1,1),
    ContactInfo NVARCHAR(100) NOT NULL,  -- Email hoặc SĐT
    OTPCode NVARCHAR(6) NOT NULL,
    ExpiryTime DATETIME NOT NULL,
    IsUsed BIT DEFAULT 0,
    FailedAttempts INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Index để tìm kiếm nhanh
CREATE INDEX IX_OTP_Contact ON OTPVerification(ContactInfo, ExpiryTime);
GO

-- TẠO BẢNG LỊCH SỬ ĐẶT LẠI MẬT KHẨU
CREATE TABLE PasswordResetHistory (
    ID INT PRIMARY KEY IDENTITY(1,1),
    TenTK NVARCHAR(30),
    ContactInfo NVARCHAR(100),
    ResetMethod NVARCHAR(20), -- 'OTP' hoặc 'Email'
    ResetTime DATETIME DEFAULT GETDATE(),
    Success BIT,
    CONSTRAINT FK_PasswordReset_TaiKhoan FOREIGN KEY (TenTK) REFERENCES TaiKhoan(tenTK)
);
GO

-- STORED PROCEDURE: Kiểm tra email/SĐT tồn tại
CREATE PROCEDURE sp_KiemTraContactTonTai
    @ContactInfo NVARCHAR(100),
    @TonTai BIT OUTPUT,
    @TenTK NVARCHAR(30) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT @TenTK = nv.tenTK
    FROM NhanVien nv
    WHERE nv.email = @ContactInfo OR nv.soDienThoai = @ContactInfo;
    
    IF @TenTK IS NOT NULL
        SET @TonTai = 1
    ELSE
        SET @TonTai = 0
END
GO

-- STORED PROCEDURE: Lưu OTP
CREATE PROCEDURE sp_LuuOTP
    @ContactInfo NVARCHAR(100),
    @OTPCode NVARCHAR(6),
    @ExpiryMinutes INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ExpiryTime DATETIME = DATEADD(MINUTE, @ExpiryMinutes, GETDATE());
    
    -- Xóa OTP cũ chưa dùng
    DELETE FROM OTPVerification 
    WHERE ContactInfo = @ContactInfo AND IsUsed = 0;
    
    -- Thêm OTP mới
    INSERT INTO OTPVerification (ContactInfo, OTPCode, ExpiryTime, IsUsed, FailedAttempts)
    VALUES (@ContactInfo, @OTPCode, @ExpiryTime, 0, 0);
    
    SELECT 'Success' AS Result, @ExpiryTime AS ExpiryTime;
END
GO


-- STORED PROCEDURE: Xác thực OTP
CREATE PROCEDURE sp_XacThucOTP
    @ContactInfo NVARCHAR(100),
    @OTPCode NVARCHAR(6),
    @IsValid BIT OUTPUT,
    @Message NVARCHAR(200) OUTPUT,
    @FailedAttempts INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ID INT;
    DECLARE @Status NVARCHAR(20);
    
    -- Kiểm tra trạng thái OTP
    SELECT TOP 1
        @ID = ID,
        @FailedAttempts = FailedAttempts,
        @Status = CASE 
            WHEN ExpiryTime < GETDATE() THEN 'EXPIRED'
            WHEN IsUsed = 1 THEN 'USED'
            WHEN FailedAttempts >= 5 THEN 'LOCKED'
            WHEN OTPCode = @OTPCode THEN 'VALID'
            ELSE 'INVALID'
        END
    FROM OTPVerification 
    WHERE ContactInfo = @ContactInfo
    ORDER BY CreatedAt DESC;
    
    -- Xử lý theo trạng thái
    IF @Status = 'VALID'
    BEGIN
        -- OTP đúng
        UPDATE OTPVerification SET IsUsed = 1 WHERE ID = @ID;
        SET @IsValid = 1;
        SET @Message = N'Xác thực thành công';
    END
    ELSE IF @Status = 'INVALID'
    BEGIN
        -- OTP sai - Tăng số lần thất bại
        UPDATE OTPVerification SET FailedAttempts = FailedAttempts + 1 WHERE ID = @ID;
        SET @IsValid = 0;
        SET @FailedAttempts = @FailedAttempts + 1;
        SET @Message = N'Mã OTP không đúng';
    END
    ELSE IF @Status = 'EXPIRED'
    BEGIN
        SET @IsValid = 0;
        SET @Message = N'Mã OTP đã hết hạn';
    END
    ELSE IF @Status = 'USED'
    BEGIN
        SET @IsValid = 0;
        SET @Message = N'Mã OTP đã được sử dụng';
    END
    ELSE IF @Status = 'LOCKED'
    BEGIN
        SET @IsValid = 0;
        SET @Message = N'Bạn đã nhập sai quá 5 lần';
    END
    ELSE
    BEGIN
        SET @IsValid = 0;
        SET @Message = N'Không tìm thấy OTP';
    END
END
GO

-- STORED PROCEDURE: Cập nhật mật khẩu
CREATE PROCEDURE sp_CapNhatMatKhau
    @ContactInfo NVARCHAR(100),
    @MatKhauMoi NVARCHAR(200),
    @Salt VARCHAR(50),
    @Success BIT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @TenTK NVARCHAR(30);
    
    BEGIN TRY
        -- Lấy TenTK từ email/SĐT
        SELECT @TenTK = nv.tenTK
        FROM NhanVien nv
        WHERE nv.email = @ContactInfo OR nv.soDienThoai = @ContactInfo;
        
        IF @TenTK IS NULL
        BEGIN
            SET @Success = 0;
            SET @Message = N'Không tìm thấy tài khoản';
            RETURN;
        END
        
        -- Cập nhật mật khẩu
        UPDATE TaiKhoan 
        SET matKhau = @MatKhauMoi,
            salt = @Salt
        WHERE tenTK = @TenTK;
        
        -- Lưu lịch sử
        INSERT INTO PasswordResetHistory (TenTK, ContactInfo, ResetMethod, Success)
        VALUES (@TenTK, @ContactInfo, 'OTP', 1);
        
        SET @Success = 1;
        SET @Message = N'Đặt lại mật khẩu thành công';
    END TRY
    BEGIN CATCH
        SET @Success = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

-- STORED PROCEDURE: Dọn dẹp OTP cũ (tùy chọn)
CREATE PROCEDURE sp_DonDepOTPCu
AS
BEGIN
    DELETE FROM OTPVerification 
    WHERE CreatedAt < DATEADD(HOUR, -24, GETDATE());
    
    SELECT @@ROWCOUNT AS RowsDeleted;
END
GO
=======
>>>>>>> 7071c4bdd46fb5a04935eeb8ada9abbee7a7f388
