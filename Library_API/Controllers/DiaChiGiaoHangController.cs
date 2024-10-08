using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiaChiGiaoHangController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DiaChiGiaoHangController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select * from
                            dbo.DiaChiGiaoHang
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


        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"
                    select * from
                    dbo.DiaChiGiaoHang
                    where nd_Id = @nd_Id
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Id", id); 
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(DiaChiGiaoHang dcgh)
        {
            string query = @"
                    INSERT INTO dbo.DiaChiGiaoHang
                    ( nd_Id, dcgh_TenNguoiNhan, dcgh_SoDienThoai, dcgh_DiaChi)
             VALUES ( @nd_Id, @dcgh_TenNguoiNhan, @dcgh_SoDienThoai, @dcgh_DiaChi)
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Id", dcgh.NdId);
                    myCommand.Parameters.AddWithValue("@dcgh_TenNguoiNhan", dcgh.DcghTenNguoiNhan);
                    myCommand.Parameters.AddWithValue("@dcgh_SoDienThoai", dcgh.DcghSoDienThoai);
                    myCommand.Parameters.AddWithValue("@dcgh_DiaChi", dcgh.DcghDiaChi);
                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }



        [HttpPut]
        public JsonResult Put(DiaChiGiaoHang dcgh)
        {
            string query = @"
                            update dbo.DiaChiGiaoHang
                            set dcgh_TenNguoiNhan = @dcgh_TenNguoiNhan,
                                dcgh_SoDienThoai = @dcgh_SoDienThoai,
                                dcgh_DiaChi = @dcgh_DiaChi
                            where dcgh_Id=@dcgh_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@dcgh_Id", dcgh.DcghId);
                    myCommand.Parameters.AddWithValue("@dcgh_TenNguoiNhan", dcgh.DcghTenNguoiNhan);
                    myCommand.Parameters.AddWithValue("@dcgh_SoDienThoai", dcgh.DcghSoDienThoai);
                    myCommand.Parameters.AddWithValue("@dcgh_DiaChi", dcgh.DcghDiaChi);
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
            string query = @"
                            delete from dbo.DiaChiGiaoHang
                            where dcgh_Id=@dcgh_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@dcgh_Id", id);
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
