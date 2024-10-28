using ExcelDataReader;
using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportFileController : ControllerBase
    {
        private readonly OnlineLibraryContext _libraryContext;

        public ImportFileController(OnlineLibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        [HttpPost("UploadExcelFile")]
        public IActionResult UploadExcelFile([FromForm] IFormFile file)
        {
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file upload");
                }

                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\Upload";
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        bool isHeaderSkipped = false;
                        do
                        {
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                NguoiDung nd = new NguoiDung();
                                nd.NdUsername = reader.GetValue(1)?.ToString() ?? string.Empty;
                                nd.NdCccd = reader.GetValue(2)?.ToString() ?? string.Empty;
                                nd.NdSoDienThoai = reader.GetValue(3)?.ToString() ?? string.Empty;
                                nd.NdHinhThe = reader.GetValue(4)?.ToString() ?? string.Empty;
                                nd.NdPassword = reader.GetValue(5)?.ToString() ?? string.Empty;
                                nd.NdHoTen = reader.GetValue(6)?.ToString() ?? string.Empty;

                                if (!reader.IsDBNull(7))
                                {
                                    nd.NdNgaySinh = DateTime.Parse(reader.GetValue(7).ToString());
                                }

                                nd.NdGioiTinh = reader.GetValue(8)?.ToString() ?? string.Empty;
                                nd.NdEmail = reader.GetValue(9)?.ToString() ?? string.Empty;
                                nd.NdDiaChi = reader.GetValue(10)?.ToString() ?? string.Empty;

                                if (!reader.IsDBNull(11))
                                {
                                    nd.NdNgayDangKy = DateTime.Parse(reader.GetValue(11).ToString());
                                }
                                else
                                {
                                    nd.NdNgayDangKy = DateTime.Now;
                                }

                                nd.NdThoiGianSuDung = reader.GetValue(12)?.ToString() ?? string.Empty;

                                if (!reader.IsDBNull(13))
                                {
                                    nd.NdActive = Convert.ToBoolean(reader.GetValue(13));
                                }
                                else
                                {
                                    nd.NdActive = true;
                                }

                                nd.QId = reader.GetValue(14)?.ToString() ?? "02";

                                if (!reader.IsDBNull(15))
                                {
                                    nd.LndLoaiNguoiDung = Convert.ToInt32(reader.GetValue(15));
                                }
                                else
                                {
                                    nd.LndLoaiNguoiDung = 1; // Giá trị mặc định
                                }

                                _libraryContext.Add(nd);
                                _libraryContext.SaveChanges();
                            }

                        } while (reader.NextResult());
                    }
                }

                return Ok("Successfully Inserted");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
