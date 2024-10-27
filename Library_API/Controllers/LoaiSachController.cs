using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiSachController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoaiSachController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select ls_Id, ls_TenLoaiSach from
                            dbo.LoaiSach
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
        public JsonResult Post(LoaiSach ls)
        {
            string query = @"
                    INSERT INTO dbo.LoaiSach (ls_TenLoaiSach)
                    VALUES (@ls_TenLoaiSach)
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ls_TenLoaiSach", ls.LsTenLoaiSach);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }


        [HttpPut]
        public JsonResult Put(LoaiSach ls)
        {
            string query = @"
                            update dbo.Loaisach
                            set ls_TenLoaiSach = @ls_TenLoaiSach
                            where ls_Id=@ls_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ls_Id", ls.LsId);
                    myCommand.Parameters.AddWithValue("@ls_TenLoaiSach", ls.LsTenLoaiSach);
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
            // Kiểm tra xem có sách nào tham chiếu tới loại sách này không
            string checkQuery = "SELECT COUNT(1) FROM dbo.Sach WHERE ls_Id = @ls_Id";
            // Câu lệnh SQL để xóa loại sách nếu không có sách liên quan
            string deleteQuery = "DELETE FROM dbo.LoaiSach WHERE ls_Id = @ls_Id";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Kiểm tra khóa ngoại
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, myCon))
                {
                    checkCommand.Parameters.AddWithValue("@ls_Id", id);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Trả về thông báo lỗi nếu còn sách tham chiếu
                        return new JsonResult("Không thể xóa loại sách này vì có sách liên quan.");
                    }
                }

                // Xóa nếu không có bản ghi tham chiếu
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myCon))
                {
                    deleteCommand.Parameters.AddWithValue("@ls_Id", id);
                    deleteCommand.ExecuteNonQuery();
                }

                myCon.Close();
            }

            return new JsonResult("Xóa loại sách thành công.");
        }

    }
}
