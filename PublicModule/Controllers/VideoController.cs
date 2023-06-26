using AutoMapper;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PublicModule.Models;
using PublicModule.Pages;

namespace PublicModule.Controllers
{
    [Authorize]
    public class VideoController : Controller
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IMapper _mapper;

        private readonly VideoService _videoService;
        private readonly TagService _tagService;
        private readonly GenreService _genreService;
        private readonly ImageService _imageService;
        private readonly VideoTagService _videoTagService;

        public VideoController(ILogger<VideoController> logger, IMapper mapper, VideoService videoService)
        {
            _mapper = mapper;
            _logger = logger;
            _videoService = videoService;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchQuery, bool clearFilter = false,
            int page = 1)
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowRegister = false;
            ViewBag.ShowLogout = true;
            //Spremanje filtera pomocu cookija 
            if (clearFilter)
            {
                Response.Cookies.Delete("SearchQueryVideo");
                searchQuery = null;
            }

            else if (string.IsNullOrEmpty(searchQuery))
            {
                if (Request.Cookies.TryGetValue("SearchQueryVideo", out string storedSearchQuery))
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
                Response.Cookies.Append("SearchQueryVideo", searchQuery, cookieOptions);
            }

            var videos = await _videoService.GetAllVideos();
            var sortVideos = _mapper.Map<IEnumerable<VMPublicVideo>>(videos);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                sortVideos = sortVideos.Where(v =>
                    v.Name.Contains(searchQuery)
                    || v.Description.Contains(searchQuery)
                    || v.Genre.Name.Contains(searchQuery)
                    || v.VideoTags.Any(t => t.Tag.Name.Contains(searchQuery)));
            }

            switch (sortOrder)
            {
                case "Name":
                    sortVideos = sortVideos.OrderBy(v => v.Name);
                    break;
                case "Genre":
                    sortVideos = sortVideos.OrderBy(v => v.Genre.Name);
                    break;
                case "Name desc":
                    sortVideos = sortVideos.OrderByDescending(v => v.Name);
                    break;
                case "Genre desc":
                    sortVideos = sortVideos.OrderByDescending(v => v.Genre.Name);
                    break;
                default:
                    sortVideos = sortVideos.OrderByDescending(v => v.Id);
                    break;
            }

            const int pageSize = 6;
            if (page < 1)
            {
                page = 1;
            }

            int videosCount = sortVideos.Count();
            var pager = new Pager(videosCount, page, pageSize);

            int videoSkip = (page - 1) * pageSize;

            var data = sortVideos.Skip(videoSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            //Da bi odrzao sort nakon izmjene stranice
            ViewBag.CurrentSortOrder = sortOrder;


            return View(data);
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewBag.ShowLogin = false;
            ViewBag.ShowRegister = false;
            ViewBag.ShowLogout = true;

            var video = await _videoService.GetVideoById(id);
            var vmVideo = _mapper.Map<VMPublicVideo>(video);

            return View(vmVideo);
        }
    }
}
