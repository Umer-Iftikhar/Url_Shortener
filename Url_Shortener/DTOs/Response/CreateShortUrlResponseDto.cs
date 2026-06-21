using Url_Shortener.Models;

namespace Url_Shortener.DTOs.Response
{
    public class CreateShortUrlResponseDto
    {
        public string ShortUrl { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public string ShortCode { get; set; } = string.Empty;
    }
}
