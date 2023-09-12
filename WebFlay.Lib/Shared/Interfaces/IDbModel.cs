using System.ComponentModel.DataAnnotations;

namespace WebFlay.Lib.Shared.Interfaces;

public interface IDbModel
{
    [Key] public int Id { get; set; }
}