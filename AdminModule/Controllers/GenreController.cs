using AdminModule.Models;
using AutoMapper;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminModule.Controllers
{
    public class GenreController : Controller
    {
        private readonly GenreService _genreService;
        private readonly ILogger<GenreController> _logger;
        private readonly IMapper _mapper;

        public GenreController(ILogger<GenreController> logger,IMapper mapper, GenreService genreService) 
        {
            _genreService= genreService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetGenres()
        {
            var genres = await _genreService.GetAllGenres();
            var vmGenres = _mapper.Map<IEnumerable<VMGenre>>(genres);
            return Ok(new { vmGenres });
        }

        public async Task<IActionResult> GetGenre(int id)
        {
            var genre = await _genreService.GetGenreById(id);
            var vmGenre = _mapper.Map<VMGenre>(genre);
            return Ok(new { vmGenre });
        }


        public ActionResult Create()
        {
            return View();
        }

        // POST:

        [HttpPost]
        public async Task<JsonResult> Create([FromBody] VMGenre genre)
            {
            try
            {
                await _genreService.AddGenre(
                         new BLL.Models.BLGenre
                         {
                             Name = genre.Name,
                             Description = genre.Description
                         }
                     );
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPut]
        public async Task<JsonResult> Edit(int id, [FromBody] VMGenre genre)
        {
            try
            {
                var currentGenre = await _genreService.GetGenreById(id);
                if (currentGenre == null) return Json(new { success = false });

                currentGenre.Name = genre.Name;
                currentGenre.Description = genre.Description;
                await _genreService.UpdateGenre(currentGenre);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(int id)
        {
            try
            {
                var genreExists = await _genreService.GetGenreById(id);
                if (genreExists == null) return Json(new { success = false });

                await _genreService.DeleteGenre(id);

                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }
    }
}
