using BLL.Services;
using DAL.Models;
using DAL.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using DAL.Responses;
using System.Net.Mail;
using BLL.Models;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly NotificationService _notificationService;


        public UsersController(UserService userService, NotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<BLUser>> RegisterUser([FromBody] UserRequest request)
        {
            var client = new SmtpClient("127.0.0.1", 25);
            var sender = "admin@testing-webapi.com";

            string registrationSubject = "Account registration RWAMovies app";
            string registrationBody = "Folow this URL to validate your account: DUMMY URL";

            var allNotifications = await _notificationService.GetAllNotifications();
            var registrationNotification =
                    allNotifications.Where(
                        x => !x.SentAt.HasValue && x.Subject == registrationSubject);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                //Add usera
                var newUser = await _userService.AddUser(request);

                //Create new Notification
                var dbNotification = new BLNotification
                {
                    ReceiverEmail = request.Email,
                    Subject = registrationSubject,
                    Body = registrationBody,
                    CreatedAt = DateTime.Now
                };

                await _notificationService.AddNotification(dbNotification);

                //Create and send mail
                var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(dbNotification.ReceiverEmail));

                mail.Subject = dbNotification.Subject;
                mail.Body = dbNotification.Body;

                client.Send(mail);

                dbNotification.SentAt = DateTime.UtcNow;
                _notificationService.SaveNotificationData();

                return Ok(new UserResponse
                {
                    Id = newUser.Id,
                    SecurityToken = newUser.SecurityToken
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ValidateUserEmail([FromBody] EmailValidationRequest request)
        {
            try
            {
                await _userService.ValidateEmail(request);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<Tokens>> JwtTokens([FromBody] JWTTokensRequest request)
        {
            try
            {
                return Ok(await _userService.JwtTokens(request));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
