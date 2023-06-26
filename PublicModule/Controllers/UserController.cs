using AutoMapper;
using BLL.Models;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicModule.Models;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PublicModule.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;
        private readonly NotificationService _notificationService;
        private readonly CountryService _countryService;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, UserService userService, 
            CountryService countryService,NotificationService notificationService, IMapper mapper)
        {
            _logger = logger;
            _userService = userService;
            _notificationService = notificationService;
            _countryService = countryService;
            _mapper = mapper;
        }

        private async Task LoadViewBagData()
        {
            var country = await _countryService.GetAllCountries();
            var vmCountry = _mapper.Map<IEnumerable<VMPublicCountry>>(country);
            ViewBag.CountryOfResidenceId = new SelectList(vmCountry, "Id", "Name");
        }

        public async Task<IActionResult> Index()
        {
            var blUsers = await _userService.GetAllUsers();
            var vmUsers = _mapper.Map<IEnumerable<VMPublicUser>>(blUsers);

            return View(vmUsers);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowRegister = false;
            ViewBag.ShowLogout = true;
            ViewBag.ShowUsername = true;
            var user = await _userService.GetUserById(id);
            var vmUser = _mapper.Map<VMPublicUser>(user);

            return View(vmUser);
        }


        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowRegister = true;
            ViewBag.ShowLogout = false;
            ViewBag.ShowUsername = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(VMPublicUserLogin loginInfo)
        {
            if (!ModelState.IsValid)
                return View(loginInfo);

            var user = await _userService.GetConfirmedUser(
                loginInfo.Username,
                loginInfo.Password);

            // if user is null, add error to ModelState and return view
            if (user == null)
            {
                ViewBag.ShowLogin = false;
                ViewBag.ShowRegister = true;
                ViewBag.ShowLogout = false;
                ViewBag.ShowUsername = false;
                ModelState.AddModelError("", "Invalid username or password");
                return View(loginInfo);
            }

            if (user.DeletedAt != null)
            {
                ViewBag.ShowLogin = false;
                ViewBag.ShowRegister = true;
                ViewBag.ShowLogout = false;
                ViewBag.ShowUsername = false;
                ModelState.AddModelError("", "Your account has been deleted!");
                return View(loginInfo);
            }
            try
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties()).Wait();


                return RedirectToAction("Index", "Video");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();

            return RedirectToAction("Login");
        }


        //GET I POST ZA REGISTER

        public async Task<IActionResult> Register()
        {
            ViewBag.ShowLogin = true;
            ViewBag.ShowRegister = false;
            ViewBag.ShowLogout = false;
            ViewBag.ShowUsername = false;
            await LoadViewBagData();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(VMUserRegister user)
        {

            // If there is a user with the same email or username!
            var allUsers = await _userService.GetAllUsers();
            var existingUser = allUsers.FirstOrDefault(c => c.Username == user.Username);
            var existingEmail = allUsers.FirstOrDefault(c => c.Email == user.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "The username is already in use, Please try another.");
                await LoadViewBagData();
                return View(user);
            }
            if (existingEmail != null)
            {
                ModelState.AddModelError(string.Empty, "The email is already in use, Please try another.");
                await LoadViewBagData();
                return View(user);
            }

            try
            {
                var client = new SmtpClient("127.0.0.1", 25);
                var sender = "admin@testing-webapi.com";

                string registrationSubject = "Account registration RWAMovies app";
                string registrationBody = "User Successfuly created";

                var allNotifications = await _notificationService.GetAllNotifications();
                var registrationNotification =
                        allNotifications.Where(
                            x => !x.SentAt.HasValue && x.Subject == registrationSubject);

                await _userService.AddUser(
                         new DAL.Requests.UserRequest
                         {
                             FirstName = user.FirstName,
                             LastName = user.LastName,
                             Username = user.Username,
                             Email = user.Email,
                             Phone = user.Phone,
                             Country = user.Country,
                             Password = user.Password
                         }
                     );

                var vmNotification = new VMPublicNotification
                {
                    ReceiverEmail = user.Email,
                    Subject = registrationSubject,
                    Body = registrationBody,
                    CreatedAt = DateTime.Now
                };

                var dbNotification = _mapper.Map<BLNotification>(vmNotification);

                await _notificationService.AddNotification(dbNotification);

                // Create and send mail!
                var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(dbNotification.ReceiverEmail));

                mail.Subject = dbNotification.Subject;
                mail.Body = dbNotification.Body;

                try
                {
                    client.Send(mail);

                    dbNotification.SentAt = DateTime.UtcNow;
                    await _notificationService.UpdateNotification(dbNotification);
                }
                catch (SmtpException ex)
                {
                    // I need this when the Smtp service is not available
                }

                // Validation of the user
                var dbUsers = await _userService.GetAllUsers();
                var dbUser = dbUsers.FirstOrDefault(u => u.Email == user.Email);

                await ConfirmEmail(dbUser.Email, dbUser.SecurityToken);

                return RedirectToAction(nameof(Login));
            }
            catch
            {
                ViewBag.ShowLogin = false;
                ViewBag.ShowRegister = false;
                ViewBag.ShowLogout = true;
                ViewBag.ShowUsername = true;
                return View();
            }
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowRegister = false;
            ViewBag.ShowLogout = true;
            ViewBag.ShowUsername = true;
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(VMPublicChangePassword changePassword)
        {
            if (!ModelState.IsValid)
            {
                
                return View(changePassword);
            }

            try
            {
                await _userService.ChangePassword(changePassword.Username, changePassword.NewPassword);
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Check your credentials, something is wrong!");
                return View(changePassword);
            }

            return RedirectToAction("Index", "Video");
        }

        public async Task ConfirmEmail(string email, string securityToken)
        {
           ViewBag.ShowLogin = false;
           ViewBag.ShowRegister = false;
           ViewBag.ShowLogout = false;
           ViewBag.ShowUsername = false;

           var dbUsers =  await _userService.GetAllUsers();
            var dbUser = dbUsers.FirstOrDefault(
                u => u.Email == email && u.SecurityToken == securityToken);

            dbUser.IsConfirmed = true;

            await _userService.UpdateUser(dbUser);
        }

    }
}
