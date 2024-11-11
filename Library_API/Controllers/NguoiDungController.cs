using Microsoft.AspNetCore.Mvc;
using Library_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library_API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public NguoiDungController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select * from
                            dbo.NguoiDung
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
                    dbo.NguoiDung
                    where nd_Id = @Id
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


        [HttpGet("FindByPmId/{pmId}")]
        public JsonResult FindByPmId(int pmId)
        {
            string query = @"
        SELECT * FROM dbo.NguoiDung
        WHERE nd_Id IN (
            SELECT nd_Id FROM dbo.PhieuMuon
            WHERE pm_Id = @PmId
        )
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PmId", pmId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpPut]
        public JsonResult Put(NguoiDung nd)
        {
            string query = @"
                            update dbo.NguoiDung
                            set nd_Active = @nd_Active
                            where nd_Id=@nd_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Id", nd.NdId);
                    myCommand.Parameters.AddWithValue("@nd_Active", nd.NdActive);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(NguoiDung nd)
        {
            // Updated query to include SCOPE_IDENTITY()
            string query = @"
        INSERT INTO dbo.NguoiDung
        (nd_Username, nd_CCCD, nd_SoDienThoai, nd_HinhThe, nd_Password, 
        nd_HoTen, nd_NgaySinh, nd_GioiTinh, nd_Email, nd_DiaChi, nd_NgayDangKy,
        nd_ThoiGianSuDung, nd_active, q_Id, lnd_LoaiNguoiDung)
        VALUES
        (@nd_Username, @nd_CCCD, @nd_SoDienThoai, @nd_HinhThe, @nd_Password, 
        @nd_HoTen, @nd_NgaySinh, @nd_GioiTinh, @nd_Email, @nd_DiaChi, @nd_NgayDangKy,
        @nd_ThoiGianSuDung, @nd_active, @q_Id, @lnd_LoaiNguoiDung);
        
        -- Retrieve the ID of the newly inserted record
        SELECT SCOPE_IDENTITY();
    ";

            int newId;
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Username", nd.NdUsername);
                    myCommand.Parameters.AddWithValue("@nd_CCCD", nd.NdCccd);
                    myCommand.Parameters.AddWithValue("@nd_SoDienThoai", nd.NdSoDienThoai);
                    myCommand.Parameters.AddWithValue("@nd_HinhThe", nd.NdHinhThe);
                    myCommand.Parameters.AddWithValue("@nd_Password", nd.NdPassword);
                    myCommand.Parameters.AddWithValue("@nd_HoTen", nd.NdHoTen);
                    myCommand.Parameters.AddWithValue("@nd_NgaySinh", nd.NdNgaySinh);
                    myCommand.Parameters.AddWithValue("@nd_GioiTinh", nd.NdGioiTinh);
                    myCommand.Parameters.AddWithValue("@nd_Email", nd.NdEmail);
                    myCommand.Parameters.AddWithValue("@nd_DiaChi", nd.NdDiaChi);
                    myCommand.Parameters.AddWithValue("@nd_NgayDangKy", nd.NdNgayDangKy);
                    myCommand.Parameters.AddWithValue("@nd_ThoiGianSuDung", nd.NdThoiGianSuDung);
                    myCommand.Parameters.AddWithValue("@nd_active", nd.NdActive);
                    myCommand.Parameters.AddWithValue("@q_Id", nd.QId);
                    myCommand.Parameters.AddWithValue("@lnd_LoaiNguoiDung", nd.LndLoaiNguoiDung);

                    // Execute the query and retrieve the ID of the new record
                    newId = Convert.ToInt32(myCommand.ExecuteScalar());
                }
                myCon.Close();
            }

            return new JsonResult(newId);
        }

        // Kiem tra username

        [HttpGet("CheckUsernameAvailability")]
        public async Task<IActionResult> CheckUsernameAvailability(string username)
        {
            string checkUsernameQuery = @"
                                    SELECT COUNT(1) FROM dbo.NguoiDung
                                    WHERE nd_Username = @nd_Username;
                                    ";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand checkCmd = new SqlCommand(checkUsernameQuery, myCon))
                {
                    checkCmd.Parameters.AddWithValue("@nd_Username", username);
                    int usernameCount = (int)checkCmd.ExecuteScalar();

                    if (usernameCount > 0)
                    {
                        // Username already exists, return a conflict response
                        return Ok(false);
                    }

                }

                myCon.Close();
            }
            return Ok(true); // Username khả dụng

        }


        [HttpGet("search-borrower")]
        public JsonResult SearchBorrower(string? cccd, string? username)
        {
            if (string.IsNullOrEmpty(cccd) && string.IsNullOrEmpty(username))
            {
                return new JsonResult(new { error = "Vui lòng cung cấp username hoặc CCCD để tìm kiếm." })
                {
                    StatusCode = 400
                };
            }

            string query = @"
        SELECT nd_Id, nd_Username, nd_HoTen
        FROM dbo.NguoiDung
        WHERE (@username IS NULL OR nd_Username = @username)
          AND (@cccd IS NULL OR nd_CCCD = @cccd)";

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("MyConnection")))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@cccd", (object?)cccd ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@username", (object?)username ?? DBNull.Value);

                DataTable resultTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(resultTable);

                if (resultTable.Rows.Count > 0)
                {
                    return new JsonResult(resultTable);
                }
                else
                {
                    return new JsonResult("Không tìm thấy người mượn");
                }
            }
        }


    }
}
