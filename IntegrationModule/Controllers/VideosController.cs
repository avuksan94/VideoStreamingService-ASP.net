using AutoMapper;
using Azure.Core;
using BLL.Models;
using BLL.Services;
using DAL.Models;
using DAL.Requests;
using DAL.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Text.Json;
using DAL.Repo;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    //Controller is protected the Token is only valid for 10 minutes
    public class VideosController : ControllerBase
    {
        private readonly VideoService _videoService;
        private readonly TagService _tagService;
        private readonly VideoTagService _videoTagService;
        private readonly GenreService _genreService;
        private readonly ImageService _imageService;



        public VideosController(VideoService videoService, TagService tagService, VideoTagService videoTagService, ImageService imageService, GenreService genreService)
        {
            _videoService = videoService;
            _tagService = tagService;
            _videoTagService = videoTagService;
            _imageService = imageService;
            _genreService = genreService;
        }

        // GET: api/<VideoController>
        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<VideoResponse>>> GetVideos()
        {
            try
            {
                var allVideos = await _videoService.GetAllVideos();
                var allTags = await _tagService.GetAllTags();

                return Ok(allVideos.Select(dbVideo =>
                    new VideoResponse
                    {
                        Name = dbVideo.Name,
                        Description = dbVideo.Description,
                        Image = dbVideo.Image.Content,
                        TotalTime = dbVideo.TotalSeconds,
                        StreamingUrl = dbVideo.StreamingUrl,
                        Genre = dbVideo.Genre.Name,
                        Tags = string.Join(",", dbVideo.VideoTags
                                    .Select(vt => vt.Tag)
                                    .Where(tag => tag != null)
                                    .Select(tag => tag!.Name))
                    }
                )); 
                
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        // GET api/<VideoController>/5
        [HttpGet("[action]")]
        public async Task<ActionResult<VideoResponse>> GetVideo(int id)
        {
            try
            {
                var allVideos = await _videoService.GetAllVideos();
                var dbVideo = allVideos.FirstOrDefault(v => v.Id == id);
                var allTags = _tagService.GetAllTags();
                if (dbVideo == null)
                    return NotFound();

                return Ok(new VideoResponse
                {
                    Name = dbVideo.Name,
                    Description = dbVideo.Description,
                    Image = dbVideo.Image.Content,
                    TotalTime = dbVideo.TotalSeconds,
                    StreamingUrl = dbVideo.StreamingUrl,
                    Genre = dbVideo.Genre.Name,
                    Tags = string.Join(",", dbVideo.VideoTags
                                    .Select(vt => vt.Tag)
                                    .Where(tag => tag != null)
                                    .Select(tag => tag!.Name))
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        // POST api/<VideoController>
        [HttpPost("[action]")]
        public async Task<ActionResult<VideoResponse>> AddVideo(VideoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                string[] tags = request.Tags.Split(',');
                var alldbTags = await _tagService.GetAllTags();
                var dbTags = alldbTags.Where(x => tags.Contains(x.Name));

                var dbVideo = new BLVideo
                {
                    Name = request.Name,
                    Description = request.Description,
                    ImageId = request.ImageId,
                    TotalSeconds = request.TotalTime,
                    StreamingUrl = request.StreamingUrl,
                    GenreId = request.GenreId,
                    VideoTags = dbTags.Select(x => new BLVideoTag { Tag = x }).ToList()
                };

                foreach (var tag in tags)
                {
                    if (!dbTags.Any(t => t.Name == tag))
                    {
                        var newTag = new BLTag { Name = tag };
                        dbVideo.VideoTags.Add(new BLVideoTag { Tag = newTag });
                    }
                }

                await _videoService.AddVideo(dbVideo);

                var responseVideo = new VideoResponse
                {
                    Name = dbVideo.Name,
                    Description = dbVideo.Description,
                    Image = dbVideo.ImageId.ToString(),
                    TotalTime = dbVideo.TotalSeconds,
                    StreamingUrl = dbVideo.StreamingUrl,
                    Genre = dbVideo.GenreId.ToString(),
                    Tags = string.Join(",", dbVideo.VideoTags
                                        .Select(vt => vt.Tag)
                                        .Where(tag => tag != null)
                                        .Select(tag => tag.Name))
                };

                return Ok(responseVideo);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while processing your request.");
            }
        }

        //i let this unsecured so its easier to test,without postman
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult<List<BLVideo>>> Search(int page, int size,string searchName,string sortBy, string orderingDirection)
        {
            try
            {
                var videos = from s in await _videoService.GetAllVideosLazy() select s;

                //Ordering(default asc)
                switch (sortBy)
                {
                    case "id":
                        videos = videos.OrderBy(v => v.Id);
                        break;
                    case "name":
                        videos = videos.OrderBy(v => v.Name);
                        break;
                    case "total time":
                        videos = videos.OrderBy(v => v.TotalSeconds);
                        break;
                    default:
                        videos = videos.OrderBy(v => v.Id);
                        break;
                }

                //Order by
                if (string.Compare(orderingDirection, "desc", true) == 0)
                {
                    videos = videos.Reverse();
                }

                // Filtering
                if (!searchName.IsNullOrEmpty())
                    videos = videos.Where(v => v.Name.Contains(searchName));

                // Paging
                videos = videos.Skip((page - 1) * size).Take(size);

                return Ok(videos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        // PUT api/<VideoController>/5
        //Currently working, for updating tags,trying to solve issue
        [HttpPut("[action]")]
        public async Task<ActionResult<VideoResponse>> Modify(int id, [FromBody] VideoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbVideos = await _videoService.GetAllVideos();
                var dbVideo = dbVideos.FirstOrDefault(v => v.Id == id);
                if (dbVideo == null)
                    return NotFound($"Could not find video with id {id}");

                dbVideo.Name = request.Name;
                dbVideo.Description = request.Description;
                dbVideo.GenreId = request.GenreId;
                dbVideo.TotalSeconds = request.TotalTime;
                dbVideo.StreamingUrl = request.StreamingUrl;
                dbVideo.ImageId = request.ImageId;

                var requestTags = request.Tags.Split(',');

                // (1) Remove unused tags
                var toRemove = dbVideo.VideoTags.Where(vt => !requestTags.Contains(vt.Tag.Name));
                foreach (var vt in toRemove)
                {
                    await _videoTagService.DeleteVideoTag(vt.Id);
                }

                // (2) Add new tags
                var existingDbTagNames = dbVideo.VideoTags.Select(vt => vt.Tag.Name);
                var newTagNames = requestTags.Except(existingDbTagNames);
                foreach (var newTagName in newTagNames)
                {
                    var dbTags = await _tagService.GetAllTags();
                    var dbTag = dbTags.FirstOrDefault(t => newTagName == t.Name);
                    if (dbTag == null)
                        continue;

                    dbVideo.VideoTags.Add(new BLVideoTag
                    {
                        Video = dbVideo,
                        Tag = dbTag
                    });
                    await _tagService.SaveTagData();
                }

                await _videoService.UpdateVideo(dbVideo);

                var gettingGenre = await _genreService.GetGenreById(dbVideo.GenreId);
                var gettingImage = await _imageService.GetImageById(dbVideo.ImageId);

                return Ok(new VideoResponse
                {
                    Name = dbVideo.Name,
                    Description = dbVideo.Description,
                    Image = gettingImage.Content,
                    TotalTime = dbVideo.TotalSeconds,
                    StreamingUrl = dbVideo.StreamingUrl,
                    Genre = gettingGenre.Name,
                    Tags = string.Join(",", dbVideo.VideoTags
                                   .Select(vt => vt.Tag)
                                   .Where(tag => tag != null)
                                   .Select(tag => tag!.Name))
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while processing your request");
            }
        }


        // DELETE api/<VideoController>/5
        [HttpDelete("[action]")]
        public async Task<ActionResult<VideoResponse>> RemoveVideo(int id)
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

                return Ok(new VideoResponse
                {
                    Name = dbVideo.Name,
                    Description = dbVideo.Description,
                    Image = dbVideo.Image.Content,
                    TotalTime = dbVideo.TotalSeconds,
                    StreamingUrl = dbVideo.StreamingUrl,
                    Genre = dbVideo.Genre.Name,
                    Tags = deletedTags
                }) ;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
