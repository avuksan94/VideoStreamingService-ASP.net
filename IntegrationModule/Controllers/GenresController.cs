using BLL.Models;
using BLL.Services;
using DAL.Models;
using DAL.Requests;
using DAL.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenresController(GenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<GenreResponse>>> GetGenres()
        {
            try
            {
                var allGenres = await _genreService.GetAllGenres();

                return Ok(allGenres.Select(dbGenre =>
                    new GenreResponse
                    {
                        Id = dbGenre.Id,
                        Name = dbGenre.Name,
                        Description = dbGenre.Description
                    }
                ));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        
        [HttpGet("[action]")]
        public async Task<ActionResult<GenreResponse>> GetGenre(int id)
        {
            try
            {
                var dbGenre = await _genreService.GetGenreById(id);


                if (dbGenre == null)
                    return NotFound();

                return Ok(new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name,
                    Description = dbGenre.Description
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


        [HttpPost("[action]")]
        public async Task<ActionResult<GenreResponse>> AddGenre(GenreRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var allGenres = await _genreService.GetAllGenres();
                var checkIfGenreExists = allGenres.FirstOrDefault(g => g.Name == request.Name);

                if (checkIfGenreExists == null)
                {
                    var dbGenre = new BLGenre
                    {
                        Name = request.Name,
                        Description = request.Description
                    };

                    await _genreService.AddGenre(dbGenre);

                    var newlyAddedGenre = await _genreService.GetGenreByName(request.Name);

                    return Ok(new GenreResponse
                    {
                        Id = newlyAddedGenre.Id,
                        Name = dbGenre.Name,
                        Description = dbGenre.Description,

                    });
                }
                else return Conflict("The Genre you are trying to add already exists");

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("[action]")]
        public async Task <ActionResult<GenreResponse>> ModifyGenre(int id, [FromBody] GenreRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbGenre = await _genreService.GetGenreById(id);

                if (dbGenre == null)
                    return NotFound();

                dbGenre.Name = request.Name;
                dbGenre.Description = request.Description;


                await _genreService.UpdateGenre(dbGenre);

                return Ok(new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name,
                    Description = dbGenre.Description
                    
                });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<GenreResponse>> RemoveGenre(int id)
        {
            try
            {
                var dbGenre = await _genreService.GetGenreById(id);
                if (dbGenre == null)
                    return NotFound();

                await _genreService.DeleteGenre(id);

                return Ok(new GenreResponse
                {
                    Id = dbGenre.Id,
                    Name = dbGenre.Name,
                    Description = dbGenre.Description

                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
