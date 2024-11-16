using Library_API.Helpter;
using Library_API.Models;
using Library_API.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly OnlineLibraryContext _dbContext;
        public MailController(IConfiguration configuration, IEmailService service, OnlineLibraryContext dbContext)
        {
            _configuration = configuration;
            _emailService = service;
            _dbContext = dbContext;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> SendMail()
        {
            try
            {

                var overdueUsers = await _dbContext.NguoiDungs
                   .Join(_dbContext.PhieuMuons,
                    nd => nd.NdId,
                    pm => pm.NdId,
                    (nd, pm) => new { User = nd, PM = pm })
                    .Where(x => x.PM.TtmId == 1 && x.PM.PmHanTra < DateTime.Today && x.PM.PmLoaiMuon=="Mượn trực tiếp")
                    .Select(x => x.User.NdEmail)
                    .ToListAsync();


                foreach (var email in overdueUsers)
                {
                    Mailrequest mailrequest = new Mailrequest();
                    mailrequest.ToEmail = email;
                    mailrequest.Subject = "Thông báo quá hạn trả sách - Loại mượn: Mượn trực tiếp tại thư viện";
                    mailrequest.Body = GetHtmlcontent();

                    await _emailService.SendEmailAsync(mailrequest);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("SendEmailOnline")]
        public async Task<IActionResult> SendMailOnline()
        {
            try
            {

                var overdueUsers = await _dbContext.NguoiDungs
                   .Join(_dbContext.PhieuMuons,
                    nd => nd.NdId,
                    pm => pm.NdId,
                    (nd, pm) => new { User = nd, PM = pm })
                    .Where(x => x.PM.TtmId == 1 && x.PM.PmHanTra < DateTime.Today && x.PM.PmLoaiMuon == "Đặt online")
                    .Select(x => x.User.NdEmail)
                    .ToListAsync();


                foreach (var email in overdueUsers)
                {
                    Mailrequest mailrequest = new Mailrequest();
                    mailrequest.ToEmail = email;
                    mailrequest.Subject = "Thông báo quá hạn trả sách - Loại mượn: Mượn online";
                    mailrequest.Body = GetHtmlcontent();

                    await _emailService.SendEmailAsync(mailrequest);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private string GetHtmlcontent()
        {
            //string Response = "<div style=\"width:100%;background-color:light;text-align:center;margin:10px\">";
            //Response += "<h1>Chào mừng bạn đến với Thư Viện Mon&Man</h1>";
            ////Response += "<img src=\"https://yt3.googleusercontent.com/v5hyLB4am6E0GZ3y-JXVCxT9g8157eSeNggTZKkWRSfq_B12sCCiZmRhZ4JmRop-nMA18D2IPw=s176-c-k-c0x00ffffff-no-rj\" />";
            ////Response += "<h2>Thanks for subscribed us</h2>";
            ////Response += "<a href=\"https://www.youtube.com/channel/UCsbmVmB_or8sVLLEq4XhE_A/join\">Please join membership by click the link</a>";
            //Response += "<div><h1>Hãy trả sách đúng thời hạn nhóe!</h1></div>";
            //Response += "</div>";

            string Response = "<p>Chào bạn,</p>";
            Response += "<p>Đây là thông báo từ thư viện Trường Đại học Cần Thơ. Chúng tôi ghi nhận được bạn đã không trả sách đúng hạn.</p>";
            Response += "<p>Chúng tôi gửi mail này nhằm cảnh báo và yêu cầu bạn trả sách cho thư viện chúng tôi.</p>";
            Response += "<p>Thư viện sẽ bắt đầu tính phí đóng phạt từ sau hạn trả sách 1 ngày. Khi mang sách đến thư viện bạn sẽ nhận được hóa đơn đóng phạt được tính theo quy định.</p>";
            Response += "<p>Lưu ý rằng, nếu sau 30 ngày kể từ Hạn trả sách mà bạn vẫn chưa tiến hành trả sách về thư viện cũng như đóng phạt theo yêu cầu thì tài khoản của bạn sẽ bị khóa tạm thời trong 100 ngày (tài khoản bị khóa sẽ không được mượn sách) và nếu sai phạm liên tục thì sẽ bị khóa tài khoản vĩnh viễn. Nếu muốn kích hoạt lại tài khoản thì bạn cần đóng đủ tiền phạt và trả đủ số lượng sách cho thư viện.</p>";
            Response += "<br>";
            Response += "<p>Thân mến,</p>";
            Response += "<p>Admin Thư viện</p>";


            return Response;
        }


        [HttpPost("SendWelcomeEmail")]
        public async Task<IActionResult> SendWelcomeEmail(int userId)
        {
            try
            {
                // Lấy thông tin người dùng từ bảng NguoiDung dựa trên userId
                var user = await _dbContext.NguoiDungs
                    .Where(nd => nd.NdId == userId)
                    .Select(nd => new { nd.NdEmail, nd.NdUsername, nd.NdPassword })
                    .FirstOrDefaultAsync();

                // Kiểm tra xem người dùng có tồn tại hay không
                if (user == null)
                {
                    return NotFound("Người dùng không tồn tại.");
                }

                // Tạo yêu cầu gửi mail
                Mailrequest mailrequest = new Mailrequest
                {
                    ToEmail = user.NdEmail,
                    Subject = "Thông tin về tài khoản hội viên thư viên trường Đại học",
                    Body = GetWelcomeEmailContent(user.NdUsername, user.NdPassword)
                };

                // Gửi email
                await _emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private string GetWelcomeEmailContent(string username, string password)
        {
            string response = "<p>Chào bạn,</p>";
            response += "<p>Chúc mừng bạn đã tạo tài khoản thành công trên hệ thống thư viện Trường Đại học Cần Thơ.</p>";
            response += "<p>Dưới đây là thông tin tài khoản của bạn:</p>";
            response += $"<p><strong>Username:</strong> {username}</p>";
            response += $"<p><strong>Password:</strong> {password}</p>";
            response += "<p>Hãy bảo mật thông tin này và thay đổi mật khẩu sau khi đăng nhập lần đầu.</p>";
            response += "<br>";
            response += "<p>Thân mến,</p>";
            response += "<p>Admin Thư viện</p>";

            return response;
        }


        // Send OTP
        private string GenerateRandomNumber()
        {
            Random random = new Random();
            string randomNumber = random.Next(0, 100000000).ToString("D6");
            return randomNumber;
        }

        private async Task SendOtpMail(string usermail, string OtpText, string Name)
        {
            var mailRequest = new Mailrequest();
            mailRequest.ToEmail = usermail;
            mailRequest.Subject = "OTP xác thực đổi mật khẩu";
            mailRequest.Body = GenerateEmailBody(Name, OtpText);
            await this._emailService.SendEmailAsync(mailRequest);
        }

        private string GenerateEmailBody(string name, string otptext)
        {
            string emailbody = "<div style='width:100%;background-color:grey'>";
            emailbody += "<h1>Hi " + name + ", Thanks for registering</h1>";
            emailbody += "<h2>Please enter OTP text and complete the registeration</h2>";
            emailbody += "<h2>OTP Text is :" + otptext + "</h2>";
            emailbody += "</div>";

            return emailbody;
        }
    }
}
