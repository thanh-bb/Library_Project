using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Library_API.Dtos;


namespace Library_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PhieuMuonOnlineController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PhieuMuonOnlineController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("Count/{userId}")]
        public JsonResult GetNumberOfBorrowReceiptsForUserInMonth(int userId)
        {
            // Get the current month and year
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            string query = @"
        SELECT COUNT(*) AS NumberOfBorrowReceipts
        FROM dbo.PhieuMuonOnline
        WHERE MONTH(pmo_NgayDat) = @Month 
        AND YEAR(pmo_NgayDat) = @Year
        AND nd_Id = @UserId
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Month", currentMonth);
                    myCommand.Parameters.AddWithValue("@Year", currentYear);
                    myCommand.Parameters.AddWithValue("@UserId", userId);
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            // Retrieve the count from the DataTable
            int numberOfBorrowReceipts = Convert.ToInt32(table.Rows[0]["NumberOfBorrowReceipts"]);

            return new JsonResult(numberOfBorrowReceipts);
        }


        [HttpGet("CheckMuonOnl/{nd_Id}/{s_Id}")]
        public JsonResult CheckMuonOnl(int nd_Id, int s_Id)
        {
            string query = @"
                SELECT pmo.pmo_TrangThai
                FROM dbo.PhieuMuonOnline pmo
                INNER JOIN dbo.ChiTietPhieuMuonOnline ctpmo ON pmo.pmo_Id = ctpmo.pmo_Id
                WHERE pmo.nd_Id = @nd_Id AND ctpmo.s_Id = @s_Id ";

            string result = "không có kết quả trả về"; // Mặc định là "No" nếu không có kết quả trả về

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Id", nd_Id);
                    myCommand.Parameters.AddWithValue("@s_Id", s_Id);
                    myCon.Open();
                    object queryResult = myCommand.ExecuteScalar();
                    if (queryResult != null)
                    {
                        result = queryResult.ToString();
                    }
                }
            }

            return new JsonResult(result);
        }

        [HttpGet("{ndId}")]
        public JsonResult Get(int ndId)
        {
            string query = @"
              SELECT DISTINCT pmo.pmo_Id, pmo.nd_Id, pmo.pmo_NgayDat, pmo.pmo_HanTra, pmo_LoaiGiaoHang, 
               pmo.pmo_TrangThai, pmo.pmo_PhuongThucThanhToan, pmo.dcgh_Id,
               ctpmo.s_Id, ctpmo.ctpmo_SoLuongSachMuon, 
               s.s_TenSach, 
               tt.tt_PhuongThuc, tt.tt_TrangThai, tt.tt_NgayThanhToan, tt.tt_SoTien
                FROM dbo.PhieuMuonOnline pmo
                INNER JOIN dbo.ChiTietPhieuMuonOnline ctpmo ON pmo.pmo_Id = ctpmo.pmo_Id
                INNER JOIN dbo.Sach s ON ctpmo.s_Id = s.s_Id
                LEFT JOIN dbo.ThanhToan tt ON pmo.pmo_Id = tt.pmo_Id
                WHERE pmo.nd_Id = @NdId

    ";

            List<QuanLyPhieuMuonOnl> quanLyPhieuMuonOnls = new List<QuanLyPhieuMuonOnl>();

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NdId", ndId);
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        QuanLyPhieuMuonOnl phieuMuonOnl = new QuanLyPhieuMuonOnl
                        {
                            PmoId = Convert.ToInt32(myReader["pmo_Id"]),
                            NdId = myReader["nd_Id"] as int?,
                            SId = myReader["s_Id"] as int?,
                            TenSach = myReader["s_TenSach"].ToString(),
                            SoLuongSach = Convert.ToInt32(myReader["ctpmo_SoLuongSachMuon"]),
                            PmoNgayDat = myReader["pmo_NgayDat"] as DateTime?,
                            HanTra = Convert.ToDateTime(myReader["pmo_HanTra"]),
                            PmoTrangThai = myReader["pmo_TrangThai"].ToString(),
                            PmoLoaiGiaoHang = myReader["pmo_LoaiGiaoHang"].ToString(),
                            PmoPhuongThucThanhToan = myReader["pmo_PhuongThucThanhToan"].ToString(),
                            DcghId = myReader["dcgh_Id"] as int?,
                            // Thông tin thanh toán
                            TtPhuongThuc = myReader["tt_PhuongThuc"].ToString(),
                            TtTrangThai = myReader["tt_TrangThai"].ToString(),
                            TtNgayThanhToan = myReader["tt_NgayThanhToan"] as DateTime?,
                            TtSoTien = myReader["tt_SoTien"] != DBNull.Value ? Convert.ToDouble(myReader["tt_SoTien"]) : (double?)null

                    };
                        quanLyPhieuMuonOnls.Add(phieuMuonOnl);
                    }

                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuMuonOnls);
        }


    }


}
