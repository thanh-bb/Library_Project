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



    }


}
