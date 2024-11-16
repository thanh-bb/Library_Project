using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;


namespace Library_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuMuonController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PhieuMuonController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select * from
                            dbo.PhieuMuon
                            "
            ;

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {

            string query = @"
                            select * from
                            dbo.PhieuMuon
                            where nd_Id= @Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(PhieuMuon pm)
        {
            string query = @"
    INSERT INTO dbo.PhieuMuon
    (pm_NgayMuon, pm_HanTra, ttm_Id, nd_Id, pm_TrangThaiXetDuyet, pm_LoaiMuon)
    VALUES (@pm_NgayMuon, @pm_HanTra, @ttm_Id, @nd_Id, @pm_TrangThaiXetDuyet, @pm_LoaiMuon);
    SELECT SCOPE_IDENTITY(); -- Lấy ID của phiếu mượn mới thêm vào
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            int newPmId;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@pm_NgayMuon", pm.PmNgayMuon);
                    myCommand.Parameters.AddWithValue("@pm_HanTra", pm.PmHanTra);
                    myCommand.Parameters.AddWithValue("@ttm_Id", pm.TtmId); // Tham chiếu đến ttm_Id thay vì pm_TrangThaiMuon
                    myCommand.Parameters.AddWithValue("@nd_Id", pm.NdId);
                    myCommand.Parameters.AddWithValue("@pm_TrangThaiXetDuyet", pm.PmTrangThaiXetDuyet);
                    myCommand.Parameters.AddWithValue("@pm_LoaiMuon", pm.PmLoaiMuon);

                    // Thực hiện lấy ID của phiếu mượn mới thêm vào
                    newPmId = Convert.ToInt32(myCommand.ExecuteScalar());
                }

                // Thêm chi tiết phiếu mượn
                foreach (var chiTiet in pm.ChiTietPhieuMuons)
                {
                    string insertChiTietQuery = @"
            INSERT INTO dbo.ChiTietPhieuMuon
            (pm_Id, s_Id, ctpm_SoLuongSachMuon)
            VALUES (@pm_Id, @s_Id, @ctpm_SoLuongSachMuon)
            ";

                    using (SqlCommand insertChiTietCommand = new SqlCommand(insertChiTietQuery, myCon))
                    {
                        insertChiTietCommand.Parameters.AddWithValue("@pm_Id", newPmId); // Sử dụng ID mới của phiếu mượn
                        insertChiTietCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        insertChiTietCommand.Parameters.AddWithValue("@ctpm_SoLuongSachMuon", chiTiet.CtpmSoLuongSachMuon);
                        insertChiTietCommand.ExecuteNonQuery();
                    }

                    // Cập nhật số lượng sách trong kho
                    string updateSoLuongQuery = @"
            UPDATE dbo.Sach
            SET s_SoLuong = s_SoLuong - @ctpm_SoLuongSachMuon
            WHERE s_Id = @s_Id
            ";

                    using (SqlCommand updateSoLuongCommand = new SqlCommand(updateSoLuongQuery, myCon))
                    {
                        updateSoLuongCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        updateSoLuongCommand.Parameters.AddWithValue("@ctpm_SoLuongSachMuon", chiTiet.CtpmSoLuongSachMuon);
                        updateSoLuongCommand.ExecuteNonQuery();
                    }

                    // Kiểm tra và cập nhật trạng thái mượn của sách
                    string checkSoLuongQuery = @"
            SELECT s_SoLuong
            FROM dbo.Sach
            WHERE s_Id = @s_Id
            ";

                    using (SqlCommand checkSoLuongCommand = new SqlCommand(checkSoLuongQuery, myCon))
                    {
                        checkSoLuongCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        int soLuongConLai = Convert.ToInt32(checkSoLuongCommand.ExecuteScalar());

                        if (soLuongConLai == 0)
                        {
                            // Cập nhật ttm_Id cho trạng thái mượn của sách (giả sử ttm_Id = 4 là "Hết sách")
                            string updateTrangThaiQuery = @"
                    UPDATE dbo.Sach
                    SET ttm_Id = 4
                    WHERE s_Id = @s_Id
                    ";

                            using (SqlCommand updateTrangThaiCommand = new SqlCommand(updateTrangThaiQuery, myCon))
                            {
                                updateTrangThaiCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                                updateTrangThaiCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }



        [HttpPut]
        public JsonResult Put(PhieuMuon pm)
        {
            string query = @"
                    UPDATE dbo.PhieuMuon
                    SET ttm_Id = @ttm_Id
                    WHERE pm_Id = @pm_Id
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@pm_Id", pm.PmId);
                    myCommand.Parameters.AddWithValue("@ttm_Id", pm.TtmId); // Tham chiếu tới ttm_Id thay vì pm_TrangThaiMuon
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Cập nhật trạng thái phiếu mượn thành công");
        }



        [HttpPut("XetDuyet")]
        public JsonResult XetDuyet(PhieuMuon pm)
        {
            string query = @"
        UPDATE dbo.PhieuMuon
        SET pm_TrangThaiXetDuyet = @pm_TrangThaiXetDuyet
        WHERE pm_Id = @pm_Id
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@pm_Id", pm.PmId);
                    myCommand.Parameters.AddWithValue("@pm_TrangThaiXetDuyet", pm.PmTrangThaiXetDuyet);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                // Kiểm tra nếu trạng thái xét duyệt là "Đã xét duyệt" thì cập nhật trạng thái mượn thành "Đang mượn"
                if (pm.PmTrangThaiXetDuyet == "Đã xét duyệt")
                {
                    // Cập nhật `ttm_Id` thành 1 (giả sử 1 là `ttm_Id` cho "Đang mượn")
                    string updateTrangThaiMuonQuery = @"
                UPDATE dbo.PhieuMuon
                SET ttm_Id = 1  -- 1 là mã cho trạng thái 'Đang mượn'
                WHERE pm_Id = @pm_Id
            ";

                    using (SqlCommand updateCommand = new SqlCommand(updateTrangThaiMuonQuery, myCon))
                    {
                        updateCommand.Parameters.AddWithValue("@pm_Id", pm.PmId);
                        updateCommand.ExecuteNonQuery();
                    }
                }

                myCon.Close();
            }

            return new JsonResult("Cập nhật trạng thái xét duyệt và trạng thái mượn thành công");
        }



        [HttpPost("CapNhatTrangThai")]
        public JsonResult CapNhatTrangThai()
        {
            string query = @"
        UPDATE dbo.PhieuMuon
        SET ttm_Id = 
            CASE
                WHEN ttm_Id = '1' AND DATEDIFF(day, GETDATE(), pm_HanTra) = -1 THEN '2'
                ELSE ttm_Id
            END
        WHERE ttm_Id = '1' AND DATEDIFF(day, GETDATE(), pm_HanTra) = -1
    ";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCon.Open();
                    myCommand.ExecuteNonQuery();
                    myCon.Close();
                }
            }

            return new JsonResult("Cập nhật trạng thái phiếu mượn thành công");
        }



        [HttpGet("Count/{userId}")]
        public JsonResult GetNumberOfBorrowReceiptsForUserInMonth(int userId)
        {
            // Get the current month and year
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            string query = @"
    SELECT COUNT(*) AS NumberOfBorrowReceipts
    FROM dbo.PhieuMuon
    WHERE MONTH(pm_NgayMuon) = @Month 
    AND YEAR(pm_NgayMuon) = @Year
    AND nd_Id = @UserId 
    AND (pm_TrangThaiXetDuyet = N'Chờ xét duyệt' OR (ttm_Id = 1  OR ttm_Id = 2 OR ttm_Id = 3 OR ttm_Id = 4))
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


    }


}
