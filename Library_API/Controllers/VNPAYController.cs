using Library_API.Model_VNPay;
using Library_API.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VNPAYController : ControllerBase
    {
        private readonly IVnPayService _vnPayService;
        private readonly IConfiguration _configuration;

        public VNPAYController(IVnPayService vnPayService, IConfiguration configuration)
        {
            _vnPayService = vnPayService;
            _configuration = configuration;
        }

        [HttpGet("index")]
        public IActionResult Index()
        {
            return Ok("VNPAY API is running");
        }

        [HttpPost("CreatePaymentUrl")]
        public IActionResult CreatePaymentUrl([FromBody] PaymentInformationModel model)
        {
            // Gọi dịch vụ VNPAY để tạo URL thanh toán với thông tin từ model
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            // Trả về URL cho client để chuyển hướng người dùng đến trang thanh toán
            return Ok(new { paymentUrl = url });
        }

        [HttpGet("PaymentCallback")]
        public IActionResult PaymentCallback()
        {
            // Xử lý phản hồi từ VNPAY sau khi người dùng thực hiện thanh toán
            var response = _vnPayService.PaymentExecute(Request.Query);

            // Tạo URL của frontend với kết quả thanh toán
            string frontendUrl = _configuration["FrontendUrl"] ?? "http://localhost:3000/vnpay"; 
            string queryString = $"?success={response.Success}&transactionId={response.TransactionId}&orderId={response.OrderId}";

            // Chuyển hướng người dùng đến trang frontend kèm theo kết quả
            return Redirect(frontendUrl + queryString);
        }
    }
}
