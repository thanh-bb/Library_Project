using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HinhMinhHoaController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public HinhMinhHoaController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @" select * from dbo.HinhMinhHoa where s_Id = @Id ";

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
        public JsonResult Post(HinhMinhHoa hmh)
        {
            string query = @"
             INSERT INTO dbo.HinhMinhHoa (s_Id, hmh_HinhAnhMaHoa)
             VALUES (@s_Id, @hmh_HinhAnhMaHoa)
            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@s_Id", hmh.SId);
                    myCommand.Parameters.AddWithValue("@hmh_HinhAnhMaHoa", hmh.HmhHinhAnhMaHoa);
                    myCommand.ExecuteNonQuery();
                }

                myCon.Close();
            }

            return new JsonResult("Thêm ảnh thành công");
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
        public JsonResult Put(HinhMinhHoa hinh)
        {
            string query = @"
        UPDATE dbo.HinhMinhHoa
        SET hmh_HinhAnhMaHoa = @hmh_HinhAnhMaHoa
        WHERE hmh_Id = @hmh_Id AND s_Id = @s_Id
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@hmh_Id", hinh.HmhId);
                    myCommand.Parameters.AddWithValue("@s_Id", hinh.SId);
                    myCommand.Parameters.AddWithValue("@hmh_HinhAnhMaHoa", hinh.HmhHinhAnhMaHoa);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Cập nhật thành công");
        }


        [HttpDelete]
        [Route("DeleteImage")]
        public JsonResult DeleteImage(HinhMinhHoa hinh)
        {
            string query = @"
        DELETE FROM dbo.HinhMinhHoa
        WHERE s_Id = @s_Id AND hmh_Id = @hmh_Id";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@s_Id", hinh.SId);
                    myCommand.Parameters.AddWithValue("@hmh_Id", hinh.HmhId);

                    myCommand.ExecuteNonQuery();
                }
                myCon.Close();
            }

            return new JsonResult("Xóa hình ảnh thành công");
        }


    }
}
