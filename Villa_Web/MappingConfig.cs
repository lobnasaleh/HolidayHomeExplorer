using AutoMapper;
using Villa_Web.Models;
using Villa_Web.Models.DTO;

namespace Villa_Web
{
    public class MappingConfig:Profile
    {
        public MappingConfig()
        {
            CreateMap<VillaCreateDTO,VillaDTO>().ReverseMap();
            CreateMap<VillaUpdateDTO, VillaDTO>().ReverseMap();

            CreateMap<VillaNumberCreateDTO, VillaNumberDTO>().ReverseMap();
            CreateMap<VillaNumberUpdateDTO, VillaNumberDTO>().ReverseMap();

        }
    }
}
