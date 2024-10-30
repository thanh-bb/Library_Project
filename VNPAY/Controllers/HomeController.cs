using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VNPAY.Models;
using VNPAY.Services;

namespace VNPAY.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVnPayService _vnPayService;

        public HomeController(IVnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        public IActionResult Index()
        {
            string customerName = HttpContext.Request.Query["customerName"];
            string amount = HttpContext.Request.Query["amount"];
            string description = HttpContext.Request.Query["description"];

            // Tạo ViewModel hoặc sử dụng ViewBag để truyền dữ liệu đến View
            ViewBag.CustomerName = customerName;
            ViewBag.Amount = amount;
            ViewBag.Description = description;

            return View();
        }

       
        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        public IActionResult PaymentCallback()
        {
            // Xử lý phản hồi từ VNPAY
            var response = _vnPayService.PaymentExecute(Request.Query);

            // Tạo URL của frontend với kết quả
            string frontendUrl = "http://localhost:3000/vnpay"; 
            string queryString = $"?success={response.Success}&transactionId={response.TransactionId}&orderId={response.OrderId}";

            // Chuyển hướng người dùng đến trang frontend kèm theo kết quả
            return Redirect(frontendUrl + queryString);
        }

        // signup
        public IActionResult Index1()
        {
            string customerName = HttpContext.Request.Query["customerName"];
            string amount = HttpContext.Request.Query["amount"];
            string description = HttpContext.Request.Query["description"];

            // Tạo ViewModel hoặc sử dụng ViewBag để truyền dữ liệu đến View
            ViewBag.CustomerName = customerName;
            ViewBag.Amount = amount;
            ViewBag.Description = description;

            return View("Index1");
        }

        public IActionResult CreatePaymentUrl1(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl1(model, HttpContext);

            return Redirect(url);
        }

        public IActionResult PaymentCallback1()
        {
            // Xử lý phản hồi từ VNPAY
            var response = _vnPayService.PaymentExecute(Request.Query);

            // Tạo URL của frontend với kết quả
            string frontendUrl = "http://localhost:3000/VnpayForSignUp";
            string queryString = $"?success={response.Success}&transactionId={response.TransactionId}&orderId={response.OrderId}";

            // Chuyển hướng người dùng đến trang frontend kèm theo kết quả
            return Redirect(frontendUrl + queryString);
        }

    }
}
