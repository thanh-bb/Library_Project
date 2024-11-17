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

    public virtual DbSet<ChiTietGioHang> ChiTietGioHangs { get; set; }

    public virtual DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }

    public virtual DbSet<ChiTietPhieuTra> ChiTietPhieuTras { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

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

    public virtual DbSet<PhieuTra> PhieuTras { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<TacGium> TacGia { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<TrangThaiMuon> TrangThaiMuons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-S6UJRMK\\SQLEXPRESS;Database=OnlineLibrary;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietGioHang>(entity =>
        {
            entity.HasKey(e => e.CtghId).HasName("PK__ChiTietG__0903B75D9FDF5E7E");

            entity.ToTable("ChiTietGioHang");

            entity.Property(e => e.CtghId).HasColumnName("ctgh_Id");
            entity.Property(e => e.CtghSoLuong).HasColumnName("ctgh_SoLuong");
            entity.Property(e => e.GhId).HasColumnName("gh_Id");
            entity.Property(e => e.SId).HasColumnName("s_Id");

            entity.HasOne(d => d.Gh).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.GhId)
                .HasConstraintName("FK__ChiTietGi__gh_Id__5FB337D6");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietGioHangs)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__ChiTietGio__s_Id__60A75C0F");
        });

        modelBuilder.Entity<ChiTietPhieuMuon>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.PmId }).HasName("PK__ChiTietP__DD56A22A8B27F943");

            entity.ToTable("ChiTietPhieuMuon");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.CtpmSoLuongSachMuon).HasColumnName("ctpm_SoLuongSachMuon");

            entity.HasOne(d => d.Pm).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.PmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pm_Id__5070F446");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__4F7CD00D");
        });

        modelBuilder.Entity<ChiTietPhieuTra>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.PtId }).HasName("PK__ChiTietP__BA5EB8880ED6B825");

            entity.ToTable("ChiTietPhieuTra");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.PtId).HasColumnName("pt_Id");
            entity.Property(e => e.CtptSoLuongSachTra).HasColumnName("ctpt_SoLuongSachTra");

            entity.HasOne(d => d.Pt).WithMany(p => p.ChiTietPhieuTras)
                .HasForeignKey(d => d.PtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pt_Id__5441852A");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuTras)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__534D60F1");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.DmId).HasName("PK__DanhMuc__56140182C0739657");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.DmId).HasColumnName("dm_Id");
            entity.Property(e => e.DmTenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("dm_TenDanhMuc");
        });

        modelBuilder.Entity<GioHang>(entity =>
        {
            entity.HasKey(e => e.GhId).HasName("PK__GioHang__B435E72B0569C7E5");

            entity.ToTable("GioHang");

            entity.Property(e => e.GhId).HasColumnName("gh_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");

            entity.HasOne(d => d.Nd).WithMany(p => p.GioHangs)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__GioHang__nd_Id__5EBF139D");
        });

        modelBuilder.Entity<HinhMinhHoa>(entity =>
        {
            entity.HasKey(e => e.HmhId).HasName("PK__HinhMinh__207A44E8ED082FAC");

            entity.ToTable("HinhMinhHoa");

            entity.Property(e => e.HmhId).HasColumnName("hmh_Id");
            entity.Property(e => e.HmhHinhAnhMaHoa).HasColumnName("hmh_HinhAnhMaHoa");
            entity.Property(e => e.SId).HasColumnName("s_Id");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.HinhMinhHoas)
                .HasForeignKey(d => d.SId)
                .HasConstraintName("FK__HinhMinhHo__s_Id__5DCAEF64");
        });

        modelBuilder.Entity<KeSach>(entity =>
        {
            entity.HasKey(e => e.KsId).HasName("PK__KeSach__09A0E5C89D23381C");

            entity.ToTable("KeSach");

            entity.Property(e => e.KsId).HasColumnName("ks_Id");
            entity.Property(e => e.KsTenKe)
                .HasMaxLength(150)
                .HasColumnName("ks_TenKe");
        });

        modelBuilder.Entity<LoaiNguoiDung>(entity =>
        {
            entity.HasKey(e => e.LndId).HasName("PK__LoaiNguo__BD3502E325E24BB5");

            entity.ToTable("LoaiNguoiDung");

            entity.Property(e => e.LndId).HasColumnName("lnd_Id");
            entity.Property(e => e.LndTenLoaiNguoiDung)
                .HasMaxLength(100)
                .HasColumnName("lnd_TenLoaiNguoiDung");
        });

        modelBuilder.Entity<LoaiSach>(entity =>
        {
            entity.HasKey(e => e.LsId).HasName("PK__LoaiSach__E0FFD76EDA1BCAA8");

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
            entity.HasKey(e => e.NdId).HasName("PK__NguoiDun__3BE4FDC595AD59AB");

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
                .HasConstraintName("FK__NguoiDung__lnd_L__5629CD9C");

            entity.HasOne(d => d.QIdNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.QId)
                .HasConstraintName("FK__NguoiDung__q_Id__5535A963");
        });

        modelBuilder.Entity<NguoiDungDangKy>(entity =>
        {
            entity.HasKey(e => e.NddkId).HasName("PK__NguoiDun__5AE7E2328C42997A");

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
            entity.Property(e => e.NddkSoTien)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("nddk_SoTien");
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
            entity.HasKey(e => e.NxbId).HasName("PK__NhaXuatB__FDF00B9B16ABA06D");

            entity.ToTable("NhaXuatBan");

            entity.Property(e => e.NxbId).HasColumnName("nxb_Id");
            entity.Property(e => e.NxbTenNhaXuatBan)
                .HasMaxLength(100)
                .HasColumnName("nxb_TenNhaXuatBan");
        });

        modelBuilder.Entity<Osach>(entity =>
        {
            entity.HasKey(e => e.OsId).HasName("PK__OSach__3750B0BDA3566BD0");

            entity.ToTable("OSach");

            entity.Property(e => e.OsId).HasColumnName("os_Id");
            entity.Property(e => e.OsTenO)
                .HasMaxLength(150)
                .HasColumnName("os_TenO");
        });

        modelBuilder.Entity<PhieuDongPhat>(entity =>
        {
            entity.HasKey(e => e.PdpId).HasName("PK__PhieuDon__A65BF78E25D47B94");

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
                .HasConstraintName("FK__PhieuDong__pm_Id__571DF1D5");
        });

        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.PmId).HasName("PK__PhieuMuo__26B01F6E362BEB56");

            entity.ToTable("PhieuMuon");

            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmDaXuatPhat)
                .HasDefaultValue(false)
                .HasColumnName("pm_DaXuatPhat");
            entity.Property(e => e.PmHanTra)
                .HasColumnType("datetime")
                .HasColumnName("pm_HanTra");
            entity.Property(e => e.PmLoaiMuon)
                .HasMaxLength(50)
                .HasColumnName("pm_LoaiMuon");
            entity.Property(e => e.PmNgayMuon)
                .HasColumnType("datetime")
                .HasColumnName("pm_NgayMuon");
            entity.Property(e => e.PmTrangThaiXetDuyet)
                .HasMaxLength(150)
                .HasColumnName("pm_TrangThaiXetDuyet");
            entity.Property(e => e.TtmId).HasColumnName("ttm_Id");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuMuon__nd_Id__4D94879B");

            entity.HasOne(d => d.Ttm).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.TtmId)
                .HasConstraintName("FK__PhieuMuon__ttm_I__4E88ABD4");
        });

        modelBuilder.Entity<PhieuTra>(entity =>
        {
            entity.HasKey(e => e.PtId).HasName("PK__PhieuTra__5631B54807D4CF44");

            entity.ToTable("PhieuTra");

            entity.Property(e => e.PtId).HasColumnName("pt_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.PtNgayTra)
                .HasColumnType("datetime")
                .HasColumnName("pt_NgayTra");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuTra__nd_Id__5165187F");

            entity.HasOne(d => d.Pm).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.PmId)
                .HasConstraintName("FK__PhieuTra__pm_Id__52593CB8");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.QId).HasName("PK__Quyen__3D56876864A6F7EC");

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
            entity.HasKey(e => e.UserId).HasName("PK__Refresht__1788CC4C54F3B5C4");

            entity.ToTable("Refreshtoken");

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.RefreshToken1).HasColumnName("RefreshToken");
            entity.Property(e => e.TokenId).HasMaxLength(50);
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__Sach__2F3DA3DC278B4D5E");

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
                .HasConstraintName("FK__Sach__ks_Id__5BE2A6F2");

            entity.HasOne(d => d.Ls).WithMany(p => p.Saches)
                .HasForeignKey(d => d.LsId)
                .HasConstraintName("FK__Sach__ls_Id__5AEE82B9");

            entity.HasOne(d => d.Nxb).WithMany(p => p.Saches)
                .HasForeignKey(d => d.NxbId)
                .HasConstraintName("FK__Sach__nxb_Id__59063A47");

            entity.HasOne(d => d.Os).WithMany(p => p.Saches)
                .HasForeignKey(d => d.OsId)
                .HasConstraintName("FK__Sach__os_Id__5CD6CB2B");

            entity.HasOne(d => d.Tg).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TgId)
                .HasConstraintName("FK__Sach__tg_Id__5812160E");

            entity.HasOne(d => d.Tl).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TlId)
                .HasConstraintName("FK__Sach__tl_Id__59FA5E80");
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.TgId).HasName("PK__TacGia__006D09FB81100C93");

            entity.Property(e => e.TgId).HasColumnName("tg_Id");
            entity.Property(e => e.TgTenTacGia)
                .HasMaxLength(100)
                .HasColumnName("tg_TenTacGia");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.TlId).HasName("PK__TheLoai__8506AC8643F2625E");

            entity.ToTable("TheLoai");

            entity.Property(e => e.TlId).HasColumnName("tl_Id");
            entity.Property(e => e.DmId).HasColumnName("dm_Id");
            entity.Property(e => e.TlTenTheLoai)
                .HasMaxLength(100)
                .HasColumnName("tl_TenTheLoai");

            entity.HasOne(d => d.Dm).WithMany(p => p.TheLoais)
                .HasForeignKey(d => d.DmId)
                .HasConstraintName("FK__TheLoai__dm_Id__4CA06362");
        });

        modelBuilder.Entity<TrangThaiMuon>(entity =>
        {
            entity.HasKey(e => e.TtmId).HasName("PK__TrangTha__D9B1BFAFCD54C44F");

            entity.ToTable("TrangThaiMuon");

            entity.Property(e => e.TtmId).HasColumnName("ttm_Id");
            entity.Property(e => e.TtmMoTa)
                .HasMaxLength(250)
                .HasColumnName("ttm_MoTa");
            entity.Property(e => e.TtmTenTrangThai)
                .HasMaxLength(150)
                .HasColumnName("ttm_TenTrangThai");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
