using Url_Shortener.DTOs.Request;
using Url_Shortener.DTOs.Response;

namespace Url_Shortener.Services.Interfaces
{
    public interface IShortUrlService
    {
        Task<CreateShortUrlResponseDto> CreateAsync(CreateShortUrlRequestDto request, string baseUrl);
        Task<string?> GetOriginalUrlAsync(string shortCode);
    }
}
