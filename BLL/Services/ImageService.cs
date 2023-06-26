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
    public class ImageService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ImageService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BLImage>> GetAllImages()
        {
            var dbImages =  await Task.Run(() => _unitOfWork.ImageRepository.GetAsync());
            var blImages = _mapper.Map<IEnumerable<BLImage>>(dbImages);
            return blImages;
        }

        public async Task<BLImage> GetImageById(int id)
        {
            var dbImage =  await Task.Run(() => _unitOfWork.ImageRepository.GetByIDAsync(id));
            var blImage = _mapper.Map<Models.BLImage>(dbImage);
            return blImage;
        }

        public async Task AddImage(BLImage blImage)
        {
            var image = _mapper.Map<Image>(blImage);
            await _unitOfWork.ImageRepository.InsertAsync(image);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateImage(BLImage blImage)
        {
            var existingImage = await _unitOfWork.ImageRepository.GetByIDAsync(blImage.Id);

            if (existingImage == null)
            {
                return;
            }

            _mapper.Map(blImage, existingImage);

            await _unitOfWork.ImageRepository.UpdateAsync(existingImage);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteImage(int id)
        {
            await _unitOfWork.ImageRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public void SaveGenreData() => _unitOfWork.Save();
    }
}
