using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Url_Shortener.DTOs.Request;
using Url_Shortener.DTOs.Response;
using Url_Shortener.Services.Interfaces;

namespace Url_Shortener.Controllers
{
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IShortUrlService _service;
        private readonly ILogger<UrlController> _logger;

        public UrlController(IShortUrlService service, ILogger<UrlController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [HttpPost("api/shorten")]
        [ProducesResponseType(typeof(CreateShortUrlResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateShortUrlResponseDto>> Create([FromBody] CreateShortUrlRequestDto request)
        {
            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host.Value}";
                var response = await _service.CreateAsync(request, baseUrl);
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to create short URL");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in Create");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
        }

        [HttpGet("{shortCode}")]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RedirectToOriginal(string shortCode)
        {
            try
            {
                var originalUrl = await _service.GetOriginalUrlAsync(shortCode);
                return new RedirectResult(originalUrl!, permanent: false);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Short code not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception in RedirectToOriginal for shortCode: {ShortCode}", shortCode);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
        }
    }
}
