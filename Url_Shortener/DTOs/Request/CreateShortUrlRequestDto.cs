using System.ComponentModel.DataAnnotations;

namespace Url_Shortener.DTOs.Request
{
    public class CreateShortUrlRequestDto
    {
        [Required]
        [Url]
        [MaxLength(2048)]
        public string OriginalUrl { get; set; } = string.Empty;
    }
}
