using Microsoft.EntityFrameworkCore;
using Url_Shortener.Data;
using Url_Shortener.Models;
using Url_Shortener.Repositories.Interfaces;

namespace Url_Shortener.Repositories.Implementations
{
    public class ShortUrlRepository : IShortUrlRepository
    {
        private readonly AppDbContext _context;
        public ShortUrlRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(ShortUrl shortUrl)
        {
            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ShortUrl shortUrl)
        {
            await _context.SaveChangesAsync();
        }
        public async Task<ShortUrl?> GetByShortCodeAsync(string shortCode)
        {
            return await _context.ShortUrls
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ShortCode == shortCode);
        }
    }
}
