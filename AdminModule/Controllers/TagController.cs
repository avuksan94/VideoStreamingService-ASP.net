using AdminModule.Models;
using AutoMapper;
using Azure.Core;
using BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace AdminModule.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<TagController> _logger;
        private readonly IMapper _mapper;

        private readonly TagService _tagService;


        public TagController(ILogger<TagController> logger, IMapper mapper, TagService tagService)
        {
            _mapper = mapper;
            _logger = logger;
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetAllTags();
            var vmTags = _mapper.Map<IEnumerable<VMTag>>(tags);
            return View(vmTags);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VMTag tag)
        {
            try
            {
                var allTags = await _tagService.GetAllTags();
                var checkIfTagExists = allTags.FirstOrDefault(g => g.Name == tag.Name);

                if (checkIfTagExists == null)
                {
                    await _tagService.AddTag(
                         new BLL.Models.BLTag
                         {
                             Name = tag.Name,
                         }
                     );

                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(string.Empty, "The tag already exists!");
                return View(tag);
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            var tag = await _tagService.GetTagById(id);
            var vmTag = _mapper.Map<VMTag>(tag);

            return View(vmTag);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VMTag tag)
        {
            try
            {
                var dbTag = await _tagService.GetTagById(id);

                dbTag.Name = tag.Name;

                await _tagService.UpdateTag(dbTag);

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
            var tag = await _tagService.GetTagById(id);
            var vmTag = _mapper.Map<VMTag>(tag);

            return View(vmTag);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _tagService.DeleteTag(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
