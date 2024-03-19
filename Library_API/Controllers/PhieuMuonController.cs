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

        [HttpPost]
        public JsonResult Post(PhieuMuon pm)
        {
            string query = @"
        INSERT INTO dbo.PhieuMuon
        (pm_NgayMuon, pm_HanTra, pm_TrangThai, nd_Id)
        VALUES (@pm_NgayMuon, @pm_HanTra, @pm_TrangThai, @nd_Id);
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
                    myCommand.Parameters.AddWithValue("@pm_TrangThai", pm.PmTrangThai);
                    myCommand.Parameters.AddWithValue("@nd_Id", pm.NdId);

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
                            string updateTrangThaiQuery = @"
                            UPDATE dbo.Sach
                            SET s_TrangThaiMuon = 'false'
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
                            update dbo.PhieuMuon
                            set pm_TrangThai = @pm_TrangThai
                            where pm_Id=@pm_Id
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
                    myCommand.Parameters.AddWithValue("@pm_TrangThai", pm.PmTrangThai);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Cập nhật trạng thái phiếu mượn thành công");
        }

        [HttpPost("approve")]
        public IActionResult Approve(PhieuMuon pm)
        {
            // Kiểm tra xem pm có trạng thái "Đã trả" không
            if (pm.PmTrangThai != "Đã trả")
            {
                return BadRequest("Phiếu mượn chưa được trả");
            }

            // Tạo mới Phiếu Trả
            PhieuTra pt = new PhieuTra
            {
                PtNgayTra = DateTime.Now, // Ngày trả là ngày hiện tại
                NdId = pm.NdId // Người dùng trả sách
            };

            // Thêm Phiếu Trả vào cơ sở dữ liệu
            // Lưu lại ID của Phiếu Trả mới được tạo
            int newPtId;
            string insertPtQuery = @"
        INSERT INTO dbo.PhieuTra (pt_NgayTra, nd_Id)
        VALUES (@pt_NgayTra, @nd_Id);
        SELECT SCOPE_IDENTITY(); -- Lấy ID của phiếu trả mới thêm vào
    ";
            using (SqlConnection myCon = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                myCon.Open();
                using (SqlCommand insertPtCommand = new SqlCommand(insertPtQuery, myCon))
                {
                    insertPtCommand.Parameters.AddWithValue("@pt_NgayTra", pt.PtNgayTra);
                    insertPtCommand.Parameters.AddWithValue("@nd_Id", pt.NdId);
                    newPtId = Convert.ToInt32(insertPtCommand.ExecuteScalar());
                }

                // Tạo chi tiết phiếu trả cho mỗi sách được mượn trong phiếu mượn đã được duyệt
                foreach (var chiTiet in pm.ChiTietPhieuMuons)
                {
                    string insertCtPtQuery = @"
                INSERT INTO dbo.ChiTietPhieuTra (pt_Id, s_Id, ctpt_SoLuongSachTra)
                VALUES (@pt_Id, @s_Id, @ctpt_SoLuongSachTra);
            ";
                    using (SqlCommand insertCtPtCommand = new SqlCommand(insertCtPtQuery, myCon))
                    {
                        insertCtPtCommand.Parameters.AddWithValue("@pt_Id", newPtId); // Sử dụng ID mới của phiếu trả
                        insertCtPtCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        insertCtPtCommand.Parameters.AddWithValue("@ctpt_SoLuongSachTra", chiTiet.CtpmSoLuongSachMuon); // Số lượng sách trả sẽ giống với số lượng sách mượn
                        insertCtPtCommand.ExecuteNonQuery();
                    }

                    // Cập nhật số lượng sách trong kho nếu cần thiết
                    // Và cập nhật trạng thái mượn của sách
                    // Các bước này tương tự như bạn đã thực hiện trong phương thức POST của PhieuMuonController
                }

                myCon.Close();
            }

            return Ok("Đã tạo phiếu trả thành công");
        }

    }


}
