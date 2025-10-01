create database QuanLyHopDongQuanTrac;
go
use QuanLyHopDongQuanTrac;
go

create table TaiKhoan (
	tenTK nvarchar(30) PRIMARY KEY,
	matKhau varchar(200),
	salt varchar(50), --salt để decode lại mật khẩu--
	vaiTro bit --0 là nhân viên, 1 là admin--
);
go

create table PhongBan (
	maPhong varchar(15) PRIMARY KEY,
	tenPhong nvarchar(30),
	truongPhong varchar(15),
);

go

create table NhanVien (
	maNV varchar(15) PRIMARY KEY,
	tenTK nvarchar(30),
	maPhong varchar(15),
	hoTen nvarchar(60),
	ngaySinh date,
	gioiTinh bit, --0 là nam, 1 là nữ--
	diaChi text,
	soDienThoai varchar(10),
	email varchar(50),
	constraint fk_NhanVien_Account foreign key (tenTK) references TaiKhoan(tenTK),
	constraint fk_NhanVien_PhongBan foreign key (maPhong) references PhongBan(maPhong)
);
go
alter table PhongBan add constraint fk_PhongBan_NhanVien foreign key (truongPhong) references NhanVien (maNV);
go
insert into TaiKhoan values ('admin', '$2a$10$hpojnVVHwzZjeWVs643z6urqQG2HQxxUlvH2If/ZLUY72Q915fNL.', 'qZKCaFaCkxQDF1fjxCeT7Q==', 1) --23092025--



--Cập nhật mật khẩu 
update TaiKhoan
set matKhau ='$2a$10$hpojnVVHwzZjeWVs643z6urqQG2HQxxUlvH2If/ZLUY72Q915fNL.'
WHERE tenTK = 'admin';

---proc lấy tài khoản 
CREATE PROCEDURE layTaikhoan 
    @tenTK NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tenTK, matKhau, vaiTro
    FROM TaiKhoan
    WHERE tenTK = @tenTK;
END
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

-- Tài khoản admin đã có rồi
-- Thêm tài khoản cho nhân viên

insert into TaiKhoan(tenTK, matKhau, salt, vaiTro)
values
('nv001@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 'abc123==', 0),
('nv002@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 'abc123==', 0),
('nv003@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 'abc123==', 0),
('nv004@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 'abc123==', 0),
('nv005@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 'abc123==', 0),
('nv006@company.com', '$2a$10$abcdefgh1234567890testhashxxxyyyzzz111222333444555', 'abc123==', 0);


-- Dữ liệu Nhân viên
insert into NhanVien(maNV, tenTK, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email)
values
('NV001', 'nv001@company.com', 'P001', N'Nguyễn Văn A', '1990-05-12', 0, N'Hà Nội', '0901234567', 'nv001@company.com'),
('NV002', 'nv002@company.com', 'P004', N'Trần Thị B', '1992-09-20', 1, N'Hà Nội', '0902345678', 'nv002@company.com'),
('NV003', 'nv003@company.com', 'P003', N'Lê Văn C', '1988-03-15', 0, N'Hải Phòng', '0913456789', 'nv003@company.com'),
('NV004', 'nv004@company.com', 'P002', N'Phạm Thị D', '1995-07-25', 1, N'Đà Nẵng', '0914567890', 'nv004@company.com'),
('NV005', 'nv005@company.com', 'P005', N'Hoàng Văn E', '1993-11-02', 0, N'Hồ Chí Minh', '0925678901', 'nv005@company.com'),
('NV006', 'nv006@company.com', 'P006', N'Vũ Thị F', '1996-01-10', 1, N'Cần Thơ', '0936789012', 'nv006@company.com');

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