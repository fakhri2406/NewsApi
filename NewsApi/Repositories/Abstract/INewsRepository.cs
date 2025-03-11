using NewsApi.Models;

namespace NewsApi.Repositories.Abstract;

public interface INewsRepository
{
    Task<IEnumerable<News>> GetAllAsync();
    Task<News> GetByIdAsync(int id);
    Task<News> CreateAsync(News news);
    Task UpdateAsync(News news);
    Task DeleteAsync(int id);
}