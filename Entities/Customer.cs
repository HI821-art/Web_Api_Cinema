using System.ComponentModel.DataAnnotations;

namespace Web_Api_Cinema.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public ICollection<Seat>? Seats { get; set; } = new List<Seat>();
    }

}