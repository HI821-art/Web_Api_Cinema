using Web_Api_Cinema.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web_Api_Cinema.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Range(1888, int.MaxValue, ErrorMessage = "Year cannot be negative or less than 1888")]
        public int Year { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Genre { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be greater than 0")]
        public int Duration { get; set; }
        [Required]
        public string CoverImage { get; set; }
        [Required]
        public string Country { get; set; }
        [Url(ErrorMessage = "Invalid URL format")]
        public string TrailerUrl { get; set; }
        public ICollection<Actor>? Actors { get; set; } = new List<Actor>();
        [Required]
        public int DirectorId { get; set; }
        public Director? Director { get; set; }
        public ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();
    }
}
