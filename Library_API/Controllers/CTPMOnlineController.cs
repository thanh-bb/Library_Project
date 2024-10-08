using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CTPMOnlineController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CTPMOnlineController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //[HttpPost]
        //public JsonResult Post(PhieuMuonOnline pmo)
        //{
        //    string query = @"
        //                    insert into dbo.PhieuMuonOnline
        //                    (nd_Id, pmo_NgayDat, pmo_LoaiGiaoHang, pmo_PhuongThucThanhToan, dcgh_Id, pmo_TrangThai)
        //                    values (@nd_Id, @pmo_NgayDat, @pmo_LoaiGiaoHang, @pmo_PhuongThucThanhToan, @dcgh_Id, @pmo_TrangThai);
        //                    SELECT SCOPE_IDENTITY(); 
        //                    ";

        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("MyConnection");
        //    int newPmoId;
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myCommand.Parameters.AddWithValue("@nd_Id", pmo.NdId);
        //            myCommand.Parameters.AddWithValue("@pmo_NgayDat", pmo.PmoNgayDat);
        //            myCommand.Parameters.AddWithValue("@pmo_LoaiGiaoHang", pmo.PmoLoaiGiaoHang);
        //            myCommand.Parameters.AddWithValue("@pmo_PhuongThucThanhToan", pmo.PmoPhuongThucThanhToan);
        //            myCommand.Parameters.AddWithValue("@dcgh_Id", pmo.DcghId);
        //            myCommand.Parameters.AddWithValue("@pmo_TrangThai", pmo.PmoTrangThai);

        //            newPmoId = Convert.ToInt32(myCommand.ExecuteScalar());
        //        }

        //        foreach (var cto in pmo.ChiTietPhieuMuonOnlines)
        //        {
        //            string ctquery = @"
        //                             insert into dbo.ChiTietPhieuMuonOnline
        //                             (pmo_Id, s_Id, ctpmo_SoLuongSachMuon)
        //                             values (@pmo_Id, @s_Id, @ctpmo_SoLuongSachMuon)
        //                            ";

        //            using (SqlCommand insertChiTietCommand = new SqlCommand(ctquery, myCon))
        //            {
        //                insertChiTietCommand.Parameters.AddWithValue("@pmo_Id", cto.PmoId); // Sử dụng ID mới của phiếu mượn
        //                insertChiTietCommand.Parameters.AddWithValue("@s_Id", cto.SId);
        //                insertChiTietCommand.Parameters.AddWithValue("@ctpmo_SoLuongSachMuon", cto.CtpmoSoLuongSachMuon);
        //                insertChiTietCommand.ExecuteNonQuery();
        //            }

        //            // cap nhat so luong sach trong kho
        //            string updateSLQuery = @"
        //            UPDATE dbo.Sach
        //            SET s_SoLuong = s_SoLuong - @ctpmo_SoLuongSachMuon
        //            WHERE s_Id = @s_Id
        //            ";

        //            using (SqlCommand updateSoLuongCommand = new SqlCommand(updateSLQuery, myCon))
        //            {
        //                updateSoLuongCommand.Parameters.AddWithValue("@s_Id", cto.SId);
        //                updateSoLuongCommand.Parameters.AddWithValue("@ctpmo_SoLuongSachMuon", cto.CtpmoSoLuongSachMuon);
        //                updateSoLuongCommand.ExecuteNonQuery();
        //            }


        //            // Kiểm tra và cập nhật trạng thái mượn của sách
        //            string checkSoLuongQuery = @"
        //            SELECT s_SoLuong
        //            FROM dbo.Sach
        //            WHERE s_Id = @s_Id
        //            ";

        //            using (SqlCommand checkSoLuongCommand = new SqlCommand(checkSoLuongQuery, myCon))
        //            {
        //                checkSoLuongCommand.Parameters.AddWithValue("@s_Id", cto.SId);
        //                int soLuongConLai = Convert.ToInt32(checkSoLuongCommand.ExecuteScalar());

        //                if (soLuongConLai == 0)
        //                {
        //                    string updateTrangThaiQuery = @"
        //                    UPDATE dbo.Sach
        //                    SET s_TrangThaiMuon = 'false'
        //                    WHERE s_Id = @s_Id
        //                    ";

        //                    using (SqlCommand updateTrangThaiCommand = new SqlCommand(updateTrangThaiQuery, myCon))
        //                    {
        //                        updateTrangThaiCommand.Parameters.AddWithValue("@s_Id", cto.SId);
        //                        updateTrangThaiCommand.ExecuteNonQuery();
        //                    }
        //                }
        //            }
        //        }
        //        myCon.Close();
        //    }
        //    return new JsonResult("Thêm thành công");
        //}


        [HttpPost]
        public JsonResult Post(PhieuMuonOnline pmo)
        {
            string query = @"
                    INSERT INTO dbo.PhieuMuonOnline
                    (nd_Id, pmo_NgayDat, pmo_LoaiGiaoHang, pmo_PhuongThucThanhToan, dcgh_Id, pmo_TrangThai)
                    VALUES (@nd_Id, @pmo_NgayDat, @pmo_LoaiGiaoHang, @pmo_PhuongThucThanhToan, @dcgh_Id, @pmo_TrangThai);
                    SELECT SCOPE_IDENTITY(); 
                    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            int newPmoId;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@nd_Id", pmo.NdId);
                    myCommand.Parameters.AddWithValue("@pmo_NgayDat", pmo.PmoNgayDat);
                    myCommand.Parameters.AddWithValue("@pmo_LoaiGiaoHang", pmo.PmoLoaiGiaoHang);
                    myCommand.Parameters.AddWithValue("@pmo_PhuongThucThanhToan", pmo.PmoPhuongThucThanhToan);
                    myCommand.Parameters.AddWithValue("@dcgh_Id", pmo.DcghId);
                    myCommand.Parameters.AddWithValue("@pmo_TrangThai", pmo.PmoTrangThai);

                    newPmoId = Convert.ToInt32(myCommand.ExecuteScalar());
                }

                // Lưu chi tiết phiếu mượn online (cập nhật chi tiết mượn sách)
                foreach (var cto in pmo.ChiTietPhieuMuonOnlines)
                {
                    string ctquery = @"
                             INSERT INTO dbo.ChiTietPhieuMuonOnline
                             (pmo_Id, s_Id, ctpmo_SoLuongSachMuon)
                             VALUES (@pmo_Id, @s_Id, @ctpmo_SoLuongSachMuon)
                            ";

                    using (SqlCommand insertChiTietCommand = new SqlCommand(ctquery, myCon))
                    {
                        insertChiTietCommand.Parameters.AddWithValue("@pmo_Id", newPmoId);
                        insertChiTietCommand.Parameters.AddWithValue("@s_Id", cto.SId);
                        insertChiTietCommand.Parameters.AddWithValue("@ctpmo_SoLuongSachMuon", cto.CtpmoSoLuongSachMuon);
                        insertChiTietCommand.ExecuteNonQuery();
                    }

                    // Cập nhật số lượng sách trong kho và kiểm tra trạng thái
                    // Cập nhật số lượng sách trong kho
                    string updateSoLuongQuery = @"
                    UPDATE dbo.Sach
                    SET s_SoLuong = s_SoLuong - @ctpm_SoLuongSachMuon
                    WHERE s_Id = @s_Id
                    ";

                    using (SqlCommand updateSoLuongCommand = new SqlCommand(updateSoLuongQuery, myCon))
                    {
                        updateSoLuongCommand.Parameters.AddWithValue("@s_Id", cto.SId);
                        updateSoLuongCommand.Parameters.AddWithValue("@ctpm_SoLuongSachMuon", cto.CtpmoSoLuongSachMuon);
                        updateSoLuongCommand.ExecuteNonQuery();
                    }

                    // Kiểm tra và cập nhật trạng thái mượn của sách
                    string checkSoLuongQuery = @"
                    SELECT s_SoLuong
                    FROM dbo.Sach
                    WHERE s_Id = @s_Id
                    ";

                    using (SqlCommand checkSoLuongCommand = new SqlCommand(checkSoLuongQuery, myCon))
                    {
                        checkSoLuongCommand.Parameters.AddWithValue("@s_Id", cto.SId);
                        int soLuongConLai = Convert.ToInt32(checkSoLuongCommand.ExecuteScalar());

                        if (soLuongConLai == 0)
                        {
                            string updateTrangThaiQuery = @"
                            UPDATE dbo.Sach
                            SET s_TrangThaiMuon = 'false'
                            WHERE s_Id = @s_Id
                            ";

                            using (SqlCommand updateTrangThaiCommand = new SqlCommand(updateTrangThaiQuery, myCon))
                            {
                                updateTrangThaiCommand.Parameters.AddWithValue("@s_Id", cto.SId);
                                updateTrangThaiCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }

                // Xử lý thanh toán dựa trên phương thức thanh toán
                if (pmo.PmoPhuongThucThanhToan == "COD")
                {
                    // Lưu thông tin thanh toán COD
                    string thanhToanQuery = @"
            INSERT INTO dbo.ThanhToan
            (pmo_Id, tt_PhuongThuc, tt_TrangThai, tt_NgayThanhToan)
            VALUES (@pmo_Id, @tt_PhuongThuc, @tt_TrangThai, @tt_NgayThanhToan)
            ";

                    using (SqlCommand thanhToanCommand = new SqlCommand(thanhToanQuery, myCon))
                    {
                        thanhToanCommand.Parameters.AddWithValue("@pmo_Id", newPmoId);
                        thanhToanCommand.Parameters.AddWithValue("@tt_PhuongThuc", "COD");
                        thanhToanCommand.Parameters.AddWithValue("@tt_TrangThai", "Chờ thanh toán");  // COD là thanh toán khi nhận hàng
                        thanhToanCommand.Parameters.AddWithValue("@tt_NgayThanhToan", DateTime.Now);
                        thanhToanCommand.ExecuteNonQuery();
                    }
                }
                else if (pmo.PmoPhuongThucThanhToan == "VNPAY")
                {
                    // Lưu thông tin thanh toán VNPAY
                    string thanhToanQuery = @"
            INSERT INTO dbo.ThanhToan
            (pmo_Id, tt_PhuongThuc, tt_TrangThai, tt_NgayThanhToan)
            VALUES (@pmo_Id, @tt_PhuongThuc, @tt_TrangThai, @tt_NgayThanhToan)
            ";

                    using (SqlCommand thanhToanCommand = new SqlCommand(thanhToanQuery, myCon))
                    {
                        thanhToanCommand.Parameters.AddWithValue("@pmo_Id", newPmoId);
                        thanhToanCommand.Parameters.AddWithValue("@tt_PhuongThuc", "VNPAY");
                        thanhToanCommand.Parameters.AddWithValue("@tt_TrangThai", "Chờ xác nhận");  // VNPAY có thể cần chờ xác nhận
                        thanhToanCommand.Parameters.AddWithValue("@tt_NgayThanhToan", DateTime.Now);
                        thanhToanCommand.ExecuteNonQuery();
                    }

                    // Ở đây bạn có thể thực hiện API call đến VNPAY nếu cần thiết
                    // (gọi đến API thanh toán của VNPAY)
                }

                myCon.Close();
            }

            return new JsonResult("Thêm thành công");
        }

    }
}
