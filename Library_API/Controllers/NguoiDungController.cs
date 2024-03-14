using Microsoft.AspNetCore.Mvc;
using Library_API.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Library_API.Controllers
{
    //[Authorize(Roles ="admin, user")]
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

    
    }
}
