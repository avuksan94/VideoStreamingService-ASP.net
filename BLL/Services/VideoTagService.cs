using AutoMapper;
using Azure;
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
    public class VideoTagService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VideoTagService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddVideoTag(BLVideoTag blVideoTag)
        {
            var videoTag = _mapper.Map<VideoTag>(blVideoTag);
            await _unitOfWork.VideoTagRepository.InsertAsync(videoTag);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddVideoTagWithoutSave(BLVideoTag blVideoTag)
        {
            var videoTag = _mapper.Map<VideoTag>(blVideoTag);
            await _unitOfWork.VideoTagRepository.InsertAsync(videoTag);
        }

        public async Task DeleteVideoTag(int id)
        {
            await _unitOfWork.VideoTagRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteVideoTagWithoutSave(BLVideoTag bLVideoTag)
        {
            await _unitOfWork.VideoTagRepository.DeleteAsync(bLVideoTag);
        }

        public async Task SaveVideoData() => await _unitOfWork.SaveAsync();

    }
}
