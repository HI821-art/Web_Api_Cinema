using Microsoft.AspNetCore.Identity;

public class User : IdentityUser
{
    public DateTime Birthdate { get; set; }
    public string Address { get; set; }
    public string Gender { get; set; }


    public ICollection<FavoriteItem> FavoriteItems { get; set; } = new List<FavoriteItem>();
}
