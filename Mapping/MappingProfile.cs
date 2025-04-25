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
        }

    }
}
