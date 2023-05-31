

using DbHelper.Models;
using System.ComponentModel.DataAnnotations;

namespace GenericAdo.Models;

internal class Product:BaseEntity
{
    [Key]
    public int ProductId { get; set; } 
    public string ProductName { get; set; } = null!;
    public int? CategoryId { get; set;}
    public string? QuantityPerUnit { get; set; }
    public decimal? UnitPrice { get; set; }
    public int? UnitsInStock { get; set; }
    public int? UnitsInOrder { get; set; }
    public int? RecorderLevel { get; set; }
    public bool Discontinued { get; set; }
}
