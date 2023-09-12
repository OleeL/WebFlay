using WebFlay.Lib.Shared.Enums;

namespace WebFlay.Lib.Models;

public class ScrapeResult
{

    public string? Name { get; set; }
    
    public string? Url { get; set; }
    
    public string? Position { get; set; }

    public SearchEngine SearchEngine { get; set; }

    public DateTime CreatedOn { get; set; }
}