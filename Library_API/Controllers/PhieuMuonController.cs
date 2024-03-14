using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

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
                }

                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }


    }


}
