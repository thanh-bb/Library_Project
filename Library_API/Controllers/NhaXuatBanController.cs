using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NhaXuatBanController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public NhaXuatBanController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select nxb_Id, nxb_TenNhaXuatBan from
                            dbo.NhaXuatBan
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
        public JsonResult Post(NhaXuatBan nxb)
        {
            string query = @"insert into dbo.NhaXuatBan (nxb_TenNhaXuatBan)
                            values (@nxb_TenNhaXuatBan)";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            { 
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nxb_TenNhaXuatBan", nxb.NxbTenNhaXuatBan);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }
            return new JsonResult("Thêm thành công");

        }


        [HttpPut]
        public JsonResult Put (NhaXuatBan nxb)
        {
            string query = @"Update dbo.NhaXuatBan
                        set nxb_TenNhaXuatBan = @nxb_TenNhaXuatBan
                        where nxb_Id = @nxb_Id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nxb_Id", nxb.NxbId);
                    myCommand.Parameters.AddWithValue("@nxb_TenNhaXuatBan", nxb.NxbTenNhaXuatBan);
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
            string checkQuery = "SELECT COUNT(1) FROM dbo.Sach WHERE nxb_Id = @nxb_Id";
            // Câu lệnh SQL xóa bản ghi nếu không có ràng buộc khóa ngoại
            string deleteQuery = "DELETE FROM dbo.NhaXuatBan WHERE nxb_Id = @nxb_Id";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Kiểm tra khóa ngoại trước khi xóa
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, myCon))
                {
                    checkCommand.Parameters.AddWithValue("@nxb_Id", id);
                    int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // Trả về lỗi nếu có sách tham chiếu
                        return new JsonResult("Không thể xóa nhà xuất bản này vì có sách liên quan.");
                    }
                }

                // Xóa nếu không có bản ghi tham chiếu
                using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, myCon))
                {
                    deleteCommand.Parameters.AddWithValue("@nxb_Id", id);
                    deleteCommand.ExecuteNonQuery();
                }

                myCon.Close();
            }

            return new JsonResult("Xóa nhà xuất bản thành công.");
        }




    }
}
