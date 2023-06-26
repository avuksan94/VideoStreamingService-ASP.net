using AutoMapper;
using PublicModule.Models;

namespace PublicModule.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<BLL.Models.BLVideo, VMPublicVideo>()
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
            .ForMember(dest => dest.VideoTags, opt => opt.MapFrom(src => src.VideoTags))
            .ForMember(dest => dest.NewTags, opt => opt.Ignore());

            CreateMap<VMPublicVideo, BLL.Models.BLVideo>()
              .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
             .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre))
             .ForMember(dest => dest.VideoTags, opt => opt.MapFrom(src => src.VideoTags));

            CreateMap<BLL.Models.BLTag, VMPublicTag>();
            CreateMap<VMPublicTag, BLL.Models.BLTag>();

            CreateMap<BLL.Models.BLVideoTag, VMPublicVideoTag>();
            CreateMap<VMPublicVideoTag, BLL.Models.BLVideoTag>();

            CreateMap<BLL.Models.BLCountry, VMPublicCountry>();
            CreateMap<VMPublicCountry, BLL.Models.BLCountry>();

            CreateMap<BLL.Models.BLGenre, VMPublicGenre>();
            CreateMap<VMPublicGenre, BLL.Models.BLGenre>();

            CreateMap<BLL.Models.BLImage, VMPublicImage>();
            CreateMap<VMPublicImage, BLL.Models.BLImage>();

            CreateMap<BLL.Models.BLUser, VMPublicUser>()
            .ForMember(dest => dest.CountryOfResidenceId, opt => opt.MapFrom(src => src.CountryOfResidence.Id))
            .ForMember(dest => dest.CountryOfResidence, opt => opt.MapFrom(src => src.CountryOfResidence));
            CreateMap<VMPublicUser, BLL.Models.BLUser>()
             .ForMember(dest => dest.CountryOfResidenceId, opt => opt.MapFrom(src => src.CountryOfResidence.Id))
             .ForMember(dest => dest.CountryOfResidence, opt => opt.MapFrom(src => src.CountryOfResidence));

            CreateMap<BLL.Models.BLNotification, VMPublicNotification>();
            CreateMap<VMPublicNotification, BLL.Models.BLNotification>();
        }
    }
}
