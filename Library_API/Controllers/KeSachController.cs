using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KeSachController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public KeSachController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select ks_Id, ks_TenKe from
                            dbo.KeSach
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
    }
}
