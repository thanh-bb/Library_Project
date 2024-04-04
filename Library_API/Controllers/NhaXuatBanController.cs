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
        public JsonResult Delete ( int id)
        {
            string query = @"delete from dbo.NhaXuatBan where nxb_Id = @nxb_Id";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nxb_Id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Xoa thanh cong");
        }



    }
}
