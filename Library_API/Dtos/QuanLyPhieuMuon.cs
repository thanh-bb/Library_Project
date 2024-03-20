namespace Library_API.Dtos
{
    public class QuanLyPhieuMuon
    {
        public int Id_PhieuMuon { get; set; }

        public int Id_User { get; set; }

        public int Id_Sach {  get; set; }
        public string TenSach {  get; set; }

        public int SoLuongSach { get; set; }

        public DateTime NgayMuon { get; set; }

        public DateTime HanTra { get; set; }

        public string TrangThai { get; set; }


    }
}
