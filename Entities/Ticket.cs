using Web_Api_Cinema.Entities;
using System.ComponentModel.DataAnnotations;

namespace Web_Api_Cinema.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public int SeatId { get; set; }
        public Seat Seat { get; set; }

        [Required]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public DateTime PurchaseTime { get; set; }

        public string UserId { get; set; } 
        public bool IsReserved => !string.IsNullOrEmpty(UserId);
    }
}

