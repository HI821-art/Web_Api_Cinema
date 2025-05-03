using System.Security.Claims;
using Web_Api_Cinema.Entities;
using Web_Api_Cinema.Interfaces;
using Microsoft.EntityFrameworkCore;
using Web_Api_Cinema.Data;

namespace Web_Api_Cinema.Services
{
    public class FavoritesServiceDb : IFavoriteService
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly MovieDbContext _context;

        private string? UserId => _accessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        private bool IsAuthenticated => UserId != null;

        public FavoritesServiceDb(IHttpContextAccessor accessor, MovieDbContext context)
        {
            _accessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private User? GetUserWithFavoriteItems()
        {
            return _context.Users
                .Include(u => u.FavoriteItems)
                .ThenInclude(fi => fi.Movie)
                .SingleOrDefault(u => u.Id == UserId);
        }

        public List<int> GetIds()
        {
            if (!IsAuthenticated) return new List<int>();

            var user = GetUserWithFavoriteItems();
            return user?.FavoriteItems.Select(fi => fi.MovieId).ToList() ?? new List<int>();
        }

        public List<Movie> GetAll()
        {
            if (!IsAuthenticated) return new List<Movie>();

            var user = GetUserWithFavoriteItems();
            return user?.FavoriteItems.Select(fi => fi.Movie!).ToList() ?? new List<Movie>();
        }

        public void Add(int movieId)
        {
            if (!IsAuthenticated) return;

            if (_context.FavoriteItems.Any(fi => fi.UserId == UserId && fi.MovieId == movieId))
                return;

            var favoriteItem = new FavoriteItem
            {
                MovieId = movieId,
                UserId = UserId!
            };

            _context.FavoriteItems.Add(favoriteItem);
            _context.SaveChanges();
        }

        public void Remove(int movieId)
        {
            if (!IsAuthenticated) return;

            var favoriteItem = _context.FavoriteItems
                .FirstOrDefault(fi => fi.UserId == UserId && fi.MovieId == movieId);

            if (favoriteItem == null) return;

            _context.FavoriteItems.Remove(favoriteItem);
            _context.SaveChanges();
        }

        public void Clear()
        {
            if (!IsAuthenticated) return;

            var user = GetUserWithFavoriteItems();
            if (user == null) return;

            _context.FavoriteItems.RemoveRange(user.FavoriteItems);
            _context.SaveChanges();
        }

        public int GetCount()
        {
            if (UserId != null) 
            {
                return _context.FavoriteItems.Count(fi => fi.UserId == UserId);
            }

            var sessionFavorites = GetIds();
            return sessionFavorites.Count;
        }

    }
}
