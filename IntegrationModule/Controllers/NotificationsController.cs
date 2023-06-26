using BLL.Models;
using BLL.Services;
using DAL.Models;
using DAL.Requests;
using DAL.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly NotificationService _notificationService;

        public NotificationsController(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetAllNotification()
        {
            try
            {
                var allNotification = await _notificationService.GetAllNotifications();

                return Ok(allNotification.Select(dbNotification =>
                    new NotificationResponse
                    {
                        Id = dbNotification.Id,
                        ReceiverEmail = dbNotification.ReceiverEmail,
                        Subject = dbNotification.Subject,
                        Body = dbNotification.Body

                    }
                ));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<NotificationResponse>> GetNotification(int id)
        {
            try
            {
                var dbNotification = await _notificationService.GetNotificationById(id);

                if (dbNotification == null)
                    return NotFound();

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<NotificationResponse>> CreateNotification(NotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

             
                    var dbNotification = new BLNotification
                    {
                        ReceiverEmail = request.ReceiverEmail,
                        Subject = request.Subject,
                        Body = request.Body,
                        CreatedAt= DateTime.Now
                    };

                    await _notificationService.AddNotification(dbNotification);

                    return Ok(new NotificationResponse
                    {
                        Id = dbNotification.Id,
                        ReceiverEmail = dbNotification.ReceiverEmail,
                        Subject = dbNotification.Subject,
                        Body = dbNotification.Body
                    });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<NotificationResponse>> ModifyNotification(int id, [FromBody] NotificationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = await _notificationService.GetNotificationById(id);

                if (dbNotification == null)
                    return NotFound();

                dbNotification.ReceiverEmail = request.ReceiverEmail;
                dbNotification.Subject = request.Subject;
                dbNotification.Body = request.Body;

                await _notificationService.UpdateNotification(dbNotification);

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpDelete("[action]")]
        public async Task<ActionResult<NotificationResponse>> RemoveNotification(int id)
        {
            try
            {
                var dbNotification = await _notificationService.GetNotificationById(id);
                if (dbNotification == null)
                    return NotFound();

                await _notificationService.DeleteNotification(id);

                return Ok(new NotificationResponse
                {
                    Id = dbNotification.Id,
                    ReceiverEmail = dbNotification.ReceiverEmail,
                    Subject = dbNotification.Subject,
                    Body = dbNotification.Body
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Pitanje da li ovo treba biti sinkrono ili asinkrono,pitati prof
        [HttpPost("[action]")]
        public async Task<ActionResult> SendAllNotifications()
        {
            var client = new SmtpClient("127.0.0.1", 25);
            var sender = "admin@testing-webapi.com";

            try
            {
                var allNotifications = await _notificationService.GetAllNotifications();
                var unsentNotifications = allNotifications.Where(x => !x.SentAt.HasValue);

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

                        // Save the updated notification to the database
                        await _notificationService.UpdateNotification(notification);

                    }
                    catch (Exception)
                    {
                        // 
                    }
                }

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("[action]/{count}")]
        public async Task<ActionResult<SendNotificationsResponse>> SendNotificationBatch(int? count)
        {
            var client = new SmtpClient("127.0.0.1", 25);
            var sender = "admin@my-cool-webapi.com";

            try
            {
                var allNotifications = await _notificationService.GetAllNotifications();
                var unsentNotifications =
                     allNotifications
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
                        await _notificationService.UpdateNotification(notification);

                        sendSuccessCount++;
                    }
                    catch (Exception)
                    {
                        sendFailCount++;
                    }
                }
                return Ok(new SendNotificationsResponse
                {
                    SuccessCount = sendSuccessCount,
                    FailCount = sendFailCount
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //Unsent messages
        [HttpGet("[action]")]
        public async Task<IActionResult> GetNumberOfUnsentNotifications()
        {
            int unsent = 0;
            var allNotification = await _notificationService.GetAllNotifications();

            foreach (var item in allNotification)
            {
                if (item.SentAt is null)
                {
                    unsent++;
                }
            }

            return Ok(unsent);
        }

    }
}
