using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_API.Models;

public partial class OnlineLibraryContext : DbContext
{
    public OnlineLibraryContext()
    {
    }

    public OnlineLibraryContext(DbContextOptions<OnlineLibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BinhLuan> BinhLuans { get; set; }

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }

    public virtual DbSet<ChiTietPhieuMuonOnline> ChiTietPhieuMuonOnlines { get; set; }

    public virtual DbSet<ChiTietPhieuTra> ChiTietPhieuTras { get; set; }

    public virtual DbSet<ChiTietPhieuTraOnline> ChiTietPhieuTraOnlines { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<DiaChiGiaoHang> DiaChiGiaoHangs { get; set; }

    public virtual DbSet<GioHang> GioHangs { get; set; }

    public virtual DbSet<HinhMinhHoa> HinhMinhHoas { get; set; }

    public virtual DbSet<KeSach> KeSaches { get; set; }

    public virtual DbSet<LoaiNguoiDung> LoaiNguoiDungs { get; set; }

    public virtual DbSet<LoaiSach> LoaiSaches { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NguoiDungDangKy> NguoiDungDangKies { get; set; }

    public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }

    public virtual DbSet<Osach> Osaches { get; set; }

    public virtual DbSet<PhieuDongPhat> PhieuDongPhats { get; set; }

    public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }

    public virtual DbSet<PhieuMuonOnline> PhieuMuonOnlines { get; set; }

    public virtual DbSet<PhieuTra> PhieuTras { get; set; }

    public virtual DbSet<PhieuTraOnline> PhieuTraOnlines { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<TacGium> TacGia { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-S6UJRMK\\SQLEXPRESS;Database=OnlineLibrary;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BinhLuan>(entity =>
        {
            entity.HasKey(e => e.BlId).HasName("PK__BinhLuan__86E41292DA7E11E2");

            entity.ToTable("BinhLuan");

            entity.Property(e => e.BlId).HasColumnName("bl_Id");
            entity.Property(e => e.BlNgayDang)
                .HasColumnType("datetime")
                .HasColumnName("bl_NgayDang");
            entity.Property(e => e.BlNoiDung)
                .HasMaxLength(500)
                .HasColumnName("bl_NoiDung");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.SId).HasColumnName("s_Id");

            entity.HasOne(d => d.Nd).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__BinhLuan__nd_Id__6C190EBB");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.BinhLuans)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__BinhLuan__s_Id__6D0D32F4");
        });

        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.CtghId).HasName("PK__ChiTietG__0903B75D342E9F3A");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.CtghId).HasColumnName("ctgh_Id");
            entity.Property(e => e.CtghSoLuong).HasColumnName("ctgh_SoLuong");
            entity.Property(e => e.GhId).HasColumnName("gh_Id");
            entity.Property(e => e.SId).HasColumnName("s_Id");

            entity.HasOne(d => d.Gh).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.GhId)
                .HasConstraintName("FK__ChiTietGi__gh_Id__6A30C649");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__ChiTietGio__s_Id__6B24EA82");
        });

        modelBuilder.Entity<ChiTietPhieuMuon>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.PmId }).HasName("PK__ChiTietP__DD56A22A2916005F");

            entity.ToTable("ChiTietPhieuMuon");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.CtpmSoLuongSachMuon).HasColumnName("ctpm_SoLuongSachMuon");

            entity.HasOne(d => d.Pm).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.PmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pm_Id__5AEE82B9");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__59FA5E80");
        });

        modelBuilder.Entity<ChiTietPhieuMuonOnline>(entity =>
        {
            entity.HasKey(e => new { e.PmoId, e.SId }).HasName("PK__ChiTietP__126D038F0A115748");

            entity.ToTable("ChiTietPhieuMuonOnline");

            entity.Property(e => e.PmoId).HasColumnName("pmo_Id");
            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.CtpmoSoLuongSachMuon).HasColumnName("ctpmo_SoLuongSachMuon");

            entity.HasOne(d => d.Pmo).WithMany(p => p.ChiTietPhieuMuonOnlines)
                .HasForeignKey(d => d.PmoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pmo_I__6FE99F9F");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuMuonOnlines)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__70DDC3D8");
        });

        modelBuilder.Entity<ChiTietPhieuTra>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.PtId }).HasName("PK__ChiTietP__BA5EB8888E16AA3D");

            entity.ToTable("ChiTietPhieuTra");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.PtId).HasColumnName("pt_Id");
            entity.Property(e => e.CtptSoLuongSachTra).HasColumnName("ctpt_SoLuongSachTra");

            entity.HasOne(d => d.Pt).WithMany(p => p.ChiTietPhieuTras)
                .HasForeignKey(d => d.PtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pt_Id__5EBF139D");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuTras)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__5DCAEF64");
        });

        modelBuilder.Entity<ChiTietPhieuTraOnline>(entity =>
        {
            entity.HasKey(e => e.CtptoId).HasName("PK__ChiTietP__1AE7D792D8422EA3");

            entity.ToTable("ChiTietPhieuTraOnline");

            entity.Property(e => e.CtptoId).HasColumnName("ctpto_Id");
            entity.Property(e => e.CtptoSoLuongSachTra).HasColumnName("ctpto_SoLuongSachTra");
            entity.Property(e => e.PtoId).HasColumnName("pto_Id");
            entity.Property(e => e.SId).HasColumnName("s_Id");

            entity.HasOne(d => d.Pto).WithMany(p => p.ChiTietPhieuTraOnlines)
                .HasForeignKey(d => d.PtoId)
                .HasConstraintName("FK__ChiTietPh__pto_I__74AE54BC");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuTraOnlines)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__ChiTietPhi__s_Id__75A278F5");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.DmId).HasName("PK__DanhMuc__56140182CF0085AC");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.DmId).HasColumnName("dm_Id");
            entity.Property(e => e.DmTenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("dm_TenDanhMuc");
        });

        modelBuilder.Entity<DiaChiGiaoHang>(entity =>
        {
            entity.HasKey(e => e.DcghId).HasName("PK__DiaChiGi__01730B8686D8A76C");

            entity.ToTable("DiaChiGiaoHang");

            entity.Property(e => e.DcghId).HasColumnName("dcgh_Id");
            entity.Property(e => e.DcghDiaChi)
                .HasMaxLength(200)
                .HasColumnName("dcgh_DiaChi");
            entity.Property(e => e.DcghSoDienThoai)
                .HasMaxLength(20)
                .HasColumnName("dcgh_SoDienThoai");
            entity.Property(e => e.DcghTenNguoiNhan)
                .HasMaxLength(100)
                .HasColumnName("dcgh_TenNguoiNhan");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");

            entity.HasOne(d => d.Nd).WithMany(p => p.DiaChiGiaoHangs)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__DiaChiGia__nd_Id__76969D2E");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.GhId).HasName("PK__GioHang__B435E72B026494AE");

            entity.ToTable("GioHang");

            entity.Property(e => e.GhId).HasColumnName("gh_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");

            entity.HasOne(d => d.Nd).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__GioHang__nd_Id__693CA210");
        });

        modelBuilder.Entity<HinhMinhHoa>(entity =>
        {
            entity.HasKey(e => e.HmhId).HasName("PK__HinhMinh__207A44E8A06B72D1");

            entity.ToTable("HinhMinhHoa");

            entity.Property(e => e.HmhId).HasColumnName("hmh_Id");
            entity.Property(e => e.HmhHinhAnhMaHoa).HasColumnName("hmh_HinhAnhMaHoa");
            entity.Property(e => e.SId).HasColumnName("s_Id");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.HinhMinhHoas)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__HinhMinhHo__s_Id__68487DD7");
        });

        modelBuilder.Entity<KeSach>(entity =>
        {
            entity.HasKey(e => e.KsId).HasName("PK__KeSach__09A0E5C8C255CCE5");

            entity.ToTable("KeSach");

            entity.Property(e => e.KsId).HasColumnName("ks_Id");
            entity.Property(e => e.KsTenKe)
                .HasMaxLength(150)
                .HasColumnName("ks_TenKe");
        });

        modelBuilder.Entity<LoaiNguoiDung>(entity =>
        {
            entity.HasKey(e => e.LndId).HasName("PK__LoaiNguo__BD3502E37AEAC360");

            entity.ToTable("LoaiNguoiDung");

            entity.Property(e => e.LndId).HasColumnName("lnd_Id");
            entity.Property(e => e.LndTenLoaiNguoiDung)
                .HasMaxLength(100)
                .HasColumnName("lnd_TenLoaiNguoiDung");
        });

        modelBuilder.Entity<LoaiSach>(entity =>
        {
            entity.HasKey(e => e.LsId).HasName("PK__LoaiSach__E0FFD76E09851CD7");

            entity.ToTable("LoaiSach");

            entity.Property(e => e.LsId).HasColumnName("ls_Id");
            entity.Property(e => e.LsGhiChu)
                .HasMaxLength(200)
                .HasColumnName("ls_GhiChu");
            entity.Property(e => e.LsKichThuoc)
                .HasMaxLength(100)
                .HasColumnName("ls_KichThuoc");
            entity.Property(e => e.LsTenLoaiSach)
                .HasMaxLength(100)
                .HasColumnName("ls_TenLoaiSach");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.NdId).HasName("PK__NguoiDun__3BE4FDC553D59CCB");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.LndLoaiNguoiDung).HasColumnName("lnd_LoaiNguoiDung");
            entity.Property(e => e.NdActive).HasColumnName("nd_active");
            entity.Property(e => e.NdCccd)
                .HasMaxLength(12)
                .HasColumnName("nd_CCCD");
            entity.Property(e => e.NdDiaChi)
                .HasMaxLength(200)
                .HasColumnName("nd_DiaChi");
            entity.Property(e => e.NdEmail)
                .HasMaxLength(100)
                .HasColumnName("nd_Email");
            entity.Property(e => e.NdGioiTinh)
                .HasMaxLength(10)
                .HasColumnName("nd_GioiTinh");
            entity.Property(e => e.NdHinhThe).HasColumnName("nd_HinhThe");
            entity.Property(e => e.NdHoTen)
                .HasMaxLength(100)
                .HasColumnName("nd_HoTen");
            entity.Property(e => e.NdNgayDangKy)
                .HasColumnType("datetime")
                .HasColumnName("nd_NgayDangKy");
            entity.Property(e => e.NdNgaySinh)
                .HasColumnType("datetime")
                .HasColumnName("nd_NgaySinh");
            entity.Property(e => e.NdPassword)
                .HasMaxLength(20)
                .HasColumnName("nd_Password");
            entity.Property(e => e.NdSoDienThoai)
                .HasMaxLength(20)
                .HasColumnName("nd_SoDienThoai");
            entity.Property(e => e.NdThoiGianSuDung)
                .HasMaxLength(100)
                .HasColumnName("nd_ThoiGianSuDung");
            entity.Property(e => e.NdUsername)
                .HasMaxLength(50)
                .HasColumnName("nd_Username");
            entity.Property(e => e.QId)
                .HasMaxLength(50)
                .HasColumnName("q_Id");

            entity.HasOne(d => d.LndLoaiNguoiDungNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.LndLoaiNguoiDung)
                .HasConstraintName("FK__NguoiDung__lnd_L__60A75C0F");

            entity.HasOne(d => d.QIdNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.QId)
                .HasConstraintName("FK__NguoiDung__q_Id__5FB337D6");
        });

        modelBuilder.Entity<NguoiDungDangKy>(entity =>
        {
            entity.HasKey(e => e.NddkId).HasName("PK__NguoiDun__5AE7E232B108030D");

            entity.ToTable("NguoiDungDangKy");

            entity.Property(e => e.NddkId).HasColumnName("nddk_Id");
            entity.Property(e => e.NddkCccd)
                .HasMaxLength(12)
                .HasColumnName("nddk_CCCD");
            entity.Property(e => e.NddkCccdMatSau).HasColumnName("nddk_CCCD_MatSau");
            entity.Property(e => e.NddkCccdMatTruoc).HasColumnName("nddk_CCCD_MatTruoc");
            entity.Property(e => e.NddkDiaChi)
                .HasMaxLength(200)
                .HasColumnName("nddk_DiaChi");
            entity.Property(e => e.NddkEmail)
                .HasMaxLength(100)
                .HasColumnName("nddk_Email");
            entity.Property(e => e.NddkGioiTinh)
                .HasMaxLength(10)
                .HasColumnName("nddk_GioiTinh");
            entity.Property(e => e.NddkHinhThe).HasColumnName("nddk_HinhThe");
            entity.Property(e => e.NddkHinhThucTraPhi)
                .HasMaxLength(100)
                .HasColumnName("nddk_HinhThucTraPhi");
            entity.Property(e => e.NddkHoTen)
                .HasMaxLength(100)
                .HasColumnName("nddk_HoTen");
            entity.Property(e => e.NddkNgayDangKy)
                .HasColumnType("datetime")
                .HasColumnName("nddk_NgayDangKy");
            entity.Property(e => e.NddkNgaySinh)
                .HasColumnType("datetime")
                .HasColumnName("nddk_NgaySinh");
            entity.Property(e => e.NddkSoDienThoai)
                .HasMaxLength(20)
                .HasColumnName("nddk_SoDienThoai");
            entity.Property(e => e.NddkThoiGianSuDung)
                .HasMaxLength(100)
                .HasColumnName("nddk_ThoiGianSuDung");
            entity.Property(e => e.NddkTrangThaiDuyet)
                .HasMaxLength(50)
                .HasColumnName("nddk_TrangThaiDuyet");
            entity.Property(e => e.NddkTrangThaiThanhToan)
                .HasMaxLength(100)
                .HasColumnName("nddk_TrangThaiThanhToan");
        });

        modelBuilder.Entity<NhaXuatBan>(entity =>
        {
            entity.HasKey(e => e.NxbId).HasName("PK__NhaXuatB__FDF00B9BC8C8414A");

            entity.ToTable("NhaXuatBan");

            entity.Property(e => e.NxbId).HasColumnName("nxb_Id");
            entity.Property(e => e.NxbTenNhaXuatBan)
                .HasMaxLength(100)
                .HasColumnName("nxb_TenNhaXuatBan");
        });

        modelBuilder.Entity<Osach>(entity =>
        {
            entity.HasKey(e => e.OsId).HasName("PK__OSach__3750B0BD44D73F12");

            entity.ToTable("OSach");

            entity.Property(e => e.OsId).HasColumnName("os_Id");
            entity.Property(e => e.OsTenO)
                .HasMaxLength(150)
                .HasColumnName("os_TenO");
        });

        modelBuilder.Entity<PhieuDongPhat>(entity =>
        {
            entity.HasKey(e => e.PdpId).HasName("PK__PhieuDon__A65BF78E7CC7771D");

            entity.ToTable("PhieuDongPhat");

            entity.Property(e => e.PdpId).HasColumnName("pdp_Id");
            entity.Property(e => e.PdpNgayDong)
                .HasColumnType("datetime")
                .HasColumnName("pdp_NgayDong");
            entity.Property(e => e.PdpTongTienPhat).HasColumnName("pdp_TongTienPhat");
            entity.Property(e => e.PdpTrangThaiDong).HasColumnName("pdp_TrangThaiDong");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");

            entity.HasOne(d => d.Pm).WithMany(p => p.PhieuDongPhats)
                .HasForeignKey(d => d.PmId)
                .HasConstraintName("FK__PhieuDong__pm_Id__619B8048");
        });

        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.PmId).HasName("PK__PhieuMuo__26B01F6E91BC7486");

            entity.ToTable("PhieuMuon");

            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmHanTra)
                .HasColumnType("datetime")
                .HasColumnName("pm_HanTra");
            entity.Property(e => e.PmNgayMuon)
                .HasColumnType("datetime")
                .HasColumnName("pm_NgayMuon");
            entity.Property(e => e.PmTrangThaiMuon)
                .HasMaxLength(150)
                .HasColumnName("pm_TrangThaiMuon");
            entity.Property(e => e.PmTrangThaiXetDuyet)
                .HasMaxLength(150)
                .HasColumnName("pm_TrangThaiXetDuyet");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuMuon__nd_Id__59063A47");
        });

        modelBuilder.Entity<PhieuMuonOnline>(entity =>
        {
            entity.HasKey(e => e.PmoId).HasName("PK__PhieuMuo__C09ED9B2412C0A59");

            entity.ToTable("PhieuMuonOnline");

            entity.Property(e => e.PmoId).HasColumnName("pmo_Id");
            entity.Property(e => e.DcghId).HasColumnName("dcgh_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmoLoaiGiaoHang)
                .HasMaxLength(50)
                .HasColumnName("pmo_LoaiGiaoHang");
            entity.Property(e => e.PmoNgayDat)
                .HasColumnType("datetime")
                .HasColumnName("pmo_NgayDat");
            entity.Property(e => e.PmoPhuongThucThanhToan)
                .HasMaxLength(50)
                .HasColumnName("pmo_PhuongThucThanhToan");
            entity.Property(e => e.PmoTrangThai)
                .HasMaxLength(50)
                .HasColumnName("pmo_TrangThai");

            entity.HasOne(d => d.Dcgh).WithMany(p => p.PhieuMuonOnlines)
                .HasForeignKey(d => d.DcghId)
                .HasConstraintName("FK__PhieuMuon__dcgh___6EF57B66");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuMuonOnlines)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuMuon__nd_Id__6E01572D");
        });

        modelBuilder.Entity<PhieuTra>(entity =>
        {
            entity.HasKey(e => e.PtId).HasName("PK__PhieuTra__5631B54801272514");

            entity.ToTable("PhieuTra");

            entity.Property(e => e.PtId).HasColumnName("pt_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.PtNgayTra)
                .HasColumnType("datetime")
                .HasColumnName("pt_NgayTra");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuTra__nd_Id__5BE2A6F2");

            entity.HasOne(d => d.Pm).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.PmId)
                .HasConstraintName("FK__PhieuTra__pm_Id__5CD6CB2B");
        });

        modelBuilder.Entity<PhieuTraOnline>(entity =>
        {
            entity.HasKey(e => e.PtoId).HasName("PK__PhieuTra__6856AD9D4C648AEE");

            entity.ToTable("PhieuTraOnline");

            entity.Property(e => e.PtoId).HasColumnName("pto_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmoId).HasColumnName("pmo_Id");
            entity.Property(e => e.PtoHinhThucTra)
                .HasMaxLength(50)
                .HasColumnName("pto_HinhThucTra");
            entity.Property(e => e.PtoNgayTra)
                .HasColumnType("datetime")
                .HasColumnName("pto_NgayTra");
            entity.Property(e => e.PtoTrangThai)
                .HasMaxLength(50)
                .HasColumnName("pto_TrangThai");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuTraOnlines)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuTraO__nd_Id__73BA3083");

            entity.HasOne(d => d.Pmo).WithMany(p => p.PhieuTraOnlines)
                .HasForeignKey(d => d.PmoId)
                .HasConstraintName("FK__PhieuTraO__pmo_I__72C60C4A");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.QId).HasName("PK__Quyen__3D5687688762BFD7");

            entity.ToTable("Quyen");

            entity.Property(e => e.QId)
                .HasMaxLength(50)
                .HasColumnName("q_Id");
            entity.Property(e => e.QTenQuyen)
                .HasMaxLength(100)
                .HasColumnName("q_TenQuyen");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Refresht__1788CC4CD4207B3F");

            entity.ToTable("Refreshtoken");

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.RefreshToken1).HasColumnName("RefreshToken");
            entity.Property(e => e.TokenId).HasMaxLength(50);
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__Sach__2F3DA3DCD4732276");

            entity.ToTable("Sach");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.KsId).HasColumnName("ks_Id");
            entity.Property(e => e.LsId).HasColumnName("ls_Id");
            entity.Property(e => e.NxbId).HasColumnName("nxb_Id");
            entity.Property(e => e.OsId).HasColumnName("os_Id");
            entity.Property(e => e.SChiDoc).HasColumnName("s_ChiDoc");
            entity.Property(e => e.SMoTa)
                .HasMaxLength(500)
                .HasColumnName("s_MoTa");
            entity.Property(e => e.SNamXuatBan)
                .HasColumnType("datetime")
                .HasColumnName("s_NamXuatBan");
            entity.Property(e => e.SSoLuong).HasColumnName("s_SoLuong");
            entity.Property(e => e.STenSach)
                .HasMaxLength(100)
                .HasColumnName("s_TenSach");
            entity.Property(e => e.STrangThaiMuon).HasColumnName("s_TrangThaiMuon");
            entity.Property(e => e.STrongLuong)
                .HasMaxLength(100)
                .HasColumnName("s_TrongLuong");
            entity.Property(e => e.TgId).HasColumnName("tg_Id");
            entity.Property(e => e.TlId).HasColumnName("tl_Id");

            entity.HasOne(d => d.Ks).WithMany(p => p.Saches)
                .HasForeignKey(d => d.KsId)
                .HasConstraintName("FK__Sach__ks_Id__66603565");

            entity.HasOne(d => d.Ls).WithMany(p => p.Saches)
                .HasForeignKey(d => d.LsId)
                .HasConstraintName("FK__Sach__ls_Id__656C112C");

            entity.HasOne(d => d.Nxb).WithMany(p => p.Saches)
                .HasForeignKey(d => d.NxbId)
                .HasConstraintName("FK__Sach__nxb_Id__6383C8BA");

            entity.HasOne(d => d.Os).WithMany(p => p.Saches)
                .HasForeignKey(d => d.OsId)
                .HasConstraintName("FK__Sach__os_Id__6754599E");

            entity.HasOne(d => d.Tg).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TgId)
                .HasConstraintName("FK__Sach__tg_Id__628FA481");

            entity.HasOne(d => d.Tl).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TlId)
                .HasConstraintName("FK__Sach__tl_Id__6477ECF3");
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.TgId).HasName("PK__TacGia__006D09FBE213B469");

            entity.Property(e => e.TgId).HasColumnName("tg_Id");
            entity.Property(e => e.TgTenTacGia)
                .HasMaxLength(100)
                .HasColumnName("tg_TenTacGia");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.TtId).HasName("PK__ThanhToa__1B53DCFEA2F75A10");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.TtId).HasColumnName("tt_Id");
            entity.Property(e => e.PmoId).HasColumnName("pmo_Id");
            entity.Property(e => e.TtNgayThanhToan)
                .HasColumnType("datetime")
                .HasColumnName("tt_NgayThanhToan");
            entity.Property(e => e.TtPhuongThuc)
                .HasMaxLength(50)
                .HasColumnName("tt_PhuongThuc");
            entity.Property(e => e.TtTrangThai)
                .HasMaxLength(50)
                .HasColumnName("tt_TrangThai");

            entity.HasOne(d => d.Pmo).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.PmoId)
                .HasConstraintName("FK__ThanhToan__pmo_I__71D1E811");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.TlId).HasName("PK__TheLoai__8506AC86E76033C2");

            entity.ToTable("TheLoai");

            entity.Property(e => e.TlId).HasColumnName("tl_Id");
            entity.Property(e => e.DmId).HasColumnName("dm_Id");
            entity.Property(e => e.TlTenTheLoai)
                .HasMaxLength(100)
                .HasColumnName("tl_TenTheLoai");

            entity.HasOne(d => d.Dm).WithMany(p => p.TheLoais)
                .HasForeignKey(d => d.DmId)
                .HasConstraintName("FK__TheLoai__dm_Id__5812160E");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
