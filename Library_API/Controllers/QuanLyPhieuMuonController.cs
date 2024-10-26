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
        SELECT pm.pm_Id,pm.nd_Id, pm.pm_TrangThaiMuon,pm.pm_TrangThaiXetDuyet, pm.pm_NgayMuon, pm.pm_HanTra,
               ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon,
               s.s_TenSach
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
        INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
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
                            TrangThaiMuon = myReader["pm_TrangThaiMuon"].ToString(),
                            TrangThaiXetDuyet = myReader["pm_TrangThaiXetDuyet"].ToString()
                        };
                        quanLyPhieuMuons.Add(phieuMuon);
                    }

                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuMuons);
        }



        [HttpGet("{ndId}")]
        public JsonResult Get(int ndId)
        {
            string query = @"
        SELECT pm.pm_Id,pm.nd_Id, pm.pm_TrangThaiMuon, pm.pm_TrangThaiXetDuyet, pm.pm_NgayMuon, pm.pm_HanTra,
               ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon,
               s.s_TenSach
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
        INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
        WHERE pm.nd_Id = @NdId
    ";

            List<QuanLyPhieuMuon> quanLyPhieuMuons = new List<QuanLyPhieuMuon>();

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
                        QuanLyPhieuMuon phieuMuon = new QuanLyPhieuMuon
                        {
                            Id_PhieuMuon = Convert.ToInt32(myReader["pm_Id"]),
                            Id_User = Convert.ToInt32(myReader["nd_Id"]),
                            Id_Sach = Convert.ToInt32(myReader["s_Id"]),
                            TenSach = myReader["s_TenSach"].ToString(),
                            SoLuongSach = Convert.ToInt32(myReader["ctpm_SoLuongSachMuon"]),
                            NgayMuon = Convert.ToDateTime(myReader["pm_NgayMuon"]),
                            HanTra = Convert.ToDateTime(myReader["pm_HanTra"]),
                            TrangThaiMuon = myReader["pm_TrangThaiMuon"].ToString(),
                            TrangThaiXetDuyet = myReader["pm_TrangThaiXetDuyet"].ToString()
                        };
                        quanLyPhieuMuons.Add(phieuMuon);
                    }

                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuMuons);
        }



        [HttpGet("ByPmId/{pmId}")]
        public JsonResult GetByPmId(int pmId)
        {
            string query = @"
        SELECT pm.pm_Id,pm.nd_Id, pm.pm_TrangThaiMuon, pm.pm_TrangThaiXetDuyet, pm.pm_NgayMuon, pm.pm_HanTra,
               ctpm.s_Id, ctpm.ctpm_SoLuongSachMuon,
               s.s_TenSach
        FROM dbo.PhieuMuon pm
        INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
        INNER JOIN dbo.Sach s ON ctpm.s_Id = s.s_Id
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
                            TrangThaiMuon = myReader["pm_TrangThaiMuon"].ToString(),
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
    SELECT pm.pm_TrangThaiXetDuyet
    FROM dbo.PhieuMuon pm
    INNER JOIN dbo.ChiTietPhieuMuon ctpm ON pm.pm_Id = ctpm.pm_Id
    WHERE pm.nd_Id = @nd_Id AND ctpm.s_Id = @s_Id ";

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
                        result = queryResult.ToString(); // Lấy giá trị của cột pm_TrangThai
                    }
                }
            }

            return new JsonResult(result);
        }


    }
}
