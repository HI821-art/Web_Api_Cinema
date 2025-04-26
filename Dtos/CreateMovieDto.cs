using System.ComponentModel.DataAnnotations;

namespace Web_Api_Cinema.DTOs
{
    public class CreateMovieDto
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Duration { get; set; }
        public string CoverImage { get; set; }
        public string Country { get; set; }
        public string TrailerUrl { get; set; }
        public int DirectorId { get; set; }
    }


}