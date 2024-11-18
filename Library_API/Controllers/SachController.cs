using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SachController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public SachController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select * from
                            dbo.Sach
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
                    dbo.Sach
                    where s_Id = @Id
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id); // Thêm tham số cho ID sách
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [Route("CreateBook")]
        [HttpPost]
        public JsonResult CreateBook(Sach s)
        {
            string query = @"
                    INSERT INTO dbo.Sach
                    (s_TenSach, s_SoLuong, s_MoTa, s_TrongLuong, s_NamXuatBan, s_ChiDoc,
                     tg_Id, nxb_Id,tl_Id, ls_Id, ks_Id, os_Id)
                    VALUES (@s_TenSach, @s_SoLuong, @s_MoTa, @s_TrongLuong, @s_NamXuatBan, @s_ChiDoc,
                     @tg_Id, @nxb_Id,@tl_Id, @ls_Id, @ks_Id, @os_Id )      

                    SELECT SCOPE_IDENTITY();
                    ";

            int newId;
           // DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
          //  SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@s_TenSach", s.STenSach);
                   myCommand.Parameters.AddWithValue("@s_SoLuong", s.SSoLuong);
                    myCommand.Parameters.AddWithValue("@s_MoTa", s.SMoTa);
                    myCommand.Parameters.AddWithValue("@s_TrongLuong", s.STrongLuong);
                    myCommand.Parameters.AddWithValue("@s_NamXuatBan", s.SNamXuatBan);
             //       myCommand.Parameters.AddWithValue("@s_TrangThaiMuon", s.STrangThaiMuon);
                    myCommand.Parameters.AddWithValue("@s_ChiDoc", s.SChiDoc);
                    myCommand.Parameters.AddWithValue("@tg_Id", s.TgId);
                    myCommand.Parameters.AddWithValue("@nxb_Id", s.NxbId);
                    myCommand.Parameters.AddWithValue("@tl_Id", s.TlId);
                    myCommand.Parameters.AddWithValue("@ls_Id", s.LsId);
                    myCommand.Parameters.AddWithValue("@ks_Id", s.KsId);
                    myCommand.Parameters.AddWithValue("@os_Id", s.OsId);
                    // myCommand.Parameters.AddWithValue("@s_HinhAnh", s.SHinhAnh);
                    newId = Convert.ToInt32(myCommand.ExecuteScalar());

                    //myReader = myCommand.ExecuteReader();
                    //table.Load(myReader);
                    //myReader.Close();
                    
                }
                myCon.Close();
            }

            return new JsonResult(newId);
        }


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

        //[HttpPut]
        //public JsonResult Put(Sach s)
        //{
        //    string query = @"
        //    UPDATE dbo.Sach
        //    SET s_TenSach = @s_TenSach,
        //        s_SoLuong= @s_SoLuong,
        //        s_MoTa=@s_MoTa,
        //        s_TrongLuong=@s_TrongLuong,
        //        s_NamXuatBan= @s_NamXuatBan,
        //        s_ChiDoc=@s_ChiDoc,
        //        tg_Id=@tg_Id, 
        //        nxb_Id=@nxb_Id, 
        //        tl_Id=@tl_Id,
        //        ls_Id=@ls_Id, 
        //        ks_Id =@ks_Id,
        //        os_Id =@os_Id
        //        WHERE s_Id = @s_Id
        //    ";

        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("MyConnection");
        //    SqlDataReader myReader;
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@s_Id", s.SId);
        //            myCommand.Parameters.AddWithValue("@s_TenSach", s.STenSach);
        //            myCommand.Parameters.AddWithValue("@s_SoLuong", s.SSoLuong);
        //            myCommand.Parameters.AddWithValue("@s_MoTa", s.SMoTa);
        //            myCommand.Parameters.AddWithValue("@s_TrongLuong", s.STrongLuong);
        //            myCommand.Parameters.AddWithValue("@s_NamXuatBan", s.SNamXuatBan);
        //        //    myCommand.Parameters.AddWithValue("@s_TrangThaiMuon", s.STrangThaiMuon);
        //            myCommand.Parameters.AddWithValue("@s_ChiDoc", s.SChiDoc);
        //            myCommand.Parameters.AddWithValue("@tg_Id", s.TgId);
        //            myCommand.Parameters.AddWithValue("@nxb_Id", s.NxbId);
        //            myCommand.Parameters.AddWithValue("@tl_Id", s.TlId);
        //            myCommand.Parameters.AddWithValue("@ls_Id", s.LsId);
        //            myCommand.Parameters.AddWithValue("@ks_Id", s.KsId);
        //            myCommand.Parameters.AddWithValue("@os_Id", s.OsId);
        //            // myCommand.Parameters.AddWithValue("@s_HinhAnh", s.SHinhAnh);

        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);
        //            myReader.Close();
        //            myCon.Close();
        //        }
        //    }

        //    return new JsonResult("Cập nhật thành công");
        //}


        [HttpPut("NhapKho/{id}")]
        public JsonResult NhapKho(int id, int soLuongNhap)
        {
            string query = @"
        UPDATE dbo.Sach
        SET s_SoLuong = s_SoLuong + @SoLuongNhap
        WHERE s_Id = @Id
        ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Id", id);
                    myCommand.Parameters.AddWithValue("@SoLuongNhap", soLuongNhap);

                    myCommand.ExecuteNonQuery();
                }
            }

            return new JsonResult("Nhập kho thành công");
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string querySach = @"
                 DELETE FROM dbo.Sach
                 WHERE s_Id = @s_Id";

            string queryHinhMinhHoa = @"
                 DELETE FROM dbo.HinhMinhHoa
                 WHERE s_Id = @s_Id";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlTransaction transaction = myCon.BeginTransaction())
                {
                    try
                    {
                        // Xóa Hình Minh Họa trước
                        using (SqlCommand myCommand = new SqlCommand(queryHinhMinhHoa, myCon, transaction))
                        {
                            myCommand.Parameters.AddWithValue("@s_Id", id);
                            myCommand.ExecuteNonQuery();
                        }

                        // Xóa Sách sau khi xóa Hình Minh Họa thành công
                        using (SqlCommand myCommand = new SqlCommand(querySach, myCon, transaction))
                        {
                            myCommand.Parameters.AddWithValue("@s_Id", id);
                            int affectedRows = myCommand.ExecuteNonQuery();

                            // Kiểm tra xem có bản ghi nào được xóa không
                            if (affectedRows == 0)
                            {
                                throw new Exception("Không tìm thấy sách với ID này hoặc không thể xóa.");
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return new JsonResult($"Xóa thất bại: {ex.Message}");
                    }
                    finally
                    {
                        myCon.Close();
                    }
                }
            }

            return new JsonResult("Xóa thành công");
        }

        [HttpPut("Put_TrangThai")]  
        public JsonResult Put_TrangThai(Sach s)
        {
            string query = @"
                            update dbo.Sach
                            set s_TrangThaiMuon = @s_TrangThaiMuon
                            where s_Id=@s_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@s_Id", s.SId);
                    myCommand.Parameters.AddWithValue("@s_TrangThaiMuon", s.STrangThaiMuon);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Cập nhật thành công");
        }


        [HttpGet("GetSach")]
        public JsonResult GetSach(string tenSach = "")
        {
            string query = @"
                    SELECT * 
                    FROM dbo.Sach
                    WHERE s_TrangThaiMuon = 1 
                      AND s_ChiDoc = 0 
                      AND s_TenSach LIKE '%' + @tenSach + '%'";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    // Thêm tham số `@tenSach` để lọc theo từ khóa tìm kiếm
                    myCommand.Parameters.AddWithValue("@tenSach", tenSach);

                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }

            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Sach s)
        {
            string query = @"
        UPDATE dbo.Sach
        SET s_TenSach = @s_TenSach,
            s_SoLuong = @s_SoLuong,
            s_MoTa = @s_MoTa,
            s_TrongLuong = @s_TrongLuong,
            s_NamXuatBan = @s_NamXuatBan,
            s_ChiDoc = @s_ChiDoc,
            tg_Id = @tg_Id, 
            nxb_Id = @nxb_Id, 
            tl_Id = @tl_Id,
            ls_Id = @ls_Id, 
            ks_Id = @ks_Id,
            os_Id = @os_Id
        WHERE s_Id = @s_Id";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();

                // Cập nhật thông tin sách
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@s_Id", s.SId);
                    myCommand.Parameters.AddWithValue("@s_TenSach", s.STenSach);
                    myCommand.Parameters.AddWithValue("@s_SoLuong", s.SSoLuong);
                    myCommand.Parameters.AddWithValue("@s_MoTa", s.SMoTa);
                    myCommand.Parameters.AddWithValue("@s_TrongLuong", s.STrongLuong);
                    myCommand.Parameters.AddWithValue("@s_NamXuatBan", s.SNamXuatBan);
                    myCommand.Parameters.AddWithValue("@s_ChiDoc", s.SChiDoc);
                    myCommand.Parameters.AddWithValue("@tg_Id", s.TgId);
                    myCommand.Parameters.AddWithValue("@nxb_Id", s.NxbId);
                    myCommand.Parameters.AddWithValue("@tl_Id", s.TlId);
                    myCommand.Parameters.AddWithValue("@ls_Id", s.LsId);
                    myCommand.Parameters.AddWithValue("@ks_Id", s.KsId);
                    myCommand.Parameters.AddWithValue("@os_Id", s.OsId);

                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }

                // Cập nhật hoặc thêm mới hình ảnh minh họa
                foreach (var image in s.HinhMinhHoas)
                {
                    string imageQuery = @"
                IF EXISTS (SELECT 1 FROM dbo.HinhMinhHoa WHERE hmh_Id = @hmh_Id)
                    UPDATE dbo.HinhMinhHoa
                    SET hmh_HinhAnhMaHoa = @hmh_HinhAnhMaHoa
                    WHERE hmh_Id = @hmh_Id
                ELSE
                    INSERT INTO dbo.HinhMinhHoa (s_Id, hmh_HinhAnhMaHoa)
                    VALUES (@s_Id, @hmh_HinhAnhMaHoa)";

                    using (SqlCommand imageCommand = new SqlCommand(imageQuery, myCon))
                    {
                        imageCommand.Parameters.AddWithValue("@hmh_Id", image.HmhId == 0 ? (object)DBNull.Value : image.HmhId);
                        imageCommand.Parameters.AddWithValue("@s_Id", s.SId);
                        imageCommand.Parameters.AddWithValue("@hmh_HinhAnhMaHoa", image.HmhHinhAnhMaHoa);

                        imageCommand.ExecuteNonQuery();
                    }
                }

                myCon.Close();
            }

            return new JsonResult("Cập nhật thành công");
        }

    }
}
