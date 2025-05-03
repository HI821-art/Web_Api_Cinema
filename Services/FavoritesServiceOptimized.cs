using Web_Api_Cinema.Entities;
using Web_Api_Cinema.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class FavoritesServiceOptimized : IFavoriteService
{
    private readonly MovieDbContext _context;
    private readonly IHttpContextAccessor _accessor;
    private readonly string? _userId;

    public FavoritesServiceOptimized(IHttpContextAccessor accessor, MovieDbContext context)
    {
        _context = context;
        _accessor = accessor;
        _userId = accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public List<int> GetIds()
    {
        if (_userId != null) 
        {
            return _context.FavoriteItems
                .Where(fi => fi.UserId == _userId)
                .Select(fi => fi.MovieId)
                .ToList();
        }

      
        var sessionFavorites = _accessor.HttpContext?.Session.GetString("FavoriteMovies");
        return sessionFavorites != null
            ? System.Text.Json.JsonSerializer.Deserialize<List<int>>(sessionFavorites)
            : new List<int>();
    }

    public List<Movie> GetAll()
    {
        if (_userId != null)
        {
            return _context.FavoriteItems
                .Include(fi => fi.Movie)
                .Where(fi => fi.UserId == _userId)
                .Select(fi => fi.Movie)
                .ToList();
        }

        // For unauthenticated users, retrieve from session
        var movieIds = GetIds();
        return _context.Movies
            .Where(m => movieIds.Contains(m.Id))
            .ToList();
    }

    public void Add(int movieId)
    {
        if (_userId != null) // Authenticated user
        {
            if (_context.FavoriteItems.Any(fi => fi.UserId == _userId && fi.MovieId == movieId)) return;

            _context.FavoriteItems.Add(new FavoriteItem { UserId = _userId, MovieId = movieId });
            _context.SaveChanges();
        }
        else // Unauthenticated user
        {
            var sessionFavorites = GetIds();
            if (!sessionFavorites.Contains(movieId))
            {
                sessionFavorites.Add(movieId);
                _accessor.HttpContext?.Session.SetString("FavoriteMovies", System.Text.Json.JsonSerializer.Serialize(sessionFavorites));
            }
        }
    }

    public void Remove(int movieId)
    {
        if (_userId != null) // Authenticated user
        {
            var favorite = _context.FavoriteItems.FirstOrDefault(fi => fi.UserId == _userId && fi.MovieId == movieId);
            if (favorite == null) return;

            _context.FavoriteItems.Remove(favorite);
            _context.SaveChanges();
        }
        else // Unauthenticated user
        {
            var sessionFavorites = GetIds();
            if (sessionFavorites.Contains(movieId))
            {
                sessionFavorites.Remove(movieId);
                _accessor.HttpContext?.Session.SetString("FavoriteMovies", System.Text.Json.JsonSerializer.Serialize(sessionFavorites));
            }
        }
        }

    public int GetCount()
    {
        if (_userId != null) 
        {
            return _context.FavoriteItems.Count(fi => fi.UserId == _userId);
        }

       
        return GetIds().Count;
    }




    public void Clear()
    {
        if (_userId != null)
        {
            var favorites = _context.FavoriteItems.Where(fi => fi.UserId == _userId);
            _context.FavoriteItems.RemoveRange(favorites);
            _context.SaveChanges();
        }
        else
        {
             _accessor.HttpContext?.Session.Remove("FavoriteMovies");
        }
    }
}
