using AutoMapper;
using MovieService.Dtos.ActorDtos;
using MovieService.Dtos.CategoryDtos;
using MovieService.Dtos.MovieDtos;
using MovieService.Dtos.ProducerDtos;
using MovieService.Models;
using System.IO;

namespace MovieService.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Movie, MovieReadDto>();

            CreateMap<MovieCreateDto, Movie>()
                .ForMember(dest => dest.ImgSmall, opt => opt.MapFrom(src => ConvertToByteArray(src.ImgSmall)))
                .ForMember(dest => dest.ImgLarge, opt => opt.MapFrom(src => ConvertToByteArray(src.ImgLarge)));

            CreateMap<Actor, ActorReadDto>();

            CreateMap<ActorCreateDto, Actor>()
                .ForMember(dest => dest.Img, opt => opt.MapFrom(src => ConvertToByteArray(src.Img)));

            CreateMap<Category, CategoryReadDto>();
            CreateMap<CategoryCreateDto, Category>();

            CreateMap<Producer, ProducerReadDto>();
            CreateMap<ProducerCreateDto, Producer>()
                .ForMember(dest => dest.Img, opt => opt.MapFrom(src => ConvertToByteArray(src.Img)));
        }

        private byte[]? ConvertToByteArray(IFormFile? formFile)
        {
            if (formFile == null || formFile.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
