using WebFlay.Lib.Models;
using WebFlay.Lib.Shared.Enums;

namespace WebFlay.Lib.Shared.Builders;

public class ScrapeResultBuilder : ScrapeResult
{
    public ScrapeResultBuilder()
    {
        CreatedOn = DateTime.UtcNow;
    }

    public ScrapeResultBuilder AddName(string name)
    {
        Name = name;
        return this;
    }

    public ScrapeResultBuilder AddUrl(string url)
    {
        Url = url;
        return this;
    }

    public ScrapeResultBuilder AddPosition(string? position)
    {
        Position = position;
        return this;
    }

    public ScrapeResultBuilder AddSearchEngine(SearchEngine searchEngine)
    {
        SearchEngine = searchEngine;
        return this;
    }

    public ScrapeResult Build()
    {
        return new()
        {
            Name = Name ?? string.Empty,
            Url = Url ?? string.Empty,
            Position = Position,
            SearchEngine = SearchEngine,
            CreatedOn = CreatedOn
        };
    }
}