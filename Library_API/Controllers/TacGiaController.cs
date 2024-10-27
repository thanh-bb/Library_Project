using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TacGiumController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TacGiumController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select tg_Id, tg_TenTacGia from
                            dbo.TacGia
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
        public JsonResult Post(TacGium tg)
        {
            string query = @"
                    INSERT INTO dbo.TacGia (tg_TenTacGia)
                    VALUES (@tg_TenTacGia)
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@tg_TenTacGia", tg.TgTenTacGia);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }


        [HttpPut]
        public JsonResult Put(TacGium tg)
        {
            string query = @"
                            update dbo.TacGia
                            set tg_TenTacGia = @tg_TenTacGia
                            where tg_Id=@tg_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@tg_Id", tg.TgId);
                    myCommand.Parameters.AddWithValue("@tg_TenTacGia", tg.TgTenTacGia);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            // Câu lệnh SQL kiểm tra khóa ngoại
            string checkQuery = "SELECT COUNT(1) FROM dbo.Sach WHERE tg_Id = @tg_Id";
            // Câu lệnh SQL xóa bản ghi nếu không có ràng buộc khóa ngoại
            string deleteQuery = "DELETE FROM dbo.TacGia WHERE tg_Id = @tg_Id";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Kiểm tra khóa ngoại trước khi xóa
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, myCon))
                {
                    checkCommand.Parameters.AddWithValue("@tg_Id", id);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Trả về lỗi nếu có sách tham chiếu
                        return new JsonResult("Không thể xóa tác giả này vì có sách liên quan.");
                    }
                }

                // Xóa nếu không có bản ghi tham chiếu
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myCon))
                {
                    deleteCommand.Parameters.AddWithValue("@tg_Id", id);
                    deleteCommand.ExecuteNonQuery();
                }

                myCon.Close();
            }

            return new JsonResult("Xóa tác giả thành công.");
        }


    }
}
