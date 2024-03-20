using ExcelDataReader;
using ExcelToDatabase.Models;
using ExcelToDatabase.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;

namespace ExcelToDatabase.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LibraryContext _context;

        public HomeController(ILogger<HomeController> logger, LibraryContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                NguoiDung nd = new NguoiDung();
                                nd.NdUsername = reader.GetValue(1)?.ToString() ?? string.Empty;
                                nd.NdPassword = reader.GetValue(2)?.ToString() ?? string.Empty;
                                nd.NdHoTen = reader.GetValue(3)?.ToString() ?? string.Empty;


                                if (!reader.IsDBNull(4))
                                {
                                    // Chuyển đổi từ string sang DateTime và gán trực tiếp vào thuộc tính
                                    nd.NdNgaySinh = DateTime.Parse(reader.GetValue(4).ToString());
                                }
                                else
                                {
                                    continue;
                                }

                                nd.NdGioiTinh = reader.GetValue(5).ToString();
                                nd.NdDiaChi = reader.GetValue(6).ToString();

                                if (!reader.IsDBNull(7))
                                {
                                    // Chuyển đổi từ string sang DateTime và gán trực tiếp vào thuộc tính
                                    nd.NdNgayDangKy = DateTime.Parse(reader.GetValue(7).ToString());
                                }
                                else
                                {
                                    nd.NdNgayDangKy = DateTime.Now;
                                }
                                //
                                if (!reader.IsDBNull(8))
                                {
                                    // Chuyển đổi từ string sang kiểu boolean
                                    nd.NdActive = Convert.ToBoolean(reader.GetValue(8));
                                }
                                else
                                {
                                    nd.NdActive = true;
                                }


                                if (!reader.IsDBNull(9))
                                {
                                    nd.QId = reader.GetValue(9).ToString();
                                }
                                else
                                {

                                    nd.QId = "02";
                                }


                                _context.Add(nd);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "success";
                    }
                }
            }
            else
                ViewBag.Message = "empty";
            return View();
        }
    }
}
