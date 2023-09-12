using WebFlay.Lib.Models;
using WebFlay.Lib.Shared.Enums;

namespace WebFlay.Lib.Services;

public interface IScrapeService
{
    Task<ScrapeResult?> Scrape(string name, string url, SearchEngine searchEngine);

    Task<(IEnumerable<ScrapeResult> results, int totalResutls)> GetHistory(int index = 0,
        int pageSize = 100);

    Task<(IEnumerable<ScrapeResult> results, int totalResutls)> GetHistoryByFilter(
        int index = 0,
        int pageSize = 100,
        string? name = null,
        string? url = null,
        SearchEngine? searchEngine = null
    );
}