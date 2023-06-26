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
    public class TagService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(UnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BLTag>> GetAllTags()
        {
            var dbTags =  await _unitOfWork.TagRepository.GetAsync();
            var blTags = _mapper.Map<IEnumerable<BLTag>>(dbTags);
            return blTags;
        }

        public async Task<BLTag> GetTagById(int id)
        {
            var dbTags =  await _unitOfWork.TagRepository.GetAsync();
            var dbTag = dbTags.FirstOrDefault(t => t.Id == id);
            var blTag = _mapper.Map<Models.BLTag>(dbTag);
            return blTag;
        }

        public async Task<BLTag> GetTagByName(string name)
        {
            var dbTags = await _unitOfWork.TagRepository.GetAsync();
            var dbTag = dbTags.FirstOrDefault(t => t.Name == name);
            var blTag = _mapper.Map<Models.BLTag>(dbTag);
            return blTag;
        }

        public async Task AddTag(BLTag blTag)
        {
            var tag = _mapper.Map<Tag>(blTag);
            await _unitOfWork.TagRepository.InsertAsync(tag);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateTag(BLTag blTag)
        {
            var existingTag = await _unitOfWork.TagRepository.GetByIDAsync(blTag.Id);

            if (existingTag == null)
            {
                return;
            }

            _mapper.Map(blTag, existingTag);

            await _unitOfWork.TagRepository.UpdateAsync(existingTag);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteTag(int id)
        {
            await _unitOfWork.TagRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task SaveTagData() => await _unitOfWork.SaveAsync();
    }
}
