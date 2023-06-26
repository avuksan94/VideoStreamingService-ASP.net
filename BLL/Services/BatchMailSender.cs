using DAL.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    //Ovo je primjer sa vjezbi (IHostedService)
    public class BatchMailSender : IDisposable
    {
        private Timer _timer;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BatchMailSender> _logger;

        public BatchMailSender(ILogger<BatchMailSender> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BatchMailSender started");

            int interval = 30;
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(interval));

            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("Running recurrent mail sending job...");

            int? count = 10;
            string smtpServer = "127.0.0.1";
            int smtpPort = 25;
            string sender = "admin@my-cool-webapi.com";

            using (var scope = _scopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<RwaMoviesContext>();

                try
                {
                    var client = new SmtpClient(smtpServer, smtpPort);

                    var unsentNotifications =
                        _dbContext.Notifications
                            .Where(x => !x.SentAt.HasValue)
                            .OrderBy(x => x.CreatedAt)
                            .AsQueryable();

                    if (count.HasValue)
                        unsentNotifications = unsentNotifications.Take(count.Value);

                    int sendSuccessCount = 0;
                    int sendFailCount = 0;
                    foreach (var notification in unsentNotifications)
                    {
                        try
                        {
                            var mail = new MailMessage(
                                from: new MailAddress(sender),
                                to: new MailAddress(notification.ReceiverEmail));

                            mail.Subject = notification.Subject;
                            mail.Body = notification.Body;

                            client.Send(mail);

                            notification.SentAt = DateTime.UtcNow;

                            sendSuccessCount++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError($"Sending failed: {ex.Message}");
                            sendFailCount++;
                        }
                    }

                    _dbContext.SaveChanges();

                    _logger.LogInformation($"Success count: {sendSuccessCount}");
                    _logger.LogInformation($"Fail count: {sendFailCount}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Batch sending failed: {ex.Message}");
                }
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            _logger.LogInformation("BatchMailSender stopped.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
