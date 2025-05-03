using Web_Api_Cinema.Entities;

public interface ISeatService
{
    IEnumerable<Seat> GetAvailableSeats(int sessionId);
    bool ReserveSeat(int sessionId, int seatNumber, string userId);
    int GetAvailableCount(int sessionId);
    Session? GetSessionWithSeats(int sessionId);
    IEnumerable<(Movie Movie, IEnumerable<Seat> Seats)> GetReservedSeatsByUser(string userId); 
}
