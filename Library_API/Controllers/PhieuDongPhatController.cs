using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace Library_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PhieuDongPhatController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		public PhieuDongPhatController(IConfiguration configuration)
		{
			_configuration = configuration;
		}


		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
                            select * from
                            dbo.PhieuDongPhat
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
                            dbo.PhieuDongPhat
							where pdp_Id = @Id
                            ";

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


        [HttpGet("FindByNdId/{ndId}")]
        public JsonResult FindByNdId(int ndId)
        {
            string query = @"
                    SELECT pdp.* 
                    FROM dbo.PhieuDongPhat pdp
                    INNER JOIN dbo.PhieuMuon pm ON pdp.pm_Id = pm.pm_Id
                    WHERE pm.nd_Id = @NdId
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NdId", ndId);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }



        //      [HttpPost]
        //public JsonResult Post(int pm_Id)
        //{
        //	// Lấy ngày hôm nay
        //	DateTime ngayHomNay = DateTime.Today;

        //	// Lấy ngày hạn trả của phiếu mượn (pm_HanTra)
        //	DateTime ngayHanTra;
        //          DateTime ngayTra;
        //          string queryLayNgayHanTra = "SELECT pm_HanTra FROM dbo.PhieuMuon WHERE pm_Id = @pm_Id";

        //	string sqlDataSource = _configuration.GetConnectionString("MyConnection");

        //          string queryLayNgayTra = @"
        //      SELECT pm_HanTra, pt_NgayTra 
        //      FROM dbo.PhieuMuon pm
        //      LEFT JOIN dbo.PhieuTra pt ON pm.pm_Id = pt.pm_Id
        //      WHERE pm.pm_Id = @pm_Id";

        //          using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //	{
        //		myCon.Open();
        //		using (SqlCommand cmd = new SqlCommand(queryLayNgayHanTra, myCon))
        //		{
        //			cmd.Parameters.AddWithValue("@pm_Id", pm_Id);
        //			ngayHanTra = (DateTime)cmd.ExecuteScalar();
        //		}
        //		myCon.Close();
        //	}

        //	// Tính số ngày trễ hạn
        //	int soNgayTreHan = (int)(ngayHomNay - ngayHanTra).TotalDays;

        //	// Nếu số ngày trễ hạn âm (không trễ hạn), đặt tổng tiền phạt bằng 0
        //	if (soNgayTreHan < 0)
        //	{
        //		return new JsonResult("Không trễ hạn, không phạt tiền");
        //	}

        //	// Tính tổng tiền phạt
        //	double tongTienPhat = soNgayTreHan * 2000;

        //	// Tạo một đối tượng PhieuDongPhat với pm_Id và tổng tiền phạt được tính toán
        //	PhieuDongPhat pdp = new PhieuDongPhat
        //	{
        //		PmId = pm_Id,
        //		PdpTongTienPhat = tongTienPhat,
        //		PdpNgayDong = ngayHomNay,
        //		PdpTrangThaiDong = false // Chưa thanh toán
        //	};

        //	// Thêm PhieuDongPhat vào cơ sở dữ liệu
        //	string query = @"
        //	 INSERT INTO dbo.PhieuDongPhat (pdp_TongTienPhat, pdp_NgayDong, pdp_TrangThaiDong, pm_Id)
        //	 VALUES (@pdp_TongTienPhat, @pdp_NgayDong, @pdp_TrangThaiDong, @pm_Id)
        //	 ";

        //	using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //	{
        //		myCon.Open();
        //		using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //		{
        //			myCommand.Parameters.AddWithValue("@pdp_TongTienPhat", pdp.PdpTongTienPhat);
        //			myCommand.Parameters.AddWithValue("@pdp_NgayDong", pdp.PdpNgayDong);
        //			myCommand.Parameters.AddWithValue("@pdp_TrangThaiDong", pdp.PdpTrangThaiDong);
        //			myCommand.Parameters.AddWithValue("@pm_Id", pdp.PmId);

        //			myCommand.ExecuteNonQuery();
        //		}
        //		myCon.Close();
        //	}

        //	// update PM
        //          string updateQuery = "UPDATE dbo.PhieuMuon SET pm_DaXuatPhat = 1 WHERE pm_Id = @pm_Id";

        //          using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //          {
        //              myCon.Open();
        //              using (SqlCommand cmd = new SqlCommand(updateQuery, myCon))
        //              {
        //                  cmd.Parameters.AddWithValue("@pm_Id", pm_Id);
        //                  cmd.ExecuteNonQuery();
        //              }
        //              myCon.Close();
        //          }


        //          string queryy = @"
        //      SELECT pdp_Id, pdp_TongTienPhat, pdp_NgayDong, pdp_TrangThaiDong, pm_Id
        //      FROM dbo.PhieuDongPhat
        //      WHERE pm_Id = @pm_Id
        //      ORDER BY pdp_Id DESC
        //      ";

        //	PhieuDongPhat pdpDetail = null;

        //	using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //	{
        //		myCon.Open();
        //		using (SqlCommand cmd = new SqlCommand(queryy, myCon))
        //		{
        //			cmd.Parameters.AddWithValue("@pm_Id", pm_Id);

        //			SqlDataReader reader = cmd.ExecuteReader();

        //			// Lặp qua kết quả trả về từ truy vấn SQL và lấy chi tiết của phiếu đóng phạt
        //			if (reader.Read())
        //			{
        //				pdpDetail = new PhieuDongPhat
        //				{
        //					PdpId = Convert.ToInt32(reader["pdp_Id"]),
        //					PdpTongTienPhat = Convert.ToDouble(reader["pdp_TongTienPhat"]),
        //					PdpNgayDong = Convert.ToDateTime(reader["pdp_NgayDong"]),
        //					PdpTrangThaiDong = Convert.ToBoolean(reader["pdp_TrangThaiDong"]),
        //					PmId = Convert.ToInt32(reader["pm_Id"])
        //				};
        //			}

        //			reader.Close();
        //		}
        //		myCon.Close();
        //	}

        //	// Trả về chi tiết của phiếu đóng phạt vừa được thêm vào cơ sở dữ liệu
        //	if (pdpDetail != null)
        //	{
        //		return new JsonResult(pdpDetail);
        //	}
        //	else
        //	{
        //		return new JsonResult("Không tìm thấy chi tiết phiếu đóng phạt sau khi thêm vào cơ sở dữ liệu.");
        //	}
        //}

        [HttpPost]
        public JsonResult Post(int pm_Id)
        {
            // Fetch current date (reset time to midnight)
            DateTime ngayHomNay = DateTime.Today;

            // Variables to store the due date (Hạn trả) and return date (Ngày trả)
            DateTime ngayHanTra = DateTime.MinValue;
            DateTime ngayTra = DateTime.MinValue;

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            // Query to fetch due date (Hạn trả) and return date (Ngày trả)
            string queryLayNgayTra = @"
        SELECT pm_HanTra, pt_NgayTra 
        FROM dbo.PhieuMuon pm
        LEFT JOIN dbo.PhieuTra pt ON pm.pm_Id = pt.pm_Id
        WHERE pm.pm_Id = @pm_Id";

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand cmd = new SqlCommand(queryLayNgayTra, myCon))
                {
                    cmd.Parameters.AddWithValue("@pm_Id", pm_Id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Fetch Hạn trả
                            if (!reader.IsDBNull(0))
                                ngayHanTra = reader.GetDateTime(0);

                            // Fetch Ngày trả
                            if (!reader.IsDBNull(1))
                                ngayTra = reader.GetDateTime(1);
                        }
                        else
                        {
                            return new JsonResult(new { message = "Không tìm thấy thông tin phiếu mượn.", success = false });
                        }
                    }
                }
                myCon.Close();
            }

            // Use today's date if return date is not available
            if (ngayTra == DateTime.MinValue)
            {
                ngayTra = ngayHomNay;
            }

            // Ensure valid due date
            if (ngayHanTra == DateTime.MinValue)
            {
                return new JsonResult(new { message = "Hạn trả không hợp lệ.", success = false });
            }

            // Calculate overdue days (ignoring time)
            int soNgayTreHan = (ngayTra.Date - ngayHanTra.Date).Days;

            // Check overdue days
            if (soNgayTreHan > 0)
            {
                // Calculate fine amount (e.g., 2000 VND per overdue day)
                double tongTienPhat = soNgayTreHan * 2000;

                // Create a new PhieuDongPhat object
                PhieuDongPhat pdp = new PhieuDongPhat
                {
                    PmId = pm_Id,
                    PdpTongTienPhat = tongTienPhat,
                    PdpNgayDong = ngayHomNay,
                    PdpTrangThaiDong = false // Not paid yet
                };

                // Insert PhieuDongPhat into the database
                string insertQuery = @"
            INSERT INTO dbo.PhieuDongPhat (pdp_TongTienPhat, pdp_NgayDong, pdp_TrangThaiDong, pm_Id)
            VALUES (@pdp_TongTienPhat, @pdp_NgayDong, @pdp_TrangThaiDong, @pm_Id)";

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(insertQuery, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@pdp_TongTienPhat", pdp.PdpTongTienPhat);
                        myCommand.Parameters.AddWithValue("@pdp_NgayDong", pdp.PdpNgayDong);
                        myCommand.Parameters.AddWithValue("@pdp_TrangThaiDong", pdp.PdpTrangThaiDong);
                        myCommand.Parameters.AddWithValue("@pm_Id", pdp.PmId);

                        myCommand.ExecuteNonQuery();
                    }
                    myCon.Close();
                }

                // Update PhieuMuon table to mark as fined
                string updateQuery = "UPDATE dbo.PhieuMuon SET pm_DaXuatPhat = 1 WHERE pm_Id = @pm_Id";

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(updateQuery, myCon))
                    {
                        cmd.Parameters.AddWithValue("@pm_Id", pm_Id);
                        cmd.ExecuteNonQuery();
                    }
                    myCon.Close();
                }

                // Fetch the fine record to return as response
                string fetchQuery = @"
            SELECT pdp_Id, pdp_TongTienPhat, pdp_NgayDong, pdp_TrangThaiDong, pm_Id
            FROM dbo.PhieuDongPhat
            WHERE pm_Id = @pm_Id
            ORDER BY pdp_Id DESC";

                PhieuDongPhat pdpDetail = null;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand cmd = new SqlCommand(fetchQuery, myCon))
                    {
                        cmd.Parameters.AddWithValue("@pm_Id", pm_Id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                pdpDetail = new PhieuDongPhat
                                {
                                    PdpId = Convert.ToInt32(reader["pdp_Id"]),
                                    PdpTongTienPhat = Convert.ToDouble(reader["pdp_TongTienPhat"]),
                                    PdpNgayDong = Convert.ToDateTime(reader["pdp_NgayDong"]),
                                    PdpTrangThaiDong = Convert.ToBoolean(reader["pdp_TrangThaiDong"]),
                                    PmId = Convert.ToInt32(reader["pm_Id"])
                                };
                            }
                        }
                    }
                    myCon.Close();
                }

                if (pdpDetail != null)
                {
                    return new JsonResult(new { data = pdpDetail, success = true });
                }
            }

            return new JsonResult(new { message = "Không trễ hạn, không phạt tiền.", success = false });
        }




        [HttpPost("get-report")]
        public async Task<IActionResult> GetReportFromUrl([FromBody] string url)
        {
            var options = new LaunchOptions
            {
                Headless = true,
            };

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            using var browser = await Puppeteer.LaunchAsync(options);
            using var page = await browser.NewPageAsync();
            using var memoryStream = new MemoryStream();

            await page.GoToAsync(url);

            var pdfStream = await page.PdfDataAsync();

            return File(pdfStream, "application/pdf", "Report.pdf");
        }




        [HttpPut("UpdateTrangThaiThanhToan")]
        public JsonResult UpdateTrangThaiThanhToan(PhieuDongPhat pdp)
        {
            string query = @"
                            update dbo.PhieuDongPhat
                            set pdp_TrangThaiDong = @pdp_TrangThaiDong 
                               WHERE pdp_Id = @pdp_Id;
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@pdp_Id", pdp.PdpId);
                    myCommand.Parameters.AddWithValue("@pdp_TrangThaiDong", pdp.PdpTrangThaiDong);

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
