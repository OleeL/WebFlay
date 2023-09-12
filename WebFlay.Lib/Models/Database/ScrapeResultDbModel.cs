using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebFlay.Lib.Shared.Enums;
using WebFlay.Lib.Shared.Interfaces;

namespace WebFlay.Lib.Models.Database;

[Table("ScrapeResults")]
public class ScrapeResultDbModel : IDbModel
{
    [Key] public int Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public string? Position { get; set; }

    public SearchEngine SearchEngine { get; set; }

    public DateTime CreatedOn { get; set; }
}