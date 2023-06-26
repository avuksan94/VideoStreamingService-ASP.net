using AutoMapper;
using BLL.Models;
using DAL.Models;
using DAL.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class CountryService
    {
        private readonly UnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public CountryService(UnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BLCountry>> GetAllCountries()
        {
            var dbCountries = await _unitOfWork.CountryRepository.GetAsync();
            var blCountires = _mapper.Map<IEnumerable<BLCountry>>(dbCountries);
            return blCountires;
        }

        public async Task<BLCountry> GetCountryById(int id)
        {
            var dbCountries = await _unitOfWork.CountryRepository.GetAsync();
            var dbCountry = dbCountries.FirstOrDefault(c => c.Id == id);
            var blCountry = _mapper.Map<Models.BLCountry>(dbCountry);
            return blCountry;
        }

        public async Task AddCountry(BLCountry blCountry)
        {
            var country = _mapper.Map<Country>(blCountry);
            await _unitOfWork.CountryRepository.InsertAsync(country);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCountry(BLCountry blCountry)
        {
            var existingCountry = await _unitOfWork.CountryRepository.GetByIDAsync(blCountry.Id);

            if (existingCountry == null)
            {
                return;
            }

            _mapper.Map(blCountry, existingCountry);

            await _unitOfWork.CountryRepository.UpdateAsync(existingCountry);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCountry(int id)
        {
            await _unitOfWork.CountryRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public void SaveCountryData() => _unitOfWork.Save();
    }
}
