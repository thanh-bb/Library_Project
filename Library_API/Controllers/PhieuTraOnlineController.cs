using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuTraOnlineController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public PhieuTraOnlineController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                SELECT pto.*, nd.nd_HoTen
                FROM dbo.PhieuTraOnline pto
                INNER JOIN dbo.NguoiDung nd ON pto.nd_Id = nd.nd_Id
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(PhieuTraOnline pto)
        {
            string query = @"
                INSERT INTO dbo.PhieuTraOnline
                (pmo_Id, nd_Id, pto_NgayTra)
                VALUES (@pmo_Id, @nd_Id, @pto_NgayTra);
                SELECT SCOPE_IDENTITY();
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            int newPtoId;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@pmo_Id", pto.PmoId);
                    myCommand.Parameters.AddWithValue("@nd_Id", pto.NdId);
                    myCommand.Parameters.AddWithValue("@pto_NgayTra", pto.PtoNgayTra);
                  

                    newPtoId = Convert.ToInt32(myCommand.ExecuteScalar());
                }

                // Insert into ChiTietPhieuTraOnline
                foreach (var chiTiet in pto.ChiTietPhieuTraOnlines)
                {
                    string insertChiTietQuery = @"
                        INSERT INTO dbo.ChiTietPhieuTraOnline
                        (pto_Id, s_Id, ctpto_SoLuongSachTra)
                        VALUES (@pto_Id, @s_Id, @ctpto_SoLuongSachTra);
                    ";

                    using (SqlCommand insertChiTietCommand = new SqlCommand(insertChiTietQuery, myCon))
                    {
                        insertChiTietCommand.Parameters.AddWithValue("@pto_Id", newPtoId);
                        insertChiTietCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        insertChiTietCommand.Parameters.AddWithValue("@ctpto_SoLuongSachTra", chiTiet.CtptoSoLuongSachTra);
                        insertChiTietCommand.ExecuteNonQuery();
                    }

                    // Update book stock after return
                    string updateSoLuongQuery = @"
                        UPDATE dbo.Sach
                        SET s_SoLuong = s_SoLuong + @ctpto_SoLuongSachTra
                        WHERE s_Id = @s_Id;
                    ";

                    using (SqlCommand updateSoLuongCommand = new SqlCommand(updateSoLuongQuery, myCon))
                    {
                        updateSoLuongCommand.Parameters.AddWithValue("@ctpto_SoLuongSachTra", chiTiet.CtptoSoLuongSachTra);
                        updateSoLuongCommand.Parameters.AddWithValue("@s_Id", chiTiet.SId);
                        updateSoLuongCommand.ExecuteNonQuery();
                    }
                }

                myCon.Close();
            }

            return new JsonResult(new { Success = true, PhieuTraOnlineId = newPtoId });
        }
    }
}
