using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Library_API.Models;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DanhMucController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select dm_Id, dm_TenDanhMuc from
                            dbo.DanhMuc
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
        public JsonResult Post(DanhMuc dm)
        {
            string query = @"
                    INSERT INTO dbo.DanhMuc (dm_TenDanhMuc)
                    VALUES (@dm_TenDanhMuc)
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@dm_TenDanhMuc", dm.DmTenDanhMuc);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }


        [HttpPut]
        public JsonResult Put(DanhMuc dm)
        {
            string query = @"
                            update dbo.Danhmuc
                            set dm_TenDanhMuc = @dm_TenDanhMuc
                            where dm_Id=@dm_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@dm_Id", dm.DmId);
                    myCommand.Parameters.AddWithValue("@dm_TenDanhMuc", dm.DmTenDanhMuc);
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
            string checkQuery = "SELECT COUNT(1) FROM dbo.TheLoai WHERE dm_Id = @dm_Id";
            // Câu lệnh SQL xóa bản ghi nếu không có bản ghi liên quan
            string deleteQuery = "DELETE FROM dbo.DanhMuc WHERE dm_Id = @dm_Id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Kiểm tra xem có bất kỳ thể loại nào tham chiếu đến danh mục này không
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, myCon))
                {
                    checkCommand.Parameters.AddWithValue("@dm_Id", id);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Nếu có bản ghi tham chiếu, trả về lỗi
                        return new JsonResult("Không thể xóa danh mục này vì có thể loại liên quan.");
                    }
                }

                // Nếu không có bản ghi tham chiếu, tiến hành xóa danh mục
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myCon))
                {
                    deleteCommand.Parameters.AddWithValue("@dm_Id", id);
                    deleteCommand.ExecuteNonQuery();
                }

                myCon.Close();
            }

            return new JsonResult("Xóa danh mục thành công.");
        }

    }
}
