using Url_Shortener.Models;

namespace Url_Shortener.Repositories.Interfaces
{
    public interface IShortUrlRepository
    {
        Task AddAsync(ShortUrl originaUrl);
        Task UpdateAsync(ShortUrl shortUrl);
        Task<ShortUrl?> GetByShortCodeAsync(string shortCode);
    }
}

