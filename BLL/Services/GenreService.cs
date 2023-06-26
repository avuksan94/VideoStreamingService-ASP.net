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
    public class GenreService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BLGenre>> GetAllGenres()
        {
            var dbGenres = await  _unitOfWork.GenreRepository.GetAsync();
            var blGenres = _mapper.Map<IEnumerable<BLGenre>>(dbGenres);
            return blGenres;
        }

        public async Task<BLGenre> GetGenreById(int id)
        {
            var dbGenres = await _unitOfWork.GenreRepository.GetAsync();
            var dbGenre = dbGenres.FirstOrDefault(g => g.Id == id);
            var blGenre = _mapper.Map<Models.BLGenre>(dbGenre);
            return blGenre;
        }

        public async Task<BLGenre> GetGenreByName(string name)
        {
            var dbGenres = await _unitOfWork.GenreRepository.GetAsync();
            var dbGenre = dbGenres.FirstOrDefault(g => g.Name == name);
            var blGenre = _mapper.Map<Models.BLGenre>(dbGenre);
            return blGenre;
        }

        public async Task AddGenre(BLGenre blGenre)
        {
            var genre = _mapper.Map<Genre>(blGenre);
            await _unitOfWork.GenreRepository.InsertAsync(genre);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateGenre(BLGenre blGenre)
        {
            var existingGenre = await _unitOfWork.GenreRepository.GetByIDAsync(blGenre.Id);

            if (existingGenre == null)
            {
                return;
            }

            _mapper.Map(blGenre, existingGenre);

            await _unitOfWork.GenreRepository.UpdateAsync(existingGenre);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteGenre(int id)
        {
            await _unitOfWork.GenreRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task SaveGenreData() => await _unitOfWork.SaveAsync();
    }
}
