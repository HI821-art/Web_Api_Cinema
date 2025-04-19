using Web_Api_Cinema.Entities;

public class FavoriteItem
{
    public string UserId { get; set; }
    public User User { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}
