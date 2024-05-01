using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuTraController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PhieuTraController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                    SELECT pt.*, nd.nd_HoTen
                    FROM dbo.PhieuTra pt
                    INNER JOIN dbo.NguoiDung nd ON pt.nd_Id = nd.nd_Id
                    ";

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
        public JsonResult Post(PhieuTra pt)
        {

            string query = @"
            INSERT INTO dbo.PhieuTra
            ( pt_NgayTra, nd_Id, pm_Id)
            VALUES ( @pt_NgayTra, @nd_Id, @pm_Id);
            SELECT SCOPE_IDENTITY(); -- Lấy ID của phiếu trả mới thêm vào
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            int newPtId;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))            
                {
                    
                    myCommand.Parameters.AddWithValue("@pt_NgayTra", pt.PtNgayTra);                  
                    myCommand.Parameters.AddWithValue("@nd_Id", pt.NdId);
                    myCommand.Parameters.AddWithValue("@pm_Id", pt.PmId);
                    // Thực hiện lấy ID của phiếu mượn mới thêm vào
                    newPtId = Convert.ToInt32(myCommand.ExecuteScalar());
                }

                // Thêm chi tiết phiếu mượn
                foreach (var chiTiet in pt.ChiTietPhieuTras)
                {
                    string insertChiTietQuery = @"
                    INSERT INTO dbo.ChiTietPhieuTra
                    (s_Id, pt_Id, ctpt_SoLuongSachTra)
                    VALUES (@s_Id, @pt_Id, @ctpt_SoLuongSachTra)
                    ";

                    using (SqlCommand insertChiTietCommand = new SqlCommand(insertChiTietQuery, myCon))
                    {
                        insertChiTietCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        insertChiTietCommand.Parameters.AddWithValue("@pt_Id", newPtId); // Sử dụng ID mới của phiếu mượn                       
                        insertChiTietCommand.Parameters.AddWithValue("@ctpt_SoLuongSachTra", chiTiet.CtptSoLuongSachTra);
                        insertChiTietCommand.ExecuteNonQuery();
                    }
                    // Cập nhật số lượng sách trả vào bảng sách
                    string updateSoLuongQuery = @"
                     UPDATE dbo.Sach
                     SET s_SoLuong = s_SoLuong + @ctpt_SoLuongSachTra
                     WHERE s_Id = @s_Id
                      ";

                    using (SqlCommand updateSoLuongCommand = new SqlCommand(updateSoLuongQuery, myCon))
                    {
                        updateSoLuongCommand.Parameters.AddWithValue("@ctpt_SoLuongSachTra", chiTiet.CtptSoLuongSachTra);
                        updateSoLuongCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
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

                        if (soLuongConLai > 0)
                        {
                            string updateTrangThaiQuery = @"
                            UPDATE dbo.Sach
                            SET s_TrangThaiMuon = 'true'
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

            return new JsonResult(table);
        }




    }
}