USE [master];
GO
IF DB_ID(N'QuanLyHopDongQuanTrac') IS NOT NULL
BEGIN
    ALTER DATABASE [QuanLyHopDongQuanTrac] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [QuanLyHopDongQuanTrac];
END
IF DB_ID(N'QuanLyHopDongQuanTrac222') IS NOT NULL
BEGIN
    ALTER DATABASE [QuanLyHopDongQuanTrac222] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [QuanLyHopDongQuanTrac222];
END
GO
CREATE DATABASE [QuanLyHopDongQuanTrac];
GO
USE [QuanLyHopDongQuanTrac]
GO
/****** Object:  UserDefinedTableType [dbo].[ChiTietThongSoType]    Script Date: 10/30/2025 12:14:41 AM ******/
CREATE TYPE [dbo].[ChiTietThongSoType] AS TABLE(
	[maTS] [varchar](15) NULL,
	[maPhong] [varchar](15) NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[fn_EmailHopLe]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Kiểm tra email hợp lệ 
CREATE FUNCTION [dbo].[fn_EmailHopLe](@Email NVARCHAR(100))
RETURNS BIT
AS
BEGIN
    IF @Email NOT LIKE '%_@_%._%' 
       OR @Email LIKE '%..%' 
       OR @Email LIKE '%.@%' 
       OR RIGHT(@Email, 4) NOT IN ('.com', '.net', '.org', '.edu', '.gov', '.vn')
        RETURN 0;
    RETURN 1;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_SoDienThoaiHopLe]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Kiểm tra số điện thoại 
CREATE FUNCTION [dbo].[fn_SoDienThoaiHopLe](@sdt VARCHAR(20))
RETURNS BIT
AS
BEGIN
    IF @sdt LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
        RETURN 1;
    RETURN 0;
END;
GO
/****** Object:  UserDefinedFunction [dbo].[fn_TaoMaDN]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_TaoMaDN]()
RETURNS VARCHAR(15)
AS
BEGIN
    DECLARE @newMaDN VARCHAR(15);
    DECLARE @maxNum INT;
    
    SELECT @maxNum = MAX(CAST(SUBSTRING(maDN, 3, LEN(maDN)) AS INT))
    FROM Dot_Nen
    WHERE maDN LIKE 'DN%';
    
    IF @maxNum IS NULL
        SET @maxNum = 0;
    
    SET @newMaDN = 'DN' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
    
    RETURN @newMaDN;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_TaoMaDNTS]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[fn_TaoMaDNTS]()
RETURNS VARCHAR(15)
AS
BEGIN
    DECLARE @newMaDNTS VARCHAR(15);
    DECLARE @maxNum INT;
    
    SELECT @maxNum = MAX(CAST(SUBSTRING(maDNTS, 5, LEN(maDNTS)) AS INT))
    FROM Dot_Nen_Ts
    WHERE maDNTS LIKE 'DNTS%';
    
    IF @maxNum IS NULL
        SET @maxNum = 0;
    
    SET @newMaDNTS = 'DNTS' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
    
    RETURN @newMaDNTS;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_TaoMaDot]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- BƯỚC 9: TẠO FUNCTION TẠO MÃ ĐỢT TỰ ĐỘNG
-- =============================================
CREATE   FUNCTION [dbo].[fn_TaoMaDot]()
RETURNS VARCHAR(15)
AS
BEGIN
    DECLARE @newMaDot VARCHAR(15);
    DECLARE @maxNum INT;
    
    SELECT @maxNum = MAX(CAST(SUBSTRING(maDot, 3, LEN(maDot)) AS INT))
    FROM DotQuanTrac
    WHERE maDot LIKE 'DT%';
    
    IF @maxNum IS NULL
        SET @maxNum = 0;
    
    SET @newMaDot = 'DT' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
    
    RETURN @newMaDot;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_TaoMaThongSo]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_TaoMaThongSo]()
RETURNS VARCHAR(15)
AS
BEGIN
    DECLARE @newMaTS VARCHAR(15);
    DECLARE @maxNum INT;
    
    -- Lấy số lớn nhất trong phần số của mã
    SELECT @maxNum = MAX(CAST(SUBSTRING(maTS, 3, LEN(maTS)) AS INT))
    FROM ThongSoMoiTruong
    WHERE maTS LIKE 'TS%';
    
    IF @maxNum IS NULL
        SET @maxNum = 0;
    
    -- Tạo mã mới với 4 chữ số
    SET @newMaTS = 'TS' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
    
    RETURN @newMaTS;
END
GO

/****** Object:  UserDefinedFunction [dbo].[fn_TaoMaKQ]    Script Date: 11/05/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_TaoMaKQ]()
RETURNS VARCHAR(15)
AS
BEGIN
    DECLARE @newMaKQ VARCHAR(15);
    DECLARE @maxNum INT;
    
    SELECT @maxNum = MAX(CAST(SUBSTRING(maKQ, 3, LEN(maKQ)) AS INT))
    FROM KetQuaHeader
    WHERE maKQ LIKE 'KQ%';
    
    IF @maxNum IS NULL
        SET @maxNum = 0;
    
    SET @newMaKQ = 'KQ' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
    
    RETURN @newMaKQ;
END
GO
/****** Object:  UserDefinedFunction [dbo].[fn_TuoiHopLe]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_TuoiHopLe](@ngaySinh DATE)
RETURNS BIT
AS
BEGIN
    DECLARE @tuoi INT = DATEDIFF(YEAR, @ngaySinh, GETDATE());
    IF (DATEADD(YEAR, @tuoi, @ngaySinh) > GETDATE())
        SET @tuoi = @tuoi - 1;

    IF @tuoi BETWEEN 16 AND 65 RETURN 1;
    RETURN 0;
END;
GO
/****** Object:  Table [dbo].[BaoCaoKetQua]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BaoCaoKetQua](
	[maBC] [varchar](15) NOT NULL,
	[nguoiXuat] [varchar](15) NOT NULL,
	[ngayXuat] [date] NOT NULL,
	[fileBaoCao] [varchar](50) NOT NULL,
	[maDot] [varchar](15) NULL,
 CONSTRAINT [PK_BaoCaoKetQua] PRIMARY KEY CLUSTERED 
(
	[maBC] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietDotQuanTrac]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietDotQuanTrac](
	[maDot] [varchar](15) NOT NULL,
	[maNen] [varchar](15) NOT NULL,
 CONSTRAINT [pk_ChiTietDotQuanTrac] PRIMARY KEY CLUSTERED 
(
	[maDot] ASC,
	[maNen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChiTietQuanTrac]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChiTietQuanTrac](
	[maNen] [varchar](15) NOT NULL,
	[maTS] [varchar](15) NOT NULL,
	[maPhong] [varchar](15) NOT NULL,
 CONSTRAINT [pk_ChiTietQuanTrac] PRIMARY KEY CLUSTERED 
(
	[maNen] ASC,
	[maTS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Dot_Nen]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dot_Nen](
	[maDN] [varchar](15) NOT NULL,
	[maDot] [varchar](15) NOT NULL,
	[maNen] [varchar](15) NOT NULL,
	[tenViTri] [nvarchar](200) NULL,
	[toaDo] [nvarchar](100) NULL,
	[ghiChu] [text] NULL,
PRIMARY KEY CLUSTERED 
(
	[maDN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[TrangThai_KhachHang](
    [maTrangThai] INT IDENTITY(1,1) PRIMARY KEY,
    [tenTrangThai] NVARCHAR(50) NOT NULL
);
GO 


/****** Object:  Table [dbo].[Dot_Nen_Ts]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dot_Nen_Ts](
	[maDNTS] [varchar](15) NOT NULL,
	[maDN] [varchar](15) NOT NULL,
	[maTS] [varchar](15) NOT NULL,
	[tenTS] [nvarchar](50) NOT NULL,
	[donVi] [nvarchar](15) NULL,
	[giaTriToiThieu] [float] NULL,
	[giaTriToiDa] [float] NULL,
	[phuongPhap] [nvarchar](200) NULL,
	[maPhong] [varchar](15) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[maDNTS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DotQuanTrac]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DotQuanTrac](
	[maDot] [varchar](15) NOT NULL,
	[maHD] [varchar](15) NULL,
	[noiDung] [nvarchar](max) NULL,
	[dotQuanTrac] [nvarchar](100) NOT NULL,
	[ngayBatDau] [date] NULL,
	[ngayDuKien] [date] NULL,
	[ngayTraKQ] [date] NULL,
	[trangThai] [int] NULL,
	[thuTuDot] [int] NULL
 CONSTRAINT [PK_DotQuanTrac] PRIMARY KEY CLUSTERED 
(
	[maDot] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HopDong]    Script Date: 10/17/2025 9:01:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HopDong](
	[maHD] [varchar](15) NOT NULL,
	[maKH] [varchar](15) NOT NULL,
	[ngayKy] [date] NOT NULL,
	[ngayKetThucHD] [date] NOT NULL,
	[trangThai] [varchar](15) NULL,
	[tanSuatQuanTrac] [varchar](15) NOT NULL DEFAULT 'TSQT01',
	[soHD] [nvarchar](20) NULL DEFAULT N'Chưa cập nhật'
 CONSTRAINT [PK_HopDong] PRIMARY KEY CLUSTERED 
(
	[maHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KetQua]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KetQua](
	[maKQ] [varchar](15) NOT NULL,
	[nhanVienNhap] [varchar](15) NOT NULL,
	[ngayDo] [date] NOT NULL,
	[giaTriDoDuoc] [int] NOT NULL,
	[maDNTS] [varchar](15) NULL,
 CONSTRAINT [PK_KetQua] PRIMARY KEY CLUSTERED 
(
	[maKQ] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
)
GO
/****** Object:  Table [dbo].[KhachHang]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KhachHang](
	[maKH] [varchar](15) NOT NULL,
	[tenDoanhNghiep] [nvarchar](100) NOT NULL,
	[kyHieuDN] [nvarchar](20) NULL,
	[diaChi] [nvarchar](150) NOT NULL,
	[nguoiDaiDien] [nvarchar](50) NOT NULL,
	[soDienThoaiKH] [varchar](10) NOT NULL,
	[maSoThue] [varchar](20) NULL,
	[emailNguoiDaiDien] [varchar](100) NULL,
	[emailDoanhNghiep] [varchar](100) NULL,
	[trangThai] [int] NOT NULL,
 CONSTRAINT [PK_KhachHang] PRIMARY KEY CLUSTERED 
(
	[maKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NenMau]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NenMau](
	[maNen] [varchar](15) NOT NULL,
	[moTa] [nvarchar](max) NULL,
	[tenNenMau] [nvarchar](100) NULL,
 CONSTRAINT [PK_NenMau] PRIMARY KEY CLUSTERED 
(
	[maNen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NhanVien]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NhanVien](
	[maNV] [varchar](15) NOT NULL,
	[maPhong] [varchar](15) NOT NULL,
	[hoTen] [nvarchar](60) NOT NULL,
	[ngaySinh] [date] NULL,
	[gioiTinh] [bit] NULL,
	[diaChi] [nvarchar](150) NULL,
	[soDienThoai] [varchar](10) NOT NULL,
	[email] [varchar](50) NULL,
	[ngayTao] [date] NULL,
	[trangThai] [int] NOT NULL,
	[daXoa] BIT NOT NULL DEFAULT 0 ,
	[anhDaiDien] NVARCHAR(255) NULL,
 CONSTRAINT [PK_NhanVien] PRIMARY KEY CLUSTERED 
(
	[maNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

----Bảng trạng thái nhân viên 
CREATE TABLE [dbo].[TrangThai_NhanVien] (
    [maTrangThai] INT IDENTITY(1,1) PRIMARY KEY,
    [tenTrangThai] NVARCHAR(50) NOT NULL
);

/****** Object:  Table [dbo].[OTPVerification]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OTPVerification](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ContactInfo] [nvarchar](100) NOT NULL,
	[OTPCode] [nvarchar](6) NOT NULL,
	[ExpiryTime] [datetime] NOT NULL,
	[IsUsed] [bit] NULL,
	[FailedAttempts] [int] NULL,
	[CreatedAt] [datetime] NULL,
 CONSTRAINT [PK_OTPVerification] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhongBan]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhongBan](
	[maPhong] [varchar](15) NOT NULL,
	[tenPhong] [nvarchar](30) NOT NULL,
	[truongPhong] [varchar](15) NULL,
 CONSTRAINT [PK_PhongBan] PRIMARY KEY CLUSTERED 
(
	[maPhong] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TaiKhoan]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TaiKhoan](
	[tenTK] [varchar](50) NOT NULL,
	[matKhau] [varchar](100) NOT NULL,
	[vaiTro] [bit] NOT NULL,
 CONSTRAINT [PK_TaiKhoan] PRIMARY KEY CLUSTERED 
(
	[tenTK] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ThongSoMoiTruong]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ThongSoMoiTruong](
	[maTS] [varchar](15) NOT NULL,
	[tenTS] [nvarchar](30) NOT NULL,
	[giaTriToiDa] [float] NULL,
	[giaTriToiThieu] [float] NULL,
	[donVi] [nvarchar](15) NULL,
	[phuongPhap] [nvarchar](200) NULL,
 CONSTRAINT [PK_ThongSoMoiTruong] PRIMARY KEY CLUSTERED 
(
	[maTS] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[trangThaiHD]    Script Date: 10/17/2025 9:01:46 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[trangThaiHD](
	[maTT] [varchar](15) NOT NULL,
	[tenTT] [nvarchar](30) NOT NULL
 CONSTRAINT [PK_trangThaiHD] PRIMARY KEY CLUSTERED 
(
	[maTT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tanSuatQT](
	[maTSQT] [varchar](15) NOT NULL,
	[tenTSQT] [nvarchar](30) NOT NULL
 CONSTRAINT [PK_tanSuatQT] PRIMARY KEY CLUSTERED 
(
	[maTSQT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TrangThai_Dot]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TrangThai_Dot](
	[maTrangThai] [int] NOT NULL,
	[tenTrangThai] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[maTrangThai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[KetQuaHeader]    Script Date: 11/05/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KetQuaHeader](
    [maKQ] [varchar](15) NOT NULL,
    [maDot] [varchar](15) NOT NULL,
    [nhanVienNhap] [varchar](15) NOT NULL,
    [ngayTao] [datetime] NOT NULL DEFAULT GETDATE(),
    [ngayTraKQ] [date] NULL,
    [trangThaiXacNhan] [bit] NOT NULL DEFAULT 0,
    [ghiChu] [nvarchar](max) NULL,
 CONSTRAINT [PK_KetQuaHeader] PRIMARY KEY CLUSTERED ([maKQ] ASC)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[KetQuaNenMau]    Script Date: 11/05/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KetQuaNenMau](
    [maKQNen] [varchar](15) NOT NULL,
    [maKQ] [varchar](15) NOT NULL,
    [maNen] [varchar](15) NOT NULL,
    [viTri] [nvarchar](200) NULL,
    [toaDo] [nvarchar](100) NULL,
 CONSTRAINT [PK_KetQuaNenMau] PRIMARY KEY CLUSTERED ([maKQNen] ASC)
) ON [PRIMARY]
GO

CREATE TABLE dbo.ChatSession (
    MaPhien        INT IDENTITY(1,1) PRIMARY KEY,
    TenTK          VARCHAR(50)  NOT NULL,
    TenPhienChat   NVARCHAR(200) NULL,         
    CreatedAt      DATETIME     NOT NULL DEFAULT GETDATE(),
    UpdatedAt      DATETIME     NOT NULL DEFAULT GETDATE(),
    DaXoa          BIT          NOT NULL DEFAULT 0
);
GO

CREATE TABLE dbo.ChatMessage (
    MaTinNhan      INT IDENTITY(1,1) PRIMARY KEY,
    MaPhien        INT          NOT NULL,
    ThuTu          INT          NOT NULL,
    VaiTroGui      VARCHAR(20)  NOT NULL,  
    TenNguoiGui    NVARCHAR(100) NULL,      
    NoiDung        NVARCHAR(MAX) NOT NULL,
    ThoiGianTao    DATETIME     NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_ChatMessage_ChatSession
        FOREIGN KEY (MaPhien) REFERENCES dbo.ChatSession(MaPhien)
);
GO



/****** Object:  Table [dbo].[KetQuaChiTiet]    Script Date: 11/05/2025 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[KetQuaChiTiet](
    [maKQCT] [varchar](15) NOT NULL,
    [maKQNen] [varchar](15) NOT NULL,
    [maTS] [varchar](15) NOT NULL,
    [donVi] [nvarchar](15) NULL,
    [phuongPhapPhanTich] [nvarchar](200) NULL,
    [ketQua] [float] NOT NULL,
    [gioiHanPhatHien] [nvarchar](50) NULL,
    [qcvn] [nvarchar](50) NULL,
 CONSTRAINT [PK_KetQuaChiTiet] PRIMARY KEY CLUSTERED ([maKQCT] ASC)
) ON [PRIMARY]
GO

--ThaiTon them bang AI
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AI_TaiKy](
    [maAITaiKy]                 VARCHAR(50)   NOT NULL,   -- VD: KH001_HD001_D003
    [maKH]                      VARCHAR(15)   NOT NULL,
    [maHD]                      VARCHAR(15)   NOT NULL,
    [thuTuDot]                  INT           NOT NULL,

    -- Feature hợp đồng
    [thoiHanHopDong_Thang]      INT           NOT NULL,

    -- One-hot tần suất
    [tanSuat_KhongCo]           BIT           NOT NULL,
    [tanSuat_TheoQuy]           BIT           NOT NULL,
    [tanSuat_6Thang]            BIT           NOT NULL,

    -- Số đợt & tỉ lệ hoàn thành
    [soDot_DuKien]              INT           NOT NULL,
    [soDot_HoanThanh_ToiHienTai] INT          NOT NULL,
    [tiLeHoanThanh]             FLOAT         NOT NULL,

    -- Trễ hạn
    [trungBinh_TreHan]          FLOAT         NULL,
    [treHan_ToiDa]              INT           NULL,
    [treHan_NhoNhat]            INT           NULL,
    [soDot_BiTre]               INT           NULL,
    [tiLeDotTre]                FLOAT         NULL,

    -- Thời lượng xử lý
    [trungBinh_ThoiLuongXuLy]   FLOAT         NULL,
    [xuLy_ToiDa]                INT           NULL,
    [xuLy_NhoNhat]              INT           NULL,

    -- Label ML
    [tiepTuc_HopTac]            BIT           NULL,

    [ngaySnapshot]              DATETIME      NOT NULL DEFAULT GETDATE(),

	[duBao_TiepTuc] FLOAT NULL,   -- Xác suất tiếp tục hợp tác (0–1)
    [duBao_Label]   BIT   NULL,   -- Nhãn dự đoán 0/1

    CONSTRAINT [PK_AI_TaiKy] PRIMARY KEY CLUSTERED 
    (
        [maAITaiKy] ASC
    )
) ON [PRIMARY]
GO

-- =============================================
-- FOREIGN KEYS CHO HỆ THỐNG KẾT QUẢ
-- =============================================
ALTER TABLE [dbo].[KetQuaHeader] WITH CHECK ADD CONSTRAINT [FK_KetQuaHeader_DotQuanTrac] 
FOREIGN KEY([maDot]) REFERENCES [dbo].[DotQuanTrac]([maDot]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[KetQuaHeader] WITH CHECK ADD CONSTRAINT [FK_KetQuaHeader_NhanVien] 
FOREIGN KEY([nhanVienNhap]) REFERENCES [dbo].[NhanVien]([maNV])
GO

ALTER TABLE [dbo].[KetQuaNenMau] WITH CHECK ADD CONSTRAINT [FK_KetQuaNenMau_Header] 
FOREIGN KEY([maKQ]) REFERENCES [dbo].[KetQuaHeader]([maKQ]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[KetQuaNenMau] WITH CHECK ADD CONSTRAINT [FK_KetQuaNenMau_NenMau] 
FOREIGN KEY([maNen]) REFERENCES [dbo].[NenMau]([maNen])
GO

ALTER TABLE [dbo].[KetQuaChiTiet] WITH CHECK ADD CONSTRAINT [FK_KetQuaChiTiet_NenMau] 
FOREIGN KEY([maKQNen]) REFERENCES [dbo].[KetQuaNenMau]([maKQNen]) ON DELETE CASCADE
GO

ALTER TABLE [dbo].[KetQuaChiTiet] WITH CHECK ADD CONSTRAINT [FK_KetQuaChiTiet_ThongSo] 
FOREIGN KEY([maTS]) REFERENCES [dbo].[ThongSoMoiTruong]([maTS])

GO
INSERT [dbo].[Dot_Nen] ([maDN], [maDot], [maNen], [tenViTri], [toaDo], [ghiChu]) VALUES (N'DN0001', N'DT0001', N'NM0002', N'Song Hong', N'tgh', N'khong')
GO
INSERT [dbo].[Dot_Nen_Ts] ([maDNTS], [maDN], [maTS], [tenTS], [donVi], [giaTriToiThieu], [giaTriToiDa], [phuongPhap], [maPhong]) VALUES (N'DNTS0001', N'DN0001', N'TS0001', N'Amonica', N'mg/L', 0, 1000, N'ikkk', N'P003')
GO
INSERT [dbo].[DotQuanTrac] ([maDot], [maHD], [noiDung], [dotQuanTrac], [ngayBatDau], [ngayDuKien], [ngayTraKQ], [trangThai], [thuTuDot]) VALUES (N'DT0001', N'HD011', N'quan trac dinh ki', N'quy 4', CAST(N'2025-11-08' AS Date), CAST(N'2026-01-09' AS Date), CAST(N'2025-11-08' AS Date), 1, 1)
GO
-- Đang hiệu lực
INSERT [dbo].[HopDong] ([maHD], [maKH], [ngayKy], [ngayKetThucHD], [trangThai], [tanSuatQuanTrac], [soHD]) VALUES (N'HD001', N'KH001', CAST(N'2025-01-15' AS Date), CAST(N'2025-06-30' AS Date), 'TT03', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD002', N'KH001', CAST(N'2025-07-01' AS Date), CAST(N'2025-12-31' AS Date), 'TT01', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD003', N'KH002', CAST(N'2025-02-10' AS Date), CAST(N'2025-08-10' AS Date), 'TT02', 'TSQT01', 'HD2025/24/2');
INSERT [dbo].[HopDong] VALUES (N'HD004', N'KH003', CAST(N'2025-03-05' AS Date), CAST(N'2025-09-05' AS Date), 'TT02', 'TSQT02', null);
INSERT [dbo].[HopDong] VALUES (N'HD005', N'KH004', CAST(N'2025-01-20' AS Date), CAST(N'2025-07-20' AS Date), 'TT03', 'TSQT02', null);
INSERT [dbo].[HopDong] VALUES (N'HD006', N'KH004', CAST(N'2025-08-01' AS Date), CAST(N'2026-01-31' AS Date), 'TT01', 'TSQT02', null);
INSERT [dbo].[HopDong] VALUES (N'HD007', N'KH005', CAST(N'2025-02-15' AS Date), CAST(N'2025-08-15' AS Date), 'TT02', 'TSQT03', null);
INSERT [dbo].[HopDong] VALUES (N'HD008', N'KH005', CAST(N'2025-09-01' AS Date), CAST(N'2026-03-01' AS Date), 'TT01', 'TSQT03', null);
INSERT [dbo].[HopDong] VALUES (N'HD009', N'KH006', CAST(N'2025-03-10' AS Date), CAST(N'2025-09-10' AS Date), 'TT02', 'TSQT03', 'HD2025/24213');
INSERT [dbo].[HopDong] VALUES (N'HD010', N'KH007', CAST(N'2025-04-01' AS Date), CAST(N'2025-10-01' AS Date), 'TT02', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD011', N'KH007', CAST(N'2025-10-15' AS Date), CAST(N'2026-04-15' AS Date), 'TT01', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD012', N'KH008', CAST(N'2025-05-05' AS Date), CAST(N'2025-11-05' AS Date), 'TT01', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD013', N'KH009', CAST(N'2025-01-25' AS Date), CAST(N'2025-07-25' AS Date), 'TT03', 'TSQT02', null);
INSERT [dbo].[HopDong] VALUES (N'HD014', N'KH009', CAST(N'2025-08-10' AS Date), CAST(N'2026-02-10' AS Date), 'TT01', 'TSQT02', null);
INSERT [dbo].[HopDong] VALUES (N'HD015', N'KH010', CAST(N'2025-06-01' AS Date), CAST(N'2025-12-01' AS Date), 'TT01', 'TSQT02', null);
INSERT [dbo].[HopDong] VALUES (N'HD016', N'KH011', CAST(N'2025-03-20' AS Date), CAST(N'2025-09-20' AS Date), 'TT02', 'TSQT03', null);
INSERT [dbo].[HopDong] VALUES (N'HD017', N'KH011', CAST(N'2025-10-01' AS Date), CAST(N'2026-04-01' AS Date), 'TT01', 'TSQT03', null);
INSERT [dbo].[HopDong] VALUES (N'HD018', N'KH012', CAST(N'2025-02-28' AS Date), CAST(N'2025-08-28' AS Date), 'TT02', 'TSQT03', null);
INSERT [dbo].[HopDong] VALUES (N'HD019', N'KH013', CAST(N'2025-04-15' AS Date), CAST(N'2025-10-15' AS Date), 'TT02', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD020', N'KH014', CAST(N'2025-05-20' AS Date), CAST(N'2025-11-20' AS Date), 'TT01', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD021', N'KH015', CAST(N'2025-01-10' AS Date), CAST(N'2025-07-10' AS Date), 'TT03', 'TSQT01', null);
INSERT [dbo].[HopDong] VALUES (N'HD022', N'KH015', CAST(N'2025-07-20' AS Date), CAST(N'2026-01-20' AS Date), 'TT01', 'TSQT02', null);


GO
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH001', N'Công ty ABC', N'ABC', N'Hà Nội', N'Nguyễn Văn A', N'0987654321', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH002', N'Công ty heo', N'CTB', N'Xã Định Hưng, Huyện Yên Định, Thanh Hóa', N'Trần Quang Thái', N'0910737726', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH003', N'Công ty bò con', NULL, N'Xã Mường Bang, Huyện Phù Yên, Sơn La', N'Nguyễn Hoàng Sơn', N'0022332200', NULL, NULL, NULL, 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH004', N'Công ty TNHH Môi Trường Xanh', N'MTXANH', N'Số 15 Lê Lợi, Quận 1, TP. Hồ Chí Minh', N'Phạm Văn Hùng', N'0908123456', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH005', N'Công ty CP Công Nghiệp Thực Phẩm Việt Nam', N'CNTP', N'KCN Tân Bình, Huyện Bắc Tân Uyên, Bình Dương', N'Lê Thị Minh', N'0917234567', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH006', N'Công ty TNHH Dệt May Hòa Phát', N'DMHP', N'Số 234 Quốc lộ 1A, Thị xã Bình Minh, Vĩnh Long', N'Nguyễn Minh Tuấn', N'0926345678', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH007', N'Công ty CP Hóa Chất An Phước', N'HCAP', N'Khu công nghiệp Long Hậu, Huyện Cần Giuộc, Long An', N'Trần Quốc Anh', N'0935456789', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH008', N'Công ty TNHH Chế Biến Thủy Sản Minh Hải', N'TSMH', N'Xã Hòa Thạnh, Thị xã Tân Châu, An Giang', N'Võ Thị Lan', N'0944567890', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH009', N'Công ty CP Giấy Tân Mai', N'GTM', N'KCN Phố Nối A, Huyện Yên Mỹ, Hưng Yên', N'Đặng Văn Phong', N'0953678901', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH010', N'Công ty TNHH Nhựa Đông Á', N'NDA', N'Số 88 Tỉnh lộ 15, Huyện Bình Chánh, TP. Hồ Chí Minh', N'Hoàng Minh Đức', N'0962789012', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH011', N'Công ty CP Dược Phẩm Hà Tây', N'DPHT', N'Đường Lê Trọng Tấn, Quận Hà Đông, Hà Nội', N'Bùi Thị Hương', N'0971890123', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH012', N'Công ty TNHH Sản Xuất Đồ Gỗ Phú Thọ', N'GOPT', N'Xã Thanh Minh, Thành phố Việt Trì, Phú Thọ', N'Lương Văn Sơn', N'0980901234', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH013', N'Công ty CP Xi Măng Long Sơn', N'XMLS', N'Khu công nghiệp Hòa Lạc, Huyện Thạch Thất, Hà Nội', N'Đinh Công Tuấn', N'0989012345', NULL, NULL, 'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH014', N'Công ty TNHH Sản Xuất Giày Da Thăng Long', N'GDTL', N'Số 456 Nguyễn Văn Cừ, Quận Long Biên, Hà Nội', N'Trịnh Thị Mai', N'0998123456', NULL, NULL,'pttha2005@gmail.com', 1)
INSERT [dbo].[KhachHang] ([maKH], [tenDoanhNghiep], [kyHieuDN], [diaChi], [nguoiDaiDien], [soDienThoaiKH], [maSoThue], [emailNguoiDaiDien], [emailDoanhNghiep], [trangThai]) VALUES (N'KH015', N'Công ty CP In và Bao Bì Bình Dương', N'BBBD', N'KCN Mỹ Phước 3, Thị xã Bến Cát, Bình Dương', N'Phan Thanh Long', N'0901234567', NULL, NULL, 'pttha2005@gmail.com', 1)

GO
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV001', N'P004', N'Trần Quang Thái', CAST(N'2005-09-17' AS Date), 1, N'62 Ấp Bắc Chan 1, Xã Tuyên Thạnh, Thị xã Kiến Tường, Long An', N'0854707222', N'thaideptrai@gmail.com', CAST(N'2025-10-12' AS Date), 1)
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV002', N'P003', N'Tôn Quốc Thái', CAST(N'2005-07-14' AS Date), 0, N'Xã Trung Hóa, Huyện Minh Hóa, Quảng Bình', N'0123456789', N'thaiton@gmail.com', CAST(N'2025-10-12' AS Date), 1)
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV003', N'P002', N'Nguyễn Hoàng Sơn', CAST(N'1988-12-26' AS Date), 1, N'Xã An Đồng, Huyện An Dương, Hải Phòng', N'5555500000', N'hoangson@gmail.com', CAST(N'2025-10-12' AS Date), 1)
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV006', N'P002', N'Nguyễn Tiến Phú', CAST(N'1990-01-30' AS Date), 0, N'Xã Vạn Ninh, Huyện Quảng Ninh, Quảng Bình', N'2225552222', N'tienphu@gmail.com', CAST(N'2025-10-14' AS Date), 1)
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV007', N'P001', N'Phan Trí Tâm', CAST(N'1998-12-28' AS Date), 0, N'Thị trấn Long Phú, Huyện Long Phú, Sóc Trăng', N'0567891230', N'Phantritam009@gmail.com', CAST(N'2025-10-15' AS Date), 1)
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV008', N'P001', N'ADMINISTRATOR', CAST(N'2005-09-17' AS Date), 0, N'Xã Tuyên Thạnh, Tây NInh', N'0099887766', N'admin@gmail.com', CAST(N'2025-10-15' AS Date), 1)
INSERT [dbo].[NhanVien] ([maNV], [maPhong], [hoTen], [ngaySinh], [gioiTinh], [diaChi], [soDienThoai], [email], [ngayTao], [trangThai]) VALUES (N'NV009', N'P005', N'Nguyễn Văn A', CAST(N'1998-12-28' AS Date), 0, N'Thị trấn Long Phú, Huyện Long Phú, Sóc Trăng', N'4567891230', N'ketqua@gmail.com', CAST(N'2025-10-15' AS Date), 1)

GO
SET IDENTITY_INSERT [dbo].[OTPVerification] ON 

INSERT [dbo].[OTPVerification] ([ID], [ContactInfo], [OTPCode], [ExpiryTime], [IsUsed], [FailedAttempts], [CreatedAt]) VALUES (2, N'0123456789', N'765618', CAST(N'2025-10-16T14:19:29.237' AS DateTime), 1, 0, CAST(N'2025-10-16T14:14:29.237' AS DateTime))
SET IDENTITY_INSERT [dbo].[OTPVerification] OFF
GO
INSERT [dbo].[PhongBan] ([maPhong], [tenPhong], [truongPhong]) VALUES (N'P001', N'Phòng kinh doanh ', NULL)
INSERT [dbo].[PhongBan] ([maPhong], [tenPhong], [truongPhong]) VALUES (N'P002', N'Phòng kế hoạch ', NULL)
INSERT [dbo].[PhongBan] ([maPhong], [tenPhong], [truongPhong]) VALUES (N'P003', N'Phòng hiện trường ', NULL)
INSERT [dbo].[PhongBan] ([maPhong], [tenPhong], [truongPhong]) VALUES (N'P004', N'Phòng thí nghiệm  ', NULL)
INSERT [dbo].[PhongBan] ([maPhong], [tenPhong], [truongPhong]) VALUES (N'P005', N'Phòng kết quả ', NULL)
GO
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'admin@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 1)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'hoangson@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'Phantritam009@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'superadmin@gmail.local', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 1)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'thaideptrai@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'thaiton@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'tienphu@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0)
INSERT [dbo].[TaiKhoan] ([tenTK], [matKhau], [vaiTro]) VALUES (N'ketqua@gmail.com', N'$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0)
GO
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0001', N'Amonica', 1000, 0, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0002', N'Sắt Fe', 5, 0, N'mg/L', N'CNTT')
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0003', N'Nhôm Al', 2, 0, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0005', N'H2S', 0, 0, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0007', N'Mangane', 0, 0, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0010', N'Co2', 1000, 100, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0011', N'Kali', 150, 100, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0012', N'Hg', 2, 0.08, N'mg/L', NULL)
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0013', N'Cu', 3, 1, N'mg/L', N'TCNH')
--INSERT [dbo].[ThongSoMoiTruong] ([maTS], [tenTS], [giaTriToiDa], [giaTriToiThieu], [donVi], [phuongPhap]) VALUES (N'TS0014', N'Ag', 1, 0.09, N'g', N'CCNA')

GO
INSERT INTO [ThongSoMoiTruong] VALUES
('TS0001', N'pH', NULL, NULL, N'-', NULL),
('TS0002', N'BOD5', 50, 0, N'mg/L', N'TCVN 6001-1:2008'),
('TS0003', N'COD', 150, 0, N'mg/L', N'SMEWW 5220C:2017'),
('TS0004', N'TSS', 10, 0, N'mg/L', N'TCVN 6179-1:1996'),
('TS0005', N'DO', 40, 0, N'mg/L', N'TCVN 6638:2000'),
('TS0006', N'Dầu mỡ', 100, 0, N'mg/L', N'TCVN 6625:2000'),
('TS0007', N'Phosphate', 6, 0, N'mg/L', N'TCVN 6202:2008'),
('TS0008', N'Cyanide', 0.5, 0, N'mg/L', N'TCVN 6637:2000'),
('TS0009', N'Độ mặn', NULL, NULL, N'mg/L', N'TCVN 6494-1:2011'),
('TS0010', N'Sulfide', 1.5, 0, N'mg/L', N'TCVN 6494-1:2011'),
('TS0011', N'Sắt Fe', 2, 0, N'mg/L', N'TCVN 6193:1996'),
('TS0012', N'Nhôm Al', 3, 0, N'mg/L', N'TCVN 6193:1996'),
('TS0013', N'Đồng Cu', 0.1, 0, N'mg/L', N'TCVN 6193:1996'),
('TS0014', N'Chì Pb', 0.01, 0, N'mg/L', N'TCVN 6193:1996'),
('TS0015', N'Thủy ngân Hg', 0.005, 0, N'mg/L', N'TCVN 6193:1996');
GO
INSERT [dbo].[TrangThai_Dot] ([maTrangThai], [tenTrangThai]) VALUES (1, N'Đã lập kế hoạch đơn hàng')
INSERT [dbo].[TrangThai_Dot] ([maTrangThai], [tenTrangThai]) VALUES (2, N'Đang thực hiện đơn hàng')
INSERT [dbo].[TrangThai_Dot] ([maTrangThai], [tenTrangThai]) VALUES (3, N'Chờ xác nhận')
INSERT [dbo].[TrangThai_Dot] ([maTrangThai], [tenTrangThai]) VALUES (4, N'Quá hạn')
INSERT [dbo].[TrangThai_Dot] ([maTrangThai], [tenTrangThai]) VALUES (5, N'Đã xác nhận')
INSERT [dbo].[TrangThai_Dot] ([maTrangThai], [tenTrangThai]) VALUES (6, N'Đã báo cáo')
GO

INSERT INTO [dbo].[TrangThai_NhanVien] (tenTrangThai)
VALUES 
(N'Đang hoạt động'),
(N'Nghỉ phép'),
(N'Nghỉ ốm'),
(N'Nghỉ thai sản'),
(N'Công tác'),
(N'Ngưng hoạt động');
go 
INSERT INTO TrangThai_KhachHang (tenTrangThai)
VALUES (N'Đang hợp tác'), (N'Ngừng hợp tác');
GO
--/****** Object:  INSERT [dbo].[tanSuatQT]    Script Date: 10/17/2025 9:01:46 PM ******/
INSERT [dbo].[tanSuatQT] ([maTSQT], [tenTSQT]) VALUES ('TSQT01', 'Không có')
INSERT [dbo].[tanSuatQT] ([maTSQT], [tenTSQT]) VALUES ('TSQT02', '6 tháng')
INSERT [dbo].[tanSuatQT] ([maTSQT], [tenTSQT]) VALUES ('TSQT03', N'Quý')
GO
INSERT [dbo].[trangThaiHD] ([maTT], [tenTT]) VALUES ('TT01', N'Đang hiệu lực') --check ngày bắt đầu với kết thúc
INSERT [dbo].[trangThaiHD] ([maTT], [tenTT]) VALUES ('TT02', N'Hết hạn') --check ngày kết thúc
INSERT [dbo].[trangThaiHD] ([maTT], [tenTT]) VALUES ('TT03', N'Hoàn thành') --check ngày kết thúc
INSERT [dbo].[trangThaiHD] ([maTT], [tenTT]) VALUES ('TT04', N'Chấm dứt trước thời hạn') --cho phòng kinh doanh tick
GO
ALTER TABLE [dbo].[DotQuanTrac] ADD  DEFAULT ((0)) FOR [trangThai]
GO
ALTER TABLE [dbo].[KhachHang] ADD  DEFAULT ((1)) FOR [trangThai]
GO
ALTER TABLE [dbo].[KhachHang]
ADD CONSTRAINT FK_KhachHang_TrangThai
FOREIGN KEY (trangThai)
REFERENCES TrangThai_KhachHang(maTrangThai);
GO
ALTER TABLE [dbo].[NhanVien] ADD  DEFAULT ((1)) FOR [trangThai]
GO
ALTER TABLE [dbo].[OTPVerification] ADD  DEFAULT ((0)) FOR [IsUsed]
GO
ALTER TABLE [dbo].[OTPVerification] ADD  DEFAULT ((0)) FOR [FailedAttempts]
GO
ALTER TABLE [dbo].[OTPVerification] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[BaoCaoKetQua]  WITH CHECK ADD  CONSTRAINT [fk_BaoCaoKetQua_DotQuanTrac] FOREIGN KEY([maDot])
REFERENCES [dbo].[DotQuanTrac] ([maDot])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[BaoCaoKetQua] CHECK CONSTRAINT [fk_BaoCaoKetQua_DotQuanTrac]
GO
ALTER TABLE [dbo].[BaoCaoKetQua]  WITH CHECK ADD  CONSTRAINT [fk_BaoCaoKetQua_NhanVien] FOREIGN KEY([nguoiXuat])
REFERENCES [dbo].[NhanVien] ([maNV])
GO
ALTER TABLE [dbo].[BaoCaoKetQua] CHECK CONSTRAINT [fk_BaoCaoKetQua_NhanVien]
GO
ALTER TABLE [dbo].[ChiTietDotQuanTrac]  WITH CHECK ADD  CONSTRAINT [fk_ChiTietDotQuanTrac_DotQuanTrac] FOREIGN KEY([maDot])
REFERENCES [dbo].[DotQuanTrac] ([maDot])
GO
ALTER TABLE [dbo].[ChiTietDotQuanTrac] CHECK CONSTRAINT [fk_ChiTietDotQuanTrac_DotQuanTrac]
GO
ALTER TABLE [dbo].[ChiTietDotQuanTrac]  WITH CHECK ADD  CONSTRAINT [fk_ChiTietDotQuanTrac_NenMau] FOREIGN KEY([maNen])
REFERENCES [dbo].[NenMau] ([maNen])
GO
ALTER TABLE [dbo].[ChiTietDotQuanTrac] CHECK CONSTRAINT [fk_ChiTietDotQuanTrac_NenMau]
GO
ALTER TABLE [dbo].[ChiTietQuanTrac]  WITH CHECK ADD  CONSTRAINT [fk_ChiTietQuanTrac_NenMau] FOREIGN KEY([maNen])
REFERENCES [dbo].[NenMau] ([maNen])
GO
ALTER TABLE [dbo].[ChiTietQuanTrac] CHECK CONSTRAINT [fk_ChiTietQuanTrac_NenMau]
GO
ALTER TABLE [dbo].[ChiTietQuanTrac]  WITH CHECK ADD  CONSTRAINT [fk_ChiTietQuanTrac_PhongBan] FOREIGN KEY([maPhong])
REFERENCES [dbo].[PhongBan] ([maPhong])
GO
ALTER TABLE [dbo].[ChiTietQuanTrac] CHECK CONSTRAINT [fk_ChiTietQuanTrac_PhongBan]
GO
ALTER TABLE [dbo].[ChiTietQuanTrac]  WITH CHECK ADD  CONSTRAINT [fk_ChiTietQuanTrac_ThongSoMoiTruong] FOREIGN KEY([maTS])
REFERENCES [dbo].[ThongSoMoiTruong] ([maTS])
GO
ALTER TABLE [dbo].[ChiTietQuanTrac] CHECK CONSTRAINT [fk_ChiTietQuanTrac_ThongSoMoiTruong]
GO
ALTER TABLE [dbo].[Dot_Nen]  WITH NOCHECK ADD  CONSTRAINT [FK_DotNen_DotQuanTrac] FOREIGN KEY([maDot])
REFERENCES [dbo].[DotQuanTrac] ([maDot])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Dot_Nen] CHECK CONSTRAINT [FK_DotNen_DotQuanTrac]
GO
ALTER TABLE [dbo].[Dot_Nen]  WITH NOCHECK ADD  CONSTRAINT [FK_DotNen_NenMau] FOREIGN KEY([maNen])
REFERENCES [dbo].[NenMau] ([maNen])
GO
ALTER TABLE [dbo].[Dot_Nen] CHECK CONSTRAINT [FK_DotNen_NenMau]
GO
ALTER TABLE [dbo].[Dot_Nen_Ts]  WITH NOCHECK ADD  CONSTRAINT [FK_DotNenTs_DotNen] FOREIGN KEY([maDN])
REFERENCES [dbo].[Dot_Nen] ([maDN])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Dot_Nen_Ts] CHECK CONSTRAINT [FK_DotNenTs_DotNen]
GO
ALTER TABLE [dbo].[Dot_Nen_Ts]  WITH NOCHECK ADD  CONSTRAINT [FK_DotNenTs_PhongBan] FOREIGN KEY([maPhong])
REFERENCES [dbo].[PhongBan] ([maPhong])
GO
ALTER TABLE [dbo].[Dot_Nen_Ts] CHECK CONSTRAINT [FK_DotNenTs_PhongBan]
GO
ALTER TABLE [dbo].[Dot_Nen_Ts]  WITH NOCHECK ADD  CONSTRAINT [FK_DotNenTs_ThongSo] FOREIGN KEY([maTS])
REFERENCES [dbo].[ThongSoMoiTruong] ([maTS])
GO
ALTER TABLE [dbo].[Dot_Nen_Ts] CHECK CONSTRAINT [FK_DotNenTs_ThongSo]
GO
ALTER TABLE [dbo].[DotQuanTrac]  WITH NOCHECK ADD  CONSTRAINT [fk_DotQuanTrac_HopDong] FOREIGN KEY([maHD])
REFERENCES [dbo].[HopDong] ([maHD])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[DotQuanTrac] CHECK CONSTRAINT [fk_DotQuanTrac_HopDong]
GO
ALTER TABLE [dbo].[DotQuanTrac]  WITH CHECK ADD  CONSTRAINT [FK_DQT_TrangThai] FOREIGN KEY([trangThai])
REFERENCES [dbo].[TrangThai_Dot] ([maTrangThai])
GO
ALTER TABLE [dbo].[DotQuanTrac] CHECK CONSTRAINT [FK_DQT_TrangThai]
GO
ALTER TABLE [dbo].[HopDong]  WITH CHECK ADD  CONSTRAINT [fk_HopDong_KhachHang] FOREIGN KEY([maKH])
REFERENCES [dbo].[KhachHang] ([maKH])
GO
ALTER TABLE [dbo].[HopDong] CHECK CONSTRAINT [fk_HopDong_KhachHang]
GO
--khoa ngoai trang thai của bảng nhân viên 
ALTER TABLE [dbo].[NhanVien]
ADD CONSTRAINT FK_NhanVien_TrangThai
FOREIGN KEY ([trangThai]) REFERENCES [dbo].[TrangThai_NhanVien]([maTrangThai]);
go 

--ALTER TABLE [dbo].[KetQua] CHECK CONSTRAINT [fk_KetQua_BaoCaoKetQua]
--GO
--ALTER TABLE [dbo].[KetQua] CHECK CONSTRAINT [FK_KetQua_DotNenTs]
--GO
--ALTER TABLE [dbo].[KetQua]  WITH CHECK ADD  CONSTRAINT [fk_KetQua_NenMau] FOREIGN KEY([maNen])
--REFERENCES [dbo].[NenMau] ([maNen])
--GO
--ALTER TABLE [dbo].[KetQua] CHECK CONSTRAINT [fk_KetQua_NenMau]
GO
ALTER TABLE [dbo].[KetQua]  WITH CHECK ADD  CONSTRAINT [fk_KetQua_NhanVien] FOREIGN KEY([nhanVienNhap])
REFERENCES [dbo].[NhanVien] ([maNV])
GO
ALTER TABLE [dbo].[KetQua] CHECK CONSTRAINT [fk_KetQua_NhanVien]
GO
--ALTER TABLE [dbo].[KetQua]  WITH CHECK ADD  CONSTRAINT [fk_KetQua_ThongSoMoiTruong] FOREIGN KEY([maTS])
--REFERENCES [dbo].[ThongSoMoiTruong] ([maTS])
--GO
--ALTER TABLE [dbo].[KetQua] CHECK CONSTRAINT [fk_KetQua_ThongSoMoiTruong]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [fk_NhanVien_PhongBan] FOREIGN KEY([maPhong])
REFERENCES [dbo].[PhongBan] ([maPhong])
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [fk_NhanVien_PhongBan]
GO
ALTER TABLE [dbo].[NhanVien]  WITH CHECK ADD  CONSTRAINT [fk_NhanVien_TaiKhoan] FOREIGN KEY([email])
REFERENCES [dbo].[TaiKhoan] ([tenTK])
GO
ALTER TABLE [dbo].[NhanVien] CHECK CONSTRAINT [fk_NhanVien_TaiKhoan]
GO
ALTER TABLE [dbo].[PhongBan]  WITH CHECK ADD  CONSTRAINT [fk_PhongBan_NhanVien] FOREIGN KEY([truongPhong])
REFERENCES [dbo].[NhanVien] ([maNV])
GO
ALTER TABLE [dbo].[PhongBan] CHECK CONSTRAINT [fk_PhongBan_NhanVien]
GO
ALTER TABLE [dbo].[Dot_Nen_Ts]  WITH NOCHECK ADD  CONSTRAINT [CHK_DotNenTs_GiaTri] CHECK  (([giaTriToiThieu] IS NULL OR [giaTriToiDa] IS NULL OR [giaTriToiThieu]<=[giaTriToiDa]))
GO
ALTER TABLE [dbo].[Dot_Nen_Ts] CHECK CONSTRAINT [CHK_DotNenTs_GiaTri]
GO
ALTER TABLE [dbo].[DotQuanTrac]  WITH NOCHECK ADD  CONSTRAINT [chk_DotQuanTrac_NgayDuKien] CHECK  (([ngayDuKien]>=[ngayBatDau]))
GO
ALTER TABLE [dbo].[DotQuanTrac] CHECK CONSTRAINT [chk_DotQuanTrac_NgayDuKien]
GO
ALTER TABLE [dbo].[DotQuanTrac]  WITH NOCHECK ADD  CONSTRAINT [chk_DotQuanTrac_NgayTraKQ] CHECK  (([ngayTraKQ] IS NULL OR [ngayTraKQ]>=[ngayBatDau]))
GO
ALTER TABLE [dbo].[DotQuanTrac] CHECK CONSTRAINT [chk_DotQuanTrac_NgayTraKQ]
GO
--/****** Object:  INSERT [dbo].[tanSuatQT]    Script Date: 10/17/2025 9:01:46 PM ******/
ALTER TABLE [dbo].[HopDong]  WITH CHECK ADD  CONSTRAINT [fk_HopDong_tanSuatQT] FOREIGN KEY([tanSuatQuanTrac])
REFERENCES [dbo].[tanSuatQT] ([maTSQT])
GO
ALTER TABLE [dbo].[HopDong]  WITH CHECK ADD  CONSTRAINT [fk_HopDong_trangThaiHD] FOREIGN KEY([trangThai])
REFERENCES [dbo].[trangThaiHD] ([maTT])
GO
/****** Object:  StoredProcedure [dbo].[layDanhSachNhanVien]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE or alter  procedure [dbo].[layDanhSachNhanVien]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        nv.maNV,
		 --nv.tenTK,
		 nv.maPhong,
        nv.hoTen,
        nv.ngaySinh,
        CASE nv.gioiTinh WHEN 0 THEN N'Nam' ELSE N'Nữ' END AS gioiTinh,
        nv.diaChi,
        nv.soDienThoai,
		nv.email,
		nv.trangThai,
        pb.tenPhong
    FROM NhanVien nv
    LEFT JOIN PhongBan pb ON nv.maPhong = pb.maPhong;
END
GO
--CREATE OR ALTER PROC LayDanhSachNhanVien_PhanTrang
  --  @PageNumber INT,
    --@PageSize INT
----AS
------BEGIN
    --SET NOCOUNT ON;

    ---SELECT 
       -- nv.maNV,
      --  nv.maPhong,
        --nv.hoTen,
       -- nv.ngaySinh,
    --    CASE nv.gioiTinh 
      --      WHEN 0 THEN N'Nam' 
        --    ELSE N'Nữ' 
    --    END AS gioiTinh,
      --  nv.diaChi,
       -- nv.soDienThoai,
       -- nv.email,
       -- nv.trangThai,
       -- pb.tenPhong,
		 --CASE WHEN EXISTS (
           -- SELECT 1 FROM PhongBan 
           -- WHERE truongPhong = nv.maNV
       -- ) THEN 1 ELSE 0 END AS isTruongPhong

  --  FROM NhanVien nv
   -- LEFT JOIN PhongBan pb ON nv.maPhong = pb.maPhong
   -- WHERE (nv.daXoa = 0) 
--	ORDER BY nv.maNV
  --  OFFSET (@PageNumber - 1) * @PageSize ROWS
   -- FETCH NEXT @PageSize ROWS ONLY;
--END
--GO

CREATE OR ALTER PROC LayDanhSachNhanVien_PhanTrang 
    @PageNumber INT, 
    @PageSize INT 
AS 
BEGIN 
    SET NOCOUNT ON; 
 
    SELECT  
        nv.maNV, 
        nv.maPhong, 
        nv.hoTen, 
        nv.ngaySinh, 
        CASE nv.gioiTinh  
            WHEN 0 THEN N'Nam'  
            ELSE N'Nữ'  
        END AS gioiTinh, 
        nv.diaChi, 
        nv.soDienThoai, 
        nv.email, 
        nv.trangThai, 
        pb.tenPhong, 
        CASE WHEN EXISTS ( 
            SELECT 1 FROM PhongBan  
            WHERE truongPhong = nv.maNV 
        ) THEN 1 ELSE 0 END AS isTruongPhong 
 
    FROM NhanVien nv 
    LEFT JOIN PhongBan pb ON nv.maPhong = pb.maPhong 
    INNER JOIN TaiKhoan tk ON nv.email = tk.tenTK  -- JOIN qua email
    WHERE (nv.daXoa = 0) 
        AND (tk.vaiTro = 0)  -- Chỉ lấy nhân viên (vaiTro = 0)
    ORDER BY nv.maNV 
    OFFSET (@PageNumber - 1) * @PageSize ROWS 
    FETCH NEXT @PageSize ROWS ONLY; 
END 
GO
/****** Object:  StoredProcedure [dbo].[LayDSKH]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROC LayDSKH
AS
BEGIN
    SELECT * FROM KhachHang
END


/****** Object:  StoredProcedure [dbo].[LayDSPhongBan]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[LayDSPhongBan]
as 
begin
	set nocount on;
	select maPhong, tenPhong
	from PhongBan;
end
GO
/****** Object:  StoredProcedure [dbo].[LayPhongBanTheoMa]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LayPhongBanTheoMa]
    @maPhong NVARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        maPhong, 
        tenPhong, 
        truongPhong
    FROM 
        PhongBan
    WHERE 
        maPhong = @maPhong;
END;
GO
/****** Object:  StoredProcedure [dbo].[LayPhongBanTheoTaiKhoan]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE or Alter  PROCEDURE [dbo].[LayPhongBanTheoTaiKhoan]
    @tenTK VARCHAR(50)
AS
BEGIN
    SELECT maPhong 
    FROM NhanVien 
    WHERE email = @tenTK;
END;
GO

/****** Object:  StoredProcedure [dbo].[layTaikhoan]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--PROCEDURES--
--proc lấy tài khoản--
CREATE PROCEDURE [dbo].[layTaikhoan] 
    @tenTK NVARCHAR(30)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tenTK, matKhau, vaiTro
    FROM TaiKhoan
    WHERE tenTK = @tenTK;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CapNhatEmailNhanVien]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_CapNhatEmailNhanVien]
    @maNV VARCHAR(15),
    @oldEmail VARCHAR(50),
    @newEmail VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF ISNULL(@oldEmail, '') = ISNULL(@newEmail, '')
            RETURN;

        IF EXISTS (SELECT 1 FROM TaiKhoan WHERE tenTK = @newEmail AND tenTK <> @oldEmail)
        BEGIN
            RAISERROR(N'Tên đăng nhập (email) mới đã tồn tại trong hệ thống!', 16, 1);
            RETURN;
        END;

        IF EXISTS (SELECT 1 FROM TaiKhoan WHERE tenTK = @oldEmail)
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE tenTK = @newEmail)
            BEGIN
                INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro)
                SELECT @newEmail, matKhau, vaiTro
                FROM TaiKhoan
                WHERE tenTK = @oldEmail;
            END

            UPDATE NhanVien
            SET email = @newEmail
            WHERE maNV = @maNV;

            IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE email = @oldEmail)
                DELETE FROM TaiKhoan WHERE tenTK = @oldEmail;
        END
        ELSE
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE tenTK = @newEmail)
            BEGIN
                INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro)
                VALUES (@newEmail, '123456', 0);
            END

            UPDATE NhanVien
            SET email = @newEmail
            WHERE maNV = @maNV;
        END
    END TRY
    BEGIN CATCH
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_CapNhatMatKhau]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Cập nhật mật khẩu theo ContactInfo (email hoặc số điện thoại)  14/10/2025 PTT
CREATE   PROCEDURE [dbo].[sp_CapNhatMatKhau]
    @ContactInfo NVARCHAR(100),
    @MatKhauMoi  VARCHAR(100),
    @Success     BIT           OUTPUT,
    @Message     NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @TenTK VARCHAR(50);

        -- Ánh xạ contact -> tài khoản đăng nhập (email được dùng làm tenTK)
        SELECT TOP 1 @TenTK = email
        FROM dbo.NhanVien
        WHERE email = @ContactInfo OR soDienThoai = @ContactInfo;

        IF @TenTK IS NULL
        BEGIN
            SET @Success = 0; SET @Message = N'Không tìm thấy người dùng phù hợp ContactInfo.';
            RETURN;
        END;

        IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE tenTK = @TenTK)
        BEGIN
            SET @Success = 0; SET @Message = N'Tài khoản không tồn tại.';
            RETURN;
        END;

        UPDATE dbo.TaiKhoan
        SET matKhau = @MatKhauMoi
        WHERE tenTK = @TenTK;

        SET @Success = 1; SET @Message = N'Đổi mật khẩu thành công.';
    END TRY
    BEGIN CATCH
        SET @Success = 0; SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DonDepOTPCu]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- STORED PROCEDURE: Dọn dẹp OTP cũ (tùy chọn)
CREATE PROCEDURE [dbo].[sp_DonDepOTPCu]
AS
BEGIN
    DELETE FROM OTPVerification 
    WHERE CreatedAt < DATEADD(HOUR, -24, GETDATE());
    
    SELECT @@ROWCOUNT AS RowsDeleted;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDanhSachNenMau]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_GetDanhSachNenMau]
    @keyword NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        maNen,
        tenNenMau,
        moTa
    FROM NenMau
    WHERE
        @keyword IS NULL
        OR @keyword = ''
        OR maNen LIKE '%' + @keyword + '%'
        OR tenNenMau LIKE '%' + @keyword + '%'
        OR moTa LIKE '%' + @keyword + '%'
    ORDER BY maNen;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDanhSachThongSo]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetDanhSachThongSo]
AS
BEGIN
    SELECT maTS,
           tenTS,
           giaTriToiDa,
           giaTriToiThieu,
           donVi,
           phuongPhap
    FROM ThongSoMoiTruong
    ORDER BY tenTS
END
GO
/****** Object:  StoredProcedure [dbo].[sp_HoanTatKeHoachQuanTrac]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HoanTatKeHoachQuanTrac]
    @maDot       VARCHAR(15),
    @maHD        VARCHAR(15),
    @noiDung     NVARCHAR(MAX) = NULL,
    @dotQuanTrac NVARCHAR(20),
    @ngayBatDau  DATE,
    @ngayDuKien  DATE,
    @ngayTraKQ   DATE = NULL,
    @trangThai   INT  = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        ----------------------------------------------------
        -- 1. VALIDATION cơ bản
        ----------------------------------------------------
        -- Đợt phải tồn tại
        IF NOT EXISTS (SELECT 1 FROM DotQuanTrac WHERE maDot = @maDot)
        BEGIN
            RAISERROR(N'Đợt quan trắc không tồn tại!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        -- Hợp đồng phải hợp lệ
        IF NOT EXISTS (SELECT 1 FROM HopDong WHERE maHD = @maHD AND trangThai = 'TT01')
        BEGIN
            RAISERROR(N'Hợp đồng không tồn tại hoặc không còn hiệu lực!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        -- Ngày tháng hợp lệ
        IF @ngayDuKien < @ngayBatDau
        BEGIN
            RAISERROR(N'Ngày dự kiến phải >= ngày bắt đầu!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        IF @ngayTraKQ IS NOT NULL AND @ngayTraKQ < @ngayBatDau
        BEGIN
            RAISERROR(N'Ngày trả kết quả phải >= ngày bắt đầu!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        -- Phải có ít nhất 1 nền mẫu
        IF NOT EXISTS (SELECT 1 FROM Dot_Nen WHERE maDot = @maDot)
        BEGIN
            RAISERROR(N'Kế hoạch phải có ít nhất một nền mẫu!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        -- Tất cả nền mẫu phải có thông tin vị trí
        IF EXISTS (
            SELECT 1 FROM Dot_Nen 
            WHERE maDot = @maDot 
              AND (tenViTri IS NULL OR LTRIM(RTRIM(tenViTri)) = '')
        )
        BEGIN
            RAISERROR(N'Tất cả nền mẫu phải có thông tin vị trí!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        -- Tất cả nền mẫu phải có ít nhất 1 thông số
        IF EXISTS (
            SELECT 1 FROM Dot_Nen dn
            WHERE dn.maDot = @maDot
              AND NOT EXISTS (
                  SELECT 1 FROM Dot_Nen_Ts dnts 
                  WHERE dnts.maDN = dn.maDN
              )
        )
        BEGIN
            RAISERROR(N'Tất cả nền mẫu phải có ít nhất một thông số!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END;
        
        ----------------------------------------------------
        -- 2. TÍNH THỨ TỰ ĐỢT (thuTuDot) NẾU CHƯA CÓ
        ----------------------------------------------------
        DECLARE @thuTuDot INT;

        SELECT @thuTuDot = thuTuDot
        FROM DotQuanTrac
        WHERE maDot = @maDot;

        IF @thuTuDot IS NULL
        BEGIN
            SELECT @thuTuDot = ISNULL(MAX(thuTuDot), 0) + 1
            FROM DotQuanTrac
            WHERE maHD = @maHD;
        END;

        ----------------------------------------------------
        -- 3. (Giữ) VALIDATION: thuTuDot không vượt số đợt dự kiến
        --    Nếu bạn cũng muốn bỏ luôn rule này thì xóa block này đi.
        ----------------------------------------------------
        DECLARE 
            @thoiHanThang   INT,
            @soThangMotDot  INT,
            @soDot_DuKien   INT,
            @tanSuat        VARCHAR(15);

        SELECT 
            @thoiHanThang = DATEDIFF(MONTH, h.ngayKy, h.ngayKetThucHD),
            @tanSuat      = h.tanSuatQuanTrac
        FROM HopDong h
        WHERE h.maHD = @maHD;

        SET @soThangMotDot = CASE @tanSuat
                                WHEN 'TSQT01' THEN NULL   -- không định kỳ
                                WHEN 'TSQT02' THEN 6      -- 6 tháng/đợt
                                WHEN 'TSQT03' THEN 3      -- theo quý
                             END;

        IF @soThangMotDot IS NULL 
           OR @soThangMotDot = 0 
           OR @thoiHanThang <= 0
        BEGIN
            SET @soDot_DuKien = 1;
        END
        ELSE
        BEGIN
            SET @soDot_DuKien = CEILING(@thoiHanThang * 1.0 / @soThangMotDot);
        END;

        IF @thuTuDot > @soDot_DuKien
        BEGIN
            RAISERROR(
                N'Số thứ tự đợt (%d) vượt quá số đợt dự kiến (%d) của hợp đồng.',
                16, 1, @thuTuDot, @soDot_DuKien
            );
            ROLLBACK TRANSACTION;
            RETURN;
        END;

        ----------------------------------------------------
        -- 4. UPDATE ĐỢT QUAN TRẮC
        ----------------------------------------------------
        UPDATE DotQuanTrac
        SET 
            maHD        = @maHD,
            thuTuDot    = @thuTuDot,
            noiDung     = @noiDung,
            dotQuanTrac = @dotQuanTrac,
            ngayBatDau  = @ngayBatDau,
            ngayDuKien  = @ngayDuKien,
            ngayTraKQ   = @ngayTraKQ,
            trangThai   = @trangThai
        WHERE maDot = @maDot;

        ----------------------------------------------------
        -- 5. CẬP NHẬT NHÃN tiepTuc_HopTac TRONG AI_TaiKy
        --    Khi KH này lại phát sinh thêm 1 đợt (hợp đồng đang hiệu lực),
        --    coi như "tiếp tục hợp tác" so với các snapshot trước đó.
        ----------------------------------------------------
        DECLARE @maKH VARCHAR(15);

        SELECT @maKH = maKH
        FROM HopDong
        WHERE maHD = @maHD;

        IF @maKH IS NOT NULL
        BEGIN
            ;WITH LastSnap AS (
                SELECT TOP (1) *
                FROM AI_TaiKy
                WHERE maKH = @maKH
                  AND (tiepTuc_HopTac IS NULL OR tiepTuc_HopTac = 0)
                ORDER BY ngaySnapshot DESC
            )
            UPDATE LastSnap
            SET tiepTuc_HopTac = 1;
        END;

        ----------------------------------------------------
        -- 6. COMMIT & TRẢ KẾT QUẢ
        ----------------------------------------------------
        COMMIT TRANSACTION;
        
        SELECT 
            dt.maDot,
            dt.maHD,
            dt.thuTuDot,
            dt.dotQuanTrac,
            dt.ngayBatDau,
            dt.ngayDuKien,
            dt.trangThai,
            COUNT(DISTINCT dn.maDN)   AS soLuongNenMau,
            COUNT(dnts.maDNTS)        AS tongSoThongSo,
            N'Hoàn tất kế hoạch quan trắc thành công!' AS thongBao
        FROM DotQuanTrac dt
        LEFT JOIN Dot_Nen     dn   ON dt.maDot = dn.maDot
        LEFT JOIN Dot_Nen_Ts  dnts ON dn.maDN  = dnts.maDN
        WHERE dt.maDot = @maDot
        GROUP BY dt.maDot, dt.maHD, dt.thuTuDot, dt.dotQuanTrac, dt.ngayBatDau, dt.ngayDuKien, dt.trangThai;
        
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END
GO



/****** Object:  StoredProcedure [dbo].[sp_HoanTatKeHoachQuanTrac_V2]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_HoanTatKeHoachQuanTrac_V2]
    @maDot VARCHAR(15),
    @maHD VARCHAR(15),
    @noiDung NVARCHAR(MAX) = NULL,
    @dotQuanTrac NVARCHAR(100),
    @ngayBatDau DATE,
    @ngayDuKien DATE,
    @ngayTraKQ DATE = NULL,
    @trangThai INT = 1
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- ✅ Validate: Đợt phải tồn tại
        IF NOT EXISTS (SELECT 1 FROM DotQuanTrac WHERE maDot = @maDot)
        BEGIN
            RAISERROR(N'Đợt quan trắc không tồn tại!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ Validate: Hợp đồng phải tồn tại và còn hiệu lực
        IF NOT EXISTS (SELECT 1 FROM HopDong WHERE maHD = @maHD AND trangThai = 1)
        BEGIN
            RAISERROR(N'Hợp đồng không tồn tại hoặc không còn hiệu lực!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ Validate: Ngày tháng
        IF @ngayDuKien < @ngayBatDau
        BEGIN
            RAISERROR(N'Ngày dự kiến phải >= ngày bắt đầu!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @ngayTraKQ IS NOT NULL AND @ngayTraKQ < @ngayBatDau
        BEGIN
            RAISERROR(N'Ngày trả kết quả phải >= ngày bắt đầu!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ Validate: Phải có ít nhất 1 nền mẫu
        IF NOT EXISTS (SELECT 1 FROM Dot_Nen WHERE maDot = @maDot)
        BEGIN
            RAISERROR(N'Kế hoạch phải có ít nhất một nền mẫu!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ Validate: Tất cả Dot_Nen phải có thông tin vị trí
        IF EXISTS (
            SELECT 1 FROM Dot_Nen 
            WHERE maDot = @maDot 
              AND (tenViTri IS NULL OR LTRIM(RTRIM(tenViTri)) = '')
        )
        BEGIN
            RAISERROR(N'Tất cả nền mẫu phải có thông tin vị trí!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ Validate: Tất cả Dot_Nen phải có ít nhất 1 thông số
        IF EXISTS (
            SELECT 1 FROM Dot_Nen dn
            WHERE dn.maDot = @maDot
              AND NOT EXISTS (
                  SELECT 1 FROM Dot_Nen_Ts dnts 
                  WHERE dnts.maDN = dn.maDN
              )
        )
        BEGIN
            RAISERROR(N'Tất cả nền mẫu phải có ít nhất một thông số!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- 🔥 CHỈ CẬP NHẬT DotQuanTrac (KHÔNG ĐỘNG VÀO Dot_Nen_Ts)
        UPDATE DotQuanTrac
        SET 
            maHD = @maHD,
            noiDung = @noiDung,
            dotQuanTrac = @dotQuanTrac,
            ngayBatDau = @ngayBatDau,
            ngayDuKien = @ngayDuKien,
            ngayTraKQ = @ngayTraKQ,
            trangThai = @trangThai
        WHERE maDot = @maDot;
        
        COMMIT TRANSACTION;
        
        -- ✅ Trả về thông tin tổng hợp
        SELECT 
            dt.maDot,
            dt.maHD,
            dt.dotQuanTrac,
            dt.ngayBatDau,
            dt.ngayDuKien,
            dt.trangThai,
            COUNT(DISTINCT dn.maDN) AS soLuongNenMau,
            COUNT(dnts.maDNTS) AS tongSoThongSo,
            N'Hoàn tất kế hoạch quan trắc thành công!' AS thongBao
        FROM DotQuanTrac dt
        LEFT JOIN Dot_Nen dn ON dt.maDot = dn.maDot
        LEFT JOIN Dot_Nen_Ts dnts ON dn.maDN = dnts.maDN
        WHERE dt.maDot = @maDot
        GROUP BY dt.maDot, dt.maHD, dt.dotQuanTrac, dt.ngayBatDau, dt.ngayDuKien, dt.trangThai;
        
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_KiemTraContactTonTai]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- STORED PROCEDURE: Kiểm tra email/SĐT tồn tại
CREATE PROCEDURE [dbo].[sp_KiemTraContactTonTai]
    @ContactInfo NVARCHAR(100),
    @TonTai BIT OUTPUT,
    @TenTK NVARCHAR(30) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT @TenTK = nv.email
    FROM NhanVien nv
    WHERE nv.email = @ContactInfo OR nv.soDienThoai = @ContactInfo;
    
    IF @TenTK IS NOT NULL
        SET @TonTai = 1
    ELSE
        SET @TonTai = 0
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayChiTietQuanTracTheoNen]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LayChiTietQuanTracTheoNen]
    @maNen VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    -- Lấy thông tin từ catalog NenMau
    SELECT 
        nm.maNen,
        nm.tenNenMau,
        nm.moTa,
        ts.maTS,
        ts.tenTS,
        ts.donVi,
        ts.giaTriToiThieu,
        ts.giaTriToiDa,
        ts.phuongPhap
    FROM NenMau nm
    CROSS JOIN ThongSoMoiTruong ts -- Hoặc INNER JOIN nếu có bảng liên kết catalog
    WHERE nm.maNen = @maNen
    ORDER BY ts.tenTS;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDanhSachDotQuanTrac]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LayDanhSachDotQuanTrac]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        d.MaDot,
        d.MaHD,
        d.NoiDung,
		 kh.tenDoanhNghiep AS TenKhachHang,
        d.DotQuanTrac,
        d.NgayBatDau,
        d.NgayDuKien,
        d.NgayTraKQ,
        d.TrangThai AS MaTrangThai,  -- Giữ mã để dùng khi cần
        t.tenTrangThai AS TrangThai  -- Hiển thị tên trạng thái
    FROM DotQuanTrac d
    LEFT JOIN TrangThai_Dot t ON d.TrangThai = t.maTrangThai
	 LEFT JOIN HopDong hd ON d.MaHD = hd.maHD  
	 LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH
    ORDER BY d.MaDot DESC;  -- Sắp xếp theo mã đợt mới nhất
END
GO

/****** Object:  StoredProcedure [dbo].[sp_LayDanhSachThongSo]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_LayDanhSachThongSo]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        maTS,
        tenTS,
        donVi,
        giaTriToiDa,
        giaTriToiThieu
    FROM ThongSoMoiTruong
    ORDER BY tenTS;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDanhSachTrangThai]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_LayDanhSachTrangThai]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT maTrangThai, tenTrangThai
    FROM TrangThai_Dot
    ORDER BY maTrangThai;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDotNenTheoMaDot]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LayDotNenTheoMaDot]
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        dn.maDN,
        dn.maDot,
        dn.maNen,
        nm.tenNenMau,
        dn.tenViTri,
        dn.toaDo,
        dn.ghiChu
    FROM Dot_Nen dn
    INNER JOIN NenMau nm ON dn.maNen = nm.maNen
    WHERE dn.maDot = @maDot
    ORDER BY dn.maDN;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayDotNenTsTheoMaDN]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- 4. PROCEDURE MỚI: sp_LayDotNenTsTheoMaDN
-- Lấy danh sách thông số của một Dot_Nen
-- =============================================
CREATE PROCEDURE [dbo].[sp_LayDotNenTsTheoMaDN]
    @maDN VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        dnts.maDNTS,
        dnts.maDN,
        dnts.maTS,
        dnts.tenTS,
        dnts.donVi,
        dnts.giaTriToiThieu,
        dnts.giaTriToiDa,
        dnts.phuongPhap,
        dnts.maPhong,
        pb.tenPhong
    FROM Dot_Nen_Ts dnts
    INNER JOIN PhongBan pb ON dnts.maPhong = pb.maPhong
    WHERE dnts.maDN = @maDN
    ORDER BY dnts.tenTS;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayTenThongSoMoiTruong]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[sp_LayTenThongSoMoiTruong]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa
    FROM ThongSoMoiTruong
    ORDER BY tenTS;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LayThongTinDotNen]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_LayThongTinDotNen]
    @maDN NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT maDN, maDot, maNen, tenViTri, toaDo, ghiChu
    FROM Dot_Nen
    WHERE maDN = @maDN;
END;
GO
/****** Object:  StoredProcedure [dbo].[sp_LuuChiTietNenMau]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_LuuChiTietNenMau]
    @maDN VARCHAR(15),
    @tenViTri NVARCHAR(200),
    @toaDo NVARCHAR(100) = NULL,
    @ghiChu NVARCHAR(MAX) = NULL,
    @danhSachThongSo NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- ✅ VALIDATION (giữ nguyên)
        IF NOT EXISTS (SELECT 1 FROM Dot_Nen WHERE maDN = @maDN)
        BEGIN
            RAISERROR(N'Bản ghi Dot_Nen không tồn tại!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @danhSachThongSo IS NULL OR LTRIM(RTRIM(@danhSachThongSo)) = ''
        BEGIN
            RAISERROR(N'Phải có ít nhất một thông số!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ UPDATE Dot_Nen (giữ nguyên)
        UPDATE Dot_Nen
        SET 
            tenViTri = @tenViTri,
            toaDo = @toaDo,
            ghiChu = @ghiChu
        WHERE maDN = @maDN;
        
        -- ✅ XÓA các thông số cũ (giữ nguyên)
        DELETE FROM Dot_Nen_Ts WHERE maDN = @maDN;
        
        -- ✅ VALIDATION maPhong (giữ nguyên)
        IF EXISTS (
            SELECT 1
            FROM OPENJSON(@danhSachThongSo) ts
            WHERE JSON_VALUE(ts.[value], '$.maPhong') IS NOT NULL
              AND JSON_VALUE(ts.[value], '$.maPhong') <> ''
              AND NOT EXISTS (
                  SELECT 1 FROM PhongBan p 
                  WHERE p.maPhong = JSON_VALUE(ts.[value], '$.maPhong')
              )
        )
        BEGIN
            DECLARE @badPhong NVARCHAR(50) = (
                SELECT TOP 1 JSON_VALUE(ts.[value], '$.maPhong')
                FROM OPENJSON(@danhSachThongSo) ts
                WHERE JSON_VALUE(ts.[value], '$.maPhong') IS NOT NULL
                  AND JSON_VALUE(ts.[value], '$.maPhong') <> ''
                  AND NOT EXISTS (
                      SELECT 1 FROM PhongBan p 
                      WHERE p.maPhong = JSON_VALUE(ts.[value], '$.maPhong')
                  )
            );
            RAISERROR(N'Mã phòng "%s" không tồn tại!', 16, 1, @badPhong);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ✅ VALIDATION maTS (giữ nguyên)
        IF EXISTS (
            SELECT 1
            FROM OPENJSON(@danhSachThongSo)
            WHERE JSON_VALUE([value], '$.maTS') IS NULL
               OR JSON_VALUE([value], '$.maTS') = ''
        )
        BEGIN
            RAISERROR(N'Phải có maTS cho mỗi thông số!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- 🔥 FIX: Tạo bảng tạm với mã tự động AN TOÀN
        CREATE TABLE #TempThongSo (
            RowNum INT IDENTITY(1,1),
            maDNTS VARCHAR(15),
            maTS VARCHAR(15),
            tenTS NVARCHAR(50),
            donVi NVARCHAR(15),
            giaTriToiThieu FLOAT,
            giaTriToiDa FLOAT,
            phuongPhap NVARCHAR(200),
            maPhong VARCHAR(15)
        );
        
        -- 🔥 Lấy số bắt đầu AN TOÀN với UPDLOCK
        DECLARE @StartNumber INT;
        
        SELECT @StartNumber = ISNULL(MAX(CAST(RIGHT(maDNTS, 4) AS INT)), 0) + 1
        FROM Dot_Nen_Ts WITH (UPDLOCK, TABLOCKX) -- 🔒 Khóa bảng
        WHERE maDNTS LIKE 'DNTS%';
        
        -- 🔥 Parse JSON và tạo mã ĐÚNG
        INSERT INTO #TempThongSo (maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong)
        SELECT
            JSON_VALUE(ts.[value], '$.maTS'),
            COALESCE(NULLIF(JSON_VALUE(ts.[value], '$.tenTS'), ''), t.tenTS),
            COALESCE(NULLIF(JSON_VALUE(ts.[value], '$.donVi'), ''), t.donVi),
            COALESCE(TRY_CAST(JSON_VALUE(ts.[value], '$.giaTriToiThieu') AS FLOAT), t.giaTriToiThieu),
            COALESCE(TRY_CAST(JSON_VALUE(ts.[value], '$.giaTriToiDa') AS FLOAT), t.giaTriToiDa),
            COALESCE(NULLIF(JSON_VALUE(ts.[value], '$.phuongPhap'), ''), t.phuongPhap),
            NULLIF(JSON_VALUE(ts.[value], '$.maPhong'), '')
        FROM OPENJSON(@danhSachThongSo) ts
        INNER JOIN ThongSoMoiTruong t 
            ON t.maTS = JSON_VALUE(ts.[value], '$.maTS');
        
        -- 🔥 Cập nhật mã DNTS theo thứ tự
        UPDATE #TempThongSo
        SET maDNTS = 'DNTS' + RIGHT('0000' + CAST(@StartNumber + RowNum - 1 AS VARCHAR), 4);
        
        -- 🔥 INSERT vào bảng chính
        INSERT INTO Dot_Nen_Ts (maDNTS, maDN, maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong)
        SELECT maDNTS, @maDN, maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong
        FROM #TempThongSo;
        
        DROP TABLE #TempThongSo;
        
        COMMIT TRANSACTION;
       
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_LuuOTP]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- STORED PROCEDURE: Lưu OTP
CREATE PROCEDURE [dbo].[sp_LuuOTP]
    @ContactInfo NVARCHAR(100),
    @OTPCode NVARCHAR(6),
    @ExpiryMinutes INT = 5
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @ExpiryTime DATETIME = DATEADD(MINUTE, @ExpiryMinutes, GETDATE());
    
    DELETE FROM OTPVerification 
    WHERE ContactInfo = @ContactInfo AND IsUsed = 0;
    
    INSERT INTO OTPVerification (ContactInfo, OTPCode, ExpiryTime, IsUsed, FailedAttempts)
    VALUES (@ContactInfo, @OTPCode, @ExpiryTime, 0, 0);
    
    SELECT 'Success' AS Result, @ExpiryTime AS ExpiryTime;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SuaNhanVien]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_SuaNhanVien]
    @maNV VARCHAR(15),
    @maPhong VARCHAR(15),
    @hoTen NVARCHAR(60),
    @ngaySinh DATE,
    @gioiTinh BIT,
    @diaChi NVARCHAR(150),
    @soDienThoai VARCHAR(20),
    @Email VARCHAR(50),
    @isTruongPhong BIT,
    @trangThai INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- 1️⃣ Kiểm tra nhân viên tồn tại
        IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE maNV = @maNV)
        BEGIN
            RAISERROR(N'Không tìm thấy nhân viên cần sửa!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 2️⃣ Kiểm tra trạng thái có hợp lệ không
        IF NOT EXISTS (SELECT 1 FROM TrangThai_NhanVien WHERE maTrangThai = @trangThai)
        BEGIN
            RAISERROR(N'Trạng thái nhân viên không hợp lệ!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 3️⃣ Lấy email cũ để cập nhật liên quan
        DECLARE @oldEmail VARCHAR(50);
        SELECT @oldEmail = email FROM NhanVien WHERE maNV = @maNV;

        -- 4️⃣ Kiểm tra họ tên hợp lệ
        IF LTRIM(RTRIM(@hoTen)) = ''
        BEGIN
            RAISERROR(N'Họ tên không được để trống!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        IF PATINDEX('%[^a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ ]%', @hoTen) > 0
        BEGIN
            RAISERROR(N'Họ tên không hợp lệ! Chỉ được chứa chữ cái và khoảng trắng.', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 5️⃣ Kiểm tra tuổi hợp lệ (16–65)
        DECLARE @tuoi INT = DATEDIFF(YEAR, @ngaySinh, GETDATE());
        IF (DATEADD(YEAR, @tuoi, @ngaySinh) > GETDATE()) SET @tuoi -= 1;

        IF @tuoi < 16 OR @tuoi > 65
        BEGIN
            RAISERROR(N'Tuổi không hợp lệ! Nhân viên phải từ 16 đến 65 tuổi.', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 6️⃣ Kiểm tra số điện thoại hợp lệ (chỉ số và đủ 10 ký tự, bắt đầu bằng 0)
      	DECLARE @soDienThoaiTrimmed VARCHAR(20) = LTRIM(RTRIM(@soDienThoai));  -- Thêm dòng này
		IF LEN(@soDienThoaiTrimmed) <> 10 OR LEFT(@soDienThoaiTrimmed, 1) <> '0' OR PATINDEX('%[^0-9]%', @soDienThoaiTrimmed) > 0
		BEGIN
			RAISERROR(N'Số điện thoại không hợp lệ! Phải bắt đầu bằng 0, đúng 10 chữ số, không chứa ký tự khác.', 16, 1);
			ROLLBACK TRAN;
			RETURN;
		END;

        IF PATINDEX('%[^0-9]%', @soDienThoai) > 0
        BEGIN
            RAISERROR(N'Số điện thoại chỉ được chứa chữ số (0–9).', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 7️⃣ Kiểm tra email hợp lệ (đúng định dạng cơ bản)
        IF @Email NOT LIKE '%_@_%._%' 
           OR @Email LIKE '%..%' 
           OR @Email LIKE '%.@%' 
           OR RIGHT(@Email, 4) NOT IN ('.com', '.net', '.org', '.edu', '.gov', '.vn')
        BEGIN
            RAISERROR(N'Email không hợp lệ! Vui lòng nhập đúng định dạng (vd: abc@gmail.com).', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 8️⃣ Kiểm tra trùng email
        IF EXISTS (SELECT 1 FROM NhanVien WHERE email = @Email AND maNV <> @maNV)
        BEGIN
            RAISERROR(N'Email này đã tồn tại cho nhân viên khác!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 9️⃣ Cập nhật email liên quan
        EXEC sp_CapNhatEmailNhanVien @maNV, @oldEmail, @Email;

        -- 🔟 Kiểm tra trưởng phòng
        IF @isTruongPhong = 1
        BEGIN
            IF EXISTS (SELECT 1 FROM PhongBan WHERE maPhong = @maPhong AND truongPhong IS NOT NULL AND truongPhong <> @maNV)
            BEGIN
                RAISERROR(N'Phòng ban này đã có trưởng phòng khác!', 16, 1);
                ROLLBACK TRAN; RETURN;
            END;
        END
        ELSE
        BEGIN
            -- Nếu bỏ vai trò trưởng phòng → xóa khỏi bảng PhongBan
            UPDATE PhongBan SET truongPhong = NULL WHERE truongPhong = @maNV;
        END;

        -- 1️⃣1️⃣ Cập nhật thông tin nhân viên
        UPDATE NhanVien
        SET maPhong     = @maPhong,
            hoTen       = @hoTen,
            ngaySinh    = @ngaySinh,
            gioiTinh    = @gioiTinh,
            diaChi      = @diaChi,
            soDienThoai = @soDienThoai,
            email       = @Email,
            trangThai   = @trangThai
        WHERE maNV = @maNV;

        -- 1️⃣2️⃣ Nếu là trưởng phòng → gán vào PhongBan
        IF @isTruongPhong = 1
        BEGIN
            UPDATE PhongBan
            SET truongPhong = @maNV
            WHERE maPhong = @maPhong;
        END;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH;
END;
GO

/****** Object:  StoredProcedure [dbo].[sp_SuaThongSoMoiTruong]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_SuaThongSoMoiTruong]
    @maTS varchar(15),
    @tenTS NVARCHAR(200),
    @giaTriToiDa FLOAT,
    @giaTriToiThieu FLOAT,
    @donVi NVARCHAR(50),
    @phuongPhap NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ThongSoMoiTruong
    SET 
        tenTS = @tenTS,
        giaTriToiDa = @giaTriToiDa,
        giaTriToiThieu = @giaTriToiThieu,
        donVi = @donVi,
        phuongPhap = @phuongPhap
    WHERE maTS = @maTS;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_TaoDotQuanTracDraft]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_TaoDotQuanTracDraft]
    @maDot VARCHAR(15) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        SET @maDot = dbo.fn_TaoMaDot();

        INSERT INTO DotQuanTrac (
            maDot,
            maHD,
            noiDung,
            dotQuanTrac,
            ngayBatDau,
            ngayDuKien,
            ngayTraKQ,
            trangThai,
			thuTuDot
        )
        VALUES (
            @maDot,
            NULL,
            NULL,
            N'Đã lập kế hoạch đơn hàng ',
            GETDATE(),
            GETDATE(),
            NULL,
            1,
			NULL
        );

        COMMIT TRANSACTION;

        SELECT @maDot AS maDot, N'Tạo nháp thành công!' AS thongBao;

    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ThemNenMau]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_ThemNenMau]
    @moTa nvarchar (max),
	@tenNenMau  nvarchar(100)

AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @maNen VARCHAR(15);

        -- Tạo mã mới: NM0001, NM0002,...
        SELECT @maNen = 'NM' +
            RIGHT('0000' + CAST(ISNULL(MAX(CAST(SUBSTRING(maNen, 3, 10) AS INT)), 0) + 1 AS VARCHAR(10)), 4)
        FROM NenMau;

        -- INSERT NenMau với mã mới
        INSERT INTO NenMau (maNen, moTa , tenNenMau)
        VALUES (@maNen, @moTa , @tenNenMau);
        COMMIT TRANSACTION;

        -- Trả về mã nền mới để UI load UserControl
        SELECT @maNen AS MaNenMoi;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ThemNenMauVaoDot]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_ThemNenMauVaoDot]
    @maDot VARCHAR(15),
    @maNen VARCHAR(15),
    @maDN VARCHAR(15) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Validate: Đợt quan trắc phải tồn tại
        IF NOT EXISTS (SELECT 1 FROM DotQuanTrac WHERE maDot = @maDot)
        BEGIN
            RAISERROR(N'Đợt quan trắc không tồn tại!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Validate: Nền mẫu phải tồn tại
        IF NOT EXISTS (SELECT 1 FROM NenMau WHERE maNen = @maNen)
        BEGIN
            RAISERROR(N'Nền mẫu không tồn tại!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- Tạo mã Dot_Nen mới
        SET @maDN = dbo.fn_TaoMaDN();
        
        -- Tạo bản ghi Dot_Nen với thông tin vị trí = NULL
        -- Các thông tin này sẽ được cập nhật ở Bước 4
        INSERT INTO Dot_Nen (
            maDN,
            maDot,
            maNen,
            tenViTri,    -- NULL
            toaDo,       -- NULL
            ghiChu       -- NULL
        )
        VALUES (
            @maDN,
            @maDot,
            @maNen,
            NULL,
            NULL,
            NULL
        );
        
        COMMIT TRANSACTION;
        
        -- Trả về thông tin để client load form chi tiết
        SELECT 
            @maDN AS maDN,
            @maDot AS maDot,
            @maNen AS maNen,
            nm.tenNenMau,
            nm.moTa,
            N'Thêm nền mẫu thành công!' AS thongBao
        FROM NenMau nm
        WHERE nm.maNen = @maNen;
        
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ThemThongSoMoiTruong]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE     PROCEDURE [dbo].[sp_ThemThongSoMoiTruong]
    @tenTS NVARCHAR(30),
    @donVi NVARCHAR(15),
	@phuongPhap NVARCHAR(100),
    @giaTriToiDa float = NULL,
    @giaTriToiThieu float = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF LTRIM(RTRIM(@tenTS)) = ''
            THROW 50000, N'Tên thông số không được để trống!', 1;

        IF LTRIM(RTRIM(@donVi)) = ''
            THROW 50000, N'Đơn vị không được để trống!', 1;
		IF LTRIM(RTRIM(@phuongPhap)) = ''
            THROW 50000, N'Phương pháp không được để trống!', 1;

        IF @giaTriToiThieu IS NOT NULL AND @giaTriToiDa IS NOT NULL AND @giaTriToiThieu > @giaTriToiDa
            THROW 50000, N'Giá trị tối thiểu không được lớn hơn giá trị tối đa!', 1;

        IF EXISTS (
            SELECT 1
            FROM ThongSoMoiTruong
            WHERE tenTS = @tenTS AND donVi = @donVi
        )
            THROW 50000, N'Thông số này đã tồn tại trong hệ thống!', 1;

        DECLARE @maTS VARCHAR(15) = dbo.fn_TaoMaThongSo();

		INSERT INTO ThongSoMoiTruong (maTS, tenTS, donVi, phuongPhap, giaTriToiDa, giaTriToiThieu)
		VALUES (@maTS, @tenTS, @donVi, @phuongPhap, @giaTriToiDa, @giaTriToiThieu);

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        DECLARE @Err NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@Err, 16, 1);
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_XacThucOTP]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- STORED PROCEDURE: Xác thực OTP
CREATE PROCEDURE [dbo].[sp_XacThucOTP]
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
    
    IF @Status = 'VALID'
    BEGIN
        UPDATE OTPVerification SET IsUsed = 1 WHERE ID = @ID;
        SET @IsValid = 1;
        SET @Message = N'Xác thực thành công';
    END
    ELSE IF @Status = 'INVALID'
    BEGIN
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
/****** Object:  StoredProcedure [dbo].[sp_XoaDotQuanTrac]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_XoaDotQuanTrac]
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Nếu có bảng phụ liên quan thì phải xóa trước
        -- Ví dụ:
        -- DELETE FROM Dot_Nen_Ts WHERE maDNTS IN (...) hoặc theo maDot
        -- DELETE FROM Dot_Nen WHERE maDot = @maDot

        DELETE FROM DotQuanTrac WHERE maDot = @maDot;

        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Xóa đợt quan trắc thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0
            ROLLBACK TRANSACTION;

        SELECT 0 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_XoaNenMau]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_XoaNenMau]
    @maNen VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    -- Kiểm tra tồn tại
    IF NOT EXISTS (SELECT 1 FROM NenMau WHERE maNen = @maNen)
    BEGIN
        RAISERROR(N'Mã nền mẫu không tồn tại!', 16, 1);
        RETURN;
    END

    -- Kiểm tra đã được dùng trong Dot_Nen chưa
    IF EXISTS (SELECT 1 FROM Dot_Nen WHERE maNen = @maNen)
    BEGIN
        RAISERROR(N'Nền mẫu đã được sử dụng trong đợt quan trắc, không thể xóa!', 16, 1);
        RETURN;
    END

    -- Xóa
    DELETE FROM NenMau WHERE maNen = @maNen;

    SELECT @maNen AS maNen, N'Xóa nền mẫu thành công!' AS thongBao;
END
GO
/****** Object:  StoredProcedure [dbo].[sp_XoaNhanVien]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--2/11/2025 proc xóa nhân viên :
CREATE PROCEDURE [dbo].[sp_XoaNhanVien] 
    @maNV VARCHAR(15) 
AS 
BEGIN 
    SET NOCOUNT ON; 
 
    BEGIN TRY 
        BEGIN TRAN; 
 
        -- ✅ Kiểm tra nhân viên tồn tại
        IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE maNV = @maNV) 
        BEGIN 
            RAISERROR(N'Không tìm thấy nhân viên cần xóa!', 16, 1); 
            ROLLBACK TRAN; 
            RETURN; 
        END; 

        -- ✅ Kiểm tra trạng thái phải = 6
        DECLARE @trangThai INT;
        SELECT @trangThai = trangThai FROM NhanVien WHERE maNV = @maNV;

        IF @trangThai != 6
        BEGIN
            RAISERROR(N'Chỉ được xóa nhân viên có trạng thái "Ngưng hoạt động"!', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END;

        -- ✅ XÓA MỀM: Đánh dấu daXoa = 1
        UPDATE NhanVien 
        SET daXoa = 1 
        WHERE maNV = @maNV;
 
        -- ✅ Cập nhật trưởng phòng thành NULL nếu là trưởng phòng
        UPDATE PhongBan 
        SET truongPhong = NULL 
        WHERE truongPhong = @maNV; 
 
        COMMIT TRAN; 
    END TRY 
    BEGIN CATCH 
        ROLLBACK TRAN; 
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE(); 
        RAISERROR(@ErrMsg, 16, 1); 
    END CATCH; 
END; 
GO
/****** Object:  StoredProcedure [dbo].[sp_XoaThongSoMoiTruong]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[sp_XoaThongSoMoiTruong]
    @maTS VARCHAR(15),
    @ketQua NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;
        
        -- Kiểm tra thông số tồn tại
        IF NOT EXISTS (SELECT 1 FROM ThongSoMoiTruong WHERE maTS = @maTS)
        BEGIN
            SET @ketQua = N'Không tìm thấy thông số cần xóa!';
            ROLLBACK TRAN;
            RETURN;
        END
        
        -- Kiểm tra có đang được sử dụng không
        IF EXISTS (SELECT 1 FROM ChiTietQuanTrac WHERE maTS = @maTS)
        BEGIN
            SET @ketQua = N'Không thể xóa! Thông số này đang được sử dụng trong kế hoạch quan trắc.';
            ROLLBACK TRAN;
            RETURN;
        END
      
        
        -- Xóa thông số
        DELETE FROM ThongSoMoiTruong WHERE maTS = @maTS;
        
        SET @ketQua = N'Xóa thông số thành công!';
        
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        SET @ketQua = N'Lỗi: ' + ERROR_MESSAGE();
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SuaKhachHang]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SuaKhachHang]
    @maKH VARCHAR(15),
    @tenDoanhNghiep NVARCHAR(100),
    @kyHieuDN NVARCHAR(20) = NULL,
    @diaChi NVARCHAR(150),
    @nguoiDaiDien NVARCHAR(50),
    @soDienThoaiKH VARCHAR(10),
    @maSoThue VARCHAR(20) = NULL,
    @emailNguoiDaiDien VARCHAR(100) = NULL,
    @emailDoanhNghiep VARCHAR(100) = NULL,
    @trangThai INT = 1
AS
BEGIN
    SET NOCOUNT ON;

    ------------------------------------------------------------
    -- Kiểm tra mã khách hàng tồn tại
    ------------------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE maKH = @maKH)
    BEGIN
        RAISERROR(N'Mã khách hàng không tồn tại.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra trạng thái hợp lệ
    ------------------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM TrangThai_KhachHang WHERE maTrangThai = @trangThai)
    BEGIN
        RAISERROR(N'Mã trạng thái khách hàng không tồn tại!', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra số điện thoại
    ------------------------------------------------------------
    IF @soDienThoaiKH NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
    BEGIN
        RAISERROR(N'Số điện thoại không hợp lệ! Phải gồm đúng 10 chữ số.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra tên người đại diện
    ------------------------------------------------------------
    IF @nguoiDaiDien LIKE '%[^A-Za-zÀ-ỹĂĐĨŨƠăđĩũơƯĂẰẲẴẶẮÂẦẨẪẬẤĐÊỀỂỄỆẾÔỒỔỖỘỐƠỜỞỠỢỚƯỪỬỮỰỨàằẳẵặắâầẩẫậấđêềểễệếôồổỗộốơờởỡợớưừửữựứ ]%'
    BEGIN
        RAISERROR(N'Tên người đại diện chỉ được chứa chữ cái và khoảng trắng.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra mã số thuế (chỉ số và dấu "-")
    ------------------------------------------------------------
    IF @maSoThue IS NOT NULL AND @maSoThue NOT LIKE '%[0-9-]%'
    BEGIN
        RAISERROR(N'Mã số thuế chỉ được chứa số và dấu gạch ngang.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra email hợp lệ
    ------------------------------------------------------------
    IF @emailNguoiDaiDien IS NOT NULL AND @emailNguoiDaiDien NOT LIKE '_%@_%._%'
    BEGIN
        RAISERROR(N'Email người đại diện không hợp lệ.', 16, 1);
        RETURN;
    END;

    IF @emailDoanhNghiep IS NOT NULL AND @emailDoanhNghiep NOT LIKE '_%@_%._%'
    BEGIN
        RAISERROR(N'Email doanh nghiệp không hợp lệ.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Cập nhật thông tin khách hàng
    ------------------------------------------------------------
    UPDATE dbo.KhachHang
    SET 
        tenDoanhNghiep    = @tenDoanhNghiep,
        kyHieuDN          = @kyHieuDN,
        diaChi            = @diaChi,
        nguoiDaiDien      = @nguoiDaiDien,
        soDienThoaiKH     = @soDienThoaiKH,
        maSoThue          = @maSoThue,
        emailNguoiDaiDien = @emailNguoiDaiDien,
        emailDoanhNghiep  = @emailDoanhNghiep,
        trangThai         = @trangThai
    WHERE maKH = @maKH;

    PRINT N'Cập nhật thông tin khách hàng thành công.';
END;
GO

/****** Object:  StoredProcedure [dbo].[TaiLenPTNvaPHT]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[TaiLenPTNvaPHT]
AS
BEGIN
    SELECT maPhong, tenPhong
    FROM PhongBan
    WHERE tenPhong IN (N'Phòng thí nghiệm', N'Phòng hiện trường')
END
GO
/****** Object:  StoredProcedure [dbo].[ThemKhachHang]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--begin proc thêm khách hàng 
CREATE OR ALTER PROCEDURE [dbo].[ThemKhachHang]
    @tenDoanhNghiep NVARCHAR(100),
    @kyHieuDN NVARCHAR(20) = NULL,
    @diaChi NVARCHAR(150),
    @nguoiDaiDien NVARCHAR(50),
    @soDienThoaiKH VARCHAR(10),
    @maSoThue VARCHAR(20) = NULL,
    @emailNguoiDaiDien VARCHAR(100) = NULL,
    @emailDoanhNghiep VARCHAR(100) = NULL,
    @trangThai INT = 1 -- 1 = Đang hợp tác (FK)
AS
BEGIN
    SET NOCOUNT ON;

    ------------------------------------------------------------
    -- Kiểm tra trạng thái hợp lệ
    ------------------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM TrangThai_KhachHang WHERE maTrangThai = @trangThai)
    BEGIN
        RAISERROR(N'Mã trạng thái khách hàng không tồn tại!', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra số điện thoại
    ------------------------------------------------------------
    IF @soDienThoaiKH NOT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'
    BEGIN
        RAISERROR(N'Số điện thoại không hợp lệ! Phải gồm đúng 10 chữ số.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra tên người đại diện
    ------------------------------------------------------------
    IF @nguoiDaiDien LIKE '%[^A-Za-zÀ-ỹĂĐĨŨƠăđĩũơƯĂẰẲẴẶẮÂẦẨẪẬẤĐÊỀỂỄỆẾÔỒỔỖỘỐƠỜỞỠỢỚƯỪỬỮỰỨàằẳẵặắâầẩẫậấđêềểễệếôồổỗộốơờởỡợớưừửữựứ ]%'
    BEGIN
        RAISERROR(N'Tên người đại diện chỉ được chứa chữ cái và khoảng trắng.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra mã số thuế (chỉ số và dấu "-")
    ------------------------------------------------------------
    IF @maSoThue IS NOT NULL AND @maSoThue NOT LIKE '%[0-9-]%'
    BEGIN
        RAISERROR(N'Mã số thuế chỉ được chứa số và dấu gạch ngang.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Kiểm tra email hợp lệ
    ------------------------------------------------------------
    IF @emailNguoiDaiDien IS NOT NULL AND @emailNguoiDaiDien NOT LIKE '_%@_%._%'
    BEGIN
        RAISERROR(N'Email người đại diện không hợp lệ.', 16, 1);
        RETURN;
    END;

    IF @emailDoanhNghiep IS NOT NULL AND @emailDoanhNghiep NOT LIKE '_%@_%._%'
    BEGIN
        RAISERROR(N'Email doanh nghiệp không hợp lệ.', 16, 1);
        RETURN;
    END;

    ------------------------------------------------------------
    -- Sinh mã khách hàng tự động
    ------------------------------------------------------------
    DECLARE @newMaKH VARCHAR(15);
    DECLARE @maxNum INT;

    SELECT @maxNum = MAX(CAST(SUBSTRING(maKH, 3, LEN(maKH)) AS INT))
    FROM dbo.KhachHang
    WHERE maKH LIKE 'KH%';

    IF @maxNum IS NULL SET @maxNum = 0;
    SET @newMaKH = 'KH' + RIGHT('000' + CAST(@maxNum + 1 AS VARCHAR(3)), 3);

    ------------------------------------------------------------
    -- Thêm khách hàng
    ------------------------------------------------------------
    INSERT INTO dbo.KhachHang
    (
        maKH,
        tenDoanhNghiep,
        kyHieuDN,
        diaChi,
        nguoiDaiDien,
        soDienThoaiKH,
        maSoThue,
        emailNguoiDaiDien,
        emailDoanhNghiep,
        trangThai
    )
    VALUES
    (
        @newMaKH,
        @tenDoanhNghiep,
        @kyHieuDN,
        @diaChi,
        @nguoiDaiDien,
        @soDienThoaiKH,
        @maSoThue,
        @emailNguoiDaiDien,
        @emailDoanhNghiep,
        @trangThai
    );

    PRINT N'Thêm khách hàng thành công với mã: ' + @newMaKH;
END;
GO
--end Proc thêm khách hàng
/****** Object:  StoredProcedure [dbo].[ThemNhanVien]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[ThemNhanVien]
    @maPhong VARCHAR(15),
    @hoTen NVARCHAR(60),
    @ngaySinh DATE,
    @gioiTinh BIT,
    @diaChi NVARCHAR(150),
    @soDienThoai VARCHAR(20),
    @Email VARCHAR(50), 
    @isTruongPhong BIT,
    @trangThai INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @maNV VARCHAR(15);
    DECLARE @so INT;
    DECLARE @tuoi INT;

    BEGIN TRY
        BEGIN TRAN;

        SET @tuoi = DATEDIFF(YEAR, @ngaySinh, GETDATE());
        IF (DATEADD(YEAR, @tuoi, @ngaySinh) > GETDATE())
            SET @tuoi = @tuoi - 1;

        IF @tuoi < 16 OR @tuoi > 65
        BEGIN
            RAISERROR(N'Tuổi không hợp lệ! Nhân viên phải từ 16 đến 65 tuổi.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END;

        -- ✅ Kiểm tra họ tên hợp lệ
        IF PATINDEX('%[^a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ ]%', @hoTen) > 0
        BEGIN
            RAISERROR(N'Họ tên không hợp lệ! Chỉ được chứa chữ cái và khoảng trắng.', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END;

        -- ✅ Kiểm tra số điện thoại: phải đúng 10 ký tự, toàn số, không chứa chữ
       -- Phần kiểm tra sdt cũ (thay thế bằng đoạn này)
		DECLARE @soDienThoaiTrimmed VARCHAR(20) = LTRIM(RTRIM(@soDienThoai));  -- Thêm dòng này
		IF LEN(@soDienThoaiTrimmed) <> 10 OR LEFT(@soDienThoaiTrimmed, 1) <> '0' OR PATINDEX('%[^0-9]%', @soDienThoaiTrimmed) > 0
		BEGIN
			RAISERROR(N'Số điện thoại không hợp lệ! Phải bắt đầu bằng 0, đúng 10 chữ số, không chứa ký tự khác.', 16, 1);
			ROLLBACK TRAN;
			RETURN;
		END;
		-- Sau đó dùng @soDienThoaiTrimmed trong INSERT nếu cần, nhưng vì VARCHAR(10) fixed, có thể dùng trực tiếp sau trim.

        -- ✅ Kiểm tra email hợp lệ
        IF @Email NOT LIKE '%_@_%._%' 
            OR @Email LIKE '%..%' 
            OR @Email LIKE '%.@%' 
            OR RIGHT(@Email, 4) NOT IN ('.com', '.net', '.org', '.edu', '.gov', '.vn')
        BEGIN
            RAISERROR(N'Email không hợp lệ! Vui lòng nhập đúng định dạng (vd: abc@gmail.com).', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END;

        -- ✅ Email không trùng
        IF EXISTS (SELECT 1 FROM NhanVien WHERE email = @Email)
        BEGIN
            RAISERROR(N'Email này đã tồn tại cho nhân viên khác!', 16, 1);
            ROLLBACK TRAN;
            RETURN;
        END;

        -- ✅ Kiểm tra trưởng phòng
        IF @isTruongPhong = 1
        BEGIN
            IF EXISTS (SELECT 1 FROM PhongBan WHERE maPhong = @maPhong AND truongPhong IS NOT NULL)
            BEGIN
                RAISERROR(N'Phòng ban này đã có trưởng phòng!', 16, 1);
                ROLLBACK TRAN;
                RETURN;
            END;
        END;

        -- ✅ Sinh mã NV tự động
        SELECT @so = CAST(SUBSTRING(maNV, 3, LEN(maNV)) AS INT)
        FROM NhanVien
        WHERE maNV = (SELECT MAX(maNV) FROM NhanVien);

        IF @so IS NULL SET @so = 0;
        SET @so = @so + 1;
        SET @maNV = 'NV' + RIGHT('000' + CAST(@so AS VARCHAR(3)), 3);

        -- ✅ Tạo tài khoản nếu chưa có
        IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE tenTK = @Email)
        BEGIN
            INSERT INTO TaiKhoan(tenTK, matKhau, vaiTro)
            VALUES(@Email, '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0);
        END;

        -- ✅ Thêm nhân viên
        INSERT INTO NhanVien (maNV, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email, ngayTao, trangThai)
        VALUES (@maNV, @maPhong, @hoTen, @ngaySinh, @gioiTinh, @diaChi, @soDienThoaiTrimmed, @Email, GETDATE(), @trangThai);

        -- ✅ Cập nhật trưởng phòng nếu có
        IF @isTruongPhong = 1
        BEGIN
            UPDATE PhongBan
            SET truongPhong = @maNV
            WHERE maPhong = @maPhong;
        END;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH;
END;
GO
/****** Object:  StoredProcedure [dbo].[XoaKhachHang]    Script Date: 10/30/2025 12:14:41 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[XoaKhachHang]
    @maKH VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    --------------------------------------------------
    -- Kiểm tra mã khách hàng có tồn tại không
    --------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE maKH = @maKH)
    BEGIN
        RAISERROR(N'Mã khách hàng không tồn tại.', 16, 1);
        RETURN;
    END;

    --------------------------------------------------
    -- (Tuỳ chọn) Kiểm tra ràng buộc dữ liệu liên quan
    -- Ví dụ: nếu khách hàng đang có hóa đơn, hợp đồng,...
    -- thì không cho xóa để tránh lỗi khóa ngoại (FK)
    --------------------------------------------------
    -- IF EXISTS (SELECT 1 FROM dbo.HoaDon WHERE maKH = @maKH)
    -- BEGIN
    --     RAISERROR(N'Không thể xóa khách hàng vì đang tồn tại dữ liệu liên quan.', 16, 1);
    --     RETURN;
    -- END;

    --------------------------------------------------
    -- Thực hiện xóa
    --------------------------------------------------
    DELETE FROM dbo.KhachHang
    WHERE maKH = @maKH;

    PRINT N'Đã xóa khách hàng thành công.';
END;
GO

/****** Object:  StoredProcedure [dbo].[layDanhSachHopDong]    Script Date: 10/18/2025 11:55:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[layDanhSachHopDong]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        hd.maHD,
		hd.maKH,
        hd.ngayKy,
        hd.ngayKetThucHD,
		tt.tenTT as trangThai,
		tsqt.tenTSQT as tanSuatQuanTrac,
		hd.soHD
    FROM HopDong as hd LEFT JOIN tanSuatQT tsqt ON hd.tanSuatQuanTrac = tsqt.maTSQT LEFT JOIN dbo.trangThaiHD AS tt  ON tt.maTT = hd.trangThai;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[layDanhSachHopDongVaTenDN]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        hd.maHD,
        hd.maHD + ' - ' + kh.tenDoanhNghiep AS maHDVaKH
    FROM HopDong hd
    INNER JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE hd.trangThai = 'TT01'
    ORDER BY hd.ngayKy DESC;
END
GO

/****** Object:  StoredProcedure [dbo].[ThemHopDong]    Script Date: 10/18/2025 9:01:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ThemHopDong]
    @maKH              VARCHAR(15),
    @soHD              VARCHAR(15),
    @tanSuatQuanTrac   VARCHAR(15),
    @ngayKy            DATE,
    @ngayKetThucHD     DATE,
    @trangThai         VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @maHD   VARCHAR(15);
    DECLARE @so     INT;
    DECLARE @dayKy  VARCHAR(10) = CONVERT(VARCHAR(10), @ngayKy, 103);
    DECLARE @today  DATE = CAST(GETDATE() AS DATE);
    DECLARE @tenDN  NVARCHAR(100);
    DECLARE @thoiHan INT = DATEDIFF(MONTH, @ngayKy, @ngayKetThucHD);

    BEGIN TRY
        
        -----------------------------------------------------
        -- BASIC VALIDATION
        -----------------------------------------------------
        IF (@ngayKy >= @ngayKetThucHD)
            RAISERROR(N'Ngày kết thúc phải sau ngày ký.', 16, 1);

        IF NOT EXISTS (SELECT 1 FROM dbo.KhachHang WHERE maKH = @maKH)
            RAISERROR(N'Mã khách hàng không tồn tại.', 16, 1);

        IF NOT EXISTS (SELECT 1 FROM dbo.tanSuatQT WHERE maTSQT = @tanSuatQuanTrac)
            RAISERROR(N'Mã tần suất quan trắc không tồn tại.', 16, 1);

        IF EXISTS (SELECT 1 FROM dbo.HopDong WHERE maKH = @maKH AND ngayKy = @ngayKy)
            RAISERROR(N'Đã có hợp đồng của khách hàng này vào ngày %s.', 16, 1, @dayKy);

        IF EXISTS (SELECT 1 FROM dbo.HopDong WHERE soHD = @soHD AND maKH = @maKH)
            RAISERROR(N'Số hợp đồng đã tồn tại cho khách hàng này.', 16, 1);

        -- Trạng thái HĐ
        IF (@trangThai = 'TT01' AND NOT (@ngayKy <= @today AND @today <= @ngayKetThucHD))
            RAISERROR(N'Đang hiệu lực yêu cầu ngày hiện tại nằm trong khoảng ngày ký đến ngày kết thúc.', 16, 1);

        IF (@trangThai = 'TT02' AND NOT (@today > @ngayKetThucHD))
            RAISERROR(N'Hết hạn yêu cầu ngày hiện tại đã sau ngày kết thúc.', 16, 1);

        IF (@trangThai = 'TT03')
            RAISERROR(N'Hoàn thành yêu cầu ngày hiện tại đã sau ngày kết thúc..', 16, 1);

        IF (@trangThai = 'TT04' AND NOT(@ngayKy <= @today AND @today <= @ngayKetThucHD))
            RAISERROR(N'Chấm dứt trước thời hạn yêu cầu hợp đồng đang trong thời gian hiệu lực.', 16, 1);


        -----------------------------------------------------
        -- 🔥 VALIDATION TẦN SUẤT QUAN TRẮC (bạn yêu cầu)
        -----------------------------------------------------

        -- TSQT01 → Không quan trắc → không yêu cầu gì
        IF (@tanSuatQuanTrac = 'TSQT02')   -- 6 tháng
        BEGIN
            IF @thoiHan < 12       -- cần >=12 tháng để có ít nhất 2 đợt
                RAISERROR(N'Tần suất 6 tháng yêu cầu hợp đồng phải từ 12 tháng trở lên (ít nhất 2 đợt).', 16, 1);
        END

        IF (@tanSuatQuanTrac = 'TSQT03')   -- quý
        BEGIN
            IF @thoiHan < 6        -- cần >=6 tháng để có ít nhất 2 đợt
                RAISERROR(N'Tần suất theo quý yêu cầu hợp đồng phải từ 6 tháng trở lên (ít nhất 2 đợt).', 16, 1);
        END


        -----------------------------------------------------
        -- CREATE CONTRACT
        -----------------------------------------------------
        SELECT @tenDN = tenDoanhNghiep FROM dbo.KhachHang WHERE maKH = @maKH;

        BEGIN TRAN;

        SELECT @so = MAX(CAST(SUBSTRING(maHD, 3, 10) AS INT))
        FROM dbo.HopDong WITH (UPDLOCK, HOLDLOCK);

        IF @so IS NULL SET @so = 0;
        SET @so = @so + 1;

        SET @maHD = 'HD' + RIGHT('000' + CAST(@so AS VARCHAR(10)), 3);

        INSERT INTO dbo.HopDong (maHD, maKH, soHD, tanSuatQuanTrac, ngayKy, ngayKetThucHD, trangThai)
        VALUES (@maHD, @maKH, @soHD, @tanSuatQuanTrac, @ngayKy, @ngayKetThucHD, @trangThai);

        COMMIT TRAN;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum INT = ERROR_NUMBER();
        DECLARE @ErrState INT = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO

 --Sửa mô tả nền mẫu 
CREATE PROCEDURE [dbo].[sp_SuaMoTaNenMau]
    @maNen VARCHAR(15),
    @moTa NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE NenMau
        SET moTa = @moTa
        WHERE maNen = @maNen;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO
--sửa chi tiết nền mẫu 
CREATE OR ALTER PROCEDURE [dbo].[sp_SuaChiTietNenMau]
    @maDN VARCHAR(15),
    @tenViTri NVARCHAR(200),
    @toaDo NVARCHAR(100) = NULL,
    @ghiChu NVARCHAR(MAX) = NULL,
    @danhSachThongSo NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- ========================================
        -- 1️⃣ VALIDATION
        -- ========================================
        IF NOT EXISTS (SELECT 1 FROM Dot_Nen WHERE maDN = @maDN)
        BEGIN
            RAISERROR(N'Bản ghi Dot_Nen không tồn tại!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @danhSachThongSo IS NULL OR LTRIM(RTRIM(@danhSachThongSo)) = ''
        BEGIN
            RAISERROR(N'Phải có ít nhất một thông số!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- VALIDATION maPhong
        IF EXISTS (
            SELECT 1
            FROM OPENJSON(@danhSachThongSo) ts
            WHERE JSON_VALUE(ts.[value], '$.maPhong') IS NOT NULL
              AND JSON_VALUE(ts.[value], '$.maPhong') <> ''
              AND NOT EXISTS (
                  SELECT 1 FROM PhongBan p 
                  WHERE p.maPhong = JSON_VALUE(ts.[value], '$.maPhong')
              )
        )
        BEGIN
            DECLARE @badPhong NVARCHAR(50) = (
                SELECT TOP 1 JSON_VALUE(ts.[value], '$.maPhong')
                FROM OPENJSON(@danhSachThongSo) ts
                WHERE JSON_VALUE(ts.[value], '$.maPhong') IS NOT NULL
                  AND JSON_VALUE(ts.[value], '$.maPhong') <> ''
                  AND NOT EXISTS (
                      SELECT 1 FROM PhongBan p 
                      WHERE p.maPhong = JSON_VALUE(ts.[value], '$.maPhong')
                  )
            );
            RAISERROR(N'Mã phòng "%s" không tồn tại!', 16, 1, @badPhong);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- VALIDATION maTS
        IF EXISTS (
            SELECT 1
            FROM OPENJSON(@danhSachThongSo)
            WHERE JSON_VALUE([value], '$.maTS') IS NULL
               OR JSON_VALUE([value], '$.maTS') = ''
        )
        BEGIN
            RAISERROR(N'Phải có maTS cho mỗi thông số!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- VALIDATION: Kiểm tra trùng maTS
        IF EXISTS (
            SELECT maTS
            FROM OPENJSON(@danhSachThongSo)
            CROSS APPLY (
                SELECT JSON_VALUE([value], '$.maTS') AS maTS
            ) x
            GROUP BY maTS
            HAVING COUNT(*) > 1
        )
        BEGIN
            DECLARE @duplicateTS VARCHAR(15) = (
                SELECT TOP 1 maTS
                FROM OPENJSON(@danhSachThongSo)
                CROSS APPLY (
                    SELECT JSON_VALUE([value], '$.maTS') AS maTS
                ) x
                GROUP BY maTS
                HAVING COUNT(*) > 1
            );
            RAISERROR(N'Thông số "%s" bị trùng lặp trong danh sách!', 16, 1, @duplicateTS);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- VALIDATION: Giá trị min/max
        IF EXISTS (
            SELECT 1
            FROM OPENJSON(@danhSachThongSo)
            WHERE TRY_CAST(JSON_VALUE([value], '$.giaTriToiThieu') AS FLOAT) > 
                  TRY_CAST(JSON_VALUE([value], '$.giaTriToiDa') AS FLOAT)
        )
        BEGIN
            RAISERROR(N'Giá trị tối thiểu không được lớn hơn giá trị tối đa!', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        -- ========================================
        -- 2️⃣ UPDATE Dot_Nen
        -- ========================================
        UPDATE Dot_Nen
        SET 
            tenViTri = @tenViTri,
            toaDo = @toaDo,
            ghiChu = @ghiChu
        WHERE maDN = @maDN;
        
        -- ========================================
        -- 3️⃣ XÓA TẤT CẢ THÔNG SỐ CŨ
        -- ========================================
        DELETE FROM Dot_Nen_Ts WHERE maDN = @maDN;
        
        -- ========================================
        -- 4️⃣ TẠO BẢNG TẠM VỚI MÃ TỰ ĐỘNG
        -- ========================================
        CREATE TABLE #TempThongSo (
            RowNum INT IDENTITY(1,1),
            maDNTS VARCHAR(15),
            maDNTS_Cu VARCHAR(15), -- Lưu mã cũ (nếu có)
            maTS VARCHAR(15),
            tenTS NVARCHAR(50),
            donVi NVARCHAR(15),
            giaTriToiThieu FLOAT,
            giaTriToiDa FLOAT,
            phuongPhap NVARCHAR(200),
            maPhong VARCHAR(15)
        );
        
        -- Lấy số bắt đầu AN TOÀN
        DECLARE @StartNumber INT;
        
        SELECT @StartNumber = ISNULL(MAX(CAST(RIGHT(maDNTS, 4) AS INT)), 0) + 1
        FROM Dot_Nen_Ts WITH (UPDLOCK, TABLOCKX)
        WHERE maDNTS LIKE 'DNTS%';
        
        -- ========================================
        -- 5️⃣ PARSE JSON
        -- ========================================
        INSERT INTO #TempThongSo (maDNTS_Cu, maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong)
        SELECT
            JSON_VALUE(ts.[value], '$.maDNTS'), -- Lưu mã cũ (có thể NULL)
            JSON_VALUE(ts.[value], '$.maTS'),
            COALESCE(NULLIF(JSON_VALUE(ts.[value], '$.tenTS'), ''), t.tenTS),
            COALESCE(NULLIF(JSON_VALUE(ts.[value], '$.donVi'), ''), t.donVi),
            COALESCE(TRY_CAST(JSON_VALUE(ts.[value], '$.giaTriToiThieu') AS FLOAT), t.giaTriToiThieu),
            COALESCE(TRY_CAST(JSON_VALUE(ts.[value], '$.giaTriToiDa') AS FLOAT), t.giaTriToiDa),
            COALESCE(NULLIF(JSON_VALUE(ts.[value], '$.phuongPhap'), ''), t.phuongPhap),
            NULLIF(JSON_VALUE(ts.[value], '$.maPhong'), '')
        FROM OPENJSON(@danhSachThongSo) ts
        INNER JOIN ThongSoMoiTruong t 
            ON t.maTS = JSON_VALUE(ts.[value], '$.maTS');
        
        -- ========================================
        -- 6️⃣ GÁN MÃ DNTS
        -- ========================================
        -- Ưu tiên giữ mã cũ, nếu không có thì tạo mới
        UPDATE #TempThongSo
        SET maDNTS = CASE
            WHEN maDNTS_Cu IS NOT NULL AND maDNTS_Cu <> '' 
            THEN maDNTS_Cu  -- GIỮ MÃ CŨ
            ELSE 'DNTS' + RIGHT('0000' + CAST(@StartNumber + RowNum - 1 AS VARCHAR), 4) -- TẠO MÃ MỚI
        END;
        
        -- ========================================
        -- 7️⃣ INSERT LẠI VÀO BẢNG CHÍNH
        -- ========================================
        INSERT INTO Dot_Nen_Ts (maDNTS, maDN, maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong)
        SELECT maDNTS, @maDN, maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong
        FROM #TempThongSo;
        
        DROP TABLE #TempThongSo;
        
        COMMIT TRANSACTION;
        
    END TRY
    BEGIN CATCH
        IF XACT_STATE() <> 0 ROLLBACK TRANSACTION;
        DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMsg, 16, 1);
    END CATCH
END
GO

--Thêm bảng trạng thái của khách hàng :
--Proc tải trạng thái khách hàng lên combobox 
CREATE PROCEDURE dbo.sp_LayTrangThaiKhachHang
AS
BEGIN
    SET NOCOUNT ON;
    SELECT maTrangThai, tenTrangThai
    FROM dbo.TrangThai_KhachHang
    ORDER BY maTrangThai;
END;
GO

--Lấy danh sách khách hàng phân trang 
create or ALTER PROC LayDSKH_PhanTrang
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        kh.maKH,
        kh.tenDoanhNghiep,
        kh.kyHieuDN,
        kh.diaChi,
        kh.nguoiDaiDien,
        kh.soDienThoaiKH,
        kh.maSoThue,
        kh.emailNguoiDaiDien,
        kh.emailDoanhNghiep,
        kh.trangThai 
    FROM KhachHang kh
    ORDER BY kh.maKH
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
--đếm tổng số khách hàng 
CREATE PROC DemTongKhachHang
AS
BEGIN
    SELECT COUNT(*) AS TongSo FROM KhachHang;
END
GO


--Lấy danh sách phân trang của kế hoạch quan trắc 
CREATE OR ALTER PROCEDURE [dbo].[LayDotQuanTrac_PhanTrang]
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        d.MaDot,
        d.MaHD,
        kh.tenDoanhNghiep AS TenKhachHang,  -- ✅ Thêm tên khách hàng
        d.NoiDung,
        d.DotQuanTrac,
        d.NgayBatDau,
        d.NgayDuKien,
        d.NgayTraKQ,
        d.TrangThai AS MaTrangThai,
        t.tenTrangThai AS TrangThai
    FROM DotQuanTrac d
    LEFT JOIN TrangThai_Dot t ON d.TrangThai = t.maTrangThai
    LEFT JOIN HopDong hd ON d.MaHD = hd.maHD          -- ✅ Join HopDong
    LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH       -- ✅ Join KhachHang
    ORDER BY d.MaDot DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
--Đếm kế hoạch quan trắc 
CREATE PROC DemTongKHQT
AS
BEGIN
    SELECT COUNT(*) AS TongSo FROM DotQuanTrac;
END
GO
--Đếm tổng nhân viên 
CREATE OR ALTER PROC DemTongNhanVien
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) 
    FROM NhanVien
    WHERE (daXoa IS NULL OR daXoa = 0);
END
GO

CREATE PROCEDURE [dbo].[sp_LayTrangThaiNhanVien]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        maTrangThai,
        tenTrangThai
    FROM [dbo].[TrangThai_NhanVien]
    ORDER BY maTrangThai;
END;
GO

CREATE PROCEDURE sp_XoaNenMauKhoiDot
    @maDN VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM Dot_Nen_Ts WHERE maDN = @maDN;
    DELETE FROM Dot_Nen WHERE maDN = @maDN;
END
GO
-- PTT THÊM PHẦN QUẢN LÝ KẾT QỦA VÀ INSERT THÊM DỮ LIỆU 5/11/2025

---- Lấy danh sách kết quả (cho dgvDanhsachketqua)
--CREATE PROCEDURE [dbo].[sp_LayDanhSachKetQua]
--AS
--BEGIN
--    SET NOCOUNT ON;
    
--    SELECT 
--        kqh.maKQ,
--        kqh.ngayTao,
--        kqh.ngayTraKQ,
--        nv.hoTen AS NguoiNhap,
--        CASE WHEN kqh.trangThaiXacNhan = 1 THEN N'Đã xác nhận' ELSE N'Chờ xác nhận' END AS TrangThai,
--        kqh.ghiChu,
--        dqt.dotQuanTrac,
--        dqt.maDot,
--        kh.tenDoanhNghiep AS TenKhachHang,
--        kh.emailDoanhNghiep AS EmailKhachHang,
--        kh.diaChi AS DiaChiKhachHang,
--        (SELECT COUNT(*) FROM KetQuaNenMau WHERE maKQ = kqh.maKQ) AS SoNenMau
--    FROM KetQuaHeader kqh
--    LEFT JOIN NhanVien nv ON kqh.nhanVienNhap = nv.maNV
--    LEFT JOIN DotQuanTrac dqt ON kqh.maDot = dqt.maDot
--    LEFT JOIN HopDong hd ON dqt.maHD = hd.maHD
--    LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH
--    ORDER BY kqh.ngayTao DESC;
--END

--GO

---- Lấy chi tiết kết quả theo mã KQ (cho dgvChiTiet)
--CREATE OR ALTER PROCEDURE [dbo].[sp_LayChiTietKetQuaTheoMaKQ]
--    @maKQ VARCHAR(15)
--AS
--BEGIN
--    SET NOCOUNT ON;
    
--    SELECT 
--        kqh.maKQ,
--        kqh.ngayTao,
--        kqh.ngayTraKQ,
--        nv.hoTen AS NguoiNhap,
--        kqh.trangThaiXacNhan,
--        kqh.ghiChu,
--        dqt.dotQuanTrac,
--        dqt.maDot,
--        -- ✅ THÊM THÔNG TIN KHÁCH HÀNG
--        kh.tenDoanhNghiep AS TenKhachHang,
--        kh.emailDoanhNghiep AS EmailKhachHang,
--        kh.diaChi AS DiaChiKhachHang,
--        dqt.noiDung AS DiaDiemQuanTrac,
--        -- Thông tin nền mẫu
--        kqn.maKQNen,
--        kqn.maNen,
--        nm.tenNenMau,
--        kqn.viTri,
--        kqn.toaDo,
--        -- Thông tin chi tiết thông số
--        kqct.maKQCT,
--        kqct.maTS,
--        ts.tenTS,
--        kqct.donVi,
--        kqct.phuongPhapPhanTich,
--        kqct.ketQua,
--        kqct.gioiHanPhatHien,
--        kqct.qcvn,
--        -- Đánh giá kết quả
--        CASE 
--            WHEN ts.giaTriToiDa IS NOT NULL AND kqct.ketQua > ts.giaTriToiDa THEN N'Vượt ngưỡng'
--            WHEN ts.giaTriToiThieu IS NOT NULL AND kqct.ketQua < ts.giaTriToiThieu THEN N'Dưới ngưỡng'
--            ELSE N'Đạt chuẩn'
--        END AS TinhTrang
--    FROM KetQuaHeader kqh
--    LEFT JOIN NhanVien nv ON kqh.nhanVienNhap = nv.maNV
--    LEFT JOIN DotQuanTrac dqt ON kqh.maDot = dqt.maDot
--    LEFT JOIN HopDong hd ON dqt.maHD = hd.maHD
--    LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH
--    LEFT JOIN KetQuaNenMau kqn ON kqh.maKQ = kqn.maKQ
--    LEFT JOIN NenMau nm ON kqn.maNen = nm.maNen
--    LEFT JOIN KetQuaChiTiet kqct ON kqn.maKQNen = kqct.maKQNen
--    LEFT JOIN ThongSoMoiTruong ts ON kqct.maTS = ts.maTS
--    WHERE kqh.maKQ = @maKQ
--    ORDER BY kqn.maKQNen, ts.tenTS;
--END
--GO

---- Cập nhật trạng thái xác nhận kết quả
--CREATE PROCEDURE [dbo].[sp_CapNhatTrangThaiKetQua]
--    @maKQ VARCHAR(15),
--    @trangThaiXacNhan BIT
--AS
--BEGIN
--    SET NOCOUNT ON;
--    BEGIN TRY
--        BEGIN TRANSACTION;
        
--        UPDATE KetQuaHeader 
--        SET trangThaiXacNhan = @trangThaiXacNhan 
--        WHERE maKQ = @maKQ;
        
--        IF @@ROWCOUNT = 0
--        BEGIN
--            ROLLBACK TRANSACTION;
--            SELECT 0 AS Result, N'Không tìm thấy kết quả!' AS Message;
--            RETURN;
--        END
        
--        COMMIT TRANSACTION;
--        SELECT 1 AS Result, N'Cập nhật trạng thái thành công!' AS Message;
--    END TRY
--    BEGIN CATCH
--        IF @@TRANCOUNT > 0
--            ROLLBACK TRANSACTION;
--        SELECT 0 AS Result, ERROR_MESSAGE() AS Message;
--    END CATCH
--END
--GO

-- Thêm mới kết quả header
CREATE PROCEDURE [dbo].[sp_ThemKetQuaHeader]
    @maDot VARCHAR(15),
    @nhanVienNhap VARCHAR(15),
    @ngayTraKQ DATE = NULL,
    @ghiChu NVARCHAR(MAX) = NULL,
    @maKQ VARCHAR(15) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        SET @maKQ = dbo.fn_TaoMaKQ();
        
        INSERT INTO KetQuaHeader (maKQ, maDot, nhanVienNhap, ngayTao, ngayTraKQ, trangThaiXacNhan, ghiChu)
        VALUES (@maKQ, @maDot, @nhanVienNhap, GETDATE(), @ngayTraKQ, 0, @ghiChu);
        
        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Thêm kết quả thành công!' AS Message, @maKQ AS MaKQ;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 0 AS Result, ERROR_MESSAGE() AS Message, NULL AS MaKQ;
    END CATCH
END
GO

-- Thêm nền mẫu vào kết quả
CREATE PROCEDURE [dbo].[sp_ThemKetQuaNenMau]
    @maKQ VARCHAR(15),
    @maNen VARCHAR(15),
    @viTri NVARCHAR(200) = NULL,
    @toaDo NVARCHAR(100) = NULL,
    @maKQNen VARCHAR(15) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Tạo mã KQNen mới
        DECLARE @maxNum INT;
        SELECT @maxNum = MAX(CAST(SUBSTRING(maKQNen, 5, LEN(maKQNen)) AS INT))
        FROM KetQuaNenMau WHERE maKQNen LIKE 'KQNM%';
        IF @maxNum IS NULL SET @maxNum = 0;
        SET @maKQNen = 'KQNM' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
        
        INSERT INTO KetQuaNenMau (maKQNen, maKQ, maNen, viTri, toaDo)
        VALUES (@maKQNen, @maKQ, @maNen, @viTri, @toaDo);
        
        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Thêm nền mẫu thành công!' AS Message, @maKQNen AS MaKQNen;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 0 AS Result, ERROR_MESSAGE() AS Message, NULL AS MaKQNen;
    END CATCH
END
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_suaHopDong]
    @maHD VARCHAR(15),
    @maKH VARCHAR(15),
    @ngayKy DATE,
    @ngayKetThucHD DATE,
    @trangThai VARCHAR(15),        -- ví dụ: TT01/TT02/TT03/TT04
    @tanSuatQuanTrac VARCHAR(15),  -- FK tới tanSuatQT.maTSQT
    @soHD NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;  -- an toàn rollback tự động khi lỗi

    BEGIN TRY
        BEGIN TRAN;

        -- 1) Tồn tại hợp đồng cần sửa
        IF NOT EXISTS (SELECT 1 FROM HopDong WHERE maHD = @maHD)
        BEGIN
            RAISERROR(N'Không tìm thấy hợp đồng cần sửa!', 16, 1);
        END

        -- 2) Kiểm tra ngày
        IF (@ngayKy >= @ngayKetThucHD)
        BEGIN
            RAISERROR(N'Ngày kết thúc phải sau ngày ký.', 16, 1);
        END

        -- 3) Kiểm tra tồn tại KH & Tần suất (nếu chưa có FK cứng)
        IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE maKH = @maKH)
        BEGIN
            RAISERROR(N'Không tìm thấy khách hàng.', 16, 1);
        END

        IF NOT EXISTS (SELECT 1 FROM tanSuatQT WHERE maTSQT = @tanSuatQuanTrac)
        BEGIN
            RAISERROR(N'Không tìm thấy tần suất quan trắc.', 16, 1);
        END

        -- 4) Quy tắc trạng thái theo ngày (cân nhắc điều chỉnh cho TT03 nếu duyệt tay)
        DECLARE @today DATE = CAST(GETDATE() AS DATE);

        IF (@trangThai = 'TT01' AND NOT (@ngayKy <= @today AND @today <= @ngayKetThucHD))
            RAISERROR(N'Trạng thái đang hiệu lực yêu cầu ngày hiện tại nằm trong khoảng từ ngày ký đến ngày kết thúc.', 16, 1);

        IF (@trangThai = 'TT02' AND NOT (@today > @ngayKetThucHD))
            RAISERROR(N'Trạng thái hết hạn yêu cầu ngày hiện tại đã sau ngày kết thúc.', 16, 1);

        -- Nếu TT03 là duyệt tay, hãy cân nhắc bỏ ràng buộc ngày:
        -- IF (@trangThai = 'TT03' AND NOT (@today > @ngayKetThucHD))
        --     RAISERROR(N'Trạng thái hoàn thành yêu cầu ngày hiện tại đã sau ngày kết thúc.', 16, 1);

        IF (@trangThai = 'TT04' AND NOT (@ngayKy <= @today AND @today <= @ngayKetThucHD))
            RAISERROR(N'Trạng thái chấm dứt trước thời hạn yêu cầu hợp đồng đang trong thời gian hiệu lực.', 16, 1);

        -- 5) Check trùng (cùng KH, cùng ngày ký) nhưng loại trừ chính hợp đồng đang sửa
        DECLARE @tenDN NVARCHAR(100);
        SELECT @tenDN = tenDoanhNghiep FROM KhachHang WHERE maKH = @maKH;

        IF EXISTS (
            SELECT 1
            FROM HopDong
            WHERE maKH = @maKH
              AND ngayKy = @ngayKy
              AND maHD <> @maHD
        )
        BEGIN
            DECLARE @day VARCHAR(10) = CONVERT(VARCHAR(10), @ngayKy, 103); -- dd/MM/yyyy
            RAISERROR(N'Đã có hợp đồng của %s vào ngày %s.', 16, 1, @tenDN, @day);
        END

        -- 6) Cập nhật
        UPDATE HopDong
        SET maKH             = @maKH,
            ngayKy           = @ngayKy,
            ngayKetThucHD    = @ngayKetThucHD,
            trangThai        = @trangThai,
            tanSuatQuanTrac  = @tanSuatQuanTrac,
            soHD             = @soHD
        WHERE maHD = @maHD;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0) ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrState INT = ERROR_STATE();

        RAISERROR(@ErrMsg, @ErrSeverity, @ErrState);
    END CATCH
END
GO
-- Thêm chi tiết thông số đo
CREATE PROCEDURE [dbo].[sp_ThemKetQuaChiTiet]
    @maKQNen VARCHAR(15),
    @maTS VARCHAR(15),
    @donVi NVARCHAR(15) = NULL,
    @phuongPhapPhanTich NVARCHAR(200) = NULL,
    @ketQua FLOAT,
    @gioiHanPhatHien NVARCHAR(50) = NULL,
    @qcvn NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Tạo mã KQCT mới
        DECLARE @maKQCT VARCHAR(15);
        DECLARE @maxNum INT;
        SELECT @maxNum = MAX(CAST(SUBSTRING(maKQCT, 5, LEN(maKQCT)) AS INT))
        FROM KetQuaChiTiet WHERE maKQCT LIKE 'KQCT%';
        IF @maxNum IS NULL SET @maxNum = 0;
        SET @maKQCT = 'KQCT' + RIGHT('0000' + CAST(@maxNum + 1 AS VARCHAR(4)), 4);
        
        INSERT INTO KetQuaChiTiet (maKQCT, maKQNen, maTS, donVi, phuongPhapPhanTich, ketQua, gioiHanPhatHien, qcvn)
        VALUES (@maKQCT, @maKQNen, @maTS, @donVi, @phuongPhapPhanTich, @ketQua, @gioiHanPhatHien, @qcvn);
        
        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Thêm chi tiết thành công!' AS Message, @maKQCT AS MaKQCT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 0 AS Result, ERROR_MESSAGE() AS Message, NULL AS MaKQCT;
    END CATCH
END
GO

-- Xóa kết quả
CREATE PROCEDURE [dbo].[sp_XoaKetQua]
    @maKQ VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra trạng thái xác nhận
        DECLARE @trangThai BIT;
        SELECT @trangThai = trangThaiXacNhan FROM KetQuaHeader WHERE maKQ = @maKQ;
        
        IF @trangThai = 1
        BEGIN
            ROLLBACK TRANSACTION;
            SELECT 0 AS Result, N'Không thể xóa kết quả đã xác nhận!' AS Message;
            RETURN;
        END
        
        -- Xóa (CASCADE sẽ tự động xóa các bảng liên quan)
        DELETE FROM KetQuaHeader WHERE maKQ = @maKQ;
        
        COMMIT TRANSACTION;
        SELECT 1 AS Result, N'Xóa kết quả thành công!' AS Message;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        SELECT 0 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- Lấy thông tin tổng quan của kết quả
CREATE PROCEDURE [dbo].[sp_LayThongTinKetQua]
    @maKQ VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        kqh.maKQ,
        kqh.ngayTao,
        kqh.ngayTraKQ,
        kqh.trangThaiXacNhan,
        kqh.ghiChu,
        nv.hoTen AS NguoiNhap,
        nv.email AS EmailNguoiNhap,
        pb.tenPhong,
        dqt.dotQuanTrac,
        dqt.noiDung,
        COUNT(DISTINCT kqn.maNen) AS TongSoNenMau,
        COUNT(DISTINCT kqct.maTS) AS TongSoThongSo
    FROM KetQuaHeader kqh
    INNER JOIN NhanVien nv ON kqh.nhanVienNhap = nv.maNV
    INNER JOIN PhongBan pb ON nv.maPhong = pb.maPhong
    LEFT JOIN DotQuanTrac dqt ON kqh.maDot = dqt.maDot
    LEFT JOIN KetQuaNenMau kqn ON kqh.maKQ = kqn.maKQ
    LEFT JOIN KetQuaChiTiet kqct ON kqn.maKQNen = kqct.maKQNen
    WHERE kqh.maKQ = @maKQ
    GROUP BY kqh.maKQ, kqh.ngayTao, kqh.ngayTraKQ, kqh.trangThaiXacNhan, 
             kqh.ghiChu, nv.hoTen, nv.email, pb.tenPhong, dqt.dotQuanTrac, dqt.noiDung;
END
GO

-- =============================================
-- STORED PROCEDURES CHO QUẢN LÝ BÁO CÁO (Giữ nguyên tương thích)
-- =============================================

CREATE PROCEDURE [dbo].[sp_LayDanhSachBaoCao]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        bc.maBC AS MaBC,
        bc.maDot AS MaDot,
        nv.hoTen AS TenNguoiXuat,
        bc.ngayXuat AS NgayXuat,
        (SELECT COUNT(DISTINCT maNen) FROM Dot_Nen WHERE maDot = bc.maDot) AS SoNenMau,
        (SELECT COUNT(DISTINCT maTS) FROM Dot_Nen_Ts dnts 
         INNER JOIN Dot_Nen dn ON dnts.maDN = dn.maDN 
         WHERE dn.maDot = bc.maDot) AS TongSoThongSo,
        CASE 
            WHEN EXISTS (
                SELECT 1 FROM KetQuaHeader kqh 
                WHERE kqh.maDot = bc.maDot AND kqh.trangThaiXacNhan = 0
            ) THEN N'Chưa hoàn tất'
            ELSE N'Đã xác nhận'
        END AS TrangThai
    FROM BaoCaoKetQua bc
    INNER JOIN NhanVien nv ON bc.nguoiXuat = nv.maNV
    ORDER BY bc.ngayXuat DESC;
END
GO

CREATE PROCEDURE [dbo].[sp_LayChiTietKetQuaTheoBC]
    @maBC VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Lấy maDot từ báo cáo
    DECLARE @maDot VARCHAR(15);
    SELECT @maDot = maDot FROM BaoCaoKetQua WHERE maBC = @maBC;
    
    -- Lấy tất cả kết quả của đợt quan trắc này
    SELECT 
        kqh.maKQ,
        kqh.ngayTao,
        kqh.ngayTraKQ,
        nv.hoTen AS NguoiNhap,
        kqh.trangThaiXacNhan,
        kqh.ghiChu,
        kqn.maNen,
        nm.tenNenMau,
        kqn.viTri,
        kqn.toaDo,
        kqct.maTS,
        ts.tenTS,
        kqct.donVi,
        kqct.phuongPhapPhanTich,
        kqct.ketQua,
        kqct.gioiHanPhatHien,
        kqct.qcvn,
        CASE 
            WHEN ts.giaTriToiDa IS NOT NULL AND kqct.ketQua > ts.giaTriToiDa THEN N'Vượt ngưỡng'
            WHEN ts.giaTriToiThieu IS NOT NULL AND kqct.ketQua < ts.giaTriToiThieu THEN N'Dưới ngưỡng'
            ELSE N'Đạt chuẩn'
        END AS TinhTrang
    FROM KetQuaHeader kqh
    INNER JOIN NhanVien nv ON kqh.nhanVienNhap = nv.maNV
    LEFT JOIN KetQuaNenMau kqn ON kqh.maKQ = kqn.maKQ
    LEFT JOIN NenMau nm ON kqn.maNen = nm.maNen
    LEFT JOIN KetQuaChiTiet kqct ON kqn.maKQNen = kqct.maKQNen
    LEFT JOIN ThongSoMoiTruong ts ON kqct.maTS = ts.maTS
    WHERE kqh.maDot = @maDot
    ORDER BY kqh.ngayTao DESC, kqn.maKQNen, ts.tenTS;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    procedure [dbo].[layDanhSachTSQT]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		tsqt.maTSQT,
		tsqt.tenTSQT
    FROM tanSuatQT tsqt
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE    procedure [dbo].[layDanhSachTrangThaiHD]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
		tt.maTT,
		tt.tenTT
    FROM trangThaiHD tt
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[layDotQuanTracNhapLieu_PhanTrang]
    @pageNumber INT,
    @pageSize   INT,
    @maPhong    VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Dot AS (
        SELECT
            d.maDot,
            d.maHD,
            d.ngayBatDau,
            d.ngayDuKien,
            ngayConLai = DATEDIFF(DAY, CAST(GETDATE() AS date), CAST(d.ngayDuKien AS date)),

            -- Hoàn thành nếu KHÔNG còn bất kỳ TS nào (thuộc phòng này) chưa có giá trị đo
            hoanThanh = CASE
                WHEN NOT EXISTS (
                    SELECT 1
                    FROM Dot_Nen dn
                    JOIN Dot_Nen_TS ts ON ts.maDN = dn.maDN
                    LEFT JOIN KetQua k  ON k.maDNTS = ts.maDNTS
                    WHERE dn.maDot = d.maDot
                      AND ts.maPhong = @maPhong
                      AND (k.maDNTS IS NULL OR k.giaTriDoDuoc IS NULL)
                )
                THEN 1 ELSE 0
            END
        FROM DotQuanTrac d
        -- 🔒 Chỉ các đợt thuộc 1 trong các trạng thái cho phép nhập liệu
        WHERE d.trangThai IN (1, 2, 4)

        -- 🔒 Chỉ những đợt có ÍT NHẤT 1 công việc giao cho phòng @maPhong
          AND EXISTS (
                SELECT 1
                FROM Dot_Nen dn
                JOIN Dot_Nen_TS ts ON ts.maDN = dn.maDN
                WHERE dn.maDot = d.maDot
                  AND ts.maPhong = @maPhong
          )
    ),
    Base AS (
        SELECT
            maDot, maHD, ngayBatDau, ngayDuKien, ngayConLai,
            trangThai =
                CASE
                    WHEN hoanThanh = 1              THEN N'Hoàn thành'
                    WHEN ngayConLai < 0             THEN N'Hết hạn'
                    WHEN ngayConLai BETWEEN 0 AND 7 THEN N'Gần hết hạn'
                    ELSE                               N'Còn hạn'
                END
        FROM Dot
    )
    -- Phân trang
    SELECT  maDot, maHD, ngayBatDau, ngayDuKien, ngayConLai, trangThai
    FROM    Base
    ORDER BY ngayConLai DESC, maDot
    OFFSET (@pageNumber - 1) * @pageSize ROWS
    FETCH NEXT @pageSize ROWS ONLY;

    -- Trả thêm tổng bản ghi cho UI
    SELECT TotalRecords = COUNT(*) FROM Base;
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[LayDanhSachNenMauNhapLieu]
    @maPhong VARCHAR(15),
    @maDot   VARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        dn.maDN,  -- Mã Dot_Nen
        tenDayDu =
            CONCAT(
                nm.tenNenMau, 
                N' - ', 
                ISNULL(dn.tenViTri, N''), 
                N' - ',
                CASE 
                    WHEN COUNT(ts.maDNTS) > 0
                     AND COUNT(ts.maDNTS) = SUM(CASE WHEN k.giaTriDoDuoc IS NOT NULL THEN 1 ELSE 0 END)
                    THEN N'Hoàn thành'
                    ELSE N'Chưa hoàn thành'
                END
            )
    FROM Dot_Nen dn
    JOIN NenMau nm     ON nm.maNen = dn.maNen
    JOIN Dot_Nen_TS ts ON ts.maDN = dn.maDN AND ts.maPhong = @maPhong
    LEFT JOIN KetQua k ON k.maDNTS = ts.maDNTS
    WHERE dn.maDot = @maDot
    GROUP BY dn.maDN, nm.tenNenMau, dn.tenViTri
    ORDER BY nm.tenNenMau, dn.tenViTri;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[LayThongTinDotNen]
    @maDN VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT                      
        dn.tenViTri as viTri,              
        dn.toaDo as toaDo,
        dn.ghiChu as ghiChu,                        
        nm.tenNenMau as tenNenMau
    FROM Dot_Nen dn
    JOIN NenMau nm ON nm.maNen = dn.maNen
    WHERE dn.maDN = @maDN;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[LayDanhSachThongSoTheoDotNenVaPhong]
    @maDN     VARCHAR(20),
    @maPhong  VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ts.maDNTS as maDNTS,            
        ts.maDN,                  
        ts.maPhong,                   
        ts.maTS as maTS,              
        ts.tenTS as tenTS,        
		ts.phuongPhap as phuongPhap,
        ts.donVi as donVi,                
        ts.giaTriToiDa as giaTriToiDa,             
        ts.giaTriToiThieu as giaTriToiThieu,            
        k.giaTriDoDuoc,     
		k.ngayDo,
        trangThai = CASE 
                        WHEN k.giaTriDoDuoc IS NOT NULL THEN N'Đã nhập'
                        ELSE N'Chưa nhập'
                    END
    FROM Dot_Nen_TS ts
    JOIN ThongSoMoiTruong t ON t.maTS = ts.maTS
    LEFT JOIN KetQua k ON k.maDNTS = ts.maDNTS
    WHERE ts.maDN = @maDN
      AND ts.maPhong = @maPhong
    ORDER BY 
        CASE 
            WHEN k.giaTriDoDuoc IS NULL THEN 0
            ELSE 1
        END,
        t.tenTS;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[LayThongSoTheoMaDotNenTS]
    @maDNTS  NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1)
           tenTS,
           donVi,
           giaTriToiThieu,
           giaTriToiDa
    FROM dbo.Dot_Nen_TS
    WHERE maDNTS = @maDNTS
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[LayNhanVienTheoTenDN]
    @Email NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (1) maNV
    FROM dbo.NhanVien
    WHERE email = @Email;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[ThemKetQua]
    @maDNTS        VARCHAR(15),
    @maNV          VARCHAR(15),
    @ngayDo        DATE,
    @giaTriDoDuoc  DECIMAL(18,2)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Kiểm tra giá trị NULL hoặc rỗng
        IF (@maDNTS IS NULL OR LTRIM(RTRIM(@maDNTS)) = '')
        BEGIN
            RAISERROR(N'Mã đợt nền thông số không được để trống.', 16, 1);
            RETURN;
        END

        IF (@maNV IS NULL OR LTRIM(RTRIM(@maNV)) = '')
        BEGIN
            RAISERROR(N'Mã nhân viên không được để trống.', 16, 1);
            RETURN;
        END

        IF (@ngayDo IS NULL)
        BEGIN
            RAISERROR(N'Ngày đo không được để trống.', 16, 1);
            RETURN;
        END

        IF (@giaTriDoDuoc IS NULL)
        BEGIN
            RAISERROR(N'Giá trị đo được không được để trống.', 16, 1);
            RETURN;
        END

		IF (@ngayDo > GETDATE())
        BEGIN
            RAISERROR(N'Ngày đo không được trước ngày hiện tại.', 16, 1);
            RETURN;
        END

        -- Bắt đầu giao dịch
        BEGIN TRAN;

        DECLARE @maKQ VARCHAR(15);
        DECLARE @so INT;

        -- Sinh mã tự động dạng KQ001, KQ002,...
        SELECT @so = CAST(SUBSTRING(maKQ, 3, LEN(maKQ)) AS INT)
        FROM KetQua
        WHERE maKQ = (SELECT MAX(maKQ) FROM KetQua);

        IF @so IS NULL SET @so = 0;
        SET @so = @so + 1;

        SET @maKQ = 'KQ' + RIGHT('000' + CAST(@so AS VARCHAR(3)), 3);
		if EXISTS (
            SELECT 1
            FROM KetQua kq
            WHERE kq.maDNTS = @maDNTS) 
		BEGIN
			UPDATE dbo.KetQua set maKQ = @maKQ, nhanVienNhap = @maNV, ngayDo = @ngayDo, giaTriDoDuoc = @giaTriDoDuoc where maDNTS = @maDNTS
		END;
		else 
		BEGIN
			INSERT INTO dbo.KetQua (maKQ, maDNTS, nhanVienNhap, ngayDo, giaTriDoDuoc)
			VALUES (@maKQ, @maDNTS, @maNV, @ngayDo, @giaTriDoDuoc);
		END;

        COMMIT TRAN;
        PRINT N'Thêm kết quả thành công với mã: ' + @maKQ;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH
END
GO

PRINT N'========================================';
PRINT N'✅ HOÀN THÀNH KHỞI TẠO DATABASE';
PRINT N'- Đã tạo 7 functions';
PRINT N'- Đã tạo 18 tables (3 bảng mới cho hệ thống kết quả)';
PRINT N'- Đã thêm dữ liệu mẫu';
PRINT N'- Đã tạo 11 stored procedures';
PRINT N'========================================';
PRINT N'';
PRINT N'CẤU TRÚC MỚI:';
PRINT N'- KetQuaHeader: Thông tin chung kết quả (MaKQ, NgayTao, NguoiNhap, TrangThai)';
PRINT N'- KetQuaNenMau: Danh sách nền mẫu trong kết quả';
PRINT N'- KetQuaChiTiet: Chi tiết các thông số đo của từng nền mẫu';
PRINT N'========================================';
GO
-- =============================================
-- INSERT 20 MẪU DỮ LIỆU KẾT QUẢ QUAN TRẮC (FIXED)
-- =============================================
-- Thêm thêm một số đợt quan trắc
INSERT INTO [DotQuanTrac] VALUES
('DT0002', 'HD007', N'Quan trắc định kỳ quý 3', N'Quý 3/2025', '2025-08-01', '2025-09-30', '2025-09-15', 5, 1),
('DT0003', 'HD007', N'Quan trắc định kỳ quý 2', N'Quý 2/2025', '2025-05-01', '2025-06-30', '2025-06-20', 5, 2),
('DT0004', 'HD007', N'Quan trắc đột xuất tháng 10', N'Tháng 10/2025', '2025-10-01', '2025-10-15', '2025-10-10', 2, 3);
GO
-- Thêm thêm một số nền mẫu
INSERT INTO [NenMau] VALUES
('NM0001', N'Nền mẫu nước biển', N'Nước biển'),
('NM0002', N'Nền mẫu nước sông', N'Nước sông'),
('NM0003', N'Nền mẫu nước thải', N'Nước thải'),  
('NM0004', N'Nền mẫu nước ngầm', N'Nước ngầm'),
('NM0005', N'Nền mẫu nước mưa', N'Nước mưa'),
('NM0006', N'Nền mẫu đất', N'Đất'),
('NM0007', N'Nền mẫu không khí', N'Không khí');

GO
-- INSERT 20 KẾT QUẢ HEADER
INSERT INTO [KetQuaHeader] VALUES
-- Kết quả đã xác nhận (tháng trước)
('KQ0003', 'DT0002', 'NV001', '2025-09-01', '2025-09-15', 1, N'Kết quả quan trắc quý 3 - Đã xác nhận'),
('KQ0004', 'DT0002', 'NV002', '2025-09-05', '2025-09-15', 1, N'Bổ sung kết quả sau mưa lớn'),
('KQ0005', 'DT0002', 'NV001', '2025-09-10', '2025-09-15', 1, N'Kết quả kiểm tra lại điểm ô nhiễm'),
-- Kết quả chờ xác nhận (tháng hiện tại)
('KQ0006', 'DT0001', 'NV002', '2025-11-02', '2025-12-15', 0, N'Kết quả quan trắc tuần 1 tháng 11'),
('KQ0007', 'DT0001', 'NV001', '2025-11-03', '2025-12-15', 0, N'Kết quả quan trắc điểm bổ sung'),
('KQ0008', 'DT0001', 'NV003', '2025-11-04', '2025-12-15', 0, N'Quan trắc sau sự cố tràn dầu'),
('KQ0009', 'DT0001', 'NV002', '2025-11-05', '2025-12-15', 0, N'Kết quả định kỳ tuần 1'),
('KQ0010', 'DT0001', 'NV001', '2025-11-06', '2025-12-15', 0, NULL),
-- Kết quả đợt tháng 10 (đã xác nhận)
('KQ0011', 'DT0004', 'NV002', '2025-10-02', '2025-10-10', 1, N'Quan trắc đột xuất - Nghi ngờ ô nhiễm'),
('KQ0012', 'DT0004', 'NV001', '2025-10-03', '2025-10-10', 1, N'Kiểm tra lại kết quả bất thường'),
('KQ0013', 'DT0004', 'NV003', '2025-10-05', '2025-10-10', 1, N'Quan trắc mở rộng khu vực'),
-- Kết quả quý 3 bổ sung
('KQ0014', 'DT0002', 'NV002', '2025-08-15', '2025-09-15', 1, N'Quan trắc đầu quý 3'),
('KQ0015', 'DT0002', 'NV001', '2025-08-20', '2025-09-15', 1, N'Quan trắc giữa quý 3'),
('KQ0016', 'DT0002', 'NV003', '2025-08-25', '2025-09-15', 1, N'Quan trắc cuối quý 3'),
-- Kết quả tháng 11 mới nhất (chưa xác nhận)
('KQ0017', 'DT0001', 'NV001', '2025-11-07', '2025-12-15', 0, N'Quan trắc tuần 2 tháng 11'),
('KQ0018', 'DT0001', 'NV002', '2025-11-08', '2025-12-15', 0, N'Quan trắc bổ sung điểm mới'),
('KQ0019', 'DT0001', 'NV003', '2025-11-09', '2025-12-15', 0, N'Kiểm tra chất lượng nước mưa'),
('KQ0020', 'DT0001', 'NV001', '2025-11-10', '2025-12-15', 0, N'Quan trắc định kỳ'),
('KQ0021', 'DT0001', 'NV002', '2025-11-11', '2025-12-15', 0, N'Phân tích kim loại nặng'),
('KQ0022', 'DT0001', 'NV003', '2025-11-12', '2025-12-15', 0, N'Quan trắc toàn diện tháng 11');
GO
-- INSERT NỀN MẪU CHO CÁC KẾT QUẢ
INSERT INTO [KetQuaNenMau] VALUES
-- KQ0003: 3 nền mẫu
('KQNM0004', 'KQ0003', 'NM0001', N'Biển Hải Phòng - Điểm 1', N'20.8571, 106.6830'),
('KQNM0005', 'KQ0003', 'NM0002', N'Sông Hồng - Điểm 2', N'21.0345, 105.8512'),
('KQNM0006', 'KQ0003', 'NM0003', N'Nước thải - KCN', N'21.0123, 105.8234'), 

-- KQ0004: 2 nền mẫu
('KQNM0007', 'KQ0004', 'NM0002', N'Sông Đồng Nai - Điểm 1', N'10.9461, 106.8189'),
('KQNM0008', 'KQ0004', 'NM0003', N'KCN Long An - Điểm xả', N'10.7142, 106.3975'),
-- KQ0005: 4 nền mẫu
('KQNM0009', 'KQ0005', 'NM0001', N'Biển Vũng Tàu - Gần bờ', N'10.4113, 107.1362'),
('KQNM0010', 'KQ0005', 'NM0002', N'Sông Sài Gòn - Điểm 3', N'10.7545, 106.6801'),
('KQNM0011', 'KQ0005', 'NM0003', N'Khu xử lý nước thải', N'10.6234, 106.5678'),
('KQNM0012', 'KQ0005', 'NM0004', N'Nước ngầm - Giếng khoan', N'10.7234, 106.6234'),
-- KQ0006: 3 nền mẫu
('KQNM0013', 'KQ0006', 'NM0002', N'Sông Hồng - Cầu Long Biên', N'21.0456, 105.8567'),
('KQNM0014', 'KQ0006', 'NM0003', N'Nhà máy HCAP - Điểm 1', N'10.5433, 106.4234'),
('KQNM0015', 'KQ0006', 'NM0004', N'Nước ngầm - Khu dân cư', N'10.7890, 106.6543'),
-- KQ0007: 2 nền mẫu
('KQNM0016', 'KQ0007', 'NM0001', N'Biển Hải Phòng - Điểm 2', N'20.8671, 106.6930'),
('KQNM0017', 'KQ0007', 'NM0002', N'Sông Cầu - Điểm giám sát', N'13.7841, 109.2167'),
-- KQ0008: 5 nền mẫu (sự cố tràn dầu)
('KQNM0018', 'KQ0008', 'NM0001', N'Biển - Tâm sự cố', N'10.4213, 107.1462'),
('KQNM0019', 'KQ0008', 'NM0001', N'Biển - Bán kính 500m', N'10.4267, 107.1489'),
('KQNM0020', 'KQ0008', 'NM0001', N'Biển - Bán kính 1km', N'10.4313, 107.1562'),
('KQNM0021', 'KQ0008', 'NM0002', N'Sông ven biển', N'10.4156, 107.1389'),
('KQNM0022', 'KQ0008', 'NM0003', N'Nước thải gần khu vực', N'10.4189, 107.1423'),
-- KQ0009: 3 nền mẫu
('KQNM0023', 'KQ0009', 'NM0002', N'Sông Hồng - Điểm 1', N'21.0245, 105.8412'),
('KQNM0024', 'KQ0009', 'NM0003', N'Nhà máy - Xả thải', N'10.5433, 106.4234'),
('KQNM0025', 'KQ0009', 'NM0004', N'Nước ngầm - KV1', N'10.7123, 106.6345'),
-- KQ0010: 2 nền mẫu
('KQNM0026', 'KQ0010', 'NM0002', N'Sông Đồng Nai - Thượng nguồn', N'11.0567, 107.2345'),
('KQNM0027', 'KQ0010', 'NM0003', N'Nước thải KCN', N'10.7234, 106.5123'),
-- KQ0011: 4 nền mẫu (đột xuất)
('KQNM0028', 'KQ0011', 'NM0002', N'Sông - Điểm nghi ngờ', N'10.9567, 106.8345'),
('KQNM0029', 'KQ0011', 'NM0003', N'Nước thải - Nguồn xả', N'10.9456, 106.8234'),
('KQNM0030', 'KQ0011', 'NM0002', N'Sông - Hạ lưu 1km', N'10.9478, 106.8456'),
('KQNM0031', 'KQ0011', 'NM0004', N'Nước ngầm lân cận', N'10.9512, 106.8389'),
-- KQ0012: 3 nền mẫu
('KQNM0032', 'KQ0012', 'NM0002', N'Sông Hồng - Kiểm tra lại', N'21.0345, 105.8512'),
('KQNM0033', 'KQ0012', 'NM0003', N'Nhà máy - Điểm xả', N'10.5433, 106.4234'),
('KQNM0034', 'KQ0012', 'NM0001', N'Biển - Gần cửa sông', N'20.8671, 106.6930'),
-- KQ0013: 6 nền mẫu (mở rộng khu vực)
('KQNM0035', 'KQ0013', 'NM0002', N'Sông - Điểm trung tâm', N'10.9567, 106.8345'),
('KQNM0036', 'KQ0013', 'NM0002', N'Sông - Phía Đông', N'10.9678, 106.8456'),
('KQNM0037', 'KQ0013', 'NM0002', N'Sông - Phía Tây', N'10.9456, 106.8234'),
('KQNM0038', 'KQ0013', 'NM0003', N'Nước thải - Nguồn 1', N'10.9512, 106.8389'),
('KQNM0039', 'KQ0013', 'NM0003', N'Nước thải - Nguồn 2', N'10.9589, 106.8412'),
('KQNM0040', 'KQ0013', 'NM0004', N'Nước ngầm - Khu vực', N'10.9623, 106.8456'),
-- KQ0014: 2 nền mẫu
('KQNM0041', 'KQ0014', 'NM0001', N'Biển Hải Phòng - Đầu Q3', N'20.8571, 106.6830'),
('KQNM0042', 'KQ0014', 'NM0002', N'Sông Hồng - Đầu Q3', N'21.0245, 105.8412'),
-- KQ0015: 3 nền mẫu
('KQNM0043', 'KQ0015', 'NM0001', N'Biển Vũng Tàu - Giữa Q3', N'10.4113, 107.1362'),
('KQNM0044', 'KQ0015', 'NM0002', N'Sông Sài Gòn - Giữa Q3', N'10.7545, 106.6801'),
('KQNM0045', 'KQ0015', 'NM0003', N'Nhà máy HCAP - Giữa Q3', N'10.5433, 106.4234'),
-- KQ0016: 3 nền mẫu
('KQNM0046', 'KQ0016', 'NM0002', N'Sông Đồng Nai - Cuối Q3', N'10.9461, 106.8189'),
('KQNM0047', 'KQ0016', 'NM0003', N'KCN Long An - Cuối Q3', N'10.7142, 106.3975'),
('KQNM0048', 'KQ0016', 'NM0004', N'Nước ngầm - Cuối Q3', N'10.7234, 106.6234'),
-- KQ0017: 4 nền mẫu
('KQNM0049', 'KQ0017', 'NM0002', N'Sông Hồng - Tuần 2/T11', N'21.0245, 105.8412'),
('KQNM0050', 'KQ0017', 'NM0003', N'Nhà máy - Tuần 2/T11', N'10.5433, 106.4234'),
('KQNM0051', 'KQ0017', 'NM0004', N'Nước ngầm - Tuần 2/T11', N'10.7123, 106.6345'),
('KQNM0052', 'KQ0017', 'NM0001', N'Biển - Tuần 2/T11', N'20.8571, 106.6830'),
-- KQ0018: 2 nền mẫu
('KQNM0053', 'KQ0018', 'NM0002', N'Sông Cầu - Điểm mới', N'13.7841, 109.2167'),
('KQNM0054', 'KQ0018', 'NM0005', N'Nước mưa - Khu vực mới', N'13.7923, 109.2234'),
-- KQ0019: 3 nền mẫu (nước mưa)
('KQNM0055', 'KQ0019', 'NM0005', N'Nước mưa - Khu A', N'10.7567, 106.6789'),
('KQNM0056', 'KQ0019', 'NM0005', N'Nước mưa - Khu B', N'10.7623, 106.6845'),
('KQNM0057', 'KQ0019', 'NM0005', N'Nước mưa - Khu C', N'10.7689, 106.6912'),
-- KQ0020: 3 nền mẫu
('KQNM0058', 'KQ0020', 'NM0002', N'Sông Hồng - Định kỳ', N'21.0245, 105.8412'),
('KQNM0059', 'KQ0020', 'NM0003', N'Nhà máy - Định kỳ', N'10.5433, 106.4234'),
('KQNM0060', 'KQ0020', 'NM0004', N'Nước ngầm - Định kỳ', N'10.7234, 106.6234'),
-- KQ0021: 4 nền mẫu (kim loại nặng)
('KQNM0061', 'KQ0021', 'NM0002', N'Sông - Phân tích KLN', N'10.9567, 106.8345'),
('KQNM0062', 'KQ0021', 'NM0003', N'Nước thải - Phân tích KLN', N'10.9456, 106.8234'),
('KQNM0063', 'KQ0021', 'NM0004', N'Nước ngầm - Phân tích KLN', N'10.9623, 106.8456'),
('KQNM0064', 'KQ0021', 'NM0001', N'Biển - Phân tích KLN', N'10.4213, 107.1462'),
-- KQ0022: 5 nền mẫu (toàn diện)
('KQNM0065', 'KQ0022', 'NM0001', N'Biển Hải Phòng - Toàn diện', N'20.8571, 106.6830'),
('KQNM0066', 'KQ0022', 'NM0002', N'Sông Hồng - Toàn diện', N'21.0245, 105.8412'),
('KQNM0067', 'KQ0022', 'NM0003', N'Nhà máy HCAP - Toàn diện', N'10.5433, 106.4234'),
('KQNM0068', 'KQ0022', 'NM0004', N'Nước ngầm - Toàn diện', N'10.7234, 106.6234'),
('KQNM0069', 'KQ0022', 'NM0005', N'Nước mưa - Toàn diện', N'10.7567, 106.6789');
GO
-- INSERT CHI TIẾT CÁC THÔNG SỐ
-- KQ0003 - Nền mẫu KQNM0004 (Biển)
INSERT INTO [KetQuaChiTiet] VALUES
('KQCT0016', 'KQNM0004', 'TS0001', N'-', N'TCVN 6492:2011', 8.12, N'', N'5,5 - 9'),
('KQCT0017', 'KQNM0004', 'TS0006', N'mg/L', N'TCVN 6625:2000', 45.3, N'KPH (LOD=4)', N'100'),
('KQCT0018', 'KQNM0004', 'TS0009', N'mg/L', N'TCVN 6494-1:2011', 18500, N'', N'Không quy định'),
('KQCT0019', 'KQNM0004', 'TS0010', N'mg/L', N'TCVN 6494-1:2011', 0.8, N'KPH (LOD=0,05)', N'1,5');
-- KQ0003 - Nền mẫu KQNM0005 (Sông)
INSERT INTO [KetQuaChiTiet] VALUES
('KQCT0020', 'KQNM0005', 'TS0001', N'-', N'TCVN 6492:2011', 7.45, N'', N'5,5 - 9'),
('KQCT0021', 'KQNM0005', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 28.5, N'KPH (LOD=2)', N'50'),
('KQCT0022', 'KQNM0005', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 95.7, N'KPH (LOD=3)', N'150'),
('KQCT0023', 'KQNM0005', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.45, N'KPH (LOD=0,01)', N'10'),
('KQCT0024', 'KQNM0005', 'TS0005', N'mg/L', N'TCVN 6638:2000', 8.2, N'', N'40');
-- KQ0003 - Nền mẫu KQNM0006 (Nước thải)
INSERT INTO [KetQuaChiTiet] VALUES
('KQCT0026', 'KQNM0006', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 55.3, N'KPH (LOD=2)', N'50'),
('KQCT0027', 'KQNM0006', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 165.8, N'KPH (LOD=3)', N'150'),
('KQCT0028', 'KQNM0006', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 12.7, N'KPH (LOD=0,01)', N'10');
-- KQ0004 - KQNM0007, KQNM0008
INSERT INTO [KetQuaChiTiet] VALUES
('KQCT0029', 'KQNM0007', 'TS0001', N'-', N'TCVN 6492:2011', 7.52, N'', N'5,5 - 9'),
('KQCT0030', 'KQNM0007', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 29.7, N'KPH (LOD=2)', N'50'),
('KQCT0031', 'KQNM0007', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 98.4, N'KPH (LOD=3)', N'150'),
('KQCT0032', 'KQNM0008', 'TS0001', N'-', N'TCVN 6492:2011', 6.92, N'', N'5,5 - 9'),
('KQCT0033', 'KQNM0008', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 51.8, N'KPH (LOD=2)', N'50'),
('KQCT0034', 'KQNM0008', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 10.45, N'KPH (LOD=0,01)', N'10');
-- KQ0005 - KQNM0009, KQNM0010, KQNM0011, KQNM0012
INSERT INTO [KetQuaChiTiet] VALUES
('KQCT0035', 'KQNM0009', 'TS0001', N'-', N'TCVN 6492:2011', 8.15, N'', N'5,5 - 9'),
('KQCT0036', 'KQNM0009', 'TS0006', N'mg/L', N'TCVN 6625:2000', 43.2, N'KPH (LOD=4)', N'100'),
('KQCT0037', 'KQNM0010', 'TS0001', N'-', N'TCVN 6492:2011', 7.48, N'', N'5,5 - 9'),
('KQCT0038', 'KQNM0010', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 118.6, N'KPH (LOD=3)', N'150'),
('KQCT0039', 'KQNM0011', 'TS0001', N'-', N'TCVN 6492:2011', 7.12, N'', N'5,5 - 9'),
('KQCT0040', 'KQNM0011', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 47.5, N'KPH (LOD=2)', N'50'),
('KQCT0041', 'KQNM0012', 'TS0001', N'-', N'TCVN 6492:2011', 6.83, N'', N'5,5 - 9'),
('KQCT0042', 'KQNM0012', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.18, N'KPH (LOD=0,01)', N'10');
-- KQ0006 - KQNM0013, KQNM0014, KQNM0015
INSERT INTO [KetQuaChiTiet] VALUES
('KQCT0043', 'KQNM0013', 'TS0001', N'-', N'TCVN 6492:2011', 7.35, N'', N'5,5 - 9'),
('KQCT0044', 'KQNM0013', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 33.6, N'KPH (LOD=2)', N'50'),
('KQCT0045', 'KQNM0013', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.72, N'KPH (LOD=0,01)', N'10'),
('KQCT0046', 'KQNM0014', 'TS0001', N'-', N'TCVN6492:2011', 7.08, N'', N'5,5 - 9'),
('KQCT0047', 'KQNM0014', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 156.3, N'KPH (LOD=3)', N'150'),
('KQCT0048', 'KQNM0015', 'TS0001', N'-', N'TCVN 6492:2011', 6.91, N'', N'5,5 - 9'),
('KQCT0049', 'KQNM0015', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.11, N'KPH (LOD=0,01)', N'2');

-- KQ0007 - KQNM0016, KQNM0017
INSERT INTO [KetQuaChiTiet] VALUES 
('KQCT0050', 'KQNM0016', 'TS0001', N'-', N'TCVN 6492:2011', 8.08, N'', N'5,5 - 9'),
('KQCT0051', 'KQNM0016', 'TS0006', N'mg/L', N'TCVN 6625:2000', 38.9, N'KPH (LOD=4)', N'100'),
('KQCT0052', 'KQNM0017', 'TS0001', N'-', N'TCVN 6492:2011', 7.25, N'', N'5,5 - 9'),
('KQCT0053', 'KQNM0017', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 26.4, N'KPH (LOD=2)', N'50');

-- KQ0008 (Sự cố tràn dầu) - 5 nền mẫu với dữ liệu bất thường
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0018 (Tâm sự cố)
('KQCT0054', 'KQNM0018', 'TS0001', N'-', N'TCVN 6492:2011', 7.23, N'', N'5,5 - 9'),
('KQCT0055', 'KQNM0018', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 78.5, N'KPH (LOD=2)', N'50'),
('KQCT0056', 'KQNM0018', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 245.3, N'KPH (LOD=3)', N'150'),
('KQCT0057', 'KQNM0018', 'TS0008', N'mg/L', N'TCVN 6637:2000', 0.85, N'KPH (LOD=0,05)', N'0,5'),

-- KQNM0019 (500m)
('KQCT0058', 'KQNM0019', 'TS0001', N'-', N'TCVN 6492:2011', 7.45, N'', N'5,5 - 9'),
('KQCT0059', 'KQNM0019', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 52.3, N'KPH (LOD=2)', N'50'),
('KQCT0060', 'KQNM0019', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 178.9, N'KPH (LOD=3)', N'150'),

-- KQNM0020 (1km)
('KQCT0061', 'KQNM0020', 'TS0001', N'-', N'TCVN 6492:2011', 7.67, N'', N'5,5 - 9'),
('KQCT0062', 'KQNM0020', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 38.7, N'KPH (LOD=2)', N'50'),
('KQCT0063', 'KQNM0020', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 135.4, N'KPH (LOD=3)', N'150'),

-- KQNM0021 (Sông ven biển)
('KQCT0064', 'KQNM0021', 'TS0001', N'-', N'TCVN 6492:2011', 7.56, N'', N'5,5 - 9'),
('KQCT0065', 'KQNM0021', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 1.23, N'KPH (LOD=0,01)', N'10'),

-- KQNM0022 (Nước thải gần khu vực)
('KQCT0066', 'KQNM0022', 'TS0001', N'-', N'TCVN 6492:2011', 6.95, N'', N'5,5 - 9'),
('KQCT0067', 'KQNM0022', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 54.7, N'KPH (LOD=2)', N'50');

-- KQ0009 - KQNM0023, KQNM0024, KQNM0025
INSERT INTO [KetQuaChiTiet] VALUES 
('KQCT0068', 'KQNM0023', 'TS0001', N'-', N'TCVN 6492:2011', 7.41, N'', N'5,5 - 9'),
('KQCT0069', 'KQNM0023', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.89, N'KPH (LOD=0,01)', N'10'),
('KQCT0070', 'KQNM0024', 'TS0001', N'-', N'TCVN 6492:2011', 7.18, N'', N'5,5 - 9'),
('KQCT0071', 'KQNM0024', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 43.7, N'KPH (LOD=2)', N'50'),
('KQCT0072', 'KQNM0025', 'TS0001', N'-', N'TCVN 6492:2011', 6.78, N'', N'5,5 - 9'),
('KQCT0073', 'KQNM0025', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.07, N'KPH (LOD=0,01)', N'2');

-- KQ0010 - KQNM0026, KQNM0027
INSERT INTO [KetQuaChiTiet] VALUES 
('KQCT0074', 'KQNM0026', 'TS0001', N'-', N'TCVN 6492:2011', 7.56, N'', N'5,5 - 9'),
('KQCT0075', 'KQNM0026', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 102.3, N'KPH (LOD=3)', N'150'),
('KQCT0076', 'KQNM0027', 'TS0001', N'-', N'TCVN 6492:2011', 6.95, N'', N'5,5 - 9'),
('KQCT0077', 'KQNM0027', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 49.2, N'KPH (LOD=2)', N'50');

-- KQ0011 - KQNM0028, KQNM0029, KQNM0030, KQNM0031 (đột xuất)
INSERT INTO [KetQuaChiTiet] VALUES 
('KQCT0078', 'KQNM0028', 'TS0001', N'-', N'TCVN 6492:2011', 7.32, N'', N'5,5 - 9'),
('KQCT0079', 'KQNM0028', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 5.67, N'KPH (LOD=0,01)', N'10'),
('KQCT0080', 'KQNM0029', 'TS0001', N'-', N'TCVN 6492:2011', 6.83, N'', N'5,5 - 9'),
('KQCT0081', 'KQNM0029', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 58.9, N'KPH (LOD=2)', N'50'),
('KQCT0082', 'KQNM0030', 'TS0001', N'-', N'TCVN 6492:2011', 7.45, N'', N'5,5 - 9'),
('KQCT0083', 'KQNM0030', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 134.5, N'KPH (LOD=3)', N'150'),
('KQCT0084', 'KQNM0031', 'TS0001', N'-', N'TCVN 6492:2011', 6.89, N'', N'5,5 - 9'),
('KQCT0085', 'KQNM0031', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.23, N'KPH (LOD=0,01)', N'10');

-- KQ0012 - KQNM0032, KQNM0033, KQNM0034
INSERT INTO [KetQuaChiTiet] VALUES 
('KQCT0086', 'KQNM0032', 'TS0001', N'-', N'TCVN 6492:2011', 7.38, N'', N'5,5 - 9'),
('KQCT0087', 'KQNM0032', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 34.5, N'KPH (LOD=2)', N'50'),
('KQCT0088', 'KQNM0033', 'TS0001', N'-', N'TCVN 6492:2011', 7.02, N'', N'5,5 - 9'),
('KQCT0089', 'KQNM0033', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 7.89, N'KPH (LOD=0,01)', N'10'),
('KQCT0090', 'KQNM0034', 'TS0001', N'-', N'TCVN 6492:2011', 8.11, N'', N'5,5 - 9'),
('KQCT0091', 'KQNM0034', 'TS0009', N'mg/L', N'TCVN 6494-1:2011', 18900, N'', N'Không quy định');

-- KQ0013 (Mở rộng khu vực) - 6 nền mẫu
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0035
('KQCT0092', 'KQNM0035', 'TS0001', N'-', N'TCVN 6492:2011', 7.34, N'', N'5,5 - 9'),
('KQCT0093', 'KQNM0035', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 1.25, N'KPH (LOD=0,01)', N'10'),
('KQCT0094', 'KQNM0035', 'TS0005', N'mg/L', N'TCVN 6638:2000', 12.5, N'', N'40'),

-- KQNM0036
('KQCT0095', 'KQNM0036', 'TS0001', N'-', N'TCVN 6492:2011', 7.56, N'', N'5,5 - 9'),
('KQCT0096', 'KQNM0036', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.98, N'KPH (LOD=0,01)', N'10'),
('KQCT0097', 'KQNM0036', 'TS0006', N'mg/L', N'TCVN 6625:2000', 67.3, N'KPH (LOD=4)', N'100'),

-- KQNM0037
('KQCT0098', 'KQNM0037', 'TS0001', N'-', N'TCVN 6492:2011', 7.12, N'', N'5,5 - 9'),
('KQCT0099', 'KQNM0037', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 31.8, N'KPH (LOD=2)', N'50'),
('KQCT0100', 'KQNM0037', 'TS0007', N'mg/L', N'TCVN 6202:2008', 2.34, N'', N'6'),

-- KQNM0038
('KQCT0101', 'KQNM0038', 'TS0001', N'-', N'TCVN 6492:2011', 6.78, N'', N'5,5 - 9'),
('KQCT0102', 'KQNM0038', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 48.9, N'KPH (LOD=2)', N'50'),
('KQCT0103', 'KQNM0038', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 142.6, N'KPH (LOD=3)', N'150'),

-- KQNM0039
('KQCT0104', 'KQNM0039', 'TS0001', N'-', N'TCVN 6492:2011', 7.89, N'', N'5,5 - 9'),
('KQCT0105', 'KQNM0039', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 9.45, N'KPH (LOD=0,01)', N'10'),
('KQCT0106', 'KQNM0039', 'TS0008', N'mg/L', N'TCVN 6637:2000', 0.38, N'KPH (LOD=0,05)', N'0,5'),

-- KQNM0040 (Nước ngầm)
('KQCT0107', 'KQNM0040', 'TS0001', N'-', N'TCVN 6492:2011', 6.95, N'', N'5,5 - 9'),
('KQCT0108', 'KQNM0040', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.15, N'KPH (LOD=0,01)', N'10'),
('KQCT0109', 'KQNM0040', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.08, N'KPH (LOD=0,01)', N'2'),
('KQCT0110', 'KQNM0040', 'TS0013', N'mg/L', N'TCVN 6193:1996', 0.03, N'KPH (LOD=0,005)', N'0,1');

-- KQ0014, KQ0015, KQ0016 (Quý 3)
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0041, KQNM0042
('KQCT0111', 'KQNM0041', 'TS0001', N'-', N'TCVN 6492:2011', 8.18, N'', N'5,5 - 9'),
('KQCT0112', 'KQNM0041', 'TS0006', N'mg/L', N'TCVN 6625:2000', 41.2, N'KPH (LOD=4)', N'100'),
('KQCT0113', 'KQNM0042', 'TS0001', N'-', N'TCVN 6492:2011', 7.42, N'', N'5,5 - 9'),
('KQCT0114', 'KQNM0042', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 112.8, N'KPH (LOD=3)', N'150'),

-- KQNM0043, KQNM0044, KQNM0045
('KQCT0115', 'KQNM0043', 'TS0001', N'-', N'TCVN 6492:2011', 8.09, N'', N'5,5 - 9'),
('KQCT0116', 'KQNM0043', 'TS0006', N'mg/L', N'TCVN 6625:2000', 39.7, N'KPH (LOD=4)', N'100'),
('KQCT0117', 'KQNM0044', 'TS0001', N'-', N'TCVN 6492:2011', 7.51, N'', N'5,5 - 9'),
('KQCT0118', 'KQNM0044', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.94, N'KPH (LOD=0,01)', N'10'),
('KQCT0119', 'KQNM0045', 'TS0001', N'-', N'TCVN 6492:2011', 7.13, N'', N'5,5 - 9'),
('KQCT0120', 'KQNM0045', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 45.8, N'KPH (LOD=2)', N'50'),

-- KQNM0046, KQNM0047, KQNM0048
('KQCT0121', 'KQNM0046', 'TS0001', N'-', N'TCVN 6492:2011', 7.48, N'', N'5,5 - 9'),
('KQCT0122', 'KQNM0046', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 106.4, N'KPH (LOD=3)', N'150'),
('KQCT0123', 'KQNM0047', 'TS0001', N'-', N'TCVN 6492:2011', 6.97, N'', N'5,5 - 9'),
('KQCT0124', 'KQNM0047', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 50.3, N'KPH (LOD=2)', N'50'),
('KQCT0125', 'KQNM0048', 'TS0001', N'-', N'TCVN 6492:2011', 6.82, N'', N'5,5 - 9'),
('KQCT0126', 'KQNM0048', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.13, N'KPH (LOD=0,01)', N'2');

-- KQ0017 (Tuần 2 tháng 11) - 4 nền mẫu
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0049 (Sông)
('KQCT0127', 'KQNM0049', 'TS0001', N'-', N'TCVN 6492:2011', 7.28, N'', N'5,5 - 9'),
('KQCT0128', 'KQNM0049', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 32.4, N'KPH (LOD=2)', N'50'),
('KQCT0129', 'KQNM0049', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 108.7, N'KPH (LOD=3)', N'150'),
('KQCT0130', 'KQNM0049', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.67, N'KPH (LOD=0,01)', N'10'),
('KQCT0131', 'KQNM0049', 'TS0005', N'mg/L', N'TCVN 6638:2000', 6.8, N'', N'40'),

-- KQNM0050 (Nhà máy)
('KQCT0132', 'KQNM0050', 'TS0001', N'-', N'TCVN 6492:2011', 7.15, N'', N'5,5 - 9'),
('KQCT0133', 'KQNM0050', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 44.2, N'KPH (LOD=2)', N'50'),
('KQCT0134', 'KQNM0050', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 138.5, N'KPH (LOD=3)', N'150'),
('KQCT0135', 'KQNM0050', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 7.89, N'KPH (LOD=0,01)', N'10'),

-- KQNM0051 (Nước ngầm)
('KQCT0136', 'KQNM0051', 'TS0001', N'-', N'TCVN 6492:2011', 6.78, N'', N'5,5 - 9'),
('KQCT0137', 'KQNM0051', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.08, N'KPH (LOD=0,01)', N'10'),
('KQCT0138', 'KQNM0051', 'TS0009', N'mg/L', N'TCVN 6494-1:2011', 245.7, N'', N'Không quy định'),
('KQCT0139', 'KQNM0051', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.05, N'KPH (LOD=0,01)', N'2'),

-- KQNM0052 (Biển)
('KQCT0140', 'KQNM0052', 'TS0001', N'-', N'TCVN 6492:2011', 8.05, N'', N'5,5 - 9'),
('KQCT0141', 'KQNM0052', 'TS0006', N'mg/L', N'TCVN 6625:2000', 52.3, N'KPH (LOD=4)', N'100'),
('KQCT0142', 'KQNM0052', 'TS0009', N'mg/L', N'TCVN 6494-1:2011', 19200, N'', N'Không quy định');

-- KQ0018, KQ0019 (Nước mưa)
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0053, KQNM0054
('KQCT0143', 'KQNM0053', 'TS0001', N'-', N'TCVN 6492:2011', 7.29, N'', N'5,5 - 9'),
('KQCT0144', 'KQNM0053', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 27.8, N'KPH (LOD=2)', N'50'),
('KQCT0145', 'KQNM0053', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.76, N'KPH (LOD=0,01)', N'10'),
('KQCT0146', 'KQNM0054', 'TS0001', N'-', N'TCVN 6492:2011', 6.61, N'', N'5,5 - 9'),
('KQCT0147', 'KQNM0054', 'TS0006', N'mg/L', N'TCVN 6625:2000', 14.2, N'KPH (LOD=4)', N'100'),

-- KQNM0055, KQNM0056, KQNM0057 (Nước mưa)
('KQCT0148', 'KQNM0055', 'TS0001', N'-', N'TCVN 6492:2011', 6.45, N'', N'5,5 - 9'),
('KQCT0149', 'KQNM0055', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 1.23, N'KPH (LOD=0,01)', N'10'),
('KQCT0150', 'KQNM0055', 'TS0006', N'mg/L', N'TCVN 6625:2000', 15.8, N'KPH (LOD=4)', N'100'),
('KQCT0151', 'KQNM0055', 'TS0007', N'mg/L', N'TCVN 6202:2008', 0.45, N'', N'6'),

('KQCT0152', 'KQNM0056', 'TS0001', N'-', N'TCVN 6492:2011', 6.67, N'', N'5,5 - 9'),
('KQCT0153', 'KQNM0056', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.98, N'KPH (LOD=0,01)', N'10'),
('KQCT0154', 'KQNM0056', 'TS0006', N'mg/L', N'TCVN 6625:2000', 18.2, N'KPH (LOD=4)', N'100'),

('KQCT0155', 'KQNM0057', 'TS0001', N'-', N'TCVN 6492:2011', 6.52, N'', N'5,5 - 9'),
('KQCT0156', 'KQNM0057', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 1.45, N'KPH (LOD=0,01)', N'10'),
('KQCT0157', 'KQNM0057', 'TS0005', N'mg/L', N'TCVN 6638:2000', 3.2, N'<5', N'40');

-- KQ0020 - KQNM0058, KQNM0059, KQNM0060
INSERT INTO [KetQuaChiTiet] VALUES 
('KQCT0158', 'KQNM0058', 'TS0001', N'-', N'TCVN 6492:2011', 7.36, N'', N'5,5 - 9'),
('KQCT0159', 'KQNM0058', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 35.6, N'KPH (LOD=2)', N'50'),
('KQCT0160', 'KQNM0058', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 119.3, N'KPH (LOD=3)', N'150'),
('KQCT0161', 'KQNM0059', 'TS0001', N'-', N'TCVN 6492:2011', 7.11, N'', N'5,5- 9'),
('KQCT0162', 'KQNM0059', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 44.9, N'KPH (LOD=2)', N'50'),
('KQCT0163', 'KQNM0059', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 8.12, N'KPH (LOD=0,01)', N'10'),
('KQCT0164', 'KQNM0060', 'TS0001', N'-', N'TCVN 6492:2011', 6.85, N'', N'5,5 - 9'),
('KQCT0165', 'KQNM0060', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.16, N'KPH (LOD=0,01)', N'10'),
('KQCT0166', 'KQNM0060', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.09, N'KPH (LOD=0,01)', N'2');

-- KQ0021 (Kim loại nặng) - 4 nền mẫu
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0061 (Sông)
('KQCT0167', 'KQNM0061', 'TS0001', N'-', N'TCVN 6492:2011', 7.45, N'', N'5,5 - 9'),
('KQCT0168', 'KQNM0061', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.15, N'KPH (LOD=0,01)', N'2'),
('KQCT0169', 'KQNM0061', 'TS0012', N'mg/L', N'TCVN 6193:1996', 0.45, N'KPH (LOD=0,01)', N'3'),
('KQCT0170', 'KQNM0061', 'TS0013', N'mg/L', N'TCVN 6193:1996', 0.025, N'KPH (LOD=0,005)', N'0,1'),
('KQCT0171', 'KQNM0061', 'TS0014', N'mg/L', N'TCVN 6193:1996', 0.003, N'KPH (LOD=0,001)', N'0,01'),
('KQCT0172', 'KQNM0061', 'TS0015', N'mg/L', N'TCVN 6193:1996', 0.0008, N'KPH (LOD=0,0001)', N'0,005'),

-- KQNM0062 (Nước thải)
('KQCT0173', 'KQNM0062', 'TS0001', N'-', N'TCVN 6492:2011', 6.89, N'', N'5,5 - 9'),
('KQCT0174', 'KQNM0062', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.85, N'KPH (LOD=0,01)', N'2'),
('KQCT0175', 'KQNM0062', 'TS0012', N'mg/L', N'TCVN 6193:1996', 1.23, N'KPH (LOD=0,01)', N'3'),
('KQCT0176', 'KQNM0062', 'TS0013', N'mg/L', N'TCVN 6193:1996', 0.078, N'KPH (LOD=0,005)', N'0,1'),
('KQCT0177', 'KQNM0062', 'TS0014', N'mg/L', N'TCVN 6193:1996', 0.007, N'KPH (LOD=0,001)', N'0,01'),

-- KQNM0063 (Nước ngầm)
('KQCT0178', 'KQNM0063', 'TS0001', N'-', N'TCVN 6492:2011', 6.95, N'', N'5,5 - 9'),
('KQCT0179', 'KQNM0063', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.12, N'KPH (LOD=0,01)', N'2'),
('KQCT0180', 'KQNM0063', 'TS0012', N'mg/L', N'TCVN 6193:1996', 0.28, N'KPH (LOD=0,01)', N'3'),
('KQCT0181', 'KQNM0063', 'TS0013', N'mg/L', N'TCVN 6193:1996', 0.015, N'KPH (LOD=0,005)', N'0,1'),

-- KQNM0064 (Biển)
('KQCT0182', 'KQNM0064', 'TS0001', N'-', N'TCVN 6492:2011', 8.12, N'', N'5,5 - 9'),
('KQCT0183', 'KQNM0064', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.08, N'KPH (LOD=0,01)', N'2'),
('KQCT0184', 'KQNM0064', 'TS0012', N'mg/L', N'TCVN 6193:1996', 0.18, N'KPH (LOD=0,01)', N'3'),
('KQCT0185', 'KQNM0064', 'TS0013', N'mg/L', N'TCVN 6193:1996', 0.012, N'KPH (LOD=0,005)', N'0,1');

-- KQ0022 (Toàn diện) - 5 nền mẫu với nhiều thông số
INSERT INTO [KetQuaChiTiet] VALUES 
-- KQNM0065 (Biển)
('KQCT0186', 'KQNM0065', 'TS0001', N'-', N'TCVN 6492:2011', 8.23, N'', N'5,5 - 9'),
('KQCT0187', 'KQNM0065', 'TS0006', N'mg/L', N'TCVN 6625:2000', 48.7, N'KPH (LOD=4)', N'100'),
('KQCT0188', 'KQNM0065', 'TS0009', N'mg/L', N'TCVN 6494-1:2011', 18750, N'', N'Không quy định'),
('KQCT0189', 'KQNM0065', 'TS0010', N'mg/L', N'TCVN 6494-1:2011', 0.75, N'KPH (LOD=0,05)', N'1,5'),
('KQCT0190', 'KQNM0065', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.09, N'KPH (LOD=0,01)', N'2'),

-- KQNM0066 (Sông Hồng)
('KQCT0191', 'KQNM0066', 'TS0001', N'-', N'TCVN 6492:2011', 7.38, N'', N'5,5 - 9'),
('KQCT0192', 'KQNM0066', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 36.8, N'KPH (LOD=2)', N'50'),
('KQCT0193', 'KQNM0066', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 125.4, N'KPH (LOD=3)', N'150'),
('KQCT0194', 'KQNM0066', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.56, N'KPH (LOD=0,01)', N'10'),
('KQCT0195', 'KQNM0066', 'TS0005', N'mg/L', N'TCVN 6638:2000', 7.3, N'', N'40'),
('KQCT0196', 'KQNM0066', 'TS0006', N'mg/L', N'TCVN 6625:2000', 72.5, N'KPH (LOD=4)', N'100'),
('KQCT0197', 'KQNM0066', 'TS0007', N'mg/L', N'TCVN 6202:2008', 1.89, N'', N'6'),

-- KQNM0067 (Nhà máy HCAP)
('KQCT0198', 'KQNM0067', 'TS0001', N'-', N'TCVN 6492:2011', 7.05, N'', N'5,5 - 9'),
('KQCT0199', 'KQNM0067', 'TS0002', N'mg/L', N'TCVN 6001-1:2008', 46.3, N'KPH (LOD=2)', N'50'),
('KQCT0200', 'KQNM0067', 'TS0003', N'mg/L', N'SMEWW 5220C:2017', 143.7, N'KPH (LOD=3)', N'150'),
('KQCT0201', 'KQNM0067', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 8.25, N'KPH (LOD=0,01)', N'10'),
('KQCT0202', 'KQNM0067', 'TS0008', N'mg/L', N'TCVN 6637:2000', 0.42, N'KPH (LOD=0,05)', N'0,5'),
('KQCT0203', 'KQNM0067', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.67, N'KPH (LOD=0,01)', N'2'),

-- KQNM0068 (Nước ngầm)
('KQCT0204', 'KQNM0068', 'TS0001', N'-', N'TCVN 6492:2011', 6.87, N'', N'5,5 - 9'),
('KQCT0205', 'KQNM0068', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 0.12, N'KPH (LOD=0,01)', N'10'),
('KQCT0206', 'KQNM0068', 'TS0009', N'mg/L', N'TCVN 6494-1:2011', 278.3, N'', N'Không quy định'),
('KQCT0207', 'KQNM0068', 'TS0011', N'mg/L', N'TCVN 6193:1996', 0.06, N'KPH (LOD=0,01)', N'2'),
('KQCT0208', 'KQNM0068', 'TS0012', N'mg/L', N'TCVN 6193:1996', 0.23, N'KPH (LOD=0,01)', N'3'),
('KQCT0209', 'KQNM0068', 'TS0013', N'mg/L', N'TCVN 6193:1996', 0.008, N'KPH (LOD=0,005)', N'0,1'),

-- KQNM0069 (Nước mưa)
('KQCT0210', 'KQNM0069', 'TS0001', N'-', N'TCVN 6492:2011', 6.58, N'', N'5,5 - 9'),
('KQCT0211', 'KQNM0069', 'TS0004', N'mg/L', N'TCVN 6179-1:1996', 1.15, N'KPH (LOD=0,01)', N'10'),
('KQCT0212', 'KQNM0069', 'TS0005', N'mg/L', N'TCVN 6638:2000', 2.8, N'<5', N'40'),
('KQCT0213', 'KQNM0069', 'TS0006', N'mg/L', N'TCVN 6625:2000', 12.3, N'KPH (LOD=4)', N'100'),
('KQCT0214', 'KQNM0069', 'TS0007', N'mg/L', N'TCVN 6202:2008', 0.38, N'', N'6');
GO

-----THÁI SỬA 11/11
CREATE TABLE dbo.ThongBao (
    maTB        VARCHAR(15)     NOT NULL,       -- Mã thông báo
    loaiTB      NVARCHAR(50)    NOT NULL,       -- Loại thông báo (VD: QUA_HAN_DOT, QUA_HAN_HD)
    maDot       VARCHAR(15)     NULL,           -- FK -> DotQuanTrac
    maHD        VARCHAR(15)     NULL,           -- FK -> HopDong
    tieuDe      NVARCHAR(255)   NOT NULL,       -- Tiêu đề thông báo
    noiDung     NVARCHAR(MAX)   NULL,           -- Nội dung chi tiết
    ngayTao     DATETIME        NOT NULL DEFAULT GETDATE(),  -- Ngày tạo thông báo
    daGuiEmail  BIT             NOT NULL DEFAULT(0),         -- Đã gửi email hay chưa (0 = chưa, 1 = đã gửi)

    CONSTRAINT PK_ThongBao PRIMARY KEY (maTB),

    -- Ràng buộc FK nhưng KHÔNG CASCADE để tránh lỗi multiple path
    CONSTRAINT FK_ThongBao_DotQuanTrac
        FOREIGN KEY (maDot) REFERENCES dbo.DotQuanTrac(maDot)
        ON DELETE SET NULL
        ON UPDATE NO ACTION,

    CONSTRAINT FK_ThongBao_HopDong
        FOREIGN KEY (maHD) REFERENCES dbo.HopDong(maHD)
        ON DELETE SET NULL
        ON UPDATE NO ACTION
);
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_LayChiTietDotQuanTrac] --Mục đích là sửa đợt quan trắc 
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Bảng 1: Thông tin đợt quan trắc
    SELECT 
        dt.maDot,
        dt.maHD,
        dt.noiDung,
        dt.dotQuanTrac,
        dt.ngayBatDau,
        dt.ngayDuKien,
        dt.ngayTraKQ,
        dt.trangThai
    FROM DotQuanTrac dt
    WHERE dt.maDot = @maDot;
    
    -- Bảng 2: Danh sách nền mẫu của đợt
    SELECT 
        dn.maDN,
        dn.maNen,
        dn.tenViTri,
        dn.toaDo,
        dn.ghiChu,
        nm.tenNenMau,
        nm.moTa
    FROM Dot_Nen dn
    INNER JOIN NenMau nm ON dn.maNen = nm.maNen
    WHERE dn.maDot = @maDot;
    
    -- Bảng 3: Chi tiết thông số của từng nền mẫu
    SELECT 
        dnts.maDNTS,
        dnts.maDN,
        dnts.maTS,
        dnts.tenTS,
        dnts.donVi,
        dnts.giaTriToiThieu,
        dnts.giaTriToiDa,
        dnts.phuongPhap,
        dnts.maPhong,
        pb.tenPhong
    FROM Dot_Nen_Ts dnts
    INNER JOIN Dot_Nen dn ON dnts.maDN = dn.maDN
    LEFT JOIN PhongBan pb ON dnts.maPhong = pb.maPhong
    WHERE dn.maDot = @maDot
    ORDER BY dnts.maDN, dnts.maDNTS;
END
GO


------- Kiểm tra quá hạn đợt quan trắc và insert vào bảng thông báo 
CREATE OR ALTER PROCEDURE sp_KiemTraQuaHanDotQuanTrac
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);
    DECLARE @NewMaTB VARCHAR(15);

    DECLARE cur CURSOR FOR
    SELECT dq.maDot, dq.maHD, kh.tenDoanhNghiep, dq.ngayDuKien
    FROM DotQuanTrac dq
    JOIN HopDong hd ON dq.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE dq.trangThai <> 6
      AND dq.ngayDuKien < @NgayHienTai
      AND EXISTS (
            SELECT 1
            FROM Dot_Nen_TS dnts
            JOIN Dot_Nen dn ON dnts.maDN = dn.maDN
            WHERE dn.maDot = dq.maDot
              AND NOT EXISTS (
                    SELECT 1 FROM KetQua kq WHERE kq.maDNTS = dnts.maDNTS
              )
        )
      AND NOT EXISTS (
            SELECT 1 FROM ThongBao tb 
            WHERE tb.maDot = dq.maDot AND tb.loaiTB = 'QUA_HAN_DOT'
      );

    DECLARE @maDot VARCHAR(15), @maHD VARCHAR(15), @tenKH NVARCHAR(255), @ngayDuKien DATE, @soNgayTre INT;

    OPEN cur;
    FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @soNgayTre = DATEDIFF(DAY, @ngayDuKien, @NgayHienTai);

        SELECT @NewMaTB = 'TB' + RIGHT('000000' + CAST(ISNULL(CAST(SUBSTRING(MAX(maTB), 3, 6) AS INT), 0) + 1 AS VARCHAR(6)), 6)
        FROM ThongBao;

        INSERT INTO ThongBao(maTB, loaiTB, maDot, maHD, tieuDe, noiDung, ngayTao)
        VALUES (
            @NewMaTB,
            'QUA_HAN_DOT',
            @maDot,
            @maHD,
            N'Đợt quan trắc ' + @maDot + N' đã quá hạn trả kết quả',
            N'Khách hàng: ' + @tenKH + 
            N'. Ngày dự kiến: ' + CONVERT(VARCHAR(10), @ngayDuKien, 103) +
            N'. Ngày hiện tại: ' + CONVERT(VARCHAR(10), @NgayHienTai, 103) +
            N'. Số ngày trễ: ' + CAST(@soNgayTre AS NVARCHAR(10)),
            GETDATE()
        );

        FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO

CREATE OR ALTER PROCEDURE sp_layDanhSachThongBaoQuaHan
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);

    SELECT 
        tb.maTB,
        tb.maDot,
        tb.maHD,
        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao,
        tb.loaiTB,
        kh.tenDoanhNghiep AS tenKhachHang,
        dq.ngayDuKien
    FROM ThongBao tb
    INNER JOIN DotQuanTrac dq ON tb.maDot = dq.maDot
    INNER JOIN HopDong hd ON dq.maHD = hd.maHD
    INNER JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE tb.loaiTB = 'QUA_HAN_DOT'
    ORDER BY tb.ngayTao DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_LayThongBaoTheoDot --phục vụ gửi mail 
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);

    SELECT 
        tb.maTB,
        tb.maDot,
        tb.maHD,
        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao,
        tb.loaiTB,
        kh.tenDoanhNghiep AS tenKhachHang,
        dq.ngayDuKien
    FROM ThongBao tb
    INNER JOIN DotQuanTrac dq ON tb.maDot = dq.maDot
    INNER JOIN HopDong hd ON dq.maHD = hd.maHD
    INNER JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE tb.loaiTB = 'QUA_HAN_DOT'
      AND ISNULL(tb.daGuiEmail, 0) = 0  
      AND tb.maDot = @maDot      -- ✅ thêm điều kiện này
    ORDER BY tb.ngayTao DESC;
END;
GO

CREATE PROCEDURE sp_LayEmailTruongPhong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        pb.maPhong,
        pb.tenPhong,
        nv.hoTen AS tenTruongPhong,
        nv.email
    FROM PhongBan pb
    INNER JOIN NhanVien nv ON pb.truongPhong = nv.maNV
    WHERE nv.email IS NOT NULL AND nv.email <> '';
END;
GO

go
CREATE PROCEDURE sp_CapNhatTrangThaiEmail
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ThongBao
    SET daGuiEmail = 1
    WHERE maDot = @maDot
      AND loaiTB = 'QUA_HAN_DOT'; -- chỉ cập nhật cho loại cảnh báo quá hạn
END;
GO

CREATE OR ALTER PROCEDURE sp_CapNhatThongTinCaNhan
    @MaNV        VARCHAR(15),
    @HoTen       NVARCHAR(60),
    @NgaySinh    DATE           = NULL,
    @GioiTinh    BIT            = NULL,    -- 1: Nam, 0: Nữ
    @DiaChi      NVARCHAR(150)  = NULL,
    @SoDienThoai VARCHAR(20),
    @Email       VARCHAR(50)    = NULL,
    @AnhDaiDien  NVARCHAR(255)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- 1️⃣ Kiểm tra nhân viên tồn tại
        IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE maNV = @MaNV AND daXoa = 0)
        BEGIN
            RAISERROR(N'Không tìm thấy nhân viên cần cập nhật!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 2️⃣ Kiểm tra họ tên hợp lệ
        IF LTRIM(RTRIM(@HoTen)) = ''
        BEGIN
            RAISERROR(N'Họ tên không được để trống!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        IF PATINDEX('%[^a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ ]%', @HoTen) > 0
        BEGIN
            RAISERROR(N'Họ tên không hợp lệ! Chỉ được chứa chữ cái và khoảng trắng.', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 3️⃣ Kiểm tra tuổi hợp lệ (16–65)
        IF @NgaySinh IS NOT NULL
        BEGIN
            DECLARE @Tuoi INT = DATEDIFF(YEAR, @NgaySinh, GETDATE());
            IF (DATEADD(YEAR, @Tuoi, @NgaySinh) > GETDATE()) SET @Tuoi -= 1;
            IF @Tuoi < 16 OR @Tuoi > 65
            BEGIN
                RAISERROR(N'Tuổi không hợp lệ! Nhân viên phải từ 16 đến 65 tuổi.', 16, 1);
                ROLLBACK TRAN; RETURN;
            END;
        END;

        -- 4️⃣ Kiểm tra số điện thoại hợp lệ (10 chữ số, bắt đầu bằng 0)
        DECLARE @soDienThoaiTrimmed VARCHAR(20) = LTRIM(RTRIM(@SoDienThoai));
        IF LEN(@soDienThoaiTrimmed) <> 10 
           OR LEFT(@soDienThoaiTrimmed, 1) <> '0' 
           OR PATINDEX('%[^0-9]%', @soDienThoaiTrimmed) > 0
        BEGIN
            RAISERROR(N'Số điện thoại không hợp lệ! Phải bắt đầu bằng 0, đúng 10 chữ số, không chứa ký tự khác.', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 5️⃣ Kiểm tra email hợp lệ
        IF @Email IS NULL OR LTRIM(RTRIM(@Email)) = ''
        BEGIN
            RAISERROR(N'Email không được để trống!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        IF @Email NOT LIKE '%_@_%._%' 
           OR @Email LIKE '%..%' 
           OR @Email LIKE '%.@%' 
           OR RIGHT(@Email, 4) NOT IN ('.com', '.net', '.org', '.edu', '.gov', '.vn')
        BEGIN
            RAISERROR(N'Email không hợp lệ! Vui lòng nhập đúng định dạng (vd: abc@gmail.com).', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 6️⃣ Kiểm tra trùng email
        IF EXISTS (SELECT 1 FROM NhanVien WHERE email = @Email AND maNV <> @MaNV AND daXoa = 0)
        BEGIN
            RAISERROR(N'Email này đã tồn tại cho nhân viên khác!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 7️⃣ Cập nhật thông tin cá nhân
        UPDATE NhanVien
        SET hoTen       = @HoTen,
            ngaySinh    = @NgaySinh,
            gioiTinh    = @GioiTinh,
            diaChi      = @DiaChi,
            soDienThoai = @SoDienThoai,
            email       = @Email,
            anhDaiDien  = @AnhDaiDien
        WHERE maNV = @MaNV AND daXoa = 0;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[layDotQuanTracNhapLieu_PhanTrang]
    @pageNumber INT,
    @pageSize   INT,
    @maPhong    VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Dot AS (
        SELECT
            d.maDot,
            d.maHD,
            d.ngayBatDau,
            d.ngayDuKien,
            ngayConLai = DATEDIFF(DAY, CAST(GETDATE() AS date), CAST(d.ngayDuKien AS date)),

            -- Hoàn thành nếu KHÔNG còn bất kỳ TS nào (thuộc phòng này) chưa có giá trị đo
            hoanThanh = CASE
                WHEN NOT EXISTS (
                    SELECT 1
                    FROM Dot_Nen dn
                    JOIN Dot_Nen_TS ts ON ts.maDN = dn.maDN
                    LEFT JOIN KetQua k  ON k.maDNTS = ts.maDNTS
                    WHERE dn.maDot = d.maDot
                      AND ts.maPhong = @maPhong
                      AND (k.maDNTS IS NULL OR k.giaTriDoDuoc IS NULL)
                )
                THEN 1 ELSE 0
            END
        FROM DotQuanTrac d
        -- 🔒 Chỉ các đợt thuộc 1 trong các trạng thái cho phép nhập liệu
        WHERE d.trangThai IN (1, 2, 4)

        -- 🔒 Chỉ những đợt có ÍT NHẤT 1 công việc giao cho phòng @maPhong
          AND EXISTS (
                SELECT 1
                FROM Dot_Nen dn
                JOIN Dot_Nen_TS ts ON ts.maDN = dn.maDN
                WHERE dn.maDot = d.maDot
                  AND ts.maPhong = @maPhong
          )
    ),
    Base AS (
        SELECT
            maDot, maHD, ngayBatDau, ngayDuKien, ngayConLai,
            trangThai =
                CASE
                    WHEN hoanThanh = 1              THEN N'Hoàn thành'
                    WHEN ngayConLai < 0             THEN N'Hết hạn'
                    WHEN ngayConLai BETWEEN 0 AND 7 THEN N'Gần hết hạn'
                    ELSE                               N'Còn hạn'
                END
        FROM Dot
    )
    -- Phân trang
    SELECT  maDot, maHD, ngayBatDau, ngayDuKien, ngayConLai, trangThai
    FROM    Base
    ORDER BY ngayConLai DESC, maDot
    OFFSET (@pageNumber - 1) * @pageSize ROWS
    FETCH NEXT @pageSize ROWS ONLY;

    -- Trả thêm tổng bản ghi cho UI
    SELECT TotalRecords = COUNT(*) FROM Base;
END
GO

CREATE OR ALTER PROCEDURE [dbo].[sp_LayChiTietDotQuanTrac] 
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Bảng 1: Thông tin đợt quan trắc
    SELECT 
        dt.maDot,
        dt.maHD,
        dt.noiDung,
        dt.dotQuanTrac,
        dt.ngayBatDau,
        dt.ngayDuKien,
        dt.ngayTraKQ,
        dt.trangThai
    FROM DotQuanTrac dt
    WHERE dt.maDot = @maDot;
    
    -- Bảng 2: Danh sách nền mẫu của đợt
    SELECT 
        dn.maDN,
        dn.maNen,
        dn.tenViTri,
        dn.toaDo,
        dn.ghiChu,
        nm.tenNenMau,
        nm.moTa
    FROM Dot_Nen dn
    INNER JOIN NenMau nm ON dn.maNen = nm.maNen
    WHERE dn.maDot = @maDot;
    
    -- Bảng 3: Chi tiết thông số của từng nền mẫu
    SELECT 
        dnts.maDNTS,
        dnts.maDN,
        dnts.maTS,
        dnts.tenTS,
        dnts.donVi,
        dnts.giaTriToiThieu,
        dnts.giaTriToiDa,
        dnts.phuongPhap,
        dnts.maPhong,
        pb.tenPhong
    FROM Dot_Nen_Ts dnts
    INNER JOIN Dot_Nen dn ON dnts.maDN = dn.maDN
    LEFT JOIN PhongBan pb ON dnts.maPhong = pb.maPhong
    WHERE dn.maDot = @maDot
    ORDER BY dnts.maDN, dnts.maDNTS;
END
GO


------- Kiểm tra quá hạn đợt quan trắc và insert vào bảng thông báo 
CREATE OR ALTER PROCEDURE sp_KiemTraQuaHanDotQuanTrac
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);
    DECLARE @NewMaTB VARCHAR(15);
    DECLARE @maDot VARCHAR(15), @maHD VARCHAR(15), @tenKH NVARCHAR(255), @ngayDuKien DATE, @soNgayTre INT;

    DECLARE cur CURSOR FOR
    SELECT dq.maDot, dq.maHD, kh.tenDoanhNghiep, dq.ngayDuKien
    FROM DotQuanTrac dq
    JOIN HopDong hd ON dq.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE dq.trangThai <> 6
      AND dq.ngayDuKien < @NgayHienTai
      AND EXISTS (
            SELECT 1
            FROM Dot_Nen_TS dnts
            JOIN Dot_Nen dn ON dnts.maDN = dn.maDN
            WHERE dn.maDot = dq.maDot
              AND NOT EXISTS (SELECT 1 FROM KetQua kq WHERE kq.maDNTS = dnts.maDNTS)
        )
      AND NOT EXISTS (
            SELECT 1 FROM ThongBao tb 
            WHERE tb.maDot = dq.maDot AND tb.loaiTB = 'QUA_HAN_DOT'
      );

    OPEN cur;
    FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @soNgayTre = DATEDIFF(DAY, @ngayDuKien, @NgayHienTai);

        SELECT @NewMaTB = 
            'TB' + RIGHT('000000' + CAST(ISNULL(CAST(SUBSTRING(MAX(maTB), 3, 6) AS INT), 0) + 1 AS VARCHAR(6)), 6)
        FROM ThongBao;

        INSERT INTO ThongBao(maTB, loaiTB, maDot, maHD, tieuDe, noiDung, ngayTao)
        VALUES (
            @NewMaTB, 'QUA_HAN_DOT', @maDot, @maHD,
            N'Đợt quan trắc ' + @maDot + N' đã quá hạn trả kết quả',
            N'Khách hàng: ' + @tenKH +
            N'. Ngày dự kiến: ' + CONVERT(VARCHAR(10), @ngayDuKien, 103) +
            N'. Ngày hiện tại: ' + CONVERT(VARCHAR(10), @NgayHienTai, 103) +
            N'. Số ngày trễ: ' + CAST(@soNgayTre AS NVARCHAR(10)),
            GETDATE()
        );

        -- 🔹 Bổ sung fan-out vào ThongBao_NguoiDung
        INSERT INTO dbo.ThongBao_NguoiDung (maTB, maNV, trangThaiDoc, ngayDoc)
        SELECT @NewMaTB, nv.maNV, 0, NULL
        FROM dbo.NhanVien nv
        WHERE nv.trangThai = 1; -- tùy logic hệ thống

        FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO

CREATE OR ALTER PROCEDURE sp_layDanhSachThongBaoQuaHan
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);

    SELECT 
        tb.maTB,
        tb.maDot,
        tb.maHD,
        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao,
        tb.loaiTB,
        kh.tenDoanhNghiep AS tenKhachHang,
        dq.ngayDuKien
    FROM ThongBao tb
    INNER JOIN DotQuanTrac dq ON tb.maDot = dq.maDot
    INNER JOIN HopDong hd ON dq.maHD = hd.maHD
    INNER JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE tb.loaiTB = 'QUA_HAN_DOT'
	 AND ISNULL(tb.daGuiEmail, 0) = 0 
    ORDER BY tb.ngayTao DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_LayThongBaoTheoDot 
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);

    SELECT 
        tb.maTB,
        tb.maDot,
        tb.maHD,
        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao,
        tb.loaiTB,
        kh.tenDoanhNghiep AS tenKhachHang,
        dq.ngayDuKien
    FROM ThongBao tb
    INNER JOIN DotQuanTrac dq ON tb.maDot = dq.maDot
    INNER JOIN HopDong hd ON dq.maHD = hd.maHD
    INNER JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE tb.loaiTB = 'QUA_HAN_DOT'
      AND ISNULL(tb.daGuiEmail, 0) = 0  
      AND tb.maDot = @maDot     
    ORDER BY tb.ngayTao DESC;
END;
GO

CREATE OR ALTER PROCEDURE sp_LayEmailTruongPhong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        pb.maPhong,
        pb.tenPhong,
        nv.hoTen AS tenTruongPhong,
        nv.email
    FROM PhongBan pb
    INNER JOIN NhanVien nv ON pb.truongPhong = nv.maNV
    WHERE nv.email IS NOT NULL AND nv.email <> '';
END;
GO


CREATE OR ALTER PROCEDURE sp_CapNhatTrangThaiEmail
    @maDot VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ThongBao
    SET daGuiEmail = 1
    WHERE maDot = @maDot
      AND loaiTB = 'QUA_HAN_DOT'; -- chỉ cập nhật cho loại cảnh báo quá hạn
END;
GO

CREATE OR ALTER PROCEDURE sp_CapNhatThongTinCaNhan
    @MaNV        VARCHAR(15),
    @HoTen       NVARCHAR(60),
    @NgaySinh    DATE           = NULL,
    @GioiTinh    BIT            = NULL,    -- 1: Nam, 0: Nữ
    @DiaChi      NVARCHAR(150)  = NULL,
    @SoDienThoai VARCHAR(20),
    @Email       VARCHAR(50)    = NULL,
    @AnhDaiDien  NVARCHAR(255)  = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- 1️⃣ Kiểm tra nhân viên tồn tại
        IF NOT EXISTS (SELECT 1 FROM NhanVien WHERE maNV = @MaNV AND daXoa = 0)
        BEGIN
            RAISERROR(N'Không tìm thấy nhân viên cần cập nhật!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 2️⃣ Kiểm tra họ tên hợp lệ
        IF LTRIM(RTRIM(@HoTen)) = ''
        BEGIN
            RAISERROR(N'Họ tên không được để trống!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        IF PATINDEX('%[^a-zA-ZÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵýỷỹ ]%', @HoTen) > 0
        BEGIN
            RAISERROR(N'Họ tên không hợp lệ! Chỉ được chứa chữ cái và khoảng trắng.', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 3️⃣ Kiểm tra tuổi hợp lệ (16–65)
        IF @NgaySinh IS NOT NULL
        BEGIN
            DECLARE @Tuoi INT = DATEDIFF(YEAR, @NgaySinh, GETDATE());
            IF (DATEADD(YEAR, @Tuoi, @NgaySinh) > GETDATE()) SET @Tuoi -= 1;
            IF @Tuoi < 16 OR @Tuoi > 65
            BEGIN
                RAISERROR(N'Tuổi không hợp lệ! Nhân viên phải từ 16 đến 65 tuổi.', 16, 1);
                ROLLBACK TRAN; RETURN;
            END;
        END;

        -- 4️⃣ Kiểm tra số điện thoại hợp lệ (10 chữ số, bắt đầu bằng 0)
        DECLARE @soDienThoaiTrimmed VARCHAR(20) = LTRIM(RTRIM(@SoDienThoai));
        IF LEN(@soDienThoaiTrimmed) <> 10 
           OR LEFT(@soDienThoaiTrimmed, 1) <> '0' 
           OR PATINDEX('%[^0-9]%', @soDienThoaiTrimmed) > 0
        BEGIN
            RAISERROR(N'Số điện thoại không hợp lệ! Phải bắt đầu bằng 0, đúng 10 chữ số, không chứa ký tự khác.', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 5️⃣ Kiểm tra email hợp lệ
        IF @Email IS NULL OR LTRIM(RTRIM(@Email)) = ''
        BEGIN
            RAISERROR(N'Email không được để trống!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        IF @Email NOT LIKE '%_@_%._%' 
           OR @Email LIKE '%..%' 
           OR @Email LIKE '%.@%' 
           OR RIGHT(@Email, 4) NOT IN ('.com', '.net', '.org', '.edu', '.gov', '.vn')
        BEGIN
            RAISERROR(N'Email không hợp lệ! Vui lòng nhập đúng định dạng (vd: abc@gmail.com).', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 6️⃣ Kiểm tra trùng email
        IF EXISTS (SELECT 1 FROM NhanVien WHERE email = @Email AND maNV <> @MaNV AND daXoa = 0)
        BEGIN
            RAISERROR(N'Email này đã tồn tại cho nhân viên khác!', 16, 1);
            ROLLBACK TRAN; RETURN;
        END;

        -- 7️⃣ Cập nhật thông tin cá nhân
        UPDATE NhanVien
        SET hoTen       = @HoTen,
            ngaySinh    = @NgaySinh,
            gioiTinh    = @GioiTinh,
            diaChi      = @DiaChi,
            soDienThoai = @SoDienThoai,
            email       = @Email,
            anhDaiDien  = @AnhDaiDien
        WHERE maNV = @MaNV AND daXoa = 0;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN;
        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrMsg, 16, 1);
    END CATCH;
END;
GO

CREATE OR ALTER PROCEDURE sp_LayThongTinCaNhan
    @Email VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
           maNV,
           maPhong,
           hoTen,
           ngaySinh,
           gioiTinh,
           diaChi,
           soDienThoai,
           email,
           anhDaiDien
    FROM   NhanVien
    WHERE  daXoa = 0        -- không lấy nhân viên đã xóa mềm
       AND email = @Email;  -- nhân viên trùng email đăng nhập
END;
GO
----Tối ưu thông báo cho nhân viên 12/11 
CREATE TABLE dbo.ThongBao_NguoiDung (
    maTB        VARCHAR(15)  NOT NULL,       -- FK -> ThongBao
    maNV        VARCHAR(15)  NOT NULL,       -- FK -> NhanVien
    trangThaiDoc BIT          NOT NULL DEFAULT 0, -- 0: chưa đọc, 1: đã đọc
    ngayDoc     DATETIME      NULL,
    CONSTRAINT PK_ThongBao_NguoiDung PRIMARY KEY (maTB, maNV),
    CONSTRAINT FK_TB_ND_ThongBao FOREIGN KEY (maTB) REFERENCES dbo.ThongBao(maTB),
    CONSTRAINT FK_TB_ND_NhanVien FOREIGN KEY (maNV) REFERENCES dbo.NhanVien(maNV)
);

-- Index khuyến nghị để đếm nhanh & lọc theo người
CREATE INDEX IX_TB_ND_maNV_trangThaiDoc ON dbo.ThongBao_NguoiDung(maNV, trangThaiDoc);
CREATE INDEX IX_TB_ND_maNV ON dbo.ThongBao_NguoiDung(maNV);

GO
CREATE PROCEDURE dbo.sp_LayThongBaoTheoNhanVien
    @maNV VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT  
        tb.maTB,
        tb.maHD,
        tb.maDot,
        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao,
        tb.loaiTB,
        kh.tenDoanhNghiep AS tenKhachHang,   -- nếu cần hiển thị KH
        nd.trangThaiDoc,                      -- ✅ thêm cột trạng thái đọc
        nd.ngayDoc                            -- (tùy bạn, để xem lúc nào đọc)
    FROM dbo.ThongBao tb
    JOIN dbo.ThongBao_NguoiDung nd ON nd.maTB = tb.maTB
    LEFT JOIN dbo.HopDong hd ON tb.maHD = hd.maHD
    LEFT JOIN dbo.KhachHang kh ON hd.maKH = kh.maKH
    WHERE nd.maNV = @maNV
    ORDER BY tb.ngayTao DESC, tb.maTB DESC;
END;
GO
----Đánh dấu thông báo là đã đọc 
CREATE PROCEDURE dbo.sp_DanhDauThongBaoDaDoc
    @maTB VARCHAR(15),
    @maNV VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE dbo.ThongBao_NguoiDung
    SET trangThaiDoc = 1,
        ngayDoc      = ISNULL(ngayDoc, GETDATE())
    WHERE maTB = @maTB AND maNV = @maNV;
END
GO
-----xóa thông báo cho người dùng 
CREATE PROCEDURE dbo.sp_XoaThongBaoNguoiDung
    @maTB VARCHAR(15),
    @maNV VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM dbo.ThongBao_NguoiDung
    WHERE maTB = @maTB AND maNV = @maNV;
END
GO
----đếm thông báo chưa đọc 
CREATE PROCEDURE dbo.sp_DemThongBaoChuaDoc
    @maNV VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS SoChuaDoc
    FROM dbo.ThongBao_NguoiDung
    WHERE maNV = @maNV AND trangThaiDoc = 0;
END
GO
CREATE PROCEDURE sp_KiemTraHopDongQuaHan
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);
    DECLARE @NewMaTB VARCHAR(15);
    DECLARE @maHD VARCHAR(15), @maKH VARCHAR(15), @tenKH NVARCHAR(255);

    DECLARE cur CURSOR FOR
    SELECT hd.maHD, hd.maKH, kh.tenDoanhNghiep
    FROM HopDong hd
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE hd.ngayKetThucHD < @NgayHienTai
      AND hd.trangThai NOT IN ('TT03', 'TT04')
      AND NOT EXISTS (
            SELECT 1 
            FROM ThongBao tb
            WHERE tb.maHD = hd.maHD AND tb.loaiTB = 'HOP_DONG_QUA_HAN'
      );

    OPEN cur;
    FETCH NEXT FROM cur INTO @maHD, @maKH, @tenKH;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Sinh mã thông báo
        SELECT @NewMaTB =
            'TB' + RIGHT('000000'+ CAST(ISNULL(MAX(CAST(SUBSTRING(maTB,3,6) AS INT)),0)+1 AS VARCHAR(6)),6)
        FROM ThongBao;

        -- Insert vào bảng ThongBao
        INSERT INTO ThongBao(maTB, loaiTB, maHD, tieuDe, noiDung, ngayTao)
        VALUES (
            @NewMaTB,
            'HOP_DONG_QUA_HAN',
            @maHD,
            N'Hợp đồng ' + @maHD + N' đã quá hạn',
            N'Hợp đồng của khách hàng: ' + @tenKH +
            N'. Ngày hiện tại: ' + CONVERT(VARCHAR(10), @NgayHienTai, 103),
            GETDATE()
        );

        -- Gửi thông báo đến toàn bộ nhân viên phòng kinh doanh P001
        INSERT INTO ThongBao_NguoiDung(maTB, maNV, trangThaiDoc)
        SELECT @NewMaTB, nv.maNV, 0
        FROM NhanVien nv
        WHERE nv.maPhong = 'P001'
          AND nv.trangThai = 1;

        FETCH NEXT FROM cur INTO @maHD, @maKH, @tenKH;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO

CREATE OR ALTER PROCEDURE sp_LayDanhSachThongBaoHopDongQuaHan
AS
BEGIN
    SELECT tb.maTB, tb.maHD, kh.tenDoanhNghiep AS tenKhachHang
    FROM ThongBao tb
    JOIN HopDong hd ON tb.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE tb.loaiTB = 'HOP_DONG_QUA_HAN'
      AND ISNULL(tb.daGuiEmail, 0) = 0;
END;
GO

CREATE PROCEDURE sp_CapNhatTrangThaiEmailHopDong
    @maTB VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ThongBao
    SET daGuiEmail = 1
    WHERE maTB = @maTB
      AND loaiTB = 'HOP_DONG_QUA_HAN';
END;
GO

CREATE OR ALTER PROCEDURE dbo.sp_LayThongBaoTheoNhanVien_PhanTrang
    @maNV VARCHAR(15),
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT  
        tb.maTB,
        tb.maHD,
        tb.maDot,
        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao,
        tb.loaiTB,
        kh.tenDoanhNghiep AS tenKhachHang,
        nd.trangThaiDoc,
        nd.ngayDoc
    FROM dbo.ThongBao tb
    JOIN dbo.ThongBao_NguoiDung nd ON nd.maTB = tb.maTB
    LEFT JOIN dbo.HopDong hd ON tb.maHD = hd.maHD
    LEFT JOIN dbo.KhachHang kh ON hd.maKH = kh.maKH
    WHERE nd.maNV = @maNV
    ORDER BY tb.ngayTao DESC, tb.maTB DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END;
GO

-- Stored Procedure đếm tổng số thông báo
CREATE PROCEDURE dbo.sp_DemTongSoThongBao
    @maNV VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(*) AS TongSo
    FROM dbo.ThongBao tb
    JOIN dbo.ThongBao_NguoiDung nd ON nd.maTB = tb.maTB
    WHERE nd.maNV = @maNV;
END;
GO

create proc layTrangthaihopdong
as
begin
	select * from trangThaiHD
end
go

CREATE OR ALTER PROC sp_layDanhSachThongSo_PhanTrang
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        maTS,
        tenTS,
        giaTriToiDa,
        giaTriToiThieu,
        donVi,
        phuongPhap
    FROM ThongSoMoiTruong
    ORDER BY tenTS
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROC sp_layDanhSachNenMau_PhanTrang
    @PageNumber INT,
    @PageSize INT,
    @keyword NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        maNen,
        tenNenMau,
        moTa
    FROM NenMau
    WHERE 
        @keyword IS NULL
        OR @keyword = ''
        OR maNen LIKE '%' + @keyword + '%'
        OR tenNenMau LIKE '%' + @keyword + '%'
        OR moTa LIKE '%' + @keyword + '%'
    ORDER BY maNen
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

CREATE OR ALTER PROC demSoLuongThongSo
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalRecords
    FROM ThongSoMoiTruong;
END
GO

CREATE OR ALTER PROC demSoLuongNenMau
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalRecords
    FROM NenMau;
END
GO

CREATE OR ALTER PROCEDURE layDanhSachHopDong_PhanTrang
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        hd.maHD,
        hd.maKH,
        hd.ngayKy,
        hd.ngayKetThucHD,
        tt.tenTT AS trangThai,
        tsqt.tenTSQT AS tanSuatQuanTrac,
        hd.soHD
    FROM HopDong AS hd
        LEFT JOIN tanSuatQT tsqt 
            ON hd.tanSuatQuanTrac = tsqt.maTSQT
        LEFT JOIN dbo.trangThaiHD AS tt  
            ON tt.maTT = hd.trangThai
    ORDER BY
        -- 1. Đang hiệu lực lên trước
        CASE 
            WHEN tt.tenTT = N'Đang hiệu lực' THEN 0 
            ELSE 1 
        END,
        -- 2. Trong nhóm đang hiệu lực: còn ít ngày nhất đứng đầu
        CASE 
            WHEN hd.ngayKetThucHD >= CAST(GETDATE() AS date) 
                THEN DATEDIFF(DAY, CAST(GETDATE() AS date), hd.ngayKetThucHD)
            ELSE 999999 -- đã hết hạn đẩy xuống cuối
        END,
        -- 3. Tie-break: hợp đồng ký sớm đứng trước
        hd.ngayKy ASC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO


CREATE OR ALTER PROCEDURE demSoLuongHopDong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS TotalRecords
    FROM HopDong;
END
GO

CREATE   PROCEDURE [dbo].[sp_KiemTraVaSinhThongBaoNhacKyHopDong]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NewMaTB VARCHAR(15);
    DECLARE @maDot VARCHAR(15), @maHD VARCHAR(15);
    DECLARE @maKH VARCHAR(15), @tenKH NVARCHAR(255);
    DECLARE @ngayBatDau DATE, @tanSuat VARCHAR(15), @ngayNhac DATE;
    DECLARE @ngayTraKQ DATE;

    DECLARE cur CURSOR FOR
    SELECT 
        d.maDot,
        d.maHD,
        kh.maKH,
        kh.tenDoanhNghiep,
        d.ngayBatDau,
        d.ngayTraKQ,
        hd.tanSuatQuanTrac,
        CASE 
            WHEN hd.tanSuatQuanTrac = 'TSQT03' 
                THEN DATEADD(DAY, 75, d.ngayBatDau)      -- 2 tháng 15 ngày
            WHEN hd.tanSuatQuanTrac = 'TSQT02' 
                THEN DATEADD(DAY, 165, d.ngayBatDau)     -- 5 tháng 15 ngày
        END AS ngayNhac
    FROM DotQuanTrac d
    JOIN HopDong hd ON d.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE 
        d.ngayTraKQ IS NOT NULL                           -- đã hoàn thành đợt
        AND NOT EXISTS (                                   -- chưa từng gửi TB này
            SELECT 1 
            FROM ThongBao tb 
            WHERE tb.maDot = d.maDot 
              AND tb.loaiTB = 'NHAC_KY_HOP_DONG'
        );

    OPEN cur;
    FETCH NEXT FROM cur INTO @maDot, @maHD, @maKH, @tenKH, @ngayBatDau, @ngayTraKQ, @tanSuat, @ngayNhac;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Chỉ gửi khi đúng ngày nhắc hoặc đã quá ngày nhắc
        IF @ngayNhac <= CAST(GETDATE() AS DATE)
        BEGIN
            -- Sinh mã TB giống format cũ
            SELECT @NewMaTB =
                'TB' + RIGHT('000000'+ CAST(ISNULL(MAX(CAST(SUBSTRING(maTB,3,6) AS INT)),0)+1 AS VARCHAR(6)),6)
            FROM ThongBao;

            -- Thêm vào ThongBao
            INSERT INTO ThongBao(maTB, loaiTB, maDot, maHD, tieuDe, noiDung, ngayTao)
            VALUES (
                @NewMaTB,
                'NHAC_KY_HOP_DONG',
                @maDot,
                @maHD,
                N'Nhắc ký hợp đồng mới cho khách hàng ' + @tenKH,
                N'Đã hoàn thành đợt quan trắc. Cần nhắc khách hàng ký hợp đồng mới.',
                GETDATE()
            );
        END

        FETCH NEXT FROM cur INTO @maDot, @maHD, @maKH, @tenKH, @ngayBatDau, @ngayTraKQ, @tanSuat, @ngayNhac;
    END;

    CLOSE cur;
    DEALLOCATE cur;

END
GO 

CREATE PROCEDURE sp_LayDSNhacKyHopDong
AS
BEGIN
    SELECT 
        tb.maTB,
        tb.maDot,
        tb.maHD,
        hd.maKH,
        kh.tenDoanhNghiep,
        kh.emailDoanhNghiep AS email,

        d.ngayBatDau,                   
        d.ngayTraKQ,                    
        hd.tanSuatQuanTrac,              

        tb.tieuDe,
        tb.noiDung,
        tb.ngayTao

    FROM ThongBao tb
    JOIN HopDong hd ON tb.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    JOIN DotQuanTrac d ON d.maDot = tb.maDot   

    WHERE 
        tb.loaiTB = 'NHAC_KY_HOP_DONG'
        AND ISNULL(tb.daGuiEmail, 0) = 0;
END
GO

CREATE PROCEDURE sp_CapNhatTrangThaiEmail_NhacHD
    @maTB VARCHAR(15)
AS
BEGIN
    UPDATE ThongBao 
    SET daGuiEmail = 1
    WHERE maTB = @maTB
	AND loaiTB = 'NHAC_KY_HOP_DONG';
END
GO

CREATE OR ALTER PROCEDURE [dbo].[layDanhSachNhanVien_TimKiem]
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        nv.maNV,
        nv.maPhong,
        nv.hoTen,
        nv.ngaySinh,
        nv.gioiTinh,
        nv.diaChi,
        nv.soDienThoai,
        nv.email,
        nv.trangThai,
        pb.tenPhong,
        CASE nv.trangThai 
            WHEN 1 THEN N'Đang hoạt động'
            WHEN 2 THEN N'Nghỉ phép'
            WHEN 4 THEN N'Nghỉ thai sản'
            WHEN 5 THEN N'Công tác'
            WHEN 6 THEN N'Ngưng hoạt động'
            ELSE N'Không xác định'
        END AS tenTrangThai
    FROM NhanVien nv
    LEFT JOIN PhongBan pb ON nv.maPhong = pb.maPhong
    WHERE nv.trangThai != 7  -- Loại bỏ nhân viên đã xóa
    ORDER BY nv.maNV;
END
GO

--THÊM PHẦN FACE ID 21/11/2025 PTT
IF OBJECT_ID(N'[dbo].[FaceRecognition]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[FaceRecognition];
END
GO

-- Tạo bảng FaceRecognition
CREATE TABLE [dbo].[FaceRecognition](
    [tenTK]       VARCHAR(50)    NOT NULL,         
    [faceData]    VARBINARY(MAX) NOT NULL,           
    [ngayTao]     DATETIME       NOT NULL DEFAULT GETDATE(),
    [ngayCapNhat] DATETIME       NULL,
    
    CONSTRAINT [PK_FaceRecognition] PRIMARY KEY CLUSTERED ([tenTK] ASC),
    
    -- Khóa ngoại liên kết với bảng TaiKhoan
    CONSTRAINT [FK_FaceRecognition_TaiKhoan] FOREIGN KEY([tenTK]) 
        REFERENCES [dbo].[TaiKhoan]([tenTK]) ON DELETE CASCADE
);
GO

-- Index tối ưu truy vấn
IF NOT EXISTS (
    SELECT 1 FROM sys.indexes 
    WHERE name = N'IX_FaceRecognition_TenTK' 
      AND object_id = OBJECT_ID(N'[dbo].[FaceRecognition]')
)
BEGIN
    CREATE NONCLUSTERED INDEX [IX_FaceRecognition_TenTK]
        ON [dbo].[FaceRecognition]([tenTK]);
END
GO

-- STORED PROCEDURE 1: Lưu/Cập nhật dữ liệu khuôn mặt

CREATE PROCEDURE [dbo].[sp_LuuFaceData]
    @tenTK    VARCHAR(50),
    @faceData VARBINARY(MAX),
    @Success  BIT OUTPUT,
    @Message  NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- Kiểm tra tài khoản tồn tại
        IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE tenTK = @tenTK)
        BEGIN
            SET @Success = 0;
            SET @Message = N'Tài khoản không tồn tại!';
            ROLLBACK TRAN;
            RETURN;
        END

        -- Nếu đã có dữ liệu Face thì cập nhật, ngược lại thì thêm mới
        IF EXISTS (SELECT 1 FROM FaceRecognition WHERE tenTK = @tenTK)
        BEGIN
            UPDATE FaceRecognition
            SET faceData    = @faceData,
                ngayCapNhat = GETDATE()
            WHERE tenTK = @tenTK;

            SET @Message = N'Cập nhật dữ liệu khuôn mặt thành công!';
        END
        ELSE
        BEGIN
            INSERT INTO FaceRecognition(tenTK, faceData, ngayTao)
            VALUES(@tenTK, @faceData, GETDATE());

            SET @Message = N'Lưu dữ liệu khuôn mặt thành công!';
        END

        SET @Success = 1;
        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        SET @Success = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO

-- STORED PROCEDURE 2: Lấy dữ liệu Face theo tài khoản
CREATE PROCEDURE [dbo].[sp_LayFaceDataTheoTK]
    @tenTK VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        tenTK,
        faceData,
        ngayTao,
        ngayCapNhat
    FROM FaceRecognition
    WHERE tenTK = @tenTK;
END
GO


-- STORED PROCEDURE 3: Lấy tất cả dữ liệu khuôn mặt

CREATE PROCEDURE [dbo].[sp_LayTatCaFaceData]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        tenTK,
        faceData,
        ngayTao,
        ngayCapNhat
    FROM FaceRecognition
    ORDER BY ngayTao DESC;
END
GO

-- STORED PROCEDURE 4: Xóa dữ liệu Face của tài khoản

CREATE PROCEDURE [dbo].[sp_XoaFaceData]
    @tenTK   VARCHAR(50),
    @Success BIT OUTPUT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM FaceRecognition WHERE tenTK = @tenTK)
        BEGIN
            SET @Success = 0;
            SET @Message = N'Không tìm thấy dữ liệu khuôn mặt!';
            RETURN;
        END

        DELETE FROM FaceRecognition WHERE tenTK = @tenTK;

        SET @Success = 1;
        SET @Message = N'Xóa dữ liệu khuôn mặt thành công!';
    END TRY
    BEGIN CATCH
        SET @Success = 0;
        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO


-- STORED PROCEDURE 5: Kiểm tra tài khoản đã đăng ký Face ID chưa
CREATE PROCEDURE [dbo].[sp_KiemTraFaceDataTonTai]
    @tenTK  VARCHAR(50),
    @TonTai BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM FaceRecognition WHERE tenTK = @tenTK)
        SET @TonTai = 1;
    ELSE
        SET @TonTai = 0;
END

GO
-----23/11
--CREATE PROCEDURE [dbo].[LayPhongBanTheoTaiKhoan]
--    @tenTK VARCHAR(50)
--AS
--BEGIN
--    SELECT maPhong 
--    FROM NhanVien 
--    WHERE email = @tenTK;
--END;
--GO
-- 1. Stored procedure lấy danh sách kết quả CÓ PHÂN TRANG
CREATE OR ALTER PROCEDURE [dbo].[LayDanhSachKetQua_PhanTrang]
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);
    DECLARE @NgayThongBao DATE;
    DECLARE @maDot VARCHAR(15),
            @maHD VARCHAR(15),
            @tenKH NVARCHAR(255),
            @ngayDuKien DATE,
            @NewMaTB VARCHAR(15);

    DECLARE cur CURSOR FOR
    SELECT dq.maDot, dq.maHD, kh.tenDoanhNghiep, dq.ngayDuKien
    FROM DotQuanTrac dq
    JOIN HopDong hd ON dq.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE dq.trangThai <> 6
      AND dq.ngayDuKien IS NOT NULL
      AND DATEADD(DAY, -2, dq.ngayDuKien) = @NgayHienTai
      AND NOT EXISTS (
            SELECT 1 FROM ThongBao tb
            WHERE tb.maDot = dq.maDot
              AND tb.loaiTB = 'NHAC_SAP_DEN_HAN_DOT'
      );

    OPEN cur;
    FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @NewMaTB =
            'TB' + RIGHT('000000' + CAST(ISNULL(MAX(CAST(SUBSTRING(maTB, 3, 6) AS INT)), 0) + 1 AS VARCHAR(6)), 6)
        FROM ThongBao;

        INSERT INTO ThongBao (maTB, loaiTB, maDot, maHD, tieuDe, noiDung, ngayTao)
        VALUES (
            @NewMaTB,
            'NHAC_SAP_DEN_HAN_DOT',
            @maDot,
            @maHD,
            N'Nhắc sắp đến hạn trả kết quả cho đợt ' + @maDot,
            N'Khách hàng: ' + @tenKH
            + N'. Ngày dự kiến trả kết quả: ' + CONVERT(VARCHAR(10), @ngayDuKien, 103)
            + N'. Hệ thống gửi nhắc nhở trước 2 ngày.',
            GETDATE()
        );

        INSERT INTO ThongBao_NguoiDung(maTB, maNV, trangThaiDoc, ngayDoc)
        SELECT @NewMaTB, nv.maNV, 0, NULL
        FROM NhanVien nv
        WHERE nv.trangThai = 1;

        FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_suaHopDong]
    @maHD VARCHAR(15),
    @maKH VARCHAR(15),
    @ngayKy DATE,
    @ngayKetThucHD DATE,
    @trangThai VARCHAR(15),        -- ví dụ: TT01/TT02/TT03/TT04
    @tanSuatQuanTrac VARCHAR(15),  -- FK tới tanSuatQT.maTSQT
    @soHD NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;  -- an toàn rollback tự động khi lỗi

    BEGIN TRY
        BEGIN TRAN;

        -- 1) Tồn tại hợp đồng cần sửa
        IF NOT EXISTS (SELECT 1 FROM HopDong WHERE maHD = @maHD)
        BEGIN
            RAISERROR(N'Không tìm thấy hợp đồng cần sửa!', 16, 1);
        END

        -- 2) Kiểm tra ngày
        IF (@ngayKy >= @ngayKetThucHD)
        BEGIN
            RAISERROR(N'Ngày kết thúc phải sau ngày ký.', 16, 1);
        END

        -- 3) Kiểm tra tồn tại KH & Tần suất (nếu chưa có FK cứng)
        IF NOT EXISTS (SELECT 1 FROM KhachHang WHERE maKH = @maKH)
        BEGIN
            RAISERROR(N'Không tìm thấy khách hàng.', 16, 1);
        END

        IF NOT EXISTS (SELECT 1 FROM tanSuatQT WHERE maTSQT = @tanSuatQuanTrac)
        BEGIN
            RAISERROR(N'Không tìm thấy tần suất quan trắc.', 16, 1);
        END

        -- 4) Quy tắc trạng thái theo ngày (cân nhắc điều chỉnh cho TT03 nếu duyệt tay)
        DECLARE @today DATE = CAST(GETDATE() AS DATE);

        IF (@trangThai = 'TT01' AND NOT (@ngayKy <= @today AND @today <= @ngayKetThucHD))
            RAISERROR(N'Trạng thái đang hiệu lực yêu cầu ngày hiện tại nằm trong khoảng từ ngày ký đến ngày kết thúc.', 16, 1);

        IF (@trangThai = 'TT02' AND NOT (@today > @ngayKetThucHD))
            RAISERROR(N'Trạng thái hết hạn yêu cầu ngày hiện tại đã sau ngày kết thúc.', 16, 1);

        -- Nếu TT03 là duyệt tay, hãy cân nhắc bỏ ràng buộc ngày:
        -- IF (@trangThai = 'TT03' AND NOT (@today > @ngayKetThucHD))
        --     RAISERROR(N'Trạng thái hoàn thành yêu cầu ngày hiện tại đã sau ngày kết thúc.', 16, 1);

        IF (@trangThai = 'TT04' AND NOT (@ngayKy <= @today AND @today <= @ngayKetThucHD))
            RAISERROR(N'Trạng thái chấm dứt trước thời hạn yêu cầu hợp đồng đang trong thời gian hiệu lực.', 16, 1);

        -- 5) Check trùng (cùng KH, cùng ngày ký) nhưng loại trừ chính hợp đồng đang sửa
        DECLARE @tenDN NVARCHAR(100);
        SELECT @tenDN = tenDoanhNghiep FROM KhachHang WHERE maKH = @maKH;

        IF EXISTS (
            SELECT 1
            FROM HopDong
            WHERE maKH = @maKH
              AND ngayKy = @ngayKy
              AND maHD <> @maHD
        )
        BEGIN
            DECLARE @day VARCHAR(10) = CONVERT(VARCHAR(10), @ngayKy, 103); -- dd/MM/yyyy
            RAISERROR(N'Đã có hợp đồng của %s vào ngày %s.', 16, 1, @tenDN, @day);
        END

        -- 6) Cập nhật
        UPDATE HopDong
		SET maKH             = @maKH,
            ngayKy           = @ngayKy,
            ngayKetThucHD    = @ngayKetThucHD,
            trangThai        = @trangThai,
            tanSuatQuanTrac  = @tanSuatQuanTrac,
            soHD             = @soHD
        WHERE maHD = @maHD;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF (XACT_STATE() <> 0) ROLLBACK TRAN;

        DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrState INT = ERROR_STATE();

        RAISERROR(@ErrMsg, @ErrSeverity, @ErrState);
    END CATCH
END
GO


INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro) VALUES 
('pkdnguyenvana001@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('pkdtranthib002@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('pkdlephuongc003@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0);

--INSERT INTO NhanVien (maNV, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email, ngayTao, trangThai, daXoa) VALUES
--('NV009', 'P001', N'Nguyễn Văn An', '1990-05-15', 1, N'123 Nguyễn Huệ, Quận 1, TP.HCM', '0901234567', 'pkdnguyenvana001@gmail.com', '2024-01-10', 1, 0),
--('NV010', 'P001', N'Trần Thị Bích', '1992-08-20', 0, N'456 Lê Lợi, Quận 3, TP.HCM', '0902345678', 'pkdtranthib002@gmail.com', '2024-01-15', 1, 0),
--('NV011', 'P001', N'Lê Phương Chi', '1988-03-12', 0, N'789 Trần Hưng Đạo, Quận 5, TP.HCM', '0903456789', 'pkdlephuongc003@gmail.com', '2024-02-01', 1, 0);

-- Phòng Kế Hoạch (P002)
INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro) VALUES 
('pkhphamminhdung004@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('pkhvothaiem005@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('pkhhoangvanphuc006@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0);

INSERT INTO NhanVien (maNV, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email, ngayTao, trangThai, daXoa) VALUES
('NV012', 'P002', N'Phạm Minh Dũng', '1991-11-25', 1, N'234 Cách Mạng Tháng 8, Quận 10, TP.HCM', '0904567890', 'pkhphamminhdung004@gmail.com', '2024-01-20', 1, 0),
('NV013', 'P002', N'Võ Thái Em', '1993-07-08', 0, N'567 Võ Văn Tần, Quận 3, TP.HCM', '0905678901', 'pkhvothaiem005@gmail.com', '2024-02-05', 1, 0),
('NV014', 'P002', N'Hoàng Văn Phúc', '1989-12-30', 1, N'890 Điện Biên Phủ, Quận Bình Thạnh, TP.HCM', '0906789012', 'pkhhoangvanphuc006@gmail.com', '2024-02-10', 1, 0);

-- Phòng Hiện Trường (P003)
INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro) VALUES 
('phtdoanquanggiang007@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('phtnguyenthihanh008@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('phtbuiduchoai009@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0);

INSERT INTO NhanVien (maNV, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email, ngayTao, trangThai, daXoa) VALUES
('NV015', 'P003', N'Đoàn Quang Giang', '1994-04-18', 1, N'345 Lý Thường Kiệt, Quận 11, TP.HCM', '0907890123', 'phtdoanquanggiang007@gmail.com', '2024-02-15', 1, 0),
('NV016', 'P003', N'Nguyễn Thị Hạnh', '1990-09-22', 0, N'678 Hai Bà Trưng, Quận 1, TP.HCM', '0908901234', 'phtnguyenthihanh008@gmail.com', '2024-02-20', 1, 0),
('NV017', 'P003', N'Bùi Đức Hoài', '1987-06-14', 1, N'901 Nguyễn Thị Minh Khai, Quận 3, TP.HCM', '0909012345', 'phtbuiduchoai009@gmail.com', '2024-03-01', 1, 0);

-- Phòng Thí Nghiệm (P004)
INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro) VALUES 
('ptntrinhvaninh010@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('ptnlethikieu011@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('ptnphanduclinh012@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0);

INSERT INTO NhanVien (maNV, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email, ngayTao, trangThai, daXoa) VALUES
('NV018', 'P004', N'Trịnh Văn Inh', '1995-02-28', 1, N'112 Pasteur, Quận 1, TP.HCM', '0910123456', 'ptntrinhvaninh010@gmail.com', '2024-03-05', 1, 0),
('NV019', 'P004', N'Lê Thị Kiều', '1991-10-05', 0, N'445 Cộng Hòa, Quận Tân Bình, TP.HCM', '0911234567', 'ptnlethikieu011@gmail.com', '2024-03-10', 1, 0),
('NV020', 'P004', N'Phan Đức Linh', '1989-01-17', 1, N'778 Xô Viết Nghệ Tĩnh, Quận Bình Thạnh, TP.HCM', '0912345678', 'ptnphanduclinh012@gmail.com', '2024-03-15', 1, 0);

-- Phòng Kết Quả (P005)
INSERT INTO TaiKhoan (tenTK, matKhau, vaiTro) VALUES 
('pkqngothimai013@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('pkqvuducnam014@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0),
('pkqduongthaiquan015@gmail.com', '$2a$10$cIpQyUtNMCZDqBVqgq5cb.JD7E5ysCTIetHeq37yWpM5L5oMJ2tri', 0);

INSERT INTO NhanVien (maNV, maPhong, hoTen, ngaySinh, gioiTinh, diaChi, soDienThoai, email, ngayTao, trangThai, daXoa) VALUES
('NV021', 'P005', N'Ngô Thị Mai', '1992-12-09', 0, N'223 Nam Kỳ Khởi Nghĩa, Quận 3, TP.HCM', '0913456789', 'pkqngothimai013@gmail.com', '2024-03-20', 1, 0),
('NV022', 'P005', N'Vũ Đức Nam', '1988-05-23', 1, N'556 Hoàng Văn Thụ, Quận Phú Nhuận, TP.HCM', '0914567890', 'pkqvuducnam014@gmail.com', '2024-04-01', 1, 0),
('NV023', 'P005', N'Dương Thái Quân', '1993-08-11', 1, N'889 Lê Văn Sỹ, Quận Tân Bình, TP.HCM', '0915678901', 'pkqduongthaiquan015@gmail.com', '2024-04-05', 1, 0);

-- =============================================
-- 2. THÊM HỢP ĐỒNG (15 hợp đồng)
-- =============================================

INSERT INTO HopDong (maHD, maKH, ngayKy, ngayKetThucHD, trangThai, tanSuatQuanTrac, soHD) VALUES
('HD023', 'KH001', '2025-01-05', '2025-12-31', 'TT01', 'TSQT02', 'HD2025/001'),
('HD024', 'KH002', '2025-01-10', '2025-06-30', 'TT01', 'TSQT03', 'HD2025/002'),
('HD025', 'KH003', '2025-01-15', '2026-01-15', 'TT01', 'TSQT01', 'HD2025/003'),
('HD026', 'KH004', '2025-02-01', '2025-08-01', 'TT01', 'TSQT02', 'HD2025/004'),
('HD027', 'KH005', '2025-02-10', '2025-11-30', 'TT01', 'TSQT03', 'HD2025/005'),
('HD028', 'KH006', '2025-02-20', '2026-02-20', 'TT01', 'TSQT01', 'HD2025/006'),
('HD029', 'KH007', '2025-03-01', '2025-09-01', 'TT01', 'TSQT02', 'HD2025/007'),
('HD030', 'KH008', '2025-03-10', '2025-12-31', 'TT01', 'TSQT03', 'HD2025/008'),
('HD031', 'KH009', '2025-03-15', '2026-03-15', 'TT01', 'TSQT01', 'HD2025/009'),
('HD032', 'KH010', '2025-04-01', '2025-10-01', 'TT01', 'TSQT02', 'HD2025/010'),
('HD033', 'KH011', '2025-04-10', '2025-12-31', 'TT01', 'TSQT03', 'HD2025/011'),
('HD034', 'KH012', '2025-04-20', '2026-04-20', 'TT01', 'TSQT01', 'HD2025/012'),
('HD035', 'KH013', '2025-05-01', '2025-11-01', 'TT01', 'TSQT02', 'HD2025/013'),
('HD036', 'KH014', '2025-05-10', '2025-12-31', 'TT01', 'TSQT03', 'HD2025/014'),
('HD037', 'KH015', '2025-05-20', '2026-05-20', 'TT01', 'TSQT01', 'HD2025/015');

-- =============================================
-- 3. THÊM ĐỢT QUAN TRẮC (15 đợt)
-- =============================================

INSERT INTO DotQuanTrac (maDot, maHD, noiDung, dotQuanTrac, ngayBatDau, ngayDuKien, ngayTraKQ, trangThai) VALUES
('DT0007', 'HD023', N'Quan trắc định kỳ quý 1/2025', N'Quý 1/2025', '2025-01-15', '2025-03-31', NULL, 1),
('DT0008', 'HD024', N'Quan trắc định kỳ quý 1/2025', N'Quý 1/2025', '2025-01-20', '2025-03-31', '2025-03-25', 5),
('DT0009', 'HD025', N'Quan trắc tháng 2/2025', N'Tháng 2/2025', '2025-02-01', '2025-02-28', '2025-02-27', 5),
('DT0010', 'HD026', N'Quan trắc 6 tháng đầu năm', N'6 tháng đầu/2025', '2025-02-05', '2025-08-05', NULL, 1),
('DT0011', 'HD027', N'Quan trắc quý 1/2025', N'Quý 1/2025', '2025-02-15', '2025-04-30', NULL, 1),
('DT0012', 'HD028', N'Quan trắc tháng 3/2025', N'Tháng 3/2025', '2025-03-01', '2025-03-31', '2025-03-30', 5),
('DT0013', 'HD029', N'Quan trắc 6 tháng đầu năm', N'6 tháng đầu/2025', '2025-03-05', '2025-09-05', NULL, 1),
('DT0014', 'HD030', N'Quan trắc quý 1/2025', N'Quý 1/2025', '2025-03-15', '2025-06-15', NULL, 2),
('DT0015', 'HD031', N'Quan trắc tháng 4/2025', N'Tháng 4/2025', '2025-04-01', '2025-04-30', NULL, 1),
('DT0016', 'HD032', N'Quan trắc 6 tháng đầu năm', N'6 tháng đầu/2025', '2025-04-05', '2025-10-05', NULL, 1),
('DT0017', 'HD033', N'Quan trắc quý 2/2025', N'Quý 2/2025', '2025-04-15', '2025-07-15', NULL, 2),
('DT0018', 'HD034', N'Quan trắc tháng 5/2025', N'Tháng 5/2025', '2025-05-01', '2025-05-31', NULL, 1),
('DT0019', 'HD035', N'Quan trắc 6 tháng đầu năm', N'6 tháng đầu/2025', '2025-05-05', '2025-11-05', NULL, 1),
('DT0020', 'HD036', N'Quan trắc quý 2/2025', N'Quý 2/2025', '2025-05-15', '2025-08-15', NULL, 2),
('DT0021', 'HD037', N'Quan trắc tháng 6/2025', N'Tháng 6/2025', '2025-06-01', '2025-06-30', NULL, 1);

-- =============================================
-- 4. THÊM DOT_NEN (Nền mẫu cho các đợt - 15 bản ghi)
-- =============================================

INSERT INTO Dot_Nen (maDN, maDot, maNen, tenViTri, toaDo, ghiChu) VALUES
('DN0004', 'DT0007', 'NM0001', N'Biển Vũng Tàu - Điểm A1', '10.4113, 107.1362', N'Khu vực gần bờ'),
('DN0005', 'DT0007', 'NM0002', N'Sông Đồng Nai - Cầu Rạch Chiếc', '10.8231, 106.7797', N'Hạ lưu sông'),
('DN0006', 'DT0008', 'NM0003', N'Nước thải KCN Vĩnh Lộc', '10.8523, 106.5789', N'Điểm xả thải'),
('DN0007', 'DT0009', 'NM0004', N'Nước ngầm - Giếng khoan số 5', '10.7890, 106.6789', N'Độ sâu 25m'),
('DN0008', 'DT0010', 'NM0001', N'Biển Bà Rịa - Điểm B2', '10.5412, 107.2453', N'Khu vực xa bờ 2km'),
('DN0009', 'DT0010', 'NM0005', N'Nước mưa - Trạm Thu Đức', '10.8500, 106.7820', N'Thu mẫu theo mùa'),
('DN0010', 'DT0011', 'NM0002', N'Sông Sài Gòn - Cầu Phú Mỹ', '10.7309, 106.7110', N'Thượng nguồn'),
('DN0011', 'DT0012', 'NM0003', N'Nước thải KCN Tân Bình', '10.7987, 106.6234', N'Sau xử lý'),
('DN0012', 'DT0013', 'NM0004', N'Nước ngầm - Giếng khoan số 8', '10.8123, 106.7456', N'Độ sâu 30m'),
('DN0013', 'DT0014', 'NM0001', N'Biển Cần Giờ - Điểm C1', '10.4056, 106.9567', N'Khu vực rừng ngập mặn'),
('DN0014', 'DT0015', 'NM0002', N'Sông Vàm Cỏ - Cầu Tân An', '10.5364, 106.4158', N'Giữa dòng'),
('DN0015', 'DT0016', 'NM0005', N'Nước mẫu - Trạm Bình Dương', '10.9802, 106.6520', N'Thu mẫu tự động'),
('DN0016', 'DT0017', 'NM0003', N'Nước thải KCN Long An', '10.7142, 106.3975', N'Trước xử lý'),
('DN0017', 'DT0018', 'NM0004', N'Nước ngầm - Giếng khoan số 12', '10.7567, 106.6987', N'Độ sâu 35m'),
('DN0018', 'DT0019', 'NM0001', N'Biển Bà Rịa - Điểm D3', '10.5678, 107.2890', N'Khu vực du lịch');

-- =============================================
-- 5. THÊM DOT_NEN_TS (Thông số cho nền mẫu - 15 bản ghi)
-- =============================================

INSERT INTO Dot_Nen_Ts (maDNTS, maDN, maTS, tenTS, donVi, giaTriToiThieu, giaTriToiDa, phuongPhap, maPhong) VALUES
('DNTS0004', 'DN0004', 'TS0001', N'pH', '-', 5.5, 9.0, 'TCVN 6492:2011', 'P004'),
('DNTS0005', 'DN0004', 'TS0002', N'BOD5', 'mg/L', 0, 50, 'TCVN 6001-1:2008', 'P004'),
('DNTS0006', 'DN0005', 'TS0003', N'COD', 'mg/L', 0, 150, 'SMEWW 5220C:2017', 'P004'),
('DNTS0007', 'DN0005', 'TS0004', N'TSS', 'mg/L', 0, 10, 'TCVN 6179-1:1996', 'P004'),
('DNTS0008', 'DN0006', 'TS0006', N'Dầu mỡ', 'mg/L', 0, 100, 'TCVN 6625:2000', 'P004'),
('DNTS0009', 'DN0007', 'TS0011', N'Sắt Fe', 'mg/L', 0, 2, 'TCVN 6193:1996', 'P004'),
('DNTS0010', 'DN0008', 'TS0001', N'pH', '-', 5.5, 9.0, 'TCVN 6492:2011', 'P004'),
('DNTS0011', 'DN0009', 'TS0005', N'DO', 'mg/L', 0, 40, 'TCVN 6638:2000', 'P004'),
('DNTS0012', 'DN0010', 'TS0002', N'BOD5', 'mg/L', 0, 50, 'TCVN 6001-1:2008', 'P004'),
('DNTS0013', 'DN0011', 'TS0003', N'COD', 'mg/L', 0, 150, 'SMEWW 5220C:2017', 'P004'),
('DNTS0014', 'DN0012', 'TS0013', N'Đồng Cu', 'mg/L', 0, 0.1, 'TCVN 6193:1996', 'P004'),
('DNTS0015', 'DN0013', 'TS0009', N'Độ mặn', 'mg/L', NULL, NULL, 'TCVN 6494-1:2011', 'P004'),
('DNTS0016', 'DN0014', 'TS0004', N'TSS', 'mg/L', 0, 10, 'TCVN 6179-1:1996', 'P004'),
('DNTS0017', 'DN0015', 'TS0007', N'Phosphate', 'mg/L', 0, 6, 'TCVN 6202:2008', 'P004'),
('DNTS0018', 'DN0016', 'TS0006', N'Dầu mỡ', 'mg/L', 0, 100, 'TCVN 6625:2000', 'P004');

GO


--CREATE OR ALTER PROCEDURE [dbo].[LayDanhSachKetQua_PhanTrang]
--    @PageNumber INT,
--    @PageSize INT
--AS
--BEGIN
--    SET NOCOUNT ON;
    
--    SELECT 
--        kqh.maKQ,
--        kqh.ngayTao,
--        kqh.ngayTraKQ,
--        nv.hoTen AS TenNhanVien,
--        CASE WHEN kqh.trangThaiXacNhan = 1 THEN N'Đã xác nhận' ELSE N'Chờ xác nhận' END AS TrangThai,
--        kqh.ghiChu,
--        dqt.dotQuanTrac,
--        kh.tenDoanhNghiep AS TenKhachHang
--    FROM KetQuaHeader kqh
--    LEFT JOIN NhanVien nv ON kqh.nhanVienNhap = nv.maNV
--    LEFT JOIN DotQuanTrac dqt ON kqh.maDot = dqt.maDot
--    LEFT JOIN HopDong hd ON dqt.maHD = hd.maHD
--    LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH
--    ORDER BY kqh.ngayTao DESC
--    OFFSET (@PageNumber - 1) * @PageSize ROWS
--    FETCH NEXT @PageSize ROWS ONLY;
--END
--GO

---- 2. Stored procedure đếm tổng số kết quả
--CREATE OR ALTER PROCEDURE [dbo].[DemTongSoKetQua]
--AS
--BEGIN
--    SET NOCOUNT ON;
--    SELECT COUNT(*) FROM KetQuaHeader;
--END
--GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE dbo.sp_Log_AI_TaiKy_TheoDot
    @maDot  VARCHAR(15)     -- đợt quan trắc vừa kết thúc
AS
BEGIN
    SET NOCOUNT ON;

    --------------------------------------------------------
    -- 1. Lấy thông tin đợt & hợp đồng
    --------------------------------------------------------
    DECLARE 
        @maHD       VARCHAR(15),
        @maKH       VARCHAR(15),
        @ngayDuKien DATE,
        @ngayTraKQ  DATE,
        @thuTuDot   INT;

    SELECT 
        @maHD       = d.maHD,
        @ngayDuKien = d.ngayDuKien,
        @ngayTraKQ  = d.ngayTraKQ
    FROM DotQuanTrac d
    WHERE d.maDot = @maDot;

    -- Không tồn tại đợt hoặc chưa có ngày kết thúc thực tế => không log
    IF @maHD IS NULL OR @ngayTraKQ IS NULL
        RETURN;

    SELECT @maKH = h.maKH
    FROM HopDong h
    WHERE h.maHD = @maHD;

    --------------------------------------------------------
    -- 2. Thứ tự đợt trong hợp đồng
    --------------------------------------------------------
    SELECT @thuTuDot = thuTuDot
    FROM DotQuanTrac
    WHERE maDot = @maDot;

    IF @thuTuDot IS NULL SET @thuTuDot = 1;  -- fallback

    --------------------------------------------------------
    -- 3. Feature ở mức hợp đồng
    --------------------------------------------------------
    DECLARE 
        @thoiHanHopDong_Thang INT,
        @tanSuat_KhongCo      BIT,
        @tanSuat_TheoQuy      BIT,
        @tanSuat_6Thang       BIT,
        @soDot_DuKien         INT,
        @soDot_HoanThanh      INT,
        @tiLeHoanThanh        FLOAT;

    -- Thời hạn hợp đồng (tháng)
    SELECT 
        @thoiHanHopDong_Thang = DATEDIFF(MONTH, h.ngayKy, h.ngayKetThucHD)
    FROM HopDong h
    WHERE h.maHD = @maHD;

    -- One-hot tần suất
    SELECT
        @tanSuat_KhongCo = CASE WHEN h.tanSuatQuanTrac = 'TSQT01' THEN 1 ELSE 0 END,
        @tanSuat_TheoQuy = CASE WHEN h.tanSuatQuanTrac = 'TSQT03' THEN 1 ELSE 0 END,
        @tanSuat_6Thang  = CASE WHEN h.tanSuatQuanTrac = 'TSQT02' THEN 1 ELSE 0 END
    FROM HopDong h
    WHERE h.maHD = @maHD;

    -- Số đợt dự kiến (tổng đợt của hợp đồng)
    DECLARE @soThangMotDot INT;

    SELECT 
        @soThangMotDot = CASE h.tanSuatQuanTrac 
                             WHEN 'TSQT01' THEN NULL  -- không tần suất định kỳ
                             WHEN 'TSQT02' THEN 6     -- 6 tháng/đợt
                             WHEN 'TSQT03' THEN 3     -- 3 tháng/đợt (quý)
                         END
    FROM HopDong h
    WHERE h.maHD = @maHD;

    IF @soThangMotDot IS NULL 
       OR @soThangMotDot = 0 
       OR @thoiHanHopDong_Thang <= 0
    BEGIN
        SET @soDot_DuKien = 1;   -- không có tần suất => 1 đợt
    END
    ELSE
    BEGIN
        SET @soDot_DuKien = CEILING(@thoiHanHopDong_Thang * 1.0 / @soThangMotDot);
    END;

    -- Số đợt hoàn thành thực tế tới thời điểm hiện tại
    SELECT @soDot_HoanThanh = COUNT(*)
    FROM DotQuanTrac d
    WHERE d.maHD = @maHD
      AND d.ngayTraKQ IS NOT NULL
      AND d.ngayTraKQ <= @ngayTraKQ;

    -- Tỉ lệ hoàn thành theo tiến độ (đến đợt hiện tại)
    SET @tiLeHoanThanh = CASE 
                             WHEN @thuTuDot > 0 
                                 THEN CAST(@soDot_HoanThanh AS FLOAT) / @thuTuDot
                             ELSE 0
                         END;

    --------------------------------------------------------
    -- 4. Thống kê trễ hạn
    --------------------------------------------------------
    DECLARE
        @trungBinh_TreHan FLOAT,
        @treHan_ToiDa     INT,
        @treHan_NhoNhat   INT,
        @soDot_BiTre      INT,
        @tiLeDotTre       FLOAT;

    ;WITH Tre AS (
        SELECT 
            Tre = DATEDIFF(DAY, d.ngayDuKien, d.ngayTraKQ)
        FROM DotQuanTrac d
        WHERE d.maHD = @maHD
          AND d.ngayTraKQ IS NOT NULL
          AND d.ngayTraKQ <= @ngayTraKQ
    )
    SELECT
        @trungBinh_TreHan = ISNULL(AVG(CASE WHEN Tre > 0 THEN 1.0 * Tre END), 0),
        @treHan_ToiDa     = ISNULL(MAX(CASE WHEN Tre > 0 THEN Tre END), 0),
        @treHan_NhoNhat   = ISNULL(MIN(CASE WHEN Tre > 0 THEN Tre END), 0),
        @soDot_BiTre      = ISNULL(SUM(CASE WHEN Tre > 0 THEN 1 ELSE 0 END), 0)
    FROM Tre;

    SET @tiLeDotTre = CASE 
                          WHEN @soDot_HoanThanh > 0 
                              THEN CAST(@soDot_BiTre AS FLOAT) / @soDot_HoanThanh
                          ELSE 0
                      END;

    --------------------------------------------------------
    -- 5. Thống kê thời lượng xử lý
    --------------------------------------------------------
    DECLARE
        @trungBinh_ThoiLuongXuLy FLOAT,
        @xuLy_ToiDa              INT,
        @xuLy_NhoNhat            INT;

    ;WITH XuLy AS (
        SELECT 
            XuLyNgay = DATEDIFF(DAY, d.ngayBatDau, d.ngayTraKQ)
        FROM DotQuanTrac d
        WHERE d.maHD = @maHD
          AND d.ngayTraKQ IS NOT NULL
          AND d.ngayTraKQ <= @ngayTraKQ
    )
    SELECT
        @trungBinh_ThoiLuongXuLy = ISNULL(AVG(1.0 * XuLyNgay), 0),
        @xuLy_ToiDa              = ISNULL(MAX(XuLyNgay), 0),
        @xuLy_NhoNhat            = ISNULL(MIN(XuLyNgay), 0)
    FROM XuLy;

    --------------------------------------------------------
    -- 6. Tạo khóa chính maAITaiKy: KH001_HD001_D003
    --------------------------------------------------------
    DECLARE @maAITaiKy VARCHAR(50);

    SET @maAITaiKy = 
        @maKH + '_' + @maHD + '_' + 'D' + RIGHT('000' + CAST(@thuTuDot AS VARCHAR(3)), 3);

    --------------------------------------------------------
    -- 7. Nhãn tiepTuc_HopTac (cho training)
    --------------------------------------------------------
	DECLARE @tiepTuc_HopTac BIT;
	SET @tiepTuc_HopTac = NULL;

    --------------------------------------------------------
    -- 8. Insert vào bảng AI_TaiKy
    --------------------------------------------------------
    IF NOT EXISTS (SELECT 1 FROM AI_TaiKy WHERE maAITaiKy = @maAITaiKy)
    BEGIN
        INSERT INTO AI_TaiKy
        (
            maAITaiKy, maKH, maHD, thuTuDot,
            thoiHanHopDong_Thang,
            tanSuat_KhongCo, tanSuat_TheoQuy, tanSuat_6Thang,
            soDot_DuKien, soDot_HoanThanh_ToiHienTai, tiLeHoanThanh,
            trungBinh_TreHan, treHan_ToiDa, treHan_NhoNhat, soDot_BiTre, tiLeDotTre,
            trungBinh_ThoiLuongXuLy, xuLy_ToiDa, xuLy_NhoNhat,
            tiepTuc_HopTac,
            ngaySnapshot
        )
        VALUES
        (
            @maAITaiKy, @maKH, @maHD, @thuTuDot,
            @thoiHanHopDong_Thang,
            ISNULL(@tanSuat_KhongCo,0), ISNULL(@tanSuat_TheoQuy,0), ISNULL(@tanSuat_6Thang,0),
            @soDot_DuKien, 
            @soDot_HoanThanh, @tiLeHoanThanh,
            @trungBinh_TreHan, @treHan_ToiDa, @treHan_NhoNhat, @soDot_BiTre, @tiLeDotTre,
            @trungBinh_ThoiLuongXuLy, @xuLy_ToiDa, @xuLy_NhoNhat,
            @tiepTuc_HopTac,
            GETDATE()
        );
    END
END
GO
CREATE PROCEDURE dbo.sp_AI_UpdateLabel_KhongHopTac
    @soNgayCho INT = 90  
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.AI_TaiKy
    SET tiepTuc_HopTac = 0
    WHERE tiepTuc_HopTac IS NULL
      AND DATEADD(DAY, @soNgayCho, ngaySnapshot) < GETDATE()
      AND NOT EXISTS (
            SELECT 1 
            FROM dbo.HopDong h
            WHERE h.maKH = AI_TaiKy.maKH
              AND h.ngayKy > AI_TaiKy.ngaySnapshot
      );
END
GO

CREATE PROCEDURE dbo.sp_AI_TaiKy_GetTrainingData
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        maAITaiKy,
        maKH,
        maHD,
        thuTuDot,
        thoiHanHopDong_Thang,
        tanSuat_KhongCo,
        tanSuat_TheoQuy,
        tanSuat_6Thang,
        soDot_DuKien,
        soDot_HoanThanh_ToiHienTai,
        tiLeHoanThanh,
        trungBinh_TreHan,
        treHan_ToiDa,
        treHan_NhoNhat,
        soDot_BiTre,
        tiLeDotTre,
        trungBinh_ThoiLuongXuLy,
        xuLy_ToiDa,
        xuLy_NhoNhat,
        ngaySnapshot,
        tiepTuc_HopTac  -- LABEL
    FROM dbo.AI_TaiKy
    WHERE tiepTuc_HopTac IS NOT NULL
    ORDER BY maKH, thuTuDot;
END
GO


CREATE PROCEDURE dbo.sp_AI_TaiKy_GetPredictData
    @maKH VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        maAITaiKy,
        maKH,
        maHD,
        thuTuDot,
        thoiHanHopDong_Thang,
        tanSuat_KhongCo,
        tanSuat_TheoQuy,
        tanSuat_6Thang,
        soDot_DuKien,
        soDot_HoanThanh_ToiHienTai,
        tiLeHoanThanh,
        trungBinh_TreHan,
        treHan_ToiDa,
        treHan_NhoNhat,
        soDot_BiTre,
        tiLeDotTre,
        trungBinh_ThoiLuongXuLy,
        xuLy_ToiDa,
        xuLy_NhoNhat,
        ngaySnapshot
    FROM dbo.AI_TaiKy
    WHERE maKH = @maKH
      AND tiepTuc_HopTac IS NULL
    ORDER BY ngaySnapshot DESC;
END
GO

CREATE PROCEDURE dbo.sp_LayThongTinChoAI
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        maAITaiKy,
        maKH,
        maHD,
        thuTuDot,
        thoiHanHopDong_Thang,
        tanSuat_KhongCo,
        tanSuat_TheoQuy,
        tanSuat_6Thang,
        soDot_DuKien,
        soDot_HoanThanh_ToiHienTai,
        tiLeHoanThanh,
        trungBinh_TreHan,
        treHan_ToiDa,
        treHan_NhoNhat,
        soDot_BiTre,
        tiLeDotTre,
        trungBinh_ThoiLuongXuLy,
        xuLy_ToiDa,
        xuLy_NhoNhat,
        ngaySnapshot,
        tiepTuc_HopTac  -- LABEL
    FROM dbo.AI_TaiKy
    WHERE tiepTuc_HopTac IS NOT NULL
    ORDER BY maKH, thuTuDot;
END
GO


INSERT INTO AI_TaiKy (
    maAITaiKy, maKH, maHD, thuTuDot,
    thoiHanHopDong_Thang,
    tanSuat_KhongCo, tanSuat_TheoQuy, tanSuat_6Thang,
    soDot_DuKien, soDot_HoanThanh_ToiHienTai, tiLeHoanThanh,
    trungBinh_TreHan, treHan_ToiDa, treHan_NhoNhat, soDot_BiTre, tiLeDotTre,
    trungBinh_ThoiLuongXuLy, xuLy_ToiDa, xuLy_NhoNhat,
    tiepTuc_HopTac, ngaySnapshot
)
VALUES
('KH001_HD001_D001','KH001','HD001',1,12,0,1,0,4,1,0.25,0,0,0,0,0,12,20,7,1,'2025-01-10'),
('KH001_HD001_D002','KH001','HD001',2,12,0,1,0,4,2,0.50,1,3,1,1,0.5,15,25,9,1,'2025-04-12'),
('KH001_HD001_D003','KH001','HD001',3,12,0,1,0,4,3,0.75,2,5,1,1,0.33,18,30,12,1,'2025-07-15'),

('KH002_HD002_D001','KH002','HD002',1,24,0,0,1,4,1,0.25,4,7,3,1,1,20,35,15,0,'2025-03-21'),
('KH002_HD002_D002','KH002','HD002',2,24,0,0,1,4,2,0.50,8,10,5,1,0.5,22,40,16,0,'2025-06-25'),

('KH003_HD003_D001','KH003','HD003',1,6,1,0,0,1,1,1,0,0,0,0,0,10,18,6,1,'2025-05-01'),

('KH004_HD004_D001','KH004','HD004',1,18,0,1,0,6,1,0.16,8,12,3,1,1,20,35,10,0,'2025-03-10'),
('KH004_HD004_D002','KH004','HD004',2,18,0,1,0,6,2,0.33,7,10,2,1,0.5,18,32,9,0,'2025-06-11'),

('KH005_HD005_D001','KH005','HD005',1,12,0,0,1,2,1,0.5,0,0,0,0,0,14,22,7,1,'2025-02-25'),

('KH006_HD006_D001','KH006','HD006',1,36,0,1,0,12,1,0.083,15,20,10,1,1,25,40,18,0,'2025-03-22'),
('KH006_HD006_D002','KH006','HD006',2,36,0,1,0,12,2,0.16,20,25,15,1,0.5,28,45,22,0,'2025-06-24'),

('KH007_HD007_D001','KH007','HD007',1,24,1,0,0,1,1,1,0,0,0,0,0,9,15,5,1,'2025-05-26'),

('KH008_HD008_D001','KH008','HD008',1,12,0,0,1,2,1,0.50,3,6,2,1,1,16,28,9,1,'2025-01-11'),
('KH008_HD008_D002','KH008','HD008',2,12,0,0,1,2,2,1.00,1,3,1,1,0.5,14,20,8,1,'2025-04-12'),

('KH009_HD009_D001','KH009','HD009',1,18,0,1,0,6,1,0.16,10,15,7,1,1,21,32,11,0,'2025-02-14'),

('KH010_HD010_D001','KH010','HD010',1,24,0,0,1,4,1,0.25,12,18,8,1,1,23,38,14,0,'2025-06-10'),

('KH011_HD011_D001','KH011','HD011',1,6,1,0,0,1,1,1,0,0,0,0,0,11,18,6,1,'2025-05-28'),

('KH012_HD012_D001','KH012','HD012',1,12,0,1,0,4,1,0.25,5,8,2,1,1,17,29,10,1,'2025-03-23'),

('KH013_HD013_D001','KH013','HD013',1,18,0,1,0,6,1,0.16,8,11,4,1,1,19,30,12,0,'2025-02-15'),

('KH014_HD014_D001','KH014','HD014',1,12,0,0,1,2,1,0.5,2,4,1,1,1,13,21,8,1,'2025-04-28'),

('KH015_HD015_D001','KH015','HD015',1,24,0,1,0,8,1,0.12,15,22,10,1,1,24,37,15,0,'2025-01-12'),

('KH016_HD016_D001','KH016','HD016',1,12,1,0,0,1,1,1,0,0,0,0,0,10,19,7,1,'2025-02-15'),

('KH017_HD017_D001','KH017','HD017',1,6,1,0,0,1,1,1,0,0,0,0,0,12,21,8,1,'2025-05-03'),

('KH018_HD018_D001','KH018','HD018',1,18,0,1,0,6,1,0.16,9,14,5,1,1,20,33,12,0,'2025-03-18'),

('KH019_HD019_D001','KH019','HD019',1,24,0,0,1,4,1,0.25,11,17,7,1,1,22,36,15,0,'2025-04-11'),

('KH020_HD020_D001','KH020','HD020',1,12,0,0,1,2,1,0.5,4,7,2,1,1,16,27,9,1,'2025-03-29');

INSERT INTO AI_TaiKy (
    maAITaiKy, maKH, maHD, thuTuDot,
    thoiHanHopDong_Thang,
    tanSuat_KhongCo, tanSuat_TheoQuy, tanSuat_6Thang,
    soDot_DuKien, soDot_HoanThanh_ToiHienTai, tiLeHoanThanh,
    trungBinh_TreHan, treHan_ToiDa, treHan_NhoNhat, soDot_BiTre, tiLeDotTre,
    trungBinh_ThoiLuongXuLy, xuLy_ToiDa, xuLy_NhoNhat,
    tiepTuc_HopTac, ngaySnapshot
)
VALUES
-- KH021: HĐ 12 tháng, tần suất 6 tháng, hoàn thành đủ, ít trễ -> tiếp tục (1)
('KH021_HD021_D001','KH021','HD021',1,12,0,0,1,2,2,1.00,1,3,0,1,0.50,14,22,7,1,'2025-01-20'),

-- KH022: 24 tháng, theo quý, hoàn thành 4/8, trễ nhiều -> nghỉ (0)
('KH022_HD022_D001','KH022','HD022',1,24,0,1,0,8,4,0.50,6,10,2,3,0.75,21,34,13,0,'2025-02-18'),

-- KH023: 6 tháng, không định kỳ, 1 đợt, không trễ -> tiếp tục (1)
('KH023_HD023_D001','KH023','HD023',1,6,1,0,0,1,1,1.00,0,0,0,0,0.00,9,15,5,1,'2025-03-05'),

-- KH024: 18 tháng, 6 tháng/lần, hoàn thành 2/3, trễ nhẹ -> tiếp tục (1)
('KH024_HD024_D001','KH024','HD024',1,18,0,0,1,3,2,0.67,3,6,1,1,0.50,17,28,9,1,'2025-03-22'),

-- KH025: 18 tháng, theo quý, hoàn thành thấp + trễ nhiều -> nghỉ (0)
('KH025_HD025_D001','KH025','HD025',1,18,0,1,0,6,2,0.33,9,14,4,2,1.00,23,36,15,0,'2025-04-10'),

-- KH026: 12 tháng, không định kỳ nhưng bị trễ nặng + xử lý lâu -> nghỉ (0)
('KH026_HD026_D001','KH026','HD026',1,12,1,0,0,1,1,1.00,12,18,7,1,1.00,26,39,18,0,'2025-04-28'),

-- KH027: 24 tháng, 6 tháng/lần, hoàn thành đủ 4 đợt, trễ ít -> tiếp tục (1)
('KH027_HD027_D001','KH027','HD027',1,24,0,0,1,4,4,1.00,2,5,0,1,0.25,16,27,10,1,'2025-05-03'),

-- KH028: 36 tháng, theo quý, hoàn thành 6/12, trễ nhiều -> nghỉ (0)
('KH028_HD028_D001','KH028','HD028',1,36,0,1,0,12,6,0.50,10,18,5,4,0.67,24,40,17,0,'2025-05-25'),

-- KH029: 6 tháng, không định kỳ, không trễ -> tiếp tục (1)
('KH029_HD029_D001','KH029','HD029',1,6,1,0,0,1,1,1.00,0,0,0,0,0.00,11,18,6,1,'2025-06-02'),

-- KH030: 12 tháng, 6 tháng/lần, mới hoàn thành 1/2 đợt, trễ khá -> nghỉ (0)
('KH030_HD030_D001','KH030','HD030',1,12,0,0,1,2,1,0.50,7,11,3,1,1.00,19,30,12,0,'2025-06-12'),

-- KH031: 24 tháng, theo quý, 5/8 đợt, trễ nhẹ -> tiếp tục (1)
('KH031_HD031_D001','KH031','HD031',1,24,0,1,0,8,5,0.63,3,7,1,2,0.40,18,29,11,1,'2025-06-28'),

-- KH032: 18 tháng, không định kỳ, 1 đợt nhưng trễ kha khá, vẫn giữ (noise +) -> tiếp tục (1)
('KH032_HD032_D001','KH032','HD032',1,18,1,0,0,1,1,1.00,8,13,4,1,1.00,22,34,14,1,'2025-07-05'),

-- KH033: 12 tháng, theo quý, 3/4 đợt, trễ vừa -> tiếp tục (1)
('KH033_HD033_D001','KH033','HD033',1,12,0,1,0,4,3,0.75,4,8,1,2,0.67,17,26,9,1,'2025-07-18'),

-- KH034: 24 tháng, 6 tháng/lần, mới 2/4 đợt, trễ nhiều -> nghỉ (0)
('KH034_HD034_D001','KH034','HD034',1,24,0,0,1,4,2,0.50,11,17,6,2,1.00,25,39,18,0,'2025-08-01'),

-- KH035: 6 tháng, không định kỳ, trễ nhẹ nhưng 1 đợt duy nhất -> tiếp tục (1)
('KH035_HD035_D001','KH035','HD035',1,6,1,0,0,1,1,1.00,1,3,0,1,1.00,12,19,7,1,'2025-08-09'),

-- KH036: 18 tháng, theo quý, mới xong 1/6 đợt, trễ nhiều -> nghỉ (0)
('KH036_HD036_D001','KH036','HD036',1,18,0,1,0,6,1,0.17,6,10,3,1,1.00,20,31,12,0,'2025-08-20'),

-- KH037: 24 tháng, không định kỳ, không trễ -> tiếp tục (1)
('KH037_HD037_D001','KH037','HD037',1,24,1,0,0,1,1,1.00,0,0,0,0,0.00,13,21,8,1,'2025-09-02'),

-- KH038: 36 tháng, 6 tháng/lần, 3/6 đợt, trễ rõ -> nghỉ (0)
('KH038_HD038_D001','KH038','HD038',1,36,0,0,1,6,3,0.50,9,14,4,2,0.67,23,37,16,0,'2025-09-15'),

-- KH039: 12 tháng, 6 tháng/lần, đủ 2/2, không trễ -> tiếp tục (1)
('KH039_HD039_D001','KH039','HD039',1,12,0,0,1,2,2,1.00,0,0,0,0,0.00,15,23,8,1,'2025-09-28'),

-- KH040: 18 tháng, theo quý, 4/6 đợt, trễ nhẹ -> tiếp tục (1)
('KH040_HD040_D001','KH040','HD040',1,18,0,1,0,6,4,0.67,3,6,1,1,0.25,19,30,11,1,'2025-10-05');

INSERT INTO AI_TaiKy (
    maAITaiKy, maKH, maHD, thuTuDot,
    thoiHanHopDong_Thang,
    tanSuat_KhongCo, tanSuat_TheoQuy, tanSuat_6Thang,
    soDot_DuKien, soDot_HoanThanh_ToiHienTai, tiLeHoanThanh,
    trungBinh_TreHan, treHan_ToiDa, treHan_NhoNhat, soDot_BiTre, tiLeDotTre,
    trungBinh_ThoiLuongXuLy, xuLy_ToiDa, xuLy_NhoNhat,
    tiepTuc_HopTac, ngaySnapshot
)
VALUES
-- KH041: 12 tháng, theo quý, đủ 4/4, không trễ -> tiếp tục
('KH041_HD041_D001','KH041','HD041',1,12,0,1,0,4,4,1.00,0,0,0,0,0.00,14,22,8,1,'2025-07-10'),

-- KH042: 24 tháng, 6 tháng/lần, 2/4, trễ nhiều -> nghỉ
('KH042_HD042_D001','KH042','HD042',1,24,0,0,1,4,2,0.50,9,15,4,2,1.00,23,36,15,0,'2025-07-18'),

-- KH043: 6 tháng, không định kỳ, 1 đợt, trễ nhẹ -> tiếp tục
('KH043_HD043_D001','KH043','HD043',1,6,1,0,0,1,1,1.00,2,4,0,1,1.00,11,19,7,1,'2025-07-22'),

-- KH044: 18 tháng, theo quý, 3/6, trễ khá -> nghỉ
('KH044_HD044_D001','KH044','HD044',1,18,0,1,0,6,3,0.50,7,12,3,2,0.67,21,33,13,0,'2025-08-01'),

-- KH045: 18 tháng, 6 tháng/lần, 3/3, trễ ít -> tiếp tục
('KH045_HD045_D001','KH045','HD045',1,18,0,0,1,3,3,1.00,3,6,1,1,0.33,17,27,10,1,'2025-08-05'),

-- KH046: 12 tháng, không định kỳ, 1 đợt, trễ nặng + xử lý lâu -> nghỉ
('KH046_HD046_D001','KH046','HD046',1,12,1,0,0,1,1,1.00,13,19,8,1,1.00,27,40,18,0,'2025-08-09'),

-- KH047: 24 tháng, theo quý, 6/8, trễ nhẹ -> tiếp tục
('KH047_HD047_D001','KH047','HD047',1,24,0,1,0,8,6,0.75,4,8,1,2,0.33,19,30,11,1,'2025-08-15'),

-- KH048: 36 tháng, 6 tháng/lần, 2/6, trễ nhiều -> nghỉ
('KH048_HD048_D001','KH048','HD048',1,36,0,0,1,6,2,0.33,11,17,5,2,1.00,25,39,17,0,'2025-08-20'),

-- KH049: 6 tháng, không định kỳ, không trễ -> tiếp tục
('KH049_HD049_D001','KH049','HD049',1,6,1,0,0,1,1,1.00,0,0,0,0,0.00,10,17,6,1,'2025-08-25'),

-- KH050: 12 tháng, 6 tháng/lần, 1/2, trễ nhiều -> nghỉ
('KH050_HD050_D001','KH050','HD050',1,12,0,0,1,2,1,0.50,8,13,4,1,1.00,20,31,12,0,'2025-08-30'),

-- KH051: 24 tháng, không định kỳ, 1 đợt, không trễ -> tiếp tục
('KH051_HD051_D001','KH051','HD051',1,24,1,0,0,1,1,1.00,0,0,0,0,0.00,13,21,8,1,'2025-09-03'),

-- KH052: 18 tháng, theo quý, 2/6, trễ cao -> nghỉ
('KH052_HD052_D001','KH052','HD052',1,18,0,1,0,6,2,0.33,9,15,4,2,1.00,22,35,14,0,'2025-09-07'),

-- KH053: 18 tháng, theo quý, 5/6, trễ nhẹ -> tiếp tục
('KH053_HD053_D001','KH053','HD053',1,18,0,1,0,6,5,0.83,2,5,0,1,0.20,18,28,9,1,'2025-09-12'),

-- KH054: 24 tháng, 6 tháng/lần, 3/4, trễ vừa -> tiếp tục
('KH054_HD054_D001','KH054','HD054',1,24,0,0,1,4,3,0.75,5,9,2,1,0.33,19,29,11,1,'2025-09-16'),

-- KH055: 36 tháng, theo quý, 4/12, trễ nhiều -> nghỉ
('KH055_HD055_D001','KH055','HD055',1,36,0,1,0,12,4,0.33,12,19,6,3,0.75,26,41,18,0,'2025-09-20'),

-- KH056: 12 tháng, theo quý, 2/4, trễ nhẹ -> tiếp tục
('KH056_HD056_D001','KH056','HD056',1,12,0,1,0,4,2,0.50,3,6,1,1,0.50,16,25,9,1,'2025-09-24'),

-- KH057: 24 tháng, không định kỳ, 1 đợt, trễ nặng -> nghỉ
('KH057_HD057_D001','KH057','HD057',1,24,1,0,0,1,1,1.00,14,20,9,1,1.00,28,42,19,0,'2025-09-28'),

-- KH058: 18 tháng, 6 tháng/lần, 2/3, trễ ít -> tiếp tục
('KH058_HD058_D001','KH058','HD058',1,18,0,0,1,3,2,0.67,2,5,0,1,0.50,17,26,9,1,'2025-10-02'),

-- KH059: 18 tháng, 6 tháng/lần, 1/3, trễ nhiều -> nghỉ
('KH059_HD059_D001','KH059','HD059',1,18,0,0,1,3,1,0.33,10,16,4,1,1.00,23,35,15,0,'2025-10-05'),

-- KH060: 6 tháng, không định kỳ, 1 đợt, trễ nhẹ nhưng xử lý nhanh -> tiếp tục
('KH060_HD060_D001','KH060','HD060',1,6,1,0,0,1,1,1.00,2,4,0,1,1.00,9,14,5,1,'2025-10-07'),

-- KH061: 12 tháng, 6 tháng/lần, 2/2, không trễ -> tiếp tục
('KH061_HD061_D001','KH061','HD061',1,12,0,0,1,2,2,1.00,0,0,0,0,0.00,14,22,8,1,'2025-10-10'),

-- KH062: 24 tháng, theo quý, 2/8, trễ nhiều -> nghỉ
('KH062_HD062_D001','KH062','HD062',1,24,0,1,0,8,2,0.25,11,18,5,2,1.00,24,38,17,0,'2025-10-13'),

-- KH063: 24 tháng, theo quý, 7/8, trễ nhẹ -> tiếp tục
('KH063_HD063_D001','KH063','HD063',1,24,0,1,0,8,7,0.88,3,7,1,1,0.14,19,29,11,1,'2025-10-16'),

-- KH064: 36 tháng, 6 tháng/lần, 5/6, trễ vừa -> tiếp tục
('KH064_HD064_D001','KH064','HD064',1,36,0,0,1,6,5,0.83,4,8,1,2,0.40,21,32,12,1,'2025-10-19'),

-- KH065: 36 tháng, theo quý, 3/12, trễ nặng -> nghỉ
('KH065_HD065_D001','KH065','HD065',1,36,0,1,0,12,3,0.25,13,20,7,2,0.67,27,43,19,0,'2025-10-22'),

-- KH066: 12 tháng, không định kỳ, không trễ -> tiếp tục
('KH066_HD066_D001','KH066','HD066',1,12,1,0,0,1,1,1.00,0,0,0,0,0.00,11,18,7,1,'2025-10-24'),

-- KH067: 18 tháng, theo quý, 4/6, trễ nhẹ -> tiếp tục
('KH067_HD067_D001','KH067','HD067',1,18,0,1,0,6,4,0.67,3,6,1,1,0.25,18,27,10,1,'2025-10-26'),

-- KH068: 18 tháng, theo quý, 1/6, trễ nhiều -> nghỉ
('KH068_HD068_D001','KH068','HD068',1,18,0,1,0,6,1,0.17,10,16,4,1,1.00,22,34,14,0,'2025-10-28'),

-- KH069: 24 tháng, 6 tháng/lần, 4/4, trễ nhẹ -> tiếp tục
('KH069_HD069_D001','KH069','HD069',1,24,0,0,1,4,4,1.00,2,5,0,1,0.25,18,28,10,1,'2025-10-30'),

-- KH070: 6 tháng, không định kỳ, 1 đợt, trễ vừa + xử lý lâu -> nghỉ
('KH070_HD070_D001','KH070','HD070',1,6,1,0,0,1,1,1.00,7,11,3,1,1.00,20,29,11,0,'2025-11-01');


-- Ví dụ: tạm cho 1 dòng chưa biết kết quả thực tế
UPDATE AI_TaiKy
SET tiepTuc_HopTac = NULL,
    duBao_TiepTuc  = NULL,
    duBao_Label    = NULL
WHERE maAITaiKy = 'KH070_HD070_D001';
go



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[ThemPhienChatMoi]
    @tenTK        VARCHAR(50),
    @tenPhienChat NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE tenTK = @tenTK)
            RAISERROR(N'Tên tài khoản không tồn tại.', 16, 1);

        INSERT INTO dbo.ChatSession (TenTK, TenPhienChat)
        VALUES (@tenTK, @tenPhienChat);

        -- ✅ Trả về MaPhien mới
        SELECT CAST(SCOPE_IDENTITY() AS INT) AS MaPhienMoi;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @ErrMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum   INT            = ERROR_NUMBER();
        DECLARE @ErrState INT            = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[SuaTenPhienChat]
    @maPhien        INT,
    @tenPhienChat   NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF (@maPhien IS NULL OR @maPhien <= 0)
            RAISERROR(N'Mã lịch sử không hợp lệ.', 16, 1);

        IF (@tenPhienChat IS NULL OR @tenPhienChat = N'')
            RAISERROR(N'Tên lịch sử chat không được để trống.', 16, 1);

        UPDATE dbo.ChatSession
        SET TenPhienChat = @tenPhienChat,
            UpdatedAt    = GETDATE()
        WHERE MaPhien = @maPhien;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @ErrMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum   INT            = ERROR_NUMBER();
        DECLARE @ErrState INT            = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[XoaPhienChat]
    @maPhien INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF (@maPhien IS NULL OR @maPhien <= 0)
            RAISERROR(N'Mã lịch sử không hợp lệ.', 16, 1);

        UPDATE dbo.ChatSession
        SET DaXoa    = 1,
            UpdatedAt = GETDATE()
        WHERE MaPhien = @maPhien;

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @ErrMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum   INT            = ERROR_NUMBER();
        DECLARE @ErrState INT            = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[LayPhienTheoTenTK]
    @tenTK VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Nếu muốn báo lỗi khi tài khoản không tồn tại:
        IF NOT EXISTS (SELECT 1 FROM dbo.TaiKhoan WHERE tenTK = @tenTK)
            RAISERROR(N'Tên tài khoản không tồn tại.', 16, 1);

        -- Nếu tài khoản tồn tại nhưng chưa có phiên chat thì trả về 0 dòng, KHÔNG RAISERROR
        SELECT 
            cs.MaPhien,
            cs.TenPhienChat,
            cs.CreatedAt,
            cs.UpdatedAt
        FROM dbo.ChatSession AS cs
        WHERE cs.TenTK = @tenTK 
          AND cs.DaXoa = 0
        ORDER BY cs.UpdatedAt DESC;

        -- Không cần COMMIT TRAN vì chỉ SELECT, không mở TRAN
    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum   INT            = ERROR_NUMBER();
        DECLARE @ErrState INT            = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[ThemTinNhan]
    @maPhien     INT,
    @vaiTroGui   VARCHAR(20),
    @tenNguoiGui NVARCHAR(100),
    @noiDung     NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRAN;

        -- Kiểm tra phiên có tồn tại không
        IF NOT EXISTS (SELECT 1 FROM dbo.ChatSession WHERE MaPhien = @maPhien AND DaXoa = 0)
            RAISERROR(N'Phiên chat không tồn tại hoặc đã bị xóa.', 16, 1);

        -- Tính thứ tự tiếp theo
        DECLARE @nextThuTu INT;
        SELECT @nextThuTu = ISNULL(MAX(ThuTu), 0) + 1
        FROM dbo.ChatMessage
        WHERE MaPhien = @maPhien;

        INSERT INTO dbo.ChatMessage (MaPhien, ThuTu, VaiTroGui, TenNguoiGui, NoiDung)
        VALUES (@maPhien, @nextThuTu, @vaiTroGui, @tenNguoiGui, @noiDung);

        COMMIT TRAN;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRAN;

        DECLARE @ErrMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum   INT            = ERROR_NUMBER();
        DECLARE @ErrState INT            = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[LayTinNhanTheoPhien]
    @maPhien INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        -- Nếu muốn, bạn có thể check phiên có tồn tại không, nhưng thường không cần RAISERROR.
        -- IF NOT EXISTS (SELECT 1 FROM dbo.ChatSession WHERE MaPhien = @maPhien AND DaXoa = 0)
        --     RAISERROR(N'Phiên chat không tồn tại hoặc đã bị xóa.', 16, 1);

        SELECT 
            cm.MaTinNhan,
            cm.MaPhien,
            cm.ThuTu,
            cm.VaiTroGui,
            cm.TenNguoiGui,
            cm.NoiDung,
            cm.ThoiGianTao
        FROM dbo.ChatMessage AS cm
        WHERE cm.MaPhien = @maPhien
        ORDER BY cm.ThuTu ASC, cm.MaTinNhan ASC;   -- đảm bảo đúng thứ tự

    END TRY
    BEGIN CATCH
        DECLARE @ErrMsg   NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrNum   INT            = ERROR_NUMBER();
        DECLARE @ErrState INT            = ERROR_STATE();
        THROW @ErrNum, @ErrMsg, @ErrState;
    END CATCH
END
GO



 --Sửa mô tả nền mẫu 
CREATE OR ALTER PROCEDURE [dbo].[sp_SuaMoTaNenMau]
    @maNen VARCHAR(15),
    @moTa NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE NenMau
        SET moTa = @moTa
        WHERE maNen = @maNen;
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
select * from AI_TaiKy

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[sp_SinhThongBaoSapDenHan_DotQuanTrac]
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @NgayHienTai DATE = CAST(GETDATE() AS DATE);
    DECLARE @NgayThongBao DATE;
    DECLARE @maDot VARCHAR(15),
            @maHD VARCHAR(15),
            @tenKH NVARCHAR(255),
            @ngayDuKien DATE,
            @NewMaTB VARCHAR(15);

    DECLARE cur CURSOR FOR
    SELECT dq.maDot, dq.maHD, kh.tenDoanhNghiep, dq.ngayDuKien
    FROM DotQuanTrac dq
    JOIN HopDong hd ON dq.maHD = hd.maHD
    JOIN KhachHang kh ON hd.maKH = kh.maKH
    WHERE dq.trangThai <> 6
      AND dq.ngayDuKien IS NOT NULL
      AND DATEADD(DAY, -2, dq.ngayDuKien) = @NgayHienTai
      AND NOT EXISTS (
            SELECT 1 FROM ThongBao tb
            WHERE tb.maDot = dq.maDot
              AND tb.loaiTB = 'NHAC_SAP_DEN_HAN_DOT'
      );

    OPEN cur;
    FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @NewMaTB =
            'TB' + RIGHT('000000' + CAST(ISNULL(MAX(CAST(SUBSTRING(maTB, 3, 6) AS INT)), 0) + 1 AS VARCHAR(6)), 6)
        FROM ThongBao;

        INSERT INTO ThongBao (maTB, loaiTB, maDot, maHD, tieuDe, noiDung, ngayTao)
        VALUES (
            @NewMaTB,
            'NHAC_SAP_DEN_HAN_DOT',
            @maDot,
            @maHD,
            N'Nhắc sắp đến hạn trả kết quả cho đợt ' + @maDot,
            N'Khách hàng: ' + @tenKH
            + N'. Ngày dự kiến trả kết quả: ' + CONVERT(VARCHAR(10), @ngayDuKien, 103)
            + N'. Hệ thống gửi nhắc nhở trước 2 ngày.',
            GETDATE()
        );

        INSERT INTO ThongBao_NguoiDung(maTB, maNV, trangThaiDoc, ngayDoc)
        SELECT @NewMaTB, nv.maNV, 0, NULL
        FROM NhanVien nv
        WHERE nv.trangThai = 1;

        FETCH NEXT FROM cur INTO @maDot, @maHD, @tenKH, @ngayDuKien;
    END;

    CLOSE cur;
    DEALLOCATE cur;
END;
GO
-- sửa 
-- ============================================
-- VIEW 1: vw_KetQuaHeader_FromKetQua
-- Mục đích: Hiển thị danh sách kết quả trên DSKQUC
-- ============================================
CREATE OR ALTER VIEW vw_KetQuaHeader_FromKetQua AS
SELECT 
    -- Tạo maKQ tổng hợp theo đợt (vì KetQua có nhiều maKQ cho 1 đợt)
    dn.maDot AS maKQ,  -- Dùng maDot làm maKQ tổng hợp
    
    -- Ngày tạo: Lấy ngày đo sớm nhất của đợt
    MIN(kq.ngayDo) AS ngayTao,
    
    -- Ngày trả KQ: Từ DotQuanTrac
    dqt.ngayTraKQ,
    
    -- Người nhập: Lấy người nhập đầu tiên (hoặc có thể lấy người cuối)
    (SELECT TOP 1 nv.hoTen 
     FROM KetQua kq2 
     INNER JOIN NhanVien nv ON kq2.nhanVienNhap = nv.maNV
     WHERE kq2.maDNTS IN (
         SELECT maDNTS FROM Dot_Nen_Ts WHERE maDN IN (
             SELECT maDN FROM Dot_Nen WHERE maDot = dn.maDot
         )
     )
     ORDER BY kq2.ngayDo ASC
    ) AS NguoiNhap,
    
    -- ✅ SỬA: Trạng thái xác nhận - ĐỌC TỪ DotQuanTrac.trangThai
    CASE 
        WHEN dqt.trangThai = 3 THEN 1  -- Hoàn thành = Đã xác nhận
        ELSE 0  -- Các trạng thái khác = Chưa xác nhận
    END AS trangThaiXacNhan,
    
    -- ✅ SỬA: Text trạng thái
    CASE 
        WHEN dqt.trangThai = 3 THEN N'Đã xác nhận'
        ELSE N'Chờ xác nhận'
    END AS TrangThai,
    
    -- Ghi chú: Từ DotQuanTrac
    dqt.noiDung AS ghiChu,
    
    -- Thông tin đợt
    dqt.dotQuanTrac,
    dqt.maDot,
    
    -- Số nền mẫu
    COUNT(DISTINCT dn.maNen) AS SoNenMau,
    
    -- Thông tin khách hàng
    kh.tenDoanhNghiep AS TenKhachHang,
    kh.emailDoanhNghiep AS EmailKhachHang,
    kh.diaChi AS DiaChiKhachHang,
    dqt.noiDung AS DiaDiemQuanTrac
    
FROM Dot_Nen dn
INNER JOIN DotQuanTrac dqt ON dn.maDot = dqt.maDot
LEFT JOIN HopDong hd ON dqt.maHD = hd.maHD
LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH
LEFT JOIN Dot_Nen_Ts dnts ON dn.maDN = dnts.maDN
LEFT JOIN KetQua kq ON dnts.maDNTS = kq.maDNTS

-- Chỉ lấy các đợt có ít nhất 1 kết quả đã nhập
WHERE EXISTS (
    SELECT 1 
    FROM Dot_Nen_Ts dnts2
    INNER JOIN KetQua kq2 ON dnts2.maDNTS = kq2.maDNTS
    WHERE dnts2.maDN IN (SELECT maDN FROM Dot_Nen WHERE maDot = dn.maDot)
)

GROUP BY 
    dn.maDot, 
    dqt.ngayTraKQ, 
    dqt.noiDung, 
    dqt.dotQuanTrac, 
    dqt.maDot,
    dqt.trangThai,  -- ✅ THÊM VÀO GROUP BY
    kh.tenDoanhNghiep,
    kh.emailDoanhNghiep,
    kh.diaChi;
GO
-- ============================================
-- VIEW 2: vw_KetQuaNenMau_FromKetQua
-- Mục đích: Hiển thị các nền mẫu của kết quả
-- ============================================
CREATE OR ALTER VIEW vw_KetQuaNenMau_FromKetQua AS
SELECT 
    -- Tạo maKQNen từ maDN
    dn.maDN AS maKQNen,
    
    -- maKQ tương ứng (là maDot)
    dn.maDot AS maKQ,
    
    -- Thông tin nền mẫu
    dn.maNen,
    nm.tenNenMau,
    
    -- Thông tin vị trí
    dn.tenViTri AS viTri,
    dn.toaDo
    
FROM Dot_Nen dn
INNER JOIN NenMau nm ON dn.maNen = nm.maNen

-- Chỉ lấy các nền mẫu có ít nhất 1 thông số đã nhập
WHERE EXISTS (
    SELECT 1 
    FROM Dot_Nen_Ts dnts
    INNER JOIN KetQua kq ON dnts.maDNTS = kq.maDNTS
    WHERE dnts.maDN = dn.maDN
);
GO
-- ============================================
-- Sửa SP: sp_LayDanhSachKetQua
-- ============================================
CREATE OR ALTER PROCEDURE sp_LayDanhSachKetQua
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        maKQ,
        ngayTao,
        ngayTraKQ,
        NguoiNhap,
        TrangThai,
        ghiChu,
        dotQuanTrac,
        maDot,
        TenKhachHang,
        EmailKhachHang,
        DiaChiKhachHang,
        SoNenMau
    FROM vw_KetQuaHeader_FromKetQua
    ORDER BY ngayTao DESC;
END
GO
-- ============================================
-- Sửa SP: LayDanhSachKetQua_PhanTrang
-- ============================================
CREATE OR ALTER PROCEDURE LayDanhSachKetQua_PhanTrang
    @PageNumber INT,
    @PageSize INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        maKQ,
        ngayTao,
        ngayTraKQ,
        NguoiNhap AS TenNhanVien,
        TrangThai,
        ghiChu,
        dotQuanTrac,
        TenKhachHang
    FROM vw_KetQuaHeader_FromKetQua
    ORDER BY ngayTao DESC
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
-- ============================================
-- Sửa SP: DemTongSoKetQua
-- ============================================
CREATE OR ALTER PROCEDURE DemTongSoKetQua
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(DISTINCT maKQ) 
    FROM vw_KetQuaHeader_FromKetQua;
END
GO
-- ============================================
-- Sửa SP: sp_LayChiTietKetQuaTheoMaKQ
-- ============================================
CREATE OR ALTER PROCEDURE sp_LayChiTietKetQuaTheoMaKQ
    @maKQ VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- =============================================
    -- DEBUG: Kiểm tra dữ liệu có tồn tại không
    -- =============================================
    DECLARE @SoNenMau INT = 0;
    DECLARE @SoThongSo INT = 0;
    
    SELECT @SoNenMau = COUNT(DISTINCT nm.maKQNen)
    FROM vw_KetQuaNenMau_FromKetQua nm
    WHERE nm.maKQ = @maKQ;
    
    SELECT @SoThongSo = COUNT(*)
    FROM vw_KetQuaNenMau_FromKetQua nm
    INNER JOIN vw_KetQuaChiTiet_FromKetQua ct ON nm.maKQNen = ct.maKQNen
    WHERE nm.maKQ = @maKQ;
    
    -- In ra log để debug
    PRINT 'MaKQ: ' + @maKQ;
    PRINT 'So nen mau: ' + CAST(@SoNenMau AS VARCHAR);
    PRINT 'So thong so: ' + CAST(@SoThongSo AS VARCHAR);
    
    -- =============================================
    -- RESULTSET 1: Header (CHỈ 1 DÒNG)
    -- =============================================
    SELECT TOP 1
        h.maKQ,
        h.ngayTao,
        h.ngayTraKQ,
        h.NguoiNhap,
        h.trangThaiXacNhan,
        h.ghiChu,
        h.dotQuanTrac,
        h.maDot,
        h.TenKhachHang,
        h.EmailKhachHang,
        h.DiaChiKhachHang,
        h.DiaDiemQuanTrac,
        -- Placeholder
        CAST(NULL AS VARCHAR(15)) AS maKQNen,
        CAST(NULL AS VARCHAR(15)) AS maNen,
        CAST(NULL AS NVARCHAR(100)) AS tenNenMau,
        CAST(NULL AS NVARCHAR(200)) AS viTri,
        CAST(NULL AS NVARCHAR(100)) AS toaDo,
        CAST(NULL AS VARCHAR(15)) AS maKQCT,
        CAST(NULL AS VARCHAR(15)) AS maTS,
        CAST(NULL AS NVARCHAR(30)) AS tenTS,
        CAST(NULL AS NVARCHAR(15)) AS donVi,
        CAST(NULL AS NVARCHAR(200)) AS phuongPhapPhanTich,
        CAST(NULL AS FLOAT) AS ketQua,
        CAST(NULL AS NVARCHAR(50)) AS gioiHanPhatHien,
        CAST(NULL AS NVARCHAR(50)) AS qcvn,
        CAST(NULL AS NVARCHAR(50)) AS TinhTrang
    FROM vw_KetQuaHeader_FromKetQua h
    WHERE h.maKQ = @maKQ;
    
    -- =============================================
    -- RESULTSET 2: Nền mẫu + Chi tiết (NHIỀU DÒNG)
    -- =============================================
    SELECT 
        h.maKQ,
        h.ngayTao,
        h.ngayTraKQ,
        h.NguoiNhap,
        h.trangThaiXacNhan,
        h.ghiChu,
        h.dotQuanTrac,
        h.maDot,
        h.TenKhachHang,
        h.EmailKhachHang,
        h.DiaChiKhachHang,
        h.DiaDiemQuanTrac,
        -- Thông tin nền mẫu
        nm.maKQNen,
        nm.maNen,
        nm.tenNenMau,
        nm.viTri,
        nm.toaDo,
        -- Thông tin chi tiết thông số
        ct.maKQCT,
        ct.maTS,
        ct.tenTS,
        ct.donVi,
        ct.phuongPhapPhanTich,
        ct.ketQua,
        ct.gioiHanPhatHien,
        ct.qcvn,
        ct.TinhTrang
    FROM vw_KetQuaHeader_FromKetQua h
    INNER JOIN vw_KetQuaNenMau_FromKetQua nm ON h.maKQ = nm.maKQ
    LEFT JOIN vw_KetQuaChiTiet_FromKetQua ct ON nm.maKQNen = ct.maKQNen
    WHERE h.maKQ = @maKQ
    ORDER BY nm.maNen, nm.tenNenMau, ct.tenTS;
END

GO
-- ============================================
-- VIEW 1: vw_KetQuaHeader_FromKetQua
-- Mục đích: Hiển thị danh sách kết quả trên DSKQUC
-- ============================================
CREATE OR ALTER VIEW vw_KetQuaHeader_FromKetQua AS
SELECT 
    dn.maDot AS maKQ,
    MIN(kq.ngayDo) AS ngayTao,
    dqt.ngayTraKQ,
    
    (SELECT TOP 1 nv.hoTen 
     FROM KetQua kq2 
     INNER JOIN NhanVien nv ON kq2.nhanVienNhap = nv.maNV
     WHERE kq2.maDNTS IN (
         SELECT maDNTS FROM Dot_Nen_Ts WHERE maDN IN (
             SELECT maDN FROM Dot_Nen WHERE maDot = dn.maDot
         )
     )
     ORDER BY kq2.ngayDo ASC
    ) AS NguoiNhap,
    
    -- ✅ ĐỌC TRẠNG THÁI TỪ DotQuanTrac.trangThai
    -- trangThai = 3: Hoàn thành = Đã xác nhận
    -- trangThai khác: Chờ xác nhận
    CASE 
        WHEN dqt.trangThai = 3 THEN 1  
        ELSE 0  
    END AS trangThaiXacNhan,
    
    CASE 
        WHEN dqt.trangThai = 3 THEN N'Đã xác nhận'
        ELSE N'Chờ xác nhận'
    END AS TrangThai,
    
    dqt.noiDung AS ghiChu,
    dqt.dotQuanTrac,
    dqt.maDot,
    COUNT(DISTINCT dn.maNen) AS SoNenMau,
    kh.tenDoanhNghiep AS TenKhachHang,
    kh.emailDoanhNghiep AS EmailKhachHang,
    kh.diaChi AS DiaChiKhachHang,
    dqt.noiDung AS DiaDiemQuanTrac
    
FROM Dot_Nen dn
INNER JOIN DotQuanTrac dqt ON dn.maDot = dqt.maDot
LEFT JOIN HopDong hd ON dqt.maHD = hd.maHD
LEFT JOIN KhachHang kh ON hd.maKH = kh.maKH
LEFT JOIN Dot_Nen_Ts dnts ON dn.maDN = dnts.maDN
LEFT JOIN KetQua kq ON dnts.maDNTS = kq.maDNTS

WHERE EXISTS (
    SELECT 1 
    FROM Dot_Nen_Ts dnts2
    INNER JOIN KetQua kq2 ON dnts2.maDNTS = kq2.maDNTS
    WHERE dnts2.maDN IN (SELECT maDN FROM Dot_Nen WHERE maDot = dn.maDot)
)

GROUP BY 
    dn.maDot, 
    dqt.ngayTraKQ, 
    dqt.noiDung, 
    dqt.dotQuanTrac, 
    dqt.maDot,
    dqt.trangThai,  
    kh.tenDoanhNghiep,
    kh.emailDoanhNghiep,
    kh.diaChi;
GO
-- ============================================
-- VIEW 2: vw_KetQuaNenMau_FromKetQua
-- Mục đích: Hiển thị các nền mẫu của kết quả
-- ============================================
CREATE OR ALTER VIEW vw_KetQuaNenMau_FromKetQua AS
SELECT 
    -- Tạo maKQNen từ maDN
    dn.maDN AS maKQNen,
    
    -- maKQ tương ứng (là maDot)
    dn.maDot AS maKQ,
    
    -- Thông tin nền mẫu
    dn.maNen,
    nm.tenNenMau,
    
    -- Thông tin vị trí
    dn.tenViTri AS viTri,
    dn.toaDo
    
FROM Dot_Nen dn
INNER JOIN NenMau nm ON dn.maNen = nm.maNen

-- Chỉ lấy các nền mẫu có ít nhất 1 thông số đã nhập
WHERE EXISTS (
    SELECT 1 
    FROM Dot_Nen_Ts dnts
    INNER JOIN KetQua kq ON dnts.maDNTS = kq.maDNTS
    WHERE dnts.maDN = dn.maDN
);
GO
-- ============================================
-- VIEW 3: vw_KetQuaChiTiet_FromKetQua
-- Mục đích: Hiển thị chi tiết các thông số đo
-- ============================================
CREATE OR ALTER VIEW vw_KetQuaChiTiet_FromKetQua AS
SELECT 
    -- Tạo maKQCT từ maKQ gốc
    kq.maKQ AS maKQCT,
    
    -- maKQNen tương ứng (là maDN)
    dnts.maDN AS maKQNen,
    
    -- Thông tin thông số
    dnts.maTS,
    dnts.tenTS,
    dnts.donVi,
    dnts.phuongPhap AS phuongPhapPhanTich,
    
    -- ✅ Kết quả đo (QUAN TRỌNG - phải cast đúng kiểu)
    CAST(kq.giaTriDoDuoc AS FLOAT) AS ketQua,
    
    -- Giới hạn phát hiện (từ giá trị tối thiểu)
    CASE 
        WHEN dnts.giaTriToiThieu IS NOT NULL 
        THEN CAST(dnts.giaTriToiThieu AS NVARCHAR(50))
        ELSE N'N/A'
    END AS gioiHanPhatHien,
    
    -- QCVN (kết hợp giá trị min-max)
    CASE 
        WHEN dnts.giaTriToiThieu IS NOT NULL OR dnts.giaTriToiDa IS NOT NULL
        THEN CONCAT(
            ISNULL(CAST(dnts.giaTriToiThieu AS NVARCHAR(20)), N'N/A'),
            N' - ',
            ISNULL(CAST(dnts.giaTriToiDa AS NVARCHAR(20)), N'N/A')
        )
        ELSE N'Không quy định'
    END AS qcvn,
    
    -- Tình trạng (so sánh với ngưỡng)
    CASE 
        WHEN dnts.giaTriToiDa IS NOT NULL AND kq.giaTriDoDuoc > dnts.giaTriToiDa 
            THEN N'Vượt ngưỡng'
        WHEN dnts.giaTriToiThieu IS NOT NULL AND kq.giaTriDoDuoc < dnts.giaTriToiThieu 
            THEN N'Dưới ngưỡng'
        ELSE N'Đạt chuẩn'
    END AS TinhTrang
    
FROM KetQua kq
INNER JOIN Dot_Nen_Ts dnts ON kq.maDNTS = dnts.maDNTS
INNER JOIN Dot_Nen dn ON dnts.maDN = dn.maDN
INNER JOIN ThongSoMoiTruong ts ON dnts.maTS = ts.maTS;
GO
-- ============================================
-- SỬA LẠI: sp_CapNhatTrangThaiKetQua
-- Cập nhật trạng thái "xác nhận" = cập nhật trạng thái đợt quan trắc
-- ============================================
CREATE OR ALTER PROCEDURE sp_CapNhatTrangThaiKetQua
    @maKQ VARCHAR(15),  -- Thực chất là maDot
    @trangThaiXacNhan BIT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM DotQuanTrac WHERE maDot = @maKQ)
        BEGIN
            SELECT 0 AS Result, N'Đợt quan trắc không tồn tại!' AS Message;
            RETURN;
        END
        
        IF @trangThaiXacNhan = 1
        BEGIN
            -- XÁC NHẬN: Chuyển trạng thái thành 3 (Hoàn thành)
            UPDATE DotQuanTrac 
            SET trangThai = 3
            WHERE maDot = @maKQ;
            
            SELECT 1 AS Result, N'Xác nhận kết quả thành công!' AS Message;
        END
        ELSE
        BEGIN
            -- HỦY XÁC NHẬN: Chuyển về 2 (Đang thực hiện)
            UPDATE DotQuanTrac 
            SET trangThai = 2
            WHERE maDot = @maKQ;
            
            SELECT 1 AS Result, N'Đã hủy xác nhận kết quả!' AS Message;
        END
    END TRY
    BEGIN CATCH
        SELECT 0 AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
