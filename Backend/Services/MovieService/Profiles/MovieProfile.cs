using AutoMapper;
using MovieService.Dtos.MovieDtos;
using MovieService.Models;

namespace MovieService.Profiles
{
    public class MoviesProfile : Profile
    {
        public MoviesProfile()
        {
            CreateMap<Movie, MovieReadDto>();
            CreateMap<MovieCreateDto, Movie>();
        }
    }
}
