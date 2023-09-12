using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using WebFlay.Lib.Data.Repositories;
using WebFlay.Lib.Extensions;
using WebFlay.Lib.Models;
using WebFlay.Lib.Shared.Builders;
using WebFlay.Lib.Shared.Enums;

namespace WebFlay.Lib.Services;

public class ScrapeService : IScrapeService
{
    private readonly IScrapeRepository _repository;

    private const string UserAgent =
        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/116.0.0.0 Safari/537.36";


    public ScrapeService(IScrapeRepository repository)
    {
        _repository = repository;
    }

    public async Task<ScrapeResult?> Scrape(string name, string url, SearchEngine searchEngine)
    {
        var result = new ScrapeResultBuilder()
            .AddName(name)
            .AddUrl(url)
            .AddSearchEngine(searchEngine);

        switch (searchEngine)
        {
            case SearchEngine.Google:
            {
                var position = await GetUrlPositionInGoogle(name, url);
                result.AddPosition(position);
                break;
            }
            case SearchEngine.Bing:
            {
                var position = await GetUrlPositionInBing(name, url);
                result.AddPosition(position);
                break;
            }
            case SearchEngine.DuckDuckGo:
            {
                var position = await GetUrlPositionInDuckDuckGo(name, url);
                result.AddPosition(position);
                break;
            }
            default:
                return null;
        }

        var finalResult = result.Build();

        await _repository.AddAsync(finalResult);

        return finalResult;
    }

    private async Task<string> GetUrlPositionInGoogle(string searchTerm, string targetUrl)
    {
        var googleUrl = $"https://www.google.com/search?q={Uri.EscapeDataString(searchTerm)}&num=100";
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        var response = await httpClient.GetStringAsync(googleUrl);
        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(response);
        var nodes = htmlDocument.DocumentNode.SelectNodes("//cite");
        if (nodes == null) return string.Empty;
        var position = 1;
        var positions = new List<int>();
        foreach (var (node, index) in nodes.Index())
        {
            if (index % 2 == 0) continue;
            var regex = new Regex(@"https?://([^/\s›]+)");
            var match = regex.Match(node.InnerText);
                if (!match.Success)
            {
                position++;
                continue;
            }

            var uriScraped = match.Groups[1].Value.CreateUri()?.GetMainDomain();
            var uriTarget = targetUrl.CreateUri()?.GetMainDomain();
            if (!string.IsNullOrEmpty(uriScraped)
                && !string.IsNullOrEmpty(uriTarget)
                && uriScraped == uriTarget)
            {
                positions.Add(position);
            }

            position++;
        }

        return positions.Count == 0 ? string.Empty : positions.Select(x => x.ToString()).Join();
    }

    private async Task<string> GetUrlPositionInBing(string searchTerm, string targetUrl)
    {
        var url = $"https://www.bing.com/search?q={Uri.EscapeDataString(searchTerm)}&count=100";
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        var response = await httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(response);
        var nodes = htmlDocument.DocumentNode.SelectNodes("//a[@class='tilk']");
        if (nodes == null) return string.Empty;
        var positions = new List<int>();
        var position = 1;
        foreach (var node in nodes)
        {
            var hrefValue = node.Attributes["href"].Value;
            if (hrefValue is null)
            {
                position++;
                continue;
            }

            var regex = new Regex(@"https?://([^/\s›]+)");
            var match = regex.Match(hrefValue);
            if (!match.Success)
            {
                position++;
                continue;
            }

            var uriScraped = match.Groups[1].Value.CreateUri()?.GetMainDomain();
            var uriTarget = targetUrl.CreateUri()?.GetMainDomain();
            if (!string.IsNullOrEmpty(uriScraped)
                && !string.IsNullOrEmpty(uriTarget)
                && uriScraped == uriTarget)
            {
                positions.Add(position);
            }

            position++;
        }

        return positions.Count == 0 ? string.Empty : positions.Select(x => x.ToString()).Join();
    }

    private async Task<string> GetUrlPositionInDuckDuckGo(string searchTerm, string targetUrl)
    {
        // s: 27 
        // v: l
        // dc: 29
        var url = $"https://html.duckduckgo.com/html?q={Uri.EscapeDataString(searchTerm)}&s=100";
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        var response = await httpClient.GetStringAsync(url);
        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(response);
        var nodes = htmlDocument.DocumentNode.SelectNodes("//a[@class='result__url']");
        if (nodes == null) return string.Empty;
        var positions = new List<int>();
        var position = 1;
        foreach (var node in nodes)
        {
            var uriScraped = node.InnerText.Trim().CreateUri()?.GetMainDomain();
            var uriTarget = targetUrl.CreateUri()?.GetMainDomain();
            if (!string.IsNullOrEmpty(uriScraped)
                && !string.IsNullOrEmpty(uriTarget)
                && uriScraped == uriTarget)
            {
                positions.Add(position);
            }

            position++;
        }

        return positions.Count == 0 ? string.Empty : positions.Select(x => x.ToString()).Join();
    }

    public async Task<(IEnumerable<ScrapeResult> results, int totalResutls)> GetHistory(
        int index = 0,
        int pageSize = 100)
    {
        var results = await _repository.GetHistory(index, pageSize);
        var totalCount = await _repository.GetTotalCount();
        return (results, totalCount);
    }

    public async Task<(IEnumerable<ScrapeResult> results, int totalResutls)> GetHistoryByFilter(int index = 0,
        int pageSize = 100,
        string? name = null,
        string? url = null,
        SearchEngine? searchEngine = null)
    {
        var results = await _repository.GetHistoryByFilter(index, pageSize, name, url, searchEngine);
        var totalCount = await _repository.GetTotalCount();
        return (results, totalCount);
    }
}