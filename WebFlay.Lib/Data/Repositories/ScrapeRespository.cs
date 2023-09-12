using Microsoft.EntityFrameworkCore;
using WebFlay.Lib.Extensions;
using WebFlay.Lib.Models;
using WebFlay.Lib.Shared.Enums;

namespace WebFlay.Lib.Data.Repositories
{
    public class ScrapeRepository : IScrapeRepository
    {
        private readonly WebFlayDbContext _db;

        public ScrapeRepository(WebFlayDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task AddAsync(ScrapeResult result)
        {
            var dbModel = result.ToScrapeResultDbModel();
            await _db.ScrapeHistory.AddAsync(dbModel);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ScrapeResult>> GetHistory(int index, int pageSize)
        {
            var dbResults = await _db.ScrapeHistory
                .Skip(index * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map ScrapeResultDbModel to ScrapeResult
            return dbResults.Select(r => r.ToScrapeResult());
        }

        public async Task<int> GetTotalCount()
        {
            return await _db.ScrapeHistory.CountAsync();
        }

        public async Task<IEnumerable<ScrapeResult>> GetHistoryByFilter(
            int index,
            int pageSize,
            string? name = null,
            string? url = null,
            SearchEngine? searchEngine = null)
        {
            var query = _db.ScrapeHistory.AsQueryable();

            if (!string.IsNullOrEmpty(name)) query = query.Where(x => x.Name == name);
            if (!string.IsNullOrEmpty(url)) query = query.Where(x => x.Url == url);
            if (searchEngine.HasValue) query = query.Where(x => x.SearchEngine == searchEngine.Value);

            var dbResults = await query
                .Skip(index * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Map ScrapeResultDbModel to ScrapeResult
            return dbResults.Select(r => new ScrapeResult 
            {
                //... Copy properties from r to ScrapeResult instance
            });
        }
    }
}
