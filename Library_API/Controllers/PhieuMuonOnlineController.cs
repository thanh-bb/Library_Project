using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;


namespace Library_API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class PhieuMuonOnlineController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public PhieuMuonOnlineController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet("Count/{userId}")]
        public JsonResult GetNumberOfBorrowReceiptsForUserInMonth(int userId)
        {
            // Get the current month and year
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            string query = @"
        SELECT COUNT(*) AS NumberOfBorrowReceipts
        FROM dbo.PhieuMuonOnline
        WHERE MONTH(pmo_NgayDat) = @Month 
        AND YEAR(pmo_NgayDat) = @Year
        AND nd_Id = @UserId
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Month", currentMonth);
                    myCommand.Parameters.AddWithValue("@Year", currentYear);
                    myCommand.Parameters.AddWithValue("@UserId", userId);
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            // Retrieve the count from the DataTable
            int numberOfBorrowReceipts = Convert.ToInt32(table.Rows[0]["NumberOfBorrowReceipts"]);

            return new JsonResult(numberOfBorrowReceipts);
        }


        [HttpGet("CheckMuonOnl/{nd_Id}/{s_Id}")]
        public JsonResult CheckMuonOnl(int nd_Id, int s_Id)
        {
            string query = @"
                SELECT pmo.pmo_TrangThai
                FROM dbo.PhieuMuonOnline pmo
                INNER JOIN dbo.ChiTietPhieuMuonOnline ctpmo ON pmo.pmo_Id = ctpmo.pmo_Id
                WHERE pmo.nd_Id = @nd_Id AND ctpmo.s_Id = @s_Id ";

            string result = "không có kết quả trả về"; // Mặc định là "No" nếu không có kết quả trả về

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Id", nd_Id);
                    myCommand.Parameters.AddWithValue("@s_Id", s_Id);
                    myCon.Open();
                    object queryResult = myCommand.ExecuteScalar();
                    if (queryResult != null)
                    {
                        result = queryResult.ToString(); 
                    }
                }
            }

            return new JsonResult(result);
        }


    }


}
