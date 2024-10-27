using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public TheLoaiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select tl_Id, tl_TenTheLoai, dm_Id from
                            dbo.TheLoai
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
        public JsonResult Post(TheLoai tl)
        {
            string query = @"
                    insert into dbo.TheLoai
                    (tl_TenTheLoai,dm_Id)
                    values (@tl_TenTheLoai,@dm_Id)                            
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                try
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@tl_TenTheLoai", tl.TlTenTheLoai);
                        myCommand.Parameters.AddWithValue("@dm_Id", tl.DmId);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                    }
                    return new JsonResult("Thêm thành công");
                }
                catch (SqlException ex)
                {
                    // Handle specific SQL errors
                    if (ex.Number == 547)  // FK constraint violation error number
                    {
                        return new JsonResult("Thêm không thành công. Mã Danh Mục không tồn tại.");
                    }
                    else
                    {
                        // Handle other errors
                        return new JsonResult("Thêm không thành công. Lỗi: " + ex.Message);
                    }
                }
                finally
                {
                    myCon.Close();
                }
            }
        }



        [HttpPut]
        public JsonResult Put(TheLoai tl)
        {
            string query = @"
                    UPDATE dbo.Theloai
                    SET tl_TenTheLoai = @tl_TenTheLoai,
                        dm_Id = @dm_Id
                    WHERE tl_Id = @tl_Id
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@tl_Id", tl.TlId);
                    myCommand.Parameters.AddWithValue("@tl_TenTheLoai", tl.TlTenTheLoai);
                    myCommand.Parameters.AddWithValue("@dm_Id", tl.DmId); // Thêm giá trị dm_Id vào tham số
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Cập nhật thành công");
        }



        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            // Câu lệnh SQL kiểm tra khóa ngoại
            string checkQuery = "SELECT COUNT(1) FROM dbo.Sach WHERE tl_Id = @tl_Id";
            // Câu lệnh SQL xóa bản ghi nếu không có ràng buộc
            string deleteQuery = "DELETE FROM dbo.TheLoai WHERE tl_Id = @tl_Id";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Kiểm tra xem có sách nào tham chiếu tới thể loại này không
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, myCon))
                {
                    checkCommand.Parameters.AddWithValue("@tl_Id", id);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Trả về lỗi nếu có sách tham chiếu
                        return new JsonResult("Không thể xóa thể loại này vì có sách liên quan.");
                    }
                }

                // Xóa nếu không có bản ghi tham chiếu
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myCon))
                {
                    deleteCommand.Parameters.AddWithValue("@tl_Id", id);
                    deleteCommand.ExecuteNonQuery();
                }

                myCon.Close();
            }

            return new JsonResult("Xóa thể loại thành công.");
        }

    }
}
