using AdminModule.Models;
using AutoMapper;
using System.Threading.Tasks;

namespace AdminModule.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BLL.Models.BLVideo, VMVideo>()
             .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
             .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
             .ForMember(dest => dest.VideoTags, opt => opt.MapFrom(src => src.VideoTags))
             .ForMember(dest => dest.NewTags, opt => opt.Ignore());

            CreateMap<VMVideo, BLL.Models.BLVideo>()
              .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
             .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
             .ForMember(dest => dest.VideoTags, opt => opt.MapFrom(src => src.VideoTags));

            CreateMap<BLL.Models.BLTag, VMTag>();
            CreateMap<VMTag, BLL.Models.BLTag>();

            CreateMap<BLL.Models.BLVideoTag, VMVideoTag>();
            CreateMap<VMVideoTag, BLL.Models.BLVideoTag>();

            CreateMap<BLL.Models.BLCountry, VMCountry>();
            CreateMap<VMCountry, BLL.Models.BLCountry>();

            CreateMap<BLL.Models.BLGenre, VMGenre>();
            CreateMap<VMGenre, BLL.Models.BLGenre>();

            CreateMap<BLL.Models.BLImage, VMImage>();
            CreateMap<VMImage, BLL.Models.BLImage>();

            CreateMap<BLL.Models.BLUser, VMUser>()
            .ForMember(dest => dest.CountryOfResidenceId, opt => opt.MapFrom(src => src.CountryOfResidence.Id))
            .ForMember(dest => dest.CountryOfResidence, opt => opt.MapFrom(src => src.CountryOfResidence)); 
            CreateMap<VMUser, BLL.Models.BLUser>()
             .ForMember(dest => dest.CountryOfResidenceId, opt => opt.MapFrom(src => src.CountryOfResidence.Id))
             .ForMember(dest => dest.CountryOfResidence, opt => opt.MapFrom(src => src.CountryOfResidence)); 

            CreateMap<BLL.Models.BLNotification, VMNotification>();
        }
    }
}
