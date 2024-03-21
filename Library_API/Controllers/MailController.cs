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
        private readonly LibraryContext _dbContext;
        public MailController(IConfiguration configuration, IEmailService service, LibraryContext dbContext)
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
                    .Where(x => x.PM.PmTrangThai == "Đang mượn" && x.PM.PmHanTra < DateTime.Today)
                    .Select(x => x.User.NdEmail)
                    .ToListAsync();


                foreach (var email in overdueUsers)
                {
                    Mailrequest mailrequest = new Mailrequest();
                    mailrequest.ToEmail = email;
                    mailrequest.Subject = "Thông báo quá hạn trả sách";
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
            string Response = "<div style=\"width:100%;background-color:light;text-align:center;margin:10px\">";
            Response += "<h1>Chào mừng bạn đến với Thư Viện Mon&Man</h1>";
            //Response += "<img src=\"https://yt3.googleusercontent.com/v5hyLB4am6E0GZ3y-JXVCxT9g8157eSeNggTZKkWRSfq_B12sCCiZmRhZ4JmRop-nMA18D2IPw=s176-c-k-c0x00ffffff-no-rj\" />";
            //Response += "<h2>Thanks for subscribed us</h2>";
            //Response += "<a href=\"https://www.youtube.com/channel/UCsbmVmB_or8sVLLEq4XhE_A/join\">Please join membership by click the link</a>";
            Response += "<div><h1>Hãy trả sách đúng thời hạn nhóe!</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
