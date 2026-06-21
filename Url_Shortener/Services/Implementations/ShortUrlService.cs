using HashidsNet;
using Url_Shortener.DTOs.Request;
using Url_Shortener.DTOs.Response;
using Url_Shortener.Models;
using Url_Shortener.Repositories.Interfaces;
using Url_Shortener.Services.Interfaces;

namespace Url_Shortener.Services.Implementations
{
    public class ShortUrlService : IShortUrlService
    {
        private readonly IShortUrlRepository _repository;
        private readonly IHashids _hashids;
        public ShortUrlService(IShortUrlRepository repository, IHashids hashids)
        {
            _repository = repository;
            _hashids = hashids;
        }
        public async Task<CreateShortUrlResponseDto> CreateAsync(CreateShortUrlRequestDto request, string baseUrl)
        {
            var entity = new ShortUrl
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = null
            };
            await _repository.AddAsync(entity);

            var shortCode = _hashids.Encode(entity.Id);

            if (string.IsNullOrEmpty(shortCode))
            {
                throw new InvalidOperationException("Failed to generate short code.");
            }


            entity.ShortCode = shortCode;

            await _repository.UpdateAsync(entity);

            var shortUrl = $"{baseUrl.TrimEnd('/')}/{shortCode}";

            var response = new CreateShortUrlResponseDto
            {
                ShortCode = shortCode,
                ShortUrl = shortUrl,
                OriginalUrl = request.OriginalUrl,
            };

            return response;
        }
        
        public async Task<string?> GetOriginalUrlAsync(string shortCode)
        {
            if (string.IsNullOrWhiteSpace(shortCode))
            { 
                throw new ArgumentException("Short code cannot be empty.", nameof(shortCode)); 
            }
            var entity = await _repository.GetByShortCodeAsync(shortCode)
            ?? throw new KeyNotFoundException($"Short code '{shortCode}' not found.");

            return entity.OriginalUrl;
        }
    }
}
