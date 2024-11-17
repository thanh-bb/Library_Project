using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Library_API.Dtos; // Thêm namespace của DTOs

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyPhieuMuonController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public QuanLyPhieuMuonController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
            SELECT pm.pm_Id, pm.nd_Id, pm.ttm_Id, pm.pm_TrangThaiXetDuyet, pm.pm_NgayMuon, pm.pm_HanTra,
                   ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon,
                   s.s_TenSach,
                   ttm.ttm_TenTrangThai,
                   pm.pm_LoaiMuon  -- New column for loan type
            FROM dbo.PhieuMuon pm
            INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
            INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
            INNER JOIN dbo.TrangThaiMuon ttm ON pm.ttm_Id = ttm.ttm_Id
            ";


            List<QuanLyPhieuMuon> quanLyPhieuMuons = new List<QuanLyPhieuMuon>();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();
                    while (myReader.Read())
                    {
                        QuanLyPhieuMuon phieuMuon = new QuanLyPhieuMuon
                        {
                            Id_PhieuMuon = Convert.ToInt32(myReader["pm_Id"]),
                            Id_User = Convert.ToInt32(myReader["nd_Id"]),
                            TenSach = myReader["s_TenSach"].ToString(),
                            SoLuongSach = Convert.ToInt32(myReader["ctpm_SoLuongSachMuon"]),
                            NgayMuon = Convert.ToDateTime(myReader["pm_NgayMuon"]),
                            HanTra = Convert.ToDateTime(myReader["pm_HanTra"]),
                            TrangThaiMuon = myReader["ttm_TenTrangThai"].ToString(),
                            TrangThaiXetDuyet = myReader["pm_TrangThaiXetDuyet"].ToString(),
                            PmLoaiMuon = myReader["pm_LoaiMuon"]?.ToString()  // Assign loan type
                        };
                        quanLyPhieuMuons.Add(phieuMuon);
                    }


                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuMuons);
        }


        [HttpGet("ListPM")]
        public JsonResult ListPM()
        {
            string query = @"
        SELECT 
            pm.pm_Id, pm.nd_Id, pm.ttm_Id, pm.pm_TrangThaiXetDuyet, 
            pm.pm_NgayMuon, pm.pm_HanTra, pm.pm_LoaiMuon, pm.pm_DaXuatPhat,
            ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon, 
            s.s_TenSach, 
            ttm.ttm_TenTrangThai
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
        INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
        INNER JOIN dbo.TrangThaiMuon ttm ON pm.ttm_Id = ttm.ttm_Id
    ";

            List<QuanLyPhieuMuon> result = new List<QuanLyPhieuMuon>();
            Dictionary<int, QuanLyPhieuMuon> groupedData = new Dictionary<int, QuanLyPhieuMuon>();

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        int pmId = Convert.ToInt32(myReader["pm_Id"]);

                        // Kiểm tra nếu phiếu mượn đã được thêm vào dictionary
                        if (!groupedData.ContainsKey(pmId))
                        {
                            groupedData[pmId] = new QuanLyPhieuMuon
                            {
                                Id_PhieuMuon = pmId,
                                Id_User = Convert.ToInt32(myReader["nd_Id"]),
                                NgayMuon = Convert.ToDateTime(myReader["pm_NgayMuon"]),
                                HanTra = Convert.ToDateTime(myReader["pm_HanTra"]),
                                TrangThaiMuon = myReader["ttm_TenTrangThai"].ToString(),
                                TrangThaiXetDuyet = myReader["pm_TrangThaiXetDuyet"].ToString(),
                                PmLoaiMuon = myReader["pm_LoaiMuon"]?.ToString(),
                                PmDaXuatPhat = Convert.ToBoolean(myReader["pm_DaXuatPhat"]),
                                ChiTiet = new List<ChiTietSach>() // Khởi tạo danh sách chi tiết sách

                            };
                        }

                        // Thêm chi tiết sách vào phiếu mượn tương ứng
                        groupedData[pmId].ChiTiet.Add(new ChiTietSach
                        {
                            Id_Sach = Convert.ToInt32(myReader["s_Id"]),
                            TenSach = myReader["s_TenSach"].ToString(),
                            SoLuongSach = Convert.ToInt32(myReader["ctpm_SoLuongSachMuon"])
                        });
                    }

                    myReader.Close();
                }
            }

            // Chuyển từ dictionary sang danh sách
            result = groupedData.Values.ToList();

            return new JsonResult(result);
        }




        [HttpGet("{ndId}")]
        public JsonResult Get(int ndId)
        {
            string query = @"
    SELECT 
        pm.pm_Id, pm.nd_Id, pm.ttm_Id, pm.pm_TrangThaiXetDuyet, 
        pm.pm_NgayMuon, pm.pm_HanTra, pm.pm_LoaiMuon, 
        ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon, 
        s.s_TenSach, 
        ttm.ttm_TenTrangThai
    FROM dbo.PhieuMuon pm
    INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
    INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
    INNER JOIN dbo.TrangThaiMuon ttm ON pm.ttm_Id = ttm.ttm_Id
    WHERE pm.nd_Id = @NdId
    ";

            List<QuanLyPhieuMuon> result = new List<QuanLyPhieuMuon>();
            Dictionary<int, QuanLyPhieuMuon> groupedData = new Dictionary<int, QuanLyPhieuMuon>();

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NdId", ndId);
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        int pmId = Convert.ToInt32(myReader["pm_Id"]);

                        // Kiểm tra nếu phiếu mượn đã được thêm vào dictionary
                        if (!groupedData.ContainsKey(pmId))
                        {
                            groupedData[pmId] = new QuanLyPhieuMuon
                            {
                                Id_PhieuMuon = pmId,
                                Id_User = Convert.ToInt32(myReader["nd_Id"]),
                                NgayMuon = Convert.ToDateTime(myReader["pm_NgayMuon"]),
                                HanTra = Convert.ToDateTime(myReader["pm_HanTra"]),
                                TrangThaiMuon = myReader["ttm_TenTrangThai"].ToString(),
                                TrangThaiXetDuyet = myReader["pm_TrangThaiXetDuyet"].ToString(),
                                PmLoaiMuon = myReader["pm_LoaiMuon"]?.ToString(),
                                ChiTiet = new List<ChiTietSach>() // Khởi tạo danh sách chi tiết sách
                            };
                        }

                        // Thêm chi tiết sách vào phiếu mượn tương ứng
                        groupedData[pmId].ChiTiet.Add(new ChiTietSach
                        {
                            Id_Sach = Convert.ToInt32(myReader["s_Id"]),
                            TenSach = myReader["s_TenSach"].ToString(),
                            SoLuongSach = Convert.ToInt32(myReader["ctpm_SoLuongSachMuon"])
                        });
                    }

                    myReader.Close();
                }
            }

            // Chuyển từ dictionary sang danh sách
            result = groupedData.Values.ToList();

            return new JsonResult(result);
        }




        [HttpGet("ByPmId/{pmId}")]
        public JsonResult GetByPmId(int pmId)
        {
            string query = @"
       SELECT pm.pm_Id, pm.nd_Id, pm.ttm_Id, pm.pm_TrangThaiXetDuyet, pm.pm_NgayMuon, pm.pm_HanTra,
               ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon,
               s.s_TenSach,
               ttm.ttm_TenTrangThai
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
        INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
        INNER JOIN dbo.TrangThaiMuon ttm ON pm.ttm_Id = ttm.ttm_Id
        WHERE pm.pm_Id = @PmId
    ";

            List<QuanLyPhieuMuon> quanLyPhieuMuons = new List<QuanLyPhieuMuon>();

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PmId", pmId);
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        QuanLyPhieuMuon phieuMuon = new QuanLyPhieuMuon
                        {
                            Id_PhieuMuon = Convert.ToInt32(myReader["pm_Id"]),
                            Id_User = Convert.ToInt32(myReader["nd_Id"]),
                            Id_Sach = Convert.ToInt32(myReader["s_Id"]),
                            TenSach = myReader["s_TenSach"].ToString(),
                            SoLuongSach = Convert.ToInt32(myReader["ctpm_SoLuongSachMuon"]),
                            NgayMuon = Convert.ToDateTime(myReader["pm_NgayMuon"]),
                            HanTra = Convert.ToDateTime(myReader["pm_HanTra"]),
                            TrangThaiMuon = myReader["ttm_TenTrangThai"].ToString(), // Lấy tên trạng thái từ bảng TrangThaiMuon
                            TrangThaiXetDuyet = myReader["pm_TrangThaiXetDuyet"].ToString()
                        };
                        quanLyPhieuMuons.Add(phieuMuon);
                    }

                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuMuons);
        }


        [HttpGet("CheckMuon/{nd_Id}/{s_Id}")]
        public JsonResult CheckMuon(int nd_Id, int s_Id)
        {
            string query = @"
    SELECT pm.ttm_Id
    FROM dbo.PhieuMuon pm
    INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
    WHERE pm.nd_Id = @nd_Id 
    AND ctpm.s_Id = @s_Id 
    AND (pm.ttm_Id = 1 OR pm.ttm_Id = 3)";

            string result = "Không có kết quả trả về"; // Mặc định nếu không có kết quả trả về

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
                        result = queryResult.ToString(); // Lấy giá trị của cột ttm_Id
                    }
                }
            }

            return new JsonResult(result);
        }


    }
}
