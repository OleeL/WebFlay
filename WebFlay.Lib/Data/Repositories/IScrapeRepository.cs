using WebFlay.Lib.Models;
using WebFlay.Lib.Shared.Enums;

namespace WebFlay.Lib.Data.Repositories;

public interface IScrapeRepository
{
    Task AddAsync(ScrapeResult result);
    Task<IEnumerable<ScrapeResult>> GetHistory(int index, int pageSize);
    Task<int> GetTotalCount();

    Task<IEnumerable<ScrapeResult>> GetHistoryByFilter(
        int index,
        int pageSize,
        string? name = null,
        string? url = null,
        SearchEngine? searchEngine = null);
}