using AutoMapper;
using BLL.Models;
using DAL.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.Models.Video, BLL.Models.BLVideo>()
             .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
             .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
             .ForMember(dest => dest.VideoTags, opt => opt.MapFrom(src => src.VideoTags));

            CreateMap<BLL.Models.BLVideo, DAL.Models.Video>()
             .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
             .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
             .ForMember(dest => dest.VideoTags, opt => opt.MapFrom(src => src.VideoTags));

            CreateMap<DAL.Models.Tag, BLL.Models.BLTag>();
            CreateMap<BLL.Models.BLTag, DAL.Models.Tag>();

            CreateMap<DAL.Models.VideoTag, BLL.Models.BLVideoTag>();
            CreateMap<BLL.Models.BLVideoTag, DAL.Models.VideoTag>();

            CreateMap<DAL.Models.Country, BLL.Models.BLCountry>();
            CreateMap<BLL.Models.BLCountry, DAL.Models.Country>();

            CreateMap<DAL.Models.Genre, BLL.Models.BLGenre>();
            CreateMap<BLL.Models.BLGenre, DAL.Models.Genre>();

            CreateMap<DAL.Models.Image, BLL.Models.BLImage>();
            CreateMap<BLL.Models.BLImage, DAL.Models.Image>();

            CreateMap<DAL.Models.User, BLL.Models.BLUser>();
            CreateMap<BLL.Models.BLUser, DAL.Models.User>();

            CreateMap<DAL.Models.Notification, BLL.Models.BLNotification>();
            CreateMap<BLL.Models.BLNotification, DAL.Models.Notification>();

            //CreateMap<BLVideo, VideoResponse>();
        }
    }
}
