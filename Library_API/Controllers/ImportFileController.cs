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
        private readonly LibraryContext _libraryContext;

        public ImportFileController(LibraryContext libraryContext)
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

                                nd.NdEmail = reader.GetValue(10).ToString() ?? string.Empty;

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
