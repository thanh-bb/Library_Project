using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungDangKyController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public NguoiDungDangKyController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select *
                            from
                            dbo.NguoiDungDangKy
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


        [HttpGet("GetSomeInfor")]
        public JsonResult GetSomeInfor()
        {
            string query = @"
                            select nddk_Id, nddk_HoTen, nddk_NgayDangKy, nddk_CCCD, nddk_Email, nddk_TrangThaiDuyet
                            from
                            dbo.NguoiDungDangKy
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
            string query = @" select * from dbo.NguoiDungDangKy where nddk_Id = @Id ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


    

    [HttpPost]
        public JsonResult Post(NguoiDungDangKy nddk)
        {
            string query = @"
                            insert into dbo.NguoiDungDangKy
                            (nddk_HoTen, nddk_CCCD, nddk_CCCD_MatTruoc, nddk_CCCD_MatSau, nddk_HinhThe,
                            nddk_NgaySinh, nddk_GioiTinh, nddk_Email,nddk_SoDienThoai, nddk_DiaChi, nddk_NgayDangKy, 
                            nddk_ThoiGianSuDung, nddk_HinhThucTraPhi, nddk_TrangThaiThanhToan, nddk_TrangThaiDuyet)
                            VALUES 
                            (@nddk_HoTen, @nddk_CCCD, @nddk_CCCD_MatTruoc, @nddk_CCCD_MatSau, @nddk_HinhThe,
                            @nddk_NgaySinh, @nddk_GioiTinh, @nddk_Email,@nddk_SoDienThoai, @nddk_DiaChi, @nddk_NgayDangKy, 
                            @nddk_ThoiGianSuDung, @nddk_HinhThucTraPhi, @nddk_TrangThaiThanhToan, @nddk_TrangThaiDuyet)
                            SELECT SCOPE_IDENTITY(); 
";
            int newNddkId;
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
        //    SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nddk_HoTen", nddk.NddkHoTen);
                    myCommand.Parameters.AddWithValue("@nddk_CCCD", nddk.NddkCccd);
                    myCommand.Parameters.AddWithValue("@nddk_CCCD_MatTruoc", nddk.NddkCccdMatTruoc);
                    myCommand.Parameters.AddWithValue("@nddk_CCCD_MatSau", nddk.NddkCccdMatSau);
                    myCommand.Parameters.AddWithValue("@nddk_NgaySinh", nddk.NddkNgaySinh);
                    myCommand.Parameters.AddWithValue("@nddk_HinhThe", nddk.NddkHinhThe);
                    myCommand.Parameters.AddWithValue("@nddk_GioiTinh", nddk.NddkGioiTinh);
                    myCommand.Parameters.AddWithValue("@nddk_Email", nddk.NddkEmail);
                    myCommand.Parameters.AddWithValue("@nddk_SoDienThoai", nddk.NddkSoDienThoai);
                    myCommand.Parameters.AddWithValue("@nddk_DiaChi", nddk.NddkDiaChi);
                    myCommand.Parameters.AddWithValue("@nddk_NgayDangKy", nddk.NddkNgayDangKy);
                    myCommand.Parameters.AddWithValue("@nddk_ThoiGianSuDung", nddk.NddkThoiGianSuDung);
                    myCommand.Parameters.AddWithValue("@nddk_HinhThucTraPhi", nddk.NddkHinhThucTraPhi);
                    myCommand.Parameters.AddWithValue("@nddk_TrangThaiThanhToan", nddk.NddkTrangThaiThanhToan);
                    myCommand.Parameters.AddWithValue("@nddk_TrangThaiDuyet", nddk.NddkTrangThaiDuyet);

                    newNddkId = Convert.ToInt32(myCommand.ExecuteScalar());


                    //myReader = myCommand.ExecuteReader();
                    //table.Load(myReader);
                    //myReader.Close();
                 
                } 
                myCon.Close();
            }
            return new JsonResult(newNddkId);
        }

        //[Route("SaveFile")]
        //[HttpPost]
        //public async Task<IActionResult> SaveFile()
        //{
        //    try
        //    {
        //        var httpRequest = Request.Form;
        //        var postedFile = httpRequest.Files[0];

        //        using (var ms = new MemoryStream())
        //        {
        //            await postedFile.CopyToAsync(ms);
        //            byte[] fileBytes = ms.ToArray();
        //            string base64String = Convert.ToBase64String(fileBytes);

        //            // Optionally, save the Base64 string to the database here

        //            return new JsonResult(new { base64String });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = "Error processing file.", error = ex.Message });
        //    }
        //}

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("hello.png");
            }
        }


        [HttpPut]
        public JsonResult Put(NguoiDungDangKy nddk)
        {
            string query = @"
                            update dbo.NguoiDungDangKy
                            set nddk_TrangThaiDuyet = @nddk_TrangThaiDuyet
                            where nddk_Id=@nddk_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nddk_Id", nddk.NddkId);
                    myCommand.Parameters.AddWithValue("@nddk_TrangThaiDuyet", nddk.NddkTrangThaiDuyet);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPut("UpdateTrangThaiThanhToan")]
        public JsonResult UpdateTrangThaiThanhToan(NguoiDungDangKy nddk)
        {
            string query = @"
                            update dbo.NguoiDungDangKy
                            set nddk_TrangThaiThanhToan = @nddk_TrangThaiThanhToan ,nddk_SoTien = @nddk_SoTien
                               WHERE nddk_Id = @nddk_Id;
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nddk_Id", nddk.NddkId);
                    myCommand.Parameters.AddWithValue("@nddk_TrangThaiThanhToan", nddk.NddkTrangThaiThanhToan);
                    myCommand.Parameters.AddWithValue("@nddk_SoTien", nddk.NddkSoTien);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

    }
}
