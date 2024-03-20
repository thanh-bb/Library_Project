using Library_API.Controllers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Library_API
{
    public class PhieuMuonWorker : BackgroundService
    {
        private readonly ILogger<PhieuMuonWorker> _logger;
        private readonly IServiceProvider _serviceProvider;

        public PhieuMuonWorker(ILogger<PhieuMuonWorker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("PhieuMuonWorker is starting.");

            stoppingToken.Register(() =>
                _logger.LogInformation("PhieuMuonWorker is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Gọi phương thức cập nhật trạng thái khi server được khởi động
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var phieuMuonController = scope.ServiceProvider.GetRequiredService<PhieuMuonController>();
                        phieuMuonController.CapNhatTrangThai();
                    }

                    _logger.LogInformation("PhieuMuonWorker is running at: {time}", DateTimeOffset.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating PhieuMuon status.");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken); 
            }
        }
    }
}
