using AutoMapper;
using LibraryWeb.Models;
using LibraryWeb.Models.DTO;

namespace LibraryWeb
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
       
            CreateMap<BookDTO, BookCreateDTO>().ReverseMap();
            CreateMap<BookDTO,BookUpdateDTO>().ReverseMap();
            CreateMap<CategoryDTO,CategoryCreateDTO>().ReverseMap();
            CreateMap<CategoryDTO,CategoryUpdateDTO>().ReverseMap();
            CreateMap<LocationDTO, LocationCreateDTO>().ReverseMap();
            CreateMap<LocationDTO, LocationUpdateDTO>().ReverseMap();
            CreateMap<EventDTO, EventCreateDTO>().ReverseMap();
            CreateMap<EventDTO, EventUpdateDTO>().ReverseMap();
        }
    }
}
