using Microsoft.AspNetCore.Mvc;
using WebFlay.Lib.Services;
using WebFlay.Lib.Shared.Enums;

namespace WebFlay.API.Web.Controllers;

[Route("[controller]")]
public class ScrapeController : Controller
{
    private readonly IScrapeService _scrapeService;

    public ScrapeController(IScrapeService scrapeService)
    {
        _scrapeService = scrapeService;
    }

    [HttpGet]
    public async Task<IActionResult> Scrape([FromQuery] string name, [FromQuery] string url, [FromQuery] SearchEngine searchEngine)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(url))
        {
            return BadRequest("Name and URL parameters are required.");
        }

        try
        {
            var result = await _scrapeService.Scrape(name, url, searchEngine);
            if (result is null) return BadRequest();
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}