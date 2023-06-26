using BLL.Models;
using BLL.Services;
using DAL.Models;
using DAL.Requests;
using DAL.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationModule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly TagService _tagService;

        public TagsController(TagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<TagResponse>>> GetTags()
        {
            try
            {
                var allTags = await _tagService.GetAllTags();

                return Ok(allTags.Select(dbTags =>
                    new TagResponse
                    {
                        Id = dbTags.Id,
                        Name = dbTags.Name
                    }
                ));

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<TagResponse>> GetTag(int id)
        {
            try
            {
                var dbTag = await _tagService.GetTagById(id);

                if (dbTag == null)
                    return NotFound();

                return Ok(new TagResponse
                {
                    Id = dbTag.Id,
                    Name = dbTag.Name
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<TagResponse>> AddTag(TagRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var allTags = await _tagService.GetAllTags();
                var checkIfTagExists = allTags.FirstOrDefault(g => g.Name == request.Name);

                if (checkIfTagExists == null)
                {
                    var dbTag = new BLTag
                    {
                        Name = request.Name
                    };

                    await _tagService.AddTag(dbTag);

                    var newlyAddedTag = await _tagService.GetTagByName(request.Name);

                    return Ok(new TagResponse
                    {
                        Id = newlyAddedTag.Id,
                        Name = dbTag.Name
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
        public async Task<ActionResult<TagResponse>> ModifyTag(int id, [FromBody] TagRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbTag = await _tagService.GetTagById(id);

                if (dbTag == null)
                    return NotFound();

                dbTag.Name = request.Name;

                await _tagService.UpdateTag(dbTag);

                return Ok(new TagResponse
                {
                    Id = dbTag.Id,
                    Name = dbTag.Name
                });

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<TagResponse>> RemoveTag(int id)
        {
            try
            {
                var dbTag = await _tagService.GetTagById(id);
                if (dbTag == null)
                    return NotFound();

                await _tagService.DeleteTag(id);

                return Ok(new TagResponse
                {
                    Id = dbTag.Id,
                    Name = dbTag.Name
                });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
