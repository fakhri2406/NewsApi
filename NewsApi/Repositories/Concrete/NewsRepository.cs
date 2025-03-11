using Microsoft.EntityFrameworkCore;
using NewsApi.Data;
using NewsApi.Models;
using NewsApi.Repositories.Abstract;

namespace NewsApi.Repositories.Concrete;

public class NewsRepository(AppDbContext _context) : INewsRepository
{
    public async Task<IEnumerable<News>> GetAllAsync()
    {
        return await _context.NewsItems.ToListAsync();
    }

    public async Task<News> GetByIdAsync(int id)
    {
        return await _context.NewsItems.FindAsync(id);
    }

    public async Task<News> CreateAsync(News news)
    {
        _context.NewsItems.Add(news);
        await _context.SaveChangesAsync();
        return news;
    }

    public async Task UpdateAsync(News news)
    {
        _context.Entry(news).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var news = await _context.NewsItems.FindAsync(id);
        if (news != null)
        {
            _context.NewsItems.Remove(news);
            await _context.SaveChangesAsync();
        }
    }
}