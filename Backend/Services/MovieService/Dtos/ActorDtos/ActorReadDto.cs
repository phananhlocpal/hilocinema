using MovieService.Models;

namespace MovieService.Dtos.ActorDtos
{
    public class ActorReadDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public byte[]? Img { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}
