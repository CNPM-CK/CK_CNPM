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
