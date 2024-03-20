using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Library_API.Dtos;
using Microsoft.Extensions.Configuration;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuanLyPhieuTraController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public QuanLyPhieuTraController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
        SELECT pt.pt_Id, pt.pt_NgayTra, pt.nd_Id, ctpt.s_Id,
               ctpt.ctpt_SoLuongSachTra, DATEDIFF(day, pm.pm_HanTra, pt.pt_NgayTra) AS SoNgayTre
        FROM dbo.PhieuTra pt
        INNER JOIN dbo.ChiTietPhieuTra ctpt ON pt.pt_Id = ctpt.pt_Id
        INNER JOIN dbo.PhieuMuon pm ON pt.pm_Id = pm.pm_Id
    ";

            List<QuanLyPhieuTra> quanLyPhieuTras = new List<QuanLyPhieuTra>();

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCon.Open();
                    SqlDataReader myReader = myCommand.ExecuteReader();

                    while (myReader.Read())
                    {
                        QuanLyPhieuTra phieuTra = new QuanLyPhieuTra
                        {
                            PtId = Convert.ToInt32(myReader["pt_Id"]),
                            PtNgayTra = myReader["pt_NgayTra"] != DBNull.Value ? Convert.ToDateTime(myReader["pt_NgayTra"]) : (DateTime?)null,
                            NdId = myReader["nd_Id"] != DBNull.Value ? Convert.ToInt32(myReader["nd_Id"]) : (int?)null,
                            SId = Convert.ToInt32(myReader["s_Id"]),
                            CtptSoLuongSachTra = myReader["ctpt_SoLuongSachTra"] != DBNull.Value ? Convert.ToInt32(myReader["ctpt_SoLuongSachTra"]) : (int?)null,
                            SoNgayTre = Convert.ToInt32(myReader["SoNgayTre"])
                        };
                        quanLyPhieuTras.Add(phieuTra);
                    }

                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuTras);
        }


        [HttpGet("{pmId}")]
        public JsonResult Get(int pmId)
        {
            string query = @"
                SELECT pt.pt_Id, pt.pt_NgayTra, pt.nd_Id, ctpt.s_Id,
                       ctpt.ctpt_SoLuongSachTra, DATEDIFF(day, pm.pm_HanTra, pt.pt_NgayTra) AS SoNgayTre
                FROM dbo.PhieuTra pt
                INNER JOIN dbo.ChiTietPhieuTra ctpt ON pt.pt_Id = ctpt.pt_Id
                INNER JOIN dbo.PhieuMuon pm ON pt.pm_Id = pm.pm_Id
                WHERE pt.pm_Id = @PmId
            ";

            List<QuanLyPhieuTra> quanLyPhieuTras = new List<QuanLyPhieuTra>();

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
                        QuanLyPhieuTra phieuTra = new QuanLyPhieuTra
                        {
                            PtId = Convert.ToInt32(myReader["pt_Id"]),
                            PtNgayTra = myReader["pt_NgayTra"] != DBNull.Value ? Convert.ToDateTime(myReader["pt_NgayTra"]) : (DateTime?)null,
                            NdId = myReader["nd_Id"] != DBNull.Value ? Convert.ToInt32(myReader["nd_Id"]) : (int?)null,
                            SId = Convert.ToInt32(myReader["s_Id"]),
                            CtptSoLuongSachTra = myReader["ctpt_SoLuongSachTra"] != DBNull.Value ? Convert.ToInt32(myReader["ctpt_SoLuongSachTra"]) : (int?)null,
                            SoNgayTre = Convert.ToInt32(myReader["SoNgayTre"])
                        };
                        quanLyPhieuTras.Add(phieuTra);
                    }

                    myReader.Close();
                }
            }

            return new JsonResult(quanLyPhieuTras);
        }
    }
}
