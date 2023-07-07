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
    public class VideoService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VideoService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BLVideo>> GetAllVideosLazy()
        {
            var dbVideos = await _unitOfWork.VideoRepository.GetAsync();
            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);
            return blVideos;
        }


        public async Task<IEnumerable<BLVideo>> GetAllVideos()
        {
            var dbVideos = await _unitOfWork.VideoRepository.GetAsync(includeProperties: "Genre,Image,VideoTags,VideoTags.Tag");
            var blVideos = _mapper.Map<IEnumerable<BLVideo>>(dbVideos);
            return blVideos;
        }

        public async Task<BLVideo> GetVideoById(int id)
        {
            var dbVideos = await _unitOfWork.VideoRepository.GetAsync(includeProperties: "Genre,Image,VideoTags,VideoTags.Tag");
            var dbVideo = dbVideos.FirstOrDefault(v => v.Id == id);

            var blVideo = _mapper.Map<Models.BLVideo>(dbVideo);
            return blVideo;
        }

        public async Task AddVideo(BLVideo blVideo)
        {
            var video = _mapper.Map<Video>(blVideo);
            await _unitOfWork.VideoRepository.InsertAsync(video);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateVideo(BLVideo blVideo)
        {
            var existingVideo = await _unitOfWork.VideoRepository.GetByIDAsync(blVideo.Id);

            if (existingVideo == null)
            {
                return;
            }

            existingVideo.Name = blVideo.Name;
            existingVideo.Description = blVideo.Description;
            existingVideo.GenreId = blVideo.GenreId;
            existingVideo.TotalSeconds = blVideo.TotalSeconds;
            existingVideo.StreamingUrl = blVideo.StreamingUrl;
            existingVideo.ImageId = blVideo.ImageId;

            await _unitOfWork.VideoRepository.UpdateAsync(existingVideo);

            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateVideoTags(BLVideo blVideo, List<int> newTagIds, List<int> removedTagIds)
        {
            var existingVideo = await _unitOfWork.VideoRepository.GetByIDAsync(blVideo.Id);

            if (existingVideo == null)
            {
                return;
            }

            existingVideo.Name = blVideo.Name;
            existingVideo.Description = blVideo.Description;
            existingVideo.GenreId = blVideo.GenreId;
            existingVideo.TotalSeconds = blVideo.TotalSeconds;
            existingVideo.StreamingUrl = blVideo.StreamingUrl;
            existingVideo.ImageId = blVideo.ImageId;

            // Update tags
            foreach (var tagId in removedTagIds)
            {
                var videoTag = existingVideo.VideoTags.FirstOrDefault(vt => vt.TagId == tagId);
                if (videoTag != null)
                {
                    existingVideo.VideoTags.Remove(videoTag);
                }
            }

            foreach (var tagId in newTagIds)
            {
                if (!existingVideo.VideoTags.Any(vt => vt.TagId == tagId))
                {
                    existingVideo.VideoTags.Add(new VideoTag
                    {
                        VideoId = existingVideo.Id,
                        TagId = tagId
                    });
                }
            }

            await _unitOfWork.VideoRepository.UpdateAsync(existingVideo);

            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteVideo(int id)
        {
            await _unitOfWork.VideoRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task SaveVideoData() => await _unitOfWork.SaveAsync();

    }
}
