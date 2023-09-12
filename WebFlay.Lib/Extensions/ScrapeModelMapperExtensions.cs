using WebFlay.Lib.Models;
using WebFlay.Lib.Models.Database;

namespace WebFlay.Lib.Extensions;

public static class ScrapeModelMapperExtensions
{
    public static ScrapeResultDbModel ToScrapeResultDbModel(this ScrapeResult model) =>
        new()
        {
            Name = model.Name ?? string.Empty,
            Url = model.Url ?? string.Empty,
            Position = model.Position,
            SearchEngine = model.SearchEngine,
            CreatedOn = model.CreatedOn
        };

    public static ScrapeResult ToScrapeResult(this ScrapeResultDbModel model) =>
        new()
        {
            Name = model.Name ?? string.Empty,
            Url = model.Url ?? string.Empty,
            Position = model.Position,
            SearchEngine = model.SearchEngine,
            CreatedOn = model.CreatedOn
        };
}