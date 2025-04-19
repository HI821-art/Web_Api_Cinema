using System.ComponentModel.DataAnnotations;

namespace Web_Api_Cinema.Entities
{
    public class Director
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Biography { get; set; }

        public ICollection<Movie>? Movies { get; set; } = new List<Movie>();
    }
}

