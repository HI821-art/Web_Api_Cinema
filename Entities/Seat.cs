using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


    namespace Web_Api_Cinema.Entities
    {
        public class Seat
        {
            public int Id { get; set; }

            public string SeatNumber { get; set; } = string.Empty;
            public string? UserId { get; set; }

        public bool IsBooked { get; set; }

        public int? CustomerId { get; set; } // ✅
        public Customer? Customer { get; set; }


        public int SessionId { get; set; }
            public Session Session { get; set; } = null!;
        }
    }


