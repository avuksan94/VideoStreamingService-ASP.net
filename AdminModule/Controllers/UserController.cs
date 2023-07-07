using AdminModule.Models;
using AutoMapper;
using Azure.Core;
using BLL.Models;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Mail;
using System.Security.Cryptography;

namespace AdminModule.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        private readonly UserService _userService;
        private readonly CountryService _countryService;
        private readonly NotificationService _notificationService;

        public UserController(UserService userService,CountryService countryService,NotificationService notificationService, ILogger<UserController> logger, IMapper mapper) 
        {
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
            _countryService = countryService;
            _notificationService = notificationService;
        }

        private async Task LoadViewBagData()
        {
            var country = await _countryService.GetAllCountries();
            var vmCountry = _mapper.Map<IEnumerable<VMCountry>>(country);
            ViewBag.CountryOfResidenceId = new SelectList(vmCountry, "Id", "Name");
        }

        public async Task<IActionResult> Index(string sortOrder, string searchQuery, bool clearFilter = false)
        {
            //https://www.javatpoint.com/asp-net-cookie#:~:text=ASP.NET%20Cookie%20is%20a,containing%20the%20date%20and%20time.
            //Spremanje filtera pomocu cookija 
            if (clearFilter)
            {
                Response.Cookies.Delete("SearchQuery");
                searchQuery = null;
            }

            else if (string.IsNullOrEmpty(searchQuery))
            {
                if (Request.Cookies.TryGetValue("SearchQuery", out string storedSearchQuery))
                {
                    searchQuery = storedSearchQuery;
                }
            }
            else
            {
                // spremi za iduci put
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true 
                };
                Response.Cookies.Append("SearchQuery", searchQuery, cookieOptions);
            }

            var users = await _userService.GetAllUsers();
            var vmUsers = _mapper.Map<IEnumerable<VMUser>>(users);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                vmUsers = vmUsers.Where(u =>
                    u.FirstName.Contains(searchQuery)
                    || u.LastName.Contains(searchQuery)
                    || u.Username.Contains(searchQuery)
                    || u.CountryOfResidence.Name.Contains(searchQuery));
            }

            switch (sortOrder)
            {
                case "First name":
                    vmUsers = vmUsers.OrderBy(u => u.FirstName);
                    break;
                case "Last name":
                    vmUsers = vmUsers.OrderBy(u => u.LastName);
                    break;
                case "Username":
                    vmUsers = vmUsers.OrderBy(u => u.Username);
                    break;
                case "Country":
                    vmUsers = vmUsers.OrderBy(u => u.CountryOfResidence.Name);
                    break;
                case "First name desc":
                    vmUsers = vmUsers.OrderByDescending(u => u.FirstName);
                    break;
                case "Last name desc":
                    vmUsers = vmUsers.OrderByDescending(u => u.LastName);
                    break;
                case "Username desc":
                    vmUsers = vmUsers.OrderByDescending(u => u.Username);
                    break;
                case "Country desc":
                    vmUsers = vmUsers.OrderByDescending(u => u.CountryOfResidence.Name);
                    break;
                default:
                    vmUsers = vmUsers.OrderByDescending(u => u.Id);
                    break;
            }
            return View(vmUsers);
        }

        public async Task<ActionResult> Create()
        {
            await LoadViewBagData();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VMCreateUser user)
        {

            //If there is a user with the same email or username!
            var allUsers = await _userService.GetAllUsers();
            var existingUser = allUsers.FirstOrDefault(c => c.Username == user.Username);
            var existingEmail = allUsers.FirstOrDefault(c => c.Email == user.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "The username is already in use,Please try another.");
                await LoadViewBagData();
                return View(user);
            }
            if (existingEmail != null)
            {
                ModelState.AddModelError(string.Empty, "The email is already in use,Please try another.");
                await LoadViewBagData();
                return View(user);
            }

            try
            {
                var client = new SmtpClient("127.0.0.1", 25);
                var sender = "admin@testing-webapi.com";

                string registrationSubject = "Account registration RWAMovies app";
                string registrationBody = "Folow this URL to validate your account: DUMMY URL";

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

                var dbNotification = new BLNotification
                {
                    ReceiverEmail = user.Email,
                    Subject = registrationSubject,
                    Body = registrationBody,
                    CreatedAt = DateTime.Now
                };

                //Kreiraj i salji mail!
                var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(dbNotification.ReceiverEmail));

                mail.Subject = dbNotification.Subject;
                mail.Body = dbNotification.Body;

                try
                {
                    client.Send(mail);
                    dbNotification.SentAt = DateTime.Now;
                    await _notificationService.AddNotification(dbNotification);
                } //ukoliko ne radi smtp, kreiraj notifikaciju ali ne smije biti poslana!
                catch (SmtpException ex)
                {
                    await _notificationService.AddNotification(dbNotification);
                }
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetUserById(id);
            var vmUser = _mapper.Map<VMUser>(user);

            return View(vmUser);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetUserById(id);
            var vmUser = _mapper.Map<VMUser>(user);
            await LoadViewBagData();
            return View(vmUser);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VMUser user)
        {
            try
            {
                var dbUser = await _userService.GetUserById(id);

                dbUser.Id = user.Id;
                dbUser.Username = user.Username;
                dbUser.FirstName = user.FirstName;
                dbUser.LastName = user.LastName;
                dbUser.Email = user.Email;
                dbUser.Phone = user.Phone;
                dbUser.IsConfirmed = user.IsConfirmed;
                dbUser.CountryOfResidenceId = user.CountryOfResidenceId;

                await _userService.UpdateUser(dbUser);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: 
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userService.GetUserById(id);
            var vmUser = _mapper.Map<VMUser>(user);

            return View(vmUser);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var dbUser = await _userService.GetUserById(id);

                dbUser.DeletedAt = DateTime.UtcNow;

                await _userService.UpdateUser(dbUser);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
