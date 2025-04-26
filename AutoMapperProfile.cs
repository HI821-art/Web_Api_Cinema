using AutoMapper;
using Web_Api_Cinema.Dtos;
using Web_Api_Cinema.DTOs;
using Web_Api_Cinema.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateMovieDto, Movie>();
        CreateMap<EditMovieDto, Movie>().ReverseMap();
        CreateMap<UpdateMovieDto, Movie>().ReverseMap();
    }
}
