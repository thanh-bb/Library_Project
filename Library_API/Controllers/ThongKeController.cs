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


        [HttpGet("ThongKeMuonSachTheoThangChiTiet")]
        public async Task<ActionResult<IEnumerable<SachNoiBat>>> GetThongKeMuonSachTheoThangChiTiet(int month, int year)
        {
            string description = $"Tháng {month}/{year}";

            var result = await _context.Saches
                .Select(s => new SachNoiBat
                {
                    SId = s.SId,
                    STenSach = s.STenSach,
                    TongSoLanMuon = _context.ChiTietPhieuMuons
                        .Where(ctpm => ctpm.SId == s.SId && _context.PhieuMuons
                            .Any(pm => pm.PmId == ctpm.PmId &&
                                       pm.PmNgayMuon.HasValue &&
                                       pm.PmNgayMuon.Value.Year == year &&
                                       pm.PmNgayMuon.Value.Month == month))
                        .Sum(ctpm => (int?)ctpm.CtpmSoLuongSachMuon) ?? 0, // Nếu không có mượn, trả về 0
                    MoTaThoiGian = description
                })
                .Where(s => s.TongSoLanMuon > 0) // Chỉ lấy các sách có dữ liệu mượn
                .OrderByDescending(s => s.TongSoLanMuon)
                .ToListAsync();

            return Ok(result);
        }


        [HttpGet("ThongKeMuonSachTheoQuy")]
        public async Task<ActionResult<IEnumerable<SachNoiBat>>> GetThongKeMuonSachTheoQuy(int year)
        {
            var result = new List<SachNoiBat>();

            // Tạo các khoảng thời gian cho từng quý
            var quarters = new List<(DateTime start, DateTime end)>
    {
        (new DateTime(year, 1, 1), new DateTime(year, 3, 31)),  // Quý 1
        (new DateTime(year, 4, 1), new DateTime(year, 6, 30)),  // Quý 2
        (new DateTime(year, 7, 1), new DateTime(year, 9, 30)),  // Quý 3
        (new DateTime(year, 10, 1), new DateTime(year, 12, 31)) // Quý 4
    };

            for (int i = 0; i < quarters.Count; i++)
            {
                var (startDate, endDate) = quarters[i];
                string quarterDescription = $"Quý {i + 1}/{year}";

                var quarterResult = await _context.Saches
                    .Select(s => new SachNoiBat
                    {
                        SId = s.SId,
                        STenSach = s.STenSach,
                        TongSoLanMuon = _context.ChiTietPhieuMuons
                            .Where(ctpm => ctpm.SId == s.SId && _context.PhieuMuons
                                .Any(pm => pm.PmId == ctpm.PmId && pm.PmNgayMuon >= startDate && pm.PmNgayMuon <= endDate))
                            .Sum(ctpm => (int?)ctpm.CtpmSoLuongSachMuon) ?? 0,
                        MoTaThoiGian = quarterDescription
                    })
                    .Where(s => s.TongSoLanMuon > 0)
                    .OrderByDescending(s => s.TongSoLanMuon)
                    .ToListAsync();

                result.AddRange(quarterResult);
            }

            return Ok(result);
        }





    }
}
