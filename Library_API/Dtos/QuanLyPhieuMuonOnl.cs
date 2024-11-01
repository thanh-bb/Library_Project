namespace Library_API.Dtos
{
    public class QuanLyPhieuMuonOnl
    {
        public int PmoId { get; set; }

        public int? NdId { get; set; }

        public DateTime? PmoNgayDat { get; set; }

        public string? PmoLoaiGiaoHang { get; set; }

        public string? PmoPhuongThucThanhToan { get; set; }

        public int? DcghId { get; set; }

        public string? PmoTrangThai { get; set; }

        public int? SId { get; set; }

        public int? CtpmoSoLuongSachMuon { get; set; }

        public string? TenSach { get; set; }

        public int SoLuongSach { get; set; }
        public DateTime HanTra { get; set; }

        public string? TtPhuongThuc { get; set; }

        public string? TtTrangThai { get; set; }

        public DateTime? TtNgayThanhToan { get; set; }

        public double? TtSoTien { get; set; }

    }
}
