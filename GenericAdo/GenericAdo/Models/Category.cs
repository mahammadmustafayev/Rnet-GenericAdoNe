

using DbHelper.Models;
using System.ComponentModel.DataAnnotations;

namespace GenericAdo.Models;

public class Category:BaseEntity
{
    [Key]
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    
}
