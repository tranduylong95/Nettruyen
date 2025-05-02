using AutoMapper;
using nettruyen.Model;
using nettruyen.Dto;
using nettruyen.Dto.Admin;

namespace nettruyen.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile() {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Comic, ComicDTO>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ComicCategories.Select(cc => cc.Category).ToList()))
                .ReverseMap();
            CreateMap<CreateComicDTO, Comic>()
          .ForMember(dest => dest.ComicCategories, opt => opt.Ignore())
          .ForMember(dest => dest.Image, opt => opt.Ignore());
            CreateMap<UpdateComicDTO, Comic>()
       .ForMember(dest => dest.ComicCategories, opt => opt.Ignore())
       .ForMember(dest => dest.Image, opt => opt.Ignore());
        }

    }
}
