﻿using MovieService.Models;

namespace MovieService.Dtos.ActorDtos
{
    public class ActorCreateDto
    {

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public IFormFile? Img { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
