using Library_API.Dtos;
using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly OnlineLibraryContext _context;

        public ThongKeController(IConfiguration configuration, OnlineLibraryContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("SachNoiBat")]
        public async Task<ActionResult<IEnumerable<SachNoiBat>>> GetSachNoiBat()
        {
            // Truy vấn sách và số lần mượn
            var result = await _context.ChiTietPhieuMuons
                .GroupBy(ctpm => ctpm.SId) // Nhóm theo s_Id (ID sách)
                .Select(g => new SachNoiBat
                {
                    SId = g.Key, // Lấy s_Id
                    STenSach = _context.Saches.FirstOrDefault(s => s.SId == g.Key).STenSach,
                    SoLanMuon = (int)g.Sum(ctpm => ctpm.CtpmSoLuongSachMuon),
                    TgId = _context.Saches.FirstOrDefault(s => s.SId == g.Key).TgId,
                    STrangThaiMuon = _context.Saches.FirstOrDefault(s => s.SId == g.Key).STrangThaiMuon
                })
                .OrderByDescending(s => s.SoLanMuon)
                .ToListAsync();

            
            return Ok(result);
        }

        //BXH bạn đọc
        [HttpGet("TopReaders")]
        public async Task<ActionResult<IEnumerable<TopReader>>> GetTopReaders()
        {
            // Get the current date and calculate the previous month
            var currentDate = DateTime.Now;
            var previousMonth = new DateTime(currentDate.Year, currentDate.Month , 1);
            var nextMonth = previousMonth.AddMonths(1);

            // Query the top readers for the previous month
            var result = await _context.ChiTietPhieuMuons
                .Where(ctpm => _context.PhieuMuons
                    .Any(pm => pm.PmId== ctpm.PmId && pm.PmNgayMuon >= previousMonth && pm.PmNgayMuon < nextMonth))
                .GroupBy(ctpm => ctpm.Pm.NdId)
                .Select(g => new TopReader
                {
                    NguoiDungId = (int)g.Key,
                    HoTen = _context.NguoiDungs.FirstOrDefault(nd => nd.NdId == g.Key).NdHoTen,
                    SoLuongSachMuon = (int)g.Sum(ctpm => ctpm.CtpmSoLuongSachMuon)
                })
                .OrderByDescending(r => r.SoLuongSachMuon)
                .Take(6) // Limit to top 10 readers
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("ThongKeMuonSachTheoNgay")]
        public async Task<ActionResult<IEnumerable<SachNoiBat>>> GetThongKeMuonSachTheoNgay(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Ngày bắt đầu phải trước ngày kết thúc.");
            }

            string description = $"Từ {startDate:dd/MM/yyyy} đến {endDate:dd/MM/yyyy}";

            var result = await _context.Saches
                .Select(s => new SachNoiBat
                {
                    SId = s.SId,
                    STenSach = s.STenSach,
                    TongSoLanMuon = _context.ChiTietPhieuMuons
                        .Where(ctpm => ctpm.SId == s.SId && _context.PhieuMuons
                            .Any(pm => pm.PmId == ctpm.PmId && pm.PmNgayMuon >= startDate && pm.PmNgayMuon <= endDate))
                        .Sum(ctpm => (int?)ctpm.CtpmSoLuongSachMuon) ?? 0, // Nếu không có mượn, trả về 0
                    MoTaThoiGian = description
                })
                .Where(s => s.TongSoLanMuon > 0) // Chỉ lấy các sách có dữ liệu mượn
                .OrderByDescending(s => s.TongSoLanMuon)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("ThongKeMuonSach7NgayQua")]
        public async Task<ActionResult<IEnumerable<SachNoiBat>>> GetThongKeMuonSach7NgayQua()
        {
            // Tính ngày bắt đầu và ngày kết thúc là 7 ngày gần đây
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddDays(-6); // 7 ngày trước tính từ hôm nay

            string description = $"7 ngày qua ({startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy})";

            var result = await _context.Saches
                .Select(s => new SachNoiBat
                {
                    SId = s.SId,
                    STenSach = s.STenSach,
                    TongSoLanMuon = _context.ChiTietPhieuMuons
                        .Where(ctpm => ctpm.SId == s.SId && _context.PhieuMuons
                            .Any(pm => pm.PmId == ctpm.PmId && pm.PmNgayMuon >= startDate && pm.PmNgayMuon <= endDate))
                        .Sum(ctpm => (int?)ctpm.CtpmSoLuongSachMuon) ?? 0, // Nếu không có mượn, trả về 0
                    MoTaThoiGian = description
                })
                .Where(s => s.TongSoLanMuon > 0) // Chỉ lấy các sách có dữ liệu mượn
                .OrderByDescending(s => s.TongSoLanMuon)
                .ToListAsync();

            return Ok(result);
        }



    }
}
