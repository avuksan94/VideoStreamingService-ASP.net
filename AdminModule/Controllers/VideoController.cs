using AdminModule.Models;
using AdminModule.Pages;
using AutoMapper;
using Azure.Core;
using BLL.Models;
using BLL.Services;
using DAL.Models;
using DAL.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AdminModule.Controllers
{
    public class VideoController : Controller
    {

        private readonly ILogger<VideoController> _logger;
        private readonly IMapper _mapper;

        private readonly VideoService _videoService;
        private readonly TagService _tagService;
        private readonly GenreService _genreService;
        private readonly ImageService _imageService;
        private readonly VideoTagService _videoTagService;

        public VideoController(ILogger<VideoController> logger,IMapper mapper, VideoService videoService, TagService tagService, 
            GenreService genreService, ImageService imageService, VideoTagService videoTagService)
        {
            _mapper = mapper;
            _logger = logger;
            _videoService = videoService;
            _tagService = tagService;
            _genreService = genreService;
            _imageService = imageService;
            _videoTagService = videoTagService;
        }

        private async Task LoadViewBagData()
        {
            var genres = await _genreService.GetAllGenres();
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(genres);
            ViewBag.GenreId = new SelectList(vmGenres, "Id", "Name");

            var images = await _imageService.GetAllImages();
            var vmImages = _mapper.Map<IEnumerable<VMImage>>(images);
            ViewBag.ImageId = new SelectList(vmImages, "Id", "Content");
            ViewBag.Images = vmImages;

            var tags = await _tagService.GetAllTags();
            var vmTags = _mapper.Map<IEnumerable<VMTag>>(tags);
            ViewBag.Tags = new SelectList(vmTags, "Id", "Name");
        }

        public async Task<IActionResult> Index(string sortOrder, string searchQuery, bool clearFilter = false,
            int page = 1)
        {

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
                //Save
                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(7),
                    HttpOnly = true,
                    Secure = true
                };
                Response.Cookies.Append("SearchQueryVideo", searchQuery, cookieOptions);
            }

            var videos = await _videoService.GetAllVideos();
            var sortVideos = _mapper.Map<IEnumerable<VMVideo>>(videos);

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

            const int pageSize = 5;
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

        public async Task<ActionResult> Create()
        {
            var genres = await _genreService.GetAllGenres();
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(genres);
            await LoadViewBagData();
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VMVideo video)
        {
            try
            {
                var allTags = await _tagService.GetAllTags();
                var allVideos = await _videoService.GetAllVideos();

                string[] tags = video.NewTags.Split(',');
                var videoTags = tags.ToList();

                var addTags = new List<BLVideoTag>();
                foreach (var tag in videoTags)
                {
                    var tagAlreadyExists = allTags.FirstOrDefault(t => t.Name == tag);

                    if (tagAlreadyExists == null)
                    {
                        //Ako ne postoji tag, dodaj ga
                        var newTag = new BLTag
                        {
                            Name = tag
                        };
                        await _tagService.AddTag(newTag);
                        addTags.Add(new BLVideoTag { Tag = newTag }); // kreiraj tag i dodaj ga u listu
                    }
                    else
                    {
                        addTags.Add(new BLVideoTag { TagId = tagAlreadyExists.Id }); // kreiraj relaciju i dodaj u listu
                    }
                }

                await _videoService.AddVideo(
                         new BLL.Models.BLVideo
                         {
                             Id = video.Id,
                             CreatedAt = video.CreatedAt,
                             Name = video.Name,
                             Description = video.Description,
                             GenreId = video.GenreId,
                             TotalSeconds = video.TotalSeconds,
                             StreamingUrl = video.StreamingUrl,
                             ImageId = video.ImageId,
                             VideoTags = addTags
                         }
                     );

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //GET
        public async Task<ActionResult> Edit(int id)
        {
            var video = await _videoService.GetVideoById(id);
            var vmVideo = _mapper.Map<VMVideo>(video);

            await LoadViewBagData();
            return View(vmVideo);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VMVideo video)
        {
            try
            {

                var dbVideo = await _videoService.GetVideoById(id);

                dbVideo.Id = video.Id;
                dbVideo.Name = video.Name;
                dbVideo.Description = video.Description;
                dbVideo.TotalSeconds = video.TotalSeconds;
                dbVideo.StreamingUrl = video.StreamingUrl;
                dbVideo.GenreId = video.GenreId;
                dbVideo.ImageId = video.ImageId;

               //// Update VideoTags
               //var allTags = await _tagService.GetAllTags();
               //var existingTags = dbVideo.VideoTags.Select(vt => vt.Tag.Name).ToList();
               //
               //var newTags = video.NewTags.Split(',').ToList();
               //
               //var tagsToAdd = newTags
               //    .Where(newTag => !existingTags.Contains(newTag))
               //    .ToList();
               //
               //// Remove old tags
               //foreach (var videoTag in dbVideo.VideoTags.ToList())
               //{
               //    await _videoTagService.DeleteVideoTag(videoTag.Id);
               //}
               //dbVideo.VideoTags.Clear();
               //
               //// Add new tags
               //foreach (var newTag in tagsToAdd)
               //{
               //    if (!string.IsNullOrWhiteSpace(newTag))
               //    {
               //        var tag = allTags.FirstOrDefault(t => t.Name == newTag);
               //        if (tag == null)
               //        {
               //            // Create a new tag if it doesn't exist
               //            tag = new BLTag
               //            {
               //                Name = newTag
               //            };
               //            await _tagService.AddTag(tag);
               //        }
               //        dbVideo.VideoTags.Add(new BLVideoTag { Tag = tag });
               //    }
               //}

                await _videoService.UpdateVideo(dbVideo);

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
            var video = await _videoService.GetVideoById(id);
            var vmVideo = _mapper.Map<VMVideo>(video);

            return View(vmVideo);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                var dbVideo = await _videoService.GetVideoById(id);
                if (dbVideo == null)
                    return NotFound();

                string deletedTags = string.Join(",", dbVideo.VideoTags
                                        .Select(vt => vt.Tag)
                                        .Where(tag => tag != null)
                                        .Select(tag => tag!.Name));

                foreach (var videoTag in dbVideo.VideoTags.ToList())
                {
                    await _videoTagService.DeleteVideoTag(videoTag.Id);
                }

                await _videoService.DeleteVideo(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }
}
