using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Library_API.Models;

public partial class LibraryContext : DbContext
{
    public LibraryContext()
    {
    }

    public LibraryContext(DbContextOptions<LibraryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietPhieuMuon> ChiTietPhieuMuons { get; set; }

    public virtual DbSet<ChiTietPhieuTra> ChiTietPhieuTras { get; set; }

    public virtual DbSet<DanhMuc> DanhMucs { get; set; }

    public virtual DbSet<GiaPhat> GiaPhats { get; set; }

    public virtual DbSet<KeSach> KeSaches { get; set; }

    public virtual DbSet<LoaiSach> LoaiSaches { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-S6UJRMK\\SQLEXPRESS;Database=library;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietPhieuMuon>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.PmId }).HasName("PK__ChiTietP__DD56A22AF460972D");

            entity.ToTable("ChiTietPhieuMuon");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.CtpmSoLuongSachMuon).HasColumnName("ctpm_SoLuongSachMuon");

            entity.HasOne(d => d.Pm).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.PmId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pm_Id__4D94879B");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuMuons)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__4CA06362");
        });

        modelBuilder.Entity<ChiTietPhieuTra>(entity =>
        {
            entity.HasKey(e => new { e.SId, e.PtId }).HasName("PK__ChiTietP__BA5EB888A8924B03");

            entity.ToTable("ChiTietPhieuTra");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.PtId).HasColumnName("pt_Id");
            entity.Property(e => e.CtptSoLuongSachTra).HasColumnName("ctpt_SoLuongSachTra");

            entity.HasOne(d => d.Pt).WithMany(p => p.ChiTietPhieuTras)
                .HasForeignKey(d => d.PtId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPh__pt_Id__5070F446");

            entity.HasOne(d => d.SIdNavigation).WithMany(p => p.ChiTietPhieuTras)
                .HasForeignKey(d => d.SId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietPhi__s_Id__4F7CD00D");
        });

        modelBuilder.Entity<DanhMuc>(entity =>
        {
            entity.HasKey(e => e.DmId).HasName("PK__DanhMuc__561401828022ADBD");

            entity.ToTable("DanhMuc");

            entity.Property(e => e.DmId).HasColumnName("dm_Id");
            entity.Property(e => e.DmTenDanhMuc)
                .HasMaxLength(100)
                .HasColumnName("dm_TenDanhMuc");
        });

        modelBuilder.Entity<GiaPhat>(entity =>
        {
            entity.HasKey(e => e.GpId).HasName("PK__GiaPhat__858628400F9A1357");

            entity.ToTable("GiaPhat");

            entity.Property(e => e.GpId).HasColumnName("gp_Id");
            entity.Property(e => e.GpNgayPhat)
                .HasColumnType("datetime")
                .HasColumnName("gp_NgayPhat");
            entity.Property(e => e.GpTienPhat).HasColumnName("gp_TienPhat");
        });

        modelBuilder.Entity<KeSach>(entity =>
        {
            entity.HasKey(e => e.KsId).HasName("PK__KeSach__09A0E5C8BCA52F1B");

            entity.ToTable("KeSach");

            entity.Property(e => e.KsId).HasColumnName("ks_Id");
            entity.Property(e => e.KsTenKe)
                .HasMaxLength(150)
                .HasColumnName("ks_TenKe");
        });

        modelBuilder.Entity<LoaiSach>(entity =>
        {
            entity.HasKey(e => e.LsId).HasName("PK__LoaiSach__E0FFD76ECAD51DBB");

            entity.ToTable("LoaiSach");

            entity.Property(e => e.LsId).HasColumnName("ls_Id");
            entity.Property(e => e.GpId).HasColumnName("gp_Id");
            entity.Property(e => e.LsGhiChu)
                .HasMaxLength(200)
                .HasColumnName("ls_GhiChu");
            entity.Property(e => e.LsKichThuoc)
                .HasMaxLength(100)
                .HasColumnName("ls_KichThuoc");
            entity.Property(e => e.LsTenLoaiSach)
                .HasMaxLength(100)
                .HasColumnName("ls_TenLoaiSach");

            entity.HasOne(d => d.Gp).WithMany(p => p.LoaiSaches)
                .HasForeignKey(d => d.GpId)
                .HasConstraintName("FK__LoaiSach__gp_Id__71D1E811");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.NdId).HasName("PK__NguoiDun__3BE4FDC5FF28C2E6");

            entity.ToTable("NguoiDung");

            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.NdActive).HasColumnName("nd_active");
            entity.Property(e => e.NdDiaChi)
                .HasMaxLength(200)
                .HasColumnName("nd_DiaChi");
            entity.Property(e => e.NdGioiTinh)
                .HasMaxLength(10)
                .HasColumnName("nd_GioiTinh");
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
            entity.Property(e => e.NdUsername)
                .HasMaxLength(50)
                .HasColumnName("nd_Username");
            entity.Property(e => e.QId)
                .HasMaxLength(50)
                .HasColumnName("q_Id");

            entity.HasOne(d => d.QIdNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.QId)
                .HasConstraintName("FK__NguoiDung__q_Id__5165187F");
        });

        modelBuilder.Entity<NhaXuatBan>(entity =>
        {
            entity.HasKey(e => e.NxbId).HasName("PK__NhaXuatB__FDF00B9BC55E5E3D");

            entity.ToTable("NhaXuatBan");

            entity.Property(e => e.NxbId).HasColumnName("nxb_Id");
            entity.Property(e => e.NxbTenNhaXuatBan)
                .HasMaxLength(100)
                .HasColumnName("nxb_TenNhaXuatBan");
        });

        modelBuilder.Entity<Osach>(entity =>
        {
            entity.HasKey(e => e.OsId).HasName("PK__OSach__3750B0BDE408C928");

            entity.ToTable("OSach");

            entity.Property(e => e.OsId).HasColumnName("os_Id");
            entity.Property(e => e.OsTenO)
                .HasMaxLength(150)
                .HasColumnName("os_TenO");
        });

        modelBuilder.Entity<PhieuDongPhat>(entity =>
        {
            entity.HasKey(e => e.PdpId).HasName("PK__PhieuDon__A65BF78EBB793F2C");

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
                .HasConstraintName("FK__PhieuDong__pm_Id__17F790F9");
        });

        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.PmId).HasName("PK__PhieuMuo__26B01F6E9D99D6A2");

            entity.ToTable("PhieuMuon");

            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmHanTra)
                .HasColumnType("datetime")
                .HasColumnName("pm_HanTra");
            entity.Property(e => e.PmNgayMuon)
                .HasColumnType("datetime")
                .HasColumnName("pm_NgayMuon");
            entity.Property(e => e.PmTrangThai)
                .HasMaxLength(150)
                .HasColumnName("pm_TrangThai");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuMuons)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuMuon__nd_Id__4BAC3F29");
        });

        modelBuilder.Entity<PhieuTra>(entity =>
        {
            entity.HasKey(e => e.PtId).HasName("PK__PhieuTra__5631B548CFD55855");

            entity.ToTable("PhieuTra");

            entity.Property(e => e.PtId).HasColumnName("pt_Id");
            entity.Property(e => e.NdId).HasColumnName("nd_Id");
            entity.Property(e => e.PmId).HasColumnName("pm_Id");
            entity.Property(e => e.PtNgayTra)
                .HasColumnType("datetime")
                .HasColumnName("pt_NgayTra");

            entity.HasOne(d => d.Nd).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.NdId)
                .HasConstraintName("FK__PhieuTra__nd_Id__4E88ABD4");

            entity.HasOne(d => d.Pm).WithMany(p => p.PhieuTras)
                .HasForeignKey(d => d.PmId)
                .HasConstraintName("FK_PhieuTra_PhieuMuon");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.QId).HasName("PK__Quyen__3D5687682713E6F5");

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
            entity.HasKey(e => e.UserId).HasName("PK__Refresht__1788CC4C39F6C4E1");

            entity.ToTable("Refreshtoken");

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.RefreshToken1).HasColumnName("RefreshToken");
            entity.Property(e => e.TokenId).HasMaxLength(50);
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.SId).HasName("PK__Sach__2F3DA3DC122796D8");

            entity.ToTable("Sach");

            entity.Property(e => e.SId).HasColumnName("s_Id");
            entity.Property(e => e.KsId).HasColumnName("ks_Id");
            entity.Property(e => e.LsId).HasColumnName("ls_Id");
            entity.Property(e => e.NxbId).HasColumnName("nxb_Id");
            entity.Property(e => e.OsId).HasColumnName("os_Id");
            entity.Property(e => e.SChiDoc).HasColumnName("s_ChiDoc");
            entity.Property(e => e.SHinhAnh).HasColumnName("s_HinhAnh");
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
                .HasConstraintName("ks_Id");

            entity.HasOne(d => d.Ls).WithMany(p => p.Saches)
                .HasForeignKey(d => d.LsId)
                .HasConstraintName("FK__Sach__ls_Id__49C3F6B7");

            entity.HasOne(d => d.Nxb).WithMany(p => p.Saches)
                .HasForeignKey(d => d.NxbId)
                .HasConstraintName("FK__Sach__nd_Id__47DBAE45");

            entity.HasOne(d => d.Os).WithMany(p => p.Saches)
                .HasForeignKey(d => d.OsId)
                .HasConstraintName("os_Id");

            entity.HasOne(d => d.Tg).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TgId)
                .HasConstraintName("FK__Sach__tg_Id__46E78A0C");

            entity.HasOne(d => d.Tl).WithMany(p => p.Saches)
                .HasForeignKey(d => d.TlId)
                .HasConstraintName("FK__Sach__tl_Id__48CFD27E");
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.TgId).HasName("PK__TacGia__006D09FB5502910F");

            entity.Property(e => e.TgId).HasColumnName("tg_Id");
            entity.Property(e => e.TgTenTacGia)
                .HasMaxLength(100)
                .HasColumnName("tg_TenTacGia");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.TlId).HasName("PK__TheLoai__8506AC86D517739E");

            entity.ToTable("TheLoai");

            entity.Property(e => e.TlId).HasColumnName("tl_Id");
            entity.Property(e => e.DmId).HasColumnName("dm_Id");
            entity.Property(e => e.TlTenTheLoai)
                .HasMaxLength(100)
                .HasColumnName("tl_TenTheLoai");

            entity.HasOne(d => d.Dm).WithMany(p => p.TheLoais)
                .HasForeignKey(d => d.DmId)
                .HasConstraintName("FK__TheLoai__dm_Id__44FF419A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
