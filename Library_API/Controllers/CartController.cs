using Library_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public CartController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("GetListCart/{nd_id}")]
        public JsonResult GetListCart(int nd_id)
        {
            string query = @"
        SELECT Sach.s_TenSach, Sach.tl_Id, Sach.tg_Id, Sach.s_MoTa, Sach.s_TrongLuong, Sach.s_NamXuatBan, ChiTietGioHang.ctgh_Id, ChiTietGioHang.s_Id, ChiTietGioHang.gh_Id
        FROM ChiTietGioHang
        JOIN Sach ON ChiTietGioHang.s_Id = Sach.s_Id
        JOIN GioHang ON ChiTietGioHang.gh_Id = GioHang.gh_Id
        WHERE GioHang.nd_Id = @NdId;
    ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@NdId", nd_id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }


        [HttpGet("CheckOrCreateUserCart")]
        public async Task<IActionResult> CheckOrCreateUserCart(int userId)
        {
            string checkCartQuery = @"
        SELECT gh_Id FROM GioHang
        WHERE nd_Id = @nd_Id;
    ";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            int cartId = 0;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                await myCon.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(checkCartQuery, myCon))
                {
                    cmd.Parameters.AddWithValue("@nd_Id", userId);
                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null)
                    {
                        cartId = Convert.ToInt32(result);  // Nếu tìm thấy giỏ hàng, trả về cartId
                    }
                }
                await myCon.CloseAsync();
            }

            if (cartId > 0)
            {
                return Ok(cartId);  // Giỏ hàng đã tồn tại, trả về cartId
            }

            // Nếu giỏ hàng chưa tồn tại, tạo mới
            string insertCartQuery = @"
        INSERT INTO GioHang (nd_Id)
        VALUES (@nd_Id);
        SELECT SCOPE_IDENTITY();
    ";

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                await myCon.OpenAsync();
                using (SqlCommand insertCmd = new SqlCommand(insertCartQuery, myCon))
                {
                    insertCmd.Parameters.AddWithValue("@nd_Id", userId);
                    object newCartId = await insertCmd.ExecuteScalarAsync();
                    cartId = Convert.ToInt32(newCartId);  // Trả về cartId mới vừa tạo
                }
                await myCon.CloseAsync();
            }

            return Ok(cartId);  // Trả về cartId mới
        }


        // tạo giỏ hàng

        //[HttpPost("CreateCart")]
        //public IActionResult CreateCart(GioHang Cart)
        //{
        //    string query = @"
        //                     INSERT INTO GioHang (nd_Id) 
        //                     VALUES (@nd_Id);
        //                     SELECT SCOPE_IDENTITY();";  // Trả về ID của giỏ hàng vừa được tạo

        //    string sqlDataSource = _configuration.GetConnectionString("MyConnection");

        //    try
        //    {
        //        using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //        {
        //            using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //            {
        //                myCommand.Parameters.AddWithValue("@nd_Id", Cart.NdId);

        //                myCon.Open();
        //                var cartId = myCommand.ExecuteScalar();  // Lấy ID của giỏ hàng vừa được tạo
        //                myCon.Close();

        //                return Ok(new { message = "Giỏ hàng đã được tạo.", CartId = cartId });
        //            }
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi cơ sở dữ liệu", error = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đã xảy ra lỗi", error = ex.Message });
        //    }
        //}


        // thêm vào chi tiết giỏ hàng

        [HttpPost("AddToCart")]
        public IActionResult AddToCart(ChiTietGioHang ctgh)
        {
            string query = @"
                            INSERT INTO ChiTietGioHang (gh_Id, s_Id) 
                            VALUES (@gh_Id, @s_Id);";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@gh_Id", ctgh.GhId);
                        myCommand.Parameters.AddWithValue("@s_Id", ctgh.SId);
                       

                        myCon.Open();
                        myCommand.ExecuteNonQuery();  // Thực hiện lệnh SQL
                        myCon.Close();

                        return Ok(new { message = "Sách đã được thêm vào giỏ hàng." });
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi cơ sở dữ liệu", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đã xảy ra lỗi", error = ex.Message });
            }
        }


        // Kiem tra username

        [HttpGet("CheckBookAvailability")]
        public async Task<IActionResult> CheckBookAvailability(string sId, string userId)
        {
            // SQL query to check if the book is in the user's cart
            string checkBookInCartQuery = @"
        SELECT COUNT(1) 
        FROM GioHang gh
        JOIN ChiTietGioHang ctgh ON gh.gh_Id = ctgh.gh_Id
        WHERE ctgh.s_Id = @s_Id AND gh.nd_Id = @userId;
    ";

            // Get the connection string from configuration
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                await myCon.OpenAsync();
                using (SqlCommand checkCmd = new SqlCommand(checkBookInCartQuery, myCon))
                {
                    // Add the parameters for book ID and user ID
                    checkCmd.Parameters.AddWithValue("@s_Id", sId);
                    checkCmd.Parameters.AddWithValue("@userId", userId);

                    // Execute the query and check if the book exists in the user's cart
                    int bookInCartCount = (int)await checkCmd.ExecuteScalarAsync();

                    // If the count is greater than 0, the book is already in the cart
                    if (bookInCartCount > 0)
                    {
                        return Ok(false); // Book is already in the cart
                    }
                }

                await myCon.CloseAsync();
            }

            return Ok(true); // Book is not in the cart, it's available to be added
        }


        [HttpDelete()]
        public JsonResult Delete(int gh_Id, int s_Id)
        {
            string query = @"
                            delete from dbo.ChiTietGioHang
                            where gh_Id=@gh_Id and s_Id= @s_Id
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            SqlDataReader myReader;
            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@gh_Id", gh_Id);
                        myCommand.Parameters.AddWithValue("@s_Id", s_Id);

                        int rowsAffected = myCommand.ExecuteNonQuery(); // Use ExecuteNonQuery for DELETE

                        if (rowsAffected > 0)
                            return new JsonResult("Xóa sách thành công");
                        else
                            return new JsonResult("Không tìm thấy sách hoặc giỏ hàng để xóa");
                    }
                }
            }
            catch (Exception ex)
            {
                return new JsonResult("Lỗi khi xóa: " + ex.Message);
            }


            return new JsonResult("Xóa sách thành công");
        }

        [HttpGet("GetCartItemCount/{nd_id}")]
        public async Task<IActionResult> GetCartItemCount(int nd_id)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM ChiTietGioHang ctgh
        JOIN GioHang gh ON ctgh.gh_Id = gh.gh_Id
        WHERE gh.nd_Id = @NdId;
    ";

            string sqlDataSource = _configuration.GetConnectionString("MyConnection");
            int itemCount = 0;

            try
            {
                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {
                    await myCon.OpenAsync();
                    using (SqlCommand cmd = new SqlCommand(query, myCon))
                    {
                        cmd.Parameters.AddWithValue("@NdId", nd_id);
                        itemCount = (int)await cmd.ExecuteScalarAsync();  // Đếm số lượng sách trong giỏ
                    }
                    await myCon.CloseAsync();
                }

                return Ok(itemCount);  // Trả về số lượng sách trong giỏ
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Đã xảy ra lỗi", error = ex.Message });
            }
        }


    }
}
