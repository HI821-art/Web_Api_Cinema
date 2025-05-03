using Web_Api_Cinema.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;

public class SeatService : ISeatService
{
    private readonly MovieDbContext _context;
    private readonly IEmailSender _emailSender;

    public SeatService(MovieDbContext context, IEmailSender emailSender)
    {
        _context = context;
        _emailSender = emailSender;
    }

    public bool ReserveSeat(int sessionId, int seatNumber, string userId)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.SessionId == sessionId && s.SeatNumber == seatNumber.ToString());
        if (seat == null || seat.IsBooked) return false;

        seat.IsBooked = true;
        seat.UserId = userId;
        _context.SaveChanges();

        // Send confirmation email
        var session = _context.Sessions.Include(s => s.Movie).FirstOrDefault(s => s.Id == sessionId);
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (session != null && user != null)
        {
            var subject = "Reservation Confirmation";
            var message = $@"
                <h1>Reservation Confirmed</h1>
                <p>Dear {user.UserName},</p>
                <p>Your seat <strong>{seat.SeatNumber}</strong> for the movie <strong>{session.Movie.Title}</strong> on <strong>{session.StartTime}</strong> has been successfully reserved.</p>
                <p>Thank you for choosing our cinema!</p>";

            _emailSender.SendEmailAsync(user.Email, subject, message).Wait();
        }

        return true;
    }

    public IEnumerable<Seat> GetAvailableSeats(int sessionId)
    {
        return _context.Seats.Where(s => s.SessionId == sessionId && !s.IsBooked).ToList();
    }

    public int GetAvailableCount(int sessionId)
    {
        return _context.Seats.Count(s => s.SessionId == sessionId && !s.IsBooked);
    }

    public Session? GetSessionWithSeats(int sessionId)
    {
        return _context.Sessions
            .Include(s => s.Seats)
            .Include(s => s.Movie)
            .FirstOrDefault(s => s.Id == sessionId);
    }

    public IEnumerable<(Movie Movie, IEnumerable<Seat> Seats)> GetReservedSeatsByUser(string userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User must be logged in to view reserved seats.");
        }

        return _context.Seats
            .Where(s => s.UserId == userId)
            .GroupBy(s => s.Session.Movie)
            .ToList()
            .Select(g => (g.Key!, g.AsEnumerable()));
    }
}
