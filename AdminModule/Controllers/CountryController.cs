using AdminModule.Models;
using AdminModule.Pages;
using AutoMapper;
using Azure;
using BLL.Services;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminModule.Controllers
{
    public class CountryController : Controller
    {
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        private readonly CountryService _countryService;


        public CountryController(ILogger<CountryController> logger, IMapper mapper, CountryService countryService)
        {
            _mapper = mapper;
            _logger = logger;
            _countryService = countryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var countries = await _countryService.GetAllCountries();
            var vmCountries = _mapper.Map<IEnumerable<VMCountry>>(countries);

            const int pageSize = 5;
            if (page < 1)
            {
                page = 1;
            }

            int countryCount = vmCountries.Count();
            var pager = new Pager(countryCount, page, pageSize);

            int countrySkip = (page - 1) * pageSize;

            var data = vmCountries.Skip(countrySkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;
            return View(data);
        }

        // GET:
        public ActionResult Create()
        {
            return View();
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(VMCountry country)
        {
            try
            {
                var allCountries = await _countryService.GetAllCountries();
                var checkIfCountryExists = allCountries.FirstOrDefault(c => c.Name == country.Name);

                if (checkIfCountryExists == null)
                {
                    await _countryService.AddCountry(
                         new BLL.Models.BLCountry
                         {
                             Name = country.Name,
                             Code = country.Code
                         }
                     );

                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "The country already exists!");
                return View(country);

            }
            catch
            {
                return View();
            }
        }

        // GET: 
        public async Task<ActionResult> Edit(int id)
        {
            var country = await _countryService.GetCountryById(id);
            var vmCountry = _mapper.Map<VMCountry>(country);

            return View(vmCountry);
        }

        // POST:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, VMCountry country)
        {
            try
            {
                var dbcountry = await _countryService.GetCountryById(id);

                dbcountry.Code = country.Code;
                dbcountry.Name = country.Name;

                await _countryService.UpdateCountry(dbcountry);
             
                

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
            var country = await _countryService.GetCountryById(id);
            var vmCountry = _mapper.Map<VMCountry>(country);

            return View(vmCountry);
        }

        // POST: 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                await _countryService.DeleteCountry(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
