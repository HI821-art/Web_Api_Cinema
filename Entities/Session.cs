using Web_Api_Cinema.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web_Api_Cinema.Entities
{
    public class Session
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public ICollection<Seat>? Seats { get; set; } = new List<Seat>();
    }

}

