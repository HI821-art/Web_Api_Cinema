using System.Collections.Generic;
using Web_Api_Cinema.Entities;

namespace Web_Api_Cinema.Interfaces
{
    public interface IFavoriteService
    {
        List<int> GetIds();
        List<Movie> GetAll();
        void Add(int id);
        void Remove(int id);
        void Clear();
        int GetCount();
    }
}
