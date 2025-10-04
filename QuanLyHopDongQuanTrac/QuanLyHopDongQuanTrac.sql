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

--2/10/2025 
---Taoj proc thêm nhân viên 
create procedure ThemNhanVien
	 @tenTK nvarchar(30),
    @maPhong varchar(15),
    @hoTen nvarchar(60),
    @ngaySinh date,
    @gioiTinh bit,
    @diaChi text,
    @soDienThoai varchar(10),
    @Email varchar(50)
as 
begin
	 set nocount on ;
	 declare @maNV varchar(15) ;
	 declare @so int  ;

	 select @so = cast(substring(maNV, 3, len(maNV)) as int)
    from NhanVien
    where maNV = (select max(maNV) from NhanVien);
	 if @so is null 
        set @so = 0;

    set @so = @so + 1;

    -- Format lại mã NV (NV + số có 3 chữ số)
    set @maNV = 'NV' + right('000' + cast(@so as varchar(3)), 3);

    -- Thêm nhân viên mới
    insert into NhanVien (maNV, tenTK, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email)
    values (@maNV, @tenTK, @maPhong, @hoTen, @ngaySinh, @gioiTinh, @diaChi, @soDienThoai, @Email);

    -- Xuất mã NV mới tạo ra để biết
    --select @maNV as NewMaNV;
end
go
--Proc lấy ds phòng ban
create proc LayDSPhongBan
as 
begin
	set nocount on;
	select maPhong, tenPhong
	from PhongBan



end

